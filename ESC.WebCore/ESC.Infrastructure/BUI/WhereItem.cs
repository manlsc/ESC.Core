using System;
using System.Text;
using System.Collections.Generic;

namespace ESC.Infrastructure
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public class WhereItem
    {
        /// <summary>
        /// 字段
        /// </summary>
        public string field { set; get; }

        /// <summary>
        /// 规则
        /// </summary>
        public string condition { set; get; }

        /// <summary>
        /// 值
        /// </summary>
        public string value { set; get; }

        /// <summary>
        /// 值类型
        /// </summary>
        public string datatype { set; get; }
    }
}