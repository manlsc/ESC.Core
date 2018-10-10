using System;
using System.ComponentModel;
using Dapper.Extensions;
using System.Collections.Generic;

namespace ESC.Infrastructure.DomainObjects
{
    public class BLocation : PocoObject
    {
        public BLocation()
        {
            Children = new List<BLocation>();
            Users = new List<SUser>();
            CreateDate = new DateTime(1900, 1, 1);
            UpdateDate = new DateTime(1900, 1, 1);
        }

        /// <summary>
        /// 存储单元编码
        /// </summary>
        [Column("LocationCode", "编码", true, true)]
        public string LocationCode { get; set; }

        /// <summary>
        /// 存储单元描述
        /// </summary>
        [Column("LocationDesc", "描述", true, true)]
        public string LocationDesc { get; set; }

        /// <summary>
        /// 存储单元类型
        /// </summary>
        [Column("LocationType", "类型", true, true, DisplayColumn = "LocationTypeName")]
        public int LocationType { get; set; }

        /// <summary>
        /// 存储单元层级
        /// </summary>
        [Column("LocationLevel", "层级", false, false)]
        public int LocationLevel { get; set; }

        /// <summary>
        /// 饱和状态
        /// </summary>
        [Column("LocationStatus", "饱和状态", false, false)]
        public int LocationStatus { get; set; }

        /// <summary>
        /// 组织机构ID
        /// </summary>
        [Column("OrgID", "组织机构", true, false, DisplayColumn = "OrgName")]
        public int OrgID { get; set; }

        /// <summary>
        /// 上级存储单元
        /// </summary>
        [Column("ParentID", "上级", true, false, DisplayColumn = "ParentName")]
        public int ParentID { get; set; }

        /// <summary>
        /// 顶级存储单元ID
        /// </summary>
        [Column("TopLocationID", "所属仓库", false, false)]
        public int TopLocationID { get; set; }

        /// <summary>
        /// 货区类型
        /// </summary>
        [Column("LocationClass", "货区类型", false, false, DisplayColumn = "LocationClassName")]
        public int LocationClass { set; get; }

        /// <summary>
        /// 是否默认
        /// </summary>
        [Column("IsDefault", "是否默认", false, false)]
        public int IsDefault { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CreateBy", "创建人", false, false, true, 180, "CreateByUserName")]
        public int CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CreateDate", "创建时间", false, false, true, 180)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [Column("UpdateBy", "更新人", false, false, true, 160, "UpdateByUserName")]
        public int UpdateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column("UpdateDate", "更新时间", false, false, true, 180)]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 可用状态
        /// </summary>
        [Column("InUse", "可用状态", false, false, true, 120, "InUseName")]
        public int InUse { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark", "备注", true, false)]
        public string Remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [ResultColumn("CreateByUserName", "创建人", false)]
        public string CreateByUserName { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [ResultColumn("UpdateByUserName", "更新人", false)]
        public string UpdateByUserName { get; set; }

        /// <summary>
        /// 组织机构
        /// </summary>
        [ResultColumn("OrgName", "组织机构", false)]
        public string OrgName { get; set; }

        /// <summary>
        /// 上级存储单元
        /// </summary>
        [ResultColumn("ParentName", "上级", false)]
        public string ParentName { get; set; }

        /// <summary>
        /// 上级编码
        /// </summary>
        [ResultColumn("ParentCode", "上级编码", false)]
        public string ParentCode { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        [ResultColumn("InUseName", "是否可用", false)]
        public string InUseName { get; set; }

        /// <summary>
        /// 存储单元类型
        /// </summary>
        [ResultColumn("LocationTypeName", "类型", false)]
        public string LocationTypeName { get; set; }

        /// <summary>
        /// 是否默认
        /// </summary>
        [ResultColumn("IsDefaultName", "是否默认", false)]
        public string IsDefaultName { get; set; }

        /// <summary>
        /// 货区类型
        /// </summary>
        [ResultColumn("LocationClassName", "货区类型", false)]
        public string LocationClassName { set; get; }

        /// <summary>
        /// 子仓库
        /// </summary>
        [Ignore]
        public List<BLocation> Children { set; get; }

        /// <summary>
        /// 库管员
        /// </summary>
        [Ignore]
        public List<SUser> Users { set; get; }
    }
}
