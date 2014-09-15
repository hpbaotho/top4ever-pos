﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private INoticeDao _noticeDao = null;
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
        private IShopDao _shopDao = null;

        #endregion

        #region Constructor

        private SysBasicDataService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _noticeDao = _daoManager.GetDao(typeof(INoticeDao)) as INoticeDao;
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
            _shopDao = _daoManager.GetDao(typeof(IShopDao)) as IShopDao;
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
                                IList<Guid> detailsGroupIdList = _goodsDao.GetDetailsGroupIDListInGoods(goods.GoodsID);
                                goods.DetailsGroupIDList = detailsGroupIdList;
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
                                IList<Guid> detailsGroupIdList = _detailsDao.GetDetailsGroupIDListInDetails(details.DetailsID);
                                details.DetailsGroupIDList = detailsGroupIdList;
                            }
                        }
                        item.DetailsList = detailsList;
                    }
                }
                // SysBasicData
                basicData.NoticeList = _noticeDao.GetAllNotice();
                basicData.RegionList = regionList;
                basicData.DiscountList = _discountDao.GetAllDiscount();
                basicData.PayoffWayList = _payoffWayDao.GetAllPayoffWay();
                basicData.ReasonList = _reasonDao.GetAllReason();
                basicData.GoodsGroupList = goodsGroupList;
                basicData.DetailsGroupList = detailsGroupList;
                basicData.GoodsSetMealList = _goodsSetMealDao.GetAllGoodsSetMeal();
                basicData.GoodsCronTriggerList = _goodsGroupDao.GetAllGoodsCronTrigger();
                basicData.ButtonStyleList = _buttonStyleDao.GetButtonStyleList();
                basicData.SysConfig = _sysConfigDao.GetSystemConfigInfo();
                basicData.PromotionList = _promotionDao.GetPromotionList();
                basicData.PromotionConditionList = _promotionDao.GetPromotionConditionList();
                basicData.PromotionCronTriggerList = _promotionDao.GetPromotionCronTriggerList();
                basicData.PromotionPresentList = _promotionDao.GetPromotionPresentList();
                //限时特价
                basicData.TotalLimitedTimeSaleList = _goodsGroupDao.GetAllGoodsLimitedTimeSale();
                //组合销售
                basicData.TotalCombinedSaleList = _goodsGroupDao.GetAllGoodsCombinedSale();

                _daoManager.CommitTransaction();
            }
            catch
            {
                //记录日志 todo
                _daoManager.RollBackTransaction();
                basicData = null;
            }

            return basicData;
        }

        public IList<GoodsGroupInfo> GetGoodsGroupListInAndroid()
        {
            IList<GoodsGroupInfo> goodsGroupInfoList = null;
            try
            {
                _daoManager.OpenConnection();
                var goodsGroupList = _goodsGroupDao.GetAllGoodsGroup();
                if (goodsGroupList != null && goodsGroupList.Count > 0)
                {
                    goodsGroupInfoList = new List<GoodsGroupInfo>();
                    foreach (GoodsGroup item in goodsGroupList)
                    {
                        var goodsGroupInfo = new GoodsGroupInfo
                        {
                            GoodsGroupID = item.GoodsGroupID,
                            GoodsGroupName = item.GoodsGroupName,
                            GoodsGroupName2nd = item.GoodsGroupName2nd,
                            GoodsGroupNo = item.GoodsGroupNo
                        };
                        goodsGroupInfoList.Add(goodsGroupInfo);
                    }
                }
            }
            catch
            {
                //记录日志 todo
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return goodsGroupInfoList;
        }

        public IList<GoodsInfo> GetGoodsListInAndroid(Guid goodsGroupId)
        {
            IList<GoodsInfo> goodsInfoList = null;
            try
            {
                _daoManager.OpenConnection();
                IList<Goods> goodsList = _goodsDao.GetGoodsListInGroup(goodsGroupId);
                if (goodsList != null && goodsList.Count > 0)
                {
                    goodsInfoList = new List<GoodsInfo>();
                    foreach (Goods item in goodsList)
                    {
                        var goodsInfo = new GoodsInfo
                        {
                            GoodsID = item.GoodsID,
                            GoodsNo = item.GoodsNo,
                            GoodsName = item.GoodsName,
                            GoodsName2nd = item.GoodsName2nd,
                            Unit = item.Unit,
                            SellPrice = item.SellPrice,
                            AutoShowDetails = item.AutoShowDetails,
                            BrevityCode = item.BrevityCode,
                            PinyinCode = item.PinyinCode,
                            DetailsGroupIds = _goodsDao.GetDetailsGroupIDListInGoods(item.GoodsID)
                        };
                        goodsInfoList.Add(goodsInfo);
                    }
                }
            }
            catch
            {
                //记录日志 todo
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return goodsInfoList;
        }

        public IList<GoodsInfo> GetTopSaleGoods()
        {
            IList<GoodsInfo> goodsInfoList = null;
            try
            {
                _daoManager.OpenConnection();
                var goodsIdList = _goodsDao.GetTopSaleGoodsId();
                if (goodsIdList != null && goodsIdList.Count > 0)
                {
                    IList<Goods> goodsList = _goodsDao.GetGoodsList(goodsIdList);
                    if (goodsList != null && goodsList.Count > 0)
                    {
                        goodsInfoList = new List<GoodsInfo>();
                        foreach (Goods item in goodsList)
                        {
                            var goodsInfo = new GoodsInfo
                            {
                                GoodsID = item.GoodsID,
                                GoodsNo = item.GoodsNo,
                                GoodsName = item.GoodsName,
                                GoodsName2nd = item.GoodsName2nd,
                                Unit = item.Unit,
                                SellPrice = item.SellPrice,
                                AutoShowDetails = item.AutoShowDetails,
                                BrevityCode = item.BrevityCode,
                                PinyinCode = item.PinyinCode
                            };
                            goodsInfoList.Add(goodsInfo);
                        }
                    }
                }
            }
            catch
            {
                //记录日志 todo
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return goodsInfoList;
        }

        #endregion
    }
}
