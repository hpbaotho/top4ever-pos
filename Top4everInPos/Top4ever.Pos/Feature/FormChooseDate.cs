using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;

namespace Top4ever.Pos.Feature
{
    public partial class FormChooseDate : Form
    {
        private DateTime? m_LastDailyStatementTime;
        private DateTime? m_BelongToDate;

        public DateTime? DailyStatementDate
        {
            get { return m_BelongToDate; } 
        }

        public FormChooseDate(DateTime? lastDailyStatementTime)
        {
            m_LastDailyStatementTime = lastDailyStatementTime;
            InitializeComponent();
        }

        private void FormChooseDate_Load(object sender, EventArgs e)
        {
            if (m_LastDailyStatementTime != null)
            {
                DateTime lastDailyStatementTime = (DateTime)m_LastDailyStatementTime;
                this.txtLastTime.Text = lastDailyStatementTime.ToLongDateString() + " " + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(lastDailyStatementTime.DayOfWeek);
            }
            string dailyTimeInterval = DailyBalanceService.GetInstance().GetDailyStatementTimeInterval();
            if (string.IsNullOrEmpty(dailyTimeInterval))
            {
                MessageBox.Show("未获取到销售数据，请确认后操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string[] timeArr = dailyTimeInterval.Split(',');
                DateTime beginTime = DateTime.Parse(timeArr[0]);
                DateTime endTime = DateTime.Parse(timeArr[1] + " 23:59:59");
                if (endTime < m_LastDailyStatementTime)
                {
                    //Fix 销售时间错误
                    beginTime = (DateTime)m_LastDailyStatementTime;
                    endTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
                }
                DateTime tempDate = beginTime;
                while (tempDate < endTime)
                {
                    comboBox1.Items.Add(tempDate.ToLongDateString() + " " + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(tempDate.DayOfWeek));
                    tempDate = tempDate.AddDays(1);
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string dailyStatementDate = comboBox1.Text;
            string[] dateArr = dailyStatementDate.Split(' ');
            if (dateArr.Length > 0)
            {
                m_BelongToDate = DateTime.Parse(dateArr[0]);
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
