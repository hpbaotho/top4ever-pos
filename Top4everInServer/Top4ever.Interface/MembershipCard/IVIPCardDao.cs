using System;
using System.Collections.Generic;

using Top4ever.Domain.MembershipCard;

namespace Top4ever.Interface.MembershipCard
{
    /// <summary>
    /// Summary description for IVIPCardDao.
    /// </summary>
    public interface IVIPCardDao
    {
        VIPCard GetVIPCard(string cardNo, string password);

        string GetCardPassword(string cardNo);

        bool UpdateCardPassword(string cardNo, string currentPassword, string newPassword);
    }
}
