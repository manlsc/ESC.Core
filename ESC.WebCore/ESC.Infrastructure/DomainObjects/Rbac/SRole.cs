using Dapper.Extensions;
using System;
using System.Collections.Generic;

namespace ESC.Infrastructure.DomainObjects
{
    /// <summary>
    /// 角色
    /// Role是关键字,所以添加一个前缀
    /// </summary>
    public class SRole : PocoObject
    {
        public SRole()
        {
            CreateDate = new DateTime(1900, 1, 1);
        }

        /// <summary>
        /// 名称
        /// </summary>
        [Column("RoleName", "名称", true, true, false, 160)]
        public string RoleName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Column("RoleDesc", "描述", true, false, false, 260)]
        public string RoleDesc { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CreateBy", "创建人", true, false, true, 260, "CreateByUserName")]
        public int CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CreateDate", "创建时间", true, false, true, 260)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [ResultColumn("CreateByUserName", "创建人", false)]
        public string CreateByUserName { get; set; }

        ///// <summary>
        ///// 角色拥有用户
        ///// </summary>
        //[Ignore]
        //public List<SUser> Users { set; get; }

        ///// <summary>
        ///// 操作权限
        ///// </summary>        
        //[Ignore]
        //public List<SOPermission> OPermissions { set; get; }
    }
}