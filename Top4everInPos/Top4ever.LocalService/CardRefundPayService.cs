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
                                                                TradePayNo varchar(30), 
                                                                PayAmount decimal(12, 4), EmployeeNo varchar(20), 
                                                                DeviceNo varchar(16), IsFixed Integer,
                                                                CreateTime datetime, LastTime datetime);";
                m_SQLiteHelper.ExecuteSql(createTableSql);
            }
        }

        public bool AddRefundPayInfo(CardRefundPay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into CardRefundPay(");
            strSql.Append("CardNo,ShopID,TradePayNo,PayAmount,EmployeeNo,DeviceNo,IsFixed,CreateTime,LastTime)");
            strSql.Append(" values (");
            strSql.Append("@CardNo,@ShopID,@TradePayNo,@PayAmount,@EmployeeNo,@DeviceNo,@IsFixed,@CreateTime,@LastTime)");
            SQLiteParameter[] parameters = {
                    new SQLiteParameter("@CardNo", DbType.String),
                    new SQLiteParameter("@ShopID", DbType.String),
                    new SQLiteParameter("@TradePayNo", DbType.String),
                    new SQLiteParameter("@PayAmount", DbType.Decimal),
                    new SQLiteParameter("@EmployeeNo", DbType.String),
                    new SQLiteParameter("@DeviceNo", DbType.String),
                    new SQLiteParameter("@IsFixed", DbType.Boolean),
                    new SQLiteParameter("@CreateTime", DbType.DateTime),
                    new SQLiteParameter("@LastTime", DbType.DateTime)};
            parameters[0].Value = model.CardNo;
            parameters[1].Value = model.ShopID;
            parameters[2].Value = model.TradePayNo;
            parameters[3].Value = model.PayAmount;
            parameters[4].Value = model.EmployeeNo;
            parameters[5].Value = model.DeviceNo;
            parameters[6].Value = false;
            parameters[7].Value = DateTime.Now;
            parameters[8].Value = DateTime.Now;

            return m_SQLiteHelper.ExecuteSql(strSql.ToString(), parameters) > 0;
        }

        public List<CardRefundPay> GetCardRefundPayList()
        {
            List<CardRefundPay> refundPayList = new List<CardRefundPay>();
            string strSql = "SELECT StoreValueID,CardNo,ShopID,TradePayNo,PayAmount,EmployeeNo,DeviceNo,IsFixed,CreateTime,LastTime FROM CardRefundPay WHERE IsFixed = @IsFixed ";
            SQLiteParameter[] parameters = {
                    new SQLiteParameter("@IsFixed", DbType.Boolean)};
            parameters[0].Value = false;
            DataTable dt = m_SQLiteHelper.Query(strSql, parameters);
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

        public bool UpdateFixedPayInfo(int storeValueId, bool IsFixed)
        {
            string strSql = "UPDATE CardRefundPay SET IsFixed = @IsFixed WHERE StoreValueID = @StoreValueID ";
            SQLiteParameter[] parameters = {
                    new SQLiteParameter("@IsFixed", DbType.Boolean),
                    new SQLiteParameter("@StoreValueID", DbType.Int32)};
            parameters[0].Value = IsFixed;
            parameters[1].Value = storeValueId;
            return m_SQLiteHelper.ExecuteSql(strSql, parameters) > 0;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        private CardRefundPay DataRowToModel(DataRow row)
        {
            CardRefundPay model = new CardRefundPay();
            if (row != null)
            {
                if (row["StoreValueID"] != DBNull.Value && row["StoreValueID"].ToString() != "")
                {
                    model.StoreValueID = int.Parse(row["StoreValueID"].ToString());
                }
                if (row["CardNo"] != DBNull.Value)
                {
                    model.CardNo = row["CardNo"].ToString();
                }
                if (row["ShopID"] != DBNull.Value)
                {
                    model.ShopID = row["ShopID"].ToString();
                }
                if (row["TradePayNo"] != DBNull.Value)
                {
                    model.TradePayNo = row["TradePayNo"].ToString();
                }
                if (row["PayAmount"] != DBNull.Value && row["PayAmount"].ToString() != "")
                {
                    model.PayAmount = decimal.Parse(row["PayAmount"].ToString());
                }
                if (row["EmployeeNo"] != DBNull.Value)
                {
                    model.EmployeeNo = row["EmployeeNo"].ToString();
                }
                if (row["DeviceNo"] != DBNull.Value)
                {
                    model.DeviceNo = row["DeviceNo"].ToString();
                }
                if (row["IsFixed"] != DBNull.Value)
                {
                    model.IsFixed = Convert.ToBoolean(row["IsFixed"]);
                }
                if (row["CreateTime"] != DBNull.Value && row["CreateTime"].ToString() != "")
                {
                    model.CreateTime = Convert.ToDateTime(row["CreateTime"]);
                }
                if (row["LastTime"] != DBNull.Value && row["LastTime"].ToString() != "")
                {
                    model.LastTime = Convert.ToDateTime(row["LastTime"]);
                }
            }
            return model;
        }
    }
}
