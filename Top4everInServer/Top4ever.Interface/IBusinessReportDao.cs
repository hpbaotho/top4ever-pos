using System;
using System.Collections.Generic;

using Top4ever.Domain.Transfer;

namespace Top4ever.Interface
{
    /// <summary>
    /// Summary description for IBusinessReportDao.
    /// </summary>
    public interface IBusinessReportDao
    {
        //交班
        BusinessReport GetTurnoverByHandover(string dailyStatementNo, string deviceNo);

        IList<OrderDiscountSum> GetOrderDiscountSumByHandover(string dailyStatementNo, int workSequence);

        IList<OrderPayoffSum> GetOrderPayoffSumByHandover(string dailyStatementNo, int workSequence);

        IList<ItemsPrice> GetItemsPriceByHandover(string dailyStatementNo, int workSequence);

        //日结
        BusinessReport GetTurnoverByDailyStatement(string dailyStatementNo);

        IList<OrderDiscountSum> GetOrderDiscountSumByDailyStatement(string dailyStatementNo);

        IList<OrderPayoffSum> GetOrderPayoffSumByDailyStatement(string dailyStatementNo);

        IList<ItemsPrice> GetItemsPriceByDailyStatement(string dailyStatementNo);

        IList<GroupPrice> GetItemsPriceByGroup(DateTime beginDate, DateTime endDate);
    }
}
