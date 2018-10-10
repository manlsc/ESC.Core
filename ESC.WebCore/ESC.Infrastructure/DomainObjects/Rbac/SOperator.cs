using Dapper.Extensions;
using System;

namespace ESC.Infrastructure.DomainObjects
{
    /// <summary>
    /// 按钮表
    /// </summary>
    public class SOperator : PocoObject
    {
        public SOperator()
        {
            OrderIndex = 0;
        }

        /// <summary>
        /// 资源ID
        /// </summary>
        [Column("ResourceID", "资源", false, false)]
        public int ResourceID { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        [Column("OperatorName", "操作名称", true, true)]
        public string OperatorName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Column("OperatorDesc", "描述", false, true)]
        public string OperatorDesc { get; set; }

        /// <summary>
        /// 绑定事件
        /// </summary>
        [Column("OnClick", "绑定事件", true, true, Width = 100)]
        public string OnClick { set; get; }

        /// <summary>
        /// 图标
        /// </summary>
        [Column("IconClass", "图标", true, false)]
        public string IconClass { set; get; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column("OrderIndex", "排序", true, true, Width = 80)]
        public int OrderIndex { set; get; }
    }
}