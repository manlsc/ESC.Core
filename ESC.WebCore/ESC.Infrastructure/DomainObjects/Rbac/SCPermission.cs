using Dapper.Extensions;

namespace ESC.Infrastructure.DomainObjects
{
    /// <summary>
    /// 列权限
    /// </summary>
    public class SCPermission : PocoObject
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [Column("RoleID", "用户编码", true, false)]
        public int RoleID { set; get; }

        /// <summary>
        /// 列ID
        /// </summary>
        [Column("ColumnID", "用户编码", true, false)]
        public int ColumnID { set; get; }

        /// <summary>
        /// 表名称
        /// </summary>
        [Column("TableName", "表名称", false, false, false, 160)]
        public string TableName { set; get; }

        /// <summary>
        /// 表描述
        /// </summary>
        [Column("TableDesc", "表描述", false, false, false, 160)]
        public string TableDesc { set; get; }

        /// <summary>
        /// 对应的数据库列
        /// </summary>
        [Column("ColumnName", "数据库列", false, false, false, 160)]
        public string ColumnName { set; get; }

        /// <summary>
        /// 对应的列描述
        /// </summary>
        [Column("Title", "列描述", false, false, false, 160)]
        public string Title { set; get; }

        /// <summary>
        /// 是否必需
        /// </summary>
        [Column("Required", "是否必需", false, false, false, 100, "RequiredName")]
        public int Required { set; get; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        [Column("Visible", "是否显示", false, false, false, 100, "VisibleName")]
        public int Visible { set; get; }

        /// <summary>
        /// 列字段类型
        /// </summary>
        [Column("ColumnType", "字段类型", false, false, false, 160)]
        public string ColumnType { set; get; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [Column("OrderIndex", "显示顺序", false, false, false, 160)]
        public int OrderIndex { set; get; }

        /// <summary>
        /// 显示列
        /// </summary>
        [Column("DisplayColumn", "显示列", false, false, false, 160)]
        public string DisplayColumn { set; get; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        [Column("Disabled", "是否禁用", false, false, false, 160, "DisabledName")]
        public int Disabled { set; get; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        [Column("Width", "宽度", false, false, false, 160)]
        public int Width { set; get; }

        /// <summary>
        /// 是否必填
        /// </summary>
        [ResultColumn("RequiredName", "是否必填", true, true)]
        public string RequiredName { set; get; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        [ResultColumn("VisibleName", "是否显示", true, true)]
        public string VisibleName { set; get; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        [ResultColumn("DisabledName", "是否禁用", true, true)]
        public string DisabledName { set; get; }
    }
}
