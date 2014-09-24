using System;

namespace Top4ever.Common
{
    public interface ILogHelper
    {
        /// <summary>
        /// ��־�ļ���:Debug
        /// </summary>
        /// <param name="message">��Ϣ����</param>
        /// <type>void</type>
        void Debug(object message);

        /// <summary>
        /// ��־�ļ���:Debug
        /// </summary>
        /// <param name="message">��Ϣ����</param>
        /// <param name="e" type="Exception">Exception</param>
        /// <type>void</type>
        void Debug(object message, Exception e);

        /// <summary>
        /// ��־�ļ���:Info
        /// </summary>
        /// <param name="message">��Ϣ����</param>
        /// <type>void</type>
        void Info(object message);

        /// <summary>
        /// ��־�ļ���:Info
        /// </summary>
        /// <param name="message">��Ϣ����</param>
        /// <param name="e" type="Exception">Exception</param>
        /// <type>void</type>
        void Info(object message, Exception e);

        /// <summary>
        /// ��־�ļ���:Warn
        /// </summary>
        /// <param name="message">��Ϣ����</param>
        /// <type>void</type>
        void Warn(object message);

        /// <summary>
        /// ��־�ļ���:Warn
        /// </summary>
        /// <param name="message">��Ϣ����</param>
        /// <param name="e" type="Exception">Exception</param>
        /// <type>void</type>
        void Warn(object message, Exception e);

        /// <summary>
        /// ��־�ļ���:Error
        /// </summary>
        /// <param name="message">��Ϣ����</param>
        /// <type>void</type>
        void Error(object message);

        /// <summary>
        /// ��־�ļ���:Error
        /// </summary>
        /// <param name="message">��Ϣ����</param>
        /// <param name="e" type="Exception">Exception</param>
        /// <type>void</type>
        void Error(object message, Exception e);

        /// <summary>
        /// ��־�ļ���:Fatal
        /// </summary>
        /// <param name="message">��Ϣ����</param>
        /// <type>void</type>
        void Fatal(object message);

        /// <summary>
        /// ��־�ļ���:Fatal
        /// </summary>
        /// <param name="message">��Ϣ����</param>
        /// <param name="e" type="Exception">Exception</param>
        /// <type>void</type>
        void Fatal(object message, Exception e);
    }
}
