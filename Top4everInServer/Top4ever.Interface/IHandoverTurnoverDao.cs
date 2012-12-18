using System;

using Top4ever.Domain;

namespace Top4ever.Interface
{
    /// <summary>
    /// Summary description for IHandoverTurnover.
    /// </summary>
    public interface IHandoverTurnoverDao
    {
        void CreateHandoverTurnover(HandoverTurnover handoverTurnover);
    }
}
