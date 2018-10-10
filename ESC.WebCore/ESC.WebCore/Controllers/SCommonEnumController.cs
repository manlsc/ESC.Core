using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace ESC.Web.Controllers
{
    public class SCommonEnumController : BaseController
    {

        // GET: SCommonEnum
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
            InitData idata = BaseInit(typeof(SCommonEnum), true);
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
            var page = ceService.PageSearch(pageIndex, pageSize, whereItems);
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
            SCommonEnum ce = GetDelete<SCommonEnum>();
            ceService.RemoveCommonEnum(ce.ID);

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

            SCommonEnum ce = GetAdd<SCommonEnum>();
            ceService.AddCommonEnum(ce);

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

            SCommonEnum ce = GetUpdate<SCommonEnum>();
            ceService.UpdateCommonEnum(ce);
            return ReturnResult(rt);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public FileResult GetEnumClass()
        {
            //string filePath = Path.Combine(HttpContext.Server.MapPath("/App_Data"), "EnumConfig.cs");
            //ceService.CreateEnumClass(filePath);
            //return File(filePath, "application/text", "EnumConfig.cs");
            return null;
        }

        #endregion
    }
}