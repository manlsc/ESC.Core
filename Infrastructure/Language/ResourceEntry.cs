using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Core
{
    /// <summary>
    /// 语言实体
    /// </summary>
    public class ResourceEntry
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 翻译字典
        /// </summary>
        public Dictionary<string, string> Values { get; set; }

        public string this[string culture]
        {
            get
            {
                culture = culture.ToLower().Replace('-','_');
                if(!this.Values.ContainsKey(culture) || string.IsNullOrEmpty(this.Values[culture]))
                {
                    //默认返回中文
                    return this.Values["zh_CN"];
                }
                return this.Values[culture];
            }
        }     
    }
}
