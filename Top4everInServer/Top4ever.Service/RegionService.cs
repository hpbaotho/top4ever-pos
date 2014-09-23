using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;
using Newtonsoft.Json;
using Top4ever.Domain;
using Top4ever.Interface;
using Top4ever.Utils;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for RegionService.
    /// </summary>
    public class RegionService
    {
        #region Private Fields

        private static readonly RegionService _instance = new RegionService();
        private readonly IDaoManager _daoManager;
        private readonly IRegionDao _regionDao;
        private readonly IDeskDao _deskDao;

        #endregion

        #region Constructor

        private RegionService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _regionDao = _daoManager.GetDao(typeof(IRegionDao)) as IRegionDao;
            _deskDao = _daoManager.GetDao(typeof(IDeskDao)) as IDeskDao;
        }

        #endregion

        #region Public methods

        public static RegionService GetInstance()
        {
            return _instance;
        }

        public IList<BizRegion> GetAllRegionAndDesk()
        {
            IList<BizRegion> regionList = null;
            try
            {
                _daoManager.OpenConnection();
                regionList = _regionDao.GetAllBizRegion();
                if (regionList != null && regionList.Count > 0)
                {
                    foreach (BizRegion region in regionList)
                    {
                        IList<BizDesk> deskList = _deskDao.GetAllBizDeskByRegion(region.RegionID);
                        region.BizDeskList = deskList;
                    }
                }
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error("[GetAllRegionAndDesk]", exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return regionList;
        }

        public IList<BizRegion> GetAllBizRegion()
        {
            IList<BizRegion> regionList = null;
            try
            {
                _daoManager.OpenConnection();
                regionList = _regionDao.GetAllBizRegion();
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error("[GetAllBizRegion]", exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return regionList;
        }

        #endregion
    }
}
