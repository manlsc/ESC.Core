using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 转移出库行+
    /// </summary>
    public class WTransferOutLineRepository : BaseRepository<WTransferOutLine>
    {

        #region 构造

        public WTransferOutLineRepository() : base(){}

        public WTransferOutLineRepository(string connectionString) : base(connectionString){}

        public WTransferOutLineRepository(DatabaseContext dbContext) : base(dbContext){}

        #endregion

        #region 语句

        public string GetSearchSql()
        {
            string searchSql = @"SELECT  SI.* ,
                                        P.LocationDesc AS PositionName ,
                                        M.MaterialName AS MaterialName ,
                                        U.UnitName ,
                                        CU.UserName AS CreateByUserName ,
                                        UU.UserName AS UpdateByUserName
                                FROM    WTransferOutLine SI WITH ( NOLOCK )
                                        LEFT JOIN BLocation P WITH ( NOLOCK ) ON SI.PositionID = P.ID
                                        LEFT JOIN BMaterial M WITH ( NOLOCK ) ON SI.MaterialID = M.ID
                                        LEFT JOIN BUnit U WITH ( NOLOCK ) ON SI.UnitID = U.ID
                                        LEFT JOIN SUser CU WITH ( NOLOCK ) ON SI.CreateBy = CU.ID
                                        LEFT JOIN SUser UU WITH ( NOLOCK ) ON SI.UpdateBy = UU.ID";
             return searchSql;
        }

        public string GetSearchSql(List<WhereItem> whereItems)
        {
            foreach (WhereItem item in whereItems)
            {
                item.field = "SI." + item.field;
            }
            return GetSearchSql() + BulidWhereSql(whereItems) + GetOrderBy();
        }

        public string GetOrderBy()
        {
            return " ORDER BY SI.ID DESC";
        }

        #endregion


        /// <summary>
        ///根据主表删除明细
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public int RemoveLinesByParentId(int parentId)
        {
            string sql = "DELETE FROM WTransferOutLine WHERE ParentID=" + parentId;
            return Execute(sql);
        }

        /// <summary>
        /// 根据主表查询明细
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<WTransferOutLine> GetLinesByParentId(int parentId)
        {
            string sql = "SELECT * FROM WTransferOutLine WITH(NOLOCK) WHERE ParentID=" + parentId;
            return Query(sql);
        }
    }
}
