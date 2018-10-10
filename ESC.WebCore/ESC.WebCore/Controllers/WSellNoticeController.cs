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
    /// 销售通知单 +
    /// </summary>
    public class WSellNoticeController : BaseController
    {
        WSellNoticeService snService = new WSellNoticeService();
        WSellService sService = new WSellService();

        // GET: WSellNotice
        public ActionResult Index()
        {
            ComboxData cData = GetCombox("NoticeStatus");
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
            InitData idata = BaseInit(typeof(WSellNotice), true);
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
            InitData idata = BaseInit(typeof(WSellNoticeLine), false);
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
            var page = snService.PageSearch(pageIndex, pageSize, whereItems);
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
            WSellNotice notice = GetDelete<WSellNotice>();
            int result = snService.RemoveSellNotice(notice);
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

            WSellNotice notice = GetAdd<WSellNotice>();

            notice.CreateBy = this.CurrentUser.ID;
            rt = snService.AddSellNotice(notice);
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

            WSellNotice notice = GetUpdate<WSellNotice>();

            notice.UpdateBy = this.CurrentUser.ID;
            snService.UpdateSellNotice(notice);

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
            WSellNotice notice = GetParam<WSellNotice>("app");
            rt = sService.AddSell(notice, CurrentUser.ID);
            return ReturnResult(rt);
        }

        /// <summary>
        /// 查询主表明细
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchSingle()
        {
            int Id = GetParam<int>("Id");
            var notice = snService.GetWSellNoticeById(Id);
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
            var page = snService.LinePageSearch(pageIndex, pageSize, parentId);
            return ReturnResult(page);
        }
        #endregion
    }
}