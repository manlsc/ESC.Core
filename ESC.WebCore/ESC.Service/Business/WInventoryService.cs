using ESC.Core;
using ESC.Core.Helper;
using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Enums;
using ESC.Infrastructure.Repository;
using Dapper.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESC.Service
{
    public class WInventoryService
    {
        protected WInventoryRepository iRepository;
        protected WInventoryLineRepository ilRepository;
        protected BLocationRepository locRepository;
        protected SNumberRuleRepository nuRepository;
        protected WOtherOutRepository ooRepository;
        protected WOtherOutLineRepository oolRepository;
        protected WOtherInRepository oiRepositroy;
        protected WOtherInLineRepository oilRepository;

        public WInventoryService()
        {
            iRepository = new WInventoryRepository();
            ilRepository = new WInventoryLineRepository(iRepository.DbCondext);
            locRepository = new BLocationRepository(iRepository.DbCondext);
            nuRepository = new SNumberRuleRepository(iRepository.DbCondext);
            ooRepository = new WOtherOutRepository(iRepository.DbCondext);
            oolRepository = new WOtherOutLineRepository(iRepository.DbCondext);
            oiRepositroy = new WOtherInRepository(iRepository.DbCondext);
            oilRepository = new WOtherInLineRepository(iRepository.DbCondext);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="inv"></param>
        public int AddInventory(List<WInventoryLine> lines, string code, int createBy)
        {
            WInventory Inv = iRepository.GetInventoryByCode(code);
            if (Inv == null)
            {
                Inv = new WInventory();
                Inv.CreateBy = createBy;
                Inv.CreateDate = DateTime.Now;
                Inv.InventoryCode = code;
                Inv.InventoryStatus = 1;
                iRepository.Insert(Inv);
            }
            List<WInventoryLine> stockLines = ilRepository.GetInventoryLinesByParent(Inv.ID);
            foreach (WInventoryLine item in lines)
            {
                bool isNotExit = true;
                foreach (WInventoryLine line in stockLines)
                {
                    if (line.StockID == item.ID)
                    {
                        isNotExit = false;
                        line.InventoryDiff = item.InventoryCount - item.StockCount;
                        ilRepository.Update(line);
                        break;
                    }
                }
                if (isNotExit)
                {
                    item.StockID = item.ID;  //库存ID
                    item.ParentID = Inv.ID;
                    item.InventoryDiff = item.InventoryCount - item.StockCount;
                    ilRepository.Insert(item);
                }
            }

            return Inv.ID;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WInventory GetInventoryById(int id)
        {
            return iRepository.GetInventoryById(id);
        }

        /// <summary>
        /// 添加盘亏出库
        /// </summary>
        /// <param name="inv"></param>
        /// <returns></returns>
        public ResultData<string> AddLoss(WInventory inv)
        {
            ResultData<string> rData = new ResultData<string>();
            if (inv.InventoryStatus == InventoryStatusEnum.Out)
            {
                rData.status = -1;
                rData.message = "单据已经盘亏.";
                return rData;
            }
            else if (inv.InventoryStatus == InventoryStatusEnum.Complete)
            {
                rData.status = -1;
                rData.message = "单据已经完成.";
                return rData;
            }

            if (ilRepository.HasNoLoss(inv.ID))
            {
                if (inv.InventoryStatus == InventoryStatusEnum.In)
                {
                    iRepository.UpdateLossStatus(inv.ID, 0, "", InventoryStatusEnum.Complete);
                }
                else
                {
                    iRepository.UpdateLossStatus(inv.ID, 0, "", InventoryStatusEnum.Out);
                }
                rData.status = -1;
                rData.message = "单据没有盘亏记录.";
                return rData;
            }

            DatabaseContext dbContext = ooRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();

                WOtherOut otherOut = new WOtherOut();
                otherOut.CreateBy = inv.CreateBy;
                otherOut.CreateDate = DateTime.Now;
                otherOut.OtherOutCode = nuRepository.GetNextNumber("QTCK");
                otherOut.StockOutType = StockOutEnum.InvShortages;
                otherOut.StockStatus = StockStatusEnum.New;

                //添加主表
                ooRepository.Insert(otherOut);

                //更新为盘点
                if (inv.InventoryStatus == InventoryStatusEnum.In)
                {
                    iRepository.UpdateLossStatus(inv.ID, otherOut.ID, otherOut.OtherOutCode, InventoryStatusEnum.Complete);
                }
                else if (inv.InventoryStatus == InventoryStatusEnum.Complete)
                {
                    rData.status = -1;
                    rData.message = "单据已经完成.";
                    return rData;
                }
                else
                {
                    iRepository.UpdateLossStatus(inv.ID, otherOut.ID, otherOut.OtherOutCode, InventoryStatusEnum.Out);
                }

                //添加明细
                oolRepository.AddOtherLinesByInv(otherOut, inv.ID);

                //返回插入的ID
                rData.result = otherOut.ID.ToString();

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
        /// 添加盘盈入库
        /// </summary>
        /// <param name="inv"></param>
        /// <returns></returns>
        public ResultData<string> AddProfit(WInventory inv)
        {
            ResultData<string> rData = new ResultData<string>();

            if (inv.InventoryStatus == InventoryStatusEnum.In)
            {
                rData.status = -1;
                rData.message = "单据已经盘盈.";
                return rData;
            }
            else if (inv.InventoryStatus == InventoryStatusEnum.Complete)
            {
                rData.status = -1;
                rData.message = "单据已经完成.";
                return rData;
            }

            if (ilRepository.HasNoProfit(inv.ID))
            {
                if (inv.InventoryStatus == InventoryStatusEnum.Out)
                {
                    iRepository.UpdateProfitStatus(inv.ID, 0, "", InventoryStatusEnum.Complete);
                }
                else
                {
                    iRepository.UpdateProfitStatus(inv.ID, 0, "", InventoryStatusEnum.In);
                }
                rData.status = -1;
                rData.message = "单据没有盘盈记录.";
                return rData;
            }

            DatabaseContext dbContext = ooRepository.DbCondext;
            try
            {
                dbContext.BeginTransaction();

                WOtherIn otherIn = new WOtherIn();
                otherIn.CreateBy = inv.CreateBy;
                otherIn.CreateDate = DateTime.Now;
                otherIn.OtherInCode = nuRepository.GetNextNumber("QTRK");
                otherIn.StockInType = StockInEnum.InvProfit;
                otherIn.StockStatus = StockStatusEnum.New;

                //添加主表
                oiRepositroy.Insert(otherIn);

                //添加明细
                oilRepository.AddOtherLinesByInv(otherIn, inv.ID);

                //更新为盘点
                if (inv.InventoryStatus == InventoryStatusEnum.Out)
                {
                    iRepository.UpdateProfitStatus(inv.ID, otherIn.ID, otherIn.OtherInCode, InventoryStatusEnum.Complete);
                }
                else
                {
                    iRepository.UpdateProfitStatus(inv.ID, otherIn.ID, otherIn.OtherInCode, InventoryStatusEnum.In);
                }

                //返回插入的ID
                rData.result = otherIn.ID.ToString();

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
        /// 获取盘点单号
        /// </summary>
        /// <returns></returns>
        public string GetInventoryCode()
        {
            return nuRepository.GetNextNumber("KCPD");
        }

        public ResultData<string> RemoveInventory(WInventory inv)
        {
            ResultData<string> rData = new ResultData<string>();
            if (inv.InventoryStatus == InventoryStatusEnum.Out)
            {
                rData.status = -1;
                rData.message = "单据已经盘亏.";
                return rData;
            }
            else if (inv.InventoryStatus == InventoryStatusEnum.In)
            {
                rData.status = -1;
                rData.message = "单据已经盘盈.";
                return rData;
            }
            else if (inv.InventoryStatus == InventoryStatusEnum.Complete)
            {
                rData.status = -1;
                rData.message = "单据已经完成.";
                return rData;
            }

            iRepository.Delete(inv.ID);
            ilRepository.RemoveLinesByParent(inv.ID);

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
        public Page<WInventory> PageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = iRepository.GetSearchSql(whereItems);
            return PageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WInventory> PageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return iRepository.Pages(pageIndex, pageSize, sql, args);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereItems"></param>
        /// <returns></returns>
        public Page<WInventoryLine> LinePageSearch(long pageIndex, long pageSize, List<WhereItem> whereItems)
        {
            string strSql = ilRepository.GetSearchSql(whereItems);
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public Page<WInventoryLine> LinePageSearch(long pageIndex, long pageSize, int parentId)
        {
            //根据主表查询
            WhereItem item = new WhereItem()
            {
                condition = "=",
                datatype = "int",
                field = "ParentID",
                value = parentId.ToString()
            };
            string strSql = ilRepository.GetSearchSql(new List<WhereItem>() { item });
            return LinePageSearch(pageIndex, pageSize, strSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Page<WInventoryLine> LinePageSearch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            return ilRepository.Pages(pageIndex, pageSize, sql, args);
        }

        #endregion
    }
}
