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
    public class BUnitService
    {
        protected BUnitRepository uRepository;

        public BUnitService()
        {
            uRepository = new BUnitRepository();
        }

        public string GetSearchSql()
        {
            return uRepository.GetSearchSql();
        }

        public string GetSearchSql(List<WhereItem> whereItems)
        {
            return uRepository.GetSearchSql(whereItems);
        }

        /// <summary>
        /// 检查非空验证
        /// </summary>
        /// <param name="user"></param>
        public string CheckEmpty(BUnit unit)
        {
            string msg = string.Empty;
            if (string.IsNullOrWhiteSpace(unit.UnitCode))
            {
                msg = "单位编码不能为空.";
                return msg;
            }
            if (string.IsNullOrWhiteSpace(unit.UnitName))
            {
                msg = "单位名称不能为空.";
                return msg;
            }

            return msg;
        }

        /// <summary>
        /// 基准单位是否存在
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="isAdd"></param>
        public bool IsNotExits(BUnit unit, bool isAdd)
        {
            return uRepository.IsNotExits(unit, isAdd);
        }

        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public int AddUnit(BUnit unit)
        {
            //添加单位 添加扩展
            object parentId = uRepository.Insert(unit);
            return Convert.ToInt32(parentId);
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool UpdateUnit(BUnit unit)
        {
            return uRepository.Update(unit);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool RemoveUnit(BUnit unit)
        {
            return uRepository.Delete(unit);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<BUnit> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = uRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<BUnit> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return uRepository.Pages(pageIndex, pageSize, sql, args);
        }

        /// <summary>
        /// 根据编码或名称查询
        /// </summary>
        /// <param name="codeName"></param>
        /// <returns></returns>
        public List<BUnit> GetUnitCodeName(string codeName)
        {
            return uRepository.GetUnitCodeName(codeName);
        }
    }
}
