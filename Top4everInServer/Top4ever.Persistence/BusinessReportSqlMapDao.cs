using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;

namespace Top4ever.Persistence
{
    /// <summary>
    /// Summary description for BusinessReportSqlMapDao
    /// </summary>
    public class BusinessReportSqlMapDao : BaseSqlMapDao, IBusinessReportDao
    {
        #region IBusinessReportDao Members

        /// <summary>
        /// 获取营业额统计
        /// </summary>
        /// <param name="dailyStatementNo">日结号</param>
        /// <param name="deviceNo">设备号</param>
        /// <returns></returns>
        public BusinessReport GetTurnoverByHandover(string dailyStatementNo, string deviceNo)
        {
            Hashtable htParam = new Hashtable();
            htParam["DailyStatementNo"] = dailyStatementNo;
            htParam["DeviceNo"] = deviceNo;

            return ExecuteQueryForObject("GetTurnoverByHandover", htParam) as BusinessReport;
        }

        /// <summary>
        /// 通过交班Id获取营业额统计
        /// </summary>
        /// <param name="handoverRecordID">交班Id</param>
        /// <returns></returns>
        public BusinessReport GetTurnoverByHandoverRecordID(Guid handoverRecordID)
        {
            return ExecuteQueryForObject("GetTurnoverByHandoverRecordID", handoverRecordID) as BusinessReport;
        }

        public BusinessReport GetTurnoverByDailyStatement(string dailyStatementNo)
        {
            Hashtable htParam = new Hashtable();
            htParam["DailyStatementNo"] = dailyStatementNo;

            return ExecuteQueryForObject("GetTurnoverByDailyStatement", htParam) as BusinessReport;
        }

        public IList<OrderDiscountSum> GetOrderDiscountSumByHandover(string dailyStatementNo, int workSequence)
        {
            HandoverRecord handoverRecord = new HandoverRecord();
            handoverRecord.DailyStatementNo = dailyStatementNo;
            handoverRecord.WorkSequence = workSequence;

            return ExecuteQueryForList<OrderDiscountSum>("GetDiscountSumByHandover", handoverRecord);
        }

        public IList<OrderDiscountSum> GetOrderDiscountSumByDailyStatement(string dailyStatementNo)
        {
            return ExecuteQueryForList<OrderDiscountSum>("GetDiscountSumByDailyStatement", dailyStatementNo);
        }

        public IList<OrderPayoffSum> GetOrderPayoffSumByHandover(string dailyStatementNo, int workSequence)
        {
            HandoverRecord handoverRecord = new HandoverRecord();
            handoverRecord.DailyStatementNo = dailyStatementNo;
            handoverRecord.WorkSequence = workSequence;

            return ExecuteQueryForList<OrderPayoffSum>("GetPayoffSumByHandover", handoverRecord);
        }

        public IList<OrderPayoffSum> GetOrderPayoffSumByDailyStatement(string dailyStatementNo)
        {
            return ExecuteQueryForList<OrderPayoffSum>("GetPayoffSumByDailyStatement", dailyStatementNo);
        }

        public IList<ItemsPrice> GetItemsPriceByHandover(string dailyStatementNo, int workSequence)
        {
            HandoverRecord handoverRecord = new HandoverRecord();
            handoverRecord.DailyStatementNo = dailyStatementNo;
            handoverRecord.WorkSequence = workSequence;

            return ExecuteQueryForList<ItemsPrice>("GetItemsPriceByHandover", handoverRecord);
        }

        public IList<ItemsPrice> GetItemsPriceByDailyStatement(string dailyStatementNo)
        {
            return ExecuteQueryForList<ItemsPrice>("GetItemsPriceByDailyStatement", dailyStatementNo);
        }

        public IList<GroupPrice> GetItemsPriceByGroup(DateTime beginDate, DateTime endDate)
        {
            Hashtable htParam = new Hashtable();
            htParam["BeginDate"] = beginDate;
            htParam["EndDate"] = endDate;
            return ExecuteQueryForList<GroupPrice>("GetItemsPriceByGroup", htParam);
        }

        #endregion
    }
}
