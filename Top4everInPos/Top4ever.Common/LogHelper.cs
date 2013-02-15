using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using log4net;

namespace Top4ever.Common
{
    public class LogHelper : ILogHelper
    {
        //������־��
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static LogHelper instance;
        private static object obj = new object();

        private LogHelper()
        {

        }

        /// <summary>
        /// ��ȡLog����
        /// </summary>
        /// <returns></returns>
        public static LogHelper GetInstance()
        {
            if (instance == null)
            {
                lock (obj)
                {
                    if (instance == null)
                    {
                        instance = new LogHelper();
                        FileInfo configFile = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Config\\log4net.config");
                        log4net.Config.XmlConfigurator.Configure(configFile);
                    }
                }
            }
            return instance;
        }

        #region ������־��¼
        /// <summary>
        /// ����������Ϣ
        /// </summary>
        /// <param name="message">�Զ������</param>
        /// <param name="exception">�쳣</param>
        public void Debug(object message, Exception exception)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(message, exception);
            }
        }

        public void Debug(object message)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(message);
            }
        }

        /// <summary>
        /// ��������Ϣ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Info(object message, Exception exception)
        {
            if (log.IsInfoEnabled)
            {
                log.Info(message, exception);
            }
        }

        public void Info(object message)
        {
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
        }

        /// <summary>
        /// ����������Ϣ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Warn(object message, Exception exception)
        {
            if (log.IsWarnEnabled)
            {
                log.Warn(message, exception);
            }
        }

        public void Warn(object message)
        {
            if (log.IsWarnEnabled)
            {
                log.Warn(message);
            }
        }

        /// <summary>
        /// ����������Ϣ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Error(object message, Exception exception)
        {
            if (log.IsErrorEnabled)
            {
                log.Error(message, exception);
            }
        }

        public void Error(object message)
        {
            if (log.IsErrorEnabled)
            {
                log.Error(message);
            }
        }

        /// <summary>
        /// ���������Դ���
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Fatal(object message, Exception exception)
        {
            if (log.IsFatalEnabled)
            {
                log.Fatal(message, exception);
            }
        }

        public void Fatal(object message)
        {
            if (log.IsFatalEnabled)
            {
                log.Fatal(message);
            }
        }
        #endregion
    }
}
