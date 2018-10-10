using Dapper.Extensions;
using System;
using System.Collections.Generic;

namespace ESC.Infrastructure.DomainObjects
{
    /// <summary>
    /// 枚举类型
    /// </summary>
    public class SCommonEnum : PocoObject
    {      
        /// <summary>
        /// 枚举类型
        /// </summary>
        [Column("EnumType", "枚举类型", true, true)]
        public string EnumType { set; get; }

        /// <summary>
        /// 字段名称
        /// </summary>
        [Column("EnumName", "字段名称", true, true)]
        public string EnumName { set; get; }

        /// <summary>
        /// 枚举值
        /// </summary>
        [Column("EnumField", "枚举值", true, true)]
        public string EnumField { set; get; }

        /// <summary>
        /// 描述
        /// </summary>
        [Column("EnumDesc", "描述", true, true)]
        public string EnumDesc { set; get; }

    }
}
