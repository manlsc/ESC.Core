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
    public class WPurchaseNoticeService
    {
        protected WPurchaseNoticeRepository pnRepository;
        protected WPurchaseNoticeLineRepository pnlRepository;
        protected SNumberRuleRepository nuRepository;

        public WPurchaseNoticeService()
        {
            pnRepository = new WPurchaseNoticeRepository();
            pnlRepository = new WPurchaseNoticeLineRepository(pnRepository.DbCondext);
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
        public WPurchaseNotice GetWPurchaseNoticeById(int Id)
        {
            return pnRepository.GetWPurchaseNoticeById(Id);
        }

        #region 查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WPurchaseNotice> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
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
        public Page<WPurchaseNotice> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
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
        public Page<WPurchaseNoticeLine> LinePageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
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
        public Page<WPurchaseNoticeLine> LinePageSearch(long pageIndex, long pageSize, int parentId)
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
        public Page<WPurchaseNoticeLine> LinePageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return pnlRepository.Pages(pageIndex, pageSize, sql, args);
        }

        #endregion

        /// <summary>
        /// 插入新其他出库
        /// </summary>
        /// <param name="inNotice"></param>
        /// <returns></returns>
        public ResultData<string> AddPurchaseNotice(WPurchaseNotice inNotice)
        {
            ResultData<string> rData = CheckValid(inNotice);
            if (rData.status != 0)
            {
                return rData;
            }
            DatabaseContext dbContext = pnRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();

                inNotice.CreateDate = DateTime.Now;
                inNotice.NoticeStatus = NoticeStatusEnum.New;
                inNotice.InNoticeCode = nuRepository.GetNextNumber("CGRT");
                pnRepository.Insert(inNotice);

                foreach (var line in inNotice.Lines)
                {
                    line.ParentID = inNotice.ID;
                    line.CreateBy = inNotice.CreateBy;
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
        /// <param name="purchaseNotice"></param>
        /// <returns></returns>
        public int RemovePurchaseNotice(WPurchaseNotice purchaseNotice)
        {
            int result = pnRepository.RemovePruchaseNoticeByStatus(purchaseNotice.ID, NoticeStatusEnum.New);
            if (result > 0)
            {
                pnlRepository.RemoveLinesByParentId(purchaseNotice.ID);
            }
            return result;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="purchaseNotice"></param>
        /// <returns></returns>
        public ResultData<string> UpdatePurchaseNotice(WPurchaseNotice purchaseNotice)
        {
            ResultData<string> rData = CheckValid(purchaseNotice);
            if (rData.status != 0)
            {
                return rData;
            }
            if (purchaseNotice.NoticeStatus != NoticeStatusEnum.New)
            {
                rData.status = -1;
                rData.message = "单据已经审核,不能编辑.";
                return rData;
            }

            DatabaseContext db = pnRepository.DbCondext;
            try
            {
                db.BeginTransaction();

                pnRepository.Update(purchaseNotice);
                foreach (var line in purchaseNotice.Lines)
                {
                    switch (line.CURD)
                    {
                        case CurdEnum.Add:
                            line.ParentID = purchaseNotice.ID;
                            line.CreateBy = purchaseNotice.CreateBy;
                            line.CreateDate = DateTime.Now;
                            pnlRepository.Insert(line);
                            break;
                        case CurdEnum.Delete:
                            pnlRepository.Delete(line);
                            break;
                        case CurdEnum.Update:
                            line.UpdateBy = purchaseNotice.UpdateBy;
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
        /// <param name="purchaseNotice"></param>
        /// <returns></returns>
        private ResultData<string> CheckValid(WPurchaseNotice purchaseNotice)
        {
            ResultData<string> rt = new ResultData<string>();
            if (purchaseNotice.WarehouseID < 0)
            {
                rt.status = -1;
                rt.message = "仓库不能为空";
                return rt;
            }
          

            foreach (var item in purchaseNotice.Lines)
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
                else if (item.InCount <= 0)
                {
                    rt.status = -1;
                    rt.message = BuilderErrorMessage(item, "入库数量必须大于0");
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
        private string BuilderErrorMessage(WPurchaseNoticeLine line, string msg)
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
