using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VechsoftPos.Feature
{
    public partial class FormCustomQty : Form
    {
        private TextBox m_ActiveTextBox;
        private decimal m_CustomQty = 0M;
        private decimal m_CustomPrice = 0M;

        #region Output
        public decimal CustomQty
        {
            get { return m_CustomQty; }
        }

        public decimal CustomPrice
        {
            get { return m_CustomPrice; }
        }
        #endregion

        public FormCustomQty(bool IsCustomQty, decimal customQty, bool IsCustomPrice, decimal customPrice)
        {
            InitializeComponent();
            txtCustomPrice.Enabled = IsCustomPrice;
            txtCustomPrice.Text = customPrice.ToString("f2");
            txtCustomQty.Enabled = IsCustomQty;
            txtCustomQty.Text = customQty.ToString("f1");
        }

        private void FormCustomQty_Load(object sender, EventArgs e)
        {
            m_ActiveTextBox = this.txtCustomPrice;
        }

        private void btnNumeric_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (m_ActiveTextBox.Text.Trim() == ".")
            {
                m_ActiveTextBox.Text = btn.Text;
            }
            else if (m_ActiveTextBox.Text.Trim() == "0" && btn.Text == ".")
            {
                m_ActiveTextBox.Text += btn.Text;
            }
            else if (m_ActiveTextBox.Text.Trim() == "0" && btn.Text != ".")
            {
                m_ActiveTextBox.Text = btn.Text;
            }
            else
            {
                if (m_ActiveTextBox.Text.IndexOf('.') > 0 && btn.Text == ".")
                {
                    //do nothing
                }
                else
                {
                    m_ActiveTextBox.Text += btn.Text;
                }
            }
            m_ActiveTextBox.Focus();
            m_ActiveTextBox.Select(m_ActiveTextBox.Text.Length, 0);//光标移动到文本的末尾
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            string inputString = m_ActiveTextBox.Text;
            if (!string.IsNullOrEmpty(inputString))
            {
                m_ActiveTextBox.Select();
                SendKeys.Send("{BACKSPACE}");
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtCustomPrice.Text.Trim()) || string.IsNullOrEmpty(this.txtCustomQty.Text.Trim()))
            {
                return;
            }
            m_CustomPrice = decimal.Parse(txtCustomPrice.Text.Trim());
            m_CustomQty = decimal.Parse(txtCustomQty.Text.Trim());
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCustomPrice_MouseDown(object sender, MouseEventArgs e)
        {
            m_ActiveTextBox = this.ActiveControl as TextBox;
        }

        private void txtCustomQty_MouseDown(object sender, MouseEventArgs e)
        {
            m_ActiveTextBox = this.ActiveControl as TextBox;
        }
    }
}
