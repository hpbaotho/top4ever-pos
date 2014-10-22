using System;
using System.Text;
using System.IO.Ports;

namespace Top4ever.Hardware.ClientShow
{
    public class SenorDualType : IDualLineClientShow, IDisposable
    {
        private PortUtil m_PortUtil;

        public SenorDualType(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            m_PortUtil = new PortUtil(portName, baudRate, parity, dataBits, stopBits);
        }

        public SenorDualType(string usbVID, string usbPID, string endpointId)
        {
            m_PortUtil = new PortUtil(usbVID, usbPID, endpointId);
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

        public void ShowUnitAndTotalPrice(string unitPrice, string totalPrice)
        {
            m_PortUtil.Open();

            //清屏
            byte[] data = new byte[] { 0x1b, 0x40 };
            m_PortUtil.Write(data);
            //第一排
            data = new byte[] { 0x1F, 0x24, 0x01, 0x01 };
            m_PortUtil.Write(data);
            string strTitle = "PRICE:", strNull = string.Empty;
            for (int i = 0; i < 20 - strTitle.Length - unitPrice.Length; i++)
            {
                strNull += " ";
            }
            m_PortUtil.Write(strTitle + strNull + unitPrice);
            //第二排
            data = new byte[] { 0x1F, 0x24, 0x01, 0x02 };
            m_PortUtil.Write(data);
            strTitle = "TOTAL:";
            strNull = string.Empty;
            for (int i = 0; i < 20 - strTitle.Length - totalPrice.Length; i++)
            {
                strNull += " ";
            }
            m_PortUtil.Write(strTitle + strNull + totalPrice);

            m_PortUtil.Close();
        }

        public void ShowReceiptAndChange(string receipt, string change)
        {
            m_PortUtil.Open();

            //清屏
            byte[] data = new byte[] { 0x1b, 0x40 };
            m_PortUtil.Write(data);
            //第一排
            data = new byte[] { 0x1F, 0x24, 0x01, 0x01 };
            m_PortUtil.Write(data);
            string strTitle = "RECEIPT:", strNull = string.Empty;
            for (int i = 0; i < 20 - strTitle.Length - receipt.Length; i++)
            {
                strNull += " ";
            }
            m_PortUtil.Write(strTitle + strNull + receipt);
            //第二排
            data = new byte[] { 0x1F, 0x24, 0x01, 0x02 };
            m_PortUtil.Write(data);
            strTitle = "CHANGE:";
            strNull = string.Empty;
            for (int i = 0; i < 20 - strTitle.Length - change.Length; i++)
            {
                strNull += " ";
            }
            m_PortUtil.Write(strTitle + strNull + change);

            m_PortUtil.Close();
        }

        public void Dispose()
        {
            m_PortUtil.Dispose();
        }
    }
}
