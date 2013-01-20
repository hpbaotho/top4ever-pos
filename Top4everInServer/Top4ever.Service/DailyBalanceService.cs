using System;

using IBatisNet.DataAccess;

using Top4ever.Domain;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;

namespace Top4ever.Service
{
    public class DailyBalanceService
    {
        #region Private Fields

        private static DailyBalanceService _instance = new DailyBalanceService();
        private IDaoManager _daoManager = null;
        private IDailyStatementDao _dailyStatementDao = null;
        private IDailyTurnoverDao _dailyTurnoverDao = null;

        #endregion

        #region Constructor

        private DailyBalanceService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _dailyStatementDao = _daoManager.GetDao(typeof(IDailyStatementDao)) as IDailyStatementDao;
            _dailyTurnoverDao = _daoManager.GetDao(typeof(IDailyTurnoverDao)) as IDailyTurnoverDao;
        }

        #endregion

        #region Public methods

        public static DailyBalanceService GetInstance()
        {
            return _instance;
        }

        public Int32 CreateDailyBalance(DailyBalance dailyBalance)
        {
            int returnValue = 0;    //日结失败
            _daoManager.BeginTransaction();
            try
            {
                //日结号
                string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
                //更新日结信息
                DailyStatement dailyStatement = dailyBalance.dailyStatement;
                dailyStatement.DailyStatementNo = dailyStatementNo;
                returnValue = _dailyStatementDao.UpdateDailyStatement(dailyStatement);
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
            //日结号
            string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
            if (!string.IsNullOrEmpty(dailyStatementNo))
            {
                timeInterval = _dailyStatementDao.GetDailyStatementTimeInterval(dailyStatementNo);
            }
            return timeInterval;
        }

        #endregion
    }
}
