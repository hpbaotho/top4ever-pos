using System;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Domain.MembershipCard;
using Top4ever.Entity;

namespace VechsoftPos.Membership
{
    public partial class FormVIPCardPayment : Form
    {
        private VIPCard _card;
        private readonly decimal _unPaidPrice;

        private decimal _cardPaidAmount;
        public decimal CardPaidAmount
        {
            get { return _cardPaidAmount; }
        }

        private string _cardNo = string.Empty;
        public string CardNo
        {
            get { return _cardNo; }
        }

        private string _cardPassword = string.Empty;
        public string CardPassword
        {
            get { return _cardPassword; }
        }

        public FormVIPCardPayment(decimal unPaidPrice)
        {
            _unPaidPrice = unPaidPrice;
            InitializeComponent();
        }

        private void FormVIPCardPayment_Load(object sender, EventArgs e)
        {
            btnConsume.DisplayColor = btnConsume.BackColor;
            btnConsume.Enabled = false;
            btnConsume.BackColor = ConstantValuePool.DisabledColor;
            this.txtConsumAmount.Text = _unPaidPrice.ToString("f2");
        }

        private void txtVIPCardNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                _card = null;
                Feature.FormNumericKeypad keyForm = new Feature.FormNumericKeypad(false);
                keyForm.DisplayText = "请输入密码";
                keyForm.IsPassword = true;
                keyForm.ShowDialog();
                if (!string.IsNullOrEmpty(keyForm.KeypadValue))
                {
                    string cardNo = txtVIPCardNo.Text.Trim();
                    string cardPassword = keyForm.KeypadValue;
                    VIPCard card;
                    int result = VIPCardService.GetInstance().SearchVIPCard(cardNo, cardPassword, out card);
                    if (card != null && result == 1)
                    {
                        if (card.Status == 0)
                        {
                            MessageBox.Show("该卡未开通，请先开卡！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        txtName.Text = card.Name;
                        txtBalance.Text = card.Balance.ToString("f2");
                        txtIntegral.Text = card.Integral.ToString();
                        txtDiscount.Text = ((1M - card.DiscountRate) * 10M).ToString("f2") + "折";
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
                        _cardNo = cardNo;
                        _cardPassword = cardPassword;
                        _card = card;
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
                        txtVIPCardNo.Text = string.Empty;
                        txtName.Text = string.Empty;
                        txtBalance.Text = string.Empty;
                        txtIntegral.Text = string.Empty;
                        txtDiscount.Text = string.Empty;
                        txtTelephone.Text = string.Empty;
                        txtAddress.Text = string.Empty;
                        txtCardStatus.Text = string.Empty;
                        txtOpenCardTime.Text = string.Empty;
                        txtLastConsumeTime.Text = string.Empty;
                        MessageBox.Show("您输入的会员卡号或者密码错误！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        txtVIPCardNo.Text = string.Empty;
                        txtName.Text = string.Empty;
                        txtBalance.Text = string.Empty;
                        txtIntegral.Text = string.Empty;
                        txtDiscount.Text = string.Empty;
                        txtTelephone.Text = string.Empty;
                        txtAddress.Text = string.Empty;
                        txtCardStatus.Text = string.Empty;
                        txtOpenCardTime.Text = string.Empty;
                        txtLastConsumeTime.Text = string.Empty;
                        MessageBox.Show("服务器出现错误，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnConsume_Click(object sender, EventArgs e)
        {
            if (_card == null || string.IsNullOrEmpty(_card.CardNo))
            {
                MessageBox.Show("请重新输入您的会员卡号！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_card.Balance < _unPaidPrice)
            {
                if (DialogResult.Yes == MessageBox.Show("卡内余额不足，是否继续使用余额支付？", "信息提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    _cardPaidAmount = _card.Balance;
                    _cardNo = _card.CardNo;
                }
                else
                {
                    _cardPaidAmount = 0M;
                    _cardNo = string.Empty;
                }
            }
            else
            {
                _cardPaidAmount = _unPaidPrice;
                _cardNo = _card.CardNo;
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _cardPaidAmount = 0M;
            this.Close();
        }
    }
}
