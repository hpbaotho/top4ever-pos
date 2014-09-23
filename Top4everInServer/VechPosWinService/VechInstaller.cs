using System;
using System.Configuration.Install;
using System.Reflection;
using Top4ever.Utils;

namespace VechPosWinService
{
    public static class VechInstaller
    {
        private static readonly string ExePath = Assembly.GetExecutingAssembly().Location;

        public static bool InstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new[] { ExePath });
            }
            catch (Exception ex)
            {
                LogHelper.GetInstance().Error(string.Format("InstallMe error. Path : {0}", ExePath), ex);
                return false;
            }
            return true;
        }

        public static bool UninstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new[] { "/u", ExePath });
            }
            catch (Exception ex)
            {
                LogHelper.GetInstance().Error(string.Format("UninstallMe error Path : {0}", ExePath), ex);
                return false;
            }
            return true;
        }
    }
}