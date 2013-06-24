using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace Top4ever.Common
{
    public class CheckNetWork
    {
        public static bool CheckConnectNetWork(string strIPAddress)
        {
            return CheckConnectNetWork(strIPAddress, 3);
        }

        public static bool CheckConnectNetWork(string strIPAddress, int tryTimes)
        {
            bool result = false;
            int haveOperTime = 0;
            while (!result && haveOperTime < tryTimes)
            {
                result = IsConnectedNetWork(strIPAddress);
                haveOperTime++;
            }
            return result;
        }

        private static bool IsConnectedNetWork(string strIPAddress)
        {
            bool result = false;
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            string data = "";
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            int timeout = 400;
            PingReply reply = pingSender.Send(strIPAddress, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
            {
                result = true;
            }
            return result;
        }
    }
}
