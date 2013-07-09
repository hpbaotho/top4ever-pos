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
    public class OrderDetailsBLL
    {
        public static byte[] LadeOrderDetails(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string strReceive = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, itemBuffer.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
            List<OrderDetails> orderDetailsList = JsonConvert.DeserializeObject<List<OrderDetails>>(strReceive);
            bool result = true;
            foreach (OrderDetails item in orderDetailsList)
            {
                result = OrderDetailsService.GetInstance().LadeOrderDetails(item);
                if (!result)
                {
                    break;
                }
            }
            if (result)
            {
                //成功
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            else
            {
                //单品提单失败
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            return objRet;
        }

        public static byte[] GetAllDeletedItems(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string beginDate = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, ParamFieldLength.BEGINDATE).Trim('\0');
            string endDate = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.BEGINDATE, ParamFieldLength.ENDDATE).Trim('\0');
            int dateType = BitConverter.ToInt32(itemBuffer, ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.BEGINDATE + ParamFieldLength.ENDDATE);

            DeletedAllItems deletedAllItems = OrderDetailsService.GetInstance().GetAllDeletedItems(DateTime.Parse(beginDate), DateTime.Parse(endDate), dateType);
            string json = JsonConvert.SerializeObject(deletedAllItems);
            byte[] jsonByte = Encoding.UTF8.GetBytes(json);

            int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + jsonByte.Length;
            objRet = new byte[transCount];
            Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            Array.Copy(jsonByte, 0, objRet, 2 * BasicTypeLength.INT32, jsonByte.Length);
            return objRet;
        }
    }
}
