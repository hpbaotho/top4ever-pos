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

        public decimal GetCardDiscountRate(string cardNo)
        {
            decimal discountRate = 0M;
            object objValue = ExecuteQueryForObject("GetCardDiscountRate", cardNo);
            if (objValue != null)
            {
                discountRate = Convert.ToDecimal(objValue);
            }
            return discountRate;
        }

        public bool UpdateVIPCardPassword(string cardNo, string password, string newPassword)
        {
            int result = 0;
            Hashtable htParam = new Hashtable();
            htParam["CardNo"] = cardNo;
            htParam["Password"] = password;
            htParam["NewPassword"] = newPassword;
            result = ExecuteUpdate("UpdateVIPCardPassword", htParam);
            return result > 0;
        }

        public Int32 UpdateVIPCardStatus(string cardNo, string password, int status)
        {
            Hashtable htParam = new Hashtable();
            htParam["CardNo"] = cardNo;
            htParam["Password"] = password;
            htParam["Status"] = status;
            htParam["ReturnValue"] = 0;
            ExecuteQueryForObject("UpdateVIPCardStatus", htParam);
            int i = (int)htParam["ReturnValue"];    //返回值
            return i;
        }
        #endregion
    }
}
