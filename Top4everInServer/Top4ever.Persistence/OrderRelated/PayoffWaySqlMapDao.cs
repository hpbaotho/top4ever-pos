using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain.OrderRelated;
using Top4ever.Interface.OrderRelated;

namespace Top4ever.Persistence.OrderRelated
{
    /// <summary>
    /// Summary description for PayoffWaySqlMapDao
    /// </summary>
    public class PayoffWaySqlMapDao : BaseSqlMapDao, IPayoffWayDao
    {
        #region IPayoffWayDao Members

        public IList<PayoffWay> GetAllPayoffWay()
        {
            return ExecuteQueryForList<PayoffWay>("GetPayoffWayList", null);
        }

        #endregion
    }
}
