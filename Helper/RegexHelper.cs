using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ESC.Core.Helper
{
    /// <summary>
    /// 一般正则表达式帮助类
    /// </summary>
    public class RegexHelper
    {
        private static string email = @"^[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?$";
        private static string idNumber18 = @"^(\d{6})(\d{4})(\d{2})(\d{2})(\d{3})([0-9]|X)$";
        private static string idNumber15 = @"^(\d{6})(\d{4})(\d{2})(\d{3})$";
        private static string tel = @"^(\d{3}-\d{8})|(\d{4}-\d{7})$";
        private static string fax = @"^(\d{3}-\d{8})|(\d{4}-\d{7})$";
        private static string mobile = @"^(\d{3})(\d{4})(\d{4})$";

        /// <summary>
        /// 是否邮件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(string input)
        {
            return Regex.IsMatch(input, email);
        }

        /// <summary>
        /// 是否身份证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIdNumber(string input)
        {
            if (input.Length == 15)
            {
                return Regex.IsMatch(input, idNumber15);
            }
            else if (input.Length == 18)
            {
                return Regex.IsMatch(input, idNumber18);
            }
            return false;
        }

        /// <summary>
        /// 是否固定电话
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsTel(string input)
        {
            return Regex.IsMatch(input, tel);
        }

        /// <summary>
        /// 是否传真
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsFax(string input)
        {
            return Regex.IsMatch(input, fax);
        }

        /// <summary>
        /// 是否手机
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMobile(string input)
        {
            return Regex.IsMatch(input, mobile);
        }      
    }
}
