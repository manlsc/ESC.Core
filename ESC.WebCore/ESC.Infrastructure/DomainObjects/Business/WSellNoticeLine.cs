using System;
using System.ComponentModel;
using Dapper.Extensions;

namespace ESC.Infrastructure.DomainObjects
{
    public class WSellNoticeLine : PocoObject
    {
        public WSellNoticeLine()
        {
            CreateDate = new DateTime(1900, 1, 1);
            UpdateDate = new DateTime(1900, 1, 1);
        }

        /// <summary>
        /// ParentID
        /// </summary>
        [Column("ParentID", "ParentID", false, true, true, 160, "")]
        public int ParentID { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        [Column("MaterialID", "物料", true, true, false, 160, "MaterialName")]
        public int MaterialID { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        [Column("MaterialCode", "物料编码", true, true, true, 160, "")]
        public string MaterialCode { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [Column("UnitID", "单位", true, true, true, 160, "UnitName")]
        public int UnitID { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        [Column("Batch", "批次", true, true, true, 160, "")]
        public string Batch { get; set; }

        /// <summary>
        /// 通知数量
        /// </summary>
        [Column("OutCount", "通知数量", true, true, false, 160, "")]
        public decimal OutCount { get; set; }

        /// <summary>
        /// 完成数量
        /// </summary>
        [Column("CompleteCount", "完成数量", true, true, true, 160, "")]
        public decimal CompleteCount { get; set; }

        /// <summary>
        /// 下推数量
        /// </summary>
        [Column("DownCount", "下推数量", true, true, true, 160, "")]
        public decimal DownCount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark", "备注", true, false, false, 160, "")]
        public string Remark { get; set; }

        /// <summary>
        /// 所有者
        /// </summary>
        [Column("OwnerCode", "所有者", true, true, true, 160, "")]
        public string OwnerCode { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        [Column("Factory", "工厂", false, false, false, 160, "")]
        public string Factory { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CreateBy", "创建人", true, true, true, 160, "")]
        public int CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CreateDate", "创建时间", true, true, true, 160, "")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [Column("UpdateBy", "更新人", true, true, true, 160, "")]
        public int UpdateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column("UpdateDate", "更新时间", true, true, true, 160, "")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        [ResultColumn]
        public string MaterialName { get; set; }

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
