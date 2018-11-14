using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESC.Core
{
    /// <summary>
    /// 对象扩展方法
    /// </summary>
    public static class CollectionExtension
    {
        /// <summary>
        /// 判断数组是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return (collection == null) || (collection.Count == 0);
        }
    }
}
