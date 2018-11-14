using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Core
{
    public abstract class BaseResourceProvider : IResourceProvider
    {
        // 缓存列表
        private static Dictionary<string, ResourceEntry> languages = null;
        private static object lockResources = new object();

        public BaseResourceProvider()
        {
            Cache = true; //默认缓存
        }

        /// <summary>
        /// 是否缓存
        /// </summary>
        protected bool Cache { get; set; } 

        /// <summary>
        /// 返回特定语言的编码
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <param name="culture">语言编码</param>
        /// <returns>资源</returns>
        public string GetResource(string name, string culture)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Resource name cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(culture))
                throw new ArgumentException("Culture name cannot be null or empty.");

            // 本地化
            culture = culture.ToLowerInvariant();

            if (Cache && languages == null)
            {
                //一次性将所有资源添加到缓存
                lock (lockResources)
                {
                    if (languages == null)
                    {
                        languages = ReadResources().ToDictionary(r => r.Name);
                    }
                }
            }
                        
            ResourceEntry entry = null;
            if (Cache)
            {
                if (languages != null)
                {
                  entry= languages[name];
                }
            }
            if (entry == null)
            {
                entry = ReadResource(name, culture);
            }
            return entry == null ? "" : entry[culture];
        }


        /// <summary>
        /// 读取所有资源
        /// </summary>
        /// <returns>资源列表</returns>
        protected abstract IList<ResourceEntry> ReadResources();


        /// <summary>
        /// 读取特定语言的实体(如果存在添加到缓存)
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <param name="culture">文化编码</param>
        /// <returns>语言实体</returns>
        protected abstract ResourceEntry ReadResource(string name, string culture);

        /// <summary>
        /// 添加资源缓存
        /// </summary>
        /// <param name="entry">资源</param>
        protected void AddResourceCache(ResourceEntry entry)
        {
            lock (lockResources)
            {
                if (languages == null)
                {
                    languages = new Dictionary<string, ResourceEntry>();
                }
                else
                {
                    languages.Add(entry.Name, entry);
                }
            } 
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public virtual void ClearResourceCache()
        {
            languages = null;
        }
    }
}
