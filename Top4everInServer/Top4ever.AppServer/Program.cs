using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
