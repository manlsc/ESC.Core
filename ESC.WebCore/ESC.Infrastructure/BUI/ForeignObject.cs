using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESC.Infrastructure
{
    /// <summary>
    /// 外键对象
    /// </summary>
    public class ForeignObject
    {
        public ForeignObject() {
            returnDic = new Dictionary<string, string>();
        }

        /// <summary>
        /// 当前字段
        /// </summary>
        public string dataindex { set; get; }

        /// <summary>
        /// 字段所属表
        /// </summary>
        public string table { set; get; }

        /// <summary>
        /// 外键
        /// </summary>
        public string foreignKey { set; get; }

        /// <summary>
        /// 外键值
        /// </summary>
        public string foreignValue { set; get; }

        /// <summary>
        /// 返回其他字段
        /// key：原表字段  value：外键表对应字段  赋值：key=value
        /// </summary>
        public Dictionary<string, string> returnDic { set; get; }
    }
}
