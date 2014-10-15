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
        IList<Order> GetOrderList(string deskName);

        Order GetOrder(Guid orderId);

        IList<Order> GetOrderListBySearch(string strWhere, string orderBy, int pageIndex, int pageSize);

        string CreateOrder(Order order);

        bool UpdateOrder(Order order);

        bool UpdateOrderPrice(Order order);

        bool UpdatePrePayOrder(Order order);

        bool UpdatePayingOrder(Order order);

        bool UpdatePaidOrderPrice(Order order);

        Int32 GetCurrentSubOrderNo(string deskName);

        bool DeleteWholeOrder(DeletedOrder deletedOrder);

        bool UpdateSplitOrderPrice(Order order);

        bool UpdateOrderDeskName(Order order);

        bool MergeSalesOrder(DeskChange deskChange);

        bool IsExistOrderInTimeInterval(DateTime beginTime, DateTime endTime);

        bool UpdateOrderStatus(Guid orderId, int status);

        bool DeliveryTakeoutOrder(Guid orderId, string employeeNo);

        IList<DeliveryOrder> GetDeliveryOrderList(string dailyStatementNo);

        IList<HourOrderSales> GetHourSalesReport(string dailyStatementNo);

        IList<HourOrderSales> GetHourSalesReportByTime(DateTime beginTime, DateTime endTime);
    }
}
