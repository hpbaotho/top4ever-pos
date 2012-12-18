using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.BLL.Enum;
using Top4ever.Domain;
using Top4ever.Domain.Transfer;
using Top4ever.Service;
using Newtonsoft.Json;

namespace Top4ever.BLL
{
    public class SysBasicDataBLL
    {
        public static byte[] GetSysBasicData(byte[] itemBuffer)
        {
            byte[] objRet = null;
            SysBasicData sysBasicData = SysBasicDataService.GetInstance().GetSysBasicData();
            if (sysBasicData == null)
            {
                //数据获取失败
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            else
            {
                string json = JsonConvert.SerializeObject(sysBasicData);
                byte[] jsonByte = Encoding.UTF8.GetBytes(json);

                int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + jsonByte.Length;
                objRet = new byte[transCount];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
                Array.Copy(jsonByte, 0, objRet, 2 * BasicTypeLength.INT32, jsonByte.Length);
            }
            return objRet;
        }
    }
}
