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
    /// 其他出库 +
    /// </summary>
    public class WOtherOutService
    {
        WOtherOutRepository ooRepository;
        WOtherOutLineRepository oolRepository;
        SNumberRuleRepository nuRepository;

        public WOtherOutService()
        {
            ooRepository = new WOtherOutRepository();
            oolRepository = new WOtherOutLineRepository(ooRepository.DbCondext);
            nuRepository = new SNumberRuleRepository(ooRepository.DbCondext);
        }

        /// <summary>
        /// 根据ID查询明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WOtherOut GetOtherOutById(int id)
        {
            return ooRepository.GetOtherOutById(id);
        }

        /// <summary>
        /// 插入新其他出库
        /// </summary>
        /// <param name="otherOut"></param>
        /// <returns></returns>
        public ResultData<string> AddOtherOut(WOtherOut otherOut)
        {
            ResultData<string> rData = CheckValid(otherOut);
            if (rData.status != 0)
            {
                return rData;
            }

            DatabaseContext dbContext = ooRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();
                //出库记录
                List<WStockOut> stockOuts = new List<WStockOut>(otherOut.Lines.Count);

                //添加其他出库
                otherOut.CreateDate = DateTime.Now;
                otherOut.StockStatus = StockStatusEnum.New;
                otherOut.OtherOutCode = nuRepository.GetNextNumber("QTCK");
                ooRepository.Insert(otherOut);

                foreach (var line in otherOut.Lines)
                {
                    line.ParentID = otherOut.ID;
                    line.CreateBy = otherOut.CreateBy;
                    line.CreateDate = DateTime.Now;
                    oolRepository.Insert(line);
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
        /// <param name="otherOuts"></param>
        /// <returns></returns>
        public ResultData<string> RemoveOtherOuts(List<WOtherOut> otherOuts)
        {
            DatabaseContext db = ooRepository.DbCondext;
            ResultData<string> rData = new ResultData<string>();
            try
            {
                db.BeginTransaction();

                foreach (WOtherOut u in otherOuts)
                {
                    int result = RemoveOtherOut(u);
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
        /// <param name="otherOut"></param>
        /// <returns></returns>
        public int RemoveOtherOut(WOtherOut otherOut)
        {
            //删除其他出库
            int result = ooRepository.RemoveOtherOutByStatus(otherOut.ID, StockStatusEnum.New);
            if (result > 0)
            {
                oolRepository.RemoveLinesByParentId(otherOut.ID);
            }
            return result;
        }

        /// <summary>
        /// 根据Id删除其他出库
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int RemoveOtherOut(int Id)
        {
            //删除其他出库
            int result = ooRepository.RemoveOtherOutByStatus(Id, StockStatusEnum.New);
            if (result > 0)
            {
                oolRepository.RemoveLinesByParentId(Id);
            }
            return result;
        }

        /// <summary>
        /// 更新其他出库
        /// </summary>
        /// <param name="otherOut"></param>
        /// <returns></returns>
        public ResultData<string> UpdateOtherOut(WOtherOut otherOut)
        {
            ResultData<string> rData = CheckValid(otherOut);
            if (rData.status != 0)
            {
                return rData;
            }

            if (otherOut.StockStatus != StockStatusEnum.New)
            {
                rData.status = -1;
                rData.message = "单据已经审核,不能编辑.";
                return rData;
            }

            DatabaseContext db = ooRepository.DbCondext;
            try
            {
                db.BeginTransaction();

                otherOut.UpdateDate = DateTime.Now;
                ooRepository.Update(otherOut);
                foreach (var line in otherOut.Lines)
                {
                    switch (line.CURD)
                    {
                        case CurdEnum.Add:
                            line.ParentID = otherOut.ID;
                            line.CreateBy = otherOut.CreateBy;
                            line.CreateDate = DateTime.Now;
                            oolRepository.Insert(line);
                            break;
                        case CurdEnum.Delete:
                            oolRepository.Delete(line);
                            break;
                        case CurdEnum.Update:
                            line.UpdateBy = otherOut.UpdateBy;
                            line.UpdateDate = DateTime.Now;
                            oolRepository.Update(line);
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
        /// <param name="otherOut"></param>
        /// <returns></returns>
        public ResultData<string> ApproveOtherOut(WOtherOut otherOut)
        {
            ResultData<string> rData = new ResultData<string>();

            if (otherOut.Lines.Count < 1)
            {
                otherOut.Lines = oolRepository.GetLinesByParentId(otherOut.ID);
            }

            DatabaseContext dbContext = ooRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();
                //出库记录
                List<WStockOut> stockOuts = new List<WStockOut>(otherOut.Lines.Count);

                //添加其他出库
                int result = ooRepository.ApproveOtherOut(otherOut);
                if (result < 1)
                {
                    rData.status = -1;
                    rData.message = "单据已经审核或删除.";
                    dbContext.AbortTransaction();
                    return rData;
                }

                foreach (var line in otherOut.Lines)
                {
                    stockOuts.Add(CloneOtherOut(line, otherOut));
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

        #region 查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WOtherOut> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = ooRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WOtherOut> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return ooRepository.Pages(pageIndex, pageSize, sql, args);
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WOtherOutLine> LinePageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = oolRepository.GetSearchSql(whereItems);
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public Page<WOtherOutLine> LinePageSearch(long pageIndex, long pageSize, int parentId)
        {
            //根据主表查询
            WhereItem item = new WhereItem()
            {
                condition = "=",
                datatype = "int",
                field = "ParentID",
                value = parentId.ToString()
            };
            string strSql = oolRepository.GetSearchSql(new List<WhereItem>() { item });
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WOtherOutLine> LinePageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return oolRepository.Pages(pageIndex, pageSize, sql, args);
        }

        #endregion

        /// <summary>
        /// 其他出库复制入口记录
        /// </summary>
        /// <param name="line"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        private WStockOut CloneOtherOut(WOtherOutLine line, WOtherOut other)
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
            stockOut.SourceCode = other.OtherOutCode;  //出库单号
            stockOut.SourceID = line.ParentID;  //出库ID
            stockOut.SourceLineID = line.ID;  //出库行ID
            stockOut.StockOutDate = other.CreateDate;  //出库时间
            stockOut.StockOutType = other.StockOutType;  //出库类型
            stockOut.UnitID = line.UnitID;  //单位
            stockOut.WarehouseCode = line.WarehouseCode;  //仓库编码
            stockOut.WarehouseID = line.WarehouseID;  //仓库ID
            stockOut.StockInDate = line.StockInDate;  //入库时间
            stockOut.StockID = line.StockID;  //库存ID
            return stockOut;
        }

        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="otherOut"></param>
        /// <returns></returns>
        private ResultData<string> CheckValid(WOtherOut otherOut)
        {
            ResultData<string> rt = new ResultData<string>();
            foreach (var item in otherOut.Lines)
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
        private string BuilderErrorMessage(WOtherOutLine line, string msg)
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
