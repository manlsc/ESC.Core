using Dapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESC.Infrastructure.DomainObjects
{
    /// <summary>
    /// 列
    /// Column是关键字,所以添加前缀
    /// </summary>
    public class SColumn : PocoObject
    {
        public SColumn()
        {
            Disabled = 0;
        }

        /// <summary>
        /// 表名称
        /// </summary>
        [Column("TableName", "表名称", true, false, true, 160)]
        public string TableName { set; get; }

        /// <summary>
        /// 表描述
        /// </summary>
        [Column("TableDesc", "表描述", false, false, false, 160)]
        public string TableDesc { set; get; }

        /// <summary>
        /// 对应的数据库列
        /// </summary>
        [Column("ColumnName", "数据库列", true, false, true, 160)]
        public string ColumnName { set; get; }

        /// <summary>
        /// 对应的列描述
        /// </summary>
        [Column("Title", "列描述", true, true, false, 160)]
        public string Title { set; get; }

        /// <summary>
        /// 是否必需
        /// </summary>
        [Column("Required", "是否必需", true, false, false, 100, "RequiredName")]
        public int Required { set; get; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        [Column("Visible", "是否显示", true, false, false, 100, "VisibleName")]
        public int Visible { set; get; }

        /// <summary>
        /// 列字段类型
        /// </summary>
        [Column("ColumnType", "字段类型", false, false, false, 160)]
        public string ColumnType { set; get; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [Column("OrderIndex", "显示顺序", true, false, false, 160)]
        public int OrderIndex { set; get; }

        /// <summary>
        /// 显示列
        /// </summary>
        [Column("DisplayColumn", "显示列", false, false, false, 160)]
        public string DisplayColumn { set; get; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [Column("Width", "宽度", true, false, false, 160)]
        public int Width { set; get; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        [Column("Disabled", "是否禁用", true, false, false, 160, "DisabledName")]
        public int Disabled { set; get; }
      
        /// <summary>
        /// 是否必填
        /// </summary>
        [ResultColumn("RequiredName", "是否必填", false)]
        public string RequiredName { set; get; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        [ResultColumn("VisibleName", "是否显示", false)]
        public string VisibleName { set; get; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        [ResultColumn("DisabledName", "是否禁用", false)]
        public string DisabledName { set; get; }
    }
}
