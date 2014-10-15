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
using Top4ever.CustomControl;

namespace VechsoftPos.TakeawayCall
{
    public partial class FormInputCustomer : Form
    {
        private bool m_HandwritingEnabled = false;
        private TextBox m_ActiveTextBox;

        public FormInputCustomer()
        {
            InitializeComponent();
        }

        private void FormInputCustomer_Load(object sender, EventArgs e)
        {
            m_ActiveTextBox = this.txtName;
            ShowHandwriting();
        }

        private void btnHandwriting_Click(object sender, EventArgs e)
        {
            m_HandwritingEnabled = !m_HandwritingEnabled;
            ShowHandwriting();
            this.txtName.Focus();
        }

        private void ShowHandwriting()
        {
            if (m_HandwritingEnabled)
            {
                this.Height = 518;
                this.handwritingPad1.Visible = true;
                this.btnHandwriting.Text = "手写输入∧";
            }
            else
            {
                this.Height = 289;
                this.handwritingPad1.Visible = false;
                this.btnHandwriting.Text = "手写输入∨";
            }
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            string inputString = m_ActiveTextBox.Text;
            if (!string.IsNullOrEmpty(inputString))
            {
                m_ActiveTextBox.Select();
                SendKeys.Send("{BACKSPACE}");
            }
        }

        private void txtTelephone_MouseDown(object sender, MouseEventArgs e)
        {
            m_ActiveTextBox = this.ActiveControl as TextBox;
        }

        private void txtName_MouseDown(object sender, MouseEventArgs e)
        {
            m_ActiveTextBox = this.ActiveControl as TextBox;
        }

        private void txtAddress1_MouseDown(object sender, MouseEventArgs e)
        {
            m_ActiveTextBox = this.ActiveControl as TextBox;
        }

        private void txtAddress2_MouseDown(object sender, MouseEventArgs e)
        {
            m_ActiveTextBox = this.ActiveControl as TextBox;
        }

        private void txtAddress3_MouseDown(object sender, MouseEventArgs e)
        {
            m_ActiveTextBox = this.ActiveControl as TextBox;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            CustomerInfo customerInfo = new CustomerInfo
            {
                Telephone = this.txtTelephone.Text.Trim(), 
                CustomerName = this.txtName.Text.Trim(), 
                DeliveryAddress1 = this.txtAddress1.Text.Trim(), 
                DeliveryAddress2 = this.txtAddress2.Text.Trim(), 
                DeliveryAddress3 = this.txtAddress3.Text.Trim()
            };
            if (!string.IsNullOrEmpty(customerInfo.DeliveryAddress3))
            {
                customerInfo.ActiveIndex = 3;
            }
            if (!string.IsNullOrEmpty(customerInfo.DeliveryAddress2))
            {
                customerInfo.ActiveIndex = 2;
            }
            if (!string.IsNullOrEmpty(customerInfo.DeliveryAddress1))
            {
                customerInfo.ActiveIndex = 1;
            }
            customerInfo.LastModifiedEmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;

            if (string.IsNullOrEmpty(customerInfo.Telephone))
            {
                MessageBox.Show("联系电话不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(customerInfo.CustomerName))
            {
                MessageBox.Show("客户名称不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (customerInfo.ActiveIndex <= 0)
            {
                MessageBox.Show("客户的地址信息不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            int result = CustomersService.GetInstance().CreateCustomerInfo(customerInfo);
            if (result == 1)
            {
                MessageBox.Show("创建客户信息成功！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void handwritingPad1_UserHandWriting(object sender, InkWritingEventArgs e)
        {
            m_ActiveTextBox.Text += e.InkPadValue;
            m_ActiveTextBox.TabIndex = 0;
            m_ActiveTextBox.Focus();
        }
    }
}
