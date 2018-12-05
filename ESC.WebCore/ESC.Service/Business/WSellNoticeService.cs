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
    /// <summary>
    /// 销售出库通知单 +
    /// </summary>
    public class WSellNoticeService
    {
        protected WSellNoticeRepository pnRepository;
        protected WSellNoticeLineRepository pnlRepository;
        protected SNumberRuleRepository nuRepository;

        public WSellNoticeService()
        {
            pnRepository = new WSellNoticeRepository();
            pnlRepository = new WSellNoticeLineRepository(pnRepository.DbCondext);
            nuRepository = new SNumberRuleRepository(pnRepository.DbCondext);
        }

        /// <summary>
        /// 获取主表查询语句
        /// </summary>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public string GetNoticeSearchSql(List<WhereItem> whereItems)
        {
            return pnRepository.GetSearchSql(whereItems);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public WSellNotice GetWSellNoticeById(int Id)
        {
            return pnRepository.GetWSellNoticeById(Id);
        }

        #region 查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WSellNotice> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = pnRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WSellNotice> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return pnRepository.Pages(pageIndex, pageSize, sql, args);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WSellNoticeLine> LinePageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = pnlRepository.GetSearchSql(whereItems);
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public Page<WSellNoticeLine> LinePageSearch(long pageIndex, long pageSize, int parentId)
        {
            //根据主表查询
            WhereItem item = new WhereItem()
            {
                condition = "=",
                datatype = "int",
                field = "ParentID",
                value = parentId.ToString()
            };
            string strSql = pnlRepository.GetSearchSql(new List<WhereItem>() { item });
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WSellNoticeLine> LinePageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return pnlRepository.Pages(pageIndex, pageSize, sql, args);
        }

        #endregion

        /// <summary>
        /// 插入新其他出库
        /// </summary>
        /// <param name="outNotice"></param>
        /// <returns></returns>
        public ResultData<string> AddSellNotice(WSellNotice outNotice)
        {
            ResultData<string> rData = CheckValid(outNotice);
            if (rData.status != 0)
            {
                return rData;
            }
            DatabaseContext dbContext = pnRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();

                outNotice.CreateDate = DateTime.Now;
                outNotice.NoticeStatus = NoticeStatusEnum.New;
                outNotice.OutNoticeCode = nuRepository.GetNextNumber("XSCT");
                pnRepository.Insert(outNotice);

                foreach (var line in outNotice.Lines)
                {                    
                    line.ParentID = outNotice.ID;
                    line.CreateBy = outNotice.CreateBy;
                    line.CreateDate = DateTime.Now;
                    pnlRepository.Insert(line);
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

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="outNotice"></param>
        /// <returns></returns>
        public int RemoveSellNotice(WSellNotice outNotice)
        {
            int result = pnRepository.RemovePruchaseNoticeByStatus(outNotice.ID, NoticeStatusEnum.New);
            if (result > 0)
            {
                pnlRepository.RemoveLinesByParentId(outNotice.ID);
            }
            return result;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="outNotice"></param>
        /// <returns></returns>
        public ResultData<string> UpdateSellNotice(WSellNotice outNotice)
        {
            ResultData<string> rData = CheckValid(outNotice);
            if (rData.status != 0)
            {
                return rData;
            }
            if (outNotice.NoticeStatus != NoticeStatusEnum.New)
            {
                rData.status = -1;
                rData.message = "单据已经审核,不能编辑.";
                return rData;
            }

            DatabaseContext db = pnRepository.DbCondext;
            try
            {
                db.BeginTransaction();

                pnRepository.Update(outNotice);
                foreach (var line in outNotice.Lines)
                {
                    switch (line.CURD)
                    {
                        case CurdEnum.Add:
                            line.ParentID = outNotice.ID;
                            line.CreateBy = outNotice.CreateBy;
                            line.CreateDate = DateTime.Now;
                            pnlRepository.Insert(line);
                            break;
                        case CurdEnum.Delete:
                            pnlRepository.Delete(line);
                            break;
                        case CurdEnum.Update:
                            line.UpdateBy = outNotice.UpdateBy;
                            line.UpdateDate = DateTime.Now;
                            pnlRepository.Update(line);
                            break;
                    }
                }

                db.CompleteTransaction();
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw ex;
            }
            return rData;
        }


        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="outNotice"></param>
        /// <returns></returns>
        private ResultData<string> CheckValid(WSellNotice outNotice)
        {
            ResultData<string> rt = new ResultData<string>();
            if (outNotice.WarehouseID < 0)
            {
                rt.status = -1;
                rt.message = "仓库不能为空";
                return rt;
            }          

            foreach (var item in outNotice.Lines)
            {
                //删除行,不验证
                if (item.CURD == CurdEnum.Delete)
                {
                    continue;
                }
                if (item.MaterialID < 1)
                {
                    rt.status = -1;
                    rt.message = BuilderErrorMessage(item, "物料不能为空");
                    break;
                }
                else if (item.OutCount <= 0)
                {
                    rt.status = -1;
                    rt.message = BuilderErrorMessage(item, "出库数量必须大于0");
                    break;
                }
            }

            return rt;
        }

        /// <summary>
        /// 构建库存不足提示
        /// </summary>
        /// <param name="line"></param>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        private string BuilderErrorMessage(WSellNoticeLine line, string msg)
        {
            StringBuilder sb = new StringBuilder(msg);
            sb.AppendLine("，物料：" + line.MaterialCode);
            if (!string.IsNullOrWhiteSpace(line.Batch))
            {
                sb.AppendLine("，批次：" + line.Batch);
            }
            if (!string.IsNullOrWhiteSpace(line.OwnerCode))
            {
                sb.AppendLine("，货主：" + line.OwnerCode);
            }
            return sb.ToString();
        }
    }
}
