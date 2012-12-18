using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;
using Top4ever.Interface.OrderRelated;

namespace Top4ever.Service
{
    public class PayingOrderService
    {
        #region Private Fields

        private static PayingOrderService _instance = new PayingOrderService();
        private IDaoManager _daoManager = null;
        private IOrderDao _orderDao = null;
        private IOrderDetailsDao _orderDetailsDao = null;
        private IOrderDiscountDao _orderDiscountDao = null;
        private IOrderPayoffDao _orderPayoffDao = null;
        private IDailyStatementDao _dailyStatementDao = null;

        #endregion

        #region Constructor

        private PayingOrderService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _orderDao = _daoManager.GetDao(typeof(IOrderDao)) as IOrderDao;
            _orderDetailsDao = _daoManager.GetDao(typeof(IOrderDetailsDao)) as IOrderDetailsDao;
            _orderDiscountDao = _daoManager.GetDao(typeof(IOrderDiscountDao)) as IOrderDiscountDao;
            _orderPayoffDao = _daoManager.GetDao(typeof(IOrderPayoffDao)) as IOrderPayoffDao;
            _dailyStatementDao = _daoManager.GetDao(typeof(IDailyStatementDao)) as IDailyStatementDao;
        }

        #endregion

        #region Public methods

        public static PayingOrderService GetInstance()
        {
            return _instance;
        }

        public bool PayForOrder(PayingOrder payingOrder)
        { 
            bool returnValue = false;
            _daoManager.BeginTransaction();
            try
            {
                if (payingOrder != null)
                {
                    //日结
                    string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
                    //更新Order
                    if (_orderDao.UpdatePayingOrder(payingOrder.order))
                    { 
                        //更新OrderDetails
                        foreach (OrderDetails item in payingOrder.orderDetailsList)
                        {
                            _orderDetailsDao.UpdateOrderDetailsDiscount(item);
                        }
                        //插入OrderDiscount
                        if (payingOrder.orderDiscountList != null && payingOrder.orderDiscountList.Count > 0)
                        {
                            foreach (OrderDiscount item in payingOrder.orderDiscountList)
                            {
                                item.DailyStatementNo = dailyStatementNo;
                                _orderDiscountDao.CreateOrderDiscount(item);
                            }
                        }
                        //插入OrderPayoff
                        foreach (OrderPayoff item in payingOrder.orderPayoffList)
                        {
                            item.DailyStatementNo = dailyStatementNo;
                            _orderPayoffDao.CreateOrderPayoff(item);
                        }
                        returnValue = true;
                    }
                }
                _daoManager.CommitTransaction();
            }
            catch(Exception ex)
            {
                _daoManager.RollBackTransaction();
                returnValue = false;
            }
            return returnValue;
        }

        #endregion
    }
}
