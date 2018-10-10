using System.Collections.Generic;

namespace ESC.Infrastructure
{
    /// <summary>
    /// bui列
    /// </summary>
    public class BUIColumn
    {
        /// <summary>
        /// 列标题文本
        /// </summary>
        public string title { set; get; }

        /// <summary>
        /// 列字段名称
        /// </summary>
        public string dataIndex { set; get; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool disabled { set; get; }

        /// <summary>
        ///  宽度
        /// </summary>
        public int width { set; get; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool visible { set; get; }

        /// <summary>
        /// 下拉列表{text:名称,value:name}
        /// </summary>
        public List<ComboxDataItem> combox { set; get; }

        /// <summary>
        /// 是否必须
        /// </summary>
        public bool required { set; get; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string datatype { set; get; }

        /// <summary>
        /// 外键显示字段
        /// </summary>
        public string displayfield { set; get;}

        /// <summary>
        /// 绑定的外键字段 searchbox
        /// </summary>
        public ForeignObject foreign { set; get; }
    }
}