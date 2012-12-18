using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain;
using Top4ever.Interface;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for RegionService.
    /// </summary>
    public class RegionService
    {
        #region Private Fields

        private static RegionService _instance = new RegionService();
        private IDaoManager _daoManager = null;
        private IRegionDao _regionDao = null;
        private IDeskDao _deskDao = null;

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

            _daoManager.OpenConnection();
            regionList = _regionDao.GetAllBizRegion();
            if (regionList !=null && regionList.Count > 0)
            {
                foreach (BizRegion region in regionList)
                {
                    IList<BizDesk> deskList = _deskDao.GetAllBizDeskByRegion(region.RegionID);
                    region.BizDeskList = deskList;
                }
            }
            _daoManager.CloseConnection();

            return regionList;
        }

        public IList<BizRegion> GetAllBizRegion()
        {
            IList<BizRegion> regionList = null;

            _daoManager.OpenConnection();
            regionList = _regionDao.GetAllBizRegion();
            _daoManager.CloseConnection();

            return regionList;
        }

        #endregion
    }
}
