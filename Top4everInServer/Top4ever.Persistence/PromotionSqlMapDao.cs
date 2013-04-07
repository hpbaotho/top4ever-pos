using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.Domain.Promotions;
using Top4ever.Interface;

namespace Top4ever.Persistence
{
    /// <summary>
    /// Summary description for PromotionSqlMapDao
    /// </summary>
    public class PromotionSqlMapDao : BaseSqlMapDao, IPromotionDao
    {
        public IList<Promotion> GetPromotionList()
        {
            return ExecuteQueryForList<Promotion>("GetAllPromotion", null);
        }

        public IList<PromotionCondition> GetPromotionConditionList()
        {
            return ExecuteQueryForList<PromotionCondition>("GetAllPromotionCondition", null);
        }

        public IList<PromotionCronTrigger> GetPromotionCronTriggerList()
        {
            return ExecuteQueryForList<PromotionCronTrigger>("GetAllPromotionCronTrigger", null);
        }

        public IList<PromotionPresent> GetPromotionPresentList()
        {
            return ExecuteQueryForList<PromotionPresent>("GetAllPromotionPresent", null);
        }
    }
}
