using System;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Domain.Transfer;

namespace Top4ever.Interface
{
    /// <summary>
    /// Summary description for IDailyStatementDao.
    /// </summary>
    public interface IDailyStatementDao
    {
        string GetCurrentDailyStatementNo();

        void CreateDailyStatement(DailyStatement dailyStatement);

        /// <summary>
        /// 日结操作
        /// </summary>
        /// <param name="dailyStatement">日结号</param>
        /// <param name="unCheckDeviceNo">未结账的设备号</param>
        /// <returns></returns>
        int UpdateDailyStatement(DailyStatement dailyStatement, out string unCheckDeviceNo);

        string GetDailyStatementTimeInterval(string dailyStatementNo);

        DateTime GetLastDailyStatementDate();

        /// <summary>
        /// 根据所属日期获取营业时间段
        /// </summary>
        /// <param name="belongToDate"></param>
        /// <returns></returns>
        IList<DailyBalanceTime> GetDailyBalanceTime(DateTime belongToDate);

        /// <summary>
        ///  获取一段时间内各个账务日的营业额
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        IList<DailyStatementInDay> GetDailyStatementInDays(DateTime beginDate, DateTime endDate);
    }
}
