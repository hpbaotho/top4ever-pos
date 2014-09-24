using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.ClientService.Enum;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;
using Newtonsoft.Json;

namespace Top4ever.ClientService
{
    public class BusinessReportService
    {
        private static readonly BusinessReportService Instance = new BusinessReportService();

        private BusinessReportService()
        { }

        public static BusinessReportService GetInstance()
        {
            return Instance;
        }

        public BusinessReport GetReportDataByHandover(string deviceNo)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.DEVICE_NO;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_REPORTDATABYHANDOVER), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            //deviceNo
            byte[] tempByte = Encoding.UTF8.GetBytes(deviceNo);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.DEVICE_NO;

            BusinessReport bizReport = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
                    bizReport = JsonConvert.DeserializeObject<BusinessReport>(strReceive);
                }
                socket.Close();
            }
            return bizReport;
        }

        public BusinessReport GetReportDataByHandoverRecordId(Guid handoverRecordId)
        {
            const int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.HANDOVER_RECORD_ID;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_REPORTDATABYHANDOVERRECORDID), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            //handoverRecordID
            byte[] tempByte = Encoding.UTF8.GetBytes(handoverRecordId.ToString());
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.HANDOVER_RECORD_ID;

            BusinessReport bizReport = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
                    bizReport = JsonConvert.DeserializeObject<BusinessReport>(strReceive);
                }
                socket.Close();
            }
            return bizReport;
        }

        /// <summary>
        /// 获取日结营业额统计
        /// </summary>
        /// <param name="dailyStatementNo">日结号</param>
        /// <returns></returns>
        public BusinessReport GetReportDataByDailyStatement(string dailyStatementNo)
        {
            const int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.DAILY_STATEMENT_NO;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_REPORTDATABYDAILYSTATEMENT), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            //dailyStatementNo
            byte[] tempByte = Encoding.UTF8.GetBytes(dailyStatementNo);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.DAILY_STATEMENT_NO;

            BusinessReport bizReport = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
                    bizReport = JsonConvert.DeserializeObject<BusinessReport>(strReceive);
                }
                socket.Close();
            }
            return bizReport;
        }

        public IList<GroupPrice> GetItemPriceListByGroup(string beginDate, string endDate)
        {
            const int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.BEGINDATE + ParamFieldLength.ENDDATE;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_ITEMPRICELISTBYGROUP), sendByte, BasicTypeLength.INT32);
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

            IList<GroupPrice> groupPriceList = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD);
                    groupPriceList = JsonConvert.DeserializeObject<IList<GroupPrice>>(strReceive);
                }
                socket.Close();
            }
            return groupPriceList;
        }
    }
}
