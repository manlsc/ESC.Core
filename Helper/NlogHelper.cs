using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace ESC.Core.Helper
{
    public class NlogHelper
    {
        private NlogHelper()
        {

        }
        private static Logger log = LogManager.GetCurrentClassLogger();


        public static void Debug(string message)
        {
            log.Debug(message);
        }

        public static void Debug(Exception exception, string myMessage)
        {
            log.Debug(exception, myMessage);
        }
        public static void Error(string message)
        {
            log.Error(message);
        }

        public static void Error(Exception exception, string webMethodName, string myMessage)
        {
            log.Error(exception, string.Format("调用服务{0}时发生错误:{1}", webMethodName, myMessage));
        }

        public static void Info(string message)
        {
            log.Info(message);
        }

        public static void Warn(string message)
        {
            log.Warn(message);
        }
    }
}
