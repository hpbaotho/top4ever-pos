using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace Top4ever.Hardware
{
    public class SerialPortHelper : IDisposable
    {
        public event PortDataReceivedEventHandle ReceivedHandler;
        public event SerialErrorReceivedEventHandler ErrorHandler;

        private SerialPort m_SerialPort;
        private bool ReceiveEventFlag = false;  //接收事件是否有效 false表示有效

        public SerialPortHelper(string portName, int baudRate, Parity parity, bool RtsEnable)
        {
            m_SerialPort = new SerialPort(portName, baudRate, parity, 8, StopBits.One);
            m_SerialPort.RtsEnable = RtsEnable;
            m_SerialPort.ReadTimeout = 3000;
            m_SerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
            m_SerialPort.ErrorReceived += new SerialErrorReceivedEventHandler(ErrorEvent);
        }

        public void Open()
        {
            if (!m_SerialPort.IsOpen)
            {
                m_SerialPort.Open();
            }
        }

        public bool IsOpen()
        {
            return m_SerialPort.IsOpen;
        }

        //数据发送
        public void SendData(byte[] data)
        {
            if (m_SerialPort.IsOpen)
            {
                m_SerialPort.Write(data, 0, data.Length);
            }
        }

        public void SendData(byte[] data, int offset, int count)
        {
            if (m_SerialPort.IsOpen)
            {
                m_SerialPort.Write(data, offset, count);
            }
        }

        public void SendData(string text)
        {
            if (m_SerialPort.IsOpen)
            {
                m_SerialPort.Write(text);
            }
        }

        //发送命令
        public int SendCommand(byte[] sendData, ref  byte[] receiveData, int overtime)
        {
            if (m_SerialPort.IsOpen)
            {
                ReceiveEventFlag = true;        //关闭接收事件
                m_SerialPort.DiscardInBuffer();         //清空接收缓冲区                 
                m_SerialPort.Write(sendData, 0, sendData.Length);
                int num = 0, ret = 0;
                while (num++ < overtime)
                {
                    if (m_SerialPort.BytesToRead >= receiveData.Length) break;
                    System.Threading.Thread.Sleep(1);
                }
                if (m_SerialPort.BytesToRead >= receiveData.Length)
                    ret = m_SerialPort.Read(receiveData, 0, receiveData.Length);
                ReceiveEventFlag = false;       //打开事件
                return ret;
            }
            return -1;
        }

        public void ErrorEvent(object sender, SerialErrorReceivedEventArgs e)
        {
            if (ErrorHandler != null)
            {
                ErrorHandler(sender, e);
            }
        }

        //数据接收
        public void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //禁止接收事件时直接退出
            if (ReceiveEventFlag)
            {
                return;
            }
            byte[] data = new byte[m_SerialPort.BytesToRead];
            m_SerialPort.Read(data, 0, data.Length);
            if (ReceivedHandler != null)
            {
                ReceivedHandler(sender, new PortDataReciveEventArgs(data));
            }
        }

        public void Close()
        {
            if (m_SerialPort.IsOpen)
            {
                m_SerialPort.Close();
            }
        }

        public void Dispose()
        {
            Close();
        }
    }

    public delegate void PortDataReceivedEventHandle(object sender, PortDataReciveEventArgs e);
    public class PortDataReciveEventArgs : EventArgs
    {
        private byte[] data;

        public PortDataReciveEventArgs()
        {
            this.data = null;
        }

        public PortDataReciveEventArgs(byte[] data)
        {
            this.data = data;
        }

        public byte[] Data
        {
            get { return data; }
            set { data = value; }
        }
    }
}
