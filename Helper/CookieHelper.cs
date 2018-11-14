using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ESC.Core
{
    /// <summary>
    /// cookie帮助类
    /// </summary>
    public class CookieHelper
    {
        /// <summary>
        /// 添加或更新cookie
        /// </summary>
        /// <param name="hcb"></param>
        /// <param name="key">主键</param>
        /// <param name="value">主键值</param>
        /// <param name="day">过期天数</param>
        public static void AddOrUpdateCookie(HttpContextBase hcb, string key, string value, int day)
        {
            if (hcb.Request.Cookies[key] == null)
            {
                HttpCookie aCookie = new HttpCookie(key);
                aCookie.Value = value;
                aCookie.Expires = DateTime.Now.AddDays(day);
                hcb.Response.Cookies.Add(aCookie);
            }
            else
            {
                hcb.Request.Cookies[key].Value = value;
                hcb.Response.Cookies.Add(hcb.Request.Cookies[key]);
            }
        }

        /// <summary>
        /// 添加或更新cookie
        /// </summary>
        /// <param name="hcb"></param>
        /// <param name="key">主键</param>
        /// <param name="value">子健值对</param>
        /// <param name="day">过期天数</param>
        public static void AddOrUpdateCookie(HttpContextBase hcb, string key, Dictionary<string, string> values, int day)
        {
            if (hcb.Request.Cookies[key] != null)
            {
                var cValues = hcb.Request.Cookies[key].Values;
                foreach (KeyValuePair<string, string> kvp in values)
                {
                    if (cValues.AllKeys.Contains(kvp.Key))
                    {
                        cValues[kvp.Key] = kvp.Value;
                    }
                    else
                    {
                        cValues.Add(kvp.Key, kvp.Value);
                    }
                }
                hcb.Response.Cookies.Add(hcb.Request.Cookies[key]);
            }
            else
            {
                HttpCookie aCookie = new HttpCookie(key);
                foreach (KeyValuePair<string, string> kvp in values)
                {
                    aCookie.Values.Add(kvp.Key, kvp.Value);
                }
                aCookie.Expires = DateTime.Now.AddDays(day);
                hcb.Response.Cookies.Add(aCookie);
            }
        }

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="hcb"></param>
        /// <param name="key">删除主键</param>
        public static void DeleteCookie(HttpContextBase hcb, string key)
        {
            if (hcb.Request.Cookies[key] != null)
            {
                string cookieName = hcb.Request.Cookies[key].Name;
                HttpCookie aCookie = new HttpCookie(cookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                hcb.Response.Cookies.Add(aCookie);
            }
        }

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="hcb"></param>
        /// <param name="key">主键</param>
        /// <param name="subkey">子键</param>
        public static void DeleteCookie(HttpContextBase hcb, string key, string subkey)
        {
            if (hcb.Request.Cookies[key] != null)
            {
                HttpCookie aCookie = hcb.Request.Cookies[key];
                aCookie.Values.Remove(subkey);
                aCookie.Expires = DateTime.Now.AddDays(1);
                hcb.Response.Cookies.Add(aCookie);
            }
        }

        /// <summary>
        /// 获取cookie的值
        /// </summary>
        /// <param name="hcb"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCookie(HttpContextBase hcb, string key)
        {
            if (hcb.Request.Cookies[key] != null)
            {
                return hcb.Request.Cookies[key].Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="hcb"></param>
        /// <param name="key">主键</param>
        /// <returns>主键的子集</returns>
        public static Dictionary<string, string> GetCookies(HttpContextBase hcb, string key)
        {
            if (hcb.Request.Cookies[key] != null)
            {
                Dictionary<string, string> dics = new Dictionary<string, string>();
                foreach (string name in hcb.Request.Cookies[key].Values.AllKeys)
                {
                    dics.Add(name, hcb.Request.Cookies[key].Values[name]);
                }
                return dics;
            }
            return null;
        }
    }
}
