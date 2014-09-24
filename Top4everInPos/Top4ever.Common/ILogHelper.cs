using System;

namespace Top4ever.Common
{
    public interface ILogHelper
    {
        /// <summary>
        /// 日志的级别:Debug
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <type>void</type>
        void Debug(object message);

        /// <summary>
        /// 日志的级别:Debug
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <param name="e" type="Exception">Exception</param>
        /// <type>void</type>
        void Debug(object message, Exception e);

        /// <summary>
        /// 日志的级别:Info
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <type>void</type>
        void Info(object message);

        /// <summary>
        /// 日志的级别:Info
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <param name="e" type="Exception">Exception</param>
        /// <type>void</type>
        void Info(object message, Exception e);

        /// <summary>
        /// 日志的级别:Warn
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <type>void</type>
        void Warn(object message);

        /// <summary>
        /// 日志的级别:Warn
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <param name="e" type="Exception">Exception</param>
        /// <type>void</type>
        void Warn(object message, Exception e);

        /// <summary>
        /// 日志的级别:Error
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <type>void</type>
        void Error(object message);

        /// <summary>
        /// 日志的级别:Error
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <param name="e" type="Exception">Exception</param>
        /// <type>void</type>
        void Error(object message, Exception e);

        /// <summary>
        /// 日志的级别:Fatal
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <type>void</type>
        void Fatal(object message);

        /// <summary>
        /// 日志的级别:Fatal
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <param name="e" type="Exception">Exception</param>
        /// <type>void</type>
        void Fatal(object message, Exception e);
    }
}
