using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain.OrderRelated;
using Top4ever.Interface.OrderRelated;

namespace Top4ever.Persistence.OrderRelated
{
    /// <summary>
    /// Summary description for DiscountSqlMapDao
    /// </summary>
    public class DiscountSqlMapDao : BaseSqlMapDao, IDiscountDao
    {
        #region IDiscountDao Members

        public IList<Discount> GetAllDiscount()
        {
            return ExecuteQueryForList<Discount>("GetDiscountList", null);
        }

        #endregion
    }
}
