using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESC.Infrastructure
{
    /// <summary>
    /// 树节点
    /// </summary>
    public class TreeNode
    {
        public TreeNode()
        {
            children = new List<TreeNode>();
            leaf = true;
            state = "";
        }

        /// <summary>
        /// 节点的ID
        /// </summary>
        public string id { set; get; }

        /// <summary>
        /// 显示的文本
        /// </summary>
        public string text { set; get; }

        /// <summary>
        /// 路径
        /// </summary>
        public string href { set; get; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string name { set; get; }

        /// <summary>
        /// 状态
        /// open closed
        /// </summary>
        public string state { set; get; }

        /// <summary>
        /// 图标
        /// </summary>
        public string iconCls { set; get; }

        /// <summary>
        /// 子类样式
        /// </summary>
        public string chdCls { set; get; }

        /// <summary>
        /// 级别
        /// </summary>
        public int level { set; get; }

        /// <summary>
        /// 是否子节点
        /// </summary>
        public bool leaf { set; get; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<TreeNode> children { set; get; }
    }
}