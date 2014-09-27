using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Entity;

namespace VechsoftPos.Membership
{
    public partial class FormVIPCardStatus : Form
    {
        public FormVIPCardStatus()
        {
            InitializeComponent();
        }

        private void FormVIPCardStatus_Load(object sender, EventArgs e)
        {
            cmbCardStatus.Items.Clear();
            cmbCardStatus.Items.Add("开卡");
            if (RightsItemCode.FindRights(RightsItemCode.REPORTLOSS))
            {
                cmbCardStatus.Items.Add("挂失");
            }
            if (RightsItemCode.FindRights(RightsItemCode.LOCKCARD))
            {
                cmbCardStatus.Items.Add("锁卡");
            }
            if (RightsItemCode.FindRights(RightsItemCode.CANCELCARD))
            {
                cmbCardStatus.Items.Add("作废");
            }
            cmbCardStatus.SelectedIndex = 0;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string cardNo = txtCardNo.Text.Trim();
            if (string.IsNullOrEmpty(cardNo))
            {
                MessageBox.Show("请输入您的会员卡号！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string password = txtPassword.Text.Trim();
            int status = 0;
            string strStatus = cmbCardStatus.Text;
            if (strStatus == "开卡")
            {
                status = 1;
            }
            if (strStatus == "挂失")
            {
                status = 2;
            }
            if (strStatus == "锁卡")
            {
                status = 3;
            }
            if (strStatus == "作废")
            {
                status = 4;
            }
            int result = VIPCardService.GetInstance().UpdateCardStatus(cardNo, password, status);
            if (result == 0)
            {
                MessageBox.Show("卡状态更新失败！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (result == 1)
            {
                MessageBox.Show("卡状态更新成功！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            if (result == 2)
            {
                MessageBox.Show("该卡还未开通！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (result == 3)
            {
                MessageBox.Show("该卡已开通！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (result == 4)
            {
                MessageBox.Show("您输入的密码不正确！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (result == 5)
            {
                MessageBox.Show("该卡已挂失！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (result == 6)
            {
                MessageBox.Show("该卡已锁卡！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (result == 7)
            {
                MessageBox.Show("该卡已作废！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
