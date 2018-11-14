using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Core
{
    /// <summary>
    /// 引导类   
    /// </summary>
    public static class Bootstrapper
    {
        static Bootstrapper()
        {
            try
            {
                IoC.InitializeWith(new DependencyResolverFactory());
            }
            catch (ArgumentException ex)
            {
                // 配置文件缺失
                Log.Exception(ex);
            }
        }

        public static void Run()
        {
            foreach (IBootstrapperTask item in IoC.ResolveAll<IBootstrapperTask>())
            {
                item.Execute();
            }
        }
    }
}
