using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain.Transfer;
using Top4ever.Domain.OrderRelated;
using Top4ever.Interface.OrderRelated;

namespace Top4ever.Persistence.OrderRelated
{
    /// <summary>
    /// Summary description for OrderDetailsSqlMapDao
    /// </summary>
    public class OrderDetailsSqlMapDao : BaseSqlMapDao, IOrderDetailsDao
    {
        #region IOrderDetailsDao Members

        public void CreateOrderDetails(OrderDetails orderDetails)
        {
            ExecuteInsert("InsertOrderDetails", orderDetails);
        }

        public bool UpdateOrderDetails(OrderDetails orderDetails)
        {
            int result = 0;
            result = ExecuteUpdate("UpdateOrderDetails", orderDetails);
            return result > 0;
        }

        public bool UpdateOrderDetailsDiscount(OrderDetails orderDetails)
        {
            int result = 0;
            result = ExecuteUpdate("UpdateOrderDetailsDiscount", orderDetails);
            return result > 0;
        }

        public IList<OrderDetails> GetOrderDetailsList(Guid orderID)
        {
            return ExecuteQueryForList<OrderDetails>("GetOrderDetailsByOrderID", orderID);
        }

        public OrderDetails GetOrderDetails(Guid orderDetailsID)
        {
            return ExecuteQueryForObject("GetOrderDetailsByOrderDetailsID", orderDetailsID) as OrderDetails;
        }

        public Int32 GetSequenceNum(Guid orderID)
        {
            Int32 result = 1;
            object objValue = ExecuteQueryForObject("SelectMaxSequenceNum", orderID);
            if (objValue != null)
            {
                result = (Int32)objValue + 1;
            }
            return result;
        }

        public bool DeleteWholeOrderDetails(DeletedOrder deletedOrder)
        {
            int result = 0;
            result = ExecuteUpdate("UpdateDeletedOrderDetails", deletedOrder);
            return result > 0;
        }

        public bool DeleteSingleOrderDetails(DeletedOrderDetails deletedOrderDetails)
        {
            int result = 0;
            result = ExecuteUpdate("DeleteSingleOrderDetails", deletedOrderDetails);
            return result > 0;
        }

        public bool LadeOrderDetails(OrderDetails orderDetails)
        {
            int result = 0;
            result = ExecuteUpdate("UpdateLadeOrderDetails", orderDetails);
            return result > 0;
        }

        public bool SubtractSalesSplitOrder(OrderDetails orderDetails)
        {
            int result = 0;
            result = ExecuteUpdate("SubtractSalesSplitOrder", orderDetails);
            return result > 0;
        }
        #endregion
    }
}
