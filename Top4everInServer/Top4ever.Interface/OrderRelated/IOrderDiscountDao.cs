using System;
using System.Collections.Generic;

using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;

namespace Top4ever.Interface.OrderRelated
{
    /// <summary>
    /// Summary description for IOrderDiscountDao.
    /// </summary>
    public interface IOrderDiscountDao
    {
        IList<OrderDiscountSum> GetOrderDiscountSumList(string dailyStatementNo);

        void CreateOrderDiscount(OrderDiscount orderDiscount);

        bool UpdateOrderDiscount(OrderDiscount orderDiscount);

        bool DeleteOrderDiscount(Guid orderID);

        bool DeleteOrderSingleDiscount(Guid orderDetailsID);

        IList<OrderDiscount> GetOrderDiscountList(Guid orderID);

        IList<OrderDiscount> GetDeletedOrderDiscountList(Guid orderID);
    }
}
