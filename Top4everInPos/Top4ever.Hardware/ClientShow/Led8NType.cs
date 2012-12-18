using System;
using System.Text;
using System.IO.Ports;

namespace Top4ever.Hardware.ClientShow
{
    public class Led8NType : ISingleLineClientShow, IDisposable
    {
        private PortUtil m_PortUtil;

        public Led8NType(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            m_PortUtil = new PortUtil(portName, baudRate, parity, dataBits, stopBits); 
        }

        public Led8NType(string usbVID, string usbPID)
        {
            m_PortUtil = new PortUtil(usbVID, usbPID);
        }

        /// <summary>
        /// 清屏
        /// </summary>
        public void ClientShowCLS()
        {
            m_PortUtil.Open();
            
            //初始化
            byte[] initial = new byte[] { 0x1b, 0x40 };
            //清屏
            byte[] cls = new byte[] { 0x0C };
            m_PortUtil.Write(initial);
            m_PortUtil.Write(cls);

            m_PortUtil.Close();
        }

        /// <summary>
        /// 单价
        /// </summary>
        public void ShowUnitPrice(string balance)
        {
            m_PortUtil.Open();

            //初始化
            byte[] chs = new byte[] { 0x1b, 0x40 };
            //发送数据
            byte[] rec = Encoding.UTF8.GetBytes(balance);
            byte[] buf = new byte[4 + rec.Length];
            buf[0] = 0x1B;
            buf[1] = 0x51;
            buf[2] = 0x41;
            for (int i = 0; i < rec.Length; i++)
            {
                buf[i + 3] = rec[i];
            }
            buf[buf.Length - 1] = 0x0D;
            //发送LED状态
            byte[] status = new byte[] { 0x1B, 0x73, 0x31 };
            m_PortUtil.Write(chs);
            m_PortUtil.Write(buf);
            m_PortUtil.Write(status);

            m_PortUtil.Close();
        }

        /// <summary>
        /// 总计
        /// </summary>
        public void ShowTotalPrice(string balance)
        {
            m_PortUtil.Open();

            //初始化
            byte[] chs = new byte[] { 0x1b, 0x40 };
            //发送数据
            byte[] rec = Encoding.UTF8.GetBytes(balance);
            byte[] buf = new byte[4 + rec.Length];
            buf[0] = 0x1B;
            buf[1] = 0x51;
            buf[2] = 0x41;
            for (int i = 0; i < rec.Length; i++)
            {
                buf[i + 3] = rec[i];
            }
            buf[buf.Length - 1] = 0x0D;
            //发送LED状态
            byte[] status = new byte[] { 0x1B, 0x73, 0x32 };
            m_PortUtil.Write(chs);
            m_PortUtil.Write(buf);
            m_PortUtil.Write(status);

            m_PortUtil.Close();
        }

        /// <summary>
        /// 收款
        /// </summary>
        public void ShowReceipt(string balance)
        {
            m_PortUtil.Open();

            //初始化
            byte[] chs = new byte[] { 0x1b, 0x40 };
            //发送数据
            byte[] rec = Encoding.UTF8.GetBytes(balance);
            byte[] buf = new byte[4 + rec.Length];
            buf[0] = 0x1B;
            buf[1] = 0x51;
            buf[2] = 0x41;
            for (int i = 0; i < rec.Length; i++)
            {
                buf[i + 3] = rec[i];
            }
            buf[buf.Length - 1] = 0x0D;
            //发送LED状态
            byte[] status = new byte[] { 0x1B, 0x73, 0x33 };
            m_PortUtil.Write(chs);
            m_PortUtil.Write(buf);
            m_PortUtil.Write(status);

            m_PortUtil.Close();
        }

        /// <summary>
        /// 找零
        /// </summary>
        public void ShowChange(string balance)
        {
            m_PortUtil.Open();

            //初始化
            byte[] chs = new byte[] { 0x1b, 0x40 };
            //发送数据
            byte[] rec = Encoding.UTF8.GetBytes(balance);
            byte[] buf = new byte[4 + rec.Length];
            buf[0] = 0x1B;
            buf[1] = 0x51;
            buf[2] = 0x41;
            for (int i = 0; i < rec.Length; i++)
            {
                buf[i + 3] = rec[i];
            }
            buf[buf.Length - 1] = 0x0D;
            //发送LED状态
            byte[] status = new byte[] { 0x1B, 0x73, 0x34 };
            m_PortUtil.Write(chs);
            m_PortUtil.Write(buf);
            m_PortUtil.Write(status);

            m_PortUtil.Close();
        }

        public void Dispose()
        {
            m_PortUtil.Dispose();
        }
    }
}
