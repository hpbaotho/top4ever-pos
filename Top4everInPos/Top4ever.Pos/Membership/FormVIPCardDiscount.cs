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
    public partial class FormVIPCardDiscount : Form
    {
        private string m_CardNo;
        public string CardNo
        {
            get { return m_CardNo; }
        }

        private decimal m_DiscountRate;
        public decimal DiscountRate
        {
            get { return m_DiscountRate; }
        }

        private int m_Result;
        public int Result
        {
            get { return m_Result; }
        }

        public FormVIPCardDiscount()
        {
            InitializeComponent();
        }

        private void FormVIPCardDiscount_Load(object sender, EventArgs e)
        {

        }

        private void txtCardNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string cardNo = txtCardNo.Text.Trim();
                if (!string.IsNullOrEmpty(cardNo))
                {
                    m_CardNo = cardNo;
                    VIPCardService cardService = new VIPCardService();
                    m_DiscountRate = cardService.GetCardDiscountRate(cardNo);
                    if (m_DiscountRate == 0M)
                    {
                        txtDiscountRate.Text = "无折扣";
                    }
                    else
                    {
                        txtDiscountRate.Text = Convert.ToString((1M - m_DiscountRate) * 10) + "折";
                    }
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCardNo.Text.Trim()) || string.IsNullOrEmpty(txtDiscountRate.Text.Trim()))
            {
                MessageBox.Show("请先刷卡获取折扣率", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            m_Result = 1;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_Result = 0;
            this.Close();
        }
    }
}
