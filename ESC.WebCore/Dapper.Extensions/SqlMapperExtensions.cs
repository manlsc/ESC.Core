using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Collections.Concurrent;
using System.Reflection.Emit;

using Dapper;
using Dapper.Extensions;

#if NETSTANDARD1_3
using DataException = System.InvalidOperationException;
#else
using System.Threading;
#endif

namespace Dapper.Extensions
{
    /// <summary>
    /// The Dapper.Contrib extensions for Dapper
    /// </summary>
    public static partial class SqlMapperExtensions
    {
        /// <summary>
        /// The function to get a database type from the given <see cref="IDbConnection"/>.
        /// </summary>
        /// <param name="connection">The connection to get a database type name from.</param>
        public delegate string GetDatabaseTypeDelegate(IDbConnection connection);
        /// <summary>
        /// The function to get a a table name from a given <see cref="Type"/>
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to get a table name for.</param>
        public delegate string TableNameMapperDelegate(Type type);

        /// <summary>
        /// 对象缓存
        /// </summary>
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, PocoData> PocoDatas = new ConcurrentDictionary<RuntimeTypeHandle, PocoData>();

        private static readonly ISqlAdapter DefaultAdapter = new SqlServer12Adapter();
        private static readonly Dictionary<string, ISqlAdapter> AdapterDictionary
            = new Dictionary<string, ISqlAdapter>
            {
                ["sqlconnection"] = new SqlServer12Adapter(),
                ["sqlceconnection"] = new SqlCeServerAdapter(),
                ["sqliteconnection"] = new SQLiteAdapter(),
                ["mysqlconnection"] = new MySqlAdapter()
            };

        /// <summary>
        /// 获取实体信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static PocoData PocoDatasCache(Type type)
        {
            PocoData poco;
            if (PocoDatas.TryGetValue(type.TypeHandle, out poco))
            {
                return poco;
            }
            poco = new PocoData(type);
            PocoDatas[type.TypeHandle] = poco;
            return poco;
        }

        /// <summary>
        /// Returns a single entity by a single id from table "Ts".  
        /// Id must be marked with [Key] attribute.
        /// Entities created from interfaces are tracked/intercepted for changes and used by the Update() extension
        /// for optimal performance. 
        /// </summary>
        /// <typeparam name="T">Interface or type to create and populate</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="id">Id of the entity to get, must be marked with [Key] attribute</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>Entity of T</returns>
        public static T Get<T>(this IDbConnection connection, dynamic id, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var type = typeof(T);
            PocoData poco = PocoDatasCache(type);
            string sql = $"select * from {poco.Table.TableName} where {poco.Table.PrimaryKey} = @id";

            var dynParms = new DynamicParameters();
            dynParms.Add("@id", id);

            T obj = connection.Query<T>(sql, dynParms, transaction, commandTimeout: commandTimeout).FirstOrDefault();
            return obj;
        }

        /// <summary>
        /// Returns a list of entites from table "Ts".  
        /// Id of T must be marked with [Key] attribute.
        /// Entities created from interfaces are tracked/intercepted for changes and used by the Update() extension
        /// for optimal performance. 
        /// </summary>
        /// <typeparam name="T">Interface or type to create and populate</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>Entity of T</returns>
        public static IEnumerable<T> GetAll<T>(this IDbConnection connection, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var type = typeof(T);
            var cacheType = typeof(List<T>);
            PocoData poco = PocoDatasCache(type);
            string sql = $"select * from {poco.Table.TableName}";

            return connection.Query<T>(sql, null, transaction, commandTimeout: commandTimeout);
        }

        /// <summary>
        /// Specify a custom table name mapper based on the POCO type name
        /// </summary>
        public static TableNameMapperDelegate TableNameMapper;

