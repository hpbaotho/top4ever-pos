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
        void CreateOrderPayoff(OrderPayoff orderPayoff);

        bool DeleteOrderPayoff(Guid orderID);

        IList<OrderPayoff> GetOrderPayoffList(Guid orderID);

        IList<OrderPayoff> GetDeletedOrderPayoffList(Guid orderID);
    }
}
