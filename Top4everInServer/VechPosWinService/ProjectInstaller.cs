using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace VechPosWinService
{
    /// <summary>
    /// 服务安装，安装后立即启动
    /// </summary>
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        protected string ConfigServiceName = "VechPosService";
        protected string ServerName = "MSSQLSERVER";
        protected string SqlExpressName = "MSSQL$SQLEXPRESS";

        public ProjectInstaller()
        {
            InitializeComponent();
            Committed += ServiceInstallerCommitted;

            var processInstaller = new ServiceProcessInstaller { Account = ServiceAccount.LocalSystem };
            var serviceInstaller = new ServiceInstaller { 
                StartType = ServiceStartMode.Automatic,
                Description = "这是由凡趣科技提供的Pos服务端程序",
                ServiceName = ConfigServiceName
            };
            if (ExistService(ServerName))
            {
                serviceInstaller.ServicesDependedOn = new string[] { ServerName };
            }
            if (ExistService(SqlExpressName))
            {
                serviceInstaller.ServicesDependedOn = new string[] { SqlExpressName };
            }
            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);
        }

        /// <summary>
        /// 服务安装完成后自动启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServiceInstallerCommitted(object sender, InstallEventArgs e)
        {
            var controller = new ServiceController(ConfigServiceName);
            controller.Start();
        }

        /// <summary>
        /// 判断服务是否存在
        /// </summary>
        /// <returns></returns>
        private bool ExistService(string name)
        {
            ServiceController[] service = ServiceController.GetServices();
            return service.Any(t => t.ServiceName.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
