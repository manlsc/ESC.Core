using System;
using System.ComponentModel;
using Dapper.Extensions;

namespace ESC.Infrastructure.DomainObjects
{
    public class WInventoryLine : PocoObject
    {

        public WInventoryLine()
        {
            StockInDate = new DateTime(1900, 1, 1);
        }

        [Column("ParentID", "ParentID", false)]
        public int ParentID { set; get; }

        /// <summary>
        /// 库存ID
        /// </summary>
        [Column("StockID", "库存ID", false, false, false, 160, "")]
        public int StockID { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        [Column("WarehouseCode", "仓库编码", true, true, true, 160, "")]
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        [Column("WarehouseID", "仓库名称", true, true, true, 160, "WarehouseName")]
        public int WarehouseID { get; set; }

        /// <summary>
        /// 货位编码
        /// </summary>
        [Column("PositionCode", "货位编码", true, true, true, 160, "")]
        public string PositionCode { get; set; }

        /// <summary>
        /// 货位
        /// </summary>
        [Column("PositionID", "货位名称", true, true, true, 160, "PositionName")]
        public int PositionID { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        [Column("MaterialCode", "物料编码", true, true, true, 160, "")]
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        [Column("MaterialID", "物料名称", true, true, true, 160, "MaterialName")]
        public int MaterialID { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [Column("UnitID", "单位", false, true, true, 160, "UnitName")]
        public int UnitID { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        [Column("Batch", "批次", true, true, true, 160, "")]
        public string Batch { get; set; }

        /// <summary>
        /// 亚批次
        /// </summary>
        [Column("SecBatch", "亚批次", false, false, true, 160, "")]
        public string SecBatch { get; set; }

        /// <summary>
        /// 所有者
        /// </summary>
        [Column("OwnerCode", "所有者", false, true, true, 160, "")]
        public string OwnerCode { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        [Column("StockCount", "库存数量", true, true, true, 160, "")]
        public decimal StockCount { get; set; }

        /// <summary>
        /// 可用数量
        /// </summary>
        [Column("UnLimitCount", "可用数量", true, true, true, 160, "")]
        public decimal UnLimitCount { get; set; }

        /// <summary>
        /// 占用数量
        /// </summary>
        [Column("TakeCount", "占用数量", false, true, true, 160, "")]
        public decimal TakeCount { get; set; }

        /// <summary>
        /// 冻结数量
        /// </summary>
        [Column("FrozenCount", "冻结数量", false, true, true, 160, "")]
        public decimal FrozenCount { get; set; }

        /// <summary>
        /// 在途数量
        /// </summary>
        [Column("TransitCount", "在途数量", false, true, true, 160, "")]
        public decimal TransitCount { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        [Column("Factory", "工厂", false, true, true, 160, "")]
        public string Factory { get; set; }

        /// <summary>
        /// 时间批次
        /// </summary>
        [Column("DateBatch", "时间批次", false, true, true, 160, "")]
        public string DateBatch { set; get; }

        /// <summary>
        /// 入库时间
        /// </summary>
        [Column("StockInDate", "入库时间", false, true, true, 160, "")]
        public DateTime StockInDate { get; set; }

        /// <summary>
        /// 盘点数量
        /// </summary>
        [Column("InventoryCount", "盘点数量", true, false, false, 160, "")]
        public decimal? InventoryCount { get; set; }

        /// <summary>
        /// 盘点盈亏
        /// </summary>
        [Column("InventoryDiff", "盘点盈亏", true, true, true, 160, "")]
        public decimal? InventoryDiff { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        [ResultColumn]
        public string MaterialName { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        [ResultColumn]
        public string WarehouseName { get; set; }

        /// <summary>
        /// 货位
        /// </summary>
        [ResultColumn]
        public string PositionName { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [ResultColumn]
        public string UnitName { get; set; }
    }
}
