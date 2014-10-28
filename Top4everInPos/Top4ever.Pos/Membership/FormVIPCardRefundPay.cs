using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Top4ever.ClientService;
using Top4ever.LocalService;
using Top4ever.LocalService.Entity;

namespace VechsoftPos.Membership
{
    public partial class FormVIPCardRefundPay : Form
    {
        private List<CardRefundPay> m_CardRefundPayList;

        public FormVIPCardRefundPay(List<CardRefundPay> cardRefundPayList)
        {
            m_CardRefundPayList = cardRefundPayList;
            InitializeComponent();
        }

        private void FormVIPCardRefundPay_Load(object sender, EventArgs e)
        {
            BindCardRefundPay(m_CardRefundPayList);
        }

        private void BindCardRefundPay(List<CardRefundPay> cardRefundPayList)
        {
            this.dataGirdViewExt1.Rows.Clear();
            foreach (CardRefundPay item in cardRefundPayList)
            {
                int index = this.dataGirdViewExt1.Rows.Add();
                this.dataGirdViewExt1.Rows[index].Cells["colCheck"].Value = false;
                this.dataGirdViewExt1.Rows[index].Cells["colCardNo"].Value = item.CardNo;
                this.dataGirdViewExt1.Rows[index].Cells["colTradePayNo"].Value = item.TradePayNo;
                this.dataGirdViewExt1.Rows[index].Cells["colPayAmount"].Value = item.PayAmount;
                this.dataGirdViewExt1.Rows[index].Cells["colDeviceNo"].Value = item.DeviceNo;
                this.dataGirdViewExt1.Rows[index].Cells["colIsFixed"].Value = item.IsFixed ? "是" : "否";
                this.dataGirdViewExt1.Rows[index].Cells["colEmployeeNo"].Value = item.EmployeeNo;
                this.dataGirdViewExt1.Rows[index].Cells["colCreateTime"].Value = item.CreateTime.ToString("yyyy-MM-dd HH:mm");
                this.dataGirdViewExt1.Rows[index].Cells["colStoreValueID"].Value = item.StoreValueID;
            }
        }

        private void btnRefundPay_Click(object sender, EventArgs e)
        {
            bool hasChecked = false;
            foreach (DataGridViewRow row in this.dataGirdViewExt1.Rows)
            {
                hasChecked = Convert.ToBoolean(row.Cells["colCheck"].Value);
                if (hasChecked)
                {
                    break;
                }
            }
            if (!hasChecked)
            {
                MessageBox.Show("请选择需要进行支付退款操作的卡号！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool result = true;
            CardRefundPayService refundPayService = new CardRefundPayService();
            foreach (DataGridViewRow row in this.dataGirdViewExt1.Rows)
            {
                bool isCheck = Convert.ToBoolean(row.Cells["colCheck"].Value);
                if (isCheck)
                {
                    string cardNo = row.Cells["colCardNo"].Value.ToString();
                    string tradePayNo = row.Cells["colTradePayNo"].Value.ToString();
                    string cardPassword;
                    //获取卡密码
                    Feature.FormNumericKeypad keyForm = new Feature.FormNumericKeypad(false);
                    keyForm.DisplayText = string.Format("请输入卡号'{0}'的密码", cardNo);
                    keyForm.IsPassword = true;
                    keyForm.ShowDialog();
                    if (!string.IsNullOrEmpty(keyForm.KeypadValue))
                    {
                        cardPassword = keyForm.KeypadValue;
                    }
                    else
                    {
                        continue;
                    }
                    int returnValue = VIPCardTradeService.GetInstance().RefundVipCardPayment(cardNo, cardPassword, tradePayNo);
                    if (returnValue == 0)
                    {
                        result = false;
                        break;
                    }
                    int storeValueId = int.Parse(row.Cells["colStoreValueID"].Value.ToString());
                    refundPayService.UpdateFixedPayInfo(storeValueId, true);
                }
            }
            if (result)
            {
                List<CardRefundPay> cardRefundPayList = refundPayService.GetCardRefundPayList();
                if(cardRefundPayList == null || cardRefundPayList.Count == 0)
                {
                    this.Close();
                }
                else
                {
                    BindCardRefundPay(cardRefundPayList);
                }
            }
            else
            {
                List<CardRefundPay> cardRefundPayList = refundPayService.GetCardRefundPayList();
                BindCardRefundPay(cardRefundPayList);
                MessageBox.Show("退款过程出现错误，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("您确定是否要退出支付退款管理？", "信息提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                this.Close();
            }
        }
    }
}
