using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 销售退
    /// </summary>
    public class WSellReturnLineRepository : BaseRepository<WSellReturnLine>
    {

        #region 构造

        public WSellReturnLineRepository() : base() { }

        public WSellReturnLineRepository(string connectionString) : base(connectionString) { }

        public WSellReturnLineRepository(DatabaseContext dbContext) : base(dbContext) { }

        #endregion

        #region 语句

        public string GetSearchSql()
        {
            string searchSql = @"SELECT  T1.* ,
                                            T2.LocationDesc AS PositionName ,
                                            T3.MaterialName ,
                                            T4.UnitName ,
                                            T5.UserName AS CreateByUserName ,
                                            T6.UserName AS UpdateByUserName
                                    FROM    WSellReturnLine T1 WITH ( NOLOCK )
                                            LEFT JOIN BLocation T2 WITH ( NOLOCK ) ON T1.PositionID = T2.ID
                                            LEFT JOIN BMaterial T3 WITH ( NOLOCK ) ON T1.MaterialID = T3.ID
                                            LEFT JOIN BUnit T4 WITH ( NOLOCK ) ON T1.UnitID = T4.ID
                                            LEFT JOIN SUser T5 WITH ( NOLOCK ) ON T1.CreateBy = T5.ID
                                            LEFT JOIN SUser T6 WITH ( NOLOCK ) ON T1.UpdateBy = T6.ID";
            return searchSql;
        }

        public string GetSearchSql(List<WhereItem> whereItems)
        {
            foreach (var item in whereItems)
            {
                item.field = "T1." + item.field;
            }
            return GetSearchSql() + BulidWhereSql(whereItems) + GetOrderBy();
        }

        public string GetOrderBy()
        {
            return " ORDER BY T1.ID DESC";
        }


        #endregion

        /// <summary>
        /// 根据父类查询明细
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<WSellReturnLine> GetLinesByParentId(int parentId)
        {
            string sql = GetSearchSql() + " WHERE T1.ParentID=" + parentId;
            return Query(sql);
        }

        /// <summary>
        ///根据主表删除明细
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public int RemoveLinesByParentId(int parentId)
        {
            string sql = "DELETE FROM WSellReturnLine WHERE ParentID=" + parentId;
            return Execute(sql);
        }
    }
}
