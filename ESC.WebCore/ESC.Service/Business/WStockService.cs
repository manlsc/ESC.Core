using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Enums;
using ESC.Infrastructure.Repository;
using Dapper.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Service
{
    /// <summary>
    /// 库存操作 +
    /// </summary>
    public class WStockService
    {
        protected WStockRepository sRepository;  //库存
        protected WStockInRepository siRepository;  //入库记录
        protected WStockOutRepository soRepository; //出库记录

        public WStockService()
        {
            sRepository = new WStockRepository();
            siRepository = new WStockInRepository(sRepository.DbCondext);
            soRepository = new WStockOutRepository(sRepository.DbCondext);
        }

        public WStockService(DatabaseContext dbContext)
        {
            sRepository = new WStockRepository(dbContext);
            siRepository = new WStockInRepository(dbContext);
            soRepository = new WStockOutRepository(dbContext);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WStock> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = sRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 根据销售通知单条件查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WStock> PageSellNoticeSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = sRepository.GetSearchSellNoticeSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 根据采购入库单条件查询
        /// </summary>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WStock> PageSearchPurchase(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = sRepository.GetSearchPurchaseSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WStock> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return sRepository.Pages(pageIndex, pageSize, sql, args);
        }

        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="stockIns"></param>
        /// <returns></returns>
        public ResultData<string> AddStocks(List<WStockIn> stockIns)
        {
            ResultData<string> rData = new ResultData<string>();

            foreach (WStockIn stockIn in stockIns)
            {
                switch (stockIn.StockInType)
                {
                    case StockInEnum.TransferIn:  //调拨入库
                        decimal result = sRepository.DeleteTransitCount(stockIn.StockID, stockIn.InCount);
                        if (result < 0)
                        {
                            StringBuilder sb = new StringBuilder("库存在途数量小于入库数量");
                            sb.AppendLine("，货位：" + stockIn.PositionCode);
                            sb.AppendLine("，物料：" + stockIn.MaterialCode);
                            if (!string.IsNullOrWhiteSpace(stockIn.Batch))
                            {
                                sb.AppendLine("，批次：" + stockIn.Batch);
                            }
                            if (!string.IsNullOrWhiteSpace(stockIn.OwnerCode))
                            {
                                sb.AppendLine("，货主：" + stockIn.OwnerCode);
                            }
                            rData.message = sb.ToString();
                            rData.status = -1;
                            return rData;
                        }
                        break;
                }

                //根据物料+货位+入库时间+所有人+批次查询是否存在
                WStock stock = sRepository.GetStocks(stockIn.MaterialID, stockIn.PositionID, stockIn.StockInDate, stockIn.Batch);
                if (stock == null)
                {
                    stock = CloneStockIn(stockIn);
                    sRepository.Insert(stock);
                }
                else
                {
                    sRepository.AddStockCount(stock.ID, stockIn.InCount);
                }
                siRepository.Insert(stockIn);
            }
            return rData;
        }

        /// <summary>
        /// 出库
        /// </summary>
        /// <param name="stockOuts"></param>
        /// <returns></returns>
        public ResultData<string> DeleteStocks(List<WStockOut> stockOuts)
        {
            ResultData<string> rData = new ResultData<string>();
            decimal rt = 0;

            foreach (WStockOut stockOut in stockOuts)
            {
                //如果库存ID为空,则根据物料+货位+所有者+批次进行查询
                if (stockOut.StockID < 1)
                {
                    WStock stock = sRepository.GetStocks(stockOut.MaterialID, stockOut.PositionID, stockOut.Batch);
                    if (stock == null)
                    {
                        rData.status = -1;
                        rData.message = BuilderStockLessMessage(stockOut);
                        return rData;
                    }
                    stockOut.StockID = stock.ID;
                }
                switch (stockOut.StockOutType)
                {                   
                    case StockOutEnum.TransferOut:  //调拨出库
                        rt = sRepository.AddTransitCount(stockOut.StockID, stockOut.OutCount);
                        if (rt < 0)
                        {
                            rData.status = -1;
                            rData.message = BuilderStockLessMessage(stockOut);
                            return rData;
                        }
                        break;
                    default:
                        rt = sRepository.DeleteStockCount(stockOut.StockID, stockOut.OutCount);
                        if (rt < 0)
                        {
                            rData.status = -1;
                            rData.message = BuilderStockLessMessage(stockOut);
                            return rData;
                        }
                        break;
                }
                soRepository.Insert(stockOut);
            }

            return rData;
        }

        /// <summary>
        /// 构建库存不足提示
        /// </summary>
        /// <param name="stockOut"></param>
        /// <returns></returns>
        private string BuilderStockLessMessage(WStockOut stockOut)
        {
            StringBuilder sb = new StringBuilder("出库数量大于库存数量");
            sb.AppendLine("，货位：" + stockOut.PositionCode);
            sb.AppendLine("，物料：" + stockOut.MaterialCode);
            if (!string.IsNullOrWhiteSpace(stockOut.Batch))
            {
                sb.AppendLine("，批次：" + stockOut.Batch);
            }
            if (!string.IsNullOrWhiteSpace(stockOut.OwnerCode))
            {
                sb.AppendLine("，货主：" + stockOut.OwnerCode);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 入库记录转库存表
        /// </summary>
        /// <param name="stockIn"></param>
        /// <returns></returns>
        private WStock CloneStockIn(WStockIn stockIn)
        {
            WStock stock = new WStock();
            stock.Batch = string.IsNullOrEmpty(stockIn.Batch) ? "" : stockIn.Batch;  //批次
            stock.StockInDate = stockIn.StockInDate;  //入库时间
            stock.DateBatch = stockIn.StockInDate.ToString("yyyyMMdd");  //时间批次
            stock.Factory = string.IsNullOrEmpty(stockIn.Factory) ? "" : stockIn.Factory;  //工厂
            stock.MaterialCode = stockIn.MaterialCode;  //物料编码
            stock.MaterialID = stockIn.MaterialID;  //物料ID
            stock.OwnerCode = string.IsNullOrEmpty(stockIn.OwnerCode) ? "" : stockIn.OwnerCode;  //所有人
            stock.PositionCode = stockIn.PositionCode;  //货位编码
            stock.PositionID = stockIn.PositionID;  //货位ID
            stock.StockCount = stockIn.InCount;  //入库数量
            stock.UnLimitCount = stockIn.InCount;  //可用数量
            stock.UnitID = stockIn.UnitID;  //单位
            stock.WarehouseCode = stockIn.WarehouseCode;  //仓库编码
            stock.WarehouseID = stockIn.WarehouseID;  //仓库ID

            return stock;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public DataTable GetStocks(List<WhereItem> whereItems)
        {
            return sRepository.GetStocks(whereItems);
        }
    }
}
