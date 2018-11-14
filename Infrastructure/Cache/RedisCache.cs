using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Core
{
    /// <summary>
    /// 暂时不实现
    /// </summary>
   public class RedisCache:ICache
    {
        public object GetCache(string key)
        {
            throw new NotImplementedException();
        }

        public void SetCache(string key, object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveCache(string key)
        {
            throw new NotImplementedException();
        }

        public void SetCache(string key, object value, int hour, bool isAbsolute)
        {
            throw new NotImplementedException();
        }


        public void ClearCache()
        {
            throw new NotImplementedException();
        }
    }
}
