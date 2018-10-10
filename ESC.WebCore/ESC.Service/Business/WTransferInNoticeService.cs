using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Enums;
using ESC.Infrastructure.Repository;
using Dapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Service
{
   public class WTransferInNoticeService
    {
        WTransferInNoticeRepository tinRepository;
        WTransferInNoticeLineRepository tinllRepository;
        SNumberRuleRepository nuRepository;

        public WTransferInNoticeService()
        {
            tinRepository = new WTransferInNoticeRepository();
            tinllRepository = new WTransferInNoticeLineRepository(tinRepository.DbCondext);
            nuRepository = new SNumberRuleRepository(tinRepository.DbCondext);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public WTransferInNotice GetWTransferInNoticeById(int Id)
        {
            return tinRepository.GetWTransferInNoticeById(Id);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WTransferInNotice> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = tinRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WTransferInNotice> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return tinRepository.Pages(pageIndex, pageSize, sql, args);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WTransferInNoticeLine> LinePageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = tinllRepository.GetSearchSql(whereItems);
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public Page<WTransferInNoticeLine> LinePageSearch(long pageIndex, long pageSize, int parentId)
        {
            //根据主表查询
            WhereItem item = new WhereItem()
            {
                condition = "=",
                datatype = "int",
                field = "ParentID",
                value = parentId.ToString()
            };
            string strSql = tinllRepository.GetSearchSql(new List<WhereItem>() { item });
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WTransferInNoticeLine> LinePageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return tinllRepository.Pages(pageIndex, pageSize, sql, args);
        }

        /// <summary>
        /// 插入新其他出库
        /// </summary>
        /// <param name="inNotice"></param>
        /// <returns></returns>
        public ResultData<string> AddTransferInNotice(WTransferInNotice inNotice)
        {
            ResultData<string> rData = new ResultData<string>();
            DatabaseContext dbContext = tinRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();

                inNotice.CreateDate = DateTime.Now;
                inNotice.NoticeStatus = NoticeStatusEnum.New;
                inNotice.InNoticeCode = nuRepository.GetNextNumber("DBRT");
                tinRepository.Insert(inNotice);

                foreach (var line in inNotice.Lines)
                {
                    line.ParentID = inNotice.ID;
                    line.CreateBy = inNotice.CreateBy;
                    line.CreateDate = DateTime.Now;
                    tinllRepository.Insert(line);
                }

                dbContext.CompleteTransaction();
            }
            catch (Exception ex)
            {
                dbContext.AbortTransaction();
                throw ex;
            }
            return rData;
        }
    }
}
