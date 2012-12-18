using System;
using System.Configuration;

namespace Top4ever.ClientService
{
    public class PubConstant
    {
        public static string IPAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["IPAddress"];
            }
        }

        public static int Port
        {
            get
            {
                int port = 0;
                string strPort = ConfigurationManager.AppSettings["Port"];
                if (int.TryParse(strPort, out port))
                {
                    return port;
                }
                else
                {
                    port = 5689;
                    return port;
                }
            }
        }
    }
}
