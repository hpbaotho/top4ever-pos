﻿using System;
using System.Text;

using Top4ever.ClientService.Enum;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;
using Newtonsoft.Json;

namespace Top4ever.ClientService
{
    public class VIPCardTradeService
    {
        private static readonly VIPCardTradeService Instance = new VIPCardTradeService();

        private VIPCardTradeService()
        { }

        public static VIPCardTradeService GetInstance()
        {
            return Instance;
        }

        public Int32 GetVIPCardTradeList(string cardNo, string beginDate, string endDate, ref VIPCardTradeRecord cardTradeRecord)
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

            int result = 0;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    result = BitConverter.ToInt32(receiveData, ParamFieldLength.PACKAGE_HEAD);
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD + BasicTypeLength.INT32, receiveData.Length - ParamFieldLength.PACKAGE_HEAD - BasicTypeLength.INT32).Trim('\0');
                    cardTradeRecord = JsonConvert.DeserializeObject<VIPCardTradeRecord>(strReceive);
                }
                socket.Close();
            }
            return result;
        }

        public Int32 AddVIPCardStoredValue(VIPCardAddMoney cardAddMoney, out string tradePayNo)
        {
            string json = JsonConvert.SerializeObject(cardAddMoney);
            byte[] jsonByte = Encoding.UTF8.GetBytes(json);

            int cByte = ParamFieldLength.PACKAGE_HEAD + jsonByte.Length;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_ADD_CARDSTOREDVALUE), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;
            Array.Copy(jsonByte, 0, sendByte, byteOffset, jsonByte.Length);
            byteOffset += jsonByte.Length;

            int result = 0;
            tradePayNo = string.Empty;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    result = BitConverter.ToInt32(receiveData, ParamFieldLength.PACKAGE_HEAD);
                    tradePayNo = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD + BasicTypeLength.INT32, receiveData.Length - ParamFieldLength.PACKAGE_HEAD - BasicTypeLength.INT32).Trim('\0');
                }
                socket.Close();
            }
            return result;
        }

        public Int32 AddVIPCardPayment(VIPCardPayment cardPayment, out string tradePayNo)
        {
            string json = JsonConvert.SerializeObject(cardPayment);
            byte[] jsonByte = Encoding.UTF8.GetBytes(json);

            int cByte = ParamFieldLength.PACKAGE_HEAD + jsonByte.Length;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_ADD_CARDPAYMENT), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;
            Array.Copy(jsonByte, 0, sendByte, byteOffset, jsonByte.Length);
            byteOffset += jsonByte.Length;

            int result = 0;
            tradePayNo = string.Empty;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    result = BitConverter.ToInt32(receiveData, ParamFieldLength.PACKAGE_HEAD);
                    tradePayNo = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD + BasicTypeLength.INT32, receiveData.Length - ParamFieldLength.PACKAGE_HEAD - BasicTypeLength.INT32).Trim('\0');
                }
                socket.Close();
            }
            return result;
        }

        public Int32 RefundVipCardPayment(string cardNo, string cardPassword, string tradePayNo)
        {
            const int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.CARD_NO + ParamFieldLength.CARD_PASSWORD + ParamFieldLength.TRADEPAYNO;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_REFUNDPAYMENT), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            byte[] tempByte = null;
            //cardNo
            tempByte = Encoding.UTF8.GetBytes(cardNo);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.CARD_NO;
            //cardPassword
            tempByte = Encoding.UTF8.GetBytes(cardPassword);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.CARD_PASSWORD;
            //tradePayNo
            tempByte = Encoding.UTF8.GetBytes(tradePayNo);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.TRADEPAYNO;

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
    }
}
