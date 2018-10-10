using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace ESC.Web.Controllers
{
    public class WStockController : BaseController
    {
        WStockService sService = new WStockService();
        //
        // GET: /WStock/

        public ActionResult Index()
        {
            return View();
        }

        #region 弹层选择

        /// <summary>
        /// 查询库存
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchStockView()
        {
            return View();
        }

        /// <summary>
        /// 获取特定仓库的库存
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchWhStockView()
        {
            ViewBag.WarehouseID = GetParam("WarehouseID");
            return View();
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
            InitData idata = BaseInit(typeof(WStock), true);

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
            var page = sService.PageSearch(pageIndex, pageSize, whereItems);
            return ReturnResult(page);
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public FileResult DownLoad()
        {
            List<WhereItem> whereItems = GetWhereItems();
            DataTable dt = sService.GetStocks(whereItems);
            ResetColumnName(dt);
            return BaseDownloadFile(string.Format("库存{0}.xlsx",DateTime.Now.ToString("yyyyMMddhhmmss")), dt);
        }

        protected void ResetColumnName(DataTable dt)
        {
            SColumnService sService = new SColumnService();
            List<SColumn> cols=   sService.GetVisibleColumnsByTable("WStock");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].Caption = string.Empty;
                foreach (SColumn col in cols)
                {
                    if (string.IsNullOrEmpty(col.DisplayColumn))
                    {
                        if (col.ColumnName == dt.Columns[i].ColumnName)
                        {
                            dt.Columns[i].Caption = col.Title;
                            break;
                        }
                    }
                    else
                    {
                        if (col.DisplayColumn == dt.Columns[i].ColumnName)
                        {
                            dt.Columns[i].Caption = col.Title;
                            break;
                        }
                    }                  
                }
            }
        }
        #endregion
    }
}