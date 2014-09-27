using System;
using Microsoft.Win32;

namespace VechsoftPos.Register
{
    public class ProductRegister
    {
        private static string _keyDir = "Software\\Top4Pos\\VersionRegister";
        private static int _tryDays = 15;

        public static bool CheckProductRegistration(ref int remainDays)
        {
            bool result = false;
            using (RegistryKey rgs = Registry.CurrentUser.OpenSubKey(_keyDir, true))
            {
                if (rgs != null)
                {
                    string[] arr = rgs.GetValue("ProductCode").ToString().Split(' ');
                    if (Convert.ToInt32(arr.GetValue(0).ToString(), 16) + 19901191 == 99999999) //判断是否激活，激活
                    {
                        return true;
                    }
                    else //未激活
                    {
                        int firstRunDate = Convert.ToInt32(arr.GetValue(0).ToString(), 16) + 19901191;
                        string strYear = Convert.ToString(firstRunDate).Substring(0, 4);
                        string strMonth = Convert.ToString(firstRunDate).Substring(4, 2);
                        string strDay = Convert.ToString(firstRunDate).Substring(6, 2);
                        DateTime firstDate = DateTime.Parse(strYear + "-" + strMonth + "-" + strDay);
                        if (firstDate > DateTime.Now)
                        {
                            remainDays = 0;
                        }
                        else
                        {
                            int effectiveDays = Convert.ToInt32(arr.GetValue(1).ToString(), 16) - 37;
                            TimeSpan ts = DateTime.Now - firstDate;
                            remainDays = effectiveDays - ts.Days;
                            if (remainDays < 0) remainDays = 0;
                        }
                        return false;
                    }
                }
                else
                {
                    //先写入注册表
                    string[] arrDir = _keyDir.Split('\\');
                    string path = "Software";
                    for (int i = 1; i < arrDir.Length; i++)
                    {
                        path += "\\" + arrDir[i];
                        Registry.CurrentUser.CreateSubKey(path);
                    }
                    RegistryKey rgsKey = Registry.CurrentUser.OpenSubKey(_keyDir, true);
                    rgsKey.SetValue("ProductCode", Convert.ToString(Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) - 19901191, 16) + " " + Convert.ToString(_tryDays + 37, 16));
                    rgsKey.Close();
                    remainDays = _tryDays;
                    result = false;
                }
            }
            return result;
        }

        public static bool WriteProductRegistration(int validityInMonth)
        {
            string strDateTime = System.DateTime.Now.ToString("yyyyMMdd");
            DateTime time1 = DateTime.Now.AddMonths(validityInMonth);
            DateTime time2 = DateTime.Now;
            TimeSpan ts = time1 - time2;
            int days = ts.Days;
            Registry.CurrentUser.DeleteSubKey(_keyDir);
            Registry.CurrentUser.CreateSubKey(_keyDir);
            RegistryKey rgs = Registry.CurrentUser.OpenSubKey(_keyDir, true);
            rgs.SetValue("ProductCode", Convert.ToString(Convert.ToInt32(strDateTime) - 19901191, 16) + " " + Convert.ToString(days + 37, 16));
            rgs.Close();
            return true;
        }
    }
}
