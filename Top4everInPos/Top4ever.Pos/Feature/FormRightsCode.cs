using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.CustomControl;

namespace Top4ever.Pos.Feature
{
    public partial class FormRightsCode : Form
    {
        private string m_InputText = string.Empty;
        private bool m_IsPunchCard = false;
        private bool m_ReturnValue = false;
        private IList<string> m_RightsCodeList = null;
        private TextBox m_CurrentControl;

        public string InputText
        {
            set { m_InputText = value; }
        }

        public bool IsPunchCard
        {
            set { m_IsPunchCard = value; }
        }

        public bool ReturnValue
        {
            get { return m_ReturnValue; }
        }

        public IList<string> RightsCodeList
        {
            get { return m_RightsCodeList; }
        }

        public FormRightsCode()
        {
            InitializeComponent();
        }

        private void FrmPunchCard_Load(object sender, EventArgs e)
        {
            if (m_IsPunchCard)
            {
                btnConfirm.Visible = false;
            }
            else
            {
                int px = this.btnPunchIn.Location.X;
                int py = this.btnPunchIn.Location.Y;
                btnConfirm.Location = new Point(px, py);
                btnPunchIn.Visible = false;
                btnPunchOut.Visible = false;
            }
            this.Width = 303;
            this.Height = 445;
            if (!string.IsNullOrEmpty(m_InputText))
            {
                this.lbMessage.Text = m_InputText;
                int px = (this.Width - this.lbMessage.Width) / 2;
                int py = this.lbMessage.Location.Y;
                this.lbMessage.Location = new Point(px, py);
            }
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtUserName.Select();
            m_CurrentControl = this.txtUserName;
        }

        private void btnNumeric_Click(object sender, EventArgs e)
        {
            if (m_CurrentControl != null)
            {
                Button btn = sender as Button;
                int index = m_CurrentControl.SelectionStart;
                string text = m_CurrentControl.Text;
                text = text.Insert(index, btn.Text);
                m_CurrentControl.Text = text;
                //光标移动到下一位
                m_CurrentControl.Select(index + 1, 0);
            }
        }

        private void txtUserName_MouseDown(object sender, MouseEventArgs e)
        {
            m_CurrentControl = this.ActiveControl as TextBox;
        }

        private void txtPassword_MouseDown(object sender, MouseEventArgs e)
        {
            m_CurrentControl = this.ActiveControl as TextBox;
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            if (m_CurrentControl != null)
            {
                string text = m_CurrentControl.Text;
                if (text.Length > 0)
                {
                    m_CurrentControl.Select();
                    SendKeys.Send("{BACKSPACE}");
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string userName = this.txtUserName.Text.Trim();
            string password = this.txtPassword.Text.Trim();
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("用户名或者密码不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            EmployeeService employeeService = new EmployeeService();
            IList<string> rightsCodeList = employeeService.GetRightsCodeList(userName, password);
            if (rightsCodeList != null && rightsCodeList.Count > 0)
            {
                m_ReturnValue = true;
                m_RightsCodeList = rightsCodeList;
                this.Close();
            }
            else
            {
                MessageBox.Show("获取权限失败，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPunchIn_Click(object sender, EventArgs e)
        {
            string userName = this.txtUserName.Text.Trim();
            string password = this.txtPassword.Text.Trim();
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("用户名或者密码不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnPunchOut_Click(object sender, EventArgs e)
        {
            string userName = this.txtUserName.Text.Trim();
            string password = this.txtPassword.Text.Trim();
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("用户名或者密码不能为空！", "InfoPos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
    }
}