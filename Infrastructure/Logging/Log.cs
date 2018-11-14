using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESC.Core
{
    public static class Log
    {

        public static void Info(string message)
        {
            CheckHelper.IsNotEmpty(message, "message");

            GetLog().Info(message);
        }


        public static void Info(string format, params object[] args)
        {
            CheckHelper.IsNotEmpty(format, "format");

            GetLog().Info(Format(format, args));
        }

        public static void Warning(string message)
        {
            CheckHelper.IsNotEmpty(message, "message");

            GetLog().Warning(message);
        }

        public static void Warning(string format, params object[] args)
        {
            CheckHelper.IsNotEmpty(format, "format");

            GetLog().Warning(Format(format, args));
        }

        public static void Error(string message)
        {
            CheckHelper.IsNotEmpty(message, "message");

            GetLog().Error(message);
        }

        public static void Error(string format, params object[] args)
        {
            CheckHelper.IsNotEmpty(format, "format");

            GetLog().Error(Format(format, args));
        }

        public static void Exception(Exception exception)
        {
            CheckHelper.IsNotNull(exception, "exception");

            GetLog().Exception(exception);
        }

        private static ILog GetLog()
        {
            return IoC.Resolve<ILog>();
        }

        private static string Format(string format, params object[] args)
        {
            CheckHelper.IsNotEmpty(format, "format");

            return string.Format(format, args);
        }
    }
}
