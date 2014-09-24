using System;
using System.Text;

using Top4ever.ClientService.Enum;
using Top4ever.Domain;
using Newtonsoft.Json;

namespace Top4ever.ClientService
{
    public class ShopService
    {
        public Shop GetCurrentShop(string ipAddress, int port)
        {
            const int cByte = ParamFieldLength.PACKAGE_HEAD;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_SHOP), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            Shop currentShop = null;
            using (SocketClient socket = new SocketClient(ipAddress, port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int) RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD);
                    currentShop = JsonConvert.DeserializeObject<Shop>(strReceive);
                }
                socket.Close();
            }
            return currentShop;
        }
    }
}
