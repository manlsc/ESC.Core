using System;
using System.Collections.Generic;

namespace ESC.Infrastructure
{
    /// <summary>
    /// 初始化数据
    /// </summary>
    public class InitData
    {
        public InitData() {
            Columns = new List<BUIColumn>();
            Commands = new List<BUICommand>();
            Tabs = new List<BUITab>();
            PrimaryKey = "ID";
        }

        /// <summary>
        /// 列
        /// </summary>
        public List<BUIColumn> Columns { set; get; }

        /// <summary>
        /// 按钮
        /// </summary>
        public List<BUICommand> Commands { set; get; }

        /// <summary>
        /// 标签
        /// </summary>
        public List<BUITab> Tabs { set; get; }

        /// <summary>
        /// 主表ID
        /// </summary>
        public string PrimaryKey { set; get; }
    }
}