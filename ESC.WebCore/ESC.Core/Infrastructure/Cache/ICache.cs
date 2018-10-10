using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESC.Core
{
    /// <summary>
    /// 缓存接口
    /// </summary>
   public interface ICache
    {
       /// <summary>
       /// 获取缓存
       /// </summary>
       /// <param name="key"></param>
       /// <returns></returns>
       object GetCache(string key);

       /// <summary>
       /// 添加缓存
       /// </summary>
       /// <param name="key"></param>
       /// <param name="value"></param>
       void SetCache(string key, object value);

       /// <summary>
       /// 删除缓存
       /// </summary>
       /// <param name="key"></param>
       void RemoveCache(string key);

       /// <summary>
       /// 设置缓存数据
       /// isAbsolute:false 缓存在hour时间段之后，无论是否访问都过期
       /// isAbsolute:true 缓存在hour时间内有访问，则不会过期
       /// </summary>
       /// <param name="CacheKey"></param>
       /// <param name="objObject"></param>
       /// <param name="hour"></param>
       /// <param name="isAbsolute"></param>
       void SetCache(string key, object value, int hour, bool isAbsolute);

       /// <summary>
       /// 清空缓存
       /// </summary>
       void ClearCache();
   }
}
