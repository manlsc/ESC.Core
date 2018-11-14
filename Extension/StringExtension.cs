using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ESC.Core
{
    public static class StringExtension
    {
        private static readonly Regex WebUrlExpression = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex EmailExpression = new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex StripHTMLExpression = new Regex("<\\S[^><]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private static readonly char[] IllegalUrlCharacters = new[] { ';', '/', '\\', '?', ':', '@', '&', '=', '+', '$', ',', '<', '>', '#', '%', '.', '!', '*', '\'', '"', '(', ')', '[', ']', '{', '}', '|', '^', '`', '~', '–', '‘', '’', '“', '”', '»', '«' };

        /// <summary>
        /// 是否合法的URL
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsWebUrl(this string target)
        {
            return !string.IsNullOrEmpty(target) && WebUrlExpression.IsMatch(target);
        }

        /// <summary>
        /// 是否合法的邮箱
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsEmail(this string target)
        {
            return !string.IsNullOrEmpty(target) && EmailExpression.IsMatch(target);
        }

        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string UrlEncode(this string target)
        {
            return HttpUtility.UrlEncode(target);
        }

        /// <summary>
        /// URL解码
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string UrlDecode(this string target)
        {
            return HttpUtility.UrlDecode(target);
        }


        /// <summary>
        /// 将字符串最小限度地转换为 HTML 编码的字符串。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string AttributeEncode(this string target)
        {
            return HttpUtility.HtmlAttributeEncode(target);
        }

        /// <summary>
        /// 将字符串转换为 HTML 编码的字符串。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string target)
        {
            return HttpUtility.HtmlEncode(target);
        }

        /// <summary>
        /// 将已经为 HTTP 传输进行过 HTML 编码的字符串转换为已解码的字符串。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string target)
        {
            return HttpUtility.HtmlDecode(target);
        }

        /// <summary>
        /// 将字符串base64
        /// 一些特殊字符串在url传递时会出现特殊字符需要转码
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToBase64(this string target) {
            byte[] bts = Encoding.UTF8.GetBytes(target);
            return Convert.ToBase64String(bts);
        }

        /// <summary>
        /// 将base64字符串解码
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string FromBase64(this string target)
        {
            byte[] bts = Convert.FromBase64String(target);
            return Encoding.UTF8.GetString(bts);
        }

        /// <summary>
        /// 超出固定长度则用省略号代替
        /// </summary>
        /// <param name="target"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string OutToEllipsis(this string target,int length)
        {
            if (target.Length > length)
            {
                return target.Substring(0, length) + "...";
            }
            return target;
        }
    }
}
