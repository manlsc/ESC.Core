using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Xml;
using System.Threading;

namespace ESC.Core
{
    /// <summary>
    /// web请求
    /// </summary>
    public class HttpWebGetRequest
    {
        /// <summary>
        /// 获取web请求
        /// </summary>
        /// <param name="requestUriString">请求地址</param>
        /// <param name="parmData">参数信息</param>
        /// <param name="methodType">请求类型</param>
        /// <param name="contentType">HTTP头信息</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static string GetRequest(string requestUriString, string parmData, string methodType,string contentType, string userName, string password,string cookie)
        {
            string htmlContent = "";
            HttpWebRequest request;

            try
            {
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                request = WebRequest.Create(requestUriString) as HttpWebRequest;

                //如果用户名不为空,则添加身份验证
                if (!string.IsNullOrEmpty(userName))
                {
                    request.Credentials = new NetworkCredential(userName, password);
                    request.PreAuthenticate = true;
                }
                else
                {
                    request.Credentials = CredentialCache.DefaultCredentials;
                }

                //请求方法
                request.Method = string.IsNullOrEmpty(methodType) ? "POST" : methodType;

                request.CookieContainer = new CookieContainer();
                //if (!string.IsNullOrEmpty(cookie))
                //{
                //    request.CookieContainer.Add()
                //}

                //请求类型
                request.ContentType = string.IsNullOrEmpty(contentType) ? "application/x-www-form-urlencoded" : contentType;

                //是否允许重定向
                request.AllowAutoRedirect = true;


                //设置请求字符长度
                byte[] buffer = encoding.GetBytes(parmData);
                request.ContentLength = buffer.Length;

                //将请求信息添加到请求
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(buffer, 0, buffer.Length);

                reqStream.Close();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //获取请求的返回信息
                Stream resSteam = response.GetResponseStream();
                StreamReader sr = new StreamReader(resSteam);
                htmlContent = sr.ReadToEnd();

                resSteam.Close();
            }
            catch (WebException wex)
            {
                Log.Exception(wex);
            }
            catch (InvalidOperationException ioex)
            {
                Log.Exception(ioex);
            }
            finally
            {
                request = null;
            }

            return htmlContent;
        }

        /// <summary>
        /// 调用webservice
        /// </summary>
        /// <param name="requestUriString">地址</param>
        /// <param name="methodName">方法</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="dicData">传递内容</param>
        /// <param name="isLower">是否1.2版本</param>
        /// <returns></returns>
        public static string GetWebService(string requestUriString, string methodName, string userName, string password, Dictionary<string, string> dicData,bool isLower) {
            string htmlContent = "";
            HttpWebRequest request;

            try
            {
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                request = WebRequest.Create(requestUriString) as HttpWebRequest;

                //如果用户名不为空,则添加身份验证
                if (!string.IsNullOrEmpty(userName))
                {
                    request.Credentials = new NetworkCredential(userName, password);
                    request.PreAuthenticate = true;
                }
                else
                {
                    request.Credentials = CredentialCache.DefaultCredentials;
                }

                //请求方法
                request.Method = "POST";

                //请求类型
                if (isLower)
                {
                    request.ContentType = "text/xml; charset=utf-8";
                    request.Headers.Add("SOAPAction", "http://tempuri.org/" + methodName);
                }
                else
                {
                    request.ContentType = "application/soap+xml; charset=utf-8";
                }
                //是否允许重定向
                request.AllowAutoRedirect = true;


                //设置请求字符长度
                byte[] buffer = CreateSoapXml(methodName, dicData,isLower);
                request.ContentLength = buffer.Length;

                //将请求信息添加到请求
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(buffer, 0, buffer.Length);

                reqStream.Close();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //获取请求的返回信息
                Stream resSteam = response.GetResponseStream();
                StreamReader sr = new StreamReader(resSteam);
                htmlContent = sr.ReadToEnd();

                resSteam.Close();
            }
            catch (WebException wex)
            {
               Log.Exception(wex);
            }
            catch (InvalidOperationException ioex) {
                Log.Exception(ioex);
            }
            finally
            {
                request = null;
            }

            return htmlContent;
        }

        /// <summary>
        /// 创建soap格式xml
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="dicData"></param>
        /// <returns></returns>
        private static byte[] CreateSoapXml(string methodName, Dictionary<string, string> dicData, bool isLower)
        {
            XmlDocument doc = new XmlDocument();
             XmlNode soapBody;
             if (isLower)
             {
                 doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"><soap:Body><" + methodName + " xmlns=\"http://tempuri.org/\"></" + methodName + "></soap:Body></soap:Envelope>");
                 soapBody = doc.GetElementsByTagName("soap:Body")[0];
             }
             else {
                 doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\"><soap12:Body><" + methodName + " xmlns=\"http://tempuri.org/\"></" + methodName + "></soap12:Body></soap12:Envelope>");
                 soapBody = doc.GetElementsByTagName("soap12:Body")[0];
             }
            foreach (KeyValuePair<string,string> kvp in dicData)
            {
              XmlElement soapElem=  doc.CreateElement(kvp.Key);
              soapElem.InnerXml = kvp.Value;
              soapBody.AppendChild(soapElem);
            }
            return Encoding.UTF8.GetBytes(doc.OuterXml);
        }
    }
}

