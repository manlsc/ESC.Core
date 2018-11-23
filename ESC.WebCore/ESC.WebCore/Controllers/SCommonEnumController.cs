using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Text;

namespace ESC.Web.Controllers
{
    /// <summary>
    /// 枚举 +
    /// </summary>
    public class SCommonEnumController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public SCommonEnumController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

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
            string str = ceService.CreateEnumClass();
            byte[] arrBytes = Encoding.UTF8.GetBytes(str);
            return File(arrBytes, "application/text", "EnumConfig.cs");
        }

        #endregion
    }
}