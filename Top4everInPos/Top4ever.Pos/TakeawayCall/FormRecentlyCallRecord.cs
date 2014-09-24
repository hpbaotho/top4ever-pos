using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Top4ever.ClientService;
using Top4ever.Domain.Customers;

namespace Top4ever.Pos.TakeawayCall
{
    public partial class FormRecentlyCallRecord : Form
    {
        private int curIndex = 0;
        private string telephone = string.Empty;

        public string CurTelephone
        {
            get { return telephone; }
        }

        public FormRecentlyCallRecord()
        {
            InitializeComponent();
        }

        private void FormRecentlyCallRecord_Load(object sender, EventArgs e)
        {
            int status = 0; //未接来电
            IList<CallRecord> callRecordList = CustomersService.GetInstance().GetCallRecordByStatus(status);
            if (callRecordList != null && callRecordList.Count > 0)
            {
                dgvCallRecord.Rows.Clear();
                foreach (CallRecord item in callRecordList)
                {
                    int index = dgvCallRecord.Rows.Add();
                    dgvCallRecord.Rows[index].Cells[0].Value = false;
                    dgvCallRecord.Rows[index].Cells[1].Value = item.Telephone;
                    dgvCallRecord.Rows[index].Cells[2].Value = item.CustomerName;
                    dgvCallRecord.Rows[index].Cells[3].Value = item.CallTime.ToString("yyyy-MM-dd HH:mm");
                    dgvCallRecord.Rows[index].Cells[4].Value = item.CallRecordID;
                }
            }
            this.exTabControl1.Selected += new TabControlEventHandler(this.exTabControl1_Selected);
        }

