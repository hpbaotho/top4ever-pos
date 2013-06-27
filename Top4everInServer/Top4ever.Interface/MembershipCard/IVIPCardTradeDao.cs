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
        IList<VIPCardTrade> GetVIPCardTradeList(string cardNo, string beginDate, string endDate);

        VIPCardStoredVaule GetVIPCardStoredVaule(string cardNo, decimal storedVauleAmount);

        Int32 AddVIPCardStoredValue(string cardNo, decimal storeMoney, decimal giftAmount, int giftIntegral, string employeeNo, string deviceNo, string dailyStatementNo, Guid payoffID, string payoffName, out string tradePayNo);
    }
}
