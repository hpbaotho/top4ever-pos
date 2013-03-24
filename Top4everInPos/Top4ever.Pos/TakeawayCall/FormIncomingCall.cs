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
        private CustomerInfo _customerInfo = null;
        private string _address = string.Empty;

        public FormIncomingCall(string telephone, string address)
        {
            CustomersService customerService = new CustomersService();
            _customerInfo = customerService.GetCustomerInfoByPhone(telephone);
            _address = address;
            InitializeComponent();
        }

        private void FormIncomingCall_Load(object sender, EventArgs e)
        {
            if (_customerInfo != null)
            {
                txtCustomerName.Text = _customerInfo.CustomerName;
                txtAddress1.Text = _customerInfo.DeliveryAddress1;
                txtAddress2.Text = _customerInfo.DeliveryAddress2;
                txtAddress3.Text = _customerInfo.DeliveryAddress3;
                if (_customerInfo.ActiveIndex == 1)
                {
                    ckAddress1.Checked = true;
                }
                else if (_customerInfo.ActiveIndex == 2)
                {
                    ckAddress2.Checked = true;
                }
                else if (_customerInfo.ActiveIndex == 3)
                {
                    ckAddress3.Checked = true;
                }
            }
            if (!string.IsNullOrEmpty(_address))
            {
                if (txtAddress1.Text == _address)
                {
                    ckAddress1.Checked = true;
                }
                else if (txtAddress2.Text == _address)
                {
                    ckAddress2.Checked = true;
                }
                else if (txtAddress3.Text == _address)
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
            if (_customerInfo == null)  //新增
            {
                int result = customerService.CreateCustomerInfo(customerInfo);
                if (result == 1)
                {
                    this.Close();
                }
                else if (result == 2)
                {
                    MessageBox.Show("联系电话已存在，不能重复添加！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("创建客户信息失败，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else  //更新
            {
                if (customerService.UpdateCustomerInfo(customerInfo))
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("更新客户信息失败，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
