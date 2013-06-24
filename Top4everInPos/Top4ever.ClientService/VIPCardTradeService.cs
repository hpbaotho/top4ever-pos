using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.ClientService.Enum;
using Top4ever.Domain.MembershipCard;
using Top4ever.Entity;
using Newtonsoft.Json;

namespace Top4ever.ClientService
{
    public class VIPCardTradeService
    {
        public VIPCardTradeService()
        { }

        public IList<VIPCardTrade> GetCardTradeList(string cardNo, string beginDate, string endDate)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.CARD_NO + ParamFieldLength.BEGINDATE + ParamFieldLength.ENDDATE;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_VIPCARDTRADELIST), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            byte[] tempByte = null;
            //cardNo
            tempByte = Encoding.UTF8.GetBytes(cardNo);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.CARD_NO;
            //beginDate
            tempByte = Encoding.UTF8.GetBytes(beginDate);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.BEGINDATE;
            //endDate
            tempByte = Encoding.UTF8.GetBytes(endDate);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.ENDDATE;

            IList<VIPCardTrade> cardTradeList = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                socket.Connect();
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
                    cardTradeList = JsonConvert.DeserializeObject<IList<VIPCardTrade>>(strReceive);
                }
                socket.Disconnect();
            }
            return cardTradeList;
        }
    }
}
