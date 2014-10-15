using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;
using Newtonsoft.Json;
using Top4ever.Domain;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;
using Top4ever.Interface.OrderRelated;
using Top4ever.Utils;

namespace Top4ever.Service
{
    public class ReminderOrderService
    {
        #region Private Fields

        private static readonly ReminderOrderService _instance = new ReminderOrderService();
        private readonly IDaoManager _daoManager;
        private readonly IOrderDao _orderDao;
        private readonly IOrderDetailsDao _orderDetailsDao;
        private readonly IReasonDao _reasonDao;
        private readonly ISystemConfigDao _sysConfigDao;
        private readonly IPrintTaskDao _printTaskDao;

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
            if (reminderOrder == null || reminderOrder.OrderDetailsIDList == null || reminderOrder.OrderDetailsIDList.Count <= 0)
            {
                return false;
            }
            bool returnValue = false;
            try
            {
                _daoManager.OpenConnection();
                //添加打印任务
                SystemConfig systemConfig = _sysConfigDao.GetSystemConfigInfo();
                if (systemConfig.IncludeKitchenPrint)
                {
                    Order order = _orderDao.GetOrder(reminderOrder.OrderID);
                    if (order != null)
                    {
                        SalesOrder salesOrder = new SalesOrder
                        {
                            order = order,
                            orderDetailsList = _orderDetailsDao.GetOrderDetailsList(reminderOrder.OrderDetailsIDList)
                        };
                        IList<PrintTask> printTaskList = PrintTaskService.GetInstance().GetPrintTaskList(salesOrder, systemConfig.PrintStyle, systemConfig.FollowStyle, systemConfig.PrintType, 3, reminderOrder.ReasonName);
                        foreach (PrintTask printTask in printTaskList)
                        {
                            _printTaskDao.InsertPrintTask(printTask);
                        }
                    }
                }
                returnValue = true;
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[CreateReminderOrder]参数：reminderOrder_{0}", JsonConvert.SerializeObject(reminderOrder)), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return returnValue;
        }

        #endregion
    }
}
