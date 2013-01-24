using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Top4ever.Pos.Feature
{
    public partial class FormBillManagement : Form
    {
        //1:输入台号 2:输入流水号
        private int m_SeachType = 0;

        public FormBillManagement()
        {
            InitializeComponent();
        }

        private void btnDeskNo_Click(object sender, EventArgs e)
        {
            FormNumericKeypad keyForm = new FormNumericKeypad(false);
            keyForm.DisplayText = "请输入台号";
            keyForm.ShowDialog();
            if (!string.IsNullOrEmpty(keyForm.KeypadValue))
            {
                m_SeachType = 1;
                this.txtSearchValue.Text = keyForm.KeypadValue;
            }
        }

        private void btnTranSequence_Click(object sender, EventArgs e)
        {
            FormNumericKeypad keyForm = new FormNumericKeypad(false);
            keyForm.DisplayText = "请输入流水号";
            keyForm.ShowDialog();
            if (!string.IsNullOrEmpty(keyForm.KeypadValue))
            {
                m_SeachType = 2;
                this.txtSearchValue.Text = keyForm.KeypadValue;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }
    }
}
