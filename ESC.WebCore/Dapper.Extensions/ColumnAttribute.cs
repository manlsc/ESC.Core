using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Extensions
{
    /// <summary>
    /// 列映射
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        /// <summary>
        /// 数据库字段名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否禁用
        /// 默认false
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// 是否可见
        /// 默认true
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 宽度
        /// 默认160
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 是否必须
        /// 默认false
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// 外键显示名称
        /// </summary>
        public string DisplayColumn { get; set; }

        public ColumnAttribute()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">列名</param>
        public ColumnAttribute(string name)
            : this(name, name)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="title">显示名称</param>
        public ColumnAttribute(string name, string title)
            : this(name, title, true)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="title">显示名称</param>
        /// <param name="visible">是否可见</param>
        public ColumnAttribute(string name, string title, bool visible)
            : this(name, title, visible, false)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="title">显示名称</param>
        /// <param name="visible">是否可见</param>
        /// <param name="required">是否必填</param>
        public ColumnAttribute(string name, string title, bool visible, bool required)
            : this(name, title, visible, required, false, 160)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="title">显示名称</param>
        /// <param name="visible">是否可见</param>
        /// <param name="required">是否必填</param>
        /// <param name="disabled">是否禁用</param>
        /// <param name="width">页面宽度</param>
        public ColumnAttribute(string name, string title, bool visible, bool required, bool disabled, int width)
        {
            Name = name;
            Title = title;
            Visible = visible;
            Required = required;
            Disabled = disabled;
            Width = width;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="title">显示名称</param>
        /// <param name="visible">是否可见</param>
        /// <param name="required">是否必填</param>
        /// <param name="disabled">是否禁用</param>
        /// <param name="width">页面宽度</param>
        /// <param name="displaycolumn">显示列名</param>
        public ColumnAttribute(string name, string title, bool visible, bool required, bool disabled, int width, string displaycolumn)
        {
            Name = name;
            Title = title;
            Visible = visible;
            Required = required;
            Disabled = disabled;
            Width = width;
            DisplayColumn = displaycolumn;
        }
    }

}
