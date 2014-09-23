using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain;
using Top4ever.Interface;
using Top4ever.Utils;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for NoticeService.
    /// </summary>
    public class NoticeService
    {
        #region Private Fields

        private static readonly NoticeService _instance = new NoticeService();
        private readonly IDaoManager _daoManager;
        private readonly INoticeDao _noticeDao;

        #endregion

        #region Constructor

        private NoticeService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _noticeDao = _daoManager.GetDao(typeof(INoticeDao)) as INoticeDao;
        }

        #endregion

        #region Public methods

        public static NoticeService GetInstance()
        {
            return _instance;
        }

        public IList<Notice> GetAllNotice()
        {
            IList<Notice> noticeList = null;
            try
            {
                _daoManager.OpenConnection();
                noticeList = _noticeDao.GetAllNotice();
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error("[GetAllNotice]", exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return noticeList;
        }

        #endregion
    }
}
