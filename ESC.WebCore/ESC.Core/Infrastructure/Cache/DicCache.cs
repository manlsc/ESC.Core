using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESC.Core
{
    /// <summary>
    /// 数据字典缓存
    /// </summary>
    public class DicCache : ICache
    {
        static Dictionary<string, object> m_objects = new Dictionary<string, object>();
        static System.Threading.ReaderWriterLockSlim RWLock = new System.Threading.ReaderWriterLockSlim();

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetCache(string key)
        {
            RWLock.EnterReadLock();
            object idata;
            try
            {
                if (m_objects.TryGetValue(key, out idata))
                    return idata;
            }
            finally
            {
                RWLock.ExitReadLock();
            }

            return null;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetCache(string key, object value)
        {
            RWLock.EnterWriteLock();
            object idata;
            try
            {
                if (m_objects.TryGetValue(key, out idata))
                    return;

                m_objects.Add(key, value);
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        public void RemoveCache(string key)
        {
            RWLock.EnterWriteLock();
            try
            {
                m_objects.Remove(key);
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 暂时未实现
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="hour"></param>
        /// <param name="isAbsolute"></param>
        public void SetCache(string key, object value, int hour, bool isAbsolute)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void ClearCache()
        {
            RWLock.EnterWriteLock();
            try
            {
                m_objects.Clear();
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }
    }
}
