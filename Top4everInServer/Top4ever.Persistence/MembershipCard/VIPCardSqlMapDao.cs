using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain.MembershipCard;
using Top4ever.Interface.MembershipCard;

namespace Top4ever.Persistence.MembershipCard
{
    /// <summary>
    /// Summary description for VIPCardSqlMapDao
    /// </summary>
    public class VIPCardSqlMapDao : BaseSqlMapDao, IVIPCardDao
    {
        #region IVIPCardDao Members

        public VIPCard GetVIPCard(string cardNo, string password)
        {
            VIPCard card = new VIPCard();
            card.CardNo = cardNo;
            card.Password = password;
            return ExecuteQueryForObject("GetVIPCardInfo", card) as VIPCard;
        }

        public string GetCardPassword(string cardNo)
        {
            string password = string.Empty;
            object objValue = ExecuteQueryForObject("GetCardPassword", cardNo);
            if (objValue != null)
            {
                password = Convert.ToString(objValue);
            }
            return password;
        }

        public bool UpdateCardPassword(string cardNo, string currentPassword, string newPassword)
        {
            int result = 0;
            Hashtable htParam = new Hashtable();
            htParam["CardNo"] = cardNo;
            htParam["CurrentPassword"] = currentPassword;
            htParam["NewPassword"] = newPassword;
            result = ExecuteUpdate("UpdateCardPassword", htParam);
            return result > 0;
        }

        #endregion
    }
}
