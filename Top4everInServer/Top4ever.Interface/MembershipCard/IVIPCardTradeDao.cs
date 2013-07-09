using System;
using System.Collections.Generic;

using Top4ever.Domain.MembershipCard;

namespace Top4ever.Interface.MembershipCard
{
    /// <summary>
    /// Summary description for IVIPCardTradeDao.
    /// </summary>
    public interface IVIPCardTradeDao
    {
        IList<VIPCardTrade> GetVIPCardTradeList(string cardNo, DateTime beginDate, DateTime endDate);

        IList<VIPCardTrade> GetAllStoredAmount(string dailyStatementNo);

        VIPCardStoredVaule GetVIPCardStoredVaule(string cardNo, decimal storedVauleAmount);

        Int32 AddVIPCardStoredValue(string cardNo, decimal storeMoney, decimal giftAmount, int giftIntegral, string employeeNo, string deviceNo, string dailyStatementNo, Guid payoffID, string payoffName, out string tradePayNo);

        Int32 AddVIPCardPayment(string cardNo, decimal payAmount, int payIntegral, string orderNo, string employeeNo, string deviceNo, string dailyStatementNo, out string tradePayNo);

        Int32 RefundVIPCardPayment(string cardNo, string tradePayNo);
    }
}
