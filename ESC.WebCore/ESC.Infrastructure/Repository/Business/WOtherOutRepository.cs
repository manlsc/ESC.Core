using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Enums;
using System.Linq;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 其他出库 +
    /// </summary>
    public class WOtherOutRepository : BaseRepository<WOtherOut>
    {

        #region 构造

        public WOtherOutRepository() : base(){}

        public WOtherOutRepository(string connectionString) : base(connectionString){}

        public WOtherOutRepository(DatabaseContext dbContext) : base(dbContext){}

        #endregion

        #region 语句

        /// <summary>
        /// 获取查询语句
        /// </summary>
        /// <returns></returns>
        public string GetSearchSql()
        {
            string searchSql = @"SELECT  T1.* ,
                                        T2.BusinessPartnerName ,
                                        T3.UserName AS CreateByUserName ,
                                        T4.UserName AS UpdateByUserName,
	                                    T5.EnumDesc AS StockOutTypeName,
										T6.EnumDesc AS StockStatusName
                                FROM    WOtherOut T1 WITH ( NOLOCK )
                                        LEFT JOIN BBusinessPartner T2 WITH ( NOLOCK ) ON T1.BusinessPartnerID = T2.ID
                                        LEFT JOIN SUser T3 WITH ( NOLOCK ) ON T1.CreateBy = T3.ID
                                        LEFT JOIN SUser T4 WITH ( NOLOCK ) ON T1.UpdateBy = T4.ID
                                        LEFT JOIN SCommonEnum T5 WITH(NOLOCK) ON T1.StockOutType=T5.EnumField AND T5.EnumType='StockOut'
										LEFT JOIN SCommonEnum T6 WITH(NOLOCK) ON T1.StockStatus=T6.EnumField AND T6.EnumType='StockStatus'";
            return searchSql;
        }

        /// <summary>
        /// 根据ID查询明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WOtherOut GetOtherOutById(int id)
        {
            string sql = GetSearchSql() + " WHERE T1.ID=" + id;
            return Query(sql).FirstOrDefault();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public string GetSearchSql(List<WhereItem> whereItems)
        {
            foreach (WhereItem item in whereItems)
            {
                item.field = "T1." + item.field;
            }
            return GetSearchSql() + BulidWhereSql(whereItems) + GetOrderBy();
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        public string GetOrderBy()
        {
            return " ORDER BY T1.ID DESC";
        }

        #endregion


        /// <summary>
        /// 根据状态删除单据
        /// </summary>
        /// <param name="otherId"></param>
        /// <param name="stockStatus"></param>
        /// <returns></returns>
        public int RemoveOtherOutByStatus(int otherId, int stockStatus)
        {
            string sql = "DELETE FROM WOtherOut WHERE ID=" + otherId + " AND StockStatus=" + stockStatus;
            return Execute(sql);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="otherOut"></param>
        /// <returns></returns>
        public int ApproveOtherOut(WOtherOut otherOut)
        {
            string sql = string.Format("UPDATE WOtherOut SET UpdateDate='{0}',UpdateBy={1},StockStatus={2} WHERE ID={3} AND StockStatus={4}", otherOut.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss"), otherOut.UpdateBy, StockStatusEnum.Approve, otherOut.ID, StockStatusEnum.New);
            return Execute(sql);
        }
    }
}
