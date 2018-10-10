using Dapper.Extensions;
using System;
using System.Collections.Generic;

namespace ESC.Infrastructure.DomainObjects
{
    /// <summary>
    /// 组织结构
    /// </summary>
    public class SOrganization : PocoObject
    {
        public SOrganization()
        {
            ParentID = 0;
            UserID = 0;
            OrgType = 0;
            Users = new List<SUser>();
            Children = new List<SOrganization>();
        }

        /// <summary>
        /// 机构编码
        /// </summary>
        [Column("OrgCode", "机构编码", true, true)]
        public string OrgCode { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        [Column("OrgName", "机构名称", true, true)]
        public string OrgName { get; set; }

        /// <summary>
        /// 上级机构
        /// </summary>
        [Column("ParentID", "上级机构", true, false, DisplayColumn = "ParentName")]
        public int ParentID { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        [Column("UserID", "用户编码", false, false)]
        public int UserID { get; set; }

        /// <summary>
        /// 机构类型
        /// </summary>
        [Column("OrgType", "机构类型", false, false)]
        public int OrgType { get; set; }

        /// <summary>
        /// 机构地址
        /// </summary>
        [Column("Address", "机构地址", true, false)]
        public string Address { get; set; }

        /// <summary>
        /// 可用状态
        /// </summary>
        [Column("InUse", "可用状态", false, false)]
        public int InUse { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        [Column("OrgLevel", "级别", false)]
        public int OrgLevel { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        [Column("ShortName", "简称", true, false)]
        public string ShortName { get; set; }

        /// <summary>
        /// 上级机构
        /// </summary>
        [ResultColumn("ParentName", "上级机构", false)]
        public string ParentName { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        [ResultColumn("UserName", "负责人", false)]
        public string UserName { get; set; }

        /// <summary>
        /// 可用状态
        /// </summary>
        [ResultColumn("InUseName", "可用状态", false)]
        public string InUseName { set; get; }

        /// <summary>
        /// 机构人员
        /// </summary>
        [Ignore]
        public List<SUser> Users { set; get; }

        /// <summary>
        /// 子组织机构
        /// </summary>
        [Ignore]
        public List<SOrganization> Children { set; get; }
    }
}
