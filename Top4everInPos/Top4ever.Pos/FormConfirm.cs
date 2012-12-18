using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.Domain.OrderRelated;
using Top4ever.Entity.Enum;

namespace Top4ever.Pos
{
    public partial class FormConfirm : Form
    {
        public FormConfirm(decimal actualPayMoney, decimal paidInMoney, decimal needChangePay, List<OrderPayoff> orderPayoffList)
        {
            InitializeComponent();
            this.txtReceMoney.Text = actualPayMoney.ToString("f2");
            this.txtPaidInMoney.Text = paidInMoney.ToString("f2");
            this.txtNeedChangePay.Text = needChangePay.ToString("f2");
            //支付方式
            string strPayoffWay = string.Empty;
            foreach (OrderPayoff item in orderPayoffList)
            {
                decimal totalPrice = item.Quantity * item.AsPay;
                string singlePay = string.Empty;
                if (item.PayoffType == (int)PayoffWayMode.GiftVoucher)
                {
                    singlePay = string.Format("{0} : {1} 张(合 {2} 元)", item.PayoffName, item.Quantity.ToString("f2"), totalPrice.ToString("f2"));
                }
                else
                {
                    singlePay = string.Format("{0} : {1} 元", item.PayoffName, totalPrice.ToString("f2"));
                }
                strPayoffWay = strPayoffWay + "," + singlePay;
            }
            if (!string.IsNullOrEmpty(strPayoffWay))
            {
                this.txtPayoffWay.Text = strPayoffWay.Substring(1);
            }
            else
            {
                this.txtPayoffWay.Text = string.Empty;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
