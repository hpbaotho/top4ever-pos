using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.BLL.Enum;
using Top4ever.Domain.Transfer;
using Top4ever.Service;
using Newtonsoft.Json;

namespace Top4ever.BLL
{
    public class HandoverBLL
    {
        public static byte[] CreateHandover(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string strReceive = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, itemBuffer.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
            HandoverInfo handover = JsonConvert.DeserializeObject<HandoverInfo>(strReceive);

            bool result = HandoverService.GetInstance().CreateHandover(handover);
            if (result)
            {
                //交班成功
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            else
            {
                //交班失败
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            return objRet;
        }
    }
}
