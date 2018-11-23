using ESC.Core;
using ESC.Core.Helper;
using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Repository;
using Dapper.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESC.Service
{
    /// <summary>
    /// 编码规则  +
    /// </summary>
    public class SNumberRuleService
    {

        protected SNumberRuleRepository repository;

        public SNumberRuleService()
        {
            repository = new SNumberRuleRepository();
        }

        /// <summary>
        /// 自动生成单号
        /// </summary>
        /// <param name="businessType">业务类型</param>
        public string GetNextNumber(string businessType)
        {
            return repository.GetNextNumber(businessType);
        }

        /// <summary>
        /// 更新业务单号生成
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool UpdateSNumberRule(SNumberRule num)
        {
            return repository.UpdateSNumberRule(num);
        }

        /// <summary>
        /// 添加业务单号生成
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public int AddSNumberRule(SNumberRule num)
        {
            return repository.AddSNumberRule(num);
        }

        /// <summary>
        /// 删除业务单号生成
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool DeleteSNumberRule(SNumberRule num)
        {
            int curValue = repository.GetCurrentNumber(num.BusinessType);
            if (curValue == num.StartSeq)
            {
                return repository.DeleteSNumberRule(num);
            }
            return false;
        }

        /// <summary>
        /// 用户是否存在
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isAdd"></param>
        public bool IsNotExits(SNumberRule num, bool isAdd)
        {
            return repository.IsNotExits(num, isAdd);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<SNumberRule> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = repository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<SNumberRule> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return repository.Pages(pageIndex, pageSize, sql, args);
        }
    }
}
