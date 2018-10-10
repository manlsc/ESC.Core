using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Extensions
{
    /// <summary>
    /// 表信息
    /// </summary>
    public class TableInfo
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// 是否自增
        /// </summary>
        public bool AutoIncrement { get; set; }

        /// <summary>
        /// 序列名称
        /// </summary>
        public string SequenceName { get; set; }

        /// <summary>
        /// 主键属性
        /// </summary>
        public PropertyInfo KeyProperty { set; get; }
    }
}
