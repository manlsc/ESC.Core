using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using ESC.Infrastructure.DomainObjects;
using Dapper;
using Dapper.Extensions;

namespace ESC.Infrastructure.Repository
{

    /// <summary>
    /// 仓储基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepository<T> where T : PocoObject
    {
        protected DatabaseContext dbContext;

        #region 构造

        public BaseRepository()
        {
            dbContext = new DatabaseContext();
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="connectionString"></param>
        public BaseRepository(string connectionString)
        {
            dbContext = new DatabaseContext(connectionString);
        }

        /// <summary>
        /// 连接实例
        /// </summary>
        /// <param name="db"></param>
        public BaseRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #endregion

        #region 增删改查

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="Poco">实体</param>
        /// <returns></returns>
        public object Insert(T Poco) { return dbContext.Connection.Insert<T>(Poco,dbContext.Transcation); }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="Poco">实体</param>
        /// <returns></returns>
        public object Insert(List<T> Pocos) { return dbContext.Connection.Insert<List<T>>(Pocos, dbContext.Transcation); }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="Poco">实体</param>
        /// <returns></returns>
        public bool Update(T Poco) { return dbContext.Connection.Update<T>(Poco, dbContext.Transcation); }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Poco">实体</param>
        /// <returns></returns>
        public bool Delete(T Poco) { return dbContext.Connection.Delete<T>(Poco, dbContext.Transcation); }

        /// <summary>
        /// 删除所有
        /// </summary>
        /// <returns></returns>
        public bool DeleteAll() { return dbContext.Connection.DeleteAll<T>(dbContext.Transcation); }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        public bool Delete(int primaryKey) { return dbContext.Connection.Delete<T>(primaryKey, dbContext.Transcation); }

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        public T SingleOrDefault(object primaryKey) { return dbContext.Connection.Get<T>(primaryKey, dbContext.Transcation); }

        /// <summary>
        /// 查询单个实体,如果存在多条则引发异常
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public T SingleOrDefault(string sql, object param = null) { return dbContext.Connection.QuerySingleOrDefault<T>(sql, param, dbContext.Transcation); }

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public T FirstOrDefault(string sql, object param = null) { return dbContext.Connection.QueryFirstOrDefault<T>(sql, param, dbContext.Transcation); }

        /// <summary>
        /// 查询单个实体,如果存在多条则引发异常
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        public T Single(object primaryKey) { return dbContext.Connection.Get<T>(primaryKey, dbContext.Transcation); }

        /// <summary>
        /// 查询单个实体,如果存在多条则引发异常
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public T Single(string sql, object param = null) { return dbContext.Connection.QuerySingle<T>(sql, param, dbContext.Transcation); }

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public T First(string sql, object param = null) { return dbContext.Connection.QueryFirst<T>(sql, param, dbContext.Transcation); }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public List<T> QueryAll()
        {
            return dbContext.Connection.GetAll<T>().ToList();
        }

        /// <summary>
        /// 批量查询
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public List<T> Query(string sql, object param = null) { return dbContext.Connection.Query<T>(sql, param, dbContext.Transcation).ToList(); }

        /// <summary>
        /// 批量查询
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="itemsPerPage">每页记录</param>
        /// <param name="sql">查询语句</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public List<T> QueryList(long page, long itemsPerPage, string sql, params object[] param) { return dbContext.Connection.QueryList<T>(page, itemsPerPage, sql, param); }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="sql">查询语句</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public Page<T> Pages(long pageIndex, long pageSize, string sql, params object[] param) { return dbContext.Connection.Page<T>(pageIndex, pageSize, sql, param); }

        /// <summary>
        /// 返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Execute(string sql, object param = null) { return dbContext.Connection.Execute(sql, param, dbContext.Transcation); }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。 忽略额外的列或行。
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public K ExecuteScalar<K>(string sql, object param = null) { return dbContext.Connection.ExecuteScalar<K>(sql, param, dbContext.Transcation); }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public DataTable QueryTable(string sql, object param = null)
        {
            using (IDataReader reader = dbContext.Connection.ExecuteReader(sql, param))
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                return dt;
            }
        }

        /// <summary>
        /// 批量插入
        /// 每次插入的数量最好进行测试,否则会影响性能
        /// </summary>
        /// <param name="pocos"></param>
        /// <returns></returns>
        public int BatchInsert(string sql, List<T> pocos)
        {
            return dbContext.Connection.Execute(sql, pocos, dbContext.Transcation);
        }
        #endregion

        /// <summary>
        /// 获取where条件
        /// </summary>
        /// <returns></returns>
        protected virtual string BulidWhereSql(List<WhereItem> wItems)
        {
            if (wItems == null | wItems.Count < 1)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(" WHERE 1=1 ");
            for (int i = 0; i < wItems.Count; i++)
            {
                if (wItems[i].value.StartsWith("%") || wItems[i].value.EndsWith("%"))
                {
                    wItems[i].condition = "like";
                    //去除特殊查询字符
                    wItems[i].value = "'" + wItems[i].value.Replace("'", "") + "'";
                }
                else if (wItems[i].datatype.Equals("string", StringComparison.OrdinalIgnoreCase))
                {
                    wItems[i].condition = "=";
                    wItems[i].value = "'" + wItems[i].value + "'";
                }
                else
                {
                    if (wItems[i].datatype.Equals("date", StringComparison.OrdinalIgnoreCase) || wItems[i].datatype.Equals("datetime", StringComparison.OrdinalIgnoreCase))
                    {
                        DateTime dt = DateTime.Parse(wItems[i].value);
                        if (wItems[i].condition == ">=")
                        {
                            wItems[i].value = "'" + dt.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss") + "'";

                        }
                        else if (wItems[i].condition == "<=")
                        {
                            wItems[i].value = "'" + dt.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        }
                        else
                        {
                            wItems[i].value = "'" + wItems[i].value + "'";
                        }
                    }
                }
                sb.Append(" AND " + wItems[i].field + " " + wItems[i].condition + " " + wItems[i].value + " ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        public DatabaseContext DbCondext
        {
            get
            {
                return dbContext;
            }
        }
    }

}
