using System;
using System.Collections.Generic;
using System.Text;

using IBatisNet.DataAccess;

using Top4ever.Domain;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;
using Top4ever.Interface.OrderRelated;

namespace Top4ever.Service
{
    public class ReminderOrderService
    {
        #region Private Fields

        private static ReminderOrderService _instance = new ReminderOrderService();
        private IDaoManager _daoManager = null;
        private IOrderDao _orderDao = null;
        private IOrderDetailsDao _orderDetailsDao = null;
        private IReasonDao _reasonDao = null;
        private ISystemConfigDao _sysConfigDao = null;
        private IPrintTaskDao _printTaskDao = null;

        #endregion

        #region Constructor

        private ReminderOrderService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _orderDao = _daoManager.GetDao(typeof(IOrderDao)) as IOrderDao;
            _orderDetailsDao = _daoManager.GetDao(typeof(IOrderDetailsDao)) as IOrderDetailsDao;
            _reasonDao = _daoManager.GetDao(typeof(IReasonDao)) as IReasonDao;
            _sysConfigDao = _daoManager.GetDao(typeof(ISystemConfigDao)) as ISystemConfigDao;
            _printTaskDao = _daoManager.GetDao(typeof(IPrintTaskDao)) as IPrintTaskDao;
        }

        public static ReminderOrderService GetInstance()
        {
            return _instance;
        }

        public bool CreateReminderOrder(ReminderOrder reminderOrder)
        { 
            bool returnValue = false;
            _daoManager.BeginTransaction();
            try
            {
                Order order = _orderDao.GetOrder(reminderOrder.OrderID);
                if (order != null)
                {
                    IList<OrderDetails> orderDetailsList = new List<OrderDetails>();
                    foreach (Guid orderDetailsID in reminderOrder.OrderDetailsIDList)
                    {
                        OrderDetails orderDetails = _orderDetailsDao.GetOrderDetails(orderDetailsID);
                        orderDetailsList.Add(orderDetails);
                    }
                    SalesOrder salesOrder = new SalesOrder();
                    salesOrder.order = order;
                    salesOrder.orderDetailsList = orderDetailsList;

                    SystemConfig systemConfig = _sysConfigDao.GetSystemConfigInfo();
                    IList<PrintTask> printTaskList = PrintTaskService.GetInstance().GetPrintTaskList(salesOrder, systemConfig.PrintStyle, systemConfig.FollowStyle, 3, reminderOrder.ReasonName);
                    foreach (PrintTask printTask in printTaskList)
                    {
                        _printTaskDao.InsertPrintTask(printTask);
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
