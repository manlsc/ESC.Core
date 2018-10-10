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
    /// 单位管理 +
    /// </summary>
    public class BUnitController : BaseController
    {
        BUnitService uService = new BUnitService();

        // GET: BUnit
        public ActionResult Index()
        {
            List<ComboxDataItem> items = GetCombox("UnitType").items;
            return View(items);
        }
           

        #region 弹层选择

        /// <summary>
        /// 查询单位
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchUnitView()
        {
            List<ComboxDataItem> items = GetCombox("UnitType").items;
            return View(items);
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
            InitData idata = BaseInit(typeof(BUnit), true);
            foreach (var item in idata.Columns)
            {
                switch (item.dataIndex)
                {                  
                    case "OrgID":
                        item.foreign = new ForeignObject()
                        {
                            dataindex = item.dataIndex,
                            foreignKey = "id",
                            foreignValue = "text",
                            table = "Organization"
                        };
                        break;
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
                    case "UnitType":
                        item.combox = GetCombox("UnitType").items;
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
            BUnit unit = GetDelete<BUnit>();
            uService.RemoveUnit(unit);
            rt.message = "删除成功.";
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

            BUnit u = GetAdd<BUnit>();

            //非空验证和属性格式验证
            string msg = uService.CheckEmpty(u);
            if (!string.IsNullOrEmpty(msg))
            {
                rt.message = msg;
                rt.status = -1;
                return ReturnResult(rt);
            }

            //验证用户是否存在
            if (uService.IsNotExits(u, true))
            {
                u.CreateBy = this.CurrentUser.ID;
                u.CreateDate = DateTime.Now;
                u.UpdateDate = DateTime.Now;
              
                uService.AddUnit(u);
            }
            else
            {
                rt.status = -1;
                rt.message = "单位编码已经存在.";
            }
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

            BUnit u = GetUpdate<BUnit>();

            //非空验证和属性格式验证
            string msg = uService.CheckEmpty(u);
            if (!string.IsNullOrEmpty(msg))
            {
                rt.message = msg;
                rt.status = -1;
                return ReturnResult(rt);
            }

            //验证用户是否存在
            if (uService.IsNotExits(u, false))
            {
                u.UpdateDate = DateTime.Now;
                u.UpdateBy = this.CurrentUser.ID;
                uService.UpdateUnit(u);
            }
            else
            {
                rt.status = -1;
                rt.message = "单位编码已经存在.";
            }
            return ReturnResult(rt);
        }

        /// <summary>
        /// 根据编码模糊查询
        /// </summary>
        /// <returns></returns>
        public ContentResult GetUnitCodeName()
        {
            string codeName = GetParam("words");
            ResultData<List<BUnit>> rt = new ResultData<List<BUnit>>();
            rt.result = uService.GetUnitCodeName(codeName);
            return ReturnResult(rt);
        }
        #endregion
    }
}