        private void exTabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex == 0)
            {
                if (curIndex != 0)
                {
                    int status = 0; //未接来电
                    IList<CallRecord> callRecordList = CustomersService.GetInstance().GetCallRecordByStatus(status);
                    if (callRecordList != null && callRecordList.Count > 0)
                    {
                        dgvCallRecord.Rows.Clear();
                        foreach (CallRecord item in callRecordList)
                        {
                            int index = dgvCallRecord.Rows.Add();
                            dgvCallRecord.Rows[index].Cells[0].Value = false;
                            dgvCallRecord.Rows[index].Cells[1].Value = item.Telephone;
                            dgvCallRecord.Rows[index].Cells[2].Value = item.CustomerName;
                            dgvCallRecord.Rows[index].Cells[3].Value = item.CallTime.ToString("yyyy-MM-dd HH:mm");
                            dgvCallRecord.Rows[index].Cells[4].Value = item.CallRecordID;
                        }
                    }
                    curIndex = 0;
                    this.btnIgnore.Visible = true;
                }
            }
            else if (e.TabPageIndex == 1)
            {
                if (curIndex != 1)
                {
                    int status = 1; //已接来电
                    IList<CallRecord> callRecordList = CustomersService.GetInstance().GetCallRecordByStatus(status);
                    if (callRecordList != null && callRecordList.Count > 0)
                    {
                        dgvCallRecord.Rows.Clear();
                        foreach (CallRecord item in callRecordList)
                        {
                            int index = dgvCallRecord.Rows.Add();
                            dgvCallRecord.Rows[index].Cells[0].Value = item.Telephone;
                            dgvCallRecord.Rows[index].Cells[1].Value = item.CustomerName;
                            dgvCallRecord.Rows[index].Cells[2].Value = item.CallTime.ToString("yyyy-MM-dd HH:mm");
                            dgvCallRecord.Rows[index].Cells[3].Value = item.CallRecordID;
                        }
                    }
                    curIndex = 1;
                    this.btnIgnore.Visible = false;
                }
            }
            else if (e.TabPageIndex == 2)
            {
                if (curIndex != 2)
                {
                    int status = 2; //忽略来电
                    IList<CallRecord> callRecordList = CustomersService.GetInstance().GetCallRecordByStatus(status);
                    if (callRecordList != null && callRecordList.Count > 0)
                    {
                        dgvCallRecord.Rows.Clear();
                        foreach (CallRecord item in callRecordList)
                        {
                            int index = dgvCallRecord.Rows.Add();
                            dgvCallRecord.Rows[index].Cells[0].Value = item.Telephone;
                            dgvCallRecord.Rows[index].Cells[1].Value = item.CustomerName;
                            dgvCallRecord.Rows[index].Cells[2].Value = item.CallTime.ToString("yyyy-MM-dd HH:mm");
                            dgvCallRecord.Rows[index].Cells[3].Value = item.CallRecordID;
                        }
                    }
                    curIndex = 2;
                    this.btnIgnore.Visible = false;
                }
            }
            else if (e.TabPageIndex == 3)
            {
                if (curIndex != 3)
                {
                    int status = -1; //全部通话
                    IList<CallRecord> callRecordList = CustomersService.GetInstance().GetCallRecordByStatus(status);
                    if (callRecordList != null && callRecordList.Count > 0)
                    {
                        dgvCallRecord.Rows.Clear();
                        foreach (CallRecord item in callRecordList)
                        {
                            int index = dgvCallRecord.Rows.Add();
                            dgvCallRecord.Rows[index].Cells[0].Value = item.Telephone;
                            dgvCallRecord.Rows[index].Cells[1].Value = item.CustomerName;
                            dgvCallRecord.Rows[index].Cells[2].Value = item.CallTime.ToString("yyyy-MM-dd HH:mm");
                            dgvCallRecord.Rows[index].Cells[3].Value = item.CallRecordID;
                        }
                    }
                    curIndex = 3;
                    this.btnIgnore.Visible = false;
                }
            }
        }

        private void btnIgnore_Click(object sender, EventArgs e)
        {
            int status = 2; //忽略
            bool hasChange = false;
            foreach (DataGridViewRow dgvRow in dgvCallRecord.Rows)
            {
                if ((bool)dgvRow.Cells[0].Value)
                {
                    CallRecord callRecord = new CallRecord();
                    callRecord.CallRecordID = new Guid(dgvRow.Cells[4].Value.ToString());
                    callRecord.Status = status;
                    hasChange = CustomersService.GetInstance().CreateOrUpdateCallRecord(callRecord);
                }
            }
            if (hasChange)
            {
                status = 0; //未接来电
                IList<CallRecord> callRecordList = CustomersService.GetInstance().GetCallRecordByStatus(status);
                if (callRecordList != null && callRecordList.Count > 0)
                {
                    dgvCallRecord.Rows.Clear();
                    foreach (CallRecord item in callRecordList)
                    {
                        int index = dgvCallRecord.Rows.Add();
                        dgvCallRecord.Rows[index].Cells[0].Value = false;
                        dgvCallRecord.Rows[index].Cells[1].Value = item.Telephone;
                        dgvCallRecord.Rows[index].Cells[2].Value = item.CustomerName;
                        dgvCallRecord.Rows[index].Cells[3].Value = item.CallTime.ToString("yyyy-MM-dd HH:mm");
                        dgvCallRecord.Rows[index].Cells[4].Value = item.CallRecordID;
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            telephone = string.Empty;
            this.Close();
        }

        private void dgvCallRecord_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCallRecord.CurrentRow != null)
            {
                int selectIndex = dgvCallRecord.CurrentRow.Index;
                telephone = dgvCallRecord.Rows[selectIndex].Cells[1].Value.ToString();
                const int status = 1; //已接来电
                CallRecord callRecord = new CallRecord
                {
                    CallRecordID = new Guid(dgvCallRecord.Rows[selectIndex].Cells[4].Value.ToString()), 
                    Status = status
                };
                CustomersService.GetInstance().CreateOrUpdateCallRecord(callRecord);
                this.Close();
            }
        }
    }
}
