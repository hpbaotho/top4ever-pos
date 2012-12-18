using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Interface.OrderRelated;

namespace Top4ever.Service
{
    public class OrderService
    {
        #region Private Fields

        private static OrderService _instance = new OrderService();
        private IDaoManager _daoManager = null;
        private IOrderDao _orderDao = null;

        #endregion

        #region Constructor

        private OrderService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _orderDao = _daoManager.GetDao(typeof(IOrderDao)) as IOrderDao;
        }

        #endregion

        #region Public methods

        public static OrderService GetInstance()
        {
            return _instance;
        }

        public void CreateOrder(Order order)
        {
            _daoManager.OpenConnection();
            _orderDao.CreateOrder(order);
            _daoManager.CloseConnection();
        }

        public bool UpdateOrder(Order order)
        {
            bool result = false;

            _daoManager.OpenConnection();
            result = _orderDao.UpdateOrder(order);
            _daoManager.CloseConnection();

            return result;
        }

        public IList<Order> GetOrderList(string deskName)
        {
            IList<Order> orderList = null;

            _daoManager.OpenConnection();
            orderList = _orderDao.GetOrderList(deskName);
            _daoManager.CloseConnection();

            return orderList;
        }

        public Order GetOrder(Guid orderID)
        {
            Order order = null;

            _daoManager.OpenConnection();
            order = _orderDao.GetOrder(orderID);
            _daoManager.CloseConnection();

            return order;
        }

        public bool OrderDeskOperate(DeskChange deskChange)
        {
            bool result = false;
            // 转台
            if (deskChange.OrderID1st != Guid.Empty && deskChange.OrderID2nd == Guid.Empty)
            {
                //分单号
                Int32 curSubOrderNo = _orderDao.GetCurrentSubOrderNo(deskChange.DeskName);
                if (curSubOrderNo > 0)
                {
                    curSubOrderNo++;
                }
                else
                {
                    curSubOrderNo = 1;
                }
                Order order = new Order();
                order.OrderID = deskChange.OrderID1st;
                order.DeskName = deskChange.DeskName;
                order.SubOrderNo = curSubOrderNo;
                result = _orderDao.UpdateOrderDeskName(order);
            }
            // 合并
            if (deskChange.OrderID1st != Guid.Empty && deskChange.OrderID2nd != Guid.Empty)
            {
                result = _orderDao.MergeSalesOrder(deskChange);
            }
            return result;
        }

        public Int32 GetCurrentSubOrderNo(string deskName)
        {
            _daoManager.OpenConnection();
            Int32 result = _orderDao.GetCurrentSubOrderNo(deskName);
            _daoManager.CloseConnection();

            return result;
        }
        #endregion
    }
}
