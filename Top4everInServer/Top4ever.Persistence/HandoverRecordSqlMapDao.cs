using System;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Interface;

namespace Top4ever.Persistence
{
    /// <summary>
    /// Summary description for HandoverRecordSqlMapDao
    /// </summary>
    public class HandoverRecordSqlMapDao : BaseSqlMapDao, IHandoverRecordDao
    {
        #region IHandoverRecord Members

        public void CreateHandoverRecord(HandoverRecord handoverRecord)
        {
            ExecuteInsert("InsertHandoverRecord", handoverRecord);
        }

        #endregion
    }
}
