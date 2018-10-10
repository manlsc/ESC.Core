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
    /// 其他入库 +
    /// </summary>
    public class WOtherInController : BaseController
    {
        WOtherInService inService = new WOtherInService();

        // GET: WOtherIn
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
            InitData idata = BaseInit(typeof(WOtherIn), true);
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
                    case "StockInType":
                    case "StockStatus":
                        item.combox = GetCombox(item.dataIndex).items;
                        break;
                }
            }
            idata.Tabs.Add(new BUITab()
            {
                foreign = "ID",
                name = "明细",
                table = "WOtherInLine"
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
            InitData idata = BaseInit(typeof(WOtherInLine), false);
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
                        item.foreign.returnDic.Add("WarehouseID", "ParentID");
                        item.foreign.returnDic.Add("WarehouseCode", "ParentCode");
                        item.foreign.returnDic.Add("WarehouseName", "ParentName");
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

        /// <summary>
        /// 获取下拉列表
        /// </summary>
        /// <returns></returns>
        protected new ComboxData GetCombox(string field)
        {
            ComboxData cdata = new ComboxData();
            switch (field)
            {
                case "StockInType":
                    cdata.items.Add(new ComboxDataItem()
                    {
                        value = StockInEnum.OtherIn,
                        text = "其他入库"
                    });
                    cdata.items.Add(new ComboxDataItem()
                    {
                        value = StockInEnum.InvProfit,
                        text = "盘盈入库"
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
            var page = inService.PageSearch(pageIndex, pageSize, whereItems);
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
            WOtherIn otherIn = GetDelete<WOtherIn>();
            int result = inService.RemoveOtherIn(otherIn);
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

            WOtherIn otherIn = GetAdd<WOtherIn>();

            otherIn.CreateBy = this.CurrentUser.ID;
            rt = inService.AddOtherIn(otherIn);
            return ReturnResult(rt);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>                   
        public ActionResult AddView()
        {
            List<ComboxDataItem> items = GetCombox("StockInType").items;
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

            WOtherIn otherIn = GetUpdate<WOtherIn>();

            otherIn.UpdateBy = this.CurrentUser.ID;
            otherIn.UpdateDate = DateTime.Now;
            rt = inService.UpdateOtherIn(otherIn);

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

            WOtherIn otherIn = GetParam<WOtherIn>("app");

            otherIn.UpdateBy = this.CurrentUser.ID;
            otherIn.UpdateDate = DateTime.Now;
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
            var otherIn = inService.GetOtherInById(Id);
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