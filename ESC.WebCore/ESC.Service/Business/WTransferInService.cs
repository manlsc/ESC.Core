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
    public class WTransferInService
    {
        WTransferInRepository tiRepository;
        WTransferInLineRepository tilRepository;
        WTransferInNoticeLineRepository tinlRepository;
        WTransferInNoticeRepository tinRepository;
        SNumberRuleRepository nuRepository;

        public WTransferInService()
        {
            tiRepository = new WTransferInRepository();
            tilRepository = new WTransferInLineRepository(tiRepository.DbCondext);
            nuRepository = new SNumberRuleRepository(tiRepository.DbCondext);
            tinlRepository = new WTransferInNoticeLineRepository(tilRepository.DbCondext);
            tinRepository = new WTransferInNoticeRepository(tilRepository.DbCondext);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public WTransferIn GetWTransferInById(int Id)
        {
            return tiRepository.GetWTransferInById(Id);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WTransferIn> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = tiRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WTransferIn> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return tiRepository.Pages(pageIndex, pageSize, sql, args);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WTransferInLine> LinePageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = tilRepository.GetSearchSql(whereItems);
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public Page<WTransferInLine> LinePageSearch(long pageIndex, long pageSize, int parentId)
        {
            //根据主表查询
            WhereItem item = new WhereItem()
            {
                condition = "=",
                datatype = "int",
                field = "ParentID",
                value = parentId.ToString()
            };
            string strSql = tilRepository.GetSearchSql(new List<WhereItem>() { item });
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WTransferInLine> LinePageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return tilRepository.Pages(pageIndex, pageSize, sql, args);
        }

        /// <summary>
        /// 插入新其他出库
        /// </summary>
        /// <param name="transferIn"></param>
        /// <returns></returns>
        public ResultData<string> AddTransferIn(WTransferIn transferIn)
        {
            ResultData<string> rData = CheckValid(transferIn);
            if (rData.status != 0)
            {
                return rData;
            }

            DatabaseContext dbContext = tiRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();

                transferIn.CreateDate = DateTime.Now;
                transferIn.StockStatus = StockStatusEnum.New;
                transferIn.TransferInCode = nuRepository.GetNextNumber("DBRK");
                tiRepository.Insert(transferIn);

                foreach (var line in transferIn.Lines)
                {
                    line.ParentID = transferIn.ID;
                    line.CreateBy = transferIn.CreateBy;
                    line.CreateDate = DateTime.Now;
                    tilRepository.Insert(line);
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
        public ResultData<string> AddTransferIn(WTransferInNotice inNotice, int createBy)
        {
            ResultData<string> rData = new ResultData<string>();
            if (inNotice.NoticeStatus == NoticeStatusEnum.Complete)
            {
                rData.status = -1;
                rData.message = "单据已完成不能入库.";
                return rData;
            }
            //克隆主表
            WTransferIn transferIn = CloneInNotice(inNotice);
            transferIn.CreateBy = createBy;

            foreach (var item in inNotice.Lines)
            {
                if (item.InPutCount <= 0)
                {
                    continue;
                }
                //如果通知数量+下推数量+完成数量>当前入库数量
                if (item.InPutCount + item.DownCount + item.CompleteCount > item.InCount)
                {
                    rData.status = -1;
                    rData.message = BuilderNoticeLessMessage(item);
                    return rData;
                }
                //克隆明细
                transferIn.Lines.Add(CloneInNoticeLine(item));
                //更新下推数量和当前输入数量
                item.DownCount = item.DownCount + item.InPutCount;
            }

            if (transferIn.Lines.Count < 1)
            {
                rData.status = -1;
                rData.message = "入库明细为空.";
                return rData;
            }

            rData = CheckValid(transferIn);
            if (rData.status != 0)
            {
                return rData;
            }

            DatabaseContext dbContext = tiRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();

                //添加入库单
                transferIn.CreateDate = DateTime.Now;
                transferIn.StockStatus = StockStatusEnum.New;
                transferIn.TransferInCode = nuRepository.GetNextNumber("DBRK");
                tiRepository.Insert(transferIn);
                foreach (var line in transferIn.Lines)
                {
                    line.ParentID = transferIn.ID;
                    line.CreateBy = transferIn.CreateBy;
                    line.CreateDate = DateTime.Now;
                    tilRepository.Insert(line);
                }

                //更新通知单
                foreach (var item in inNotice.Lines)
                {
                    tinlRepository.AddDownCount(item);
                }

                dbContext.CompleteTransaction();
            }
            catch (Exception ex)
            {
                dbContext.AbortTransaction();
                throw ex;
            }

            //没有放到事务,因为状态更新不是很重要,不影响业务
            bool isAll = tinlRepository.IsAllDownload(inNotice.ID);
            if (isAll)
            {
                tinRepository.UpdateNoticeStatus(NoticeStatusEnum.Complete, inNotice.ID);
            }
            else
            {
                tinRepository.UpdateNoticeStatus(NoticeStatusEnum.Executing, inNotice.ID);
            }
            return rData;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="transferIn"></param>
        /// <returns></returns>
        public ResultData<string> ApproveOtherIn(WTransferIn transferIn)
        {
            ResultData<string> rData = new ResultData<string>();

            if (transferIn.Lines.Count < 1)
            {
                transferIn.Lines = tilRepository.GetLinesByParentId(transferIn.ID);
            }

            DatabaseContext dbContext = tiRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();
                //入库记录
                List<WStockIn> stockIns = new List<WStockIn>(transferIn.Lines.Count);

                //添加其他入库
                int result = tiRepository.ApproveTransferIn(transferIn);
                if (result < 1)
                {
                    rData.status = -1;
                    rData.message = "单据已经审核或删除.";
                    dbContext.AbortTransaction();
                    return rData;
                }

                foreach (var line in transferIn.Lines)
                {
                    stockIns.Add(CloneTransferIn(line, transferIn));
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
        /// <param name="inNotice"></param>
        /// <returns></returns>
        private WTransferIn CloneInNotice(WTransferInNotice inNotice)
        {
            WTransferIn transferIn = new WTransferIn();
            transferIn.BusinessPartnerID = inNotice.BusinessPartnerID;
            transferIn.CreateBy = inNotice.CreateBy;
            transferIn.CreateDate = DateTime.Now;
            transferIn.SourceCode = inNotice.InNoticeCode;
            transferIn.SourceID = inNotice.ID;
            transferIn.StockStatus = StockStatusEnum.New;
            transferIn.WarehouseCode = inNotice.WarehouseCode;
            transferIn.WarehouseID = inNotice.WarehouseID;
            return transferIn;
        }

        /// <summary>
        /// 入库通知单行转换入库单行
        /// </summary>
        /// <param name="inNoticeLine"></param>
        /// <returns></returns>
        protected WTransferInLine CloneInNoticeLine(WTransferInNoticeLine inNoticeLine)
        {
            WTransferInLine line = new WTransferInLine()
            {
                Batch = inNoticeLine.Batch,
                CreateDate = DateTime.Now,
                Factory = inNoticeLine.Factory,
                InCount = inNoticeLine.InPutCount,
                MaterialCode = inNoticeLine.MaterialCode,
                MaterialID = inNoticeLine.MaterialID,
                OwnerCode = inNoticeLine.OwnerCode,
                PositionCode = inNoticeLine.PositionCode,
                PositionID = inNoticeLine.PositionID,
                UnitID = inNoticeLine.UnitID,
                SourceLineID = inNoticeLine.ID,
                StockID=inNoticeLine.StockID
            };
            return line;
        }

        /// <summary>
        /// 其他入库复制入口记录
        /// </summary>
        /// <param name="line"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        private WStockIn CloneTransferIn(WTransferInLine line, WTransferIn transferIn)
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
            stockIn.SourceCode = transferIn.TransferInCode;
            stockIn.SourceID = line.ParentID;
            stockIn.SourceLineID = line.ID;
            stockIn.StockInDate = transferIn.CreateDate;
            stockIn.StockInType = StockInEnum.TransferIn;
            stockIn.UnitID = line.UnitID;
            stockIn.WarehouseCode = transferIn.WarehouseCode;
            stockIn.WarehouseID = transferIn.WarehouseID;
            stockIn.StockID = line.StockID;
            return stockIn;
        }

        /// <summary>
        /// 构建通知单不足提示
        /// </summary>
        /// <param name="stockOut"></param>
        /// <returns></returns>
        private string BuilderNoticeLessMessage(WTransferInNoticeLine inNoticeLine)
        {
            StringBuilder sb = new StringBuilder("入库数量大于通知数量");
            sb.AppendLine("，货位：" + inNoticeLine.PositionCode);
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
        /// <param name="transferIn"></param>
        /// <returns></returns>
        private ResultData<string> CheckValid(WTransferIn transferIn)
        {
            ResultData<string> rt = new ResultData<string>();
            if (transferIn.WarehouseID < 0)
            {
                rt.status = -1;
                rt.message = "仓库不能为空";
                return rt;
            }
           
            foreach (var item in transferIn.Lines)
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
        private string BuilderErrorMessage(WTransferInLine line, string msg)
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
