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
        /// <summary>
        /// 获取营业额统计
        /// </summary>
        /// <param name="dailyStatementNo">日结号</param>
        /// <param name="deviceNo">设备号</param>
        /// <returns></returns>
        BusinessReport GetTurnoverByHandover(string dailyStatementNo, string deviceNo);

        /// <summary>
        /// 通过交班Id获取营业额统计
        /// </summary>
        /// <param name="handoverRecordID">交班Id</param>
        /// <returns></returns>
        BusinessReport GetTurnoverByHandoverRecordID(Guid handoverRecordID);

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
