using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Top4ever.ClientService
{
    /// <summary>
    /// Implements the connection logic for the socket client.
    /// </summary>
    public sealed class SocketClient : IDisposable
    {
        private const int LEN_INT32 = 4;
        private Int32 m_OperateCode;
        private Byte[] m_ItemBuffer;
        public Int32 m_Offset;

        /// <summary>
        /// Constants for socket operations.
        /// </summary>
        private const Int32 ReceiveOperation = 1, SendOperation = 0;

        /// <summary>
        /// The socket used to send/receive messages.
        /// </summary>
        private Socket clientSocket;

        /// <summary>
        /// Flag for connected socket.
        /// </summary>
        private Boolean connected = false;

        /// <summary>
        /// Listener endpoint.
        /// </summary>
        private IPEndPoint hostEndPoint;

        /// <summary>
        /// Signals a connection.
        /// </summary>
        private AutoResetEvent autoConnectEvent = new AutoResetEvent(false);

        /// <summary>
        /// Signals the send/receive operation.
        /// </summary>
        private AutoResetEvent[] autoSendReceiveEvents = new AutoResetEvent[]
        {
            new AutoResetEvent(false),
            new AutoResetEvent(false)
        };

        /// <summary>
        /// Create an uninitialized client instance.  
        /// To start the send/receive processing
        /// call the Connect method followed by SendReceive method.
        /// </summary>
        /// <param name="hostName">Name of the host where the listener is running.</param>
        /// <param name="port">Number of the TCP port from the listener.</param>
        public SocketClient(String hostName, Int32 port)
        {
            // Get host related information.
            IPHostEntry host = Dns.GetHostEntry(hostName);

            // Addres of the host.
            IPAddress[] addressList = host.AddressList;

            // Instantiates the endpoint and socket.
            this.hostEndPoint = new IPEndPoint(addressList[addressList.Length - 1], port);
            this.clientSocket = new Socket(this.hostEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Connect to the host.
        /// </summary>
        /// <returns>True if connection has succeded, else false.</returns>
        public void Connect()
        {
            SocketAsyncEventArgs connectArgs = new SocketAsyncEventArgs();

            connectArgs.UserToken = this.clientSocket;
            connectArgs.RemoteEndPoint = this.hostEndPoint;
            connectArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnect);

            clientSocket.ConnectAsync(connectArgs);
            autoConnectEvent.WaitOne();

            SocketError errorCode = connectArgs.SocketError;
            if (errorCode != SocketError.Success)
            {
                throw new SocketException((Int32)errorCode);
            }
        }

        private void OnConnect(object sender, SocketAsyncEventArgs e)
        {
            // Signals the end of connection.
            autoConnectEvent.Set();

            // Set the flag for socket connected.
            this.connected = (e.SocketError == SocketError.Success);
        }

        /// <summary>
        /// Exchange a message with the host.
        /// </summary>
        /// <param name="sendBuffer">Message to send.</param>
        /// <param name="receiveData">the data by receive</param>
        /// <returns>the result of operate code</returns>
        public Int32 SendReceive(Byte[] sendBuffer, out Byte[] receiveData)
        {
            //指示应挂起此线程以使其他等待线程能够执行
            System.Threading.Thread.Sleep(0);
            if (this.connected)
            {
                // Prepare arguments for send/receive operation.
                SocketAsyncEventArgs completeArgs = new SocketAsyncEventArgs();
                completeArgs.SetBuffer(sendBuffer, 0, sendBuffer.Length);
                completeArgs.UserToken = this.clientSocket;
                completeArgs.RemoteEndPoint = this.hostEndPoint;
                completeArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSend);

                // Start sending asyncronally.
                clientSocket.SendAsync(completeArgs);

                // Wait for the send/receive completed.
                //AutoResetEvent.WaitAll(autoSendReceiveEvents);
                foreach (var v in autoSendReceiveEvents)
                {
                    v.WaitOne();
                }

                // Return data from SocketAsyncEventArgs buffer.
                receiveData = m_ItemBuffer;
                return m_OperateCode;
            }
            else
            {
                throw new SocketException((Int32)SocketError.NotConnected);
            }
        }

        private void OnSend(object sender, SocketAsyncEventArgs e)
        {
            // Signals the end of send.
            autoSendReceiveEvents[ReceiveOperation].Set();
            e.Completed -= new EventHandler<SocketAsyncEventArgs>(OnSend);

            if (e.SocketError == SocketError.Success)
            {
                if (e.LastOperation == SocketAsyncOperation.Send)
                {
                    // Prepare receiving.
                    Socket s = e.UserToken as Socket;

                    byte[] receiveBuffer = new byte[1024];
                    e.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
                    e.Completed += new EventHandler<SocketAsyncEventArgs>(OnReceiveCompleted);
                    s.ReceiveAsync(e);
                }
            }
            else
            {
                this.ProcessError(e);
            }
        }

        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            this.OnReceive(e);
            if (m_ItemBuffer.Length == m_Offset)
            {
                // Signals the end of receive.
                autoSendReceiveEvents[SendOperation].Set();
                e.Completed -= new EventHandler<SocketAsyncEventArgs>(OnReceiveCompleted);
            }
        }

        private void OnReceive(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0)
            {
                if (e.SocketError == SocketError.Success)
                {
                    #region Set return buffer.
                    if (m_OperateCode == 0)
                    {
                        m_OperateCode = BitConverter.ToInt32(e.Buffer, 0);
                        int bufferSize = BitConverter.ToInt32(e.Buffer, LEN_INT32);
                        m_ItemBuffer = new byte[bufferSize];

                        Array.Copy(e.Buffer, e.Offset, m_ItemBuffer, m_Offset, e.BytesTransferred);
                        m_Offset = e.BytesTransferred;
                    }
                    else
                    {
                        Array.Copy(e.Buffer, e.Offset, m_ItemBuffer, m_Offset, e.BytesTransferred);
                        m_Offset += e.BytesTransferred;
                    }
                    #endregion

                    Socket s = e.UserToken as Socket;
                    if (!s.ReceiveAsync(e))
                    {
                        // Read the next block of data sent by client.
                        this.OnReceive(e);
                    }
                }
                else
                {
                    this.ProcessError(e);
                }
            }
        }


        /// <summary>
        /// Close socket in case of failure and throws a SockeException according to the SocketError.
        /// </summary>
        /// <param name="e">SocketAsyncEventArg associated with the failed operation.</param>
        private void ProcessError(SocketAsyncEventArgs e)
        {
            Socket s = e.UserToken as Socket;
            if (s.Connected)
            {
                // close the socket associated with the client
                try
                {
                    s.Shutdown(SocketShutdown.Both);
                }
                catch (Exception)
                {
                    // throws if client process has already closed
                }
                finally
                {
                    if (s.Connected)
                    {
                        s.Close();
                    }
                }
            }

            // Throw the SocketException
            throw new SocketException((Int32)e.SocketError);
        }

        /// <summary>
        /// Disconnect from the host.
        /// </summary>
        public void Disconnect()
        {
            clientSocket.Disconnect(false);
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes the instance of SocketClient.
        /// </summary>
        public void Dispose()
        {
            autoConnectEvent.Close();
            autoSendReceiveEvents[SendOperation].Close();
            autoSendReceiveEvents[ReceiveOperation].Close();
            if (this.clientSocket.Connected)
            {
                this.clientSocket.Close();
            }
        }

        #endregion
    }
}
