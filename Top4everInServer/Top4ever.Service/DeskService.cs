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
    /// <summary>
    /// Summary description for DeskService.
    /// </summary>
    public class DeskService
    {
        #region Private Fields

        private static readonly DeskService _instance = new DeskService();
        private readonly IDaoManager _daoManager;
        private readonly IDeskDao _deskDao;

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

        public IList<BizDesk> GetAllBizDeskByRegion(Guid regionId)
        {
            IList<BizDesk> deskList = null;
            try
            {
                _daoManager.OpenConnection();
                deskList = _deskDao.GetAllBizDeskByRegion(regionId);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[GetAllBizDeskByRegion]参数：regionId_{0}", regionId), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return deskList;
        }

        public IList<string> GetAllDeskName()
        {
            IList<string> deskNameList = null;
            try
            {
                _daoManager.OpenConnection();
                deskNameList = _deskDao.GetAllDeskName();
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error("[GetAllDeskName]", exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return deskNameList;
        }

        public BizDesk GetBizDeskByName(string deskName)
        {
            BizDesk desk = null;
            try
            {
                _daoManager.OpenConnection();
                desk = _deskDao.GetBizDeskByName(deskName);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error("[GetBizDeskByName]参数：deskName_" + deskName, exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return desk;
        }

        public bool UpdateBizDeskStatus(BizDesk desk)
        {
            bool result = false;
            try
            {
                _daoManager.OpenConnection();
                result = _deskDao.UpdateBizDeskStatus(desk);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[UpdateBizDeskStatus]参数：desk_{0}", JsonConvert.SerializeObject(desk)), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return result;
        }

        public IList<DeskRealTimeInfo> GetDeskRealTimeInfo(Guid regionId)
        {
            IList<DeskRealTimeInfo> deskInfoList = null;
            try
            {
                _daoManager.OpenConnection();
                deskInfoList = _deskDao.GetDeskRealTimeInfo(regionId);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error("[GetDeskRealTimeInfo]参数：regionId_" + regionId, exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return deskInfoList;
        }

        public IList<DeskInfo> GetDeskList()
        {
            IList<DeskInfo> deskInfoList = null;
            try
            {
                _daoManager.OpenConnection();
                deskInfoList = _deskDao.GetDeskList();
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error("[GetDeskList]", exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return deskInfoList;
        }

        #endregion
    }
}
