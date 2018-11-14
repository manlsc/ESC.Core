using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Core
{
    /// <summary>
    /// 注入工厂接口
    /// </summary>
    public interface IDependencyResolverFactory
    {
        IDependencyResolver CreateInstance();
    }
}
