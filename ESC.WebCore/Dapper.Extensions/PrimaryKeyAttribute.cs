using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Extensions
{
    /// <summary>
    /// 主键
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PrimaryKeyAttribute : Attribute
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public string SequenceName { get; set; }

        /// <summary>
        /// 自增
        /// </summary>
        public bool AutoIncrement { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="primaryKey">主键</param>
        public PrimaryKeyAttribute(string primaryKey)
        {
            Value = primaryKey;
            AutoIncrement = true;
        }
    }
}
