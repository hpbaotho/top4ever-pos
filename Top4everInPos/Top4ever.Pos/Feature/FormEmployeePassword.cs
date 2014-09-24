using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Entity;

namespace Top4ever.Pos.Feature
{
    public partial class FormEmployeePassword : Form
    {
        public FormEmployeePassword()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCurrentPassword.Text.Trim()))
            {
                MessageBox.Show("请输入当前密码！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
            string currentPassword = this.txtCurrentPassword.Text.Trim();
            string newPassword = this.txtNewPassword.Text.Trim();
            int result = EmployeeService.GetInstance().UpdateEmployeePassword(ConstantValuePool.CurrentEmployee.EmployeeNo, currentPassword, newPassword);
            if (result == 1)
            {
                MessageBox.Show("密码修改成功！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else if (result == 2)
            {
                MessageBox.Show("您输入的原密码不正确！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
