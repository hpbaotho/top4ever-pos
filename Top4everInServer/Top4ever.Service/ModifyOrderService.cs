using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;
using Top4ever.Interface.OrderRelated;

namespace Top4ever.Service
{
    public class ModifyOrderService
    {
        #region Private Fields

        private static ModifyOrderService _instance = new ModifyOrderService();
        private IDaoManager _daoManager = null;
        private IOrderDao _orderDao = null;
        private IOrderDetailsDao _orderDetailsDao = null;
        private IOrderDiscountDao _orderDiscountDao = null;
        private IOrderPayoffDao _orderPayoffDao = null;
        private IDailyStatementDao _dailyStatementDao = null;

        #endregion

        #region Constructor

        private ModifyOrderService()
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

        public static ModifyOrderService GetInstance()
        {
            return _instance;
        }

        public bool ModifyForOrder(ModifiedPaidOrder modifiedOrder)
        {
            bool returnValue = false;
            _daoManager.BeginTransaction();
            try
            {
                if (modifiedOrder != null)
                {
                    //日结
                    string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
                    //更新Order
                    if (_orderDao.UpdatePaidOrderPrice(modifiedOrder.order))
                    {
                        //更新OrderDetails
                        foreach (OrderDetails item in modifiedOrder.orderDetailsList)
                        {
                            _orderDetailsDao.UpdateOrderDetailsDiscount(item);
                        }
                        if (modifiedOrder.orderDiscountList != null && modifiedOrder.orderDiscountList.Count > 0)
                        {
                            foreach (OrderDiscount item in modifiedOrder.orderDiscountList)
                            {
                                //删除OrderDiscount
                                _orderDiscountDao.DeleteOrderSingleDiscount(item.OrderDetailsID);
                                //插入OrderDiscount
                                item.DailyStatementNo = dailyStatementNo;
                                _orderDiscountDao.CreateOrderDiscount(item);
                            }
                        }
                        _orderPayoffDao.DeleteOrderPayoff(modifiedOrder.order.OrderID);
                        //插入OrderPayoff
                        foreach (OrderPayoff item in modifiedOrder.orderPayoffList)
                        {
                            item.DailyStatementNo = dailyStatementNo;
                            _orderPayoffDao.CreateOrderPayoff(item);
                        }
                        returnValue = true;
                    }
                }
                _daoManager.CommitTransaction();
            }
            catch
            {
                _daoManager.RollBackTransaction();
                returnValue = false;
            }
            return returnValue;
        }

        #endregion
    }
}
