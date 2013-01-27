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

        public string CreateOrder(Order order)
        {
            object result = ExecuteInsert("InsertOrder", order);
            if (result != null)
            {
                return result.ToString();
            }
            else
            {
                return null;
            }
        }

        public bool UpdateOrder(Order order)
        {
            int result = 0;
            result = ExecuteUpdate("UpdateOrder", order);
            return result > 0;
        }

        public bool UpdatePayingOrder(Order order)
        {
            int result = 0;
            result = ExecuteUpdate("UpdatePayingOrder", order);
            return result > 0;
        }

        public IList<Order> GetOrderList(string deskName)
        {
            return ExecuteQueryForList<Order>("GetOrderByDeskName", deskName);
        }

        public IList<Order> GetOrderListBySearch(string strWhere, int pageIndex, int pageSize)
        {
            Hashtable htParam = new Hashtable();
            htParam["strWhere"] = strWhere;
            htParam["PageIndex"] = pageIndex;
            htParam["PageSize"] = pageSize;
            return ExecuteQueryForList<Order>("GetOrderListBySearch", htParam);
        }

        public Order GetOrder(Guid orderID)
        {
            return (ExecuteQueryForObject("GetOrderByID", orderID) as Order);
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

        public bool DeleteWholeOrder(DeletedOrder deletedOrder)
        {
            int result = 0;
            result = ExecuteUpdate("UpdateDeletedOrder", deletedOrder);
            return result > 0;
        }

        public bool UpdateOrderPrice(Order order)
        {
            int result = 0;
            result = ExecuteUpdate("UpdateOrderPrice", order);
            return result > 0;
        }

        public bool UpdateSplitOrderPrice(Order order)
        {
            int result = 0;
            result = ExecuteUpdate("UpdateSplitOrderPrice", order);
            return result > 0;
        }

        public bool UpdateOrderDeskName(Order order)
        {
            int result = 0;
            result = ExecuteUpdate("UpdateOrderDeskName", order);
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

        #endregion
    }
}
