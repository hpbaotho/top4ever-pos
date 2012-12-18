using System;

using Top4ever.Domain;

namespace Top4ever.Interface
{
    /// <summary>
    /// Summary description for IHandoverRecord.
    /// </summary>
    public interface IHandoverRecordDao
    {
        void CreateHandoverRecord(HandoverRecord handoverRecord);
    }
}
