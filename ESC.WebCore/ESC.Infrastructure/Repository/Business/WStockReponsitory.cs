using System;
using System.Collections.Generic;
using ESC.Core;
using ESC.Infrastructure.DomainObjects;
using System.Linq;
using System.Data;

namespace ESC.Infrastructure.Repository
{
    /// <summary>
    /// 库存操作 +
    /// </summary>
    public class WStockRepository : BaseRepository<WStock>
    {
        #region 构造

        public WStockRepository() : base() { }

        public WStockRepository(string connectionString) : base(connectionString) { }

        public WStockRepository(DatabaseContext dbContext) : base(dbContext) { }

        #endregion

        #region 语句

        /// <summary>
        /// 获取查询语句
        /// </summary>
        /// <returns></returns>
        public string GetSearchSql()
        {
            string searchSql = @"SELECT  S.* ,
                                            M.MaterialName ,
                                            WH.LocationDesc AS WarehouseName ,
                                            L.LocationDesc AS PositionName ,
                                            BU.UnitName
                                    FROM    WStock S WITH ( NOLOCK )
                                            LEFT JOIN BMaterial M WITH ( NOLOCK ) ON S.MaterialID = M.ID
                                            LEFT JOIN BLocation WH WITH ( NOLOCK ) ON S.WarehouseID = WH.ID
                                            LEFT JOIN BLocation L WITH ( NOLOCK ) ON S.PositionID = L.ID
                                            LEFT JOIN BUnit BU WITH ( NOLOCK ) ON S.UnitID = BU.ID";
            return searchSql;
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
                switch (item.field)
                {
                    case "MaterialName":
                        item.field = "M." + item.field;
                        break;
                    case "WarehouseName":
                        item.field = "WH.WarehouseName";
                        break;
                    default:
                        item.field = "S." + item.field;
                        break;

                }
            }
            return GetSearchSql() + BulidWhereSql(whereItems) + GetOrderBy();
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        protected string GetOrderBy()
        {
            return " ORDER BY S.ID";
        }

        #endregion

        #region 查询库存

        /// <summary>
        /// 查询库存
        /// </summary>
        /// <param name="materialId">物料ID</param>
        /// <param name="whouseId">仓库ID</param>
        /// <param name="positionId">货位ID</param>
        /// <param name="batch">批次</param>
        /// <param name="ownerCode">货主</param>
        /// <returns></returns>
        public List<WStock> GetStocks(int materialId, int whouseId, int positionId, string batch, string ownerCode)
        {
            string sql = "SELECT * FROM WStock S WITH (NOLOCK) WHERE S.MaterialID=" + materialId;
            if (string.IsNullOrWhiteSpace(batch))
            {
                sql += " AND S.Batch=''";
            }
            else
            {
                sql += " AND S.Batch='" + batch + "'";
            }
            //因为货位在基础信息唯一,所以存在货位就根据货位查询库存
            if (positionId > 0)
            {
                sql += " AND S.PositionID=" + positionId;
            }
            else
            {
                sql += " AND S.WarehouseID=" + whouseId;
            }
            if (string.IsNullOrWhiteSpace(ownerCode))
            {
                sql += " AND S.OwnerCode=''";
            }
            else
            {
                sql += " AND S.OwnerCode='" + ownerCode + "'";
            }
            sql += " AND S.StockCount>0";
            sql += " ORDER BY S.CreateDate ASC";   //根据入库时间倒序
            return Query(sql);
        }

        /// <summary>
        /// 查询库存
        /// </summary>
        /// <param name="materialCode">物料编码</param>
        /// <param name="whouseCode">仓库编码</param>
        /// <param name="positionCode">货位编码</param>
        /// <param name="batch">批次</param>
        /// <param name="ownerCode">货主</param>
        /// <returns></returns>
        public List<WStock> GetStocks(string materialCode, string whouseCode, string positionCode, string batch, string ownerCode)
        {
            string sql = "SELECT * FROM WStock S WITH (NOLOCK) WHERE S.MaterialCode=" + materialCode;
            if (string.IsNullOrWhiteSpace(batch))
            {
                sql += " AND S.Batch=''";
            }
            else
            {
                sql += " AND S.Batch='" + batch + "'";
            }
            //因为货位在基础信息唯一,所以存在货位就根据货位查询库存
            if (string.IsNullOrWhiteSpace(positionCode))
            {
                sql += " AND S.WarehouseCode='" + whouseCode + "'";
            }
            else
            {
                sql += " AND S.PositionCode='" + positionCode + "'";
            }
            if (string.IsNullOrWhiteSpace(ownerCode))
            {
                sql += " AND S.OwnerCode=''";
            }
            else
            {
                sql += " AND S.OwnerCode='" + ownerCode + "'";
            }
            sql += " AND S.StockCount>0";
            sql += " ORDER BY S.CreateDate ASC";   //根据入库时间倒序
            return Query(sql);
        }

