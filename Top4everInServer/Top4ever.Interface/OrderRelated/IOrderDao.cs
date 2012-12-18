using System;
using System.Collections.Generic;

using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;

namespace Top4ever.Interface.OrderRelated
{
    /// <summary>
    /// Summary description for IOrderDao.
    /// </summary>
    public interface IOrderDao
    {
        string CreateOrder(Order order);

        bool UpdateOrder(Order order);

        bool UpdatePayingOrder(Order order);

        IList<Order> GetOrderList(string deskName);

        Order GetOrder(Guid orderID);

        Int32 GetCurrentSubOrderNo(string deskName);

        bool DeleteWholeOrder(DeletedOrder deletedOrder);

        bool UpdateOrderPrice(Order order);

        bool UpdateSplitOrderPrice(Order order);

        bool UpdateOrderDeskName(Order order);

        bool MergeSalesOrder(DeskChange deskChange);
    }
}
