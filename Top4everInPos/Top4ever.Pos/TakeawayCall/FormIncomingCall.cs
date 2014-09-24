using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
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
        /// <summary>
        /// 0:手动 1:程序
        /// </summary>
        private int _callType = 0;
        private System.Threading.Timer timer = null;

        private string _selectedAddress = string.Empty;
        public string SelectedAddress
        {
            get { return _selectedAddress; }
        }
        private CustomerInfo m_CustomerInfo = null;
        public CustomerInfo CurCustomerInfo
        {
            get { return m_CustomerInfo; }
        }
        private string m_IncomingPhoneNo = string.Empty;
        public string IncomingPhoneNo
        {
            get { return m_IncomingPhoneNo; }
        }

        public FormIncomingCall(string telephone, string address, int callType)
        {
            _customerInfo = CustomersService.GetInstance().GetCustomerInfoByPhone(telephone);
            _address = address;
            _callType = callType;
            InitializeComponent();
            this.lbTelephone.Text = telephone;
            if (callType == 1 && ConstantValuePool.BizSettingConfig.telCallConfig.Enabled && ConstantValuePool.IsTelCallWorking)
            {
                timer = new System.Threading.Timer(new TimerCallback(CallBack), null, 15000, 500);
            }
        }

        private void CallBack(Object stateObject)
        {
            if (ConstantValuePool.BizSettingConfig.telCallConfig.Enabled && ConstantValuePool.IsTelCallWorking)
            {
                if (ConstantValuePool.BizSettingConfig.telCallConfig.Model == 0 || ConstantValuePool.BizSettingConfig.telCallConfig.Model == 1)
                {
                    if (LDT.Check_State(ConstantValuePool.TelCallID) == 255)
                    {
                        string strPhoneNo = LDT.GetNumber_Tel(1).ToString();
                        if (strPhoneNo.Length > 0)
                        {
                            //创建通话记录
                            CallRecord callRecord = new CallRecord();
                            callRecord.CallRecordID = Guid.NewGuid();
                            callRecord.Telephone = strPhoneNo;
                            callRecord.CallTime = DateTime.Now;
                            callRecord.Status = 0;
                            CustomersService.GetInstance().CreateOrUpdateCallRecord(callRecord);

                            this.lbTelephone.Text = strPhoneNo;
                            this.txtCustomerName.Text = string.Empty;
                            this.txtAddress1.Text = string.Empty;
                            this.txtAddress2.Text = string.Empty;
                            this.txtAddress3.Text = string.Empty;
                            _customerInfo = CustomersService.GetInstance().GetCustomerInfoByPhone(strPhoneNo);
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
                        }
                    }
                }
                if (ConstantValuePool.BizSettingConfig.telCallConfig.Model == 0)
                {
                    ConstantValuePool.IsTelCallWorking = LDT.Plugin_Tel(ConstantValuePool.TelCallID);
                }
            }
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
            string address = string.Empty;
            if (ckAddress1.Checked)
            {
                activeIndex = 1;
                address = txtAddress1.Text.Trim();
            }
            else if (ckAddress2.Checked)
            {
                activeIndex = 2;
                address = txtAddress2.Text.Trim();
            }
            else if (ckAddress3.Checked)
            {
                activeIndex = 3;
                address = txtAddress3.Text.Trim();
            }
            if (string.IsNullOrEmpty(address))
            {
                if (_customerInfo == null)
                {
                    m_IncomingPhoneNo = lbTelephone.Text;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("选择的地址不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }
            CustomerInfo customerInfo = new CustomerInfo();
            customerInfo.Telephone = this.lbTelephone.Text;
            customerInfo.CustomerName = this.txtCustomerName.Text.Trim();
            customerInfo.DeliveryAddress1 = this.txtAddress1.Text.Trim();
            customerInfo.DeliveryAddress2 = this.txtAddress2.Text.Trim();
            customerInfo.DeliveryAddress3 = this.txtAddress3.Text.Trim();
            customerInfo.ActiveIndex = activeIndex;
            customerInfo.LastModifiedEmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;

            if (_customerInfo == null)  //新增
            {
                int result = CustomersService.GetInstance().CreateCustomerInfo(customerInfo);
                if (result == 1)
                {
                    _selectedAddress = address;
                    m_CustomerInfo = customerInfo;
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
                if (CustomersService.GetInstance().UpdateCustomerInfo(customerInfo))
                {
                    _selectedAddress = address;
                    m_CustomerInfo = customerInfo;
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

        private void ckAddress1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckAddress1.Checked)
            {
                ckAddress2.Checked = false;
                ckAddress3.Checked = false;
            }
        }

        private void ckAddress2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckAddress2.Checked)
            {
                ckAddress1.Checked = false;
                ckAddress3.Checked = false;
            }
        }

        private void ckAddress3_CheckedChanged(object sender, EventArgs e)
        {
            if (ckAddress3.Checked)
            {
                ckAddress1.Checked = false;
                ckAddress2.Checked = false;
            }
        }
    }
}
