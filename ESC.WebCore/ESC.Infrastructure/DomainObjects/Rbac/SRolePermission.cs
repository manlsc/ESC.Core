using Dapper.Extensions;
using System;

namespace ESC.Infrastructure.DomainObjects
{
   /// <summary>
    /// 角色权限
   /// </summary>
    public class SRolePermission : PocoObject
    {
        /// <summary>
        /// RoleID
        /// </summary>
         [Column("RoleID", "角色ID")]
        public int RoleID { get; set; }

        /// <summary>
        /// ResourceID
        /// </summary>
        [Column("ResourceID", "资源ID")]
        public int ResourceID { get; set; }
    }
}