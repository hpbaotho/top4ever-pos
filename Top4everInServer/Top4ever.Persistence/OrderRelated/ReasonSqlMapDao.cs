using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain.OrderRelated;
using Top4ever.Interface.OrderRelated;

namespace Top4ever.Persistence.OrderRelated
{
    /// <summary>
    /// Summary description for ReasonSqlMapDao
    /// </summary>
    public class ReasonSqlMapDao : BaseSqlMapDao, IReasonDao
    {
        #region IReasonDao Members

        public IList<Reason> GetAllReason()
        {
            return ExecuteQueryForList<Reason>("GetReasonList", null);
        }

        #endregion
    }
}
