using System;
using System.Collections.Generic;

using Top4ever.Domain;

namespace Top4ever.Interface
{
    /// <summary>
    /// Summary description for IDailyTurnoverDao.
    /// </summary>
    public interface IDailyTurnoverDao
    {
        DailyTurnover GetDailyTurnover(string dailyStatementNo);

        void CreateDailyTurnover(DailyTurnover dailyTurnover);
    }
}
