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
    /// 单号生成规则 +
    /// </summary>
    public class SNumberRuleController : BaseController
    {
        SNumberRuleService uService = new SNumberRuleService();
        //
        // GET: /SNumberRule/

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
            InitData idata = BaseInit(typeof(SNumberRule), true);
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
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult Add()
        {
            ResultData<string> rt = new ResultData<string>();

            SNumberRule u = GetAdd<SNumberRule>();

            //验证业务是否存在
            if (uService.IsNotExits(u, true))
            {
                u.CreateBy = this.CurrentUser.ID;
                u.CreateDate = DateTime.Now;
                u.UpdateDate = DateTime.Now;
                u.BusinessType = u.BusinessType.ToUpper();
                u.Prefix = u.Prefix.ToUpper();

                uService.AddSNumberRule(u);
            }
            else
            {
                rt.status = -1;
                rt.message = "业务编码已经存在.";
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

            SNumberRule u = GetUpdate<SNumberRule>();

            //验证业务是否存在
            if (uService.IsNotExits(u, false))
            {
                u.UpdateDate = DateTime.Now;
                u.UpdateBy = this.CurrentUser.ID;
                uService.UpdateSNumberRule(u);
            }
            else
            {
                rt.status = -1;
                rt.message = "业务编码已经存在.";
            }
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
            SNumberRule ce = GetDelete<SNumberRule>();
            bool result = uService.DeleteSNumberRule(ce);
            if (result==false)
            {
                rt.message = "序列已经产生单据,不能删除.";
                rt.status = -1;
            }

            return ReturnResult(rt);
        }
        #endregion
    }
}