        /// <summary>
        /// Inserts an entity into table "Ts" and returns identity id or number of inserted rows if inserting a list.
        /// </summary>
        /// <typeparam name="T">The type to insert.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToInsert">Entity to insert, can be list of entities</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>Identity of inserted entity, or number of inserted rows if inserting a list</returns>
        public static long Insert<T>(this IDbConnection connection, T entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var isList = false;

            var type = typeof(T);

            if (type.IsArray)
            {
                isList = true;
                type = type.GetElementType();
            }
            else if (type.IsGenericType)
            {
                var typeInfo = type.GetTypeInfo();
                bool implementsGenericIEnumerableOrIsGenericIEnumerable =
                    typeInfo.ImplementedInterfaces.Any(ti => ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ||
                    typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>);

                if (implementsGenericIEnumerableOrIsGenericIEnumerable)
                {
                    isList = true;
                    type = type.GetGenericArguments()[0];
                }
            }

            PocoData poco = PocoDatasCache(type);
            var adapter = GetFormatter(connection);
            StringBuilder sbColumnList = new StringBuilder(null);
            StringBuilder sbParameterList = new StringBuilder(null);

            foreach (var column in poco.Columns)
            {
                if (!column.ResultColumn)
                {
                    //如果主键是自增
                    if (poco.Table.AutoIncrement && poco.Table.PrimaryKey != null && string.Compare(column.ColumnName, poco.Table.PrimaryKey, true) == 0)
                    {
                        //如果序列存在
                        if (!string.IsNullOrEmpty(poco.Table.SequenceName))
                        {
                            //oracle暂不支持
                        }
                    }
                    else
                    {
                        adapter.AppendColumnName(sbColumnList, column.ColumnName);  //fix for issue #336
                        sbParameterList.AppendFormat("@{0},", column.ColumnName);
                    }
                }
            }

            int returnVal;
            var wasClosed = connection.State == ConnectionState.Closed;
            if (wasClosed) connection.Open();

            if (!isList)    //single entity
            {
                returnVal = adapter.Insert(connection, transaction, commandTimeout, poco.Table.TableName, sbColumnList.Remove(sbColumnList.Length - 1, 1).ToString(),
                    sbParameterList.Remove(sbParameterList.Length - 1, 1).ToString(), poco.Table.KeyProperty, entityToInsert);
            }
            else
            {
                //insert list of entities
                var cmd = $"insert into {poco.Table.TableName} ({sbColumnList.Remove(sbColumnList.Length - 1, 1).ToString()}) values ({sbParameterList.Remove(sbParameterList.Length - 1, 1).ToString()})";
                returnVal = connection.Execute(cmd, entityToInsert, transaction, commandTimeout);
            }
            if (wasClosed) connection.Close();
            return returnVal;
        }

        /// <summary>
        /// Updates entity in table "Ts", checks if the entity is modified if the entity is tracked by the Get() extension.
        /// </summary>
        /// <typeparam name="T">Type to be updated</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToUpdate">Entity to be updated</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if updated, false if not found or not modified (tracked entities)</returns>
        public static bool Update<T>(this IDbConnection connection, T entityToUpdate, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var type = typeof(T);

            if (type.IsArray)
            {
                type = type.GetElementType();
            }
            else if (type.IsGenericType)
            {
                var typeInfo = type.GetTypeInfo();
                bool implementsGenericIEnumerableOrIsGenericIEnumerable =
                    typeInfo.ImplementedInterfaces.Any(ti => ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ||
                    typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>);

                if (implementsGenericIEnumerableOrIsGenericIEnumerable)
                {
                    type = type.GetGenericArguments()[0];
                }
            }

            PocoData poco = PocoDatasCache(type);
            var adapter = GetFormatter(connection);

            StringBuilder sbColumnList = new StringBuilder(null);
            StringBuilder sbParameterList = new StringBuilder(null);

            foreach (var column in poco.Columns)
            {
                if (!column.ResultColumn)
                {
                    //如果主键是自增
                    if (poco.Table.PrimaryKey != null && string.Compare(column.ColumnName, poco.Table.PrimaryKey, true) == 0)
                    {
                        adapter.AppendColumnNameEqualsValue(sbParameterList, column.ColumnName);
                    }
                    else
                    {
                        adapter.AppendColumnNameEqualsValue(sbColumnList, column.ColumnName);  //fix for issue #336
                    }
                }
            }

            var cmd = $"update {poco.Table.TableName} set {sbColumnList.Remove(sbColumnList.Length - 1, 1).ToString()} where {sbParameterList.Remove(sbParameterList.Length - 1, 1).ToString()}";
            var updated = connection.Execute(cmd, entityToUpdate, commandTimeout: commandTimeout, transaction: transaction);
            return updated > 0;
        }

