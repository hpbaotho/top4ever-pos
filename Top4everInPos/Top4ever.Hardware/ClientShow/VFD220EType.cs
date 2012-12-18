using System;
using System.Text;
using System.IO.Ports;

namespace Top4ever.Hardware.ClientShow
{
    public class VFD220EType : IDualLineClientShow, IDisposable
    {
        private PortUtil m_PortUtil;

        public VFD220EType(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            m_PortUtil = new PortUtil(portName, baudRate, parity, dataBits, stopBits);
        }

        public VFD220EType(string usbVID, string usbPID)
        {
            m_PortUtil = new PortUtil(usbVID, usbPID);
        }

        public void ClientShowCLS()
        {
            m_PortUtil.Open();

            byte[] data = new byte[] { 0x1b, 0x40 };
            m_PortUtil.Write(data);

            m_PortUtil.Close();
        }

        public void ShowUnitAndTotalPrice(string unitPrice, string totalPrice)
        {
            m_PortUtil.Open();

            //清屏
            byte[] data = new byte[] { 0x1b, 0x40 };
            m_PortUtil.Write(data);
            //第一排
            string strTitle = "PRICE:", strNull = string.Empty;
            for (int i = 0; i < 20 - strTitle.Length - unitPrice.Length; i++)
            {
                strNull += " ";
            }
            unitPrice = strTitle + strNull + unitPrice;
            data = new byte[4 + unitPrice.Length];
            data[0] = (byte)'\x1B';
            data[1] = (byte)'\x51';
            data[2] = (byte)'\x41';
            int cByte = 3;
            byte[] bytes = Encoding.Default.GetBytes(unitPrice);
            for (int i = 0; i < unitPrice.Length; i++)
            {
                data[cByte] = bytes[i];
                cByte++;
            }
            data[cByte] = (byte)'\x0D';
            m_PortUtil.Write(data);
            //第二排
            strTitle = "TOTAL:";
            strNull = string.Empty;
            for (int i = 0; i < 20 - strTitle.Length - totalPrice.Length; i++)
            {
                strNull += " ";
            }
            totalPrice = strTitle + strNull + totalPrice;
            data = new byte[4 + totalPrice.Length];
            data[0] = (byte)'\x1B';
            data[1] = (byte)'\x51';
            data[2] = (byte)'\x42';
            cByte = 3;
            bytes = Encoding.Default.GetBytes(totalPrice);
            for (int i = 0; i < totalPrice.Length; i++)
            {
                data[cByte] = bytes[i];
                cByte++;
            }
            data[cByte] = (byte)'\x0D';
            m_PortUtil.Write(data);

            m_PortUtil.Close();
        }

        public void ShowReceiptAndChange(string receipt, string change)
        {
            m_PortUtil.Open();

            //清屏
            byte[] data = new byte[] { 0x1b, 0x40 };
            m_PortUtil.Write(data);
            //第一排
            string strTitle = "RECEIPT:", strNull = string.Empty;
            for (int i = 0; i < 20 - strTitle.Length - receipt.Length; i++)
            {
                strNull += " ";
            }
            receipt = strTitle + strNull + receipt;
            data = new byte[4 + receipt.Length];
            data[0] = (byte)'\x1B';
            data[1] = (byte)'\x51';
            data[2] = (byte)'\x41';
            int cByte = 3;
            byte[] bytes = Encoding.Default.GetBytes(receipt);
            for (int i = 0; i < receipt.Length; i++)
            {
                data[cByte] = bytes[i];
                cByte++;
            }
            data[cByte] = (byte)'\x0D';
            m_PortUtil.Write(data);
            //第二排
            strTitle = "CHANGE:";
            strNull = string.Empty;
            for (int i = 0; i < 20 - strTitle.Length - change.Length; i++)
            {
                strNull += " ";
            }
            change = strTitle + strNull + change;
            data = new byte[4 + change.Length];
            data[0] = (byte)'\x1B';
            data[1] = (byte)'\x51';
            data[2] = (byte)'\x42';
            cByte = 3;
            bytes = Encoding.Default.GetBytes(change);
            for (int i = 0; i < change.Length; i++)
            {
                data[cByte] = bytes[i];
                cByte++;
            }
            data[cByte] = (byte)'\x0D';
            m_PortUtil.Write(data);

            m_PortUtil.Close();
        }

        public void Dispose()
        {
            m_PortUtil.Dispose();
        }
    }
}
