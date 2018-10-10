using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Extensions
{

    /// <summary>
    /// 表映射
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute : Attribute
    {
        public string Value { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName">表名</param>
        public TableNameAttribute(string tableName)
        {
            Value = tableName;
        }
    }
}
