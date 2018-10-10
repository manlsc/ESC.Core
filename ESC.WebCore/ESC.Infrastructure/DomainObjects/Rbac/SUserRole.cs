using Dapper.Extensions;
using System;

namespace ESC.Infrastructure.DomainObjects
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class SUserRole : PocoObject
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Column("UserID", "用户ID")]
        public int UserID { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        [Column("RoleID", "角色ID")]
        public int RoleID { get; set; }
    }
}