using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;
using Newtonsoft.Json;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;
using Top4ever.Interface.OrderRelated;
using Top4ever.Utils;

namespace Top4ever.Service
{
    public class PayingOrderService
    {
        #region Private Fields

        private static readonly PayingOrderService _instance = new PayingOrderService();
        private readonly IDaoManager _daoManager;
        private readonly IOrderDao _orderDao;
        private readonly IOrderDetailsDao _orderDetailsDao;
        private readonly IOrderDiscountDao _orderDiscountDao;
        private readonly IOrderPayoffDao _orderPayoffDao;
        private readonly IDailyStatementDao _dailyStatementDao;

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

        public bool CreatePrePayOrder(PayingOrder payingOrder)
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
                    if (_orderDao.UpdatePrePayOrder(payingOrder.order))
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
                        returnValue = true;
                    }
                }
                _daoManager.CommitTransaction();
            }
            catch(Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[CreatePrePayOrder]参数：payingOrder_{0}", JsonConvert.SerializeObject(payingOrder)), exception);
                _daoManager.RollBackTransaction();
                returnValue = false;
            }
            return returnValue;
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
                        if (payingOrder.orderDetailsList != null && payingOrder.orderDetailsList.Count > 0)
                        {
                            foreach (OrderDetails item in payingOrder.orderDetailsList)
                            {
                                _orderDetailsDao.UpdateOrderDetailsDiscount(item);
                            }
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
            catch(Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[PayForOrder]参数：payingOrder_{0}", JsonConvert.SerializeObject(payingOrder)), exception);
                _daoManager.RollBackTransaction();
                returnValue = false;
            }
            return returnValue;
        }

        #endregion
    }
}
