using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.ClientService.Enum;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;
using Newtonsoft.Json;

namespace Top4ever.ClientService
{
    public class OrderService
    {
        private static readonly OrderService Instance = new OrderService();

        private OrderService()
        { }

        public static OrderService GetInstance()
        {
            return Instance;
        }

        public IList<Order> GetOrderList(string deskName)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.DESK_NAME;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_ORDERLIST), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            //deskName
            byte[] tempByte = Encoding.UTF8.GetBytes(deskName);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.DESK_NAME;

            Int32 operCode = 0;
            IList<Order> orderList = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD);
                    orderList = JsonConvert.DeserializeObject<IList<Order>>(strReceive);
                }
                socket.Close();
            }
            return orderList;
        }

        public IList<Order> GetOrderListBySearch(string strWhere, string orderBy, int pageIndex, int pageSize)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.SQL_WHERE + ParamFieldLength.SQL_ORDERBY + BasicTypeLength.INT32 + BasicTypeLength.INT32;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_ORDERLISTBYSEARCH), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;
            //strWhere
            byte[] tempByte = Encoding.UTF8.GetBytes(strWhere);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.SQL_WHERE;
            //orderBy
            tempByte = Encoding.UTF8.GetBytes(orderBy);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.SQL_ORDERBY;
            //PageIndex
            Array.Copy(BitConverter.GetBytes(pageIndex), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;
            //PageSize
            Array.Copy(BitConverter.GetBytes(pageSize), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            Int32 operCode = 0;
            IList<Order> orderList = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD);
                    orderList = JsonConvert.DeserializeObject<IList<Order>>(strReceive);
                }
                socket.Close();
            }
            return orderList;
        }

        public bool OrderDeskOperate(DeskChange deskChange)
        {
            string json = JsonConvert.SerializeObject(deskChange);
            byte[] jsonByte = Encoding.UTF8.GetBytes(json);

            int cByte = ParamFieldLength.PACKAGE_HEAD + jsonByte.Length;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_ORDER_DESKOPERATE), sendByte, BasicTypeLength.INT32);
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

        public bool UpdateOrderStatus(Guid orderID, int status)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.ORDER_ID + BasicTypeLength.INT32;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_UPDATE_ORDERSTATUS), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            byte[] tempByte = null;
            //orderID
            tempByte = Encoding.UTF8.GetBytes(orderID.ToString());
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.ORDER_ID;
            //status
            Array.Copy(BitConverter.GetBytes(status), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

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

        public IList<DeliveryOrder> GetDeliveryOrderList()
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_DELIVERYORDERLIST), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            IList<DeliveryOrder> deliveryOrderList = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD);
                    deliveryOrderList = JsonConvert.DeserializeObject<IList<DeliveryOrder>>(strReceive);
                }
                socket.Close();
            }
            return deliveryOrderList;
        }

        public IList<HourOrderSales> GetHourSalesReport(DateTime beginTime, DateTime endTime)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.BEGINDATE + ParamFieldLength.ENDDATE;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_HOURSALESREPORT), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            byte[] tempByte = null;
            //beginTime
            tempByte = Encoding.UTF8.GetBytes(beginTime.ToString());
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.BEGINDATE;
            //endTime
            tempByte = Encoding.UTF8.GetBytes(endTime.ToString());
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.ENDDATE;

            IList<HourOrderSales> hourSalesList = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD);
                    hourSalesList = JsonConvert.DeserializeObject<IList<HourOrderSales>>(strReceive);
                }
                socket.Close();
            }
            return hourSalesList;
        }
    }
}
