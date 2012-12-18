using System;
using System.IO.Ports;

namespace Top4ever.Hardware
{
    public class CashBox
    {
        public static void Open(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            PortUtil portUtil = new PortUtil(portName, baudRate, parity, dataBits, stopBits);
            portUtil.Open();
            //弹钱箱
            byte[] cashDrawer = new byte[] { 0x1B, 0x70, 0x00, 0x30, 0xC0 };
            portUtil.Write(cashDrawer);
            portUtil.Close();
        }

        public static void Open(string VID, string PID)
        {
            PortUtil portUtil = new PortUtil(VID, PID);
            portUtil.Open();
            //弹钱箱
            byte[] cashDrawer = new byte[] { 0x1B, 0x70, 0x00, 0x30, 0xC0 };
            portUtil.Write(cashDrawer);
            portUtil.Close();
        }

        public static void Open(string LPTName)
        {
            LPTHelper helper = new LPTHelper(LPTName);
            //弹钱箱
            string data = ((char)27).ToString() + "p" + ((char)0).ToString() + ((char)60).ToString() + ((char)255).ToString();
            helper.Write(data);
            helper.Close();
        }
    }
}
