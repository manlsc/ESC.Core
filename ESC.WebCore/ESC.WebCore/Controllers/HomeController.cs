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

namespace ESC.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {

            //DatabaseContext db = new DatabaseContext("ConnectionString");
            //List<ShortName> list = db.Connection.Query<ShortName>("  SELECT T2.CityId,T1.[ShortName]+T2.FirstAlphabet AS FirstAlphabet  FROM[CarServiceDev].[dbo].[CarNumberProvince] T1  LEFT JOIN[CarServiceDev].[dbo].[CarNumberCity] T2 ON T2.DicId = T1.DicID").ToList();

            //string str = SerializeObject(list);
            //string str = "{\"error_code\":0,\"resultcode\":\"200\",	\"reason\":\"查询成功\",\"result\":{\"province\":\"HB\",\"city\":\"HB_HD\",	\"hphm\":\"冀DHL327\",\"hpzl\":\"02\",\"lists\":[{\"date\":\"2013-12-2911:57:29\",\"area\":\"316省道53KM+200M\",\"act\":\"16362:驾驶中型以上载客载货汽车、校车、危险物品运输车辆以外的其他机动车在高速公路以外的道路上行驶超过规定时速20%以上未达50%的\",\"code\":\"\",\"fen\":\"6\",\"money\":\"100\",\"handled\":\"0\",\"archiveno\":\"320294Y000276124\",\"wzcity\":\"广东深圳\"}]	}}";
            //JObject jobj = JObject.Parse(str);
            //foreach (JProperty jpr in jobj.Properties())
            //{
            //    switch (jpr.Name)
            //    {
            //        case "error_code":
            //            int errCode = jobj["error_code"].Value<int>();  //0代表成功

            //            break;
            //        case "reason":

            //            break;
            //        case "result":
            //            if (jobj["result"].Type == JTokenType.Null)
            //            {
            //                continue;
            //            }
            //            else if (jobj["result"].HasValues)
            //            {
            //                JToken token = jobj["result"]["lists"];
            //                if (token.Type == JTokenType.Array)
            //                {
            //                    JArray array = token as JArray;
            //                    if (array.Count > 0)
            //                    {
            //                        for (int i = 0; i < array.Count; i++)
            //                        {
            //                            JToken jt = array[i];
            //                            int fen = jt.Value<int>("fen");
            //                            int money = jt.Value<int>("money");
            //                            string date = jt.Value<string>("date");
            //                            string act = jt.Value<string>("act");
            //                            string area = jt.Value<string>("area");
            //                            string wzcity = jt.Value<string>("wzcity");
            //                        }
            //                    }
            //                }
            //            }
            //            break;
            //    }
            //}
            ViewBag.CurrentUser = this.CurrentUser;
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
        /// 退出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ContentResult Logout()
        {
            //CookieHelper.DeleteCookie(this.HttpContext, "USRID");
            //将当前用户缓存12小时
            //if (CurrentUser != null)
            //{
            //    new Cache().RemoveCache(this.CurrentUser.UserCode);
            //}

            ContentResult cr = new ContentResult() { Content = "true" };
            return cr;
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult UpdatePassword()
        {
            ContentResult cr = new ContentResult();
            string newPwd = GetParam("newPwd");
            string oldPwd = GetParam("oldPwd");
            if (oldPwd != CurrentUser.Pwd)
            {
                cr.Content = "原密码输入错误.";
            }
            else
            {
                CurrentUser.Pwd = newPwd;
                CurrentUser.UpdateDate = DateTime.Now;
                if (CurrentUser.CreateDate.Year < 1900)
                {
                    CurrentUser.CreateDate = DateTime.Now;
                }
                new SUserService().UpdateUser(CurrentUser);
                cr.Content = "ok";
            }

            return cr;
        }
        
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
        public ActionResult InnerError()
        {
            return View();
        }
    }
}