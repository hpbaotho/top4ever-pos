using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Top4ever.Pos.TakeawayCall
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
            m_ActiveTextBox = this.txtTelephone;
            ShowHandwriting();
        }

        private void btnHandwriting_Click(object sender, EventArgs e)
        {
            m_HandwritingEnabled = !m_HandwritingEnabled;
            ShowHandwriting();
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
                this.Height = 265;
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

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