        /// <summary>
        /// Delete entity in table "Ts".
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToDelete">Entity to delete</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if deleted, false if not found</returns>
        public static bool Delete<T>(this IDbConnection connection, T entityToDelete, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            if (entityToDelete == null)
                throw new ArgumentException("Cannot Delete null Object", nameof(entityToDelete));

            var type = typeof(T);

            if (type.IsArray)
            {
                type = type.GetElementType();
            }
            else if (type.IsGenericType)
            {
                var typeInfo = type.GetTypeInfo();
                bool implementsGenericIEnumerableOrIsGenericIEnumerable =
                    typeInfo.ImplementedInterfaces.Any(ti => ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ||
                    typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>);

                if (implementsGenericIEnumerableOrIsGenericIEnumerable)
                {
                    type = type.GetGenericArguments()[0];
                }
            }

            PocoData poco = PocoDatasCache(type);
            var adapter = GetFormatter(connection);

            var sb = new StringBuilder();
            sb.AppendFormat("delete from {0} where ", poco.Table.TableName);
            adapter.AppendColumnNameEqualsValue(sb, poco.Table.PrimaryKey);
            var deleted = connection.Execute(sb.Remove(sb.Length - 1, 1).ToString(), entityToDelete, transaction, commandTimeout);
            return deleted > 0;
        }

        /// <summary>
        /// Delete entity in table "Ts".
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityId">entityId</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if deleted, false if not found</returns>
        public static bool Delete<T>(this IDbConnection connection, int id, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var type = typeof(T);
            PocoData poco = PocoDatasCache(type);
            var adapter = GetFormatter(connection);
            string sql = $"delete from {poco.Table.TableName} where {poco.Table.PrimaryKey} = @id";

            var dynParms = new DynamicParameters();
            dynParms.Add("@id", id);

            return connection.Execute(sql, dynParms, transaction, commandTimeout: commandTimeout) > 0;
        }