        /// <summary>
        /// 获取库存
        /// </summary>
        /// <param name="materialId">物料ID</param>
        /// <param name="positionId">货位ID</param>
        /// <param name="createDate">入库时间</param>
        /// <param name="ownerCode">货主</param>
        /// <param name="batch">批次</param>
        /// <returns></returns>
        public WStock GetStocks(int materialId, int positionId, DateTime createDate, string ownerCode, string batch)
        {
            string sql = "SELECT * FROM WStock S WITH (NOLOCK) WHERE S.MaterialID=" + materialId + " AND S.PositionID=" + positionId + " AND S.DateBatch='" + createDate.ToString("yyyyMMdd") + "'";
            if (string.IsNullOrWhiteSpace(ownerCode))
            {
                sql += " AND S.OwnerCode=''";
            }
            else
            {
                sql += " AND S.OwnerCode='" + ownerCode + "'";
            }
            if (string.IsNullOrWhiteSpace(batch))
            {
                sql += " AND S.Batch=''";
            }
            else
            {
                sql += " AND S.Batch='" + batch + "'";
            }
            sql += " AND S.StockCount>0";
            return FirstOrDefault(sql);
        }

        /// <summary>
        /// 获取库存
        /// </summary>
        /// <param name="materialId">物料ID</param>
        /// <param name="positionId">货位ID</param>
        /// <param name="createDate">入库时间</param>
        /// <param name="batch">批次</param>
        /// <returns></returns>
        public WStock GetStocks(int materialId, int positionId, DateTime createDate, string batch)
        {
            string sql = "SELECT * FROM WStock S WITH (NOLOCK) WHERE S.MaterialID=" + materialId + " AND S.PositionID=" + positionId + " AND S.DateBatch='" + createDate.ToString("yyyyMMdd") + "'";
            if (string.IsNullOrWhiteSpace(batch))
            {
                sql += " AND S.Batch=''";
            }
            else
            {
                sql += " AND S.Batch='" + batch + "'";
            }
            sql += " AND S.StockCount>0";
            return FirstOrDefault(sql);
        }

        /// <summary>
        /// 获取库存
        /// </summary>
        /// <param name="materialId">物料ID</param>
        /// <param name="positionId">货位ID</param>
        /// <param name="batch">批次</param>
        /// <returns></returns>
        public WStock GetStocks(int materialId, int positionId, string batch)
        {
            string sql = "SELECT * FROM WStock S WITH (NOLOCK) WHERE S.MaterialID=" + materialId + " AND S.PositionID=" + positionId;
            if (string.IsNullOrWhiteSpace(batch))
            {
                sql += " AND S.Batch=''";
            }
            else
            {
                sql += " AND S.Batch='" + batch + "'";
            }
            sql += " AND S.StockCount>0";
            return FirstOrDefault(sql);
        }

        #endregion

        #region 操作

        /// <summary>
        /// 新增库存数量
        /// 添加库存 添加可用数量
        /// 采购入库 其他入库
        /// </summary>
        /// <param name="stockId">库存ID</param>
        /// <param name="inCount">入库数量</param>
        /// <returns></returns>
        public int AddStockCount(int stockId, decimal inCount)
        {
            string sql = "UPDATE WStock SET UnLimitCount=UnLimitCount+" + inCount + " ,StockCount=StockCount+" + inCount + " WHERE ID=" + stockId;
            return Execute(sql);
        }

