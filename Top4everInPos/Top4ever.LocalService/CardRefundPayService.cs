using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SQLite;
using Top4ever.LocalService.Entity;

namespace Top4ever.LocalService
{
    public class CardRefundPayService
    {
        private readonly SQLiteHelper m_SQLiteHelper;

        public CardRefundPayService()
        {
            string localDriver = "C";
            if (Directory.Exists("D:\\"))
            {
                localDriver = "D";
            }
            string dbPath = localDriver + ":\\DataBase\\top4pos.db3";
            if (!File.Exists(dbPath))
            {
                //创建数据库
                SQLiteConnection.CreateFile(dbPath);
            }
            SQLiteConnectionStringBuilder connectionString = new SQLiteConnectionStringBuilder
                                                                {
                                                                    DataSource = dbPath,
                                                                    Password = "123",
                                                                    Pooling = true,
                                                                    FailIfMissing = false
                                                                };
            m_SQLiteHelper = new SQLiteHelper(connectionString.ToString());
            //判断表是否存在
            string strSql = "SELECT COUNT(*) FROM sqlite_master where type='table' and name='CardRefundPay'";
            if (!m_SQLiteHelper.Exists(strSql))
            {
                string createTableSql = @"create table CardRefundPay(StoreValueID INTEGER primary key autoincrement, 
                                                                CardNo varchar(50), ShopID varchar(36), 
                                                                TradePayNo varchar(30), DailyStatementNo varchar(20), 
                                                                PayAmount decimal(12, 4), EmployeeNo varchar(20), 
                                                                DeviceNo varchar(16), CreateDate datetime);";
                m_SQLiteHelper.ExecuteSql(createTableSql);
            }
        }

        public bool AddRefundPayInfo(CardRefundPay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into CardRefundPay(");
            strSql.Append("CardNo,ShopID,TradePayNo,DailyStatementNo,PayAmount,EmployeeNo,DeviceNo,CreateDate)");
            strSql.Append(" values (");
            strSql.Append("@CardNo,@ShopID,@TradePayNo,@DailyStatementNo,@PayAmount,@EmployeeNo,@DeviceNo,@CreateDate)");
            SQLiteParameter[] parameters = {
                    new SQLiteParameter("@CardNo", DbType.String),
                    new SQLiteParameter("@ShopID", DbType.String),
                    new SQLiteParameter("@TradePayNo", DbType.String),
                    new SQLiteParameter("@DailyStatementNo", DbType.String),
                    new SQLiteParameter("@PayAmount", DbType.Decimal),
                    new SQLiteParameter("@EmployeeNo", DbType.String),
                    new SQLiteParameter("@DeviceNo", DbType.String),
                    new SQLiteParameter("@CreateDate", DbType.DateTime)};
            parameters[0].Value = model.CardNo;
            parameters[1].Value = model.ShopID;
            parameters[2].Value = model.TradePayNo;
            parameters[3].Value = model.DailyStatementNo;
            parameters[4].Value = model.PayAmount;
            parameters[5].Value = model.EmployeeNo;
            parameters[6].Value = model.DeviceNo;
            parameters[7].Value = model.CreateDate;

            return m_SQLiteHelper.ExecuteSql(strSql.ToString(), parameters) > 0;
        }

        public List<CardRefundPay> GetCardRefundPayList()
        {
            List<CardRefundPay> refundPayList = new List<CardRefundPay>();
            string strSql = "select StoreValueID,CardNo,ShopID,TradePayNo,DailyStatementNo,PayAmount,EmployeeNo,DeviceNo,CreateDate from CardRefundPay ";
            DataTable dt = m_SQLiteHelper.Query(strSql);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CardRefundPay refundPay = DataRowToModel(dr);
                    refundPayList.Add(refundPay);
                }
            }
            return refundPayList;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public CardRefundPay DataRowToModel(DataRow row)
        {
            CardRefundPay model = new CardRefundPay();
            if (row != null)
            {
                if (row["StoreValueID"] != null && row["StoreValueID"].ToString() != "")
                {
                    model.StoreValueID = int.Parse(row["StoreValueID"].ToString());
                }
                if (row["CardNo"] != null)
                {
                    model.CardNo = row["CardNo"].ToString();
                }
                if (row["ShopID"] != null)
                {
                    model.ShopID = row["ShopID"].ToString();
                }
                if (row["TradePayNo"] != null)
                {
                    model.TradePayNo = row["TradePayNo"].ToString();
                }
                if (row["DailyStatementNo"] != null)
                {
                    model.DailyStatementNo = row["DailyStatementNo"].ToString();
                }
                if (row["PayAmount"] != null && row["PayAmount"].ToString() != "")
                {
                    model.PayAmount = decimal.Parse(row["PayAmount"].ToString());
                }
                if (row["EmployeeNo"] != null)
                {
                    model.EmployeeNo = row["EmployeeNo"].ToString();
                }
                if (row["DeviceNo"] != null)
                {
                    model.DeviceNo = row["DeviceNo"].ToString();
                }
                if (row["CreateDate"] != null && row["CreateDate"].ToString() != "")
                {
                    model.CreateDate = DateTime.Parse(row["CreateDate"].ToString());
                }
            }
            return model;
        }
    }
}
