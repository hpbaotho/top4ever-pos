using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Top4ever.Common;

namespace Top4ever.ClientService
{
    /// <summary>
    /// Implements the connection logic for the socket client.
    /// </summary>
    public sealed class SocketClient : IDisposable
    {
        private const int LEN_INT32 = 4;
        /// <summary>
        /// Listener endpoint.
        /// </summary>
        private readonly IPEndPoint _hostEndPoint;
        /// <summary>
        /// The socket used to send/receive messages.
        /// </summary>
        private Socket _clientSocket;

        public SocketClient(String hostName, Int32 port)
        {
            //Instantiates the endpoint and socket.
            IPAddress ipAddress = IPAddress.Parse(hostName);
            //把ip和端口转化为IPEndpoint实例 
            this._hostEndPoint = new IPEndPoint(ipAddress, port);
            //创建Socket 
            this._clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Exchange a message with the host.
        /// </summary>
        /// <param name="sendBuffer">Message to send.</param>
        /// <param name="receiveData">the data by receive</param>
        /// <returns>the result of operate code</returns>
        public Int32 SendReceive(Byte[] sendBuffer, out Byte[] receiveData)
        {
            int operateCode = 0;
            receiveData = null;
            try
            {
                _clientSocket.Connect(_hostEndPoint);
                //向服务器发送信息 
                _clientSocket.Send(sendBuffer, sendBuffer.Length, SocketFlags.None);

                byte[] receiveBuffer = new byte[1024];
                int receiveSize = _clientSocket.Receive(receiveBuffer, 2 * LEN_INT32, SocketFlags.None);
                if (receiveSize != 2 * LEN_INT32) throw new Exception("the length of data what the client socket receive is error.");

                // create buffer
                int byteCount = BitConverter.ToInt32(receiveBuffer, LEN_INT32);
                Byte[] itemBuffer = new byte[byteCount];
                // set first data to buffer 
                int bufferOffset = 0;
                Array.Copy(receiveBuffer, 0, itemBuffer, bufferOffset, receiveSize);
                bufferOffset += receiveSize;
                Array.Clear(receiveBuffer, 0, receiveBuffer.Length);
                // recv data from server network 
                while (bufferOffset < byteCount)
                {
                    receiveSize = _clientSocket.Receive(receiveBuffer, receiveBuffer.Length, 0);
                    // recv data and copy to buffer for next step
                    Array.Copy(receiveBuffer, 0, itemBuffer, bufferOffset, receiveSize);
                    bufferOffset += receiveSize;
                    Array.Clear(receiveBuffer, 0, receiveBuffer.Length);
                }
                receiveData = itemBuffer;
                //set operate code
                operateCode = BitConverter.ToInt32(itemBuffer, 0);
            }
            catch (Exception ex)
            {
                LogHelper.GetInstance().Error("socket send and receive data error.", ex);
            }
            return operateCode;
        }

        public void Close()
        {
            if (_clientSocket != null)
            {
                if (_clientSocket.Connected)
                {
                    _clientSocket.Shutdown(SocketShutdown.Both);
                    _clientSocket.Close();
                    _clientSocket = null;
                }
            }
        }

        public void Dispose()
        {
            if (_clientSocket != null)
            {
                if (_clientSocket.Connected)
                {
                    _clientSocket.Shutdown(SocketShutdown.Both);
                    _clientSocket.Close();
                }
            }
        }
    }
}