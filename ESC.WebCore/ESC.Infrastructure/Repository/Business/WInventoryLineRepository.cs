using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 盘点明细行 +
    /// </summary>
    public class WInventoryLineRepository : BaseRepository<WInventoryLine>
    {

        #region 构造

        public WInventoryLineRepository() : base() { }

        public WInventoryLineRepository(string connectionString) : base(connectionString) { }

        public WInventoryLineRepository(DatabaseContext dbContext) : base(dbContext) { }

        #endregion

        #region 语句

        public string GetSearchSql()
        {
            string searchSql = @"SELECT  S.* ,
                                            M.MaterialName ,
                                            WH.LocationDesc AS WarehouseName ,
                                            L.LocationDesc AS PositionName ,
                                            BU.UnitName
                                    FROM    WInventoryLine S WITH ( NOLOCK )
                                            LEFT JOIN BMaterial M WITH ( NOLOCK ) ON S.MaterialID = M.ID
                                            LEFT JOIN BLocation WH WITH ( NOLOCK ) ON S.WarehouseID = WH.ID
                                            LEFT JOIN BLocation L WITH ( NOLOCK ) ON S.PositionID = L.ID
                                            LEFT JOIN BUnit BU WITH ( NOLOCK ) ON S.UnitID = BU.ID";
            return searchSql;
        }

        public string GetSearchSql(List<WhereItem> whereItems)
        {
            foreach (var item in whereItems)
            {
                item.field = "S." + item.field;
            }
            return GetSearchSql() + BulidWhereSql(whereItems) + GetOrderBy();
        }

        public string GetOrderBy()
        {
            return " ORDER BY S.ID DESC";
        }

        #endregion

        /// <summary>
        /// 根据盘点id查询盘点明细
        /// </summary>
        /// <param name="invId"></param>
        /// <returns></returns>
        public List<WInventoryLine> GetInventoryLinesByParent(int invId)
        {
            string sql = " SELECT * FROM WInventoryLine WITH(NOLOCK) WHERE ParentID=" + invId;
            return Query(sql);
        }

        /// <summary>
        /// 是否存在盘亏
        /// </summary>
        /// <param name="invId"></param>
        /// <returns>如果不存在返回true</returns>
        public bool HasNoLoss(int invId)
        {
            string sql = " SELECT TOP 1 ID  FROM WInventoryLine WITH(NOLOCK) WHERE ParentID=" + invId + " AND InventoryDiff<0";
            string result = ExecuteScalar<string>(sql);
            return string.IsNullOrEmpty(result);
        }

        /// <summary>
        /// 是否存在盘盈
        /// </summary>
        /// <param name="invId"></param>
        /// <returns>如果不存在返回true</returns>
        public bool HasNoProfit(int invId)
        {
            string sql = " SELECT TOP 1 ID  FROM WInventoryLine WITH(NOLOCK) WHERE ParentID=" + invId + " AND InventoryDiff>0";
            string result = ExecuteScalar<string>(sql);
            return string.IsNullOrEmpty(result);
        }

        /// <summary>
        /// 根据id删除明显
        /// </summary>
        /// <param name="invId"></param>
        /// <returns></returns>
        public int RemoveLinesByParent(int invId)
        {
            string sql = " DELETE FROM WInventoryLine WHERE ParentID=" + invId;
            return Execute(sql);
        }
    }
}