        /// <summary>
        /// Delete all entities in the table related to the type T.
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if deleted, false if none found</returns>
        public static bool DeleteAll<T>(this IDbConnection connection, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var type = typeof(T);
            PocoData poco = PocoDatasCache(type);
            var statement = $"delete from {poco.Table.TableName}";
            var deleted = connection.Execute(statement, null, transaction, commandTimeout);
            return deleted > 0;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Page<T> Page<T>(this IDbConnection connection, long pageIndex, long pageSize, string sql, params object[] args)
        {
            string sqlCount, sqlPage;
            BuildPageQueries<T>(connection, (pageIndex - 1) * pageSize, pageSize, sql, ref args, out sqlCount, out sqlPage);
            Page<T> result = new Page<T>();
            result.rows = connection.Query<T>(sqlPage).ToList();
            result.total = connection.ExecuteScalar<long>(sqlCount);
            return result;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static List<T> QueryList<T>(this IDbConnection connection, long pageIndex, long pageSize, string sql, params object[] args)
        {
            string sqlCount, sqlPage;
            BuildPageQueries<T>(connection, (pageIndex - 1) * pageSize, pageSize, sql, ref args, out  sqlCount, out sqlPage);
            Page<T> result = new Page<T>();
            return connection.Query<T>(sqlPage).ToList();
        }

        /// <summary>
        ///     Starting with a regular SELECT statement, derives the SQL statements required to query a
        ///     DB for a page of records and the total number of records
        /// </summary>
        /// <typeparam name="T">The Type representing a row in the result set</typeparam>
        /// <param name="skip">The number of rows to skip before the start of the page</param>
        /// <param name="take">The number of rows in the page</param>
        /// <param name="sql">The original SQL select statement</param>
        /// <param name="args">Arguments to any embedded parameters in the SQL</param>
        /// <param name="sqlCount">Outputs the SQL statement to query for the total number of matching rows</param>
        /// <param name="sqlPage">Outputs the SQL statement to retrieve a single page of matching rows</param>
        private static void BuildPageQueries<T>(IDbConnection connection, long skip, long take, string sql, ref object[] args, out string sqlCount, out string sqlPage)
        {
            // Split the SQL
            SQLParts parts;
            var adapter = GetFormatter(connection);
            if (!PagingHelper.Instance.SplitSQL(sql, out parts))
                throw new Exception("Unable to parse SQL statement for paged query");

            sqlPage = adapter.BuildPageQuery(skip, take, parts, ref args);
            sqlCount = parts.SqlCount;
        }


        /// <summary>
        /// Specifies a custom callback that detects the database type instead of relying on the default strategy (the name of the connection type object).
        /// Please note that this callback is global and will be used by all the calls that require a database specific adapter.
        /// </summary>
        public static GetDatabaseTypeDelegate GetDatabaseType;

        /// <summary>
        /// 获取数据库适配器
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private static ISqlAdapter GetFormatter(IDbConnection connection)
        {
            var name = GetDatabaseType?.Invoke(connection).ToLower()
                       ?? connection.GetType().Name.ToLower();

            return !AdapterDictionary.ContainsKey(name)
                ? DefaultAdapter
                : AdapterDictionary[name];
        }

    }
}

/// <summary>
/// The interface for all Dapper.Contrib database operations
/// Implementing this is each provider's model.
/// </summary>
public partial interface ISqlAdapter
{
    /// <summary>
    /// Inserts <paramref name="entityToInsert"/> into the database, returning the Id of the row created.
    /// </summary>
    /// <param name="connection">The connection to use.</param>
    /// <param name="transaction">The transaction to use.</param>
    /// <param name="commandTimeout">The command timeout to use.</param>
    /// <param name="tableName">The table to insert into.</param>
    /// <param name="columnList">The columns to set with this insert.</param>
    /// <param name="parameterList">The parameters to set for this insert.</param>
    /// <param name="keyProperties">The key columns in this table.</param>
    /// <param name="entityToInsert">The entity to insert.</param>
    /// <returns>The Id of the row created.</returns>
    int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, PropertyInfo keyProperty, object entityToInsert);

    /// <summary>
    /// Adds the name of a column.
    /// </summary>
    /// <param name="sb">The string builder  to append to.</param>
    /// <param name="columnName">The column name.</param>
    void AppendColumnName(StringBuilder sb, string columnName);

    /// <summary>
    /// Adds a column equality to a parameter.
    /// </summary>
    /// <param name="sb">The string builder  to append to.</param>
    /// <param name="columnName">The column name.</param>
    void AppendColumnNameEqualsValue(StringBuilder sb, string columnName);

