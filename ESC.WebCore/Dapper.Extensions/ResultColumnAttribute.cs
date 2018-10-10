using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Extensions
{
    /// <summary>
    /// 视图列
    /// 仅用于查询,增删改忽略此属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ResultColumnAttribute : ColumnAttribute
    {
        public ResultColumnAttribute()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">列名</param>
        public ResultColumnAttribute(string name)
            : base(name, name, false)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="title">显示名称</param>
        public ResultColumnAttribute(string name, string title)
            : base(name, title)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="title">显示名称</param>
        /// <param name="visible">是否可见</param>
        public ResultColumnAttribute(string name, string title, bool visible)
            : base(name, title, visible)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="title">显示名称</param>
        /// <param name="visible">是否可见</param>
        /// <param name="required">是否必填</param>
        public ResultColumnAttribute(string name, string title, bool visible, bool required)
            : base(name, title, visible, required)
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
        public ResultColumnAttribute(string name, string title, bool visible, bool required, bool disabled, int width)
            : base(name, title, visible, required, disabled, width)
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
        /// <param name="displaycolumn">显示列名</param>
        public ResultColumnAttribute(string name, string title, bool visible, bool required, bool disabled, int width, string displaycolumn)
            : base(name, title, visible, required, disabled, width, displaycolumn)
        {
        }
    }
}
