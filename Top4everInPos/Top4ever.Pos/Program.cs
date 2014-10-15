using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Common;
using Top4ever.Domain;
using Top4ever.Entity;
using Top4ever.Entity.Config;
using VechsoftPos.Register;

namespace VechsoftPos
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
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //get config file
            Shop currentShop = null;
            const string appSettingPath = "Config/AppSetting.config";
            if (File.Exists(appSettingPath))
            {
                AppSettingConfig appSettingConfig = XmlUtil.Deserialize<AppSettingConfig>(appSettingPath);
                ConstantValuePool.BizSettingConfig = appSettingConfig;
                ShopService shopService = new ShopService();
                currentShop = shopService.GetCurrentShop(appSettingConfig.IPAddress, appSettingConfig.Port);
            }
            if (currentShop != null && !string.IsNullOrEmpty(currentShop.ShopNo))
            {
                ConstantValuePool.CurrentShop = currentShop;
                //Check for registration
                int remainDays = 0;
                if(true)    //(ProductRegister.CheckProductRegistration(ref remainDays))
                {
                    Application.Run(new FormLogin());
                }
                else
                {
                    string strVersion = string.Empty;
                    Assembly executingAssembly = Assembly.GetExecutingAssembly();
                    object[] objectAttrs = executingAssembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                    if (objectAttrs.Length > 0)
                    {
                        AssemblyFileVersionAttribute fileAttr = objectAttrs[0] as AssemblyFileVersionAttribute;
                        if (fileAttr != null)
                        {
                            strVersion = fileAttr.Version;
                        }
                    }
                    FormProductTry formTry = new FormProductTry(remainDays, currentShop.ShopName, currentShop.ShopNo, strVersion);
                    formTry.ShowDialog();
                    if (formTry.Result)
                    {
                        Application.Run(new FormLogin());
                    }
                }
            }
            else
            {
                MessageBox.Show("找不到有效的服务端程序，请检查服务是否启动或者配置是否正确。", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Run(new FormConfig());
            }
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
