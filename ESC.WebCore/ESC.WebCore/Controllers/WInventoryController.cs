using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace ESC.Web.Controllers
{
    /// <summary>
    /// 盘点 +
    /// </summary>
    public class WInventoryController : BaseController
    {
        WInventoryService iService = new WInventoryService();

        // GET: WInventory
        public ActionResult Index()
        {
            ComboxData cData = GetCombox("InventoryStatus");
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
            InitData idata = BaseInit(typeof(WInventory), true);
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
                    case "InventoryStatus":
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
            InitData idata = BaseInit(typeof(WInventoryLine), false);
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
            var page = iService.PageSearch(pageIndex, pageSize, whereItems);
            return ReturnResult(page);
        }

        /// <summary>
        /// 添加盘亏
        /// </summary>
        /// <returns></returns>
        public ContentResult AddLoss()
        {
            ResultData<string> rt = new ResultData<string>();
            WInventory inv = GetUpdate<WInventory>();

            inv.CreateBy = this.CurrentUser.ID;
            rt = iService.AddLoss(inv);
            return ReturnResult(rt);
        }

        /// <summary>
        /// 添加盘盈
        /// </summary>
        /// <returns></returns>
        public ContentResult AddProfit()
        {
            ResultData<string> rt = new ResultData<string>();
            WInventory inv = GetUpdate<WInventory>();

            inv.CreateBy = this.CurrentUser.ID;
            rt = iService.AddProfit(inv);
            return ReturnResult(rt);
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
            WInventory Inv = GetDelete<WInventory>();
            rt= iService.RemoveInventory(Inv);
            return ReturnResult(rt);
        }


        #endregion

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult Add()
        {
            ResultData<string> rt = new ResultData<string>();

            List<WInventoryLine> lines = GetAdd<List<WInventoryLine>>();
            string code = GetParam("code");
            iService.AddInventory(lines, code, this.CurrentUser.ID);
            return ReturnResult(rt);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>                   
        public ActionResult AddView()
        {
            ViewBag.InventoryCode = iService.GetInventoryCode();
            return View();
        }

        /// <summary>
        /// 明细
        /// </summary>
        /// <returns></returns>
        public ActionResult DetailView()
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
            var otherIn = iService.GetInventoryById(Id);
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
            var page = iService.LinePageSearch(pageIndex, pageSize, parentId);
            return ReturnResult(page);
        }

    }
}