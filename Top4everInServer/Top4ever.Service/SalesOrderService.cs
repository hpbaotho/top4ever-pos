using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;
using Top4ever.Interface.OrderRelated;

namespace Top4ever.Service
{
    public class SalesOrderService
    {

        #region Private Fields

        private static SalesOrderService _instance = new SalesOrderService();
        private IDaoManager _daoManager = null;
        private IOrderDao _orderDao = null;
        private IOrderDetailsDao _orderDetailsDao = null;
        private IOrderDiscountDao _orderDiscountDao = null;
        private IDailyStatementDao _dailyStatementDao = null;
        private ISystemDictionaryDao _sysDictionary = null;

        #endregion

        #region Constructor

        private SalesOrderService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _orderDao = _daoManager.GetDao(typeof(IOrderDao)) as IOrderDao;
            _orderDetailsDao = _daoManager.GetDao(typeof(IOrderDetailsDao)) as IOrderDetailsDao;
            _orderDiscountDao = _daoManager.GetDao(typeof(IOrderDiscountDao)) as IOrderDiscountDao;
            _dailyStatementDao = _daoManager.GetDao(typeof(IDailyStatementDao)) as IDailyStatementDao;
            _sysDictionary = _daoManager.GetDao(typeof(ISystemDictionaryDao)) as ISystemDictionaryDao;
        }

        #endregion

        #region Public methods

        public static SalesOrderService GetInstance()
        {
            return _instance;
        }

        public bool CreateSalesOrder(SalesOrder salesOrder)
        {
            bool returnValue = false;
            _daoManager.BeginTransaction();
            try
            {
                //日结
                string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
                if (!string.IsNullOrEmpty(dailyStatementNo))
                {
                    Order order = salesOrder.order;
                    order.DailyStatementNo = dailyStatementNo;
                    //分单号
                    Int32 curSubOrderNo = _orderDao.GetCurrentSubOrderNo(order.DeskName);
                    if (curSubOrderNo > 0)
                    {
                        curSubOrderNo++;
                    }
                    else
                    {
                        curSubOrderNo = 1;
                    }
                    order.SubOrderNo = curSubOrderNo;
                    //流水号
                    order.TranSequence = _sysDictionary.GetCurrentTranSequence();
                    string orderNo = _orderDao.CreateOrder(order);
                    order.OrderNo = orderNo;

                    //菜单品项序号
                    int seqNumber = _orderDetailsDao.GetSequenceNum(order.OrderID);
                    foreach (OrderDetails item in salesOrder.orderDetailsList)
                    {
                        item.DailyStatementNo = dailyStatementNo;
                        item.OrderBy = seqNumber;
                        _orderDetailsDao.CreateOrderDetails(item);
                        seqNumber++;
                    }

                    //折扣信息
                    if (salesOrder.orderDiscountList != null && salesOrder.orderDiscountList.Count > 0)
                    {
                        foreach (OrderDiscount item in salesOrder.orderDiscountList)
                        {
                            item.DailyStatementNo = dailyStatementNo;
                            _orderDiscountDao.CreateOrderDiscount(item);
                        }
                    }
                    returnValue = true;
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

        public bool UpdateSalesOrder(SalesOrder salesOrder)
        {
            bool returnValue = false;
            _daoManager.OpenConnection();

            Order order = salesOrder.order;
            if (_orderDao.UpdateOrder(order))
            {
                //菜单品项序号
                int seqNumber = _orderDetailsDao.GetSequenceNum(order.OrderID);
                foreach (OrderDetails item in salesOrder.orderDetailsList)
                {
                    item.OrderBy = seqNumber;
                    _orderDetailsDao.UpdateOrderDetails(item);
                    seqNumber++;
                }

                //折扣信息
                if (salesOrder.orderDiscountList != null && salesOrder.orderDiscountList.Count > 0)
                {
                    foreach (OrderDiscount item in salesOrder.orderDiscountList)
                    {
                        _orderDiscountDao.UpdateOrderDiscount(item);
                    }
                }
                returnValue = true;
            }

            _daoManager.CloseConnection();
            return returnValue;
        }

        public SalesOrder GetSalesOrder(Guid orderID)
        {
            SalesOrder salesOrder = null;
            _daoManager.OpenConnection();

            Order order = _orderDao.GetOrder(orderID);
            if (order != null)
            {
                IList<OrderDetails> orderDetailsList = _orderDetailsDao.GetOrderDetails(orderID);
                salesOrder = new SalesOrder();
                salesOrder.order = order;
                salesOrder.orderDetailsList = orderDetailsList;
            }

            _daoManager.CloseConnection();
            return salesOrder;
        }

        public bool SplitSalesOrder(SalesSplitOrder salesSplitOrder)
        { 
            bool returnValue = false;
            _daoManager.BeginTransaction();
            try
            {
                //更新账单表头价格
                if (_orderDao.UpdateSplitOrderPrice(salesSplitOrder.OriginalOrder))
                {
                    //减去分单出去的主体以及折扣
                    foreach (OrderDetails item in salesSplitOrder.SubOrderDetailsList)
                    {
                        _orderDetailsDao.SubtractSalesSplitOrder(item);
                    }
                    //新增账单
                    //日结号
                    string serialNumber = _dailyStatementDao.GetCurrentDailyStatementNo();
                    Order order = salesSplitOrder.NewOrder;
                    order.DailyStatementNo = serialNumber;
                    //分单号
                    Int32 curSubOrderNo = _orderDao.GetCurrentSubOrderNo(order.DeskName);
                    if (curSubOrderNo > 0)
                    {
                        curSubOrderNo++;
                    }
                    else
                    {
                        curSubOrderNo = 1;
                    }
                    order.SubOrderNo = curSubOrderNo;
                    //流水号
                    order.TranSequence = _sysDictionary.GetCurrentTranSequence();
                    _orderDao.CreateOrder(order);
                    //菜单品项序号
                    int seqNumber = _orderDetailsDao.GetSequenceNum(order.OrderID);
                    foreach (OrderDetails item in salesSplitOrder.NewOrderDetailsList)
                    {
                        item.OrderBy = seqNumber;
                        _orderDetailsDao.CreateOrderDetails(item);
                        seqNumber++;
                    }
                    returnValue = true;
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
