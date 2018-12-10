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
    /// 销售出库 +
    /// </summary>
    public class WSellService
    {
        WSellRepository sRepository;
        WSellLineRepository slRepository;
        WSellNoticeLineRepository snlRepository;
        WSellNoticeRepository snRepository;
        SNumberRuleRepository nuRepository;

        public WSellService()
        {
            sRepository = new WSellRepository();
            slRepository = new WSellLineRepository(sRepository.DbCondext);
            nuRepository = new SNumberRuleRepository(sRepository.DbCondext);
            snlRepository = new WSellNoticeLineRepository(slRepository.DbCondext);
            snRepository = new WSellNoticeRepository(slRepository.DbCondext);
        }

        #region 查询

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public WSell GetWSellById(int Id)
        {
            return sRepository.GetWSellById(Id);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WSell> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = sRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WSell> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return sRepository.Pages(pageIndex, pageSize, sql, args);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WSellLine> LinePageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = slRepository.GetSearchSql(whereItems);
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public Page<WSellLine> LinePageSearch(long pageIndex, long pageSize, int parentId)
        {
            //根据主表查询
            WhereItem item = new WhereItem()
            {
                condition = "=",
                datatype = "int",
                field = "ParentID",
                value = parentId.ToString()
            };
            string strSql = slRepository.GetSearchSql(new List<WhereItem>() { item });
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WSellLine> LinePageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return slRepository.Pages(pageIndex, pageSize, sql, args);
        }

        #endregion

        #region 增删改

        /// <summary>
        /// 插入新采购出库
        /// </summary>
        /// <param name="sell"></param>
        /// <returns></returns>
        public ResultData<string> AddSell(WSell sell)
        {
            ResultData<string> rData = CheckValid(sell);
            if (rData.status != 0)
            {
                return rData;
            }

            DatabaseContext dbContext = sRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();

                sell.CreateDate = DateTime.Now;
                sell.StockStatus = StockStatusEnum.New;
                sell.SellCode = nuRepository.GetNextNumber("XSCK");
                sRepository.Insert(sell);

                foreach (var line in sell.Lines)
                {
                    line.ParentID = sell.ID;
                    line.CreateBy = sell.CreateBy;
                    line.CreateDate = DateTime.Now;
                    line.Batch = string.IsNullOrEmpty(line.Batch) ? "" : line.Batch;
                    slRepository.Insert(line);
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
        /// 根据通知单生成出库单
        /// </summary>
        /// <param name="outNotice"></param>
        /// <returns></returns>
        public ResultData<string> AddSell(WSellNotice outNotice, int createBy)
        {
            ResultData<string> rData = new ResultData<string>();
            if (outNotice.NoticeStatus == NoticeStatusEnum.Complete)
            {
                rData.status = -1;
                rData.message = "单据已完成不能下推.";
                return rData;
            }

            //查询明细
            if (outNotice.Lines.Count < 1)
            {
                outNotice.Lines = snlRepository.GetLinesByParentId(outNotice.ID);
            }

            //克隆主表
            WSell sell = CloneOutNotice(outNotice);
            sell.CreateBy = createBy;
            //克隆子表
            foreach (var item in outNotice.Lines)
            {
                WSellLine line = CloneOutNoticeLine(item);
                if (line != null)
                {
                    sell.Lines.Add(line);
                }
            }

            if (sell.Lines.Count < 1)
            {
                rData.status = -1;
                rData.message = "单据已经全部下推.";
                return rData;
            }

            DatabaseContext dbContext = sRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();

                //添加出库单
                sell.CreateDate = DateTime.Now;
                sell.StockStatus = StockStatusEnum.New;
                sell.SellCode = nuRepository.GetNextNumber("XSCK");
                sRepository.Insert(sell);

                foreach (var line in sell.Lines)
                {
                    //插入出库明细
                    line.ParentID = sell.ID;
                    line.CreateBy = sell.CreateBy;
                    line.CreateDate = DateTime.Now;
                    line.Batch = string.IsNullOrEmpty(line.Batch) ? "" : line.Batch;
                    slRepository.Insert(line);

                    //更新通知单 添加下推
                    decimal rt = snlRepository.AddDownCount(line.OutCount, line.SourceLineID);
                    if (rt < 0)
                    {
                        dbContext.AbortTransaction();
                        rData.status = -1;
                        rData.message = BuilderNoticeLessMessage(line);
                        return rData;
                    }
                }

                //将插入主键赋值给返回值
                rData.result = sell.ID.ToString();

                //更新通知单状态
                if (sell.SourceID > 0)
                {
                    snRepository.UpdateNoticeStatus(NoticeStatusEnum.Executing, sell.SourceID);
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
        /// 更新销售
        /// </summary>
        /// <param name="sell"></param>
        /// <returns></returns>
        public ResultData<string> UpdateSell(WSell sell)
        {
            ResultData<string> rData = CheckValid(sell);
            if (rData.status != 0)
            {
                return rData;
            }
            if (sell.StockStatus != StockStatusEnum.New)
            {
                rData.status = -1;
                rData.message = "单据已经审核,不能编辑.";
                return rData;
            }

            DatabaseContext db = snRepository.DbCondext;
            try
            {
                db.BeginTransaction();

                foreach (var line in sell.Lines)
                {
                    switch (line.CURD)
                    {
                        case CurdEnum.Add:
                            if (line.SourceLineID > 0)
                            {
                                //添加下推
                                decimal result = snlRepository.AddDownCount(line.OutCount, line.SourceLineID);
                                if (result < 0)
                                {
                                    db.AbortTransaction();
                                    rData.status = -1;
                                    rData.message = BuilderErrorMessage(line, "出库数量大于通知单数量.");
                                    return rData;
                                }
                            }
                            line.ParentID = sell.ID;
                            line.CreateBy = sell.CreateBy;
                            line.CreateDate = DateTime.Now;
                            line.OutPutCount = line.OutCount;
                            line.Batch = string.IsNullOrEmpty(line.Batch) ? "" : line.Batch;
                            slRepository.Insert(line);

                            break;
                        case CurdEnum.Delete:
                            slRepository.Delete(line);
                            if (line.SourceLineID > 0)
                            {
                                //删除下推
                                snlRepository.RemoveDownCount(line.OutCount, line.SourceLineID);
                            }
                            break;
                        case CurdEnum.Update:
                            if (line.SourceLineID > 0)
                            {
                                decimal result = snlRepository.UpdateDownCount(line.SourceLineID, line.OutCount, line.OutPutCount);
                                if (result < 0)
                                {
                                    db.AbortTransaction();
                                    rData.status = -1;
                                    rData.message = BuilderErrorMessage(line, "出库数量大于通知单数量.");
                                    return rData;
                                }
                            }
                            line.UpdateBy = sell.UpdateBy;
                            line.OutPutCount = line.OutCount;
                            line.UpdateDate = DateTime.Now;
                            line.Batch = string.IsNullOrEmpty(line.Batch) ? "" : line.Batch;
                            slRepository.Update(line);
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
        /// <param name="sell"></param>
        /// <returns></returns>
        public int RemoveSell(WSell sell)
        {
            //查询明细
            if (sell.Lines.Count < 1)
            {
                sell.Lines = slRepository.GetLinesByParentId(sell.ID);
            }

            DatabaseContext db = snRepository.DbCondext;
            try
            {
                db.BeginTransaction();
                //删除其他出库
                int result = sRepository.RemoveSellByStatus(sell.ID, StockStatusEnum.New);
                if (result > 0)
                {
                    foreach (WSellLine line in sell.Lines)
                    {
                        if (line.SourceLineID > 0)
                        {
                            //删除下推
                            snlRepository.RemoveDownCount(line.OutCount, line.SourceLineID);
                        }
                    }
                    slRepository.RemoveLinesByParentId(sell.ID);
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
        /// <param name="sell"></param>
        /// <returns></returns>
        public ResultData<string> ApproveSell(WSell sell)
        {
            if (sell.Lines.Count < 1)
            {
                sell.Lines = slRepository.GetLinesByParentId(sell.ID);
            }

            ResultData<string> rData = CheckValid(sell);
            if (rData.status != 0)
            {
                return rData;
            }

            DatabaseContext dbContext = sRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();
                //出库记录
                List<WStockOut> stockOuts = new List<WStockOut>(sell.Lines.Count);

                //添加其他出库
                int result = sRepository.ApproveSell(sell);
                if (result < 1)
                {
                    rData.status = -1;
                    rData.message = "单据已经审核或删除.";
                    dbContext.AbortTransaction();
                    return rData;
                }

                foreach (var line in sell.Lines)
                {
                    stockOuts.Add(CloneSell(line, sell));
                    //添加完成数量
                    if (line.SourceLineID > 0)
                    {
                        snlRepository.AddCompleteCount(line.SourceLineID, line.OutCount);
                    }
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

        #endregion

        #region 辅助

        /// <summary>
        /// 出库通知单转换出库单
        /// </summary>
        /// <param name="outNotice"></param>
        /// <returns></returns>
        private WSell CloneOutNotice(WSellNotice outNotice)
        {
            WSell Sell = new WSell();
            Sell.BusinessPartnerID = outNotice.BusinessPartnerID;
            Sell.CreateBy = outNotice.CreateBy;
            Sell.CreateDate = DateTime.Now;
            Sell.SourceCode = outNotice.OutNoticeCode;
            Sell.SourceID = outNotice.ID;
            Sell.StockStatus = StockStatusEnum.New;
            Sell.WarehouseCode = outNotice.WarehouseCode;
            Sell.WarehouseID = outNotice.WarehouseID;
            return Sell;
        }

        /// <summary>
        /// 出库通知单行转换出库单行
        /// </summary>
        /// <param name="outNoticeLine"></param>
        /// <returns></returns>
        protected WSellLine CloneOutNoticeLine(WSellNoticeLine outNoticeLine)
        {
            //如果通知数量-下推-完成<0 则返回
            decimal outCount = outNoticeLine.OutCount - outNoticeLine.DownCount - outNoticeLine.CompleteCount;
            if (outCount <= 0)
            {
                return null;
            }
            WSellLine line = new WSellLine()
            {
                Batch = outNoticeLine.Batch,
                CreateDate = DateTime.Now,
                Factory = outNoticeLine.Factory,
                OutCount = outCount,
                OutPutCount = outCount,
                MaterialCode = outNoticeLine.MaterialCode,
                MaterialID = outNoticeLine.MaterialID,
                OwnerCode = outNoticeLine.OwnerCode,
                UnitID = outNoticeLine.UnitID,
                SourceLineID = outNoticeLine.ID
            };
            return line;
        }

        /// <summary>
        /// 其他出库复制入口记录
        /// </summary>
        /// <param name="line"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        private WStockOut CloneSell(WSellLine line, WSell Sell)
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
            stockOut.SourceCode = Sell.SellCode;
            stockOut.SourceID = line.ParentID;
            stockOut.SourceLineID = line.ID;
            stockOut.StockInDate = Sell.CreateDate;
            stockOut.StockOutType = StockOutEnum.Sell;
            stockOut.UnitID = line.UnitID;
            stockOut.WarehouseCode = Sell.WarehouseCode;
            stockOut.WarehouseID = Sell.WarehouseID;
            stockOut.StockID = line.StockID;
            return stockOut;
        }

        /// <summary>
        /// 构建通知单不足提示
        /// </summary>
        /// <param name="stockOut"></param>
        /// <returns></returns>
        private string BuilderNoticeLessMessage(WSellLine inNoticeLine)
        {
            StringBuilder sb = new StringBuilder("出库数量大于通知数量");
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
        /// <param name="sell"></param>
        /// <returns></returns>
        private ResultData<string> CheckValid(WSell sell)
        {
            ResultData<string> rt = new ResultData<string>();
            if (sell.WarehouseID < 0)
            {
                rt.status = -1;
                rt.message = "仓库不能为空";
                return rt;
            }

            foreach (var item in sell.Lines)
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
        private string BuilderErrorMessage(WSellLine line, string msg)
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
