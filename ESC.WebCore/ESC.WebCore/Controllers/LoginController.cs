using ESC.Core;
using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESC.Web.Controllers
{
    public class LoginController : Controller
    {

        protected SUserService uService = new SUserService();

        // GET: Login
        public ActionResult Index()
        {
            //NLoging.GetInstance().Error("123");
            return View();
        }
       
        /// <summary>
        /// 登陆
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult Login()
        {
            ContentResult cr = new ContentResult();
            string code = Request.Form["UserName"];
            string pwd = Request.Form["Password"];
            SUser user = uService.GetUserByCode(code).FirstOrDefault();

            if (user != null)
            {
                if (user.Pwd.Equals(pwd.Trim()))
                {
                    //客户端保存用户编码
                    //CookieHelper.AddOrUpdateCookie(this.HttpContext, "USRID", user.UserCode, 1);
                    cr.Content = "true" + user.UserName;
                }
                else
                {
                    cr.Content = "用户密码错误!";
                }
            }
            else
            {
                cr.Content = "用户不存在!";
            }
            return cr;
        }

    }
}