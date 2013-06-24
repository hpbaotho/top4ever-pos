using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;

namespace Top4ever.Pos.Membership
{
    public partial class FormVIPCardPassword : Form
    {
        private string cardNo = string.Empty;
        private string currentPassword = string.Empty;

        public FormVIPCardPassword(string cardNo, string currentPassword)
        {
            this.cardNo = cardNo;
            this.currentPassword = currentPassword;
            InitializeComponent();
            this.txtCurrentPassword.Text = currentPassword;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text.Trim()))
            {
                MessageBox.Show("请输入新密码！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtNewPwd.Text.Trim()))
            {
                MessageBox.Show("请再次确认密码！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtNewPassword.Text.Trim() != txtNewPwd.Text.Trim())
            {
                MessageBox.Show("两次输入的新密码不一致！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string newPassword = this.txtNewPassword.Text.Trim();
            VIPCardService cardService = new VIPCardService();
            if (cardService.UpdateCardPassword(cardNo, currentPassword, newPassword))
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("密码修改失败！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
