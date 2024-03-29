﻿using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.BLL.Enum;
using Top4ever.Domain.MembershipCard;
using Top4ever.Domain.Transfer;
using Top4ever.Service;
using Newtonsoft.Json;

namespace Top4ever.BLL
{
    public class VIPCardTradeBLL
    {
        public static byte[] GetVIPCardTradeList(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string cardNo = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, ParamFieldLength.CARD_NO).Trim('\0');
            string beginDate = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.CARD_NO, ParamFieldLength.BEGINDATE).Trim('\0');
            string endDate = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.CARD_NO + ParamFieldLength.BEGINDATE, ParamFieldLength.ENDDATE).Trim('\0');

            VIPCardTradeRecord cardTradeRecord;
            Int32 result = VIPCardTradeService.GetInstance().GetVIPCardTradeList(cardNo, DateTime.Parse(beginDate), DateTime.Parse(endDate), out cardTradeRecord);

            string json = JsonConvert.SerializeObject(cardTradeRecord);
            byte[] jsonByte = Encoding.UTF8.GetBytes(json);

            int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + BasicTypeLength.INT32 + jsonByte.Length;
            objRet = new byte[transCount];
            Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(result), 0, objRet, 2 * BasicTypeLength.INT32, BasicTypeLength.INT32);
            Array.Copy(jsonByte, 0, objRet, 3 * BasicTypeLength.INT32, jsonByte.Length);
            
            return objRet;
        }

        public static byte[] AddVIPCardStoredValue(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string strReceive = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, itemBuffer.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
            VIPCardAddMoney cardAddMoney = JsonConvert.DeserializeObject<VIPCardAddMoney>(strReceive);

            string tradePayNo = string.Empty;
            int result;
            //验证卡身份
            VIPCard card;
            int resultCode = VIPCardService.GetInstance().GetVIPCard(cardAddMoney.CardNo, cardAddMoney.CardPassword, out card);
            if (resultCode == 1 && card != null)
            {
                result = VIPCardTradeService.GetInstance().AddVIPCardStoredValue(cardAddMoney, out tradePayNo);
            }
            else
            {
                result = 99;    //会员卡号或者密码错误
            }
            byte[] buffer = Encoding.UTF8.GetBytes(tradePayNo);
            int transCount = ParamFieldLength.PACKAGE_HEAD + BasicTypeLength.INT32 + ParamFieldLength.TRADEPAYNO;
            objRet = new byte[transCount];
            Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(result), 0, objRet, ParamFieldLength.PACKAGE_HEAD, BasicTypeLength.INT32);
            Array.Copy(buffer, 0, objRet, ParamFieldLength.PACKAGE_HEAD + BasicTypeLength.INT32, buffer.Length);

            return objRet;
        }

        public static byte[] AddVIPCardPayment(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string strReceive = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, itemBuffer.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
            VIPCardPayment cardPayment = JsonConvert.DeserializeObject<VIPCardPayment>(strReceive);

            string tradePayNo = string.Empty;
            int result;
            //验证卡身份
            VIPCard card;
            int resultCode = VIPCardService.GetInstance().GetVIPCard(cardPayment.CardNo, cardPayment.CardPassword, out card);
            if (resultCode == 1 && card != null)
            {
                result = VIPCardTradeService.GetInstance().AddVIPCardPayment(cardPayment, out tradePayNo);
            }
            else
            {
                result = 99;    //会员卡号或者密码错误
            }
            byte[] buffer = Encoding.UTF8.GetBytes(tradePayNo);
            int transCount = ParamFieldLength.PACKAGE_HEAD + BasicTypeLength.INT32 + ParamFieldLength.TRADEPAYNO;
            objRet = new byte[transCount];
            Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(result), 0, objRet, ParamFieldLength.PACKAGE_HEAD, BasicTypeLength.INT32);
            Array.Copy(buffer, 0, objRet, ParamFieldLength.PACKAGE_HEAD + BasicTypeLength.INT32, buffer.Length);

            return objRet;
        }

        public static byte[] RefundVIPCardPayment(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string cardNo = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, ParamFieldLength.CARD_NO).Trim('\0');
            string cardPassword = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.CARD_NO, ParamFieldLength.CARD_PASSWORD).Trim('\0');
            string tradePayNo = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.CARD_NO + ParamFieldLength.CARD_PASSWORD, ParamFieldLength.TRADEPAYNO).Trim('\0');

            int result;
            //验证卡身份
            VIPCard card;
            int resultCode = VIPCardService.GetInstance().GetVIPCard(cardNo, cardPassword, out card);
            if (resultCode == 1 && card != null)
            {
                result = VIPCardTradeService.GetInstance().RefundVIPCardPayment(cardNo, tradePayNo);
            }
            else
            {
                result = 99;    //会员卡号或者密码错误
            }
            //返回对象
            objRet = new byte[ParamFieldLength.PACKAGE_HEAD + BasicTypeLength.INT32];
            Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD + BasicTypeLength.INT32), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(result), 0, objRet, ParamFieldLength.PACKAGE_HEAD, BasicTypeLength.INT32);
            return objRet;
        }
    }
}
