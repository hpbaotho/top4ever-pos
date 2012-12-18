using System;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Interface;

namespace Top4ever.Persistence
{
    /// <summary>
    /// Summary description for HandoverTurnoverSqlMapDao
    /// </summary>
    public class HandoverTurnoverSqlMapDao : BaseSqlMapDao, IHandoverTurnoverDao
    {
        #region IHandoverTurnover Members

        public void CreateHandoverTurnover(HandoverTurnover handoverTurnover)
        {
            ExecuteInsert("InsertHandoverTurnover", handoverTurnover);
        }

        #endregion
    }
}
