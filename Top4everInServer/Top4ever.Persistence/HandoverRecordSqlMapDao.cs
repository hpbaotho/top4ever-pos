using System;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Domain.Transfer;
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

        /// <summary>
        /// 获取交班记录
        /// </summary>
        /// <param name="dailyStatementNo">日结号</param>
        /// <returns></returns>
        public IList<EmployeeHandoverRecord> GetHandoverRecord(string dailyStatementNo)
        {
            return ExecuteQueryForList<EmployeeHandoverRecord>("SelectHandoverRecord", dailyStatementNo);
        }

        #endregion
    }
}
