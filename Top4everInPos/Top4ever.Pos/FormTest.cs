using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.ClientService.Enum;
using Top4ever.CustomControl;
using Top4ever.Domain.Accounts;
using Top4ever.Entity;

namespace Top4ever.Pos
{
    public partial class FormTest : Form
    {
        /// <summary>
        /// 1:前台登录 2:员工打卡 3:后台登录
        /// </summary>
        private int m_LoginType = 1;
        private TextBox m_ActiveTextBox;

        public FormTest()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            m_ActiveTextBox = this.txtName;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNumber_Click(object sender, EventArgs e)
        {
            VistaButton btn = sender as VistaButton;
            if (string.IsNullOrEmpty(m_ActiveTextBox.Text))
            {
                m_ActiveTextBox.Text = btn.ButtonText;
            }
            else
            {
                m_ActiveTextBox.Text += btn.ButtonText;
            }
        }

        private void FrontLoginLink_Click(object sender, EventArgs e)
        {
            m_LoginType = 1;
            this.lbSystemName.Text = "前台登录";
        }

        private void EmployeeClockIn_Click(object sender, EventArgs e)
        {
            m_LoginType = 2;
            this.lbSystemName.Text = "员工打卡";
        }

        private void ManageLink_Click(object sender, EventArgs e)
        {
            m_LoginType = 3;
            this.lbSystemName.Text = "后台管理";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (m_LoginType == 1)
            { 
                //前台登录
                Employee employee = null;
                int operCode = EmployeeService.GetInstance().EmployeeLogin(this.txtName.Text.Trim(), this.txtPassword.Text.Trim(), ref employee);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    //保存静态池内
                    ConstantValuePool.CurrentEmployee = employee;

                }
                else
                {
                    MessageBox.Show("您输入的用户名或者密码错误，请重新输入！");
                }
            }
        }

        private void txtName_MouseDown(object sender, MouseEventArgs e)
        {
            m_ActiveTextBox = this.ActiveControl as TextBox;
        }

        private void txtPassword_MouseDown(object sender, MouseEventArgs e)
        {
            m_ActiveTextBox = this.ActiveControl as TextBox;
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            string inputString = m_ActiveTextBox.Text;
            if (!string.IsNullOrEmpty(inputString))
            {
                m_ActiveTextBox.Text = m_ActiveTextBox.Text.Substring(0, inputString.Length - 1);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            m_ActiveTextBox.Text = string.Empty;
        }
    }
}
