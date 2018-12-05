using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 转移入库通知单行 +
    /// </summary>
    public class WTransferInNoticeLineRepository : BaseRepository<WTransferInNoticeLine>
    {

        #region 构造

        public WTransferInNoticeLineRepository() : base() { }

        public WTransferInNoticeLineRepository(string connectionString) : base(connectionString) { }

        public WTransferInNoticeLineRepository(DatabaseContext dbContext) : base(dbContext) { }

        #endregion

        #region 语句

        public string GetSearchSql()
        {
            string searchSql = @"  SELECT  SI.* ,
                                            P.LocationDesc AS PositionName ,
                                            M.MaterialName AS MaterialName ,
                                            U.UnitName ,
                                            CU.UserName AS CreateByUserName ,
                                            UU.UserName AS UpdateByUserName
                                    FROM    WTransferInNoticeLine SI WITH ( NOLOCK )
                                            LEFT JOIN BLocation P WITH ( NOLOCK ) ON SI.PositionID = P.ID
                                            LEFT JOIN BMaterial M WITH ( NOLOCK ) ON SI.MaterialID = M.ID
                                            LEFT JOIN BUnit U WITH ( NOLOCK ) ON SI.UnitID = U.ID
                                            LEFT JOIN SUser CU WITH ( NOLOCK ) ON SI.CreateBy = CU.ID
                                            LEFT JOIN SUser UU WITH ( NOLOCK ) ON SI.UpdateBy = UU.ID";
            return searchSql;
        }

        public string GetSearchSql(List<WhereItem> whereItems)
        {
            foreach (var item in whereItems)
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
        /// 添加下推
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public decimal AddDownCount(WTransferInNoticeLine line)
        {
            string sql = "UPDATE WTransferInNoticeLine SET InPutCount=0,PositionID=0,PositionCode='',DownCount=DownCount+" + line.InPutCount + " OUTPUT Inserted.InCount-Inserted.DownCount WHERE ID=" + line.ID;
            return ExecuteScalar<decimal>(sql);
        }

        /// <summary>
        /// 是否全部入库
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public bool IsAllDownload(int parentId)
        {
            string sql = "SELECT TOP 1 ID FROM WTransferInNoticeLine WITH(NOLOCK) WHERE ParentID=" + parentId + " AND InCount>DownCount";
            string result = ExecuteScalar<string>(sql);
            return string.IsNullOrEmpty(result);
        }
    }
}
