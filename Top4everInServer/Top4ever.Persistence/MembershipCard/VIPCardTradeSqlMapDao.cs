using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain.MembershipCard;
using Top4ever.Interface.MembershipCard;

namespace Top4ever.Persistence.MembershipCard
{
    /// <summary>
    /// Summary description for VIPCardTradeSqlMapDao
    /// </summary>
    public class VIPCardTradeSqlMapDao : BaseSqlMapDao, IVIPCardTradeDao
    {
        public IList<VIPCardTrade> GetVIPCardTradeList(string cardNo, DateTime beginDate, DateTime endDate)
        {
            Hashtable htParam = new Hashtable();
            htParam["CardNo"] = cardNo;
            htParam["BeginDate"] = beginDate;
            htParam["EndDate"] = endDate;
            return ExecuteQueryForList<VIPCardTrade>("GetVIPCardTradeList", htParam);
        }

        public VIPCardStoredVaule GetVIPCardStoredVaule(string cardNo, decimal storedVauleAmount)
        {
            Hashtable htParam = new Hashtable();
            htParam["CardNo"] = cardNo;
            htParam["StoredVauleAmount"] = storedVauleAmount;
            return ExecuteQueryForObject<VIPCardStoredVaule>("GetVIPCardStoredVaule", htParam);
        }

        public Int32 AddVIPCardStoredValue(string cardNo, decimal storeMoney, decimal giftAmount, int giftIntegral, string employeeNo, string deviceNo, string dailyStatementNo, Guid payoffID, string payoffName, out string tradePayNo)
        {
            int returnValue = 0;
            Hashtable htParam = new Hashtable();
            htParam["CardNo"] = cardNo;
            htParam["StoreMoney"] = storeMoney;
            htParam["GiftAmount"] = giftAmount;
            htParam["GiftIntegral"] = giftIntegral;
            htParam["EmployeeNo"] = employeeNo;
            htParam["DeviceNo"] = deviceNo;
            htParam["DailyStatementNo"] = dailyStatementNo;
            htParam["PayoffID"] = payoffID;
            htParam["PayoffName"] = payoffName;
            htParam["ReturnValue"] = 0;

            object result = ExecuteInsert("AddVIPCardStoredValue", htParam);
            returnValue = (int)htParam["ReturnValue"];    //返回值
            if (returnValue == 1)
            {
                tradePayNo = result.ToString();
            }
            else
            {
                tradePayNo = string.Empty;
            }
            return returnValue;
        }

        public Int32 AddVIPCardPayment(string cardNo, decimal payAmount, int payIntegral, string orderNo, string employeeNo, string deviceNo, string dailyStatementNo, out string tradePayNo)
        {
            int returnValue = 0;
            Hashtable htParam = new Hashtable();
            htParam["CardNo"] = cardNo;
            htParam["PayAmount"] = payAmount;
            htParam["PayIntegral"] = payIntegral;
            htParam["OrderNo"] = orderNo;
            htParam["EmployeeNo"] = employeeNo;
            htParam["DeviceNo"] = deviceNo;
            htParam["DailyStatementNo"] = dailyStatementNo;
            htParam["ReturnValue"] = 0;

            object result = ExecuteInsert("AddVIPCardPayment", htParam);
            returnValue = (int)htParam["ReturnValue"];    //返回值
            if (returnValue == 1)
            {
                tradePayNo = result.ToString();
            }
            else
            {
                tradePayNo = string.Empty;
            }
            return returnValue;
        }

        public Int32 RefundVIPCardPayment(string cardNo, string tradePayNo)
        {
            Hashtable htParam = new Hashtable();
            htParam["CardNo"] = cardNo;
            htParam["TradePayNo"] = tradePayNo;
            htParam["ReturnValue"] = 0;

            ExecuteUpdate("RefundVIPCardPayment", htParam);
            return (int)htParam["ReturnValue"];
        }
    }
}
