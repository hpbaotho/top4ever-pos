using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.ClientService.Enum;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;
using Newtonsoft.Json;

namespace Top4ever.ClientService
{
    public class SalesOrderService
    {
        private static readonly SalesOrderService Instance = new SalesOrderService();

        private SalesOrderService()
        { }

        public static SalesOrderService GetInstance()
        {
            return Instance;
        }

        public Int32 CreateSalesOrder(SalesOrder salesOrder)
        {
            string json = JsonConvert.SerializeObject(salesOrder);
            byte[] jsonByte = Encoding.UTF8.GetBytes(json);

            int cByte = ParamFieldLength.PACKAGE_HEAD + jsonByte.Length;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_CREATE_SALESORDER), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;
            Array.Copy(jsonByte, 0, sendByte, byteOffset, jsonByte.Length);
            byteOffset += jsonByte.Length;

            int result = 0;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    result = BitConverter.ToInt32(receiveData, ParamFieldLength.PACKAGE_HEAD);
                }
                socket.Close();
            }
            return result;
        }

        public Int32 UpdateSalesOrder(SalesOrder salesOrder)
        {
            string json = JsonConvert.SerializeObject(salesOrder);
            byte[] jsonByte = Encoding.UTF8.GetBytes(json);

            int cByte = ParamFieldLength.PACKAGE_HEAD + jsonByte.Length;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_UPDATE_SALESORDER), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;
            Array.Copy(jsonByte, 0, sendByte, byteOffset, jsonByte.Length);
            byteOffset += jsonByte.Length;

            int result = 0;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    result = BitConverter.ToInt32(receiveData, ParamFieldLength.PACKAGE_HEAD);
                }
                socket.Close();
            }
            return result;
        }

        public SalesOrder GetSalesOrder(Guid orderId)
        {
            if(orderId.Equals(Guid.Empty)) return null;

            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.ORDER_ID;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_SALESORDER), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            //orderID
            byte[] tempByte = Encoding.UTF8.GetBytes(orderId.ToString());
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.ORDER_ID;

            Int32 operCode = 0;
            SalesOrder salesOrder = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD);
                    salesOrder = JsonConvert.DeserializeObject<SalesOrder>(strReceive);
                }
                socket.Close();
            }
            return salesOrder;
        }

        public bool SplitSalesOrder(SalesSplitOrder salesSplitOrder)
        {
            string json = JsonConvert.SerializeObject(salesSplitOrder);
            byte[] jsonByte = Encoding.UTF8.GetBytes(json);

            int cByte = ParamFieldLength.PACKAGE_HEAD + jsonByte.Length;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_SPLIT_SALESORDER), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;
            Array.Copy(jsonByte, 0, sendByte, byteOffset, jsonByte.Length);
            byteOffset += jsonByte.Length;

            bool result = false;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    result = true;
                }
                socket.Close();
            }
            return result;
        }

        public SalesOrder GetSalesOrderByBillSearch(Guid orderID)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.ORDER_ID;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_SALESORDERBYBILLSEARCH), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            //orderID
            byte[] tempByte = Encoding.UTF8.GetBytes(orderID.ToString());
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.ORDER_ID;

            Int32 operCode = 0;
            SalesOrder salesOrder = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD);
                    salesOrder = JsonConvert.DeserializeObject<SalesOrder>(strReceive);
                }
                socket.Close();
            }
            return salesOrder;
        }
    }
}
