using System;
using System.Collections.Generic;
using System.Linq;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Promotions;
using Top4ever.Entity;

namespace VechsoftPos.Promotions
{
    public class PromotionConditionService
    {
        private IList<PromotionCondition> _promotionConditionList;

        public PromotionConditionService(IList<PromotionCondition> promotionConditionList)
        {
            this._promotionConditionList = promotionConditionList;
        }

        public IList<PromotionCondition> PromotionConditionList
        {
            set { _promotionConditionList = value; }
        }

        public bool IsItemEligible(IList<OrderDetails> orderDetailsList, bool isIncluded, bool AndOr)
        {
            if (_promotionConditionList == null || _promotionConditionList.Count <= 0)
            {
                return false;
            }
            if (orderDetailsList == null || orderDetailsList.Count <= 0)
            {
                return false;
            }
            bool result = false;
            if (isIncluded)
            {
                //包含
                if (AndOr)
                {
                    result = true;
                    foreach (PromotionCondition item in _promotionConditionList)
                    {
                        IList<Guid> goodsIdList = new List<Guid>();
                        for (int index = 0; index < orderDetailsList.Count; index++)
                        {
                            bool hasStatistics = goodsIdList.Any(goodsId => goodsId.Equals(orderDetailsList[index].GoodsID));
                            if (!hasStatistics)
                            {
                                bool isFit = false;
                                bool inPromotion = ItemInPromotion(orderDetailsList[index], item);
                                if (inPromotion)
                                {
                                    //其他判断条件
                                    decimal fitCount = 0M;
                                    for (int j = index; j < orderDetailsList.Count; j++)
                                    {
                                        if (orderDetailsList[index].GoodsID == orderDetailsList[j].GoodsID)
                                        {
                                            bool condition = false;
                                            if (item.MoreOrLess == 1)
                                            {
                                                condition = orderDetailsList[j].SellPrice > item.SellPrice;
                                            }
                                            else if (item.MoreOrLess == 2)
                                            {
                                                condition = orderDetailsList[j].SellPrice == item.SellPrice;
                                            }
                                            else if (item.MoreOrLess == 3)
                                            {
                                                condition = orderDetailsList[j].SellPrice < item.SellPrice;
                                            }
                                            if (condition)
                                            {
                                                condition = orderDetailsList[j].TotalDiscount / orderDetailsList[j].TotalSellPrice <= item.LeastDiscountRate;
                                            }
                                            if (condition)
                                            {
                                                fitCount += orderDetailsList[j].ItemQty;
                                            }
                                        }
                                    }
                                    isFit = fitCount >= item.Quantity;
                                }
                                if (!inPromotion || !isFit)
                                {
                                    result = false;
                                    break;
                                }
                                goodsIdList.Add(orderDetailsList[index].GoodsID);
                            }
                        }
                        if (!result) break;
                    }
                }
                else
                {
                    foreach (PromotionCondition item in _promotionConditionList)
                    {
                        IList<Guid> goodsIdList = new List<Guid>();
                        for (int index = 0; index < orderDetailsList.Count; index++)
                        {
                            bool hasStatistics = goodsIdList.Any(goodsId => goodsId.Equals(orderDetailsList[index].GoodsID));
                            if (!hasStatistics)
                            {
                                if (ItemInPromotion(orderDetailsList[index], item))
                                {
                                    //其他判断条件
                                    decimal fitCount = 0;
                                    for (int j = index; j < orderDetailsList.Count; j++)
                                    {
                                        if (orderDetailsList[index].GoodsID == orderDetailsList[j].GoodsID)
                                        {
                                            bool condition = false;
                                            if (item.MoreOrLess == 1)
                                            {
                                                condition = orderDetailsList[j].SellPrice > item.SellPrice;
                                            }
                                            else if (item.MoreOrLess == 2)
                                            {
                                                condition = orderDetailsList[j].SellPrice == item.SellPrice;
                                            }
                                            else if (item.MoreOrLess == 3)
                                            {
                                                condition = orderDetailsList[j].SellPrice < item.SellPrice;
                                            }
                                            if (condition)
                                            {
                                                condition = orderDetailsList[j].TotalDiscount / orderDetailsList[j].TotalSellPrice <= item.LeastDiscountRate;
                                            }
                                            if (condition)
                                            {
                                                fitCount += orderDetailsList[j].ItemQty;
                                            }
                                        }
                                    }
                                    bool isFit = fitCount >= item.Quantity;
                                    if (isFit)
                                    {
                                        result = true;
                                        break;
                                    }
                                }
                                goodsIdList.Add(orderDetailsList[index].GoodsID);
                            }
                        }
                        if (result) break;
                    }
                }
            }
            else
            { 
                //排除
                result = true;
                foreach (PromotionCondition item in _promotionConditionList)
                {
                    if (orderDetailsList.Any(orderDetails => ItemInPromotion(orderDetails, item)))
                    {
                        result = false;
                    }
                    if (!result) break;
                }
            }
            return result;
        }

        private bool ItemInPromotion(OrderDetails orderDetails, PromotionCondition promotionCondition)
        {
            bool result = false;
            if (promotionCondition.GroupOrItem)
            {
                //组
                if (promotionCondition.GoodsGroupID != null)
                {
                    result = GoodsUtil.IsGoodsInGroup(orderDetails.GoodsID, (Guid) promotionCondition.GoodsGroupID);
                }
            }
            else
            { 
                //品项
                if (orderDetails.GoodsID == promotionCondition.GoodsID)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
