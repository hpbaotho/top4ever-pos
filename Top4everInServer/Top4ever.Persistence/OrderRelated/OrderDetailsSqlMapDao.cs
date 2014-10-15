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
            int result = ExecuteUpdate("UpdateOrderDetails", orderDetails);
            return result > 0;
        }

        public bool UpdateOrderDetailsDiscount(OrderDetails orderDetails)
        {
            int result = ExecuteUpdate("UpdateOrderDetailsDiscount", orderDetails);
            return result > 0;
        }

        public IList<OrderDetails> GetOrderDetailsList(Guid orderId)
        {
            return ExecuteQueryForList<OrderDetails>("GetOrderDetailsByOrderID", orderId);
        }

        public IList<OrderDetails> GetDeletedOrderDetailsList(Guid orderId)
        {
            return ExecuteQueryForList<OrderDetails>("GetDeletedOrderDetails", orderId);
        }

        public OrderDetails GetOrderDetails(Guid orderDetailsId)
        {
            return ExecuteQueryForObject("GetOrderDetailsByOrderDetailsID", orderDetailsId) as OrderDetails;
        }

        public IList<OrderDetails> GetOrderDetailsList(IList<Guid> orderDetailsIds)
        {
            return ExecuteQueryForList<OrderDetails>("GetOrderDetailsList", orderDetailsIds);
        }

        public Int32 GetSequenceNum(Guid orderId)
        {
            Int32 result = 1;
            object objValue = ExecuteQueryForObject("SelectMaxSequenceNum", orderId);
            if (objValue != null)
            {
                result = (Int32)objValue + 1;
            }
            return result;
        }

        public bool DeleteWholeOrderDetails(DeletedOrder deletedOrder)
        {
            int result = ExecuteUpdate("UpdateDeletedOrderDetails", deletedOrder);
            return result > 0;
        }

        public bool DeleteSingleOrderDetails(DeletedOrderDetails deletedOrderDetails)
        {
            int result = ExecuteUpdate("DeleteSingleOrderDetails", deletedOrderDetails);
            return result > 0;
        }

        public bool LadeOrderDetails(OrderDetails orderDetails)
        {
            int result = ExecuteUpdate("UpdateLadeOrderDetails", orderDetails);
            return result > 0;
        }

        public bool SubtractSalesSplitOrder(OrderDetails orderDetails)
        {
            int result = ExecuteUpdate("SubtractSalesSplitOrder", orderDetails);
            return result > 0;
        }

        public IList<DeletedItem> GetDeletedOrderItemList(DateTime beginDate, DateTime endDate, int dateType)
        {
            Hashtable htParam = new Hashtable();
            htParam["BeginDate"] = beginDate;
            htParam["EndDate"] = endDate;
            htParam["DateType"] = dateType;
            return ExecuteQueryForList<DeletedItem>("GetDeletedOrderItem", htParam);
        }

        public IList<DeletedItem> GetDeletedGoodsItemList(DateTime beginDate, DateTime endDate, int dateType)
        {
            Hashtable htParam = new Hashtable();
            htParam["BeginDate"] = beginDate;
            htParam["EndDate"] = endDate;
            htParam["DateType"] = dateType;
            return ExecuteQueryForList<DeletedItem>("GetDeletedGoodsItem", htParam);
        }

        public decimal GetLastCustomPrice(string dailyStatementNo, Guid goodsId)
        {
            decimal result = 0M;
            OrderDetails orderDetails = new OrderDetails();
            orderDetails.DailyStatementNo = dailyStatementNo;
            orderDetails.GoodsID = goodsId;
            object objValue = ExecuteQueryForObject("GetLastCustomPrice", orderDetails);
            if (objValue != null)
            {
                result = (decimal)objValue;
            }
            return result;
        }
        #endregion
    }
}
