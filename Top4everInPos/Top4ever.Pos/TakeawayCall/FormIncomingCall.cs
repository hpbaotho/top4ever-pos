using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Domain.Customers;
using Top4ever.Entity;

namespace Top4ever.Pos.TakeawayCall
{
    public partial class FormIncomingCall : Form
    {
        private string _telephone = string.Empty;

        public FormIncomingCall(string telephone)
        {
            _telephone = telephone;
            InitializeComponent();
        }

        private void FormIncomingCall_Load(object sender, EventArgs e)
        {
            ShowCustomerInfo(_telephone);
        }

        private void ShowCustomerInfo(string telephone)
        {
            CustomersService customerService = new CustomersService();
            IList<CustomerInfo> customerInfoList = customerService.GetCustomerInfoByPhone(telephone);
            if (customerInfoList != null && customerInfoList.Count > 0)
            {
                CustomerInfo customerInfo = customerInfoList[0];
                txtCustomerName.Text = customerInfo.CustomerName;
                txtAddress1.Text = customerInfo.DeliveryAddress1;
                txtAddress2.Text = customerInfo.DeliveryAddress2;
                txtAddress3.Text = customerInfo.DeliveryAddress3;
                if (customerInfo.ActiveIndex == 1)
                {
                    ckAddress1.Checked = true;
                }
                else if (customerInfo.ActiveIndex == 2)
                {
                    ckAddress2.Checked = true;
                }
                else if (customerInfo.ActiveIndex == 3)
                {
                    ckAddress3.Checked = true;
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            int activeIndex = 1;
            if (ckAddress1.Checked)
            {
                activeIndex = 1;
            }
            else if (ckAddress2.Checked)
            {
                activeIndex = 2;
            }
            else if (ckAddress3.Checked)
            {
                activeIndex = 3;
            }
            CustomerInfo customerInfo = new CustomerInfo();
            customerInfo.Telephone = this.lbTelephone.Text;
            customerInfo.CustomerName = this.txtCustomerName.Text.Trim();
            customerInfo.DeliveryAddress1 = this.txtAddress1.Text.Trim();
            customerInfo.DeliveryAddress2 = this.txtAddress2.Text.Trim();
            customerInfo.DeliveryAddress3 = this.txtAddress3.Text.Trim();
            customerInfo.ActiveIndex = activeIndex;
            customerInfo.LastModifiedEmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
            CustomersService customerService = new CustomersService();
            if (customerService.UpdateCustomerInfo(customerInfo))
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("更新客户信息失败，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
