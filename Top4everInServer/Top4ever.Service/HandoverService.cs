using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;
using Newtonsoft.Json;
using Top4ever.Domain;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;
using Top4ever.Utils;

namespace Top4ever.Service
{
    public class HandoverService
    {
        #region Private Fields

        private static readonly HandoverService _instance = new HandoverService();
        private readonly IDaoManager _daoManager;
        private readonly IDailyStatementDao _dailyStatementDao;
        private readonly IHandoverRecordDao _handoverRecord;
        private readonly IHandoverTurnoverDao _handoverTurnover;

        #endregion

        #region Constructor

        private HandoverService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _dailyStatementDao = _daoManager.GetDao(typeof(IDailyStatementDao)) as IDailyStatementDao;
            _handoverRecord = _daoManager.GetDao(typeof(IHandoverRecordDao)) as IHandoverRecordDao;
            _handoverTurnover = _daoManager.GetDao(typeof(IHandoverTurnoverDao)) as IHandoverTurnoverDao;
        }

        #endregion

        #region Public methods

        public static HandoverService GetInstance()
        {
            return _instance;
        }

        public bool CreateHandover(HandoverInfo handover)
        {
            bool returnValue = false;
            _daoManager.BeginTransaction();
            try
            {
                //日结号
                string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
                if (!string.IsNullOrEmpty(dailyStatementNo))
                {
                    //交班记录
                    HandoverRecord handoverRecord = handover.handoverRecord;
                    handoverRecord.DailyStatementNo = dailyStatementNo;
                    _handoverRecord.CreateHandoverRecord(handoverRecord);
                    //交班金额列表
                    if (handover.handoverTurnoverList != null && handover.handoverTurnoverList.Count > 0)
                    {
                        foreach (HandoverTurnover item in handover.handoverTurnoverList)
                        {
                            item.DailyStatementNo = dailyStatementNo;
                            _handoverTurnover.CreateHandoverTurnover(item);
                        }
                    }
                    returnValue = true;
                }
                _daoManager.CommitTransaction();
            }
            catch(Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[CreateHandover]参数：handover_{0}", JsonConvert.SerializeObject(handover)), exception);
                _daoManager.RollBackTransaction();
                returnValue = false;
            }
            return returnValue;
        }

        /// <summary>
        /// 获取交班记录
        /// </summary>
        /// <param name="dailyStatementNo">日结号</param>
        /// <returns></returns>
        public IList<EmployeeHandoverRecord> GetHandoverRecord(string dailyStatementNo)
        {
            IList<EmployeeHandoverRecord> handoverRecordList = null;
            try
            {
                _daoManager.OpenConnection();
                if (!string.IsNullOrEmpty(dailyStatementNo))
                {
                    handoverRecordList = _handoverRecord.GetHandoverRecord(dailyStatementNo);
                }
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error("[GetHandoverRecord]参数：dailyStatementNo_" + dailyStatementNo, exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return handoverRecordList;
        }

        #endregion
    }
}
