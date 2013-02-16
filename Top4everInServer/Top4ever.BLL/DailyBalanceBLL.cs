using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.BLL.Enum;
using Top4ever.Domain.Transfer;
using Top4ever.Service;
using Newtonsoft.Json;

namespace Top4ever.BLL
{
    public class DailyBalanceBLL
    {
        public static byte[] CreateDailyBalance(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string strReceive = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, itemBuffer.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
            DailyBalance dailyBalance = JsonConvert.DeserializeObject<DailyBalance>(strReceive);

            int result = DailyBalanceService.GetInstance().CreateDailyBalance(dailyBalance);

            int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + BasicTypeLength.INT32;
            objRet = new byte[transCount];
            Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(result), 0, objRet, 2 * BasicTypeLength.INT32, BasicTypeLength.INT32);
            return objRet;
        }

        public static byte[] GetDailyStatementTimeInterval()
        {
            byte[] objRet = null;
            string timeInterval = DailyBalanceService.GetInstance().GetDailyStatementTimeInterval();

            byte[] byteArr = Encoding.UTF8.GetBytes(timeInterval);
            int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + byteArr.Length;
            objRet = new byte[transCount];
            Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            Array.Copy(byteArr, 0, objRet, 2 * BasicTypeLength.INT32, byteArr.Length);
            return objRet;
        }

        public static byte[] CheckLastDailyStatement()
        {
            byte[] objRet = null;

            int result = DailyBalanceService.GetInstance().CheckLastDailyStatement();

            int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + BasicTypeLength.INT32;
            objRet = new byte[transCount];
            Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(result), 0, objRet, 2 * BasicTypeLength.INT32, BasicTypeLength.INT32);
            return objRet;
        }
    }
}
