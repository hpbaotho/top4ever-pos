using System;
using System.Collections.Generic;

using Top4ever.Domain.Transfer;
using Top4ever.Domain.OrderRelated;

namespace Top4ever.Interface.OrderRelated
{
    /// <summary>
    /// Summary description for IOrderDetailsDao.
    /// </summary>
    public interface IOrderDetailsDao
    {
        void CreateOrderDetails(OrderDetails orderDetails);

        bool UpdateOrderDetails(OrderDetails orderDetails);

        bool UpdateOrderDetailsDiscount(OrderDetails orderDetails);

        IList<OrderDetails> GetOrderDetailsList(Guid orderID);

        IList<OrderDetails> GetDeletedOrderDetailsList(Guid orderID);

        OrderDetails GetOrderDetails(Guid orderDetailsID);

        Int32 GetSequenceNum(Guid orderID);

        bool DeleteWholeOrderDetails(DeletedOrder deletedOrder);

        bool DeleteSingleOrderDetails(DeletedOrderDetails deletedOrderDetails);

        bool LadeOrderDetails(OrderDetails orderDetails);

        bool SubtractSalesSplitOrder(OrderDetails orderDetails);
    }
}
