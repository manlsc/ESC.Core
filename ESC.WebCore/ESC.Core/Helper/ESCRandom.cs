using System;
using System.Collections.Generic;
using System.Text;

namespace ESC.Core.Helper
{
    /// <summary>
    /// 随机数操作
    /// </summary>
    public class ESCRandom
    {
        /// <summary>
        /// 随机数
        /// </summary>
        private readonly System.Random _random;

        /// <summary>
        /// 初始化随机数
        /// </summary>
        public ESCRandom()
        {
            _random = new System.Random();
        }

        /// <summary>
        /// 获取指定范围的随机整数
        /// </summary>
        /// <param name="max">最大值</param>
        public int Next(int max)
        {
            return _random.Next(max);
        }

        /// <summary>
        /// 获取指定范围的随机整数，该范围包括最小值，但不包括最大值
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public int Next(int min, int max)
        {
            return _random.Next(min, max);
        }

        /// <summary>
        /// 获取随机码
        /// 字母和数组组合
        /// </summary>
        /// <param name="num">长度</param>
        /// <returns></returns>
        public string RandomCode(int num)
        {
            string str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string code = "";
            int i; for (i = 0; i < num; i++)
            {
                code += str.Substring(_random.Next(0, str.Length), 1);
            }
            return code;
        }

        /// <summary>
        /// 获取随机码
        /// 只包含数组
        /// </summary>
        /// <param name="num">长度</param>
        /// <returns></returns>
        public string RandomCodeNum(int num)
        {
            string code = "";
            int i; for (i = 0; i < num; i++)
            {
                code += _random.Next(0, 10);
            }
            return code;
        }
    }
}
