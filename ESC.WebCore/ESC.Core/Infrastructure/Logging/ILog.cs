using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESC.Core
{
    public interface ILog
    {
        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message"></param>
        void Info(string message);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message"></param>
        void Warning(string message);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message"></param>
        void Error(string message);

        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="exception"></param>
        void Exception(Exception exception);
    }
}
