using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ESC.Core
{
    /// <summary>
    /// 验证帮助类
    /// </summary>
    public class CheckHelper
    {
        /// <summary>
        /// 非空验证
        /// </summary>
        /// <param name="argument">待验证字符串</param>
        /// <param name="argumentName">变量名称</param>
        public static void IsNotEmpty(string argument, string argumentName)
        {
            if (string.IsNullOrEmpty((argument ?? string.Empty).Trim()))
            {
                throw new ArgumentException(argumentName + " 不能为空.", argumentName);
            }
        }


        /// <summary>
        /// 长度验证
        /// </summary>
        /// <param name="argument">字符串</param>
        /// <param name="length">最大长度</param>
        /// <param name="argumentName"></param>
        public static void IsNotOutOfLength(string argument, int length, string argumentName)
        {
            if (argument.Trim().Length > length)
            {
                throw new ArgumentException(argumentName + " 长度不能大于 " + length + " 字节.", argumentName);
            }
        }

        /// <summary>
        /// 非空验证
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotNull(object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// 非负数验证
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotNegative(int argument, string argumentName)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// 非负数验证
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotNegative(long argument, string argumentName)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// 非负数验证
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotNegative(float argument, string argumentName)
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// 验证时间是否合法
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotInvalidDate(DateTime argument, string argumentName)
        {
            if (!argument.IsValid())
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// 如果小于当前时间抛出异常
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotInPast(DateTime argument, string argumentName)
        {
            if (argument < DateTime.UtcNow)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// 如果大于当前时间抛出异常
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotInFuture(DateTime argument, string argumentName)
        {
            if (argument > DateTime.UtcNow)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// 非空集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotEmpty<T>(ICollection<T> argument, string argumentName)
        {
            IsNotNull(argument, argumentName);

            if (argument.Count == 0)
            {
                throw new ArgumentException("集合不能为空.", argumentName);
            }
        }

        /// <summary>
        /// 区间验证
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="argumentName"></param>
        public static void IsNotOutOfRange(int argument, int min, int max, string argumentName)
        {
            if ((argument < min) || (argument > max))
            {
                throw new ArgumentOutOfRangeException(argumentName, argumentName + "必须在" + min + "-" + max + "之间");
            }
        }
    }
}
