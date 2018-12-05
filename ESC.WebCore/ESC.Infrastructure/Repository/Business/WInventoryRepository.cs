using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;
using System.Linq;
using ESC.Infrastructure.Enums;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 盘点 +
    /// </summary>
    public class WInventoryRepository : BaseRepository<WInventory>
    {

        #region 构造

        public WInventoryRepository() : base() { }

        public WInventoryRepository(string connectionString) : base(connectionString) { }

        public WInventoryRepository(DatabaseContext dbContext) : base(dbContext) { }

        #endregion

        #region 语句

        public string GetSearchSql()
        {
            string searchSql = @"   SELECT T1.* ,
                                            T3.UserName AS CreateByUserName ,
                                            T4.UserName AS UpdateByUserName ,
                                            T5.EnumDesc AS InventoryStatusName
                                     FROM   WInventory T1 WITH ( NOLOCK )
                                            LEFT JOIN SUser T3 WITH ( NOLOCK ) ON T1.CreateBy = T3.ID
                                            LEFT JOIN SUser T4 WITH ( NOLOCK ) ON T1.UpdateBy = T4.ID
                                            LEFT JOIN SCommonEnum T5 WITH ( NOLOCK ) ON T1.InventoryStatus = T5.EnumField
                                                                                            AND T5.EnumType = 'InventoryStatus'";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public WInventory GetInventoryById(int id)
        {
            string sql = GetSearchSql() + " WHERE T1.ID=" + id;
            return Query(sql).FirstOrDefault();
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public WInventory GetInventoryByCode(string code)
        {
            string sql = GetSearchSql() + " WHERE T1.InventoryCode='" + code + "'";
            return Query(sql).FirstOrDefault();
        }

        /// <summary>
        /// 更新状态-根据盘亏单
        /// </summary>
        /// <param name="Id">盘点id</param>
        /// <param name="outId">盘亏单ID</param>
        /// <param name="outCode">盘亏单Code</param>
        /// <param name="invStatus">状态</param>
        /// <returns></returns>
        public int UpdateLossStatus(int Id, int outId, string outCode, int invStatus)
        {
            string sql = string.Format("UPDATE WInventory SET OtherOutID={0},OtherOutCode='{1}',InventoryStatus={2} WHERE ID={3} AND InventoryStatus<{4}", outId, outCode, invStatus, Id, InventoryStatusEnum.Complete);
            return Execute(sql);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="Id">盘点id</param>
        /// <param name="inId">盘盈单ID</param>
        /// <param name="inCode">盘盈单Code</param>
        /// <param name="invStatus">状态</param>
        /// <returns></returns>
        public int UpdateProfitStatus(int Id, int inId, string inCode, int invStatus)
        {
            string sql = string.Format("UPDATE WInventory SET OtherInID={0},OtherInCode='{1}',InventoryStatus={2} WHERE ID={3} AND InventoryStatus<{4}", inId, inCode, invStatus, Id, InventoryStatusEnum.Complete);
            return Execute(sql);
        }
    }
}
