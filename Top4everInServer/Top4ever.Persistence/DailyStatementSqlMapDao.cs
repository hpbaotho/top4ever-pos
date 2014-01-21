using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;

namespace Top4ever.Persistence
{
    /// <summary>
    /// Summary description for DailyStatementSqlMapDao
    /// </summary>
    public class DailyStatementSqlMapDao : BaseSqlMapDao, IDailyStatementDao
    {
        #region IDailyStatementDao Members

        public string GetCurrentDailyStatementNo()
        {
            string dailySerialNum = string.Empty;
            object objValue = ExecuteQueryForObject("SelectDailyStatementNo", null);
            if (objValue != null)
            {
                dailySerialNum = Convert.ToString(objValue);
            }
            return dailySerialNum;
        }

        public void CreateDailyStatement(DailyStatement dailyStatement)
        {
            ExecuteInsert("InsertDailyStatement", dailyStatement);
        }

        /// <summary>
        /// 日结操作
        /// </summary>
        /// <param name="dailyStatement">日结号</param>
        /// <param name="unCheckDeviceNo">未结账的设备号</param>
        /// <returns></returns>
        public int UpdateDailyStatement(DailyStatement dailyStatement, out string unCheckDeviceNo)
        {
            unCheckDeviceNo = string.Empty;

            Hashtable htParam = new Hashtable();
            htParam["DailyStatementNo"] = dailyStatement.DailyStatementNo;
            htParam["DeviceNo"] = dailyStatement.DeviceNo;
            htParam["BelongToDate"] = dailyStatement.BelongToDate;
            htParam["Weather"] = dailyStatement.Weather;
            htParam["EmployeeID"] = dailyStatement.EmployeeID;
            htParam["ReturnValue"] = 0;

            object objValue = ExecuteQueryForObject("UpdateDailyStatement", htParam);
            if (objValue != null)
            {
                unCheckDeviceNo = Convert.ToString(objValue).Trim();
                if (unCheckDeviceNo.Length > 1)
                {
                    unCheckDeviceNo = unCheckDeviceNo.Substring(1);
                }
            }
            return (int)htParam["ReturnValue"];
        }

        public string GetDailyStatementTimeInterval(string dailyStatementNo)
        {
            string timeInterval = string.Empty;
            object objValue = ExecuteQueryForObject("SelectDailyStatementTimeInterval", dailyStatementNo);
            if (objValue != null)
            {
                timeInterval = Convert.ToString(objValue);
            }
            return timeInterval;
        }

        public DateTime GetLastDailyStatementDate()
        {
            object objValue = ExecuteQueryForObject("SelectLastDailyStatementDate", null);
            return Convert.ToDateTime(objValue);
        }

        /// <summary>
        /// 根据所属日期获取营业时间段
        /// </summary>
        /// <param name="belongToDate"></param>
        /// <returns></returns>
        public IList<DailyBalanceTime> GetDailyBalanceTime(DateTime belongToDate)
        {
            return ExecuteQueryForList<DailyBalanceTime>("SelectDailyBalanceTime", belongToDate);
        }
        #endregion
    }
}
