using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Domain.MembershipCard;
using Top4ever.Entity;

namespace VechsoftPos.Membership
{
    public partial class FormVIPCardSearch : Form
    {
        private string _inputPassword = string.Empty;

        public FormVIPCardSearch()
        {
            InitializeComponent();
        }

        private void FormVIPCardSearch_Load(object sender, EventArgs e)
        {
            btnModifyPassword.DisplayColor = btnModifyPassword.BackColor;
            btnModifyPassword.Enabled = false;
            btnModifyPassword.BackColor = ConstantValuePool.DisabledColor;
        }

        private void txtVIPCardNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                Feature.FormNumericKeypad keyForm = new Feature.FormNumericKeypad(false);
                keyForm.DisplayText = "请输入密码";
                keyForm.IsPassword = true;
                keyForm.ShowDialog();
                _inputPassword = keyForm.KeypadValue;
                string cardNo = txtVIPCardNo.Text.Trim();
                VIPCard card;
                int result = VIPCardService.GetInstance().SearchVIPCard(cardNo, _inputPassword, out card);
                if (card != null && result == 1)
                {
                    if (card.Status == 0)
                    {
                        MessageBox.Show("该卡号未开卡，请先开卡！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    txtName.Text = card.Name;
                    txtBalance.Text = card.Balance.ToString("f2");
                    txtIntegral.Text = card.Integral.ToString();
                    txtDiscount.Text = ((1M - card.DiscountRate) * 10M).ToString("f2") + "折";
                    txtTelephone.Text = card.Telephone;
                    txtAddress.Text = card.Address;
                    if (card.Status == 1)
                    {
                        txtCardStatus.Text = "启用";
                    }
                    if (card.Status == 2)
                    {
                        txtCardStatus.Text = "锁卡";
                    }
                    if (card.Status == 3)
                    {
                        txtCardStatus.Text = "挂失";
                    }
                    if (card.Status == 4)
                    {
                        txtCardStatus.Text = "作废";
                    }
                    txtOpenCardTime.Text = card.OpenCardTime.ToString("yyyy-MM-dd HH:mm:ss");
                    if (card.LastConsumeTime != null)
                    {
                        txtLastConsumeTime.Text = Convert.ToDateTime(card.LastConsumeTime).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    btnModifyPassword.Enabled = true;
                    btnModifyPassword.BackColor = btnModifyPassword.DisplayColor;
                }
                else if (result == 2)
                {
                    txtVIPCardNo.Text = string.Empty;
                    txtName.Text = string.Empty;
                    txtBalance.Text = string.Empty;
                    txtIntegral.Text = string.Empty;
                    txtDiscount.Text = string.Empty;
                    txtTelephone.Text = string.Empty;
                    txtAddress.Text = string.Empty;
                    txtCardStatus.Text = string.Empty;
                    txtOpenCardTime.Text = string.Empty;
                    txtLastConsumeTime.Text = string.Empty;
                    btnModifyPassword.Enabled = false;
                    btnModifyPassword.BackColor = ConstantValuePool.DisabledColor;
                    MessageBox.Show("您输入的会员卡号或者密码错误！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    txtVIPCardNo.Text = string.Empty;
                    txtName.Text = string.Empty;
                    txtBalance.Text = string.Empty;
                    txtIntegral.Text = string.Empty;
                    txtDiscount.Text = string.Empty;
                    txtTelephone.Text = string.Empty;
                    txtAddress.Text = string.Empty;
                    txtCardStatus.Text = string.Empty;
                    txtOpenCardTime.Text = string.Empty;
                    txtLastConsumeTime.Text = string.Empty;
                    btnModifyPassword.Enabled = false;
                    btnModifyPassword.BackColor = ConstantValuePool.DisabledColor;
                    MessageBox.Show("服务器出现错误，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnModifyPassword_Click(object sender, EventArgs e)
        {
            string cardNo = txtVIPCardNo.Text.Trim();
            if (!string.IsNullOrEmpty(cardNo))
            {
                FormVIPCardPassword formPassword = new FormVIPCardPassword(cardNo, _inputPassword);
                formPassword.ShowDialog();
                if (formPassword.Result)
                {
                    this.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
