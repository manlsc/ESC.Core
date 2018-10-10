using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;

namespace ESC.Infrastructure.Repository
{
    public class WTransferInNoticeRepository : BaseRepository<WTransferInNotice>
    {

        #region 构造

        public WTransferInNoticeRepository() : base(){}

        public WTransferInNoticeRepository(string connectionString) : base(connectionString){}

        public WTransferInNoticeRepository(DatabaseContext dbContext) : base(dbContext){}

        #endregion

        #region 语句

        public string GetSearchSql()
        {
             string searchSql = @"SELECT  T1.* ,
                                        T2.BusinessPartnerName ,
                                        T3.UserName AS CreateByUserName ,
                                        T4.UserName AS UpdateByUserName ,
                                        T5.EnumDesc AS NoticeStatusName ,
                                        T6.LocationDesc AS WarehouseName
                                FROM    WTransferInNotice T1 WITH ( NOLOCK )
                                        LEFT JOIN BBusinessPartner T2 WITH ( NOLOCK ) ON T1.BusinessPartnerID = T2.ID
                                        LEFT JOIN SUser T3 WITH ( NOLOCK ) ON T1.CreateBy = T3.ID
                                        LEFT JOIN SUser T4 WITH ( NOLOCK ) ON T1.UpdateBy = T4.ID
                                        LEFT JOIN SCommonEnum T5 WITH ( NOLOCK ) ON T1.NoticeStatus = T5.EnumField
                                                                                        AND T5.EnumType = 'NoticeStatus'
                                        LEFT JOIN BLocation T6 WITH ( NOLOCK ) ON T1.WarehouseID = T6.ID";
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
        /// 根据主键查询
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public WTransferInNotice GetWTransferInNoticeById(int Id)
        {
            string sql = GetSearchSql() + " WHERE T1.ID=" + Id;
            return SingleOrDefault(sql);
        }

        /// <summary>
        /// 更新通知单状态
        /// </summary>
        /// <param name="noticeStatus"></param>
        /// <param name="Id"></param>
        public void UpdateNoticeStatus(int noticeStatus,int Id)
        {
            string sql = "UPDATE WTransferInNotice SET NoticeStatus = " + noticeStatus + ",UpdateDate = GETDATE() WHERE ID = " + Id;
            Execute(sql);
        }
    }
}
