using System;
using System.ComponentModel;
using Dapper.Extensions;
using System.Collections.Generic;

namespace ESC.Infrastructure.DomainObjects
{
    public class BMaterialGroup : PocoObject
    {
        public BMaterialGroup()
        {
            CreateDate = new DateTime(1900, 1, 1);
            UpdateDate = new DateTime(1900, 1, 1);
            Children = new List<BMaterialGroup>();
        }
    
        /// <summary>
        /// 物料组编码
        /// </summary>
        [Column("GroupCode", "编码", true, true, false, 160, "")]
        public string GroupCode { get; set; }

        /// <summary>
        /// 物料组名称
        /// </summary>
        [Column("GroupName", "名称", true, true, false, 160, "")]
        public string GroupName { get; set; }

        /// <summary>
        /// 顶级物料组
        /// </summary>
        [Column("TopGroupID", "顶级物料组", false, false, false, 160, "")]
        public int TopGroupID { get; set; }

        /// <summary>
        /// 物料组级别
        /// </summary>
        [Column("GroupLevel", "级别", false, false, false, 160, "")]
        public int GroupLevel { get; set; }

        /// <summary>
        /// 组织机构
        /// </summary>
        [Column("OrgID", "组织机构", true, false, false, 160, "OrgName")]
        public int OrgID { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        [Column("InUse", "是否可用", false, false, false, 160, "InUseName")]
        public int InUse { get; set; }

        /// <summary>
        /// 父物料组
        /// </summary>
        [Column("ParentID", "父物料组", true, false, false, 160, "ParentName")]
        public int ParentID { get; set; }


        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CreateBy", "创建人", true, true, true, 160, "CreateByUserName")]
        public int CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CreateDate", "创建时间", true, true, true, 160, "")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [Column("UpdateBy", "更新人", true, true, true, 160, "UpdateByUserName")]
        public int UpdateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column("UpdateDate", "更新时间", true, true, true, 160, "")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark", "备注", true, false, false, 160, "")]
        public string Remark { get; set; }

        /// <summary>
        /// 上级物料组
        /// </summary>
        [ResultColumn("ParentName")]
        public string ParentName { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [ResultColumn("UpdateByUserName")]
        public string UpdateByUserName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [ResultColumn("CreateByUserName")]
        public string CreateByUserName { get; set; }

        /// <summary>
        /// 组织机构
        /// </summary>
        [ResultColumn("OrgName")]
        public string OrgName { get; set; }

        [Ignore]
        public List<BMaterialGroup> Children { set; get; }
    }
}
