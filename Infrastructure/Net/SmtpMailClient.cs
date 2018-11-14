using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace ESC.Core
{
    /// <summary>
    /// 邮件服务类
    /// </summary>
    public class SmtpMailClient
    {
        #region 同步

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="stmpHost">服务主机</param>
        /// <param name="strFrom">发件人地址</param>
        /// <param name="strFromPass">发件人密码</param>
        /// <param name="strTo">收件人地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        public static void SendEmail(string smtpHost, string strFrom, string strFromPass, string strTo, string subject, string body)
        {
            SendEmail(smtpHost, 25, strFrom, strFromPass, strTo, subject, body);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="stmpHost">服务主机</param>
        /// <param name="port">服务器端口</param>
        /// <param name="strFrom">发件人地址</param>
        /// <param name="strFromPass">发件人密码</param>
        /// <param name="strTo">收件人地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        public static void SendEmail(string smtpHost, int port, string strFrom, string strFromPass, string strTo, string subject, string body)
        {
            SendEmail(smtpHost, port, strFrom, strFromPass, strTo, subject, body, false);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="stmpHost">服务主机</param>
        /// <param name="port">服务器端口</param>
        /// <param name="strFrom">发件人地址</param>
        /// <param name="strFromPass">发件人密码</param>
        /// <param name="strTo">收件人地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="ssl">是否使用安全套接字层 (SSL) 加密连接</param>
        public static void SendEmail(string smtpHost, int port, string strFrom, string strFromPass, string strTo, string subject, string body, bool ssl)
        {
            SendEmail(smtpHost, port, strFrom, strFromPass, strTo, subject, body, false, null);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="stmpHost">服务主机</param>
        /// <param name="port">服务器端口</param>
        /// <param name="strFrom">发件人地址</param>
        /// <param name="strFromPass">发件人密码</param>
        /// <param name="strTo">收件人地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="ssl">是否使用安全套接字层 (SSL) 加密连接</param>
        /// <param name="strCopies">抄送人列表</param>
        public static void SendEmail(string smtpHost, int port, string strFrom, string strFromPass, string strTo, string subject, string body, bool ssl, string[] strCopies)
        {
            SendEmail(smtpHost, port, strFrom, strFromPass, strTo, subject, body, false, strCopies, null);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="stmpHost">服务主机</param>
        /// <param name="port">服务器端口</param>
        /// <param name="strFrom">发件人地址</param>
        /// <param name="strFromPass">发件人密码</param>
        /// <param name="strTo">收件人地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="ssl">是否使用安全套接字层 (SSL) 加密连接</param>
        /// <param name="strCopies">抄送人列表</param>
        /// <param name="strFiles"></param>
        public static void SendEmail(string smtpHost, int port, string strFrom, string strFromPass, string strTo, string subject, string body, bool ssl, string[] strCopies, string[] strFiles)
        {
            SmtpClient client = new SmtpClient(smtpHost, port);
            client.UseDefaultCredentials = false; //启用身份认证 
            client.Credentials = new NetworkCredential(strFrom, strFromPass);
            client.EnableSsl = ssl;

            MailMessage message = new MailMessage(strFrom, strTo, subject, body);
            message.BodyEncoding = Encoding.UTF8;
            message.SubjectEncoding = Encoding.UTF8;
            message.IsBodyHtml = false;      //是否采用html格式邮件

            if (strCopies != null && strCopies.Length > 0)
            {
                foreach (string strCopy in strCopies)
                {
                    message.CC.Add(strCopy);
                }
            }

            if (strFiles != null && strFiles.Length > 0)
            {
                foreach (string strFile in strFiles)
                {
                    if (File.Exists(strFile))
                    {
                        message.Attachments.Add(new Attachment(strFile));
                    }
                }
            }

            try
            {
                client.Send(message);
            }
            catch (SmtpFailedRecipientsException sfre)
            {
                Log.Exception(sfre);
            }
            catch (SmtpException se)
            {
                Log.Exception(se);
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }
        }

        #endregion

        #region 异步

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="stmpHost">服务主机</param>
        /// <param name="strFrom">发件人地址</param>
        /// <param name="strFromPass">发件人密码</param>
        /// <param name="strTo">收件人地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        public static void SendEmailAsync(string smtpHost, string strFrom, string strFromPass, string strTo, string subject, string body, SendCompletedEventHandler completeHandler)
        {
            SendEmailAsync(smtpHost, 25, strFrom, strFromPass, strTo, subject, body, completeHandler);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="stmpHost">服务主机</param>
        /// <param name="port">服务器端口</param>
        /// <param name="strFrom">发件人地址</param>
        /// <param name="strFromPass">发件人密码</param>
        /// <param name="strTo">收件人地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        public static void SendEmailAsync(string smtpHost, int port, string strFrom, string strFromPass, string strTo, string subject, string body, SendCompletedEventHandler completeHandler)
        {
            SendEmailAsync(smtpHost, port, strFrom, strFromPass, strTo, subject, body, false, completeHandler);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="stmpHost">服务主机</param>
        /// <param name="port">服务器端口</param>
        /// <param name="strFrom">发件人地址</param>
        /// <param name="strFromPass">发件人密码</param>
        /// <param name="strTo">收件人地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="ssl">是否使用安全套接字层 (SSL) 加密连接</param>
        public static void SendEmailAsync(string smtpHost, int port, string strFrom, string strFromPass, string strTo, string subject, string body, bool ssl, SendCompletedEventHandler completeHandler)
        {
            SendEmailAsync(smtpHost, port, strFrom, strFromPass, strTo, subject, body, false, null, completeHandler);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="stmpHost">服务主机</param>
        /// <param name="port">服务器端口</param>
        /// <param name="strFrom">发件人地址</param>
        /// <param name="strFromPass">发件人密码</param>
        /// <param name="strTo">收件人地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="ssl">是否使用安全套接字层 (SSL) 加密连接</param>
        /// <param name="strCopies">抄送人列表</param>
        public static void SendEmailAsync(string smtpHost, int port, string strFrom, string strFromPass, string strTo, string subject, string body, bool ssl, string[] strCopies, SendCompletedEventHandler completeHandler)
        {
            SendEmailAsync(smtpHost, port, strFrom, strFromPass, strTo, subject, body, false, strCopies, null, completeHandler);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="stmpHost">服务主机</param>
        /// <param name="port">服务器端口</param>
        /// <param name="strFrom">发件人地址</param>
        /// <param name="strFromPass">发件人密码</param>
        /// <param name="strTo">收件人地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="ssl">是否使用安全套接字层 (SSL) 加密连接</param>
        /// <param name="strCopies">抄送人列表</param>
        /// <param name="strFiles"></param>
        public static void SendEmailAsync(string smtpHost, int port, string strFrom, string strFromPass, string strTo, string subject, string body, bool ssl, string[] strCopies, string[] strFiles, SendCompletedEventHandler completeHandler)
        {
            SmtpClient client = new SmtpClient(smtpHost, port);
            client.UseDefaultCredentials = false; //启用身份认证 
            client.Credentials = new NetworkCredential(strFrom, strFromPass);
            client.EnableSsl = ssl;

            MailMessage message = new MailMessage(strFrom, strTo, subject, body);
            message.BodyEncoding = Encoding.UTF8;
            message.SubjectEncoding = Encoding.UTF8;
            message.IsBodyHtml = false;      //是否采用html格式邮件

            if (strCopies != null && strCopies.Length > 0)
            {
                foreach (string strCopy in strCopies)
                {
                    message.CC.Add(strCopy);
                }
            }

            if (strFiles != null && strFiles.Length > 0)
            {
                foreach (string strFile in strFiles)
                {
                    if (File.Exists(strFile))
                    {
                        message.Attachments.Add(new Attachment(strFile));
                    }
                }
            }

            if (completeHandler != null) {
                client.SendCompleted += completeHandler;
            }

            try
            {
                string userToken = "smtp";
                client.SendAsync(message, userToken);
            }
            catch (SmtpFailedRecipientsException sfre)
            {
                Log.Exception(sfre);
            }
            catch (SmtpException se)
            {
                Log.Exception(se);
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }
        }

        #endregion
    }
}
