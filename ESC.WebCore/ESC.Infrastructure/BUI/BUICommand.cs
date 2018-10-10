using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESC.Infrastructure
{
    /// <summary>
    /// 按钮
    /// </summary>
   public class BUICommand
    {
       /// <summary>
       /// 按钮名称
       /// </summary>
        public string CommandName { get; set; }

       /// <summary>
       /// 描述
       /// </summary>
        public string Description { get; set; }

       /// <summary>
       /// 单击事件
       /// </summary>
        public string OnClick { set; get; }

       /// <summary>
       /// 图标
       /// </summary>
        public string IconClass { set; get; }
    }
}
