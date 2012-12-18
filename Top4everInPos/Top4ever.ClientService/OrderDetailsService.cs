using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.ClientService.Enum;
using Top4ever.Domain.OrderRelated;
using Top4ever.Entity;
using Newtonsoft.Json;

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
                socket.Connect();
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    result = true;
                }
                socket.Disconnect();
            }
            return result;
        }
    }
}
