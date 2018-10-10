using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Infrastructure.Enums;
using ESC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace ESC.Web.Controllers
{
    public class WOtherOutController : BaseController
    {
        WOtherOutService outService = new WOtherOutService();
        // GET: WOtherOut
        public ActionResult Index()
        {
            ComboxData cData = GetCombox("StockStatus");
            return View(cData);
        }

        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Init()
        {
            InitData idata = BaseInit(typeof(WOtherOut), true);
            foreach (var item in idata.Columns)
            {
                switch (item.dataIndex)
                {
                    case "CreateBy":
                    case "UpdateBy":
                        item.foreign = new ForeignObject()
                        {
                            dataindex = item.dataIndex,
                            foreignKey = "ID",
                            foreignValue = "UserName",
                            table = "SUser"
                        };
                        break;
                    case "BusinessPartnerID":
                        item.foreign = new ForeignObject()
                        {
                            dataindex = item.dataIndex,
                            foreignKey = "ID",
                            foreignValue = "BusinessPartnerName",
                            table = "BBusinessPartner"
                        };
                        break;
                    case "StockOutType":
                    case "StockStatus":
                        item.combox = GetCombox(item.dataIndex).items;
                        break;
                }
            }

            return ReturnResult(idata);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ForeignInit()
        {
            InitData idata = BaseInit(typeof(WOtherOutLine), false);
            foreach (var item in idata.Columns)
            {
                switch (item.dataIndex)
                {
                    case "CreateBy":
                    case "UpdateBy":
                        item.foreign = new ForeignObject()
                        {
                            dataindex = item.dataIndex,
                            foreignKey = "ID",
                            foreignValue = "UserName",
                            table = "WOtherOutLine"
                        };
                        break;
                    case "WarehouseID":
                        item.foreign = new ForeignObject()
                        {
                            dataindex = item.dataIndex,
                            foreignKey = "WarehouseID",
                            foreignValue = "WarehouseName",
                            table = "WStock"
                        };
                        item.foreign.returnDic.Add("WarehouseCode", "WarehouseCode");
                        item.foreign.returnDic.Add("PositionID", "PositionID");
                        item.foreign.returnDic.Add("PositionCode", "PositionCode");
                        item.foreign.returnDic.Add("MaterialID", "MaterialID");
                        item.foreign.returnDic.Add("MaterialCode", "MaterialCode");
                        item.foreign.returnDic.Add("UnitID", "UnitID");
                        item.foreign.returnDic.Add("Batch", "Batch");
                        item.foreign.returnDic.Add("OwnerCode", "OwnerCode");
                        item.foreign.returnDic.Add("Factory", "Factory");
                        item.foreign.returnDic.Add("StockID", "ID");
                        item.foreign.returnDic.Add("MaterialName", "MaterialName");
                        item.foreign.returnDic.Add("PositionName", "PositionName");
                        item.foreign.returnDic.Add("UnitName", "UnitName");
                        item.foreign.returnDic.Add("StockInDate", "StockInDate");
                        item.foreign.returnDic.Add("OutCount", "UnLimitCount"); 
                        break;                   
                }
            }
            return ReturnResult(idata);
        }

        /// <summary>
        /// 获取下拉列表
        /// </summary>
        /// <returns></returns>
        private new ComboxData GetCombox(string field)
        {
            ComboxData cdata = new ComboxData();
            switch (field)
            {
                case "StockOutType":
                    cdata.items.Add(new ComboxDataItem()
                    {
                        value = StockOutEnum.OtherOut,
                        text = "其他出库"
                    });
                    cdata.items.Add(new ComboxDataItem()
                    {
                        value = StockOutEnum.InvShortages,
                        text = "盘盈出库"
                    });
                    break;
                case "StockStatus":
                    cdata.items.Add(new ComboxDataItem()
                    {
                        value = StockStatusEnum.New,
                        text = "新建"
                    });
                    cdata.items.Add(new ComboxDataItem()
                    {
                        value = StockStatusEnum.Approve,
                        text = "审核"
                    });
                    break;
            }

            return cdata;
        }

        #endregion

        #region 增删改查

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ContentResult Search()
        {
            List<WhereItem> whereItems = GetWhereItems();
            long pageIndex = GetPageIndex();
            long pageSize = GetPageSize();
            var page = outService.PageSearch(pageIndex, pageSize, whereItems);
            return ReturnResult(page);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult Delete()
        {
            ResultData<string> rt = new ResultData<string>();

            //获取前台传会的删除ID
            WOtherOut otherOut = GetDelete<WOtherOut>();
            int result = outService.RemoveOtherOut(otherOut);
            if (result < 1)
            {
                rt.status = -1;
                rt.message = "删除失败,只能删除新建单据.";
            }
            return ReturnResult(rt);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult Add()
        {
            ResultData<string> rt = new ResultData<string>();

            WOtherOut otherOut = GetAdd<WOtherOut>();

            otherOut.CreateBy = this.CurrentUser.ID;
            rt = outService.AddOtherOut(otherOut);
            return ReturnResult(rt);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>                   
        public ActionResult AddView()
        {
            List<ComboxDataItem> items = GetCombox("StockOutType").items;
            return View(items);
        }

        /// <summary>
        ///更新 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult Update()
        {
            ResultData<string> rt = new ResultData<string>();

            WOtherOut otherOut = GetUpdate<WOtherOut>();

            otherOut.UpdateBy = this.CurrentUser.ID;
            rt= outService.UpdateOtherOut(otherOut);

            return ReturnResult(rt);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateView()
        {
            ViewBag.ID = GetParam("ID");
            if (GetParam<int>("StockStatus") == StockStatusEnum.Approve)
            {
                ViewBag.StockStatus = "false";
            }
            else
            {
                ViewBag.StockStatus = "true";
            }
            return View();
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Approve()
        {
            ResultData<string> rt = new ResultData<string>();

            WOtherOut otherOut = GetParam<WOtherOut>("app");

            otherOut.UpdateBy = this.CurrentUser.ID;
            rt = outService.ApproveOtherOut(otherOut);

            return ReturnResult(rt);
        }

        /// <summary>
        /// 查询主表明细
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchSingle()
        {
            int Id = GetParam<int>("Id");
            var otherOut = outService.GetOtherOutById(Id);
            return ReturnResult(otherOut);
        }

        /// <summary>
        /// 查询明细
        /// </summary>
        /// <returns></returns>
        public ContentResult SearchDetail()
        {
            int parentId = GetParentID();
            long pageIndex = GetPageIndex();
            long pageSize = GetPageSize();
            var page = outService.LinePageSearch(pageIndex, pageSize, parentId);
            return ReturnResult(page);
        }
        #endregion
    }
}