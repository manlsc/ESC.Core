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
    /// 调拨入库
    /// </summary>
    public class WTransferInController : BaseController
    {
        WTransferInService inService = new WTransferInService();

        // GET: WTransferIn
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
            InitData idata = BaseInit(typeof(WTransferIn), true);
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
                            table = "WTransferIn"
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
                        break;
                    case "StockStatus":
                        item.combox = GetCombox(item.dataIndex).items;
                        break;
                }
            }
            idata.Tabs.Add(new BUITab()
            {
                foreign = "ID",
                name = "明细",
                table = "WTransferInLine"
            });

            return ReturnResult(idata);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ForeignInit()
        {
            InitData idata = BaseInit(typeof(WTransferInLine), false);
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
                            table = "WTransferInLine"
                        };
                        break;
                    case "PositionID":
                        item.foreign = new ForeignObject()
                        {
                            dataindex = item.dataIndex,
                            foreignKey = "ID",
                            foreignValue = "LocationDesc",
                            table = "BLocation"
                        };
                        break;
                    case "MaterialID":
                        item.foreign = new ForeignObject()
                        {
                            dataindex = item.dataIndex,
                            foreignKey = "ID",
                            foreignValue = "MaterialName",
                            table = "BMaterial"
                        };
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
            var page = inService.PageSearch(pageIndex, pageSize, whereItems);
            return ReturnResult(page);
        }        

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateView()
        {
            ViewBag.ID = GetParam("ID");
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

            WTransferIn otherIn = GetParam<WTransferIn>("app");

            otherIn.UpdateBy = this.CurrentUser.ID;
            rt = inService.ApproveOtherIn(otherIn);

            return ReturnResult(rt);
        }

        /// <summary>
        /// 查询主表明细
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchSingle()
        {
            int Id = GetParam<int>("Id");
            var otherIn = inService.GetWTransferInById(Id);
            return ReturnResult(otherIn);
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
            var page = inService.LinePageSearch(pageIndex, pageSize, parentId);
            return ReturnResult(page);
        }
        #endregion
    }
}