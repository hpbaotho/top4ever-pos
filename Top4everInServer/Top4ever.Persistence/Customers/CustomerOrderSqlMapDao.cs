using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain.Customers;
using Top4ever.Interface.Customers;

namespace Top4ever.Persistence.Customers
{
    /// <summary>
    /// Summary description for CustomerOrderSqlMapDao
    /// </summary>
    public class CustomerOrderSqlMapDao : BaseSqlMapDao, ICustomerOrderDao
    {
        #region ICustomerOrderDao Members

        public void CreateOrUpdateCustomerOrder(CustomerOrder customerOrder)
        {
            Hashtable htParam = new Hashtable();
            htParam["OrderID"] = customerOrder.OrderID;
            htParam["Telephone"] = customerOrder.Telephone;
            htParam["CustomerName"] = customerOrder.CustomerName;
            htParam["Address"] = customerOrder.Address;
            htParam["Remark"] = customerOrder.Remark ?? string.Empty;
            htParam["DeliveryEmployeeNo"] = customerOrder.DeliveryEmployeeNo ?? string.Empty;
            ExecuteInsert("InsertCustomerOrder", htParam);
        }

        public CustomerOrder GetCustomerOrder(Guid orderID)
        {
            return (ExecuteQueryForObject("GetCustomerOrder", orderID) as CustomerOrder);
        }

        /// <summary>
        /// 创建或者更新电话记录
        /// </summary>
        /// <param name="callRecord">电话记录</param>
        /// <returns></returns>
        public bool CreateOrUpdateCallRecord(CallRecord callRecord)
        {
            int result = 0;
            result = ExecuteUpdate("CreateOrUpdateCallRecord", callRecord);
            return result > 0;
        }

        /// <summary>
        /// 获取最近一个月所有通话记录
        /// </summary>
        /// <returns></returns>
        public IList<CallRecord> GetCallRecordList()
        {
            return ExecuteQueryForList<CallRecord>("GetCallRecordList", null);
        }

        /// <summary>
        /// 获取最近一个月特定状态通话记录
        /// </summary>
        /// <returns></returns>
        public IList<CallRecord> GetCallRecordByStatus(int status)
        {
            return ExecuteQueryForList<CallRecord>("GetCallRecordByStatus", status);
        }

        /// <summary>
        /// 获取热销产品列表
        /// </summary>
        /// <param name="telephone">电话号码</param>
        /// <returns></returns>
        public IList<TopSellGoods> GetTopSellGoods(string telephone)
        {
            return ExecuteQueryForList<TopSellGoods>("GetTopSellGoods", telephone);
        }

        #endregion
    }
}
