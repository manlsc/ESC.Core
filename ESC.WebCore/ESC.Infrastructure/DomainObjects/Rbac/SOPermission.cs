using Dapper.Extensions;
using System;

namespace ESC.Infrastructure.DomainObjects
{
    /// <summary>
    /// 按钮权限
    /// </summary>
    public class SOPermission : PocoObject
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        [Column("ResourceID", "资源", true, true)]
        public int ResourceID { get; set; }

        /// <summary>
        /// 操作ID
        /// </summary>
        [Column("OperatorID", "操作", true, true)]
        public int OperatorID { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [Column("RoleID", "角色", true, true)]
        public int RoleID { set; get; }
    }
}