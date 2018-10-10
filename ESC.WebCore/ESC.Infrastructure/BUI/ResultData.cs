using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC.Infrastructure
{
    /// <summary>
    /// 返回对象
    /// </summary>
    public class ResultData<T>
    {
        public ResultData() {
            status = 0;
            message = "ok";
        }

        public ResultData(string msg)
        {
            message = msg;
        }

        /// <summary>
        /// 返回结果状态值， 成功返回0，其他值请查看下方返回码状态表。
        /// </summary>
        public int status { set; get; }

        /// <summary>
        /// 返回的结果
        /// </summary>
        public T result { set; get; }

        /// <summary>
        /// 返回描述信息,ok代表成功,否则错误描述
        /// </summary>
        public string message { set; get; }

        /// <summary>
        /// 成功方法
        /// </summary>
        /// <param name="data">返回对象</param>
        /// <returns></returns>
        public static ResultData<T> Success(T data)
        {
            ResultData<T> result = new ResultData<T>
            {
                status = 0,
                message = string.Empty,
                result = data
            };
            return result;
        }

        /// <summary>
        /// 成功方法
        /// </summary>
        /// <param name="_data">返回数据</param>
        /// <param name="msg">返回成功文本</param>
        /// <returns></returns>
        public static ResultData<T> Success(T data, string msg)
        {
            ResultData<T> result = new ResultData<T>
            {
                status = 0,
                message = msg,
                result = data
            };
            return result;
        }

        /// <summary>
        /// 成功方法
        /// </summary>
        /// <param name="msg">返回信息</param>
        /// <returns></returns>
        public static ResultData<T> Success(string msg)
        {
            ResultData<T> result = new ResultData<T>
            {
                status = 0,
                message = msg,
            };
            return result;
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        public static ResultData<T> Success()
        {
            ResultData<T> result = new ResultData<T>
            {
                status = 0,
            };
            return result;
        }
        /// <summary>
        /// 失败方法
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static ResultData<T> Error(string errMsg)
        {
            ResultData<T> result = new ResultData<T>
            {
                status = -1,
                message = errMsg
            };
            return result;
        }
    }
}
