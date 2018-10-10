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
    /// 调拨入库通知单 +
    /// </summary>
    public class WTransferInNoticeController : BaseController
    {
        WTransferInNoticeService inService = new WTransferInNoticeService();
        WTransferInService iService = new WTransferInService();

        // GET: WTransferInNotice
        public ActionResult Index()
        {
            return View();
        }

        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Init()
        {
            InitData idata = BaseInit(typeof(WTransferInNotice), true);
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
                    case "NoticeStatus":
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
            InitData idata = BaseInit(typeof(WTransferInNoticeLine), false);
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
                            foreignKey = "ID",
                            foreignValue = "LocationDesc",
                            table = "BLocation"
                        };
                        item.foreign.returnDic.Add("PositionCode", "LocationCode");
                        break;
                }
            }
            return ReturnResult(idata);
        }
        #endregion

        #region 增删改查

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult Update()
        {
            ResultData<string> rt = new ResultData<string>();

            WTransferInNotice inNotice = GetUpdate<WTransferInNotice>();

            rt = iService.AddTransferIn(inNotice, this.CurrentUser.ID);
            return ReturnResult(rt);
        }

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
        public ActionResult Detail()
        {
            ViewBag.ID = GetParam("ID");
            return View();
        }

        /// <summary>
        /// 查询主表明细
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchSingle()
        {
            int Id = GetParam<int>("Id");
            var otherOut = inService.GetWTransferInNoticeById(Id);
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
            var page = inService.LinePageSearch(pageIndex, pageSize, parentId);
            return ReturnResult(page);
        }

        #endregion
    }
}