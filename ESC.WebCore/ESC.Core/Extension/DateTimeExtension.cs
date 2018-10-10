using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESC.Core
{
   public static class DateTimeExtension
    {
        private static readonly DateTime MinDate = new DateTime(1900, 1, 1);
        private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

       /// <summary>
       /// 验证时间是否合法  1900-1-1  9999-23-39
       /// </summary>
       /// <param name="target"></param>
       /// <returns></returns>
        public static bool IsValid(this DateTime target)
        {
            return (target >= MinDate) && (target <= MaxDate);
        }
    }
}
