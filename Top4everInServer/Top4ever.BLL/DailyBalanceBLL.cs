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

            string unCheckDeviceNo = string.Empty;  //未结账的设备号
            int result = DailyBalanceService.GetInstance().CreateDailyBalance(dailyBalance, out unCheckDeviceNo);
            byte[] strBuffer = Encoding.UTF8.GetBytes(unCheckDeviceNo);

            int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + BasicTypeLength.INT32 + ParamFieldLength.UNCHECK_DEVICE_NO;
            objRet = new byte[transCount];
            Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(result), 0, objRet, 2 * BasicTypeLength.INT32, BasicTypeLength.INT32);
            Array.Copy(strBuffer, 0, objRet, 3 * BasicTypeLength.INT32, strBuffer.Length);
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

        public static byte[] GetDailyBalanceTime(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string belongToDate = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, ParamFieldLength.BEGINDATE).Trim('\0');

            IList<DailyBalanceTime> dailyBalanceTimeList = DailyBalanceService.GetInstance().GetDailyBalanceTime(DateTime.Parse(belongToDate));
            if (dailyBalanceTimeList == null || dailyBalanceTimeList.Count <= 0)
            {
                //获取营业时间段失败或者没有数据
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            else
            {
                //成功
                string json = JsonConvert.SerializeObject(dailyBalanceTimeList);
                byte[] jsonByte = Encoding.UTF8.GetBytes(json);

                int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + jsonByte.Length;
                objRet = new byte[transCount];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
                Array.Copy(jsonByte, 0, objRet, 2 * BasicTypeLength.INT32, jsonByte.Length);
            }
            return objRet;
        }

        /// <summary>
        ///  获取一段时间内各个账务日的营业额
        /// </summary>
        public static byte[] GetDailyStatementInDays(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string beginDate = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, ParamFieldLength.BEGINDATE).Trim('\0');
            string endDate = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.BEGINDATE, ParamFieldLength.ENDDATE).Trim('\0');

            IList<DailyStatementInDay> dailyStatementList = DailyBalanceService.GetInstance().GetDailyStatementInDays(DateTime.Parse(beginDate), DateTime.Parse(endDate));
            if (dailyStatementList == null || dailyStatementList.Count == 0)
            {
                //获取单子失败
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            else
            {
                //成功
                string json = JsonConvert.SerializeObject(dailyStatementList);
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
