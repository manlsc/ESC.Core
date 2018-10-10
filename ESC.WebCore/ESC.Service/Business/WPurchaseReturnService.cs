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
    public class WPurchaseReturnService
    {
        WPurchaseReturnRepository prRepository;
        WPurchaseReturnLineRepository prlRepository;
        WPurchaseRepository pRepository;
        WPurchaseLineRepository plRepository;
        SNumberRuleRepository nuRepository;

        public WPurchaseReturnService()
        {
            prRepository = new WPurchaseReturnRepository();
            prlRepository = new WPurchaseReturnLineRepository(prRepository.DbCondext);
            nuRepository = new SNumberRuleRepository(prRepository.DbCondext);
            pRepository = new WPurchaseRepository(prlRepository.DbCondext);
            plRepository = new WPurchaseLineRepository(prlRepository.DbCondext);
        }

        #region 查询

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public WPurchaseReturn GetWPurchaseReturnById(int Id)
        {
            return prRepository.GetWPurchaseReturnById(Id);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WPurchaseReturn> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = prRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WPurchaseReturn> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return prRepository.Pages(pageIndex, pageSize, sql, args);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WPurchaseReturnLine> LinePageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = prlRepository.GetSearchSql(whereItems);
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public Page<WPurchaseReturnLine> LinePageSearch(long pageIndex, long pageSize, int parentId)
        {
            //根据主表查询
            WhereItem item = new WhereItem()
            {
                condition = "=",
                datatype = "int",
                field = "ParentID",
                value = parentId.ToString()
            };
            string strSql = prlRepository.GetSearchSql(new List<WhereItem>() { item });
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WPurchaseReturnLine> LinePageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return prlRepository.Pages(pageIndex, pageSize, sql, args);
        }

        #endregion

        /// <summary>
        /// 根据入库单下推出库单
        /// </summary>
        /// <param name="purchase"></param>
        /// <returns></returns>
        public ResultData<string> AddPurchaseReturn(WPurchase purchase, int createBy)
        {
            ResultData<string> rData = new ResultData<string>();
            if (purchase.StockStatus != StockStatusEnum.Approve)
            {
                rData.status = -1;
                rData.message = "单据未审核不能退库.";
                return rData;
            }

            //查询明细
            if (purchase.Lines.Count < 1)
            {
                purchase.Lines = plRepository.GetLinesByParentId(purchase.ID);
            }

            //克隆主表
            WPurchaseReturn purReturn = ClonePuchase(purchase);
            purReturn.CreateBy = createBy;
            //克隆子表
            foreach (var item in purchase.Lines)
            {
                WPurchaseReturnLine line = ClonePurchaseLine(item);
                if (line != null)
                {
                    purReturn.Lines.Add(line);
                }
            }

            if (purReturn.Lines.Count < 1)
            {
                rData.status = -1;
                rData.message = "单据已经全部退库.";
                return rData;
            }

            DatabaseContext dbContext = pRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();

                //添加入库单
                purReturn.CreateDate = DateTime.Now;
                purReturn.StockStatus = StockStatusEnum.New;
                purReturn.PurchaseReturnCode = nuRepository.GetNextNumber("CGTK");
                prRepository.Insert(purReturn);
                foreach (var line in purReturn.Lines)
                {
                    //插入入库明细
                    line.ParentID = purReturn.ID;
                    line.CreateBy = purReturn.CreateBy;
                    line.CreateDate = DateTime.Now;
                    prlRepository.Insert(line);

                    //更新入库单 添加退库
                    decimal rt = plRepository.AddReturnCount(line.OutCount, line.SourceLineID);
                    if (rt < 0)
                    {
                        dbContext.AbortTransaction();
                        rData.status = -1;
                        rData.message = BuilderNoticeLessMessage(line);
                        return rData;
                    }
                }

                //将插入主键赋值给返回值
                rData.result = purReturn.ID.ToString();

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
        /// 插入新采购出库
        /// </summary>
        /// <param name="purReturn"></param>
        /// <returns></returns>
        public ResultData<string> AddPurchaseReturn(WPurchaseReturn purReturn)
        {
            ResultData<string> rData = CheckValid(purReturn);
            if (rData.status != 0)
            {
                return rData;
            }

            DatabaseContext dbContext = prRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();

                purReturn.CreateDate = DateTime.Now;
                purReturn.StockStatus = StockStatusEnum.New;
                purReturn.PurchaseReturnCode = nuRepository.GetNextNumber("CGTK");
                prRepository.Insert(purReturn);

                foreach (var line in purReturn.Lines)
                {
                    line.ParentID = purReturn.ID;
                    line.CreateBy = purReturn.CreateBy;
                    line.CreateDate = DateTime.Now;
                    prlRepository.Insert(line);
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
        public ResultData<string> UpdatePurchaseReturn(WPurchaseReturn purReturn)
        {
            ResultData<string> rData = CheckValid(purReturn);
            if (rData.status != 0)
            {
                return rData;
            }
            if (purReturn.StockStatus != StockStatusEnum.New)
            {
                rData.status = -1;
                rData.message = "单据已经审核,不能编辑.";
                return rData;
            }

            DatabaseContext db = prRepository.DbCondext;
            try
            {
                db.BeginTransaction();

                foreach (var line in purReturn.Lines)
                {
                    switch (line.CURD)
                    {
                        case CurdEnum.Add:
                            if (line.SourceLineID > 0)
                            {
                                //添加退库
                                decimal result = plRepository.AddReturnCount(line.OutCount, line.SourceLineID);
                                if (result < 0)
                                {
                                    db.AbortTransaction();
                                    rData.status = -1;
                                    rData.message = BuilderErrorMessage(line, "出库数量大于入库单数量.");
                                    return rData;
                                }
                            }
                            line.ParentID = purReturn.ID;
                            line.CreateBy = purReturn.CreateBy;
                            line.CreateDate = DateTime.Now;
                            line.OutPutCount = line.OutCount;
                            prlRepository.Insert(line);

                            break;
                        case CurdEnum.Delete:
                            prlRepository.Delete(line);
                            if (line.SourceLineID > 0)
                            {
                                //删除退库
                                plRepository.RemoveReturnCount(line.OutCount, line.SourceLineID);
                            }
                            break;
                        case CurdEnum.Update:
                            if (line.SourceLineID > 0)
                            {
                                decimal result = plRepository.UpdateReturnCount(line.SourceLineID, line.OutCount, line.OutPutCount);
                                if (result < 0)
                                {
                                    db.AbortTransaction();
                                    rData.status = -1;
                                    rData.message = BuilderErrorMessage(line, "入库数量大于入库单数量.");
                                    return rData;
                                }
                            }
                            line.UpdateBy = purReturn.UpdateBy;
                            line.OutPutCount = line.OutCount;
                            line.UpdateDate = DateTime.Now;
                            prlRepository.Update(line);
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
        /// <param name="purReturn"></param>
        /// <returns></returns>
        public int RemovePurchaseReturn(WPurchaseReturn purReturn)
        {
            //查询明细
            if (purReturn.Lines.Count < 1)
            {
                purReturn.Lines = prlRepository.GetLinesByParentId(purReturn.ID);
            }         
            //删除其他出库
            int result = prRepository.RemovePurchaseReturnByStatus(purReturn.ID, StockStatusEnum.New);
            if (result > 0)
            {
                //如果退库单据，则更新出库数量
                foreach (WPurchaseReturnLine line in purReturn.Lines)
                {
                    if (line.SourceLineID > 0)
                    {
                        //退库数量
                        plRepository.RemoveReturnCount(line.OutCount, line.SourceLineID);
                    }
                }
                prlRepository.RemoveLinesByParentId(purReturn.ID);
            }
            return result;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="purReturn"></param>
        /// <returns></returns>
        public ResultData<string> ApprovePurchaseReturn(WPurchaseReturn purReturn)
        {          
            if (purReturn.Lines.Count < 1)
            {
                purReturn.Lines = prlRepository.GetLinesByParentId(purReturn.ID);
            }

            ResultData<string> rData = CheckValid(purReturn);
            if (rData.status != 0)
            {
                return rData;
            }

            DatabaseContext dbContext = prRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();
                //出库记录
                List<WStockOut> stockOuts = new List<WStockOut>(purReturn.Lines.Count);

                //添加其他出库
                int result = prRepository.ApprovePurchaseReturn(purReturn);
                if (result < 1)
                {
                    rData.status = -1;
                    rData.message = "单据已经审核或删除.";
                    dbContext.AbortTransaction();
                    return rData;
                }

                foreach (var line in purReturn.Lines)
                {
                    stockOuts.Add(ClonePurchaseReturn(line, purReturn));
                }

                //更新库存
                WStockService sService = new WStockService(dbContext);
                rData = sService.DeleteStocks(stockOuts);
                if (rData.status != 0)
                {
                    dbContext.AbortTransaction();
                }
                else
                {
                    dbContext.CompleteTransaction();
                }
            }
            catch (Exception ex)
            {
                dbContext.AbortTransaction();
                throw ex;
            }
            return rData;
        }

        /// <summary>
        /// 采购订单转换采购退
        /// </summary>
        /// <param name="purchase"></param>
        /// <returns></returns>
        private WPurchaseReturn ClonePuchase(WPurchase purchase)
        {
            WPurchaseReturn purchaseReturn = new WPurchaseReturn();
            purchaseReturn.BusinessPartnerID = purchase.BusinessPartnerID;
            purchaseReturn.CreateBy = purchase.CreateBy;
            purchaseReturn.CreateDate = DateTime.Now;
            purchaseReturn.SourceCode = purchase.PurchaseCode;
            purchaseReturn.SourceID = purchase.ID;
            purchaseReturn.StockStatus = StockStatusEnum.New;
            purchaseReturn.WarehouseCode = purchase.WarehouseCode;
            purchaseReturn.WarehouseID = purchase.WarehouseID;
            return purchaseReturn;
        }

        /// <summary>
        /// 出库入库单行转换出库单行
        /// </summary>
        /// <param name="purchaseLine"></param>
        /// <returns></returns>
        protected WPurchaseReturnLine ClonePurchaseLine(WPurchaseLine purchaseLine)
        {
            //如果通知数量-下推-完成<0 则返回
            decimal inCount = purchaseLine.InCount - purchaseLine.ReturnCount;
            if (inCount <= 0)
            {
                return null;
            }
            WPurchaseReturnLine line = new WPurchaseReturnLine()
            {
                Batch = purchaseLine.Batch,
                CreateDate = DateTime.Now,
                Factory = purchaseLine.Factory,
                OutCount = inCount,
                MaterialCode = purchaseLine.MaterialCode,
                MaterialID = purchaseLine.MaterialID,
                OwnerCode = purchaseLine.OwnerCode,
                UnitID = purchaseLine.UnitID,
                SourceLineID = purchaseLine.ID,
                PositionID = purchaseLine.PositionID,
                PositionCode = purchaseLine.PositionCode,
                OutPutCount= inCount
            };
            return line;
        }

        /// <summary>
        /// 其他出库复制入口记录
        /// </summary>
        /// <param name="line"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        private WStockOut ClonePurchaseReturn(WPurchaseReturnLine line, WPurchaseReturn purReturn)
        {
            WStockOut stockOut = new WStockOut();
            stockOut.Batch = line.Batch;
            stockOut.Factory = line.Factory;
            stockOut.OutCount = line.OutCount;
            stockOut.MaterialCode = line.MaterialCode;
            stockOut.MaterialID = line.MaterialID;
            stockOut.OwnerCode = line.OwnerCode;
            stockOut.PositionCode = line.PositionCode;
            stockOut.PositionID = line.PositionID;
            stockOut.SourceCode = purReturn.PurchaseReturnCode;
            stockOut.SourceID = line.ParentID;
            stockOut.SourceLineID = line.ID;
            stockOut.StockInDate = purReturn.CreateDate;
            stockOut.StockOutType = StockOutEnum.PurchaseReturn;
            stockOut.UnitID = line.UnitID;
            stockOut.WarehouseCode = purReturn.WarehouseCode;
            stockOut.WarehouseID = purReturn.WarehouseID;
            stockOut.StockID = line.StockID;
            return stockOut;
        }

        /// <summary>
        /// 构建出库数量大于入库数量
        /// </summary>
        /// <param name="returnLine"></param>
        /// <returns></returns>
        private string BuilderNoticeLessMessage(WPurchaseReturnLine returnLine)
        {
            StringBuilder sb = new StringBuilder("出库数量大于入库数量");
            sb.AppendLine("，物料：" + returnLine.MaterialCode);
            if (!string.IsNullOrWhiteSpace(returnLine.Batch))
            {
                sb.AppendLine("，批次：" + returnLine.Batch);
            }
            if (!string.IsNullOrWhiteSpace(returnLine.OwnerCode))
            {
                sb.AppendLine("，货主：" + returnLine.OwnerCode);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="purReutrn"></param>
        /// <returns></returns>
        private ResultData<string> CheckValid(WPurchaseReturn purReutrn)
        {
            ResultData<string> rt = new ResultData<string>();
            if (purReutrn.WarehouseID < 0)
            {
                rt.status = -1;
                rt.message = "仓库不能为空";
                return rt;
            }

            foreach (var item in purReutrn.Lines)
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
        private string BuilderErrorMessage(WPurchaseReturnLine line, string msg)
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
    }
}
