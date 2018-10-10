using System;
using System.Collections.Generic;
using System.Text;

namespace ESC.Infrastructure
{
    /// <summary>
    /// 下拉列表类
    /// </summary>
    public class ComboxData
    {
        public List<ComboxDataItem> items = new List<ComboxDataItem>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < items.Count; i++)
            {
                sb.Append("{\"text\":\"" + items[i].text + "\",\"value\":\"" + items[i].value + "\"}");
                if (i < items.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append("]");
            return sb.ToString();
        }
    }

    /// <summary>
    /// 下来列表项
    /// </summary>
    public class ComboxDataItem
    {
        /// <summary>
        /// 显示内容
        /// </summary>
        public string text { set; get; }

        /// <summary>
        /// 值
        /// </summary>
        public int value { set; get; }
    }
}