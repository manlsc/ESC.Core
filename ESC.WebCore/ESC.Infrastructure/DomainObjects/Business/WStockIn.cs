using System;
using System.ComponentModel;
using Dapper.Extensions;

namespace ESC.Infrastructure.DomainObjects
{
    /// <summary>
    /// 入库记录
    /// </summary>
    public class WStockIn : PocoObject
    {

        public WStockIn()
        {
           StockInDate = new DateTime(1900, 1, 1);
        }

        /// <summary>
        /// 仓库
        /// </summary>
        [Column("WarehouseID","仓库",true,true,false,160,"")]
        public int WarehouseID { get; set; }

        /// <summary>
        /// 货位
        /// </summary>
        [Column("PositionID","货位",true,true,false,160,"")]
        public int PositionID { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        [Column("WarehouseCode","仓库编码",true,true,false,160,"")]
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 货位编码
        /// </summary>
        [Column("PositionCode","货位编码",true,true,false,160,"")]
        public string PositionCode { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        [Column("MaterialID","物料",true,true,false,160,"")]
        public int MaterialID { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        [Column("MaterialCode","物料编码",true,true,false,160,"")]
        public string MaterialCode { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [Column("UnitID","单位",true,true,false,160,"")]
        public int UnitID { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        [Column("Batch","批次",true,true,false,160,"")]
        public string Batch { get; set; }

        /// <summary>
        /// 所有者
        /// </summary>
        [Column("OwnerCode","所有者",true,true,false,160,"")]
        public string OwnerCode { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        [Column("Factory","工厂",true,true,false,160,"")]
        public string Factory { get; set; }

        /// <summary>
        /// 入库时间
        /// </summary>
        [Column("StockInDate","入库时间",true,true,false,160,"")]
        public DateTime StockInDate { get; set; }

        /// <summary>
        /// 来源ID
        /// </summary>
        [Column("SourceID","来源ID",true,true,false,160,"")]
        public long SourceID { get; set; }

        /// <summary>
        /// 来源编码
        /// </summary>
        [Column("SourceCode","来源编码",true,true,false,160,"")]
        public string SourceCode { get; set; }

        /// <summary>
        /// 入库类型
        /// </summary>
        [Column("StockInType","入库类型",true,true,false,160,"")]
        public int StockInType { get; set; }

        /// <summary>
        /// 入库数量
        /// </summary>
        [Column("InCount","入库数量",true,true,false,160,"")]
        public decimal InCount { get; set; }

        /// <summary>
        /// 入库行ID
        /// </summary>
        [Column("SourceLineID","入库行ID",true,true,false,160,"")]
        public long SourceLineID { get; set; }

        /// <summary>
        /// 库存ID
        /// </summary>
        [Column("StockID", "库存ID", false, false, false, 160, "")]
        public int StockID { get; set; }
    }
}