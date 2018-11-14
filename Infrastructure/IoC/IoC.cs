using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Core
{
    public static class IoC
    {
        private static IDependencyResolver _resolver;

        /// <summary>
        /// 注册注入工厂
        /// </summary>
        /// <param name="factory"></param>
        public static void InitializeWith(IDependencyResolverFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            _resolver = factory.CreateInstance();
        }

        /// <summary>
        /// 注册实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public static void Register<T>(T instance) where T : class
        {
            _resolver.Register<T>(instance);
        }

        /// <summary>
        /// 注册实体接口映射
        /// </summary>
        /// <typeparam name="TFrom">接口</typeparam>
        /// <typeparam name="TTo">实例</typeparam>
        public static void RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            _resolver.RegisterType<TFrom, TTo>();
        }

        /// <summary>
        /// 解析实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return _resolver.Resolve<T>();
        }

        /// <summary>
        /// 解析所有注册类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> ResolveAll<T>()
        {
            //解析容器中所有
            return _resolver.ResolveAll<T>();

        }

        /// <summary>
        /// 重置释放
        /// </summary>
        public static void Reset()
        {
            if (_resolver != null)
            {
                _resolver.Dispose();
            }
        }
    }
}
