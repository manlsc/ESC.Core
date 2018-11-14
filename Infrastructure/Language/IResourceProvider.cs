using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Core
{
    /// <summary>
    /// 语言接口
    /// </summary>
    public interface IResourceProvider
    {
        /// <summary>
        /// 根据资源名和语言获取资源内容
        /// </summary>
        /// <param name="name"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        string GetResource(string name, string culture);

        /// <summary>
        /// 清空多语言资源缓存，保证下次请求时重新加载
        /// </summary>
        void ClearResourceCache();
    }
}
