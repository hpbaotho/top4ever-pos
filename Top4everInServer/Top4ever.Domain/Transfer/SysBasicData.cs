using System;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Domain.GoodsRelated;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Promotions;

namespace Top4ever.Domain.Transfer
{
    public class SysBasicData
    {
        public Shop CurrentShop { get; set; }

        public IList<BizRegion> RegionList { get; set; }

        public IList<Discount> DiscountList { get; set; }

        public IList<PayoffWay> PayoffWayList { get; set; }

        public IList<Reason> ReasonList { get; set; }

        public IList<GoodsGroup> GoodsGroupList { get; set; }

        public IList<DetailsGroup> DetailsGroupList { get; set; }

        public IList<GoodsSetMeal> GoodsSetMealList { get; set; }

        public IList<GoodsSetMeal> DetailsSetMealList { get; set; }

        public IList<GoodsCronTrigger> GoodsCronTriggerList { get; set; }

        public IList<ButtonStyle> ButtonStyleList { get; set; }

        public SystemConfig SysConfig { get; set; }

        public IList<Promotion> PromotionList { get; set; }

        public IList<PromotionCondition> PromotionConditionList { get; set; }

        public IList<PromotionCronTrigger> PromotionCronTriggerList { get; set; }

        public IList<PromotionPresent> PromotionPresentList { get; set; }
    }
}
