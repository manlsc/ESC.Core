using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Extensions
{
    /// <summary>
    /// 分页对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Page<T>
    {
        /// <summary>
        /// 总记录
        /// </summary>
        public long total { get; set; }

        /// <summary>
        /// 查询结果
        /// </summary>
        public List<T> rows { get; set; }
    }

}
