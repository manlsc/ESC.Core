using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Core
{
    /// <summary>
    /// 后台运行任务
    /// <remarks>一般是作业任务</remarks>
    /// </summary>
    public interface IBackgroundTask
    {
        /// <summary>
        /// 是否运行
        /// </summary>
        bool IsRunning
        {
            get;
        }

        /// <summary>
        /// 开启
        /// </summary>
        void Start();

        /// <summary>
        /// 终止
        /// </summary>
        void Stop();
    }
}
