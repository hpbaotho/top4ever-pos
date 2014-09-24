using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Domain.Customers;

namespace Top4ever.Pos.TakeawayCall
{
    public partial class FormFindCustomer : Form
    {
        private IList<CustomerInfo> m_CustomerInfoList;
        private CustomerInfo m_SelectedCustomerInfo;
        public CustomerInfo SelectedCustomerInfo
        {
            get { return m_SelectedCustomerInfo; }
        }

        public FormFindCustomer()
        {
            InitializeComponent();
        }

        private void txtTelephone_TextChanged(object sender, EventArgs e)
        {
            if (m_CustomerInfoList != null && m_CustomerInfoList.Count > 0)
            {
                string telephone = txtTelephone.Text.Trim();
                if (!string.IsNullOrEmpty(telephone) && telephone.Length >= 4)
                { 
                    IList<CustomerInfo> customerInfoList = new List<CustomerInfo>();
                    foreach (CustomerInfo item in m_CustomerInfoList)
                    {
                        if (item.Telephone.IndexOf(telephone) >= 0)
                        {
                            customerInfoList.Add(item);
                        }
                    }
                    ReBindCustomerInfo(customerInfoList);
                }
            }
            else
            {
                MessageBox.Show("获取客户信息失败，请先加载客户信息！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            if (m_CustomerInfoList != null && m_CustomerInfoList.Count > 0)
            {
                string name = txtCustomerName.Text.Trim();
                if (!string.IsNullOrEmpty(name))
                {
                    IList<CustomerInfo> customerInfoList = new List<CustomerInfo>();
                    foreach (CustomerInfo item in m_CustomerInfoList)
                    {
                        if (item.CustomerName.IndexOf(name) >= 0)
                        {
                            customerInfoList.Add(item);
                        }
                    }
                    ReBindCustomerInfo(customerInfoList);
                }
            }
            else
            {
                MessageBox.Show("获取客户信息失败，请先加载客户信息！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnInputCustomer_Click(object sender, EventArgs e)
        {
            FormInputCustomer form = new FormInputCustomer();
            form.Show();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (dgvCustomerInfo.CurrentRow != null)
            {
                int selectedIndex = dgvCustomerInfo.CurrentRow.Index;
                m_SelectedCustomerInfo = dgvCustomerInfo.Rows[selectedIndex].Cells["Telephone"].Tag as CustomerInfo;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_SelectedCustomerInfo = null;
            this.Close();
        }

        private void btnLoadCustomer_Click(object sender, EventArgs e)
        {
            IList<CustomerInfo> customerInfoList = CustomersService.GetInstance().GetAllCustomerInfo();
            if (customerInfoList != null && customerInfoList.Count > 0)
            {
                m_CustomerInfoList = customerInfoList;
                ReBindCustomerInfo(customerInfoList);
            }
        }

        private void ReBindCustomerInfo(IList<CustomerInfo> customerInfoList)
        {
            dgvCustomerInfo.Rows.Clear();
            if (customerInfoList != null && customerInfoList.Count > 0)
            {
                foreach (CustomerInfo item in customerInfoList)
                {
                    int index = dgvCustomerInfo.Rows.Add();
                    dgvCustomerInfo.Rows[index].Cells["Telephone"].Value = item.Telephone;
                    dgvCustomerInfo.Rows[index].Cells["Telephone"].Tag = item;
                    dgvCustomerInfo.Rows[index].Cells["CustomerName"].Value = item.CustomerName;
                    string address = string.Empty;
                    if (item.ActiveIndex == 1)
                    {
                        address = item.DeliveryAddress1;
                    }
                    else if (item.ActiveIndex == 2)
                    {
                        address = item.DeliveryAddress2;
                    }
                    else if (item.ActiveIndex == 3)
                    {
                        address = item.DeliveryAddress3;
                    }
                    dgvCustomerInfo.Rows[index].Cells["Address"].Value = address;
                }
                dgvCustomerInfo.Rows[0].Selected = false;
            }
        }

        private void dgvCustomerInfo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomerInfo.CurrentRow != null)
            {
                int selectedIndex = dgvCustomerInfo.CurrentRow.Index;
                m_SelectedCustomerInfo = dgvCustomerInfo.Rows[selectedIndex].Cells["Telephone"].Tag as CustomerInfo;
                this.Close();
            }
        }
    }
}
