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
    /// 采购入库 +
    /// </summary>
    public class WPurchaseService
    {
        WPurchaseRepository pRepository;
        WPurchaseLineRepository plRepository;
        WPurchaseNoticeLineRepository pnlRepository;
        WPurchaseNoticeRepository pnRepository;
        SNumberRuleRepository nuRepository;

        public WPurchaseService()
        {
            pRepository = new WPurchaseRepository();
            plRepository = new WPurchaseLineRepository(pRepository.DbCondext);
            nuRepository = new SNumberRuleRepository(pRepository.DbCondext);
            pnlRepository = new WPurchaseNoticeLineRepository(plRepository.DbCondext);
            pnRepository = new WPurchaseNoticeRepository(plRepository.DbCondext);
        }

        #region 查询

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public WPurchase GetWPurchaseById(int Id)
        {
            return pRepository.GetWPurchaseById(Id);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WPurchase> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = pRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WPurchase> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return pRepository.Pages(pageIndex, pageSize, sql, args);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WPurchaseLine> LinePageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = plRepository.GetSearchSql(whereItems);
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public Page<WPurchaseLine> LinePageSearch(long pageIndex, long pageSize, int parentId)
        {
            //根据主表查询
            WhereItem item = new WhereItem()
            {
                condition = "=",
                datatype = "int",
                field = "ParentID",
                value = parentId.ToString()
            };
            string strSql = plRepository.GetSearchSql(new List<WhereItem>() { item });
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WPurchaseLine> LinePageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return plRepository.Pages(pageIndex, pageSize, sql, args);
        }

        #endregion

        #region 增删改

        /// <summary>
        /// 插入新采购入库
        /// </summary>
        /// <param name="purchase"></param>
        /// <returns></returns>
        public ResultData<string> AddPurchase(WPurchase purchase)
        {
            ResultData<string> rData = CheckValid(purchase);
            if (rData.status != 0)
            {
                return rData;
            }

            DatabaseContext dbContext = pRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();

                purchase.CreateDate = DateTime.Now;
                purchase.StockStatus = StockStatusEnum.New;
                purchase.PurchaseCode = nuRepository.GetNextNumber("CGRK");
                pRepository.Insert(purchase);

                foreach (var line in purchase.Lines)
                {
                    line.ParentID = purchase.ID;
                    line.CreateBy = purchase.CreateBy;
                    line.CreateDate = DateTime.Now;
                    line.Batch = string.IsNullOrEmpty(line.Batch) ? "" : line.Batch;
                    plRepository.Insert(line);
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
        /// 根据入库通知添加入库
        /// </summary>
        /// <param name="inNotice"></param>
        /// <returns></returns>
        public ResultData<string> AddPurchase(WPurchaseNotice inNotice, int createBy)
        {
            ResultData<string> rData = new ResultData<string>();
            if (inNotice.NoticeStatus == NoticeStatusEnum.Complete)
            {
                rData.status = -1;
                rData.message = "单据已完成不能下推.";
                return rData;
            }

            //查询明细
            if (inNotice.Lines.Count < 1)
            {
                inNotice.Lines = pnlRepository.GetLinesByParentId(inNotice.ID);
            }

            //克隆主表
            WPurchase Purchase = CloneInNotice(inNotice);
            Purchase.CreateBy = createBy;
            //克隆子表
            foreach (var item in inNotice.Lines)
            {
                WPurchaseLine line = CloneInNoticeLine(item);
                if (line != null)
                {
                    Purchase.Lines.Add(line);
                }
            }

            if (Purchase.Lines.Count < 1)
            {
                rData.status = -1;
                rData.message = "单据已经全部下推.";
                return rData;
            }

            DatabaseContext dbContext = pRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();

                //添加入库单
                Purchase.CreateDate = DateTime.Now;
                Purchase.StockStatus = StockStatusEnum.New;
                Purchase.PurchaseCode = nuRepository.GetNextNumber("CGRK");
                pRepository.Insert(Purchase);

                foreach (var line in Purchase.Lines)
                {
                    //插入入库明细
                    line.ParentID = Purchase.ID;
                    line.CreateBy = Purchase.CreateBy;
                    line.CreateDate = DateTime.Now;
                    plRepository.Insert(line);

                    //更新通知单 添加下推
                    decimal rt = pnlRepository.AddDownCount(line.InCount, line.SourceLineID);
                    if (rt < 0)
                    {
                        dbContext.AbortTransaction();
                        rData.status = -1;
                        rData.message = BuilderNoticeLessMessage(line);
                        return rData;
                    }
                }

                //将插入主键赋值给返回值
                rData.result = Purchase.ID.ToString();

                //更新通知单状态
                if (Purchase.SourceID > 0)
                {
                    pnRepository.UpdateNoticeStatus(NoticeStatusEnum.Executing, Purchase.SourceID);
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
        /// 更新
        /// </summary>
        /// <param name="purchase"></param>
        /// <returns></returns>
        public ResultData<string> UpdatePurchase(WPurchase purchase)
        {
            ResultData<string> rData = CheckValid(purchase);
            if (rData.status != 0)
            {
                return rData;
            }
            if (purchase.StockStatus != StockStatusEnum.New)
            {
                rData.status = -1;
                rData.message = "单据已经审核,不能编辑.";
                return rData;
            }

            DatabaseContext db = pnRepository.DbCondext;
            try
            {
                db.BeginTransaction();

                foreach (var line in purchase.Lines)
                {
                    switch (line.CURD)
                    {
                        case CurdEnum.Add:
                            if (line.SourceLineID > 0)
                            {
                                //添加下推
                                decimal result = pnlRepository.AddDownCount(line.InCount, line.SourceLineID);
                                if (result < 0)
                                {
                                    db.AbortTransaction();
                                    rData.status = -1;
                                    rData.message = BuilderErrorMessage(line, "入库数量大于通知单数量.");
                                    return rData;
                                }
                            }
                            line.ParentID = purchase.ID;
                            line.CreateBy = purchase.CreateBy;
                            line.CreateDate = DateTime.Now;
                            line.InPutCount = line.InCount;
                            line.Batch = string.IsNullOrEmpty(line.Batch) ? "" : line.Batch;
                            plRepository.Insert(line);

                            break;
                        case CurdEnum.Delete:
                            plRepository.Delete(line);
                            if (line.SourceLineID > 0)
                            {
                                //删除下推
                                pnlRepository.RemoveDownCount(line.InCount, line.SourceLineID);
                            }
                            break;
                        case CurdEnum.Update:
                            if (line.SourceLineID > 0)
                            {
                                decimal result = pnlRepository.UpdateDownCount(line.SourceLineID, line.InCount, line.InPutCount);
                                if (result < 0)
                                {
                                    db.AbortTransaction();
                                    rData.status = -1;
                                    rData.message = BuilderErrorMessage(line, "入库数量大于通知单数量.");
                                    return rData;
                                }
                            }
                            line.UpdateBy = purchase.UpdateBy;
                            line.InPutCount = line.InCount;
                            line.UpdateDate = DateTime.Now;
                            line.Batch = string.IsNullOrEmpty(line.Batch) ? "" : line.Batch;
                            plRepository.Update(line);
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
        /// 删除
        /// </summary>
        /// <param name="purchase"></param>
        /// <returns></returns>
        public int RemovePurchase(WPurchase purchase)
        {
            //查询明细
            if (purchase.Lines.Count < 1)
            {
                purchase.Lines = plRepository.GetLinesByParentId(purchase.ID);
            }

            DatabaseContext db = pRepository.DbCondext;
            try
            {
                db.BeginTransaction();
                //删除其他出库
                int result = pRepository.RemovePurchaseByStatus(purchase.ID, StockStatusEnum.New);
                if (result > 0)
                {
                    foreach (WPurchaseLine line in purchase.Lines)
                    {
                        if (line.SourceLineID > 0)
                        {
                            //删除下推
                            pnlRepository.RemoveDownCount(line.InCount, line.SourceLineID);
                        }
                    }
                    plRepository.RemoveLinesByParentId(purchase.ID);
                }
                db.CompleteTransaction();
                return result;
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw ex;
            }
        }


        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="purchase"></param>
        /// <returns></returns>
        public ResultData<string> ApprovePurchase(WPurchase purchase)
        {
            if (purchase.Lines.Count < 1)
            {
                purchase.Lines = plRepository.GetLinesByParentId(purchase.ID);
            }

            ResultData<string> rData = CheckValid(purchase);
            if (rData.status != 0)
            {
                return rData;
            }

            DatabaseContext dbContext = pRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();
                //入库记录
                List<WStockIn> stockIns = new List<WStockIn>(purchase.Lines.Count);

                //更新主表状态
                int result = pRepository.ApprovePurchase(purchase);
                if (result < 1)
                {
                    rData.status = -1;
                    rData.message = "单据已经审核或删除.";
                    dbContext.AbortTransaction();
                    return rData;
                }

                foreach (var line in purchase.Lines)
                {
                    stockIns.Add(ClonePurchase(line, purchase));
                    //添加完成数量
                    if (line.SourceLineID > 0)
                    {
                        pnlRepository.AddCompleteCount(line.SourceLineID, line.InCount);
                    }
                }

                //更新库存
                WStockService sService = new WStockService(dbContext);
                rData = sService.AddStocks(stockIns);
                if (rData.status != 0)
                {
                    dbContext.AbortTransaction();
                }

                //更新通知单状态
                if (purchase.SourceID > 0)
                {
                    bool isAll = pnlRepository.IsAllComplete(purchase.SourceID);
                    if (isAll)
                    {
                        pnRepository.UpdateNoticeStatus(NoticeStatusEnum.Complete, purchase.SourceID);
                    }
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

        #endregion

        #region 辅助

        /// <summary>
        /// 入库通知单转换入库单
        /// </summary>
        /// <param name="inNotice"></param>
        /// <returns></returns>
        private WPurchase CloneInNotice(WPurchaseNotice inNotice)
        {
            WPurchase Purchase = new WPurchase();
            Purchase.BusinessPartnerID = inNotice.BusinessPartnerID;
            Purchase.CreateBy = inNotice.CreateBy;
            Purchase.CreateDate = DateTime.Now;
            Purchase.SourceCode = inNotice.InNoticeCode;
            Purchase.SourceID = inNotice.ID;
            Purchase.StockStatus = StockStatusEnum.New;
            Purchase.WarehouseCode = inNotice.WarehouseCode;
            Purchase.WarehouseID = inNotice.WarehouseID;
            return Purchase;
        }

        /// <summary>
        /// 入库通知单行转换入库单行
        /// </summary>
        /// <param name="inNoticeLine"></param>
        /// <returns></returns>
        protected WPurchaseLine CloneInNoticeLine(WPurchaseNoticeLine inNoticeLine)
        {
            //如果通知数量-下推-完成<0 则返回
            decimal inCount = inNoticeLine.InCount - inNoticeLine.DownCount - inNoticeLine.CompleteCount;
            if (inCount <= 0)
            {
                return null;
            }
            WPurchaseLine line = new WPurchaseLine()
            {
                Batch = inNoticeLine.Batch,
                CreateDate = DateTime.Now,
                Factory = inNoticeLine.Factory,
                InCount = inCount,
                InPutCount = inCount,
                MaterialCode = inNoticeLine.MaterialCode,
                MaterialID = inNoticeLine.MaterialID,
                OwnerCode = inNoticeLine.OwnerCode,
                UnitID = inNoticeLine.UnitID,
                SourceLineID = inNoticeLine.ID
            };
            return line;
        }

        /// <summary>
        /// 其他入库复制入口记录
        /// </summary>
        /// <param name="line"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        private WStockIn ClonePurchase(WPurchaseLine line, WPurchase purchase)
        {
            WStockIn stockIn = new WStockIn();
            stockIn.Batch = line.Batch;
            stockIn.Factory = line.Factory;
            stockIn.InCount = line.InCount;
            stockIn.MaterialCode = line.MaterialCode;
            stockIn.MaterialID = line.MaterialID;
            stockIn.OwnerCode = line.OwnerCode;
            stockIn.PositionCode = line.PositionCode;
            stockIn.PositionID = line.PositionID;
            stockIn.SourceCode = purchase.PurchaseCode;
            stockIn.SourceID = line.ParentID;
            stockIn.SourceLineID = line.ID;
            stockIn.StockInDate = purchase.CreateDate;
            stockIn.StockInType = StockInEnum.Purchase;
            stockIn.UnitID = line.UnitID;
            stockIn.WarehouseCode = purchase.WarehouseCode;
            stockIn.WarehouseID = purchase.WarehouseID;
            stockIn.StockID = line.StockID;
            return stockIn;
        }

        /// <summary>
        /// 构建通知单不足提示
        /// </summary>
        /// <param name="stockOut"></param>
        /// <returns></returns>
        private string BuilderNoticeLessMessage(WPurchaseLine inNoticeLine)
        {
            StringBuilder sb = new StringBuilder("入库数量大于通知数量");
            sb.AppendLine("，物料：" + inNoticeLine.MaterialCode);
            if (!string.IsNullOrWhiteSpace(inNoticeLine.Batch))
            {
                sb.AppendLine("，批次：" + inNoticeLine.Batch);
            }
            if (!string.IsNullOrWhiteSpace(inNoticeLine.OwnerCode))
            {
                sb.AppendLine("，货主：" + inNoticeLine.OwnerCode);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="purchase"></param>
        /// <returns></returns>
        private ResultData<string> CheckValid(WPurchase purchase)
        {
            ResultData<string> rt = new ResultData<string>();
            if (purchase.WarehouseID < 0)
            {
                rt.status = -1;
                rt.message = "仓库不能为空";
                return rt;
            }

            foreach (var item in purchase.Lines)
            {
                //删除行,不验证
                if (item.CURD == CurdEnum.Delete)
                {
                    continue;
                }
                if (item.PositionID < 1)
                {
                    rt.status = -1;
                    rt.message = BuilderErrorMessage(item, "货位不能为空");
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
        private string BuilderErrorMessage(WPurchaseLine line, string msg)
        {
            StringBuilder sb = new StringBuilder(msg);
            sb.AppendLine("，货位：" + line.PositionCode);
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

        #endregion
    }
}
