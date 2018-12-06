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
    /// 采购通知单
    /// </summary>
    public class WPurchaseNoticeController : BaseController
    {
        WPurchaseNoticeService pnService = new WPurchaseNoticeService();
        WPurchaseService pService = new WPurchaseService();

        // GET: WPurchaseNotice
        public ActionResult Index()
        {
            ComboxData cData = GetCombox("NoticeStatus");
            return View(cData);
        }

        #region 弹层选择

        /// <summary>
        /// 根据仓库获取货位
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchDeataiView()
        {
            ViewBag.ParentID = GetParam("ParentID");
            return View();
        }

        /// <summary>
        /// 查询明细
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ContentResult SearchDetailSc()
        {
            List<WhereItem> whereItems = GetWhereItems();
            long pageIndex = GetPageIndex();
            long pageSize = GetPageSize();
            var page = pnService.LinePageSearch(pageIndex, pageSize, whereItems);
            return ReturnResult(page);
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Init()
        {
            InitData idata = BaseInit(typeof(WPurchaseNotice), true);
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
            InitData idata = BaseInit(typeof(WPurchaseNoticeLine), false);
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
                    case "MaterialID":
                        item.foreign = new ForeignObject()
                        {
                            dataindex = item.dataIndex,
                            foreignKey = "ID",
                            foreignValue = "MaterialName",
                            table = "BMaterial"
                        };
                        item.foreign.returnDic.Add("MaterialCode", "MaterialCode");
                        item.foreign.returnDic.Add("UnitID", "UnitID");
                        item.foreign.returnDic.Add("UnitName", "UnitName");
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
            var page = pnService.PageSearch(pageIndex, pageSize, whereItems);
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
            WPurchaseNotice notice = GetDelete<WPurchaseNotice>();
            int result = pnService.RemovePurchaseNotice(notice);
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

            WPurchaseNotice notice = GetAdd<WPurchaseNotice>();

            notice.CreateBy = this.CurrentUser.ID;
            rt = pnService.AddPurchaseNotice(notice);
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

            WPurchaseNotice notice = GetUpdate<WPurchaseNotice>();

            notice.UpdateBy = this.CurrentUser.ID;
            rt = pnService.UpdatePurchaseNotice(notice);

            return ReturnResult(rt);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateView()
        {
            ViewBag.ID = GetParam("ID");
            if (GetParam<int>("NoticeStatus") == NoticeStatusEnum.New)
            {
                ViewBag.NoticeStatus = "true";
            }
            else
            {
                ViewBag.NoticeStatus = "false";
            }
            return View();
        }

        /// <summary>
        /// 下推
        /// </summary>
        /// <returns></returns>
        public ContentResult PushDown()
        {
            ResultData<string> rt = new ResultData<string>();

            //获取前台传会的删除ID
            WPurchaseNotice notice = GetParam<WPurchaseNotice>("app");
            rt = pService.AddPurchase(notice, CurrentUser.ID);
            return ReturnResult(rt);
        }

        /// <summary>
        /// 查询主表明细
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchSingle()
        {
            int Id = GetParam<int>("Id");
            var notice = pnService.GetWPurchaseNoticeById(Id);
            return ReturnResult(notice);
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
            var page = pnService.LinePageSearch(pageIndex, pageSize, parentId);
            return ReturnResult(page);
        }


        #endregion
    }
}