using System;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Domain.Transfer;

namespace Top4ever.Interface
{
    /// <summary>
    /// Summary description for IHandoverRecord.
    /// </summary>
    public interface IHandoverRecordDao
    {
        void CreateHandoverRecord(HandoverRecord handoverRecord);

        /// <summary>
        /// 获取交班记录
        /// </summary>
        /// <param name="dailyStatementNo">日结号</param>
        /// <returns></returns>
        IList<EmployeeHandoverRecord> GetHandoverRecord(string dailyStatementNo);
    }
}
