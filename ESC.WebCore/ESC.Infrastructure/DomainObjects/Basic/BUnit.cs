using System;
using System.ComponentModel;
using Dapper.Extensions;
using System.Collections.Generic;


namespace ESC.Infrastructure.DomainObjects
{
    /// <summary>
    /// 基本计量单位实体
    /// </summary>
    public class BUnit : PocoObject
    {
        public BUnit()
        {
            CreateDate = new DateTime(1900, 1, 1);
            UpdateDate = new DateTime(1900, 1, 1);
        }

        /// <summary>
        /// 单位编码
        /// </summary>
        [Column("UnitCode", "单位编码", true, true, false, 160, "")]
        public string UnitCode { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [Column("UnitName", "单位名称", true, true, false, 160, "")]
        public string UnitName { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Column("UnitType", "类型", true, false, false, 160, "UnitTypeName")]
        public int UnitType { get; set; }

        /// <summary>
        /// 组织机构
        /// </summary>
        [Column("OrgID", "组织机构", false, false, false, 160, "OrgName")]
        public int OrgID { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        [Column("InUse", "是否可用", false, false, false, 160, "InUseName")]
        public int InUse { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CreateDate", "创建时间", true, false, true, 160, "")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CreateBy", "创建人", true, false, true, 160, "CreateByUserName")]
        public int CreateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column("UpdateDate", "更新时间", true, false, true, 160, "")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [Column("UpdateBy", "更新人", true, false, true, 160, "UpdateByUserName")]
        public int UpdateBy { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark", "备注", true, false, false, 160, "")]
        public string Remark { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        [ResultColumn]
        public string CreateByUserName { set; get; }

        /// <summary>
        /// 更新人名称
        /// </summary>
        [ResultColumn]
        public string UpdateByUserName { set; get; }

        /// <summary>
        /// 组织名称
        /// </summary>
        [ResultColumn]
        public string OrgName { get; set; }

        /// <summary>
        /// 是否自定义计量单位(相对国际标准计量单位而言)
        /// </summary>
        [ResultColumn]
        public string UnitTypeName { get; set; }

        /// <summary>
        /// 可用状态
        /// </summary>
        [ResultColumn]
        public string InUseName { get; set; }
    }
}