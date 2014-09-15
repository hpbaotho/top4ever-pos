using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.BLL.Enum;
using Top4ever.Domain.Accounts;
using Top4ever.Domain.Transfer;
using Top4ever.Service;
using Newtonsoft.Json;

namespace Top4ever.BLL
{
    public class SalesOrderBLL
    {
        public static byte[] CreateSalesOrder(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string strReceive = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, itemBuffer.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
            SalesOrder salesOrder = JsonConvert.DeserializeObject<SalesOrder>(strReceive);
            //返回流水号
            Int32 tranSequence = SalesOrderService.GetInstance().CreateSalesOrder(salesOrder);
            int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + BasicTypeLength.INT32;
            objRet = new byte[transCount];
            Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(tranSequence), 0, objRet, 2 * BasicTypeLength.INT32, BasicTypeLength.INT32);
            return objRet;
        }

        public static byte[] UpdateSalesOrder(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string strReceive = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, itemBuffer.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
            SalesOrder salesOrder = JsonConvert.DeserializeObject<SalesOrder>(strReceive);

            int result = SalesOrderService.GetInstance().UpdateSalesOrder(salesOrder);

            int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + BasicTypeLength.INT32;
            objRet = new byte[transCount];
            Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(result), 0, objRet, 2 * BasicTypeLength.INT32, BasicTypeLength.INT32);
            return objRet;
        }

        public static byte[] GetSalesOrder(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string orderID = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, ParamFieldLength.ORDER_ID).Trim('\0');

            SalesOrder salesOrder = SalesOrderService.GetInstance().GetSalesOrder(new Guid(orderID));
            if (salesOrder == null || salesOrder.orderDetailsList == null || salesOrder.orderDetailsList.Count == 0)
            {
                //获取单子失败
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            else
            {
                //成功
                string json = JsonConvert.SerializeObject(salesOrder);
                byte[] jsonByte = Encoding.UTF8.GetBytes(json);

                int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + jsonByte.Length;
                objRet = new byte[transCount];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
                Array.Copy(jsonByte, 0, objRet, 2 * BasicTypeLength.INT32, jsonByte.Length);
            }
            return objRet;
        }

        public static byte[] SplitSalesOrder(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string strReceive = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, itemBuffer.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
            SalesSplitOrder salesSplitOrder = JsonConvert.DeserializeObject<SalesSplitOrder>(strReceive);

            bool result = SalesOrderService.GetInstance().SplitSalesOrder(salesSplitOrder);
            if (result)
            {
                //成功
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            else
            {
                //分单操作失败
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            return objRet;
        }

        public static byte[] GetSalesOrderByBillSearch(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string orderID = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, ParamFieldLength.ORDER_ID).Trim('\0');

            SalesOrder salesOrder = SalesOrderService.GetInstance().GetSalesOrderByBillSearch(new Guid(orderID));
            if (salesOrder == null || salesOrder.orderDetailsList == null || salesOrder.orderDetailsList.Count == 0)
            {
                //获取已支付单据失败
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            else
            {
                //获取已支付单据成功
                string json = JsonConvert.SerializeObject(salesOrder);
                byte[] jsonByte = Encoding.UTF8.GetBytes(json);

                int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + jsonByte.Length;
                objRet = new byte[transCount];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
                Array.Copy(jsonByte, 0, objRet, 2 * BasicTypeLength.INT32, jsonByte.Length);
            }
            return objRet;
        }

        public static byte[] SubmitOrderInAndroid(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string strReceive = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, itemBuffer.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
            OrderInAndroid orderInAndroid = JsonConvert.DeserializeObject<OrderInAndroid>(strReceive);
            
            //返回操作结果
            Employee employee = null;
            int result = EmployeeService.GetInstance().GetEmployee(orderInAndroid.EmployeeNo, orderInAndroid.Password, out employee);
            if (result == 1)
            {
                bool isSuccess = SalesOrderService.GetInstance().CreateOrderInAndroid(orderInAndroid.DeskName, orderInAndroid.PeopleNum, employee.EmployeeID, employee.EmployeeNo, orderInAndroid.OrderDetailList);
                if (isSuccess)
                {
                    //成功
                    objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                    Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                    Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
                }
                else
                {
                    //创建Android订单失败
                    objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                    Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                    Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
                }
            }
            else if (result == 2)
            {
                //账号或者密码错误
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_AUTHENTICATION), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            else
            {
                //数据库操作失败
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            return objRet;
        }
    }
}
