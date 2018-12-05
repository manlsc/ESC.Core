using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Enums;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 采购退库 +
    /// </summary>
    public class WPurchaseReturnRepository : BaseRepository<WPurchaseReturn>
    {

        #region 构造

        public WPurchaseReturnRepository() : base(){}

        public WPurchaseReturnRepository(string connectionString) : base(connectionString){}

        public WPurchaseReturnRepository(DatabaseContext dbContext) : base(dbContext){}

        #endregion

        #region 语句

        public string GetSearchSql()
        {
             string searchSql = @"  SELECT  T1.* ,
                                            T2.BusinessPartnerName ,
                                            T3.UserName AS CreateByUserName ,
                                            T4.UserName AS UpdateByUserName ,
                                            T5.EnumDesc AS StockStatusName ,
                                            T6.LocationDesc AS WarehouseName
                                    FROM    WPurchaseReturn T1 WITH ( NOLOCK )
                                            LEFT JOIN BBusinessPartner T2 WITH ( NOLOCK ) ON T1.BusinessPartnerID = T2.ID
                                            LEFT JOIN SUser T3 WITH ( NOLOCK ) ON T1.CreateBy = T3.ID
                                            LEFT JOIN SUser T4 WITH ( NOLOCK ) ON T1.UpdateBy = T4.ID
                                            LEFT JOIN SCommonEnum T5 WITH ( NOLOCK ) ON T1.StockStatus = T5.EnumField
                                                                                            AND T5.EnumType = 'StockStatus'
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
        public WPurchaseReturn GetWPurchaseReturnById(int Id)
        {
            string sql = GetSearchSql() + " WHERE T1.ID=" + Id;
            return SingleOrDefault(sql);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="otherIn"></param>
        /// <returns></returns>
        public int ApprovePurchaseReturn(WPurchaseReturn purchase)
        {
            string sql = string.Format("UPDATE WPurchaseReturn SET UpdateDate='{0}',UpdateBy={1},StockStatus={2} WHERE ID={3} AND StockStatus={4}", purchase.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss"), purchase.UpdateBy, StockStatusEnum.Approve, purchase.ID, StockStatusEnum.New);
            return Execute(sql);
        }

        /// <summary>
        /// 根据状态删除单据
        /// </summary>
        /// <param name="otherId"></param>
        /// <param name="stockStatus"></param>
        /// <returns></returns>
        public int RemovePurchaseReturnByStatus(int otherId, int stockStatus)
        {
            string sql = "DELETE FROM WPurchaseReturn WHERE ID=" + otherId + " AND StockStatus=" + stockStatus;
            return Execute(sql);
        }
    }
}
