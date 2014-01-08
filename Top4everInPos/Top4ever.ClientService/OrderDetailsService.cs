using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.ClientService.Enum;
using Top4ever.Domain.OrderRelated;
using Top4ever.Entity;
using Newtonsoft.Json;
using Top4ever.Domain.Transfer;

namespace Top4ever.ClientService
{
    public class OrderDetailsService
    {
        public OrderDetailsService()
        { }

        public bool LadeOrderDetails(List<OrderDetails> orderDetailsList)
        {
            string json = JsonConvert.SerializeObject(orderDetailsList);
            byte[] jsonByte = Encoding.UTF8.GetBytes(json);

            int cByte = ParamFieldLength.PACKAGE_HEAD + jsonByte.Length;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_UPDATE_LADEORDERDETAILS), sendByte, BasicTypeLength.INT32);
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

        public DeletedAllItems GetAllDeletedItems(string beginDate, string endDate, int dateType)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.BEGINDATE + ParamFieldLength.ENDDATE + BasicTypeLength.INT32;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_ALLDELETEDITEMS), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            byte[] tempByte = null;
            //beginDate
            tempByte = Encoding.UTF8.GetBytes(beginDate);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.BEGINDATE;
            //endDate
            tempByte = Encoding.UTF8.GetBytes(endDate);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.ENDDATE;
            //dateType
            Array.Copy(BitConverter.GetBytes(dateType), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            DeletedAllItems deletedAllItems = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD);
                    deletedAllItems = JsonConvert.DeserializeObject<DeletedAllItems>(strReceive);
                }
                socket.Close();
            }
            return deletedAllItems;
        }

        public decimal GetLastCustomPrice(Guid goodsID)
        {
            byte[] bufferByte = Encoding.UTF8.GetBytes(goodsID.ToString());
            int cByte = ParamFieldLength.PACKAGE_HEAD + bufferByte.Length;
            byte[] sendByte = new byte[cByte];
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_LASTCUSTOMPRICE), sendByte, BasicTypeLength.INT32);
            int byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;
            Array.Copy(bufferByte, 0, sendByte, byteOffset, bufferByte.Length);
            byteOffset += bufferByte.Length;

            double result = 0;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    result = BitConverter.ToDouble(receiveData, ParamFieldLength.PACKAGE_HEAD);
                }
                socket.Close();
            }
            return (decimal)result;
        }
    }
}
