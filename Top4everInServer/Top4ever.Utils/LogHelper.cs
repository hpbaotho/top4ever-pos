using System;
using System.Reflection;
using IBatisNet.Common.Logging;

namespace Top4ever.Utils
{
    public class LogHelper
    {
        //������־��
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static LogHelper _instance;
        private static readonly object Obj = new object();

        private LogHelper()
        {
        }

        /// <summary>
        /// ��ȡLog����
        /// </summary>
        /// <returns></returns>
        public static LogHelper GetInstance()
        {
            if (_instance == null)
            {
                lock (Obj)
                {
                    if (_instance == null)
                    {
                        _instance = new LogHelper();
                        log4net.Config.XmlConfigurator.Configure();
                        //FileInfo configFile = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Config\\log4net.config");
                        //log4net.Config.XmlConfigurator.Configure(configFile);
                    }
                }
            }
            return _instance;
        }

        #region ������־��¼

        /// <summary>
        /// ����������Ϣ
        /// </summary>
        /// <param name="message">�Զ������</param>
        /// <param name="exception">�쳣</param>
        public void Debug(object message, Exception exception)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug(message, exception);
            }
        }

        public void Debug(object message)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug(message);
            }
        }

        /// <summary>
        /// ��������Ϣ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Info(object message, Exception exception)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info(message, exception);
            }
        }

        public void Info(object message)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info(message);
            }
        }

        /// <summary>
        /// ����������Ϣ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Warn(object message, Exception exception)
        {
            Logger.Warn(message, exception);
        }

        public void Warn(object message)
        {
            Logger.Warn(message);
        }

        /// <summary>
        /// ����������Ϣ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Error(object message, Exception exception)
        {
            Logger.Error(message, exception);
        }

        public void Error(object message)
        {
            Logger.Error(message);
        }

        /// <summary>
        /// ���������Դ���
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Fatal(object message, Exception exception)
        {
            Logger.Fatal(message, exception);
        }

        public void Fatal(object message)
        {
            Logger.Fatal(message);
        }

        #endregion
    }
}