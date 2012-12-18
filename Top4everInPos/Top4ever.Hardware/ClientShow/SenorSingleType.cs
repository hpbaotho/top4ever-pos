using System;
using System.Text;
using System.IO.Ports;

namespace Top4ever.Hardware.ClientShow
{
    public class SenorSingleType : ISingleLineClientShow, IDisposable
    {
        private PortUtil m_PortUtil;

        public SenorSingleType(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            m_PortUtil = new PortUtil(portName, baudRate, parity, dataBits, stopBits);
        }

        public SenorSingleType(string usbVID, string usbPID)
        {
            m_PortUtil = new PortUtil(usbVID, usbPID);
        }

        public void ClientShowCLS()
        {
            m_PortUtil.Open();

            byte[] data = new byte[] { 0x1b, 0x40 };
            string buf = "Welcome !";
            m_PortUtil.Write(data);
            m_PortUtil.Write(buf);

            m_PortUtil.Close();
        }

        /// <summary>
        /// 单价
        /// </summary>
        public void ShowUnitPrice(string balance)
        {
            m_PortUtil.Open();

            //清屏
            byte[] data = new byte[] { 0x1b, 0x40 };
            m_PortUtil.Write(data);
            //单价
            string strTitle = "PRICE:", strNull = string.Empty;
            for (int i = 0; i < 20 - strTitle.Length - balance.Length; i++)
            {
                strNull += " ";
            }
            m_PortUtil.Write(strTitle + strNull + balance);

            m_PortUtil.Close();
        }

        /// <summary>
        /// 总计
        /// </summary>
        public void ShowTotalPrice(string balance)
        {
            m_PortUtil.Open();

            //清屏
            byte[] data = new byte[] { 0x1b, 0x40 };
            m_PortUtil.Write(data);
            //总计
            string strTitle = "TOTAL:", strNull = string.Empty;
            for (int i = 0; i < 20 - strTitle.Length - balance.Length; i++)
            {
                strNull += " ";
            }
            m_PortUtil.Write(strTitle + strNull + balance);

            m_PortUtil.Close();
        }

        /// <summary>
        /// 收款
        /// </summary>
        public void ShowReceipt(string balance)
        {
            m_PortUtil.Open();

            //收款
            byte[] data = new byte[] { 0x1b, 0x40 };
            m_PortUtil.Write(data);
            //总计
            string strTitle = "RECEIPT:", strNull = string.Empty;
            for (int i = 0; i < 20 - strTitle.Length - balance.Length; i++)
            {
                strNull += " ";
            }
            m_PortUtil.Write(strTitle + strNull + balance);

            m_PortUtil.Close();
        }

        /// <summary>
        /// 找零
        /// </summary>
        public void ShowChange(string balance)
        {
            m_PortUtil.Open();

            //收款
            byte[] data = new byte[] { 0x1b, 0x40 };
            m_PortUtil.Write(data);
            //总计
            string strTitle = "CHANGE:", strNull = string.Empty;
            for (int i = 0; i < 20 - strTitle.Length - balance.Length; i++)
            {
                strNull += " ";
            }
            m_PortUtil.Write(strTitle + strNull + balance);

            m_PortUtil.Close();
        }

        public void Dispose()
        {
            m_PortUtil.Dispose();
        }
    }
}
