using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Interface;

namespace Top4ever.Persistence
{
    /// <summary>
    /// Summary description for DailyTurnoverSqlMapDao
    /// </summary>
    public class DailyTurnoverSqlMapDao : BaseSqlMapDao, IDailyTurnoverDao
    {
        #region IDailyTurnoverDao Members

        public DailyTurnover GetDailyTurnover(string dailyStatementNo)
        {
            return (ExecuteQueryForObject("GetDailyTurnoverByStatementNo", dailyStatementNo) as DailyTurnover);
        }

        public void CreateDailyTurnover(DailyTurnover dailyTurnover)
        {
            ExecuteInsert("InsertDailyTurnover", dailyTurnover);
        }

        #endregion
    }
}
