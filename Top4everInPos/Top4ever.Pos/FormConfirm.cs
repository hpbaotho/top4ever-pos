using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.Domain.OrderRelated;
using Top4ever.Entity;
using Top4ever.Entity.Enum;

namespace VechsoftPos
{
    public partial class FormConfirm : Form
    {
        public FormConfirm(decimal actualPayMoney, decimal needChangePay, List<OrderPayoff> orderPayoffList)
        {
            InitializeComponent();
            this.lbReceAmount.Text = actualPayMoney.ToString("f2");
            this.lbNeedChangePay.Text = needChangePay.ToString("f2");
            //支付方式
            decimal paidInMoney = 0M;
            foreach (OrderPayoff item in orderPayoffList)
            {
                string strPayoffWay = string.Empty;
                decimal totalPrice = item.Quantity * item.AsPay;
                if (item.PayoffType == (int)PayoffWayMode.GiftVoucher || item.PayoffType == (int)PayoffWayMode.Coupon)
                {
                    strPayoffWay = string.Format("{0} : {1} 张(合 {2} 元)", item.PayoffName, item.Quantity, totalPrice.ToString("f2"));
                }
                else
                {
                    strPayoffWay = string.Format("{0} : {1} 元", item.PayoffName, totalPrice.ToString("f2"));
                }
                this.txtPayoffWay.AppendText(strPayoffWay + "\r\n");
                paidInMoney += totalPrice;
            }
            this.lbPaidInAmount.Text = paidInMoney.ToString("f2");
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            //更新第二屏信息
            if (Screen.AllScreens.Length > 1 && ConstantValuePool.BizSettingConfig.SecondScreenEnabled)
            {
                if (ConstantValuePool.SecondScreenForm != null && ConstantValuePool.SecondScreenForm is FormSecondScreen)
                {
                    ((FormSecondScreen)ConstantValuePool.SecondScreenForm).InitGoodsOrderInfo();
                }
            }
            this.Close();
        }
    }
}
