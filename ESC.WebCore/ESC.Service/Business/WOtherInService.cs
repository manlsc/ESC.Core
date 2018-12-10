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
    /// 其他入库 +
    /// </summary>
    public class WOtherInService
    {
        WOtherInRepository oiRepository;
        WOtherInLineRepository oilRepository;
        SNumberRuleRepository nuRepository;

        public WOtherInService()
        {
            oiRepository = new WOtherInRepository();
            oilRepository = new WOtherInLineRepository(oiRepository.DbCondext);
            nuRepository = new SNumberRuleRepository(oiRepository.DbCondext);
        }

        /// <summary>
        /// 根据ID查询明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WOtherIn GetOtherInById(int id)
        {
            return oiRepository.GetOtherInById(id);
        }

        /// <summary>
        /// 插入新其他入库
        /// </summary>
        /// <param name="otherIn"></param>
        /// <returns></returns>
        public ResultData<string> AddOtherIn(WOtherIn otherIn)
        {
            ResultData<string> rData = CheckValid(otherIn);
            if (rData.status != 0)
            {
                return rData;
            }

            DatabaseContext dbContext = oiRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();
                //入库记录
                List<WStockIn> stockIns = new List<WStockIn>(otherIn.Lines.Count);

                //添加其他入库
                otherIn.CreateDate = DateTime.Now;
                otherIn.StockStatus = StockStatusEnum.New;
                otherIn.OtherInCode = nuRepository.GetNextNumber("QTRK");
                oiRepository.Insert(otherIn);

                foreach (var line in otherIn.Lines)
                {
                    line.ParentID = otherIn.ID;
                    line.CreateBy = otherIn.CreateBy;
                    line.CreateDate = DateTime.Now;
                    line.Batch = string.IsNullOrEmpty(line.Batch) ? "" : line.Batch;
                    oilRepository.Insert(line);
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
        /// 删除其他入库
        /// </summary>
        /// <param name="otherIns"></param>
        /// <returns></returns>
        public ResultData<string> RemoveOtherIns(List<WOtherIn> otherIns)
        {
            DatabaseContext db = oiRepository.DbCondext;
            ResultData<string> rData = new ResultData<string>();
            try
            {
                db.BeginTransaction();

                foreach (WOtherIn u in otherIns)
                {
                    int result = RemoveOtherIn(u);
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
        /// 删除其他入库
        /// </summary>
        /// <param name="otherIn"></param>
        /// <returns></returns>
        public int RemoveOtherIn(WOtherIn otherIn)
        {
            //删除其他入库
            int result = oiRepository.RemoveOtherInByStatus(otherIn.ID, StockStatusEnum.New);
            if (result > 0)
            {
                oilRepository.RemoveLinesByParentId(otherIn.ID);
            }
            return result;
        }

        /// <summary>
        /// 根据Id删除其他入库
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int RemoveOtherIn(int Id)
        {
            //删除其他入库
            int result = oiRepository.RemoveOtherInByStatus(Id, StockStatusEnum.New);
            if (result > 0)
            {
                oilRepository.RemoveLinesByParentId(Id);
            }
            return result;
        }

        /// <summary>
        /// 更新其他入库
        /// </summary>
        /// <param name="otherIn"></param>
        /// <returns></returns>
        public ResultData<string> UpdateOtherIn(WOtherIn otherIn)
        {
            ResultData<string> rData = CheckValid(otherIn);
            if (rData.status != 0)
            {
                return rData;
            }

            if (otherIn.StockStatus != StockStatusEnum.New)
            {
                rData.status = -1;
                rData.message = "单据已经审核,不能编辑.";
                return rData;
            }
           
            DatabaseContext db = oiRepository.DbCondext;
            try
            {
                db.BeginTransaction();

                otherIn.UpdateDate = DateTime.Now;
                oiRepository.Update(otherIn);
                foreach (var line in otherIn.Lines)
                {
                    switch (line.CURD)
                    {
                        case CurdEnum.Add:
                            line.ParentID = otherIn.ID;
                            line.CreateBy = otherIn.CreateBy;
                            line.CreateDate = DateTime.Now;
                            line.Batch = string.IsNullOrEmpty(line.Batch) ? "" : line.Batch;
                            oilRepository.Insert(line);
                            break;
                        case CurdEnum.Delete:
                            oilRepository.Delete(line);
                            break;
                        case CurdEnum.Update:
                            line.UpdateBy = otherIn.UpdateBy;
                            line.UpdateDate = DateTime.Now;
                            line.Batch = string.IsNullOrEmpty(line.Batch) ? "" : line.Batch;
                            oilRepository.Update(line);
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
        /// <param name="otherIn"></param>
        /// <returns></returns>
        public ResultData<string> ApproveOtherIn(WOtherIn otherIn)
        {
            if (otherIn.Lines.Count < 1)
            {
                otherIn.Lines = oilRepository.GetLinesByParentId(otherIn.ID);
            }

            ResultData<string> rData = new ResultData<string>();
            DatabaseContext dbContext = oiRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();
                //入库记录
                List<WStockIn> stockIns = new List<WStockIn>(otherIn.Lines.Count);

                //添加其他入库
                int result = oiRepository.ApproveOtherIn(otherIn);
                if (result < 1)
                {
                    rData.status = -1;
                    rData.message = "单据已经审核或删除.";
                    dbContext.AbortTransaction();
                    return rData;
                }

                foreach (var line in otherIn.Lines)
                {
                    stockIns.Add(CloneOtherIn(line, otherIn));
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

        #region 查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WOtherIn> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = oiRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WOtherIn> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return oiRepository.Pages(pageIndex, pageSize, sql, args);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WOtherInLine> LinePageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = oilRepository.GetSearchSql(whereItems);
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public Page<WOtherInLine> LinePageSearch(long pageIndex, long pageSize, int parentId)
        {
            //根据主表查询
            WhereItem item = new WhereItem()
            {
                condition = "=",
                datatype = "int",
                field = "ParentID",
                value = parentId.ToString()
            };
            string strSql = oilRepository.GetSearchSql(new List<WhereItem>() { item });
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WOtherInLine> LinePageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return oilRepository.Pages(pageIndex, pageSize, sql, args);
        }

        #endregion

        /// <summary>
        /// 其他入库复制入口记录
        /// </summary>
        /// <param name="line"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        private WStockIn CloneOtherIn(WOtherInLine line, WOtherIn other)
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
            stockIn.SourceCode = other.OtherInCode;
            stockIn.SourceID = line.ParentID;
            stockIn.SourceLineID = line.ID;
            stockIn.StockInDate = other.CreateDate;
            stockIn.StockInType = other.StockInType;
            stockIn.UnitID = line.UnitID;
            stockIn.WarehouseCode = line.WarehouseCode;
            stockIn.WarehouseID = line.WarehouseID;
            return stockIn;
        }

        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="otherIn"></param>
        /// <returns></returns>
        private ResultData<string> CheckValid(WOtherIn otherIn)
        {
            ResultData<string> rt = new ResultData<string>();
            foreach (var item in otherIn.Lines)
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
        private string BuilderErrorMessage(WOtherInLine line,string msg)
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
