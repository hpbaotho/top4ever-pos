using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.ClientService.Enum;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;
using Newtonsoft.Json;

namespace Top4ever.ClientService
{
    public class DailyBalanceService
    {
        private static readonly DailyBalanceService Instance = new DailyBalanceService();

        private DailyBalanceService()
        { }

        public static DailyBalanceService GetInstance()
        {
            return Instance;
        }

        /// <summary>
        /// 日结操作
        /// </summary>
        /// <param name="dailyBalance">日结号</param>
        /// <param name="unCheckDeviceNo">未结账的设备号</param>
        /// <returns></returns>
        public Int32 CreateDailyBalance(DailyBalance dailyBalance, out string unCheckDeviceNo)
        {
            string json = JsonConvert.SerializeObject(dailyBalance);
            byte[] jsonByte = Encoding.UTF8.GetBytes(json);

            int cByte = ParamFieldLength.PACKAGE_HEAD + jsonByte.Length;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_CREATE_DAILYBALANCE), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;
            Array.Copy(jsonByte, 0, sendByte, byteOffset, jsonByte.Length);
            byteOffset += jsonByte.Length;

            int result = 0;
            unCheckDeviceNo = string.Empty;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    result = BitConverter.ToInt32(receiveData, ParamFieldLength.PACKAGE_HEAD);
                    unCheckDeviceNo = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD + BasicTypeLength.INT32, receiveData.Length - ParamFieldLength.PACKAGE_HEAD - BasicTypeLength.INT32).Trim('\0');
                }
                socket.Close();
            }
            return result;
        }

        public string GetDailyStatementTimeInterval()
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_DAILYTIMEINTERVAL), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            string dailyTimeInterval = string.Empty;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    dailyTimeInterval = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
                }
                socket.Close();
            }
            return dailyTimeInterval;
        }

        public Int32 CheckLastDailyStatement(int breakDays)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + BasicTypeLength.INT32;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_CHECK_LASTDAILYSTATEMENT), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(breakDays), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

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

        /// <summary>
        /// 根据所属日期获取营业时间段
        /// </summary>
        /// <param name="belongToDate">所属日期</param>
        /// <returns></returns>
        public IList<DailyBalanceTime> GetDailyBalanceTime(DateTime belongToDate)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.BEGINDATE;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_DAILYBALANCETIME), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            //belongToDate
            byte[] tempByte = Encoding.UTF8.GetBytes(belongToDate.ToString("yyyy-MM-dd"));
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.BEGINDATE;

            Int32 operCode = 0;
            IList<DailyBalanceTime> dailyBalanceTimeList = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD);
                    dailyBalanceTimeList = JsonConvert.DeserializeObject<IList<DailyBalanceTime>>(strReceive);
                }
                socket.Close();
            }
            return dailyBalanceTimeList;
        }

        /// <summary>
        ///  获取一段时间内各个账务日的营业额
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public IList<DailyStatementInDay> GetDailyStatementInDays(string beginDate, string endDate)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.BEGINDATE + ParamFieldLength.ENDDATE;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_DAILYSTATEMENTINDAYS), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            byte[] tempByte = null;
            //beginDate
            tempByte = Encoding.UTF8.GetBytes(beginDate);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.BEGINDATE;
            //endDate
            tempByte = Encoding.UTF8.GetBytes(endDate);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.ENDDATE;

            IList<DailyStatementInDay> dailyStatementList = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD);
                    dailyStatementList = JsonConvert.DeserializeObject<IList<DailyStatementInDay>>(strReceive);
                }
                socket.Close();
            }
            return dailyStatementList;
        }
    }
}
