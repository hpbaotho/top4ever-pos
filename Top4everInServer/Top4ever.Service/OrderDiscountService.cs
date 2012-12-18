using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain.OrderRelated;
using Top4ever.Interface.OrderRelated;

namespace Top4ever.Service
{
    public class OrderDiscountService
    {
        #region Private Fields

        private static OrderDiscountService _instance = new OrderDiscountService();
        private IDaoManager _daoManager = null;
        private IOrderDiscountDao _orderDiscountDao = null;

        #endregion

        #region Constructor

        private OrderDiscountService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _orderDiscountDao = _daoManager.GetDao(typeof(IOrderDiscountDao)) as IOrderDiscountDao;
        }

        #endregion

        #region Public methods

        public static OrderDiscountService GetInstance()
        {
            return _instance;
        }

        public void CreateOrderDiscount(OrderDiscount orderDiscount)
        {
            _daoManager.OpenConnection();
            _orderDiscountDao.CreateOrderDiscount(orderDiscount);
            _daoManager.CloseConnection();
        }

        public bool UpdateOrderDiscount(OrderDiscount orderDiscount)
        {
            bool result = false;

            _daoManager.OpenConnection();
            result = _orderDiscountDao.UpdateOrderDiscount(orderDiscount);
            _daoManager.CloseConnection();

            return result;
        }
        #endregion
    }
}
