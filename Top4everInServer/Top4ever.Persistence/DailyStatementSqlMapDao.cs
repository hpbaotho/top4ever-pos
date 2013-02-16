using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain;
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

        public int UpdateDailyStatement(DailyStatement dailyStatement)
        {
            int result = 0;
            object objValue = ExecuteQueryForObject("UpdateDailyStatement", dailyStatement);
            if (objValue != null)
            {
                result = Convert.ToInt32(objValue);
            }
            return result;
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
        #endregion
    }
}
