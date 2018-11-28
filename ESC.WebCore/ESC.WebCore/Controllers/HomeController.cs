using ESC.Core;
using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Service;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data;
using Microsoft.AspNetCore.Diagnostics;

namespace ESC.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="superUser"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        public ContentResult GetResources()
        {
            SPermissionService pService = new SPermissionService();
            //查询当前用户的所有页面权限
            List<SResource> resouses = pService.GetRPermissionByUser(CurrentUser.ID, CurrentUser.SuperUser > 0);
            List<SResource> parents = resouses.Select(r => r).Where(t => t.ParentID == 0).ToList();

            List<TreeNode> tns = new List<TreeNode>();
            foreach (SResource parent in parents)
            {
                TreeNode tn = new TreeNode();
                tn.id = "tree" + parent.ID;
                tn.text = parent.ResourceDesc;
                tn.href = parent.ResourceURL;
                tn.name = parent.ResourceName;
                tn.children.AddRange(GetTree(parent, resouses));
                tns.Add(tn);
            }
            return ReturnResult(tns);
        }

        /// <summary>
        /// 递归遍历
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="resouses"></param>
        /// <param name="tn"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public List<TreeNode> GetTree(SResource parent, List<SResource> resouses)
        {
            List<TreeNode> tns = new List<TreeNode>();

            List<SResource> children = resouses.Select(r => r).Where(t => t.ParentID == parent.ID).ToList();
            foreach (SResource child in children)
            {
                TreeNode tn = new TreeNode();
                tn.id = "tree" + child.ID;
                tn.text = child.ResourceDesc;
                tn.href = child.ResourceURL;
                tn.name = child.ResourceName;
                tn.children = GetTree(child, resouses);
                tns.Add(tn);
            }
            return tns;
        }

        ///// <summary>
        ///// 文件上传
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public ContentResult FileUpload()
        //{
        //    for (int i = 0; i < Request.Files.Count; i++)
        //    {
        //       string fileFullName= FileSaveAS(Request.Files[i]);
        //     DataTable dt= new  NopiExcelManager().ExcelToDataTable(fileFullName);
        //    }
        //    return ReturnResult(new ResultData<string>());
        //}

        /// <summary>
        /// 未找到
        /// </summary>
        /// <returns></returns>
        public ActionResult NotFond()
        {
            return View();
        }

        /// <summary>
        /// 内部错误
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }
    }
}