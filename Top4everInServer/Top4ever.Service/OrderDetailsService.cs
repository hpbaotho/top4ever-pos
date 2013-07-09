using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Interface.OrderRelated;

namespace Top4ever.Service
{
    public class OrderDetailsService
    {
        #region Private Fields

        private static OrderDetailsService _instance = new OrderDetailsService();
        private IDaoManager _daoManager = null;
        private IOrderDetailsDao _orderDetailsDao = null;

        #endregion

        #region Constructor

        private OrderDetailsService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _orderDetailsDao = _daoManager.GetDao(typeof(IOrderDetailsDao)) as IOrderDetailsDao;
        }

        #endregion

        #region Public methods

        public static OrderDetailsService GetInstance()
        {
            return _instance;
        }

        public void CreateOrderDetails(OrderDetails orderDetails)
        {
            _daoManager.OpenConnection();
            _orderDetailsDao.CreateOrderDetails(orderDetails);
            _daoManager.CloseConnection();
        }

        public bool UpdateOrderDetails(OrderDetails orderDetails)
        {
            bool result = false;

            _daoManager.OpenConnection();
            result = _orderDetailsDao.UpdateOrderDetails(orderDetails);
            _daoManager.CloseConnection();

            return result;
        }

        public bool UpdateOrderDetailsDiscount(OrderDetails orderDetails)
        {
            bool result = false;

            _daoManager.OpenConnection();
            result = _orderDetailsDao.UpdateOrderDetailsDiscount(orderDetails);
            _daoManager.CloseConnection();

            return result;
        }

        public IList<OrderDetails> GetOrderDetails(Guid orderID)
        {
            IList<OrderDetails> orderDetailsList = null;

            _daoManager.OpenConnection();
            orderDetailsList = _orderDetailsDao.GetOrderDetailsList(orderID);
            _daoManager.CloseConnection();

            return orderDetailsList;
        }

        public bool LadeOrderDetails(OrderDetails orderDetails)
        {
            bool result = false;

            _daoManager.OpenConnection();
            result = _orderDetailsDao.LadeOrderDetails(orderDetails);
            _daoManager.CloseConnection();

            return result;
        }

        public bool SubtractSalesSplitOrder(OrderDetails orderDetails)
        {
            bool result = false;

            _daoManager.OpenConnection();
            result = _orderDetailsDao.SubtractSalesSplitOrder(orderDetails);
            _daoManager.CloseConnection();

            return result;
        }

        public DeletedAllItems GetAllDeletedItems(DateTime beginDate, DateTime endDate, int dateType)
        {
            DeletedAllItems deletedItems = new DeletedAllItems();
            _daoManager.OpenConnection();
            deletedItems.DeletedOrderItemList = _orderDetailsDao.GetDeletedOrderItemList(beginDate, endDate, dateType);
            deletedItems.DeletedGoodsItemList = _orderDetailsDao.GetDeletedGoodsItemList(beginDate, endDate, dateType);
            _daoManager.CloseConnection();
            return deletedItems;
        }

        #endregion
    }
}
