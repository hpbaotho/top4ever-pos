﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Top4ever.Pos.Feature
{
    public partial class FormNumericKeypad : Form
    {
        private bool m_IsNaN = false;
        private string m_DisplayText = string.Empty;
        private string m_DefaultValue = string.Empty;
        private string m_KeypadValue = string.Empty;

        #region Input 
        public string DisplayText
        {
            set { m_DisplayText = value; }
        }

        public string DefaultValue
        {
            set { m_DefaultValue = value; }
        }
        #endregion

        #region Output
        public string KeypadValue
        {
            get { return m_KeypadValue; }
        }
        #endregion

        public FormNumericKeypad()
        {
            InitializeComponent();
        }

        public FormNumericKeypad(bool IsNumeric)
        {
            m_IsNaN = !IsNumeric;
            InitializeComponent();
        }

        private void FormNumericKeypad_Load(object sender, EventArgs e)
        {
            this.lbInput.Text = m_DisplayText;
            if (!string.IsNullOrEmpty(m_DefaultValue))
            {
                txtNumeric.Text = m_DefaultValue;
            }
            if (m_IsNaN)
            {
                btnDot.Enabled = false;
                btnDot.BackColor = Entity.ConstantValuePool.DisabledColor;
            }
        }

        private void btnNumeric_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (m_IsNaN)
            {
                this.txtNumeric.Text += btn.Text;
            }
            else
            {
                if (this.txtNumeric.Text.Trim() == ".")
                {
                    this.txtNumeric.Text = btn.Text;
                }
                else if (this.txtNumeric.Text.Trim() == "0" && btn.Text == ".")
                {
                    this.txtNumeric.Text += btn.Text;
                }
                else if (this.txtNumeric.Text.Trim() == "0" && btn.Text != ".")
                {
                    this.txtNumeric.Text = btn.Text;
                }
                else
                {
                    if (this.txtNumeric.Text.IndexOf('.') > 0 && btn.Text == ".")
                    {
                        //do nothing
                    }
                    else
                    {
                        this.txtNumeric.Text += btn.Text;
                    }
                }
            }
            this.txtNumeric.Focus();
            this.txtNumeric.Select(this.txtNumeric.Text.Length, 0);//光标移动到文本的末尾
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            string text = this.txtNumeric.Text;
            if (text.Length > 0)
            {
                this.txtNumeric.Text = text.Substring(0, text.Length - 1);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtNumeric.Text.Trim()))
            {
                return;
            }
            m_KeypadValue = this.txtNumeric.Text;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
