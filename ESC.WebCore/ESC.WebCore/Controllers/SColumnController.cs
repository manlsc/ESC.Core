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
    /// 列 +
    /// </summary>
    public class SColumnController : BaseController
    {
        SColumnService uService = new SColumnService();

        // GET: SColumn
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
            InitData idata = BaseInit(typeof(SColumn), true);
            foreach (var item in idata.Columns)
            {
                switch (item.dataIndex)
                {
                    case "Visible":
                    case "Required":
                    case "Disabled":
                        item.combox = GetCombox().items;
                        break;                    
                }
            }

            return ReturnResult(idata);
        }

        /// <summary>
        /// 获取下拉列表
        /// </summary>
        /// <returns></returns>
        private ComboxData GetCombox()
        {
            ComboxData cdata = new ComboxData();
            cdata.items.Add(new ComboxDataItem()
            {
                value = 0,
                text = "否"
            });
            cdata.items.Add(new ComboxDataItem()
            {
                value = 1,
                text = "是"
            });
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
            var page = uService.PageSearch(pageIndex, pageSize, whereItems);
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
            SColumn column = GetDelete<SColumn>();
            uService.RemoveColumn(column);
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

            SColumn u = GetAdd<SColumn>();
            uService.AddColumn(u);
            return ReturnResult(rt);
        }

        /// <summary>
        ///更新 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult Update()
        {
            ResultData<string> rt = new ResultData<string>();

            SColumn u = GetUpdate<SColumn>();
            uService.UpdateColumn(u);
            return ReturnResult(rt);
        }
        #endregion
    }
}