    string BuildPageQuery(long skip, long take, SQLParts parts, ref object[] args);
}

/// <summary>
/// The SQL Server database adapter.
/// </summary>
public partial class SqlServerAdapter : ISqlAdapter
{
    /// <summary>
    /// Inserts <paramref name="entityToInsert"/> into the database, returning the Id of the row created.
    /// </summary>
    /// <param name="connection">The connection to use.</param>
    /// <param name="transaction">The transaction to use.</param>
    /// <param name="commandTimeout">The command timeout to use.</param>
    /// <param name="tableName">The table to insert into.</param>
    /// <param name="columnList">The columns to set with this insert.</param>
    /// <param name="parameterList">The parameters to set for this insert.</param>
    /// <param name="keyProperties">The key columns in this table.</param>
    /// <param name="entityToInsert">The entity to insert.</param>
    /// <returns>The Id of the row created.</returns>
    public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, PropertyInfo keyProperty, object entityToInsert)
    {
        var cmd = $"insert into {tableName} ({columnList}) values ({parameterList});select SCOPE_IDENTITY() id";
        var multi = connection.QueryMultiple(cmd, entityToInsert, transaction, commandTimeout);

        var first = multi.Read().FirstOrDefault();
        if (first == null || first.id == null) return 0;

        var id = (int)first.id;
        if (keyProperty == null) return id;

        keyProperty.SetValue(entityToInsert, Convert.ChangeType(id, keyProperty.PropertyType), null);

        return id;
    }

    /// <summary>
    /// Adds the name of a column.
    /// </summary>
    /// <param name="sb">The string builder  to append to.</param>
    /// <param name="columnName">The column name.</param>
    public void AppendColumnName(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("[{0}],", columnName);
    }

    /// <summary>
    /// Adds a column equality to a parameter.
    /// </summary>
    /// <param name="sb">The string builder  to append to.</param>
    /// <param name="columnName">The column name.</param>
    public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("[{0}] = @{1},", columnName, columnName);
    }

    public string BuildPageQuery(long skip, long take, SQLParts parts, ref object[] args)
    {
        var helper = PagingHelper.Instance;
        // when the query does not contain an "order by", it is very slow
        if (helper.SimpleRegexOrderBy.IsMatch(parts.SqlSelectRemoved))
        {
            var m = helper.SimpleRegexOrderBy.Match(parts.SqlSelectRemoved);
            if (m.Success)
            {
                var g = m.Groups[0];
                parts.SqlSelectRemoved = parts.SqlSelectRemoved.Substring(0, g.Index);
            }
        }
        if (helper.RegexDistinct.IsMatch(parts.SqlSelectRemoved))
        {
            parts.SqlSelectRemoved = "peta_inner.* FROM (SELECT " + parts.SqlSelectRemoved + ") peta_inner";
        }
        var sqlPage = string.Format("SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) peta_rn, {1}) peta_paged WHERE peta_rn > @{2} AND peta_rn <= @{3}", parts.SqlOrderBy ?? "ORDER BY (SELECT NULL)", parts.SqlSelectRemoved, args.Length, args.Length + 1);
        args = args.Concat(new object[] { skip, skip + take }).ToArray();
        return sqlPage;
    }
}

/// <summary>
/// The SQL Server database adapter.
/// </summary>
public partial class SqlServer12Adapter : ISqlAdapter
{
    /// <summary>
    /// Inserts <paramref name="entityToInsert"/> into the database, returning the Id of the row created.
    /// </summary>
    /// <param name="connection">The connection to use.</param>
    /// <param name="transaction">The transaction to use.</param>
    /// <param name="commandTimeout">The command timeout to use.</param>
    /// <param name="tableName">The table to insert into.</param>
    /// <param name="columnList">The columns to set with this insert.</param>
    /// <param name="parameterList">The parameters to set for this insert.</param>
    /// <param name="keyProperties">The key columns in this table.</param>
    /// <param name="entityToInsert">The entity to insert.</param>
    /// <returns>The Id of the row created.</returns>
    public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, PropertyInfo keyProperty, object entityToInsert)
    {
        var cmd = $"insert into {tableName} ({columnList}) values ({parameterList});select SCOPE_IDENTITY() id";
        var multi = connection.QueryMultiple(cmd, entityToInsert, transaction, commandTimeout);

        var first = multi.Read().FirstOrDefault();
        if (first == null || first.id == null) return 0;

        var id = (int)first.id;
        if (keyProperty == null) return id;

        keyProperty.SetValue(entityToInsert, Convert.ChangeType(id, keyProperty.PropertyType), null);

        return id;
    }

    /// <summary>
    /// Adds the name of a column.
    /// </summary>
    /// <param name="sb">The string builder  to append to.</param>
    /// <param name="columnName">The column name.</param>
    public void AppendColumnName(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("[{0}],", columnName);
    }

    /// <summary>
    /// Adds a column equality to a parameter.
    /// </summary>
    /// <param name="sb">The string builder  to append to.</param>
    /// <param name="columnName">The column name.</param>
    public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("[{0}] = @{1},", columnName, columnName);
    }

    public string BuildPageQuery(long skip, long take, SQLParts parts, ref object[] args)
    {
        if (string.IsNullOrEmpty(parts.SqlOrderBy))
            parts.Sql += " ORDER BY 1 DESC";
        var sqlPage = string.Format("{0}\nOFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY", parts.Sql, skip, take);
        return sqlPage;
    }
}

