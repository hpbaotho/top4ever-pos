using System;

using IBatisNet.Common.Logging;
using IBatisNet.DataAccess;

using Top4ever.Domain;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;
using Top4ever.Interface.OrderRelated;
using System.Collections.Generic;

namespace Top4ever.Service
{
    public class DailyBalanceService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DailyBalanceService));

        #region Private Fields

        private static DailyBalanceService _instance = new DailyBalanceService();
        private IDaoManager _daoManager = null;
        private IOrderDao _orderDao = null;
        private IDailyStatementDao _dailyStatementDao = null;
        private IDailyTurnoverDao _dailyTurnoverDao = null;

        #endregion

        #region Constructor

        private DailyBalanceService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _orderDao = _daoManager.GetDao(typeof(IOrderDao)) as IOrderDao;
            _dailyStatementDao = _daoManager.GetDao(typeof(IDailyStatementDao)) as IDailyStatementDao;
            _dailyTurnoverDao = _daoManager.GetDao(typeof(IDailyTurnoverDao)) as IDailyTurnoverDao;
        }

        #endregion

        #region Public methods

        public static DailyBalanceService GetInstance()
        {
            return _instance;
        }

        public Int32 CreateDailyBalance(DailyBalance dailyBalance, out string unCheckDeviceNo)
        {
            unCheckDeviceNo = string.Empty;
            int returnValue = 0;    //日结失败
            _daoManager.BeginTransaction();
            try
            {
                //日结号
                string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
                //更新日结信息
                DailyStatement dailyStatement = dailyBalance.dailyStatement;
                dailyStatement.DailyStatementNo = dailyStatementNo;
                returnValue = _dailyStatementDao.UpdateDailyStatement(dailyStatement, out unCheckDeviceNo);
                if (returnValue == 1)
                {
                    //插入日结金额
                    DailyTurnover dailyTurnover = dailyBalance.dailyTurnover;
                    dailyTurnover.DailyStatementNo = dailyStatementNo;
                    _dailyTurnoverDao.CreateDailyTurnover(dailyTurnover);
                    //创建新的日结
                    DailyStatement item = new DailyStatement();
                    item.DailyStatementID = Guid.NewGuid();
                    item.DailyStatementNo = DateTime.Now.ToString("yyMMddHHmmssff");
                    _dailyStatementDao.CreateDailyStatement(item);
                }
                _daoManager.CommitTransaction();
            }
            catch
            {
                _daoManager.RollBackTransaction();
                returnValue = 0;
            }
            return returnValue;
        }

        public string GetDailyStatementTimeInterval()
        {
            string timeInterval = string.Empty;
            _daoManager.OpenConnection();
            //日结号
            string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
            if (!string.IsNullOrEmpty(dailyStatementNo))
            {
                timeInterval = _dailyStatementDao.GetDailyStatementTimeInterval(dailyStatementNo);
            }
            _daoManager.CloseConnection();
            return timeInterval;
        }

        public Int32 CheckLastDailyStatement()
        {
            // 0:操作失败 1:正常 2:未日结 3:时间跨度超过1天，可能服务器时间不对
            int status = 0;
            try
            {
                _daoManager.OpenConnection();
                DateTime lastBelongDate = _dailyStatementDao.GetLastDailyStatementDate();
                DateTime beginTime = DateTime.Parse(lastBelongDate.AddDays(1).ToString("yyyy-MM-dd 05:00:00"));
                DateTime endTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 05:00:00"));
                bool IsExist = _orderDao.IsExistOrderInTimeInterval(beginTime, endTime);
                if (IsExist)
                {
                    status = 2;
                }
                else
                {
                    TimeSpan ts = beginTime - endTime;
                    if (Math.Abs(ts.TotalDays) > 1)
                    {
                        status = 3;
                    }
                    else
                    {
                        status = 1;
                    }
                }
                _daoManager.CloseConnection();
            }
            catch (Exception ex)
            {
                status = 0;
                logger.Error("Database operation failed !", ex);
            }
            return status;
        }

        /// <summary>
        /// 根据所属日期获取营业时间段
        /// </summary>
        /// <param name="belongToDate">所属日期</param>
        /// <returns></returns>
        public IList<DailyBalanceTime> GetDailyBalanceTime(DateTime belongToDate)
        {
            IList<DailyBalanceTime> dailyBalanceTimeList = null;
            _daoManager.OpenConnection();
            if (belongToDate > DateTime.MinValue)
            {
                dailyBalanceTimeList = _dailyStatementDao.GetDailyBalanceTime(belongToDate);
            }
            _daoManager.CloseConnection();
            return dailyBalanceTimeList;
        }

        #endregion
    }
}
