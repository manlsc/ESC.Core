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
    /// 销售退库 +
    /// </summary>
    public class WSellReturnService
    {
        WSellReturnRepository srRepository;
        WSellReturnLineRepository srlRepository;
        WSellLineRepository slRepository;
        WSellRepository sRepository;
        SNumberRuleRepository nuRepository;

        public WSellReturnService()
        {
            srRepository = new WSellReturnRepository();
            srlRepository = new WSellReturnLineRepository(srRepository.DbCondext);
            nuRepository = new SNumberRuleRepository(srRepository.DbCondext);
            slRepository = new WSellLineRepository(srlRepository.DbCondext);
            sRepository = new WSellRepository(srlRepository.DbCondext);
        }

        #region 查询

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public WSellReturn GetWSellReturnById(int Id)
        {
            return srRepository.GetWSellReturnById(Id);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WSellReturn> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = srRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WSellReturn> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return srRepository.Pages(pageIndex, pageSize, sql, args);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WSellReturnLine> LinePageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = srlRepository.GetSearchSql(whereItems);
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public Page<WSellReturnLine> LinePageSearch(long pageIndex, long pageSize, int parentId)
        {
            //根据主表查询
            WhereItem item = new WhereItem()
            {
                condition = "=",
                datatype = "int",
                field = "ParentID",
                value = parentId.ToString()
            };
            string strSql = srlRepository.GetSearchSql(new List<WhereItem>() { item });
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WSellReturnLine> LinePageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return srlRepository.Pages(pageIndex, pageSize, sql, args);
        }

        #endregion

        /// <summary>
        /// 根据入库通知添加入库
        /// </summary>
        /// <param name="sell"></param>
        /// <returns></returns>
        public ResultData<string> AddSellReturn(WSell sell, int createBy)
        {
            ResultData<string> rData = new ResultData<string>();
            if (sell.StockStatus != StockStatusEnum.Approve)
            {
                rData.status = -1;
                rData.message = "单据未审核不能退库.";
                return rData;
            }

            //查询明细
            if (sell.Lines.Count < 1)
            {
                sell.Lines = slRepository.GetLinesByParentId(sell.ID);
            }

            //克隆主表
            WSellReturn sellReturn = CloneInNotice(sell);
            sellReturn.CreateBy = createBy;

            foreach (var item in sell.Lines)
            {
                WSellReturnLine line = CloneInNoticeLine(item);
                if (line != null)
                {
                    sellReturn.Lines.Add(line);
                }
            }

            if (sellReturn.Lines.Count < 1)
            {
                rData.status = -1;
                rData.message = "单据已经全部退库.";
                return rData;
            }

            DatabaseContext dbContext = srRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();

                //添加入库单
                sellReturn.CreateDate = DateTime.Now;
                sellReturn.StockStatus = StockStatusEnum.New;
                sellReturn.SellReturnCode = nuRepository.GetNextNumber("XSTK");
                srRepository.Insert(sellReturn);
                foreach (var line in sellReturn.Lines)
                {
                    line.ParentID = sellReturn.ID;
                    line.CreateBy = sellReturn.CreateBy;
                    line.CreateDate = DateTime.Now;
                    srlRepository.Insert(line);

                    //更新入库单 添加退库
                    decimal rt = slRepository.AddReturnCount(line.InCount, line.SourceLineID);
                    if (rt < 0)
                    {
                        dbContext.AbortTransaction();
                        rData.status = -1;
                        rData.message = BuilderNoticeLessMessage(line);
                        return rData;
                    }
                }

                //将插入主键赋值给返回值
                rData.result = sellReturn.ID.ToString();

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
        /// 插入新采购入库
        /// </summary>
        /// <param name="sellReturn"></param>
        /// <returns></returns>
        public ResultData<string> AddSellReturn(WSellReturn sellReturn)
        {
            ResultData<string> rData = CheckValid(sellReturn);
            if (rData.status != 0)
            {
                return rData;
            }

            DatabaseContext dbContext = srRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();

                sellReturn.CreateDate = DateTime.Now;
                sellReturn.StockStatus = StockStatusEnum.New;
                sellReturn.SellReturnCode = nuRepository.GetNextNumber("XSTK");
                srRepository.Insert(sellReturn);

                foreach (var line in sellReturn.Lines)
                {
                    line.ParentID = sellReturn.ID;
                    line.CreateBy = sellReturn.CreateBy;
                    line.CreateDate = DateTime.Now;
                    srlRepository.Insert(line);
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
        public ResultData<string> UpdateSellReturn(WSellReturn sellReturn)
        {
            ResultData<string> rData = CheckValid(sellReturn);
            if (rData.status != 0)
            {
                return rData;
            }
            if (sellReturn.StockStatus != StockStatusEnum.New)
            {
                rData.status = -1;
                rData.message = "单据已经审核,不能编辑.";
                return rData;
            }

            DatabaseContext db = srRepository.DbCondext;
            try
            {
                db.BeginTransaction();

                foreach (var line in sellReturn.Lines)
                {
                    switch (line.CURD)
                    {
                        case CurdEnum.Add:
                            if (line.SourceLineID > 0)
                            {
                                //添加退库
                                decimal result = slRepository.AddReturnCount(line.InCount, line.SourceLineID);
                                if (result < 0)
                                {
                                    db.AbortTransaction();
                                    rData.status = -1;
                                    rData.message = BuilderErrorMessage(line, "入库数量大于出库单数量.");
                                    return rData;
                                }
                            }
                            line.ParentID = sellReturn.ID;
                            line.CreateBy = sellReturn.CreateBy;
                            line.CreateDate = DateTime.Now;
                            line.InPutCount = line.InCount;
                            srlRepository.Insert(line);
                            break;
                        case CurdEnum.Delete:
                            srlRepository.Delete(line);
                            if (line.SourceLineID > 0)
                            {
                                //删除退库
                                slRepository.RemoveReturnCount(line.InCount, line.SourceLineID);
                            }
                            break;
                        case CurdEnum.Update:
                            if (line.SourceLineID > 0)
                            {
                                decimal result = slRepository.UpdateReturnCount(line.SourceLineID, line.InCount, line.InPutCount);
                                if (result < 0)
                                {
                                    db.AbortTransaction();
                                    rData.status = -1;
                                    rData.message = BuilderErrorMessage(line, "入库数量大于入库单数量.");
                                    return rData;
                                }
                            }
                            line.UpdateBy = sellReturn.UpdateBy;
                            line.InPutCount = line.InCount;
                            line.UpdateDate = DateTime.Now;
                            srlRepository.Update(line);
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
        public int RemoveSellReturn(WSellReturn sell)
        {
            //查询明细
            if (sell.Lines.Count < 1)
            {
                sell.Lines = srlRepository.GetLinesByParentId(sell.ID);
            }
           
            //删除其他出库
            int result = srRepository.RemoveSellReturnByStatus(sell.ID, StockStatusEnum.New);
            if (result > 0)
            {
                //如果退库单据，则更新出库数量
                foreach (WSellReturnLine line in sell.Lines)
                {
                    if (line.SourceLineID > 0)
                    {
                        //退库数量
                        slRepository.RemoveReturnCount(line.InCount, line.SourceLineID);
                    }
                }
                srlRepository.RemoveLinesByParentId(sell.ID);
            }
            return result;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="sellReturn"></param>
        /// <returns></returns>
        public ResultData<string> ApproveSellReturn(WSellReturn sellReturn)
        {
            ResultData<string> rData = new ResultData<string>();

            if (sellReturn.Lines.Count < 1)
            {
                sellReturn.Lines = srlRepository.GetLinesByParentId(sellReturn.ID);
            }

            DatabaseContext dbContext = srRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();
                //入库记录
                List<WStockIn> stockIns = new List<WStockIn>(sellReturn.Lines.Count);

                //添加其他入库
                int result = srRepository.ApproveSellReturn(sellReturn);
                if (result < 1)
                {
                    rData.status = -1;
                    rData.message = "单据已经审核或删除.";
                    dbContext.AbortTransaction();
                    return rData;
                }

                foreach (var line in sellReturn.Lines)
                {
                    stockIns.Add(CloneSellReturn(line, sellReturn));
                }

                //更新库存
                WStockService sService = new WStockService(dbContext);
                rData = sService.AddStocks(stockIns);
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
        /// 入库通知单转换入库单
        /// </summary>
        /// <param name="sell"></param>
        /// <returns></returns>
        private WSellReturn CloneInNotice(WSell sell)
        {
            WSellReturn sellReturn = new WSellReturn();
            sellReturn.BusinessPartnerID = sell.BusinessPartnerID;
            sellReturn.CreateBy = sell.CreateBy;
            sellReturn.CreateDate = DateTime.Now;
            sellReturn.SourceCode = sell.SellCode;
            sellReturn.SourceID = sell.ID;
            sellReturn.StockStatus = StockStatusEnum.New;
            sellReturn.WarehouseCode = sell.WarehouseCode;
            sellReturn.WarehouseID = sell.WarehouseID;
            return sellReturn;
        }

        /// <summary>
        /// 入库通知单行转换入库单行
        /// </summary>
        /// <param name="outNoticeLine"></param>
        /// <returns></returns>
        protected WSellReturnLine CloneInNoticeLine(WSellLine outNoticeLine)
        {
            //如果通知数量-下推-完成<0 则返回
            decimal outCount = outNoticeLine.OutCount - outNoticeLine.ReturnCount;
            if (outCount <= 0)
            {
                return null;
            }
            WSellReturnLine line = new WSellReturnLine()
            {
                Batch = outNoticeLine.Batch,
                CreateDate = DateTime.Now,
                Factory = outNoticeLine.Factory,
                InCount = outCount,
                MaterialCode = outNoticeLine.MaterialCode,
                MaterialID = outNoticeLine.MaterialID,
                OwnerCode = outNoticeLine.OwnerCode,
                UnitID = outNoticeLine.UnitID,
                SourceLineID = outNoticeLine.ID,
                PositionID = outNoticeLine.PositionID,
                PositionCode = outNoticeLine.PositionCode,
                InPutCount = outCount
            };
            return line;
        }

        /// <summary>
        /// 其他入库复制入口记录
        /// </summary>
        /// <param name="line"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        private WStockIn CloneSellReturn(WSellReturnLine line, WSellReturn SellReturn)
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
            stockIn.SourceCode = SellReturn.SellReturnCode;
            stockIn.SourceID = line.ParentID;
            stockIn.SourceLineID = line.ID;
            stockIn.StockInDate = SellReturn.CreateDate;
            stockIn.StockInType = StockInEnum.SellReturn;
            stockIn.UnitID = line.UnitID;
            stockIn.WarehouseCode = SellReturn.WarehouseCode;
            stockIn.WarehouseID = SellReturn.WarehouseID;
            stockIn.StockID = line.StockID;
            return stockIn;
        }

        /// <summary>
        /// 构建通知单不足提示
        /// </summary>
        /// <param name="stockOut"></param>
        /// <returns></returns>
        private string BuilderNoticeLessMessage(WSellReturnLine inNoticeLine)
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
        /// <param name="SellReturn"></param>
        /// <returns></returns>
        private ResultData<string> CheckValid(WSellReturn SellReturn)
        {
            ResultData<string> rt = new ResultData<string>();
            if (SellReturn.WarehouseID < 0)
            {
                rt.status = -1;
                rt.message = "仓库不能为空";
                return rt;
            }

            foreach (var item in SellReturn.Lines)
            {
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
        private string BuilderErrorMessage(WSellReturnLine line, string msg)
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
