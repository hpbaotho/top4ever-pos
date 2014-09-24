using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Domain.MembershipCard;
using Top4ever.Domain.Transfer;

namespace Top4ever.Pos.Membership
{
    public partial class FormVIPCardTradeRecord : Form
    {
        public FormVIPCardTradeRecord()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string cardNo = txtCardNo.Text.Trim();
            if (string.IsNullOrEmpty(cardNo))
            {
                MessageBox.Show("请输入您的会员卡号！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string beginDate = dtBeginDate.Value.ToString("yyyy-MM-dd");
            string endDate = dtEndDate.Value.ToString("yyyy-MM-dd");
            VIPCardTradeRecord cardTradeRecord = null;
            Int32 result = VIPCardTradeService.GetInstance().GetVIPCardTradeList(cardNo, beginDate, endDate, ref cardTradeRecord);
            if (result == 1 && cardTradeRecord != null)
            {
                txtBalance.Text = cardTradeRecord.Balance.ToString("f2");
                txtDiscount.Text = cardTradeRecord.DiscountRate.ToString("f2");
                txtIntegral.Text = cardTradeRecord.Integral.ToString();
                if (cardTradeRecord.VIPCardTradeList != null && cardTradeRecord.VIPCardTradeList.Count > 0)
                {
                    dataGirdViewExt1.Rows.Clear();
                    foreach (VIPCardTrade item in cardTradeRecord.VIPCardTradeList)
                    {
                        int index = dataGirdViewExt1.Rows.Add();
                        dataGirdViewExt1.Rows[index].Cells["colCardNo"].Value = item.CardNo;
                        dataGirdViewExt1.Rows[index].Cells["colTradePayNo"].Value = item.TradePayNo;
                        string tradeType = " ";
                        if (item.TradeType == 1)
                        {
                            tradeType += "储值";
                            dataGirdViewExt1.Rows[index].Cells["colTradeType"].Style.ForeColor = Color.Green;
                        }
                        if (item.TradeType == 2)
                        {
                            tradeType += "储值赠送";
                            dataGirdViewExt1.Rows[index].Cells["colTradeType"].Style.ForeColor = Color.Green;
                        }
                        if (item.TradeType == 3)
                        {
                            tradeType += "消费";
                            dataGirdViewExt1.Rows[index].Cells["colTradeType"].Style.ForeColor = Color.Tomato;
                        }
                        dataGirdViewExt1.Rows[index].Cells["colTradeType"].Value = tradeType;
                        dataGirdViewExt1.Rows[index].Cells["colTradeAmount"].Value = item.TradeAmount;
                        dataGirdViewExt1.Rows[index].Cells["colTradeIntegral"].Value = item.TradeIntegral;
                        dataGirdViewExt1.Rows[index].Cells["colTradeTime"].Value = item.TradeTime.ToString("yyyy-MM-dd HH:mm");
                        dataGirdViewExt1.Rows[index].Cells["colOrderNo"].Value = item.OrderNo;
                        dataGirdViewExt1.Rows[index].Cells["colEmployeeNo"].Value = item.EmployeeNo;
                    }
                }
                else
                {
                    dataGirdViewExt1.Rows.Clear();
                    txtCardNo.Text = string.Empty;
                    MessageBox.Show("找不到该卡号的交易记录！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (result == 0)
            {
                dataGirdViewExt1.Rows.Clear();
                txtCardNo.Text = string.Empty;
                MessageBox.Show("该卡未开通！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (result == 2)
            {
                dataGirdViewExt1.Rows.Clear();
                txtCardNo.Text = string.Empty;
                MessageBox.Show("该卡已挂失！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (result == 3)
            {
                dataGirdViewExt1.Rows.Clear();
                txtCardNo.Text = string.Empty;
                MessageBox.Show("该卡已锁卡！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (result == 4)
            {
                dataGirdViewExt1.Rows.Clear();
                txtCardNo.Text = string.Empty;
                MessageBox.Show("该卡已作废！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
