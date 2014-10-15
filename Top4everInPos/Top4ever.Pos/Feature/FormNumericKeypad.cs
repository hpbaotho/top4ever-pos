using System;
using System.Windows.Forms;
using Top4ever.Entity;

namespace VechsoftPos.Feature
{
    public partial class FormNumericKeypad : Form
    {
        private bool m_IsNaN = false;
        private string m_DisplayText = string.Empty;
        private string m_DefaultValue = string.Empty;
        private string m_KeypadValue = string.Empty;
        private bool m_IsPassword = false;
        private TextBox _activeTextBox;

        #region Input 
        public string DisplayText
        {
            set { m_DisplayText = value; }
        }

        public string DefaultValue
        {
            set { m_DefaultValue = value; }
        }

        public bool IsPassword
        {
            set { m_IsPassword = value; }
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

        public FormNumericKeypad(bool isNumeric)
        {
            m_IsNaN = !isNumeric;
            InitializeComponent();
        }

        private void FormNumericKeypad_Load(object sender, EventArgs e)
        {
            _activeTextBox = this.txtNumeric;
            this.lbInput.Text = m_DisplayText;
            if (!string.IsNullOrEmpty(m_DefaultValue))
            {
                txtNumeric.Text = m_DefaultValue;
            }
            if (m_IsNaN)
            {
                btnDot.Enabled = false;
                btnDot.BackColor = ConstantValuePool.DisabledColor;
            }
            if (m_IsPassword)
            {
                txtNumeric.PasswordChar = '*';
            }
        }

        private void btnNumeric_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            if (m_IsNaN)
            {
                int index = _activeTextBox.SelectionStart;
                string text = _activeTextBox.Text;
                text = text.Insert(index, btn.Text);
                _activeTextBox.Text = text;
                //光标移动到下一位
                _activeTextBox.Select(index + 1, 0);
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
            string inputString = _activeTextBox.Text;
            if (!string.IsNullOrEmpty(inputString))
            {
                _activeTextBox.Select();
                SendKeys.Send("{BACKSPACE}");
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!m_IsPassword && string.IsNullOrEmpty(this.txtNumeric.Text.Trim()))
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
