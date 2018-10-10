using System;
using System.ComponentModel;
using Dapper.Extensions;

namespace ESC.Infrastructure.DomainObjects
{
    /// <summary>
    /// 其他入库
    /// </summary>
    public class WOtherInLine : PocoObject
    {

        public WOtherInLine()
        {
           CreateDate = new DateTime(1900, 1, 1);
           UpdateDate = new DateTime(1900, 1, 1);
        }

        [Column("ParentID", "ParentID",false)]
        public int ParentID { set; get; }

        /// <summary>
        /// 仓库
        /// </summary>
        [Column("WarehouseID","仓库",false,true,false,160, "WarehouseName")]
        public int WarehouseID { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        [Column("WarehouseCode","仓库编码", false, true, true, 160,"")]
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 货位
        /// </summary>
        [Column("PositionID", "货位", true, true, false, 160, "PositionName")]
        public int PositionID { get; set; }

        /// <summary>
        /// 货位编码
        /// </summary>
        [Column("PositionCode","货位编码",true,true, true, 160,"")]
        public string PositionCode { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        [Column("MaterialID","物料",true,true,false,160, "MaterialName")]
        public int MaterialID { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        [Column("MaterialCode","物料编码",true,true, true, 160,"")]
        public string MaterialCode { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [Column("UnitID","单位",true,false, true, 160, "UnitName")]
        public int UnitID { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        [Column("Batch","批次",true,false,false,160,"")]
        public string Batch { get; set; }

        /// <summary>
        /// 所有者
        /// </summary>
        [Column("OwnerCode","所有者",false,false,false,160,"")]
        public string OwnerCode { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        [Column("Factory","工厂",false,false,false,160,"")]
        public string Factory { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CreateDate","创建时间",true,true,true,160,"")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CreateBy","创建人",true,true,true,160, "CreateByUserName")]
        public int CreateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column("UpdateDate","更新时间",true,true,true,160,"")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [Column("UpdateBy","更新人",true,true,true,160, "UpdateByUserName")]
        public int UpdateBy { get; set; }

        /// <summary>
        /// 入库数量
        /// </summary>
        [Column("InCount","入库数量",true,true,false,160,"")]
        public decimal InCount { get; set; }

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

        /// <summary>
        /// 创建人
        /// </summary>
        [ResultColumn]
        public string CreateByUserName { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [ResultColumn]
        public string UpdateByUserName { get; set; }
    }
}