/// <summary>
/// The SQL Server Compact Edition database adapter.
/// </summary>
public partial class SqlCeServerAdapter : ISqlAdapter
{
    /// <summary>
    /// Inserts <paramref name="entityToInsert"/> into the database, returning the Id of the row created.
    /// </summary>
    /// <param name="connection">The connection to use.</param>
    /// <param name="transaction">The transaction to use.</param>
    /// <param name="commandTimeout">The command timeout to use.</param>
    /// <param name="tableName">The table to insert into.</param>
    /// <param name="columnList">The columns to set with this insert.</param>
    /// <param name="parameterList">The parameters to set for this insert.</param>
    /// <param name="keyProperties">The key columns in this table.</param>
    /// <param name="entityToInsert">The entity to insert.</param>
    /// <returns>The Id of the row created.</returns>
    public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, PropertyInfo keyProperty, object entityToInsert)
    {
        var cmd = $"insert into {tableName} ({columnList}) values ({parameterList})";
        connection.Execute(cmd, entityToInsert, transaction, commandTimeout);
        var r = connection.Query("select @@IDENTITY id", transaction: transaction, commandTimeout: commandTimeout).ToList();

        if (r[0].id == null) return 0;
        var id = (int)r[0].id;

        if (keyProperty == null) return id;
        keyProperty.SetValue(entityToInsert, Convert.ChangeType(id, keyProperty.PropertyType), null);

        return id;
    }

    /// <summary>
    /// Adds the name of a column.
    /// </summary>
    /// <param name="sb">The string builder  to append to.</param>
    /// <param name="columnName">The column name.</param>
    public void AppendColumnName(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("[{0}],", columnName);
    }

    /// <summary>
    /// Adds a column equality to a parameter.
    /// </summary>
    /// <param name="sb">The string builder  to append to.</param>
    /// <param name="columnName">The column name.</param>
    public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("[{0}] = @{1},", columnName, columnName);
    }

    public string BuildPageQuery(long skip, long take, SQLParts parts, ref object[] args)
    {
        if (string.IsNullOrEmpty(parts.SqlOrderBy))
            parts.Sql += " ORDER BY ABS(1)";
        var sqlPage = string.Format("{0}\nOFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY", parts.Sql, skip, take);
        return sqlPage;
    }
}

