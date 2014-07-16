using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for DeskService.
    /// </summary>
    public class DeskService
    {
        #region Private Fields

        private static DeskService _instance = new DeskService();
        private IDaoManager _daoManager = null;
        private IDeskDao _deskDao = null;

        #endregion

        #region Constructor

        private DeskService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _deskDao = _daoManager.GetDao(typeof(IDeskDao)) as IDeskDao;
        }

        #endregion

        #region Public methods

        public static DeskService GetInstance()
        {
            return _instance;
        }

        public IList<BizDesk> GetAllBizDeskByRegion(Guid regionID)
        {
            IList<BizDesk> deskList = null;

            _daoManager.OpenConnection();
            deskList = _deskDao.GetAllBizDeskByRegion(regionID);
            _daoManager.CloseConnection();

            return deskList;
        }

        public IList<string> GetAllDeskName()
        {
            IList<string> deskNameList = null;

            _daoManager.OpenConnection();
            deskNameList = _deskDao.GetAllDeskName();
            _daoManager.CloseConnection();

            return deskNameList;
        }

        public BizDesk GetBizDeskByName(string deskName)
        {
            BizDesk desk = null;

            _daoManager.OpenConnection();
            desk = _deskDao.GetBizDeskByName(deskName);
            _daoManager.CloseConnection();

            return desk;
        }

        public bool UpdateBizDeskStatus(BizDesk desk)
        {
            bool result = false;

            _daoManager.OpenConnection();
            result = _deskDao.UpdateBizDeskStatus(desk);
            _daoManager.CloseConnection();

            return result;
        }

        public IList<DeskRealTimeInfo> GetDeskRealTimeInfo(Guid regionID)
        {
            IList<DeskRealTimeInfo> deskInfoList = null;

            _daoManager.OpenConnection();
            deskInfoList = _deskDao.GetDeskRealTimeInfo(regionID);
            _daoManager.CloseConnection();

            return deskInfoList;
        }

        public IList<DeskInfo> GetDeskList()
        {
            IList<DeskInfo> deskInfoList = null;

            _daoManager.OpenConnection();
            deskInfoList = _deskDao.GetDeskList();
            _daoManager.CloseConnection();

            return deskInfoList;
        }

        #endregion
    }
}
