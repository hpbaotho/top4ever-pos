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
    }
}
