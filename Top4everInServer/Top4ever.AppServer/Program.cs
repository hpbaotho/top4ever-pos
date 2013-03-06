using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

using Top4ever.NetServer;

namespace Top4ever.AppServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Int32 port = 5689;
                Int32 numConnections = 10;
                Int32 bufferSize = Int16.MaxValue;

                SocketListener sl = new SocketListener(numConnections, bufferSize);
                sl.Start(port);

                Console.WriteLine("Server listening on port {0}. Press any key to terminate the server process...", port);
                Console.Read();

                sl.Stop();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static DateTime GetStandardDataTime()
        {
            //"time-a.timefreq.bldrdoc.gov"     "132.163.4.101";
            //"time-b.timefreq.bldrdoc.gov"     "132.163.4.102";
            //"time-c.timefreq.bldrdoc.gov"     "132.163.4.103";
            //"utcnist.colorado.edu"                 "128.138.140.44";
            //"nist1.symmetricom.com"            "69.25.96.13";
            //"nist1.aol-va.truetime.com"         "64.236.96.53";
            string[] timeServer = new string[] { "132.163.4.101", "132.163.4.102", "132.163.4.103", "128.138.140.44", "69.25.96.13", "64.236.96.53" };

            TcpClient client = new TcpClient();
            int portNum = 13;
            byte[] bytes = new byte[1024];
            int bytesRead = 0;
            for (int i = 0; i < timeServer.Length; i++)
            {
                string hostName = timeServer[i];
                if (CheckNetWork.CheckConnectNetWork(hostName))
                {
                    try
                    {
                        client.Connect(hostName, portNum);
                        NetworkStream ns = client.GetStream();
                        bytesRead = ns.Read(bytes, 0, bytes.Length);
                        client.Close();
                        break;
                    }
                    catch (Exception ex)
                    {
                        //log ex
                    }
                }
            }
            string strTime = Encoding.ASCII.GetString(bytes, 0, bytesRead);
            string[] s = strTime.Split(' ');
            DateTime dt = DateTime.Parse(s[1] + " " + s[2]);//得到标准时间
            return dt;
        }
    }
}
