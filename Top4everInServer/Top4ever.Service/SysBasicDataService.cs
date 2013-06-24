using System;
using System.Collections;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain;
using Top4ever.Domain.GoodsRelated;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;
using Top4ever.Interface.Accounts;
using Top4ever.Interface.GoodsRelated;
using Top4ever.Interface.OrderRelated;
using Top4ever.Domain.Promotions;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for SysBasicDataService.
    /// </summary>
    public class SysBasicDataService
    {
        #region Private Fields

        private static SysBasicDataService _instance = new SysBasicDataService();
        private IDaoManager _daoManager = null;

        private IShopDao _shopDao = null;
        private IRegionDao _regionDao = null;
        private IDeskDao _deskDao = null;
        private IDiscountDao _discountDao = null;
        private IPayoffWayDao _payoffWayDao = null;
        private IReasonDao _reasonDao = null;
        private IGoodsGroupDao _goodsGroupDao = null;
        private IGoodsDao _goodsDao = null;
        private IDetailsGroupDao _detailsGroupDao = null;
        private IDetailsDao _detailsDao = null;
        private IGoodsSetMealDao _goodsSetMealDao = null;
        private IButtonStyleDao _buttonStyleDao = null;
        private ISystemConfigDao _sysConfigDao = null;
        private IPromotionDao _promotionDao = null;

        #endregion

        #region Constructor

        private SysBasicDataService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _shopDao = _daoManager.GetDao(typeof(IShopDao)) as IShopDao;
            _regionDao = _daoManager.GetDao(typeof(IRegionDao)) as IRegionDao;
            _deskDao = _daoManager.GetDao(typeof(IDeskDao)) as IDeskDao;
            _discountDao = _daoManager.GetDao(typeof(IDiscountDao)) as IDiscountDao;
            _payoffWayDao = _daoManager.GetDao(typeof(IPayoffWayDao)) as IPayoffWayDao;
            _reasonDao = _daoManager.GetDao(typeof(IReasonDao)) as IReasonDao;
            _goodsGroupDao = _daoManager.GetDao(typeof(IGoodsGroupDao)) as IGoodsGroupDao;
            _goodsDao = _daoManager.GetDao(typeof(IGoodsDao)) as IGoodsDao;
            _detailsGroupDao = _daoManager.GetDao(typeof(IDetailsGroupDao)) as IDetailsGroupDao;
            _detailsDao = _daoManager.GetDao(typeof(IDetailsDao)) as IDetailsDao;
            _goodsSetMealDao = _daoManager.GetDao(typeof(IGoodsSetMealDao)) as IGoodsSetMealDao;
            _buttonStyleDao = _daoManager.GetDao(typeof(IButtonStyleDao)) as IButtonStyleDao;
            _sysConfigDao = _daoManager.GetDao(typeof(ISystemConfigDao)) as ISystemConfigDao;
            _promotionDao = _daoManager.GetDao(typeof(IPromotionDao)) as IPromotionDao;
        }

        #endregion

        #region Public methods

        public static SysBasicDataService GetInstance()
        {
            return _instance;
        }

        public SysBasicData GetSysBasicData()
        {
            SysBasicData basicData = new SysBasicData();

            _daoManager.BeginTransaction();
            try
            {
                // shop
                Shop CurrentShop = _shopDao.GetSingleShop();
                // region
                IList<BizRegion> regionList = _regionDao.GetAllBizRegion();
                if (regionList != null && regionList.Count > 0)
                {
                    foreach (BizRegion region in regionList)
                    {
                        IList<BizDesk> deskList = _deskDao.GetAllBizDeskByRegion(region.RegionID);
                        region.BizDeskList = deskList;
                    }
                }
                // discount
                IList<Discount> discountList = _discountDao.GetAllDiscount();
                // payoffWay
                IList<PayoffWay> payoffWayList = _payoffWayDao.GetAllPayoffWay();
                // reason
                IList<Reason> reasonList = _reasonDao.GetAllReason();
                // goodsGroup
                IList<GoodsGroup> goodsGroupList = _goodsGroupDao.GetAllGoodsGroup();
                if (goodsGroupList != null && goodsGroupList.Count > 0)
                {
                    foreach (GoodsGroup item in goodsGroupList)
                    {
                        IList<Goods> goodsList = _goodsDao.GetGoodsListInGroup(item.GoodsGroupID);
                        if (goodsList != null && goodsList.Count > 0)
                        {
                            foreach (Goods goods in goodsList)
                            {
                                IList<Guid> detailsGroupIDList = _goodsDao.GetDetailsGroupIDListInGoods(goods.GoodsID);
                                goods.DetailsGroupIDList = detailsGroupIDList;
                            }
                        }
                        item.GoodsList = goodsList;
                    }
                }
                // detailsGroup
                IList<DetailsGroup> detailsGroupList = _detailsGroupDao.GetAllDetailsGroup();
                if (detailsGroupList != null && detailsGroupList.Count > 0)
                {
                    foreach (DetailsGroup item in detailsGroupList)
                    {
                        IList<Details> detailsList = _detailsDao.GetDetailsListInGroup(item.DetailsGroupID);
                        if (detailsList != null && detailsList.Count > 0)
                        {
                            foreach (Details details in detailsList)
                            {
                                IList<Guid> detailsGroupIDList = _detailsDao.GetDetailsGroupIDListInDetails(details.DetailsID);
                                details.DetailsGroupIDList = detailsGroupIDList;
                            }
                        }
                        item.DetailsList = detailsList;
                    }
                }
                IList<GoodsSetMeal> goodsSetMealList = _goodsSetMealDao.GetAllGoodsSetMeal();
                IList<GoodsCronTrigger> goodsCronTriggerList = _goodsGroupDao.GetAllGoodsCronTrigger();
                //ButtonStyle
                IList<ButtonStyle> buttonStyleList = _buttonStyleDao.GetButtonStyleList();
                //SystemConfig
                SystemConfig systemConfig = _sysConfigDao.GetSystemConfigInfo();
                //Promotion
                IList<Promotion> promotionList = _promotionDao.GetPromotionList();
                //PromotionCondition
                IList<PromotionCondition> promotionConditionList = _promotionDao.GetPromotionConditionList();
                //PromotionCronTrigger
                IList<PromotionCronTrigger> promotionCronTriggerList = _promotionDao.GetPromotionCronTriggerList();
                //PromotionPresent
                IList<PromotionPresent> promotionPresentList = _promotionDao.GetPromotionPresentList();
                // SysBasicData
                basicData.CurrentShop = CurrentShop;
                basicData.RegionList = regionList;
                basicData.DiscountList = discountList;
                basicData.PayoffWayList = payoffWayList;
                basicData.ReasonList = reasonList;
                basicData.GoodsGroupList = goodsGroupList;
                basicData.DetailsGroupList = detailsGroupList;
                basicData.GoodsSetMealList = goodsSetMealList;
                basicData.GoodsCronTriggerList = goodsCronTriggerList;
                basicData.ButtonStyleList = buttonStyleList;
                basicData.SysConfig = systemConfig;
                basicData.PromotionList = promotionList;
                basicData.PromotionConditionList = promotionConditionList;
                basicData.PromotionCronTriggerList = promotionCronTriggerList;
                basicData.PromotionPresentList = promotionPresentList;

                _daoManager.CommitTransaction();
            }
            catch
            {
                _daoManager.RollBackTransaction();
                basicData = null;
            }

            return basicData;
        }

        #endregion
    }
}
