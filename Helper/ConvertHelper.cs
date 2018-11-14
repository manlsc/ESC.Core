using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Core.Helper
{
    /// <summary>
    /// 转换帮助类
    /// </summary>
   public class ConvertHelper
    {
       /// <summary>
       /// 字符串转整型
       /// </summary>
       /// <param name="str"></param>
       /// <returns></returns>
       public static int ConvertToInt(string str)
       {
           int result = 0;
           int.TryParse(str, out result);
           return result;
       }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="str"></param>
       /// <returns></returns>
       public static decimal ConvertToDecimal(string str)
       {
           decimal result = 0;
           decimal.TryParse(str, out result);
           return result;
       }


       public static float ConvertToFloat(string str)
       {
           float result = 0;
           float.TryParse(str, out result);
           return result;
       }

       /// <summary>
       /// 将字符串转换成时间
       /// yyyy-MM-dd HH:mm:ss
       /// yyyy/MM/dd HH:mm:ss
       /// yyyyMMdd
       /// </summary>
       /// <param name="str"></param>
       /// <returns></returns>
       public static DateTime ConvertToDateTime(string str)
       {
           DateTime result = DateTime.Now;
           if (string.IsNullOrEmpty(str))
           {
               return result;
           }
           DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
           if (str.Length < 10)
           {      
               dtFormat.ShortDatePattern = "yyyyMMdd";
               DateTime.TryParse(str, dtFormat, DateTimeStyles.None, out result);
           
           }
           else if (str.Length == 10)
           {
               if (str.Contains("-"))
               {
                   dtFormat.ShortDatePattern = "yyyy-MM-dd";
                   DateTime.TryParse(str, dtFormat, DateTimeStyles.None, out result);
               }
               else
               {
                   dtFormat.ShortDatePattern = "yyyy/MM/dd";
                   DateTime.TryParse(str, dtFormat, DateTimeStyles.None, out result);
               }
           }
           else
           {
               if (str.Contains("-"))
               {
                   dtFormat.ShortDatePattern = "yyyy-MM-dd  HH:mm:ss";
                   DateTime.TryParse(str, dtFormat, DateTimeStyles.None, out result);
               }
               else
               {
                   dtFormat.ShortDatePattern = "yyyy/MM/dd  HH:mm:ss";
                   DateTime.TryParse(str, dtFormat, DateTimeStyles.None, out result);
               }
           }
           return result;
       }
    }
}
