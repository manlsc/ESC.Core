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
    /// 列操作 +
    /// </summary>
    public class SColumnService
    {

        protected SColumnRepository cRepository;

        public SColumnService()
        {
            cRepository = new SColumnRepository();
        }

        /// <summary>
        /// 插入新列
        /// </summary>
        /// <param name="scolumn"></param>
        /// <returns></returns>
        public int AddColumn(SColumn scolumn)
        {
            object result = cRepository.Insert(scolumn);
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// 删除列
        /// </summary>
        /// <param name="scolumns"></param>
        /// <returns></returns>
        public int RemoveColumns(List<SColumn> scolumns)
        {
            DatabaseContext db = cRepository.DbCondext;
            try
            {
                db.BeginTransaction();
                foreach (SColumn u in scolumns)
                {
                    RemoveColumn(u);
                }
                db.CompleteTransaction();
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw ex;
            }
            return scolumns.Count;

        }

        /// <summary>
        /// 删除列
        /// </summary>
        /// <param name="scolumn"></param>
        /// <returns></returns>
        public bool RemoveColumn(SColumn scolumn)
        {
            return cRepository.Delete(scolumn);
        }

        /// <summary>
        /// 根据Id删除列
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool RemoveColumn(int Id)
        {
            return cRepository.Delete(Id);
        }

        /// <summary>
        /// 更新列
        /// </summary>
        /// <param name="scolumn"></param>
        /// <returns></returns>
        public bool UpdateColumn(SColumn scolumn)
        {
            return cRepository.Update(scolumn);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<SColumn> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = cRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<SColumn> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return cRepository.Pages(pageIndex, pageSize, sql, args);
        }

        /// <summary>
        /// 获取可见列
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<SColumn> GetVisibleColumnsByTable(string tableName)
        {
            return cRepository.GetVisibleColumnsByTable(tableName);
        }
    }
}
