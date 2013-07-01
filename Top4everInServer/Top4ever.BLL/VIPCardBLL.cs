using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.BLL.Enum;
using Top4ever.Domain.MembershipCard;
using Top4ever.Service;
using Newtonsoft.Json;

namespace Top4ever.BLL
{
    public class VIPCardBLL
    {
        public static byte[] SearchVIPCard(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string cardNo = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, ParamFieldLength.CARD_NO).Trim('\0');
            string password = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.CARD_NO, ParamFieldLength.CARD_PASSWORD).Trim('\0');

            VIPCard card = null;
            int result = VIPCardService.GetInstance().GetVIPCard(cardNo, password, out card);
            if (result == 1)
            {
                //获取成功
                string json = JsonConvert.SerializeObject(card);
                byte[] jsonByte = Encoding.UTF8.GetBytes(json);

                int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + jsonByte.Length;
                objRet = new byte[transCount];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
                Array.Copy(jsonByte, 0, objRet, 2 * BasicTypeLength.INT32, jsonByte.Length);
            }
            else if (result == 2)
            {
                //会员卡号或者密码错误
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

        public static byte[] GetCardDiscountRate(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string cardNo = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, ParamFieldLength.CARD_NO).Trim('\0');

            decimal discountRate = VIPCardService.GetInstance().GetCardDiscountRate(cardNo);
            int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + BasicTypeLength.DOUBLE_DECIMAL;
            objRet = new byte[transCount];
            Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes((double)discountRate), 0, objRet, 2 * BasicTypeLength.INT32, BasicTypeLength.DOUBLE_DECIMAL);
            return objRet;
        }

        public static byte[] UpdateCardPassword(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string cardNo = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, ParamFieldLength.CARD_NO).Trim('\0');
            string currentPassword = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.CARD_NO, ParamFieldLength.CARD_PASSWORD).Trim('\0');
            string newPassword = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.CARD_NO + ParamFieldLength.CARD_PASSWORD, ParamFieldLength.CARD_PASSWORD).Trim('\0');

            bool result = VIPCardService.GetInstance().UpdateCardPassword(cardNo, currentPassword, newPassword);
            if (result)
            {
                //成功
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            else
            {
                //更新密码失败
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            return objRet;
        }

        public static byte[] UpdateVIPCardStatus(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string cardNo = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, ParamFieldLength.CARD_NO).Trim('\0');
            string password = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.CARD_NO, ParamFieldLength.CARD_PASSWORD).Trim('\0');
            int status = BitConverter.ToInt32(itemBuffer, ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.CARD_NO + ParamFieldLength.CARD_PASSWORD);

            int result = VIPCardService.GetInstance().UpdateVIPCardStatus(cardNo, password, status);
            //返回
            objRet = new byte[ParamFieldLength.PACKAGE_HEAD + BasicTypeLength.INT32];
            Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD + BasicTypeLength.INT32), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(result), 0, objRet, ParamFieldLength.PACKAGE_HEAD, BasicTypeLength.INT32);
            return objRet;
        }
    }
}
