using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain.GoodsRelated;
using Top4ever.Interface.GoodsRelated;

namespace Top4ever.Persistence.GoodsRelated
{
    /// <summary>
    /// Summary description for GoodsSetMealSqlMapDao
    /// </summary>
    public class GoodsSetMealSqlMapDao : BaseSqlMapDao, IGoodsSetMealDao
    {
        public IList<GoodsSetMeal> GetAllGoodsSetMeal()
        {
            return ExecuteQueryForList<GoodsSetMeal>("GetGoodsSetMealList", null);
        }
    }
}
