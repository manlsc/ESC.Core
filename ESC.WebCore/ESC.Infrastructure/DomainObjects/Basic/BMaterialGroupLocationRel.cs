using System;
using System.ComponentModel;
using Dapper.Extensions;

namespace ESC.Infrastructure.DomainObjects
{
    public class BMaterialGroupLocationRel : PocoObject
    {
        /// <summary>
        /// 存储单元
        /// </summary>
        public int LocationID { get; set; }

        /// <summary>
        /// 物料组
        /// </summary>
        public int MaterialGroupID { get; set; }

    }
}
