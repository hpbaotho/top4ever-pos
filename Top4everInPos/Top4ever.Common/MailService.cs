using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Top4ever.Common
{
    /// <summary>
    /// MailService 的摘要说明。
    /// </summary>
    public class MailService
    {
        private string m_UserName;
        private string m_Password;
        private string m_Host;
        private int m_Port;
        private bool m_EnableSsl;

        public MailService(string userName, string password, string host, int port, bool enableSsl)
        {
            m_UserName = userName;
            m_Password = password;
            m_Host = host;
            m_Port = port;
            m_EnableSsl = enableSsl;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="receiver">接收人</param>
        /// <param name="copyto">抄送</param>
        /// <param name="subject">标题</param>
        /// <param name="content">内容</param>
        /// <param name="dispalyAddress">显示发件人地址</param>
        /// <param name="displayName">显示发件人姓名</param>
        public bool SendMail(string receiver, string copyto, string subject, string content, string dispalyAddress, string displayName)
        {
            MailMessage msg = new MailMessage();
            //接收人
            if (receiver.IndexOf(';') > 0)
            {
                string[] recArr = receiver.Split(';');
                foreach (string rec in recArr)
                {
                    msg.To.Add(rec);
                }
            }
            else
            {
                msg.To.Add(receiver);
            }
            //抄送
            if (!string.IsNullOrEmpty(copyto))
            {
                if (copyto.IndexOf(';') > 0)
                {
                    string[] ccArr = copyto.Split(';');
                    foreach (string cc in ccArr)
                    {
                        msg.CC.Add(cc);
                    }
                }
                else
                {
                    msg.CC.Add(copyto);
                }
            }
            msg.From = new MailAddress(dispalyAddress, displayName, Encoding.UTF8);
            msg.Subject = subject;
            msg.SubjectEncoding = Encoding.UTF8;
            msg.Body = content;
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = false;
            msg.Priority = MailPriority.Normal;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(m_UserName, m_Password);
            client.Port = m_Port;
            client.Host = m_Host;
            client.EnableSsl = m_EnableSsl;
            bool result = false;
            try
            {
                client.Send(msg);
                result = true;
            }
            catch
            {
                result = false;
                throw;
            }
            return result;
        }
    }
}
