using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain;
using Top4ever.Interface;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for NoticeService.
    /// </summary>
    public class NoticeService
    {
        #region Private Fields

        private static NoticeService _instance = new NoticeService();
        private IDaoManager _daoManager = null;
        private INoticeDao _noticeDao = null;

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

            _daoManager.OpenConnection();
            noticeList = _noticeDao.GetAllNotice();
            _daoManager.CloseConnection();

            return noticeList;
        }

        #endregion
    }
}
