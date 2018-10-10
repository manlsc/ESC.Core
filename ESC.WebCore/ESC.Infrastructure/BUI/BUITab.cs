using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESC.Infrastructure
{
    /// <summary>
    /// 用于tab
    /// </summary>
    public class BUITab
    {
        public BUITab()
        {
            type = "grid";
            editable = true;
        }

        /// <summary>
        /// 显示的标签名称
        /// </summary>
        public string name { set; get; }	
		
        /// <summary>
        /// 主表属性名称
        /// </summary>
        public string table { set; get; }
		
        /// <summary>
        /// 外键名称
        /// </summary>
        public string foreign { set; get; }

        /// <summary>
        /// tab类型,默认grid
        /// <remarks>grid form</remarks>
        /// </summary>
        public string type { set; get; }

        /// <summary>
        /// 是否可编辑
        /// </summary>
        public bool editable { set; get; }
    }
}