using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace ESC.Core
{
    /// <summary>
    /// ASP.NET 默认缓存
    /// </summary>
    public class Cache : ICache
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetCache(string key)
        {
            return HttpRuntime.Cache[key];
        }

        /// <summary>
        /// 设置数据缓冲
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetCache(string key, object value)
        {
            HttpRuntime.Cache.Insert(key, value);
        }

        /// <summary>
        /// 设置缓存数据
        /// isAbsolute:false 缓存在hour时间段之后，无论是否访问都过期
        /// isAbsolute:true 缓存在hour时间内有访问，则不会过期
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        /// <param name="hour"></param>
        /// <param name="isAbsolute"></param>
        public void SetCache(string key, object value, int hour, bool isAbsolute)
        {
            if (isAbsolute)
            {
                HttpRuntime.Cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(hour));
            }
            else
            {
                HttpRuntime.Cache.Insert(key, value, null, DateTime.Now.AddDays(hour), System.Web.Caching.Cache.NoSlidingExpiration);
            }
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        public void RemoveCache(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void ClearCache()
        {
            IDictionaryEnumerator enumerator= HttpRuntime.Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
               HttpRuntime.Cache.Remove(enumerator.Key.ToString());
            }
        }
    }
}
