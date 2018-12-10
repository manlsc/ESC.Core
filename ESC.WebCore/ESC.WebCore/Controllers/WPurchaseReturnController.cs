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
    /// <summary>
    /// 采购退款 +
    /// </summary>
    public class WPurchaseReturnController : BaseController
    {
        WPurchaseReturnService prService = new WPurchaseReturnService();
        WPurchaseService pService = new WPurchaseService();

        // GET: WPurchaseReturn
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
            InitData idata = BaseInit(typeof(WPurchaseReturn), true);
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
                    case "WarehouseID":
                        item.foreign = new ForeignObject()
                        {
                            dataindex = item.dataIndex,
                            foreignKey = "ID",
                            foreignValue = "LocationDesc",
                            table = "BLocation"
                        };
                        item.foreign.returnDic.Add("WarehouseCode", "LocationCode");
                        break;
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
            InitData idata = BaseInit(typeof(WPurchaseReturnLine), false);
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
                    case "PositionID":
                        item.foreign = new ForeignObject()
                        {
                            dataindex = item.dataIndex,
                            foreignKey = "PositionID",
                            foreignValue = "PositionName",
                            table = "WStock"
                        };
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
            var page = prService.PageSearch(pageIndex, pageSize, whereItems);
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
            WPurchaseReturn purReturn = GetDelete<WPurchaseReturn>();
            int result = prService.RemovePurchaseReturn(purReturn);
            if (result < 1)
            {
                rt.status = -1;
                rt.message = "删除失败,只能删除新建单据.";
            }
            return ReturnResult(rt);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Approve()
        {
            ResultData<string> rt = new ResultData<string>();

            WPurchaseReturn purReturn = GetParam<WPurchaseReturn>("app");

            purReturn.UpdateBy = this.CurrentUser.ID;
            rt = prService.ApprovePurchaseReturn(purReturn);

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

            WPurchaseReturn purReturn = GetAdd<WPurchaseReturn>();

            purReturn.CreateBy = this.CurrentUser.ID;
            rt = prService.AddPurchaseReturn(purReturn);
            return ReturnResult(rt);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>                   
        public ActionResult AddView()
        {
            return View();
        }

        /// <summary>
        ///更新 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult Update()
        {
            ResultData<string> rt = new ResultData<string>();

            WPurchaseReturn purReturn = GetUpdate<WPurchaseReturn>();

            purReturn.UpdateBy = this.CurrentUser.ID;
            rt = prService.UpdatePurchaseReturn(purReturn);

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
        /// 查询主表明细
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchSingle()
        {
            int Id = GetParam<int>("Id");
            var purReturn = prService.GetWPurchaseReturnById(Id);
            return ReturnResult(purReturn);
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
            var page = prService.LinePageSearch(pageIndex, pageSize, parentId);
            return ReturnResult(page);
        }
        #endregion
    }
}