using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 列 +
    /// </summary>
    public class SColumnRepository : BaseRepository<SColumn>
    {

        #region 构造

        public SColumnRepository() : base(){}

        public SColumnRepository(string connectionString) : base(connectionString){}

        public SColumnRepository(DatabaseContext dbContext) : base(dbContext){}

        #endregion

        #region 语句

        public string GetSearchSql()
        {
             string searchSql = @"SELECT  T.* ,
                                        T1.EnumDesc AS RequiredName ,
                                        T2.EnumDesc AS VisibleName ,
                                        T3.EnumDesc AS DisabledName
                                FROM    SColumn T WITH ( NOLOCK )
                                        LEFT JOIN SCommonEnum T1 WITH ( NOLOCK ) ON T.[Required] = T1.EnumField AND T1.EnumType = 'Bool'
                                        LEFT JOIN SCommonEnum T2 WITH ( NOLOCK ) ON T.[Visible] = T2.EnumField AND T2.EnumType = 'Bool'
                                        LEFT JOIN SCommonEnum T3 WITH ( NOLOCK ) ON T.[Disabled] = T3.EnumField AND T3.EnumType = 'Bool'";
             return searchSql;
        }

        public string GetSearchSql(List<WhereItem> whereItems)
        {
             return GetSearchSql() + BulidWhereSql(whereItems) + GetOrderBy();
        }

        public string GetOrderBy()
        {
               return " ORDER BY T.ID DESC";
        }

        #endregion

        /// <summary>
        /// 获取可见列
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<SColumn> GetVisibleColumnsByTable(string tableName)
        {
            string sql = "SELECT * FROM [SColumn] WHERE TableName = '{0}' AND Visible = 1";
            return Query(string.Format(sql, tableName));
        }
    }
}
