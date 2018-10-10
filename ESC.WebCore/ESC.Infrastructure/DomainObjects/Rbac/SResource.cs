using Dapper.Extensions;
using System;
using System.Collections.Generic;

namespace ESC.Infrastructure.DomainObjects
{
    /// <summary>
    /// 资源
    ///  Resource 是关键字,所以添加一个前缀
    /// </summary>
    public class SResource : PocoObject
    {
        public SResource()
        {
            Operators = new List<SOperator>();
            Children = new List<SResource>();
            OrderIndex = 0;
            ParentID = 0;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [Column("ResourceName", "资源名称", false, true)]
        public string ResourceName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Column("ResourceDesc", "资源描述", false, true)]
        public string ResourceDesc { get; set; }

        /// <summary>
        /// 父资源
        /// </summary>
        [Column("ParentID", "父资源", false, true, DisplayColumn = "ParentName")]
        public int? ParentID { get; set; }

        /// <summary>
        /// 资源路径
        /// </summary>
        [Column("ResourceURL", "资源路径", false, true)]
        public string ResourceURL { set; get; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column("OrderIndex", "排序", false, true)]
        public int OrderIndex { set; get; }

        /// <summary>
        /// 父资源
        /// </summary>
        [ResultColumn("ParentName", "父资源", false, true)]
        public string ParentName { set; get; }

        /// <summary>
        /// 操作按钮
        /// </summary>
        [Ignore]
        public List<SOperator> Operators { set; get; }

        /// <summary>
        /// 子类资源
        /// </summary>
        [Ignore]
        public List<SResource> Children { set; get; }
    }
}