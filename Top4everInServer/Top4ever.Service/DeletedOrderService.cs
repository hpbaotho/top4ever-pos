using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;
using Top4ever.Interface.OrderRelated;
namespace Top4ever.Service
{
    public class DeletedOrderService
    {
        #region Private Fields

        private static DeletedOrderService _instance = new DeletedOrderService();
        private IDaoManager _daoManager = null;
        private IOrderDao _orderDao = null;
        private IOrderDetailsDao _orderDetailsDao = null;
        private IOrderDiscountDao _orderDiscountDao = null;
        private IOrderPayoffDao _orderPayoffDao = null;
        private ISystemConfigDao _sysConfigDao = null;
        private IPrintTaskDao _printTaskDao = null;
        private IDailyStatementDao _dailyStatementDao = null;

        #endregion

        #region Constructor

        private DeletedOrderService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _orderDao = _daoManager.GetDao(typeof(IOrderDao)) as IOrderDao;
            _orderDetailsDao = _daoManager.GetDao(typeof(IOrderDetailsDao)) as IOrderDetailsDao;
            _orderDiscountDao = _daoManager.GetDao(typeof(IOrderDiscountDao)) as IOrderDiscountDao;
            _orderPayoffDao = _daoManager.GetDao(typeof(IOrderPayoffDao)) as IOrderPayoffDao;
            _sysConfigDao = _daoManager.GetDao(typeof(ISystemConfigDao)) as ISystemConfigDao;
            _printTaskDao = _daoManager.GetDao(typeof(IPrintTaskDao)) as IPrintTaskDao;
            _dailyStatementDao = _daoManager.GetDao(typeof(IDailyStatementDao)) as IDailyStatementDao;
        }

        #endregion

        #region Public methods

        public static DeletedOrderService GetInstance()
        {
            return _instance;
        }

        public bool DeleteSingleOrder(DeletedSingleOrder deletedSingleOrder)
        {
            bool returnValue = false;
            _daoManager.BeginTransaction();
            try
            {
                Order order = new Order();
                order.OrderID = deletedSingleOrder.OrderID;
                order.TotalSellPrice = deletedSingleOrder.TotalSellPrice;
                order.ActualSellPrice = deletedSingleOrder.ActualSellPrice;
                order.DiscountPrice = deletedSingleOrder.DiscountPrice;
                order.CutOffPrice = deletedSingleOrder.CutOffPrice;
                if (_orderDao.UpdateOrderPrice(order))
                {
                    foreach (DeletedOrderDetails item in deletedSingleOrder.deletedOrderDetailsList)
                    {
                        _orderDetailsDao.DeleteSingleOrderDetails(item);
                    }
                    //获取打印任务列表
                    Order tempOrder = _orderDao.GetOrder(deletedSingleOrder.OrderID);
                    if (tempOrder != null)
                    {
                        SalesOrder salesOrder = new SalesOrder();
                        salesOrder.order = tempOrder;
                        IList<OrderDetails> tempOrderDetailsList = new List<OrderDetails>();
                        string cancelReason = string.Empty;
                        foreach (DeletedOrderDetails item in deletedSingleOrder.deletedOrderDetailsList)
                        {
                            OrderDetails orderDetails = _orderDetailsDao.GetOrderDetails(item.OrderDetailsID);
                            orderDetails.ItemQty = item.DeletedQuantity;
                            tempOrderDetailsList.Add(orderDetails);
                            if (string.IsNullOrEmpty(cancelReason) && !string.IsNullOrEmpty(item.CancelReasonName))
                            {
                                cancelReason = item.CancelReasonName;
                            }
                        }
                        salesOrder.orderDetailsList = tempOrderDetailsList;
                        SystemConfig systemConfig = _sysConfigDao.GetSystemConfigInfo();
                        IList<PrintTask> printTaskList = PrintTaskService.GetInstance().GetPrintTaskList(salesOrder, systemConfig.PrintStyle, systemConfig.FollowStyle, 2, cancelReason);
                        foreach (PrintTask printTask in printTaskList)
                        {
                            _printTaskDao.InsertPrintTask(printTask);
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

        public bool DeleteWholeOrder(DeletedOrder deletedOrder)
        {
            bool returnValue = false;
            _daoManager.BeginTransaction();
            try
            {
                //获取打印任务列表
                Order order = _orderDao.GetOrder(deletedOrder.OrderID);
                IList<OrderDetails> orderDetailsList = _orderDetailsDao.GetOrderDetailsList(deletedOrder.OrderID);
                SalesOrder salesOrder = new SalesOrder();
                salesOrder.order = order;
                salesOrder.orderDetailsList = orderDetailsList;
                SystemConfig systemConfig = _sysConfigDao.GetSystemConfigInfo();
                IList<PrintTask> printTaskList = PrintTaskService.GetInstance().GetPrintTaskList(salesOrder, systemConfig.PrintStyle, systemConfig.FollowStyle, 2, deletedOrder.CancelReasonName);
                foreach (PrintTask printTask in printTaskList)
                {
                    printTask.ItemQty = -printTask.ItemQty; //数量应该为负数
                    _printTaskDao.InsertPrintTask(printTask);
                }
                //删除账单
                if (_orderDao.DeleteWholeOrder(deletedOrder))
                {
                    if (_orderDetailsDao.DeleteWholeOrderDetails(deletedOrder))
                    {
                        //该订单可能不包含折扣
                        _orderDiscountDao.DeleteOrderDiscount(deletedOrder.OrderID);
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

        public bool DeletePaidSingleOrder(DeletedPaidOrder deletedPaidOrder)
        {
            bool returnValue = false;
            _daoManager.BeginTransaction();
            try
            {
                if (deletedPaidOrder != null)
                {
                    if (_orderDao.UpdatePaidOrderPrice(deletedPaidOrder.order))
                    {
                        foreach (DeletedOrderDetails item in deletedPaidOrder.deletedOrderDetailsList)
                        {
                            _orderDetailsDao.DeleteSingleOrderDetails(item);
                        }
                        _orderPayoffDao.DeleteOrderPayoff(deletedPaidOrder.order.OrderID);
                        //日结号
                        string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
                        //插入OrderPayoff
                        foreach (OrderPayoff item in deletedPaidOrder.orderPayoffList)
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

        public bool DeletePaidWholeOrder(DeletedOrder deletedOrder)
        {
            bool returnValue = false;
            _daoManager.BeginTransaction();
            try
            {
                if (_orderDao.DeleteWholeOrder(deletedOrder))
                {
                    if (_orderDetailsDao.DeleteWholeOrderDetails(deletedOrder))
                    {
                        _orderDiscountDao.DeleteOrderDiscount(deletedOrder.OrderID);
                        _orderPayoffDao.DeleteOrderPayoff(deletedOrder.OrderID);
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
