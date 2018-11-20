using ESC.Core;
using ESC.Infrastructure;
using ESC.Infrastructure.DomainObjects;
using ESC.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace ESC.Web.Controllers
{
    public class LoginController : Controller
    {
        protected SUserService uService = new SUserService();

        // GET: Login
        public ActionResult Index()
        {
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
                    //客户端保存用户编码 微软自带的认证太复杂，本质就是cookie进行加密和解密
                    HttpContext.Response.Cookies.Append(CookieConst.ESC_USR_UID, user.UserCode, new CookieOptions()
                    {
                        Expires = DateTimeOffset.Now.AddDays(7)
                    });
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