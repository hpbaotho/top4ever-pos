using System;
using System.Collections.Generic;

using Top4ever.Domain.Customers;

namespace Top4ever.Interface.Customers
{
    /// <summary>
    /// Summary description for ICustomerOrderDao.
    /// </summary>
    public interface ICustomerOrderDao
    {
        void CreateOrUpdateCustomerOrder(CustomerOrder customerOrder);

        CustomerOrder GetCustomerOrder(Guid orderID);

        /// <summary>
        /// 创建或者更新电话记录
        /// </summary>
        /// <param name="callRecord">电话记录</param>
        /// <returns></returns>
        bool CreateOrUpdateCallRecord(CallRecord callRecord);

        /// <summary>
        /// 获取最近一个月所有通话记录
        /// </summary>
        /// <returns></returns>
        IList<CallRecord> GetCallRecordList();

        /// <summary>
        /// 获取最近一个月特定状态通话记录
        /// </summary>
        /// <returns></returns>
        IList<CallRecord> GetCallRecordByStatus(int status);

        /// <summary>
        /// 获取热销产品列表
        /// </summary>
        /// <param name="telephone">电话号码</param>
        /// <returns></returns>
        IList<TopSellGoods> GetTopSellGoods(string telephone);
    }
}