/// <summary>
/// The MySQL database adapter.
/// </summary>
public partial class MySqlAdapter : ISqlAdapter
{
    /// <summary>
    /// Inserts <paramref name="entityToInsert"/> into the database, returning the Id of the row created.
    /// </summary>
    /// <param name="connection">The connection to use.</param>
    /// <param name="transaction">The transaction to use.</param>
    /// <param name="commandTimeout">The command timeout to use.</param>
    /// <param name="tableName">The table to insert into.</param>
    /// <param name="columnList">The columns to set with this insert.</param>
    /// <param name="parameterList">The parameters to set for this insert.</param>
    /// <param name="keyProperties">The key columns in this table.</param>
    /// <param name="entityToInsert">The entity to insert.</param>
    /// <returns>The Id of the row created.</returns>
    public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, PropertyInfo keyProperty, object entityToInsert)
    {
        var cmd = $"insert into {tableName} ({columnList}) values ({parameterList})";
        connection.Execute(cmd, entityToInsert, transaction, commandTimeout);
        var r = connection.Query("Select LAST_INSERT_ID() id", transaction: transaction, commandTimeout: commandTimeout);

        var id = r.First().id;
        if (id == null) return 0;

        if (keyProperty == null) return id;
        keyProperty.SetValue(entityToInsert, Convert.ChangeType(id, keyProperty.PropertyType), null);


        return Convert.ToInt32(id);
    }

    /// <summary>
    /// Adds the name of a column.
    /// </summary>
    /// <param name="sb">The string builder  to append to.</param>
    /// <param name="columnName">The column name.</param>
    public void AppendColumnName(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("`{0}`,", columnName);
    }

    /// <summary>
    /// Adds a column equality to a parameter.
    /// </summary>
    /// <param name="sb">The string builder  to append to.</param>
    /// <param name="columnName">The column name.</param>
    public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("`{0}` = @{1},", columnName, columnName);
    }

    /// <summary>
    ///     Builds an SQL query suitable for performing page based queries to the database
    /// </summary>
    /// <param name="skip">The number of rows that should be skipped by the query</param>
    /// <param name="take">The number of rows that should be retruend by the query</param>
    /// <param name="parts">The original SQL query after being parsed into it's component parts</param>
    /// <param name="args">Arguments to any embedded parameters in the SQL query</param>
    /// <returns>The final SQL query that should be executed.</returns>
    public string BuildPageQuery(long skip, long take, SQLParts parts, ref object[] args)
    {
        var sql = string.Format("{0}\nLIMIT {1} OFFSET {2}", parts.Sql, take, skip);
        return sql;
    }
}

/// <summary>
/// The SQLite database adapter.
/// </summary>
public partial class SQLiteAdapter : ISqlAdapter
{
    /// <summary>
    /// Inserts <paramref name="entityToInsert"/> into the database, returning the Id of the row created.
    /// </summary>
    /// <param name="connection">The connection to use.</param>
    /// <param name="transaction">The transaction to use.</param>
    /// <param name="commandTimeout">The command timeout to use.</param>
    /// <param name="tableName">The table to insert into.</param>
    /// <param name="columnList">The columns to set with this insert.</param>
    /// <param name="parameterList">The parameters to set for this insert.</param>
    /// <param name="keyProperties">The key columns in this table.</param>
    /// <param name="entityToInsert">The entity to insert.</param>
    /// <returns>The Id of the row created.</returns>
    public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, PropertyInfo keyProperty, object entityToInsert)
    {
        var cmd = $"INSERT INTO {tableName} ({columnList}) VALUES ({parameterList}); SELECT last_insert_rowid() id";
        var multi = connection.QueryMultiple(cmd, entityToInsert, transaction, commandTimeout);

        var id = multi.Read().First().id;
        if (id == null) return 0;

        if (keyProperty == null) return id;
        keyProperty.SetValue(entityToInsert, Convert.ChangeType(id, keyProperty.PropertyType), null);

        return id;
    }

    /// <summary>
    /// Adds the name of a column.
    /// </summary>
    /// <param name="sb">The string builder  to append to.</param>
    /// <param name="columnName">The column name.</param>
    public void AppendColumnName(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("\"{0}\",", columnName);
    }

    /// <summary>
    /// Adds a column equality to a parameter.
    /// </summary>
    /// <param name="sb">The string builder  to append to.</param>
    /// <param name="columnName">The column name.</param>
    public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
    {
        sb.AppendFormat("\"{0}\" = @{1},", columnName, columnName);
    }

    public string BuildPageQuery(long skip, long take, SQLParts parts, ref object[] args)
    {
        var sql = string.Format("{0}\nLIMIT {1} OFFSET {2}", parts.Sql, take, skip);
        return sql;
    }
}
