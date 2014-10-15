using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain.Transfer;
using Top4ever.Domain.OrderRelated;
using Top4ever.Interface.OrderRelated;

namespace Top4ever.Persistence.OrderRelated
{
    /// <summary>
    /// Summary description for OrderSqlMapDao
    /// </summary>
    public class OrderSqlMapDao : BaseSqlMapDao, IOrderDao
    {
        #region IOrderDao Members

        public IList<Order> GetOrderList(string deskName)
        {
            return ExecuteQueryForList<Order>("GetOrderByDeskName", deskName);
        }

        public Order GetOrder(Guid orderId)
        {
            return (ExecuteQueryForObject("GetOrderByID", orderId) as Order);
        }

        public IList<Order> GetOrderListBySearch(string strWhere, string orderBy, int pageIndex, int pageSize)
        {
            Hashtable htParam = new Hashtable();
            htParam["strWhere"] = strWhere;
            htParam["orderBy"] = orderBy;
            htParam["PageIndex"] = pageIndex;
            htParam["PageSize"] = pageSize;
            return ExecuteQueryForList<Order>("GetOrderListBySearch", htParam);
        }

        public string CreateOrder(Order order)
        {
            object result = ExecuteInsert("InsertOrder", order);
            if (result != null)
            {
                return result.ToString();
            }
            return string.Empty;
        }

        public bool UpdateOrder(Order order)
        {
            int result = ExecuteUpdate("UpdateOrder", order);
            return result > 0;
        }

        public bool UpdateOrderPrice(Order order)
        {
            int result = ExecuteUpdate("UpdateOrderPrice", order);
            return result > 0;
        }

        public bool UpdatePrePayOrder(Order order)
        {
            int result = ExecuteUpdate("UpdatePrePayOrder", order);
            return result > 0;
        }

        public bool UpdatePayingOrder(Order order)
        {
            int result = ExecuteUpdate("UpdatePayingOrder", order);
            return result > 0;
        }

        public bool UpdatePaidOrderPrice(Order order)
        {
            int result = ExecuteUpdate("UpdatePaidOrderPrice", order);
            return result > 0;
        }

        public bool DeleteWholeOrder(DeletedOrder deletedOrder)
        {
            int result = ExecuteUpdate("UpdateDeletedOrder", deletedOrder);
            return result > 0;
        }

        public Int32 GetCurrentSubOrderNo(string deskName)
        {
            Int32 result = -1;
            object objValue = ExecuteQueryForObject("SelectMaxSubOrderNo", deskName);
            if (objValue != null)
            {
                result = (Int32)objValue;
            }
            return result;
        }

        public bool UpdateSplitOrderPrice(Order order)
        {
            int result = ExecuteUpdate("UpdateSplitOrderPrice", order);
            return result > 0;
        }

        public bool UpdateOrderDeskName(Order order)
        {
            int result = ExecuteUpdate("UpdateOrderDeskName", order);
            return result > 0;
        }

        public bool MergeSalesOrder(DeskChange deskChange)
        {
            bool result = false;

            Hashtable htParam = new Hashtable();
            htParam["OrderID1st"] = deskChange.OrderID1st;
            htParam["OrderID2nd"] = deskChange.OrderID2nd;
            htParam["ReturnValue"] = 0;
            ExecuteQueryForObject("MergeSalesOrder", htParam);
            int i = (int)htParam["ReturnValue"];    //返回值
            if (i == 1)
            {
                result = true;
            }
            return result;
        }

        public bool IsExistOrderInTimeInterval(DateTime beginTime, DateTime endTime)
        {
            Hashtable htParam = new Hashtable();
            htParam["BeginTime"] = beginTime;
            htParam["EndTime"] = endTime;
            object objValue = ExecuteQueryForObject("IsExistOrderInTime", htParam);
            bool isExist = true;
            if (objValue != null)
            {
                isExist = Convert.ToInt32(objValue) > 0;
            }
            return isExist;
        }

        public bool UpdateOrderStatus(Guid orderId, int status)
        {
            Hashtable htParam = new Hashtable();
            htParam["OrderID"] = orderId;
            htParam["Status"] = status;
            int result = ExecuteUpdate("UpdateOrderStatus", htParam);
            return result > 0;
        }

        public bool DeliveryTakeoutOrder(Guid orderId, string employeeNo)
        {
            Hashtable htParam = new Hashtable();
            htParam["OrderID"] = orderId;
            htParam["EmployeeNo"] = employeeNo;
            int result = ExecuteUpdate("UpdateTakeoutOrderStatus", htParam);
            return result > 0;
        }

        public IList<DeliveryOrder> GetDeliveryOrderList(string dailyStatementNo)
        {
            return ExecuteQueryForList<DeliveryOrder>("SelectDeliveryOrder", dailyStatementNo);
        }

        public IList<HourOrderSales> GetHourSalesReport(string dailyStatementNo)
        {
            return ExecuteQueryForList<HourOrderSales>("GetHourSalesReport", dailyStatementNo);
        }

        public IList<HourOrderSales> GetHourSalesReportByTime(DateTime beginTime, DateTime endTime)
        {
            Hashtable htParam = new Hashtable();
            htParam["BeginTime"] = beginTime;
            htParam["EndTime"] = endTime;

            return ExecuteQueryForList<HourOrderSales>("GetHourSalesReportByTime", htParam);
        }

        #endregion
    }
}
