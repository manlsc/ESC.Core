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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.Extensions.Primitives;

namespace ESC.Web.Controllers
{
    public class AccountController : Controller
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
                    ////客户端保存用户编码 微软自带的认证太复杂，本质就是cookie进行加密和解密
                    //HttpContext.Response.Cookies.Append(CookieConst.ESC_USR_UID, user.UserCode, new CookieOptions()
                    //{
                    //    Expires = DateTimeOffset.Now.AddDays(7)
                    //});
                    var claims = new List<Claim>
                    {
                        new Claim("UserCode", user.UserCode),
                        new Claim("UserName", user.UserName),
                        new Claim("ID", user.ID.ToString()),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                        RedirectUri = "/Home/Index",
                        IsPersistent = true
                    };

                    HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties).Wait();

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

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdatePwd()
        {
            return View();
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Logout()
        {
            //HttpContext.Response.Cookies.Append(CookieConst.ESC_USR_UID, CurrentUser.UserCode, new CookieOptions()
            //{
            //    Expires = DateTimeOffset.Now.AddMinutes(-1)
            //});
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            return View("Index");
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
            //if (oldPwd != CurrentUser.Pwd)
            //{
            //    cr.Content = "原密码输入错误.";
            //}
            //else
            //{
            //    CurrentUser.Pwd = newPwd;
            //    CurrentUser.UpdateDate = DateTime.Now;
            //    if (CurrentUser.CreateDate.Year < 1900)
            //    {
            //        CurrentUser.CreateDate = DateTime.Now;
            //    }
            //    new SUserService().UpdateUser(CurrentUser);
            //    cr.Content = "ok";
            //}

            return cr;
        }

        /// <summary>
        /// 获取回传参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string GetParam(string name)
        {
            StringValues value = StringValues.Empty;
            if (Request.Method == "POST")
            {
                value = Request.Form[name];
            }
            if (value == StringValues.Empty)
            {
                value = Request.Query[name];
            }
            if (value == StringValues.Empty)
            {
                return "";
            }
            return value.FirstOrDefault();
        }

    }
}