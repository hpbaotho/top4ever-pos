using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.CustomControl;
using Top4ever.Domain.Accounts;
using Top4ever.Domain.Customers;
using Top4ever.Domain.Transfer;
using Top4ever.Pos.Feature;

namespace Top4ever.Pos.TakeawayCall
{
    public partial class FormDeliveryGoods : Form
    {
        private bool m_HandwritingEnabled = false;
        private SalesOrder _salesOrder = null;
        private string _telephone;
        private string _customerName;
        private string _address;
        private bool _hasDeliveried = false;
        public bool HasDeliveried
        {
            get { return _hasDeliveried; }
        }

        public FormDeliveryGoods(SalesOrder salesOrder, string telephone, string customerName, string address)
        {
            _salesOrder = salesOrder;
            _telephone = telephone;
            _customerName = customerName;
            _address = address;
            InitializeComponent();
        }

        private void FormDeliveryGoods_Load(object sender, EventArgs e)
        {
            ShowHandwriting();
            if (_salesOrder != null)
            {
                this.txtTranSeq.Text = _salesOrder.order.TranSequence.ToString();
                this.txtTotalAmount.Text = (_salesOrder.order.ActualSellPrice + _salesOrder.order.ServiceFee).ToString("f2");
                this.txtTelephone.Text = _telephone;
                this.txtName.Text = _customerName;
                this.txtAddress.Text = _address;
            }
        }

        private void btnHandwriting_Click(object sender, EventArgs e)
        {
            m_HandwritingEnabled = !m_HandwritingEnabled;
            ShowHandwriting();
        }

        private void ShowHandwriting()
        {
            if (m_HandwritingEnabled)
            {
                this.Height = 578;
                this.handwritingPad1.Visible = true;
                this.btnHandwriting.Text = "手写输入∧";
            }
            else
            {
                this.Height = 350;
                this.handwritingPad1.Visible = false;
                this.btnHandwriting.Text = "手写输入∨";
            }
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            string inputString = this.txtRemark.Text;
            if (!string.IsNullOrEmpty(inputString))
            {
                this.txtRemark.TabIndex = 0;
                this.txtRemark.Focus();
                SendKeys.Send("{BACKSPACE}");
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FormNumericKeypad keyForm = new FormNumericKeypad(false);
            keyForm.DisplayText = "请输入用户名";
            keyForm.ShowDialog();
            if (!string.IsNullOrEmpty(keyForm.KeypadValue))
            {
                string employeeNo = keyForm.KeypadValue;
                Employee employee = null;
                EmployeeService employeeService = new EmployeeService();
                int result = employeeService.GetEmployeeByNo(employeeNo, ref employee);
                if (result == 1)
                {
                    this.txtEmployeeName.Text = employee.Name;
                }
                else if (result == 2)
                {
                    MessageBox.Show("用户名不存在，请检查输入是否正确！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("获取客户信息失败，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            CustomerOrder customerOrder = new CustomerOrder();
            customerOrder.OrderID = _salesOrder.order.OrderID;
            customerOrder.Telephone = this.txtTelephone.Text;
            customerOrder.CustomerName = this.txtName.Text;
            customerOrder.Address = this.txtAddress.Text;
            customerOrder.Remark = this.txtRemark.Text.Trim();
            customerOrder.DeliveryEmployeeName = this.txtEmployeeName.Text.Trim();
            CustomersService customerService = new CustomersService();
            if (customerService.UpdateTakeoutOrderStatus(customerOrder))
            {
                //打印
                _hasDeliveried = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("出货失败，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void handwritingPad1_UserHandWriting(object sender, InkWritingEventArgs e)
        {
            this.txtRemark.Text += e.InkPadValue;
        }
    }
}
