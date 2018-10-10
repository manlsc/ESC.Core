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
    public class WTransferOutService
    {
        WTransferOutRepository toRepository;
        WTransferOutLineRepository tolRepository;
        SNumberRuleRepository nuRepository;

        public WTransferOutService()
        {
            toRepository = new WTransferOutRepository();
            tolRepository = new WTransferOutLineRepository(toRepository.DbCondext);
            nuRepository = new SNumberRuleRepository(toRepository.DbCondext);
        }

        /// <summary>
        /// 根据ID查询明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WTransferOut GetTransferOutById(int id)
        {
            return toRepository.GetTransferOutById(id);
        }

        /// <summary>
        /// 插入新其他出库
        /// </summary>
        /// <param name="transferOut"></param>
        /// <returns></returns>
        public ResultData<string> AddTransferOut(WTransferOut transferOut)
        {
            ResultData<string> rData = CheckValid(transferOut);
            if (rData.status != 0)
            {
                return rData;
            }

            DatabaseContext dbContext = toRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();
                //出库记录
                List<WStockOut> stockOuts = new List<WStockOut>(transferOut.Lines.Count);

                //添加其他出库
                transferOut.CreateDate = DateTime.Now;
                transferOut.StockStatus = StockStatusEnum.New;
                transferOut.StockOutType = StockOutEnum.TransferOut;
                transferOut.TransferOutCode = nuRepository.GetNextNumber("DBCK");
                toRepository.Insert(transferOut);

                foreach (var line in transferOut.Lines)
                {
                    line.ParentID = transferOut.ID;
                    line.CreateBy = transferOut.CreateBy;
                    line.CreateDate = DateTime.Now;
                    tolRepository.Insert(line);
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
        /// 删除其他出库
        /// </summary>
        /// <param name="transferOuts"></param>
        /// <returns></returns>
        public ResultData<string> RemoveTransferOuts(List<WTransferOut> transferOuts)
        {
            DatabaseContext db = toRepository.DbCondext;
            ResultData<string> rData = new ResultData<string>();
            try
            {
                db.BeginTransaction();

                foreach (WTransferOut u in transferOuts)
                {
                    int result = RemoveTransferOut(u);
                    if (result < 1)
                    {
                        rData.status = -1;
                        rData.message = "只有新建状态的单据才能删除.";
                        db.AbortTransaction();
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
        /// 删除其他出库
        /// </summary>
        /// <param name="transferOut"></param>
        /// <returns></returns>
        public int RemoveTransferOut(WTransferOut transferOut)
        {
            //删除其他出库
            int result = toRepository.RemoveTransferOutByStatus(transferOut.ID, StockStatusEnum.New);
            if (result > 0)
            {
                tolRepository.RemoveLinesByParentId(transferOut.ID);
            }
            return result;
        }

        /// <summary>
        /// 根据Id删除其他出库
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int RemoveTransferOut(int Id)
        {
            //删除其他出库
            int result = toRepository.RemoveTransferOutByStatus(Id, StockStatusEnum.New);
            if (result > 0)
            {
                tolRepository.RemoveLinesByParentId(Id);
            }
            return result;
        }

        /// <summary>
        /// 更新其他出库
        /// </summary>
        /// <param name="transferOut"></param>
        /// <returns></returns>
        public ResultData<string> UpdateTransferOut(WTransferOut transferOut)
        {
            ResultData<string> rData = CheckValid(transferOut);
            if (rData.status != 0)
            {
                return rData;
            }
            if (transferOut.StockStatus != StockStatusEnum.New)
            {
                rData.status = -1;
                rData.message = "单据已经审核,不能编辑.";
                return rData;
            }

            DatabaseContext db = toRepository.DbCondext;
            try
            {
                db.BeginTransaction();

                toRepository.Update(transferOut);
                foreach (var line in transferOut.Lines)
                {
                    switch (line.CURD)
                    {
                        case CurdEnum.Add:
                            line.ParentID = transferOut.ID;
                            line.CreateBy = transferOut.CreateBy;
                            line.CreateDate = DateTime.Now;
                            tolRepository.Insert(line);
                            break;
                        case CurdEnum.Delete:
                            tolRepository.Delete(line);
                            break;
                        case CurdEnum.Update:
                            line.UpdateBy = transferOut.UpdateBy;
                            line.UpdateDate = DateTime.Now;
                            tolRepository.Update(line);
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
        /// 审核
        /// </summary>
        /// <param name="transferOut"></param>
        /// <returns></returns>
        public ResultData<string> ApproveTransferOut(WTransferOut transferOut)
        {
            ResultData<string> rData = new ResultData<string>();

            if (transferOut.Lines.Count < 1)
            {
                transferOut.Lines = tolRepository.GetLinesByParentId(transferOut.ID);
            }

            DatabaseContext dbContext = toRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();
                //出库记录
                List<WStockOut> stockOuts = new List<WStockOut>(transferOut.Lines.Count);

                //添加其他出库
                int result = toRepository.ApproveTransferOut(transferOut);
                if (result < 1)
                {
                    rData.status = -1;
                    rData.message = "单据已经审核或删除.";
                    dbContext.AbortTransaction();
                    return rData;
                }

                foreach (var line in transferOut.Lines)
                {
                    stockOuts.Add(CloneTransferOut(line, transferOut));
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

            if (rData.status == 0)
            {
                //调拨入库通知单
                WTransferInNotice inNotice = CloneInNotice(transferOut);
                rData = new WTransferInNoticeService().AddTransferInNotice(inNotice);
            }
            return rData;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WTransferOut> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = toRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WTransferOut> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return toRepository.Pages(pageIndex, pageSize, sql, args);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WTransferOutLine> LinePageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = tolRepository.GetSearchSql(whereItems);
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public Page<WTransferOutLine> LinePageSearch(long pageIndex, long pageSize, int parentId)
        {
            //根据主表查询
            WhereItem item = new WhereItem()
            {
                condition = "=",
                datatype = "int",
                field = "ParentID",
                value = parentId.ToString()
            };
            string strSql = tolRepository.GetSearchSql(new List<WhereItem>() { item });
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WTransferOutLine> LinePageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return tolRepository.Pages(pageIndex, pageSize, sql, args);
        }

        /// <summary>
        /// 其他出库复制入口记录
        /// </summary>
        /// <param name="line"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        private WStockOut CloneTransferOut(WTransferOutLine line, WTransferOut other)
        {
            WStockOut stockOut = new WStockOut();
            stockOut.Batch = line.Batch;  //批次
            stockOut.Factory = line.Factory;  //工厂
            stockOut.OutCount = line.OutCount;  //出库数量
            stockOut.MaterialCode = line.MaterialCode;  //物理编码
            stockOut.MaterialID = line.MaterialID;  //物料ID
            stockOut.OwnerCode = line.OwnerCode;  //所有者
            stockOut.PositionCode = line.PositionCode; //货位编码
            stockOut.PositionID = line.PositionID;  //货位ID
            stockOut.SourceCode = other.TransferOutCode;  //出库单号
            stockOut.SourceID = line.ParentID;  //出库ID
            stockOut.SourceLineID = line.ID;  //出库行ID
            stockOut.StockOutDate = other.CreateDate;  //出库时间
            stockOut.StockOutType = other.StockOutType;  //出库类型
            stockOut.UnitID = line.UnitID;  //单位
            stockOut.WarehouseCode = other.FWarehouseCode;  //仓库编码
            stockOut.WarehouseID = other.FWarehouseID;  //仓库ID
            stockOut.StockInDate = line.StockInDate;  //入库时间
            stockOut.StockID = line.StockID;  //库存ID
            return stockOut;
        }

        /// <summary>
        /// 赋值调拨入库通知单
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        private WTransferInNotice CloneInNotice(WTransferOut other)
        {
            WTransferInNotice inNotice = new WTransferInNotice();
            inNotice.BusinessPartnerID = other.BusinessPartnerID;
            inNotice.CreateBy = other.CreateBy;
            inNotice.CreateDate = DateTime.Now;
            inNotice.SourceCode = other.TransferOutCode;
            inNotice.SourceID = other.ID;
            inNotice.WarehouseID = other.TWarehouseID;
            inNotice.WarehouseCode = other.TWarehouseCode;
            foreach (var item in other.Lines)
            {
                inNotice.Lines.Add(new WTransferInNoticeLine()
                {
                    StockID=item.StockID,
                    Batch = item.Batch,
                    CompleteCount = 0,
                    CreateBy = item.CreateBy,
                    CreateDate = DateTime.Now,
                    DownCount = 0,
                    Factory = item.Factory,
                    InCount = item.OutCount,
                    MaterialCode = item.MaterialCode,
                    MaterialID = item.MaterialID,
                    OwnerCode = item.OwnerCode,
                    InPutCount=item.OutCount,
                    UnitID = item.UnitID
                });
            }
            return inNotice;
        }

        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="transferOut"></param>
        /// <returns></returns>
        private ResultData<string> CheckValid(WTransferOut transferOut)
        {
            ResultData<string> rt = new ResultData<string>();
            if (transferOut.FWarehouseID < 0)
            {
                rt.status = -1;
                rt.message = "调出仓库不能为空";
                return rt;                
            }
            if (transferOut.TWarehouseID < 0)
            {
                rt.status = -1;
                rt.message = "调入仓库不能为空";
                return rt;
            }

            foreach (var item in transferOut.Lines)
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
        private string BuilderErrorMessage(WTransferOutLine line, string msg)
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
