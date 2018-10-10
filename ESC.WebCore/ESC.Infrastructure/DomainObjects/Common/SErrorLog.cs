using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Infrastructure.DomainObjects
{
    /// <summary>
    /// 异常日志
    /// </summary>
    public class SErrorLog : PocoObject
    {
        /// <summary>
        /// 操作人
        /// </summary>
        public int UserId { set; get; }

        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string UserName { set; get; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string ReqUrl { set; get; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime ReqDate { set; get; }

        /// <summary>
        /// 请求方法
        /// </summary>
        public string HttpMethod { set; get; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string ReqParams { set; get; }

        /// <summary>
        /// 请求IP
        /// </summary>
        public string ReqIP { set; get; }

		/// <summary>
		/// 错误信息
		/// </summary>
        public string ErrMeg { set; get; }
    }
}
