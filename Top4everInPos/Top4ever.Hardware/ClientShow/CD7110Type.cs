using System;
using System.Text;
using System.IO.Ports;

namespace Top4ever.Hardware.ClientShow
{
    /// <summary>
    /// 拍档顾显 CD7110
    /// </summary>
    public class CD7110Type : ISingleLineClientShow, IDisposable
    {
        private PortUtil m_PortUtil;

        public CD7110Type(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            m_PortUtil = new PortUtil(portName, baudRate, parity, dataBits, stopBits);
        }

        public CD7110Type(string usbVID, string usbPID, string endpointId)
        {
            m_PortUtil = new PortUtil(usbVID, usbPID, endpointId);
        }

        /// <summary>
        /// 清屏
        /// </summary>
        public void ClientShowCLS()
        {
            DisplayData(CustomerDispiayType.Clear, string.Empty);
        }

        /// <summary>
        /// 单价
        /// </summary>
        public void ShowUnitPrice(string balance)
        {
            DisplayData(CustomerDispiayType.Price, balance);
        }

        /// <summary>
        /// 总计
        /// </summary>
        public void ShowTotalPrice(string balance)
        {
            DisplayData(CustomerDispiayType.Total, balance);
        }

        /// <summary>
        /// 收款
        /// </summary>
        public void ShowReceipt(string balance)
        {
            DisplayData(CustomerDispiayType.Receipt, balance);
        }

        /// <summary>
        /// 找零
        /// </summary>
        public void ShowChange(string balance)
        {
            DisplayData(CustomerDispiayType.Change, balance);
        }

        public void Dispose()
        {
            m_PortUtil.Dispose();
        }

        /// <summary>  
        /// 客显类型  
        /// </summary> 
        private enum CustomerDispiayType
        {
            /// <summary>
            /// 清屏  
            /// </summary>
            Clear,
            /// <summary>
            /// 单价  
            /// </summary>
            Price,
            /// <summary>
            /// 合计  
            /// </summary>
            Total,
            /// <summary>
            /// 收款  
            /// </summary>
            Receipt,
            /// <summary>
            /// 找零  
            /// </summary>
            Change
        }

        /// <summary>  
        /// 数据信息展现  
        /// </summary>  
        /// <param name="data">发送的数据（清屏可以为null或者空）</param>  
        private void DisplayData(CustomerDispiayType dispiayType, string data)
        {
            m_PortUtil.Open();

            //先清屏  
            m_PortUtil.Write(((char)12).ToString());
            //指示灯  
            string status = ((char)27).ToString() + ((char)115).ToString() + ((char)(int)dispiayType).ToString();
            m_PortUtil.Write(status);
            //发送数据  
            if (!string.IsNullOrEmpty(data))
            {
                m_PortUtil.Write(((char)27).ToString() + ((char)81).ToString() + ((char)65).ToString() + data + ((char)13).ToString());
            }

            m_PortUtil.Close();
        }
    }
}
