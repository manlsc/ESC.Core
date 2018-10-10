using ESC.Core;
using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Repository;
using Dapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Service
{
    /// <summary>
    /// 日志 +
    /// </summary>
    public class LogService
    {
        protected SBehaviorLogRepository blRepository;
        protected SErrorLogRepository elRepository;

        public LogService()
        {
            blRepository = new SBehaviorLogRepository();
            elRepository = new SErrorLogRepository(blRepository.DbCondext);
        }

        /// <summary>
        /// 添加错误日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public int AddErrorLog(SErrorLog log)
        {
            elRepository.Insert(log);
            return log.ID;
        }

        /// <summary>
        /// 添加行为日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public int AddBehaviorLog(SBehaviorLog log)
        {
            blRepository.Insert(log);
            return log.ID;
        }

        /// <summary>
        /// 获取错误日志
        /// </summary>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<SErrorLog> GetErrorLog(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = elRepository.GetSearchSql(whereItems);
            return elRepository.Pages(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 获取行为日志
        /// </summary>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<SBehaviorLog> GetBehaviorLog(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = blRepository.GetSearchSql(whereItems);
            return blRepository.Pages(pageIndex, pageSize, strSql);
        }
    }
}
