using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

using Top4ever.Common;

namespace Top4ever.Pos
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            // Set the unhandled exception mode to force all Windows Forms errors to go through our handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            // Add the event handler for handling non-UI thread exceptions to the event. 
            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormLogin());
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            ILogHelper logger = LogHelper.GetInstance();
            logger.Error("未知线程异常", e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception error = (Exception)e.ExceptionObject;
            ILogHelper logger = LogHelper.GetInstance();
            logger.Error("未处理异常", error);
        }
    }
}
