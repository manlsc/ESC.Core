using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Core
{
    /// <summary>
    /// 网站开启默认执行的任务
    /// <remarks>一般初始化操作</remarks>
    /// </summary>
    interface IBootstrapperTask
    {
        void Execute();
    }
}
