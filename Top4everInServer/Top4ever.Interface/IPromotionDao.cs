using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.Domain.Promotions;

namespace Top4ever.Interface
{
    public interface IPromotionDao
    {
        IList<Promotion> GetPromotionList();

        IList<PromotionCondition> GetPromotionConditionList();

        IList<PromotionCronTrigger> GetPromotionCronTriggerList();

        IList<PromotionPresent> GetPromotionPresentList();
    }
}
