using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;

namespace VechPosWinService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            //无参数时直接运行服务
            if ((!IsMono && !Environment.UserInteractive)//Windows Service
                || (IsMono && !AppDomain.CurrentDomain.FriendlyName.Equals(Path.GetFileName(Assembly.GetEntryAssembly().CodeBase))))//MonoService
            {
                RunAsService();
                return;
            }
            if (args != null && args.Length > 0)
            {
                if (args[0].Equals("-i", StringComparison.OrdinalIgnoreCase))
                {
                    VechInstaller.InstallMe();
                    return;
                }
                if (args[0].Equals("-u", StringComparison.OrdinalIgnoreCase))
                {
                    VechInstaller.UninstallMe();
                    return;
                }
                return;
            }
            var posService = new VechPosService();
            posService.Start();
            Console.WriteLine("Server listening on port. Press any key to terminate the server process...");
            Console.ReadLine();
            posService.Close();
        }

        private static void RunAsService()
        {
            var servicesToRun = new ServiceBase[] 
			{ 
				new VechPosService() 
			};
            ServiceBase.Run(servicesToRun);
        }

        private static bool IsMono
        {
            get
            {
                return Type.GetType("Mono.Runtime") != null;
            }
        }
    }
}
