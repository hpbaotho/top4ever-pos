using System;
using System.Collections.Generic;

using Top4ever.Domain;

namespace Top4ever.Interface
{
    /// <summary>
    /// Summary description for IDailyStatementDao.
    /// </summary>
    public interface IDailyStatementDao
    {
        string GetCurrentDailyStatementNo();

        void CreateDailyStatement(DailyStatement dailyStatement);

        int UpdateDailyStatement(DailyStatement dailyStatement);
    }
}
