using System;
using System.Net.Sockets;

using Top4ever.BLL;
using Top4ever.Utils;

namespace Top4ever.NetServer
{
    /// <summary>
    /// Token for use with SocketAsyncEventArgs.
    /// </summary>
    public sealed class Token : IDisposable
    {
        private const int LEN_INT32 = 4;

        private Socket connection;
        private int m_CommandID;
        private byte[] m_ItemBuffer;
        private int m_Offset;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="connection">Socket to accept incoming data.</param>
        public Token(Socket connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Accept socket.
        /// </summary>
        public Socket Connection
        {
            get { return this.connection; }
        }

        /// <summary>
        /// Process data received from the client.
        /// </summary>
        /// <param name="args">SocketAsyncEventArgs used in the operation.</param>
        public void ProcessData(SocketAsyncEventArgs args)
        {
            Byte[] sendBuffer = CmdRoute.RouteHandler(m_CommandID, m_ItemBuffer);
            args.SetBuffer(sendBuffer, 0, sendBuffer.Length);

            // Clear, so it can receive more data from a keep-alive connection client.
            m_CommandID = 0;
            m_Offset = 0;
        }

        /// <summary>
        /// Set data received from the client.
        /// </summary>
        /// <param name="args">SocketAsyncEventArgs used in the operation.</param>
        public void SetData(SocketAsyncEventArgs args)
        {
            if (m_CommandID == 0)
            {
                m_CommandID = BitConverter.ToInt32(args.Buffer, 0);
                int bufferSize = BitConverter.ToInt32(args.Buffer, LEN_INT32);
                m_ItemBuffer = new byte[bufferSize];

                Array.Copy(args.Buffer, args.Offset, m_ItemBuffer, m_Offset, args.BytesTransferred);
                m_Offset = args.BytesTransferred;
            }
            else
            {
                Array.Copy(args.Buffer, args.Offset, m_ItemBuffer, m_Offset, args.BytesTransferred);
                m_Offset += args.BytesTransferred;
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Release instance.
        /// </summary>
        public void Dispose()
        {
            try
            {
                this.connection.Shutdown(SocketShutdown.Send);
            }
            catch (Exception ex)
            {
                LogHelper.GetInstance().Error("client socket has closed.", ex);
            }
            finally
            {
                this.connection.Close();
            }
        }

        #endregion
    }
}
