using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain.OrderRelated;
using Top4ever.Interface.OrderRelated;

namespace Top4ever.Service
{
    public class OrderPayoffService
    {
        #region Private Fields

        private static OrderPayoffService _instance = new OrderPayoffService();
        private IDaoManager _daoManager = null;
        private IOrderPayoffDao _orderPayoffDao = null;

        #endregion
        
        #region Constructor

        private OrderPayoffService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _orderPayoffDao = _daoManager.GetDao(typeof(IOrderPayoffDao)) as IOrderPayoffDao;
        }

        #endregion

        #region Public methods

        public static OrderPayoffService GetInstance()
        {
            return _instance;
        }

        public void CreateOrderPayoff(OrderPayoff orderPayoff)
        {
            _daoManager.OpenConnection();
            _orderPayoffDao.CreateOrderPayoff(orderPayoff);
            _daoManager.CloseConnection();
        }

        public bool DeleteOrderPayoff(Guid orderID)
        {
            bool result = false;

            _daoManager.OpenConnection();
            result = _orderPayoffDao.DeleteOrderPayoff(orderID);
            _daoManager.CloseConnection();

            return result;
        }

        public IList<OrderPayoff> GetOrderPayoffList(Guid orderID)
        {
            IList<OrderPayoff> orderPayoffList = null;

            _daoManager.OpenConnection();
            orderPayoffList = _orderPayoffDao.GetOrderPayoffList(orderID);
            _daoManager.CloseConnection();

            return orderPayoffList;
        }

        #endregion
    }
}
