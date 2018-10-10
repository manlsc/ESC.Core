using System;
using System.ComponentModel;
using Dapper.Extensions;

namespace ESC.Infrastructure.DomainObjects
{
    public class BLocationUserRel : PocoObject
    {
        /// <summary>
        /// 存储单元
        /// </summary>
        public int LocationID { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public int UserID { get; set; }

    }
}
