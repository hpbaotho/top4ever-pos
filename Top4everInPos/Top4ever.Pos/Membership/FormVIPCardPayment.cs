using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Domain.MembershipCard;
using Top4ever.Entity;

namespace Top4ever.Pos.Membership
{
    public partial class FormVIPCardPayment : Form
    {
        private VIPCard m_Card = null;
        private decimal m_UnPaidPrice = 0M;

        private decimal m_CardPaidAmount = 0M;
        public decimal CardPaidAmount
        {
            get { return m_CardPaidAmount; }
        }

        private string m_CardNo = string.Empty;
        public string CardNo
        {
            get { return m_CardNo; }
        }

        public FormVIPCardPayment(decimal unPaidPrice)
        {
            m_UnPaidPrice = unPaidPrice;
            InitializeComponent();
        }

        private void FormVIPCardPayment_Load(object sender, EventArgs e)
        {
            btnConsume.DisplayColor = btnConsume.BackColor;
            btnConsume.Enabled = false;
            btnConsume.BackColor = ConstantValuePool.DisabledColor;
            this.txtConsumAmount.Text = m_UnPaidPrice.ToString("f2");
        }

        private void txtVIPCardNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                m_Card = null;
                Feature.FormNumericKeypad keyForm = new Feature.FormNumericKeypad(false);
                keyForm.DisplayText = "请输入密码";
                keyForm.IsPassword = true;
                keyForm.ShowDialog();
                if (!string.IsNullOrEmpty(keyForm.KeypadValue))
                {
                    string cardNo = txtVIPCardNo.Text.Trim();
                    VIPCard card = new VIPCard();
                    VIPCardService service = new VIPCardService();
                    int result = service.SearchVIPCard(cardNo, keyForm.KeypadValue, ref card);
                    if (result == 1)
                    {
                        if (card.Status == 0)
                        {
                            MessageBox.Show("该卡号未开卡，请先开卡！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        txtName.Text = card.Name;
                        txtBalance.Text = card.Balance.ToString("f");
                        txtIntegral.Text = card.Integral.ToString();
                        txtDiscount.Text = Convert.ToString((1M - card.DiscountRate) * 10M) + "折";
                        txtTelephone.Text = card.Telephone;
                        txtAddress.Text = card.Address;
                        if (card.Status == 1)
                        {
                            txtCardStatus.Text = "启用";
                        }
                        if (card.Status == 2)
                        {
                            txtCardStatus.Text = "锁卡";
                        }
                        if (card.Status == 3)
                        {
                            txtCardStatus.Text = "挂失";
                        }
                        if (card.Status == 4)
                        {
                            txtCardStatus.Text = "作废";
                        }
                        txtOpenCardTime.Text = card.OpenCardTime.ToString("yyyy-MM-dd HH:mm:ss");
                        if (card.LastConsumeTime != null)
                        {
                            txtLastConsumeTime.Text = Convert.ToDateTime(card.LastConsumeTime).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        m_Card = card;
                        if (card.Balance == 0M)
                        {
                            btnConsume.Enabled = false;
                            btnConsume.BackColor = ConstantValuePool.DisabledColor;
                        }
                        else
                        {
                            btnConsume.Enabled = true;
                            btnConsume.BackColor = btnConsume.DisplayColor;
                        }
                    }
                    else if (result == 2)
                    {
                        MessageBox.Show("您输入的会员卡号或者密码错误！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("服务器出现错误，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void btnConsume_Click(object sender, EventArgs e)
        {
            if (m_Card == null || string.IsNullOrEmpty(m_Card.CardNo))
            {
                MessageBox.Show("请重新输入您的会员卡号！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (m_Card.Balance < m_UnPaidPrice)
            {
                if (DialogResult.Yes == MessageBox.Show("卡内余额不足，是否继续使用余额支付？", "信息提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    m_CardPaidAmount = m_Card.Balance;
                    m_CardNo = m_Card.CardNo;
                }
                else
                {
                    m_CardPaidAmount = 0M;
                }
            }
            else
            {
                m_CardPaidAmount = m_UnPaidPrice;
                m_CardNo = m_Card.CardNo;
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_CardPaidAmount = 0M;
            this.Close();
        }
    }
}
