using Dapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Infrastructure.DomainObjects
{
    /// <summary>
    /// 行为日志
    /// </summary>
    public class SBehaviorLog : PocoObject
    {
        /// <summary>
        /// 操作人
        /// </summary>
        [Column("UserId", "操作人ID", false)]
        public int UserId { set; get; }

        /// <summary>
        /// 操作人姓名
        /// </summary>
        [Column("UserName", "操作人", true, Width = 120)]
        public string UserName { set; get; }

        /// <summary>
        /// 请求地址
        /// </summary>
        [Column("ReqUrl", "请求地址", true, Width = 200)]
        public string ReqUrl { set; get; }

        /// <summary>
        /// 请求时间
        /// </summary>
        [Column("ReqDate", "请求时间", true, Width = 160)]
        public DateTime ReqDate { set; get; }

        /// <summary>
        /// 请求方法
        /// </summary>
        [Column("HttpMethod", "请求方法", true, Width = 120)]
        public string HttpMethod { set; get; }

        /// <summary>
        /// 请求参数
        /// </summary>
        [Column("ReqParams", "请求参数", true, Width = 700)]
        public string ReqParams { set; get; }

        /// <summary>
        /// 请求IP
        /// </summary>
        [Column("ReqIP", "IP地址", true, Width = 160)]
        public string ReqIP { set; get; }
    }
}
