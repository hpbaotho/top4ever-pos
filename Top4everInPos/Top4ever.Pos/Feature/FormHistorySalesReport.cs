using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Top4ever.ClientService;
using Top4ever.Domain;
using Top4ever.Domain.Transfer;

namespace VechsoftPos.Feature
{
    public partial class FormHistorySalesReport : Form
    {
        private IList<EmployeeHandoverRecord> m_HandoverRecordList = null;

        public FormHistorySalesReport()
        {
            InitializeComponent();
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            dgvDailyStatement.Rows.Clear();
            DateTime selectedDate = e.Start;
            IList<DailyBalanceTime> dailyBalanceTimeList = DailyBalanceService.GetInstance().GetDailyBalanceTime(selectedDate);
            if (dailyBalanceTimeList != null && dailyBalanceTimeList.Count > 0)
            {
                for (int i = 0; i < dailyBalanceTimeList.Count; i++)
                {
                    int index = dgvDailyStatement.Rows.Add(new DataGridViewRow());
                    dgvDailyStatement.Rows[index].Cells["colOrder"].Value = i + 1;
                    dgvDailyStatement.Rows[index].Cells["colBeginTime"].Value = dailyBalanceTimeList[i].MinTime > DateTime.MinValue ? dailyBalanceTimeList[i].MinTime.ToString("yyyy-MM-dd") : "";
                    dgvDailyStatement.Rows[index].Cells["colEndTime"].Value = dailyBalanceTimeList[i].MaxTime > DateTime.MinValue ? dailyBalanceTimeList[i].MaxTime.ToString("yyyy-MM-dd") : "";
                    dgvDailyStatement.Rows[index].Cells["colDailyStatementNo"].Value = dailyBalanceTimeList[i].DailyStatementNo;
                    dgvDailyStatement.Rows[index].Selected = false;
                }
            }
        }

        private void dgvDailyStatement_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDailyStatement.CurrentRow != null)
            {
                int index = dgvDailyStatement.CurrentRow.Index;
                dgvDeviceNo.Rows.Clear();
                string dailyStatementNo = dgvDailyStatement.Rows[index].Cells["colDailyStatementNo"].Value.ToString();
                m_HandoverRecordList = HandoverService.GetInstance().GetHandoverRecord(dailyStatementNo);
                if (m_HandoverRecordList != null && m_HandoverRecordList.Count > 0)
                {
                    List<string> deviceNoList = m_HandoverRecordList.Select(p => p.DeviceNo).Distinct().ToList();
                    for (int i = 0; i < deviceNoList.Count; i++)
                    {
                        int m = dgvDeviceNo.Rows.Add(new DataGridViewRow());
                        dgvDeviceNo.Rows[m].Cells["colIndex"].Value = i + 1;
                        dgvDeviceNo.Rows[m].Cells["colDeviceNo"].Value = deviceNoList[i];
                        dgvDeviceNo.Rows[m].Selected = false;
                    }
                }
            }
        }

        private void dgvDeviceNo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDeviceNo.CurrentRow != null)
            {
                int index = dgvDeviceNo.CurrentRow.Index;
                dgvHandover.Rows.Clear();
                string deviceNo = dgvDeviceNo.Rows[index].Cells["colDeviceNo"].Value.ToString();
                if (m_HandoverRecordList != null && m_HandoverRecordList.Count > 0)
                {
                    List<EmployeeHandoverRecord> handoverRecordList = m_HandoverRecordList.Where(p => p.DeviceNo == deviceNo).ToList();
                    for (int i = 0; i < handoverRecordList.Count; i++)
                    {
                        int m = dgvHandover.Rows.Add(new DataGridViewRow());
                        dgvHandover.Rows[m].Cells["colOrderIndex"].Value = i + 1;
                        dgvHandover.Rows[m].Cells["colWorkSequence"].Value = handoverRecordList[i].WorkSequence;
                        dgvHandover.Rows[m].Cells["colEmployeeNo"].Value = handoverRecordList[i].EmployeeNo;
                        dgvHandover.Rows[m].Cells["colHandoverTime"].Value = handoverRecordList[i].HandoverTime;
                        dgvHandover.Rows[m].Cells["colHandoverRecordID"].Value = handoverRecordList[i].HandoverRecordID;
                        dgvHandover.Rows[m].Selected = false;
                    }
                }
            }
        }

        private void btnShowDailyStatement_Click(object sender, EventArgs e)
        {
            if (dgvHandover.CurrentRow != null)
            {
                //查看交班记录
                int index = dgvHandover.CurrentRow.Index;
                int modelType = 1;
                object handoverRecordID = dgvHandover.Rows[index].Cells["colHandoverRecordID"].Value;
                FormSalesReport formReport = new FormSalesReport(modelType, handoverRecordID);
                formReport.ShowDialog();
            }
            else
            {
                if (dgvDailyStatement.CurrentRow != null)
                {
                    //查看日结记录
                    int index = dgvDailyStatement.CurrentRow.Index;
                    int modelType = 2;
                    object dailyStatementNo = dgvDailyStatement.Rows[index].Cells["colDailyStatementNo"].Value;
                    FormSalesReport formReport = new FormSalesReport(modelType, dailyStatementNo);
                    formReport.ShowDialog();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