        /// <summary>
        /// 减少库存数量
        /// 减少可用数量 减少库存
        /// 销售出库 其他出库
        /// </summary>
        /// <param name="stockId">库存ID</param>
        /// <param name="outCount">出库数量</param>
        /// <returns></returns>
        public decimal DeleteStockCount(int stockId, decimal outCount)
        {
            string sql = "UPDATE WStock SET UnLimitCount=UnLimitCount-" + outCount + " ,StockCount=StockCount-" + outCount + " OUTPUT Inserted.UnLimitCount WHERE ID=" + stockId;
            return ExecuteScalar<decimal>(sql);
        }

        /// <summary>
        /// 添加占用数量
        /// 添加占用 减少可用
        /// 通知单下推出库单  占用库存
        /// </summary>
        /// <param name="stockId">库存ID</param>
        /// <param name="takeCount">占用数量</param>
        /// <returns></returns>
        public decimal AddTakeCount(int stockId, decimal takeCount)
        {
            string sql = "UPDATE WStock SET UnLimitCount=UnLimitCount-" + takeCount + " ,TakeCount=TakeCount+" + takeCount + " OUTPUT Inserted.UnLimitCount WHERE ID=" + stockId;
            return ExecuteScalar<decimal>(sql);
        }

        #endregion

        #region 调拨出入库

        /// <summary>
        /// 添加在途 减少可用
        /// 调拨出库
        /// </summary>
        /// <param name="stockId">库存ID</param>
        /// <param name="transitCount">在途数量</param>
        /// <returns></returns>
        public decimal AddTransitCount(int stockId, decimal transitCount)
        {
            string sql = "UPDATE WStock SET UnLimitCount=UnLimitCount-" + transitCount + " ,TransitCount=TransitCount+" + transitCount + " OUTPUT Inserted.UnLimitCount WHERE ID=" + stockId;
            return ExecuteScalar<decimal>(sql);
        }

        /// <summary>
        /// 减少在途数量 减少库存数量
        /// 调拨入库
        /// </summary>
        /// <param name="stockId"></param>
        /// <param name="transitCount"></param>
        /// <returns></returns>
        public decimal DeleteTransitCount(int stockId, decimal transitCount)
        {
            string sql = "UPDATE WStock SET TransitCount=TransitCount-" + transitCount + " ,StockCount=StockCount-" + transitCount + " OUTPUT Inserted.TransitCount WHERE ID=" + stockId;
            return ExecuteScalar<decimal>(sql);
        }

        /// <summary>
        /// 减少在途,添加可用
        /// 调拨出库 退库(拒收)
        /// </summary>
        /// <param name="stockId"></param>
        /// <param name="transitCount"></param>
        /// <returns></returns>
        public decimal DeleteTransitCountBack(int stockId, decimal transitCount)
        {
            string sql = "UPDATE WStock SET UnLimitCount=UnLimitCount+" + transitCount + " ,TransitCount=TransitCount-" + transitCount + " OUTPUT Inserted.TransitCount WHERE ID=" + stockId;
            return ExecuteScalar<decimal>(sql);
        }

        #endregion

        #region 冻结解冻

        /// <summary>
        /// 添加库存冻结
        /// </summary>
        /// <param name="stockId"></param>
        /// <param name="frozenCount"></param>
        /// <returns></returns>
        public int AddFrozenCount(int stockId, decimal frozenCount)
        {
            string sql = "UPDATE WStock SET UnLimitCount=UnLimitCount-" + frozenCount + " ,FrozenCount=FrozenCount+" + frozenCount + " OUTPUT Inserted.UnLimitCount WHERE ID=" + stockId;
            return ExecuteScalar<int>(sql);
        }

        /// <summary>
        /// 减少库存冻结
        /// </summary>
        /// <param name="stockId"></param>
        /// <param name="frozenCount"></param>
        /// <returns></returns>
        public int DeleteFrozenCount(int stockId, decimal frozenCount)
        {
            string sql = "UPDATE WStock SET UnLimitCount=UnLimitCount+" + frozenCount + " ,FrozenCount=FrozenCount-" + frozenCount + " OUTPUT Inserted.FrozenCount WHERE ID=" + stockId;
            return ExecuteScalar<int>(sql);
        }

        #endregion

        #region 导出

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public DataTable GetStocks(List<WhereItem> whereItems)
        {
            string sql = GetSearchSql(whereItems);
            return QueryTable(sql);
        }

        #endregion
    }
}
