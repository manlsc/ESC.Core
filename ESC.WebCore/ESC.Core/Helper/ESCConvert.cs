using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Core
{
    /// <summary>
    /// 转换帮助类
    /// </summary>
    public class ESCConvert
    {
        /// <summary>
        /// 字符串转整型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ConvertToInt(string str)
        {
            int.TryParse(str, out int result);
            return result;
        }

        /// <summary>
        /// 字符串转双精度
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ConvertToDecimal(string str)
        {
            decimal.TryParse(str, out decimal result);
            return result;
        }

        /// <summary>
        /// 字符串转单精度
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ConvertToFloat(string str)
        {
            float.TryParse(str, out float result);
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
            DateTime result = new DateTime(1900, 1, 1);
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
            else if (str.Length >= 19)
            {
                str = str.Substring(0, 19);
                if (str.Contains("T"))
                {
                    dtFormat.ShortDatePattern = "yyyy-MM-ddTHH:mm:ss";
                    DateTime.TryParse(str, dtFormat, DateTimeStyles.None, out result);
                }
                else if (str.Contains("-"))
                {
                    dtFormat.ShortDatePattern = "yyyy-MM-dd HH:mm:ss";
                    DateTime.TryParse(str, dtFormat, DateTimeStyles.None, out result);
                }
                else
                {
                    dtFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
                    DateTime.TryParse(str, dtFormat, DateTimeStyles.None, out result);
                }
            }
            return result;
        }

        /// <summary>
        /// 转换成int
        /// </summary>
        /// <param name="data"></param>
        public static int ConvertToInt(object data)
        {
            int result = 0;
            if (data == null || data == DBNull.Value)
                return result;

            if (data is int)
                return (int)data;

            int.TryParse(data.ToString(), out result);
            return result;
        }

        /// <summary>
        /// 转换成decimal
        /// </summary>
        /// <param name="data"></param>
        public static decimal ConvertToDecimal(object data)
        {
            if (data == null || data == DBNull.Value)
                return 0;

            if (data is decimal)
                return (decimal)data;

            decimal.TryParse(data.ToString(), out decimal result);
            return result;
        }

        /// <summary>
        /// 转换成float
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static float ConvertToFloat(object data)
        {
            if (data == null || data == DBNull.Value)
                return 0;

            if (data is float)
                return (float)data;

            float.TryParse(data.ToString(), out float result);
            return result;
        }

        /// <summary>
        /// 转换成long
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static long ConvertToLong(object data)
        {
            if (data == null || data == DBNull.Value)
                return 0;

            if (data is long)
                return (long)data;

            long.TryParse(data.ToString(), out long result);
            return result;
        }


        /// <summary>
        /// 转换成datetime
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(object data)
        {
            DateTime result = new DateTime(1900, 1, 1);
            if (data == null || data == DBNull.Value)
                return result;

            if (data is DateTime)
                return (DateTime)data;

            return ConvertToDateTime(data.ToString());
        }
    }
}
