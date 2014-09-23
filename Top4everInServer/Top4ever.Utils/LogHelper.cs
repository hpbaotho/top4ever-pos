using System;
using System.Reflection;
using IBatisNet.Common.Logging;

namespace Top4ever.Utils
{
    public class LogHelper
    {
        //定义日志类
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static LogHelper _instance;
        private static readonly object Obj = new object();

        private LogHelper()
        {
        }

        /// <summary>
        /// 获取Log单例
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

        #region 错误日志记录

        /// <summary>
        /// 发布调试信息
        /// </summary>
        /// <param name="message">自定义错误</param>
        /// <param name="exception">异常</param>
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
        /// 仅发布信息
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
        /// 发布警告信息
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
        /// 发布错误信息
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
        /// 发布致命性错误
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