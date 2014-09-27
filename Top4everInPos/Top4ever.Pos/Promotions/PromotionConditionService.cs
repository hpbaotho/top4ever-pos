using System;
using System.Collections.Generic;

using Top4ever.Domain.GoodsRelated;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Promotions;
using Top4ever.Entity;

namespace VechsoftPos.Promotions
{
    public class PromotionConditionService
    {
        private IList<PromotionCondition> promotionConditionList;

        public PromotionConditionService(IList<PromotionCondition> promotionConditionList)
        {
            this.promotionConditionList = promotionConditionList;
        }

        public IList<PromotionCondition> PromotionConditionList
        {
            set { promotionConditionList = value; }
        }

        public bool IsItemEligible(IList<OrderDetails> orderDetailsList, bool IsIncluded, bool AndOr)
        {
            if (promotionConditionList == null || promotionConditionList.Count == 0)
            {
                return false;
            }
            else
            {
                bool result = false;
                if (IsIncluded)
                {
                    //包含
                    if (AndOr)
                    {
                        result = true;
                        foreach (PromotionCondition item in promotionConditionList)
                        {
                            IList<Guid> goodsIdList = new List<Guid>();
                            for (int index = 0; index < orderDetailsList.Count; index++)
                            {
                                if (!HasStatistics(orderDetailsList[index].GoodsID, goodsIdList))
                                {
                                    bool condition = false;
                                    bool InPromotion = ItemInPromotion(orderDetailsList[index], item);
                                    if (InPromotion)
                                    {
                                        //其他判断条件
                                        decimal fitCount = 0M;
                                        for (int j = index; j < orderDetailsList.Count; j++)
                                        {
                                            if (orderDetailsList[index].GoodsID == orderDetailsList[j].GoodsID)
                                            {
                                                if (item.MoreOrLess == 1)
                                                {
                                                    condition = orderDetailsList[j].SellPrice > item.SellPrice;
                                                }
                                                if (item.MoreOrLess == 2)
                                                {
                                                    condition = orderDetailsList[j].SellPrice == item.SellPrice;
                                                }
                                                if (item.MoreOrLess == 3)
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
                                        condition = fitCount >= item.Quantity;
                                    }
                                    if (!InPromotion || !condition)
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
                        foreach (PromotionCondition item in promotionConditionList)
                        {
                            IList<Guid> goodsIdList = new List<Guid>();
                            for (int index = 0; index < orderDetailsList.Count; index++)
                            {
                                if (!HasStatistics(orderDetailsList[index].GoodsID, goodsIdList))
                                {
                                    if (ItemInPromotion(orderDetailsList[index], item))
                                    {
                                        //其他判断条件
                                        bool condition = false;
                                        decimal fitCount = 0;
                                        for (int j = index; j < orderDetailsList.Count; j++)
                                        {
                                            if (orderDetailsList[index].GoodsID == orderDetailsList[j].GoodsID)
                                            {
                                                if (item.MoreOrLess == 1)
                                                {
                                                    condition = orderDetailsList[j].SellPrice > item.SellPrice;
                                                }
                                                if (item.MoreOrLess == 2)
                                                {
                                                    condition = orderDetailsList[j].SellPrice == item.SellPrice;
                                                }
                                                if (item.MoreOrLess == 3)
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
                                        condition = fitCount >= item.Quantity;
                                        if (condition)
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
                    foreach (PromotionCondition item in promotionConditionList)
                    {
                        foreach (OrderDetails orderDetails in orderDetailsList)
                        { 
                            if(ItemInPromotion(orderDetails, item))
                            {
                                result = false;
                                break;
                            }
                        }
                        if (!result) break;
                    }
                }
                return result;
            }
        }

        private bool ItemInPromotion(OrderDetails orderDetails, PromotionCondition promotionCondition)
        {
            bool result = false;
            if (promotionCondition.GroupOrItem)
            {
                //组
                result = GoodsUtil.IsGoodsInGroup(orderDetails.GoodsID, (Guid)promotionCondition.GoodsGroupID);
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

        private bool HasStatistics(Guid goodsId, IList<Guid> goodsIdList)
        {
            bool find = false;
            foreach (Guid item in goodsIdList)
            {
                if (goodsId == item)
                {
                    find = true;
                    break;
                }
            }
            return find;
        }
    }
}
