using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Domain.OrderRelated;
using Top4ever.Entity;

namespace Top4ever.Pos.Feature
{
    public partial class FormBillManagement : Form
    {
        /// <summary>
        /// 1:输入台号 2:输入流水号
        /// </summary>
        private int m_SeachType = 0;
        private int m_PageIndex = 0;
        private int m_PageSize = 25;

        public FormBillManagement()
        {
            InitializeComponent();
            this.btnPageUp.DisplayColor = this.btnPageUp.BackColor;
            this.btnPageDown.DisplayColor = this.btnPageDown.BackColor;
        }

        private void btnDeskNo_Click(object sender, EventArgs e)
        {
            FormNumericKeypad keyForm = new FormNumericKeypad(false);
            keyForm.DisplayText = "请输入台号";
            keyForm.ShowDialog();
            if (!string.IsNullOrEmpty(keyForm.KeypadValue))
            {
                m_SeachType = 1;
                this.txtSearchValue.Text = keyForm.KeypadValue;
            }
        }

        private void btnTranSequence_Click(object sender, EventArgs e)
        {
            FormNumericKeypad keyForm = new FormNumericKeypad(false);
            keyForm.DisplayText = "请输入流水号";
            keyForm.ShowDialog();
            if (!string.IsNullOrEmpty(keyForm.KeypadValue))
            {
                m_SeachType = 2;
                this.txtSearchValue.Text = keyForm.KeypadValue;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            m_PageIndex = 0;
            SearchSalesOrder();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            m_PageIndex = 0;
            m_SeachType = 0;
            this.txtSearchValue.Text = string.Empty;
        }

        private void SearchSalesOrder()
        {
            string strWhere = string.Empty;
            if (m_SeachType == 1)
            {
                strWhere = " AND DeskName = '" + this.txtSearchValue.Text + "'";
            }
            if (m_SeachType == 2)
            {
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                strWhere = " AND TranSequence = " + this.txtSearchValue.Text;
                strWhere += " AND (PayTime >= '" + currentDate + "' AND PayTime < DATEADD(DAY,1,'" + currentDate + "') )";
            }
            OrderService orderService = new OrderService();
            IList<Order> orderList = orderService.GetOrderListBySearch(strWhere, string.Empty, m_PageIndex, m_PageSize);
            if (m_PageIndex > 0)
            {
                this.btnPageUp.Enabled = true;
                this.btnPageUp.BackColor = this.btnPageUp.DisplayColor;
            }
            else
            {
                this.btnPageUp.Enabled = false;
                this.btnPageUp.BackColor = ConstantValuePool.DisabledColor;
            }
            if (orderList.Count < m_PageSize)
            {
                this.btnPageDown.Enabled = false;
                this.btnPageDown.BackColor = ConstantValuePool.DisabledColor;
            }
            else
            {
                this.btnPageDown.Enabled = true;
                this.btnPageDown.BackColor = this.btnPageDown.DisplayColor;
            }
            if (orderList.Count > 0)
            {
                this.lbPage.Text = string.Format("第 {0} 页", m_PageIndex);
                BindDataGridView1(orderList);
            }
            else
            {
                this.dataGridView1.Rows.Clear();
                this.lbOrderNo.Text = string.Empty;
                this.lbBillType.Text = string.Empty;
                this.lbDeskNo.Text = string.Empty;
                this.lbEatType.Text = string.Empty;
                this.lbEmployeeNo.Text = string.Empty;
                this.lbCashier.Text = string.Empty;
                this.lbDelEmployeeNo.Text = string.Empty;
                this.lbPage.Text = "第 1 页";
                this.lbBillIndex.Text = "0/0";
            }
        }

        private void BindDataGridView1(IList<Order> orderList)
        {
            this.dataGridView1.Rows.Clear();
            if (orderList != null && orderList.Count > 0)
            {
                foreach (Order order in orderList)
                {
                    string billType = string.Empty;
                    if (order.Status == 0)
                    {
                        billType = "未结账";
                    }
                    else if (order.Status == 1)
                    {
                        billType = "已结账";
                    }
                    else if (order.Status == 2)
                    {
                        billType = "已删除";
                    }
                    int index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells["TranSequence"].Value = order.TranSequence.ToString();
                    dataGridView1.Rows[index].Cells["BillType"].Value = billType;
                    dataGridView1.Rows[index].Cells["TotalSellPrice"].Value = order.TotalSellPrice.ToString("f2");
                    dataGridView1.Rows[index].Cells["ActualSellPrice"].Value = order.ActualSellPrice.ToString("f2");
                    dataGridView1.Rows[index].Cells["DiscountPrice"].Value = order.DiscountPrice.ToString("f2");
                    dataGridView1.Rows[index].Cells["CutOffPrice"].Value = order.CutOffPrice.ToString("f2");
                    dataGridView1.Rows[index].Cells["ServiceFee"].Value = order.ServiceFee.ToString("f2");
                    dataGridView1.Rows[index].Cells["PaymentMoney"].Value = order.PaymentMoney.ToString("f2");
                    dataGridView1.Rows[index].Cells["NeedChangePay"].Value = order.NeedChangePay.ToString("f2");
                    dataGridView1.Rows[index].Cells["MoreOrLess"].Value = (order.ActualSellPrice + order.ServiceFee - (order.PaymentMoney - order.NeedChangePay)).ToString("f2");
                    dataGridView1.Rows[index].Cells["OrderID"].Value = order.OrderID;
                }
            }
        }

        private string FillWithZero(string inputData, int dataLength)
        {
            inputData = inputData.Trim();
            int length = inputData.Length;
            if (length < dataLength)
            {
                int count = dataLength - length;
                for (int i = 0; i < count; i++)
                {
                    inputData = "0" + inputData;
                }
            }
            return inputData;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int selectedIndex = dataGridView1.CurrentRow.Index;
                if (dataGridView1.Rows[selectedIndex].Cells["OrderID"].Value != null)
                {

                }
                else
                {
                    this.lbOrderNo.Text = string.Empty;
                    this.lbBillType.Text = string.Empty;
                    this.lbDeskNo.Text = string.Empty;
                    this.lbEatType.Text = string.Empty;
                    this.lbEmployeeNo.Text = string.Empty;
                    this.lbCashier.Text = string.Empty;
                    this.lbDelEmployeeNo.Text = string.Empty;
                    this.dataGridView2.Rows.Clear();
                    this.dataGridView3.Rows.Clear();
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
