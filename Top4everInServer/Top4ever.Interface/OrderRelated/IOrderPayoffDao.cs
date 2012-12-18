using System;
using System.Collections.Generic;

using Top4ever.Domain.Transfer;
using Top4ever.Domain.OrderRelated;

namespace Top4ever.Interface.OrderRelated
{
    /// <summary>
    /// Summary description for IOrderPayoffDao.
    /// </summary>
    public interface IOrderPayoffDao
    {
        IList<OrderPayoffSum> GetOrderPayoffSumList(string dailyStatementNo);

        void CreateOrderPayoff(OrderPayoff orderPayoff);
    }
}
