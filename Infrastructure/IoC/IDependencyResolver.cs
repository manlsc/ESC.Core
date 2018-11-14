using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Core
{
    /// <summary>
    /// 注入接口
    /// </summary>
    public interface IDependencyResolver : IDisposable
    {
        /// <summary>
        /// 注册实例(单例模式)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        void Register<T>(T instance) where T : class;

        /// <summary>
        /// 注册接口和实体映射
        /// </summary>
        /// <typeparam name="TFrom">接口</typeparam>
        /// <typeparam name="TTo">实体</typeparam>
        void RegisterType<TFrom, TTo>() where TTo : TFrom;
       

        /// <summary>
        /// 解析实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>();
      

        /// <summary>
        /// 获取所有的注册实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> ResolveAll<T>();
    }
}
