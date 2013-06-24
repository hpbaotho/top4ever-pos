using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain.MembershipCard;
using Top4ever.Interface.MembershipCard;

namespace Top4ever.Persistence.MembershipCard
{
    /// <summary>
    /// Summary description for VIPCardTradeSqlMapDao
    /// </summary>
    public class VIPCardTradeSqlMapDao : BaseSqlMapDao, IVIPCardTradeDao
    {
        public IList<VIPCardTrade> GetVIPCardTradeList(string cardNo, string beginDate, string endDate)
        {
            Hashtable htParam = new Hashtable();
            htParam["CardNo"] = cardNo;
            htParam["BeginDate"] = beginDate;
            htParam["EndDate"] = endDate;
            return ExecuteQueryForList<VIPCardTrade>("GetVIPCardTradeList", htParam);
        }
    }
}
