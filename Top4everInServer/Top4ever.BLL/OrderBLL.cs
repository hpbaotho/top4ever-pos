using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.BLL.Enum;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Service;
using Newtonsoft.Json;

namespace Top4ever.BLL
{
    public class OrderBLL
    {
        public static byte[] GetOrderList(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string deskName = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, ParamFieldLength.DESK_NAME).Trim('\0');

            IList<Order> orderList = OrderService.GetInstance().GetOrderList(deskName);
            if (orderList == null || orderList.Count == 0)
            {
                //获取单子失败
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            else
            {
                //成功
                string json = JsonConvert.SerializeObject(orderList);
                byte[] jsonByte = Encoding.UTF8.GetBytes(json);

                int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + jsonByte.Length;
                objRet = new byte[transCount];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
                Array.Copy(jsonByte, 0, objRet, 2 * BasicTypeLength.INT32, jsonByte.Length);
            }
            return objRet;
        }

        public static byte[] GetOrderListBySearch(byte[] itemBuffer)
        {
            byte[] objRet = null;
            int index = ParamFieldLength.PACKAGE_HEAD;
            string strWhere = Encoding.UTF8.GetString(itemBuffer, index, ParamFieldLength.SQL_WHERE).Trim('\0');
            index += ParamFieldLength.SQL_WHERE;
            string orderBy = Encoding.UTF8.GetString(itemBuffer, index, ParamFieldLength.SQL_ORDERBY).Trim('\0');
            index += ParamFieldLength.SQL_ORDERBY;
            int pageIndex = BitConverter.ToInt32(itemBuffer, index);
            index += BasicTypeLength.INT32;
            int pageSize = BitConverter.ToInt32(itemBuffer, index);

            IList<Order> orderList = OrderService.GetInstance().GetOrderListBySearch(strWhere, orderBy, pageIndex, pageSize);
            if (orderList == null || orderList.Count == 0)
            {
                //获取单子失败
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            else
            {
                //成功
                string json = JsonConvert.SerializeObject(orderList);
                byte[] jsonByte = Encoding.UTF8.GetBytes(json);

                int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + jsonByte.Length;
                objRet = new byte[transCount];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
                Array.Copy(jsonByte, 0, objRet, 2 * BasicTypeLength.INT32, jsonByte.Length);
            }
            return objRet;
        }

        public static byte[] OrderDeskOperate(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string strReceive = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, itemBuffer.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
            DeskChange deskChange = JsonConvert.DeserializeObject<DeskChange>(strReceive);

            bool result = OrderService.GetInstance().OrderDeskOperate(deskChange);
            if (result)
            {
                //成功
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            else
            {
                //桌子操作失败
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            return objRet;
        }

        public static byte[] UpdateOrderStatus(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string orderID = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, ParamFieldLength.ORDER_ID).Trim('\0');
            int status = BitConverter.ToInt32(itemBuffer, ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.ORDER_ID);

            bool result = OrderService.GetInstance().UpdateOrderStatus(new Guid(orderID), status);
            if (result)
            {
                //更新状态成功
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            else
            {
                //更新状态失败
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            return objRet;
        }

        public static byte[] GetDeliveryOrderList(byte[] itemBuffer)
        {
            byte[] objRet = null;
            IList<DeliveryOrder> deliveryOrderList = OrderService.GetInstance().GetDeliveryOrderList();
            if (deliveryOrderList == null)
            {
                //数据获取失败
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            else
            {
                string json = JsonConvert.SerializeObject(deliveryOrderList);
                byte[] jsonByte = Encoding.UTF8.GetBytes(json);

                int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + jsonByte.Length;
                objRet = new byte[transCount];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
                Array.Copy(jsonByte, 0, objRet, 2 * BasicTypeLength.INT32, jsonByte.Length);
            }
            return objRet;
        }
    }
}
