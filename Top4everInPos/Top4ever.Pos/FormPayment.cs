using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Top4ever.Entity;
using Top4ever.Domain.OrderRelated;
using Top4ever.CustomControl;
using Top4ever.Domain;
using Top4ever.Common;
using Top4ever.Entity.Enum;
using Top4ever.Domain.Transfer;
using Top4ever.ClientService;
using Top4ever.LocalService.Entity;
using Top4ever.LocalService;
using Top4ever.Print.Entity;
using Top4ever.Print;

namespace VechsoftPos
{
    public partial class FormPayment : Form
    {
        private decimal m_TotalPrice = 0;
        /// <summary>
        /// 应收金额
        /// </summary>
        private decimal m_ActualPayMoney = 0;
        private decimal m_Discount = 0;
        private decimal m_CutOff = 0;
        private SalesOrder m_SalesOrder = null;
        private const int m_Space = 5;
        private int m_Width = 0;
        private int m_Height = 0;
        private int m_ColumnsCount = 0;
        private int m_RowsCount = 0;
        private int m_PageSize = 0;
        private int m_PageIndex = 0;
        private List<CrystalButton> payoffButtonList = new List<CrystalButton>();
        /// <summary>
        /// 当前选中的付款方式
        /// </summary>
        private PayoffWay curPayoffWay = null;
        /// <summary>
        /// 已付款方式
        /// </summary>
        private Dictionary<string, OrderPayoff> dic = new Dictionary<string, OrderPayoff>();
        /// <summary>
        /// 键盘输入的值
        /// </summary>
        private string m_InputNumber = "0";
        /// <summary>
        /// 实收金额
        /// </summary>
        private decimal m_PaidInAmount = 0;

        private bool m_IsPaidOrder = false;
        public bool IsPaidOrder
        {
            get { return m_IsPaidOrder; }
        }

        public FormPayment(SalesOrder salesOrder)
        {
            this.m_TotalPrice = salesOrder.order.TotalSellPrice;
            this.m_ActualPayMoney = salesOrder.order.ActualSellPrice;
            this.m_Discount = salesOrder.order.DiscountPrice;
            this.m_CutOff = salesOrder.order.CutOffPrice;
            this.m_SalesOrder = salesOrder;
            InitializeComponent();
            this.lbTotalPrice.Text = "总金额：" + m_TotalPrice.ToString("f2");
            this.lbDiscount.Text = "折扣：" + m_Discount.ToString("f2");
            this.lbReceMoney.Text = "应收金额：" + m_ActualPayMoney.ToString("f2");
            this.lbPaidInMoney.Text = "实收金额：0.00";
            this.lbUnpaidAmount.Text = "未付金额：0.00";
            this.lbNeedChangePay.Text = "找零：0.00";
        }

        private void FormPayment_Load(object sender, EventArgs e)
        {
            CalculateButtonSize();
            GetPayoffButton();
            DisplayPayoffButton();
            if (!RightsItemCode.FindRights(RightsItemCode.PAYMENT))
            {
                btnCheckOut.Enabled = false;
                btnCheckOut.BackColor = ConstantValuePool.DisabledColor;
            }
        }

        private void CalculateButtonSize()
        {
            if (ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls != null && ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls.Count > 0)
            {
                foreach (BizControl control in ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls)
                {
                    if (control.Name == "Payoff")
                    {
                        m_ColumnsCount = control.ColumnsCount;
                        m_RowsCount = control.RowsCount;
                        m_Width = (this.pnlPayoffWay.Width - m_Space * (control.ColumnsCount + 1)) / control.ColumnsCount;
                        m_Height = (this.pnlPayoffWay.Height - m_Space * (control.RowsCount + 1)) / control.RowsCount;
                        m_PageSize = control.ColumnsCount * control.RowsCount - 2;    //扣除向前、向后两个按钮
                    }
                }
            }
        }

        private void GetPayoffButton()
        {
            foreach (PayoffWay payoff in ConstantValuePool.PayoffWayList)
            {
                CrystalButton btn = new CrystalButton();
                btn.Name = payoff.PayoffID.ToString();
                btn.Text = payoff.PayoffName;
                btn.Width = m_Width;
                btn.Height = m_Height;
                btn.BackColor = btn.DisplayColor = Color.Blue;
                btn.Font = new Font("Microsoft YaHei", 12F, FontStyle.Regular);
                btn.ForeColor = Color.White;
                foreach (ButtonStyle btnStyle in ConstantValuePool.ButtonStyleList)
                {
                    if (payoff.ButtonStyleID.Equals(btnStyle.ButtonStyleID))
                    {
                        float emSize = (float)btnStyle.FontSize;
                        FontStyle style = FontStyle.Regular;
                        btn.Font = new Font(btnStyle.FontName, emSize, style);
                        btn.ForeColor = ColorConvert.RGB(btnStyle.ForeColor);
                        btn.BackColor = btn.DisplayColor = ColorConvert.RGB(btnStyle.BackColor);
                        break;
                    }
                }
                btn.Tag = payoff;
                btn.Click += new System.EventHandler(this.btnPayoff_Click);
                payoffButtonList.Add(btn);
            }
        }

        private void DisplayPayoffButton()
        {
            this.pnlPayoffWay.Controls.Clear();
            int startIndex = m_PageIndex * m_PageSize;
            int endIndex = (m_PageIndex + 1) * m_PageSize;
            if (payoffButtonList.Count < endIndex)
            {
                endIndex = payoffButtonList.Count;
            }
            int index = 1;
            int px = m_Space, py = m_Space;
            for (int i = startIndex; i < endIndex; i++)
            {
                CrystalButton btn = payoffButtonList[i];
                btn.Location = new Point(px, py);
                this.pnlPayoffWay.Controls.Add(btn);
                if (index % m_ColumnsCount == 0)
                {
                    px = m_Space;
                    py += m_Height + m_Space;
                }
                else
                {
                    px += m_Width + m_Space;
                }
                index++;
            }
            px = (m_ColumnsCount - 2) * m_Width + (m_ColumnsCount - 2 + 1) * m_Space;
            py = (m_RowsCount - 1) * m_Height + (m_RowsCount - 1 + 1) * m_Space;
            btnPageUp.Width = m_Width;
            btnPageUp.Height = m_Height;
            btnPageUp.Location = new Point(px, py);
            if (m_PageIndex <= 0)
            {
                btnPageUp.Enabled = false;
                btnPageUp.BackColor = ConstantValuePool.DisabledColor;
            }
            else
            {
                btnPageUp.Enabled = true;
                btnPageUp.BackColor = btnPageUp.DisplayColor;
            }
            this.pnlPayoffWay.Controls.Add(btnPageUp);
            px = (m_ColumnsCount - 1) * m_Width + (m_ColumnsCount - 1 + 1) * m_Space;
            py = (m_RowsCount - 1) * m_Height + (m_RowsCount - 1 + 1) * m_Space;
            btnPageDown.Width = m_Width;
            btnPageDown.Height = m_Height;
            btnPageDown.Location = new Point(px, py);
            if ((m_PageIndex + 1) * m_PageSize >= payoffButtonList.Count)
            {
                btnPageDown.Enabled = false;
                btnPageDown.BackColor = ConstantValuePool.DisabledColor;
            }
            else
            {
                btnPageDown.Enabled = true;
                btnPageDown.BackColor = btnPageDown.DisplayColor;
            }
            this.pnlPayoffWay.Controls.Add(btnPageDown);
        }

        private void btnPayoff_Click(object sender, EventArgs e)
        {
            foreach (CrystalButton button in payoffButtonList)
            {
                PayoffWay temp = button.Tag as PayoffWay;
                if (dic.ContainsKey(temp.PayoffID.ToString()))
                {
                    button.BackColor = ConstantValuePool.PressedColor;
                }
                else
                {
                    button.BackColor = button.DisplayColor;
                }
            }
            CrystalButton btn = sender as CrystalButton;
            btn.BackColor = ConstantValuePool.PressedColor;
            curPayoffWay = btn.Tag as PayoffWay;
            this.txtPayoff.Text = string.Format("{0}(1:{1})", curPayoffWay.PayoffName, curPayoffWay.AsPay.ToString("f2"));
            if (dic.ContainsKey(curPayoffWay.PayoffID.ToString()))
            {
                OrderPayoff orderPayoff = dic[curPayoffWay.PayoffID.ToString()];
                decimal totalPrice = orderPayoff.Quantity * orderPayoff.AsPay;
                if (curPayoffWay.PayoffType == (int)PayoffWayMode.GiftVoucher || curPayoffWay.PayoffType == (int)PayoffWayMode.Coupon)
                {
                    this.txtAmount.Text = string.Format("{0} 张(合 {1} 元)", orderPayoff.Quantity, totalPrice.ToString("f2"));
                }
                else
                {
                    this.txtAmount.Text = string.Format("{0} 元", totalPrice.ToString("f2"));
                }
            }
            else
            {
                if (curPayoffWay.PayoffType == (int)PayoffWayMode.GiftVoucher || curPayoffWay.PayoffType == (int)PayoffWayMode.Coupon)
                {
                    this.txtAmount.Text = string.Format("{0} 张(合 {1} 元)", "0", "0.00");
                }
                else
                {
                    this.txtAmount.Text = string.Format("{0} 元", "0.00");
                }
                if (curPayoffWay.PayoffType == (int)PayoffWayMode.MembershipCard)
                {
                    decimal unPaidPrice = decimal.Parse(lbUnpaidAmount.Text);
                    if (unPaidPrice > 0)
                    {
                        Membership.FormVIPCardPayment formCardPayment = new Membership.FormVIPCardPayment(unPaidPrice);
                        formCardPayment.ShowDialog();
                        if (formCardPayment.CardPaidAmount > 0)
                        {
                            OrderPayoff orderPayoff = new OrderPayoff();
                            orderPayoff.OrderPayoffID = Guid.NewGuid();
                            orderPayoff.OrderID = m_SalesOrder.order.OrderID;
                            orderPayoff.PayoffID = curPayoffWay.PayoffID;
                            orderPayoff.PayoffName = curPayoffWay.PayoffName;
                            orderPayoff.PayoffType = curPayoffWay.PayoffType;
                            orderPayoff.AsPay = curPayoffWay.AsPay;
                            orderPayoff.Quantity = formCardPayment.CardPaidAmount / curPayoffWay.AsPay;
                            orderPayoff.CardNo = formCardPayment.CardNo;
                            orderPayoff.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
                            dic.Add(curPayoffWay.PayoffID.ToString(), orderPayoff);
                        }
                        this.txtAmount.Text = string.Format("{0} 元", formCardPayment.CardPaidAmount.ToString("f2"));
                        DisplayPayoffWay();
                        //屏蔽数字键盘
                    }
                }
            }
            m_InputNumber = "0";
        }

        private void DisplayPayoffWay()
        {
            this.dgvPayment.Rows.Clear();
            decimal realPay = 0;
            if (dic.Count > 0)
            {
                foreach (KeyValuePair<string, OrderPayoff> item in dic)
                {
                    if (item.Value.Quantity > 0)
                    {
                        decimal totalPrice = item.Value.Quantity * item.Value.AsPay;
                        realPay += totalPrice;

                        int index = dgvPayment.Rows.Add();
                        dgvPayment.Rows[index].Cells["PayoffName"].Value = item.Value.PayoffName;
                        if (item.Value.PayoffType == (int)PayoffWayMode.GiftVoucher || item.Value.PayoffType == (int)PayoffWayMode.Coupon)
                        {
                            dgvPayment.Rows[index].Cells["ItemNum"].Value = item.Value.Quantity.ToString("f0");
                        }
                        else
                        {
                            dgvPayment.Rows[index].Cells["ItemNum"].Value = "/";
                        }
                        dgvPayment.Rows[index].Cells["GoodsPrice"].Value = totalPrice;
                        //设置样式
                        this.dgvPayment.CurrentRow.Selected = false;
                    }
                }
                if (this.dgvPayment.Rows.Count > 0)
                {
                    this.dgvPayment.Rows[this.dgvPayment.RowCount - 1].Selected = true;
                }
            }
            m_PaidInAmount = realPay;
            this.lbPaidInMoney.Text = "实收金额：" + realPay.ToString("f2");
            decimal unPaidPrice = m_ActualPayMoney - realPay;
            if (unPaidPrice > 0)
            {
                this.lbUnpaidAmount.Text = "未付金额：" + unPaidPrice.ToString("f2");
            }
            else
            {
                this.lbUnpaidAmount.Text = "未付金额：0.00";
                //计算找零金额
                decimal cash = 0, noCash = 0;
                List<OrderPayoff> tempPayoffList = new List<OrderPayoff>();
                foreach (KeyValuePair<string, OrderPayoff> item in dic)
                {
                    if (item.Value.PayoffType == (int)PayoffWayMode.Cash)
                    {
                        cash += item.Value.AsPay * item.Value.Quantity;
                    }
                    else
                    {
                        noCash += item.Value.AsPay * item.Value.Quantity;
                    }
                    tempPayoffList.Add(item.Value);
                }
                if (noCash > m_ActualPayMoney)
                {
                    this.lbNeedChangePay.Text = "找零：" + cash.ToString("f2");
                }
                else
                {
                    this.lbNeedChangePay.Text = "找零：" + (m_PaidInAmount - m_ActualPayMoney).ToString("f2");
                }
            }
        }

        private void btnPageUp_Click(object sender, EventArgs e)
        {
            m_PageIndex--;
            DisplayPayoffButton();
            if (m_PageIndex <= 0)
            {
                m_PageIndex = 0;
                btnPageUp.Enabled = false;
            }
            if (payoffButtonList.Count > (m_PageIndex + 1) * m_PageSize)
            {
                btnPageDown.Enabled = true;
            }
        }

        private void btnPageDown_Click(object sender, EventArgs e)
        {
            m_PageIndex++;
            DisplayPayoffButton();
            if (m_PageIndex > 0)
            {
                btnPageUp.Enabled = true;
            }
            if (payoffButtonList.Count <= (m_PageIndex + 1) * m_PageSize)
            {
                btnPageDown.Enabled = false;
            }
        }

        private void btnReal_Click(object sender, EventArgs e)
        {
            if (curPayoffWay != null && m_ActualPayMoney > 0)
            {
                decimal remainNeedPay = m_ActualPayMoney - m_PaidInAmount;
                if (remainNeedPay > 0)
                {
                    decimal ItemQty = Math.Ceiling(remainNeedPay / curPayoffWay.AsPay);
                    if (dic.ContainsKey(curPayoffWay.PayoffID.ToString()))
                    {
                        OrderPayoff orderPayoff = dic[curPayoffWay.PayoffID.ToString()];
                        ItemQty += orderPayoff.Quantity;
                    }
                    m_InputNumber = ItemQty.ToString("f2");
                    CalculateAllPrice();
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (curPayoffWay != null)
            {
                dic.Remove(curPayoffWay.PayoffID.ToString());
                if (curPayoffWay.PayoffType == (int)PayoffWayMode.GiftVoucher || curPayoffWay.PayoffType == (int)PayoffWayMode.Coupon)
                {
                    this.txtAmount.Text = string.Format("{0} 张(合 {1} 元)", "0", "0.00");
                }
                else
                {
                    this.txtAmount.Text = string.Format("{0} 元", "0.00");
                }
                DisplayPayoffWay();
                m_InputNumber = "0";
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (curPayoffWay != null)
            {
                CrystalButton btn = sender as CrystalButton;
                if (m_InputNumber == "0" && !btn.Text.Equals("."))
                {
                    m_InputNumber = btn.Text;
                }
                else
                {
                    //避免出现两个“.”
                    if (btn.Text == "." && m_InputNumber.IndexOf('.') > 0)
                    {
                        return;
                    }
                    //现金只能有两位小数
                    if (btn.Text != "." && m_InputNumber.IndexOf('.') > 0 && m_InputNumber.Substring(m_InputNumber.IndexOf('.') + 1).Length >= 2)
                    {
                        return;
                    }
                    m_InputNumber += btn.Text;
                }
                if (!m_InputNumber.EndsWith("."))
                {
                    CalculateAllPrice();
                }
            }
        }

        private void btnAddBigNumber_Click(object sender, EventArgs e)
        {
            if (curPayoffWay != null)
            {
                CrystalButton btn = sender as CrystalButton;
                string bigNumber = btn.Text.Substring(1);
                if (m_InputNumber.IndexOf('.') > 0)
                {
                    if (m_InputNumber.EndsWith("."))
                    {
                        int count = Convert.ToInt32(m_InputNumber.Substring(0, m_InputNumber.Length - 1)) + Convert.ToInt32(bigNumber);
                        m_InputNumber = count.ToString();
                    }
                    else
                    {
                        decimal count = Convert.ToDecimal(m_InputNumber) + Convert.ToDecimal(bigNumber);
                        m_InputNumber = count.ToString("f2");
                    }
                }
                else
                {
                    int count = Convert.ToInt32(m_InputNumber) + Convert.ToInt32(bigNumber);
                    m_InputNumber = count.ToString();
                }
                CalculateAllPrice();
            }
        }

        private void CalculateAllPrice()
        {
            if (dic.ContainsKey(curPayoffWay.PayoffID.ToString()))
            {
                OrderPayoff orderPayoff = dic[curPayoffWay.PayoffID.ToString()];
                orderPayoff.Quantity = Convert.ToDecimal(m_InputNumber);
            }
            else
            {
                OrderPayoff orderPayoff = new OrderPayoff();
                orderPayoff.OrderPayoffID = Guid.NewGuid();
                orderPayoff.OrderID = m_SalesOrder.order.OrderID;
                orderPayoff.PayoffID = curPayoffWay.PayoffID;
                orderPayoff.PayoffName = curPayoffWay.PayoffName;
                orderPayoff.PayoffType = curPayoffWay.PayoffType;
                orderPayoff.AsPay = curPayoffWay.AsPay;
                orderPayoff.Quantity = Convert.ToDecimal(m_InputNumber);
                orderPayoff.CardNo = "";
                orderPayoff.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
                dic.Add(curPayoffWay.PayoffID.ToString(), orderPayoff);
            }
            decimal totalPrice = Convert.ToDecimal(m_InputNumber) * curPayoffWay.AsPay;
            if (curPayoffWay.PayoffType == (int)PayoffWayMode.GiftVoucher || curPayoffWay.PayoffType == (int)PayoffWayMode.Coupon)
            {
                this.txtAmount.Text = string.Format("{0} 张(合 {1} 元)", m_InputNumber, totalPrice.ToString("f2"));
            }
            else
            {
                this.txtAmount.Text = string.Format("{0} 元", totalPrice.ToString("f2"));
            }
            DisplayPayoffWay();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            if (m_ActualPayMoney > m_PaidInAmount)
            {
                // 支付的金额不足
                MessageBox.Show("支付的金额不足，请确认后重新付款！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (m_PaidInAmount > m_ActualPayMoney)
            {
                decimal cash = 0, noCash = 0;
                List<OrderPayoff> tempPayoffList = new List<OrderPayoff>();
                foreach (KeyValuePair<string, OrderPayoff> item in dic)
                {
                    if (item.Value.PayoffType == (int)PayoffWayMode.Cash)
                    {
                        cash += item.Value.AsPay * item.Value.Quantity;
                    }
                    else
                    {
                        noCash += item.Value.AsPay * item.Value.Quantity;
                    }
                    tempPayoffList.Add(item.Value);
                }
                if (noCash > m_ActualPayMoney)
                {
                    List<OrderPayoff> cashPayoff = new List<OrderPayoff>();
                    List<OrderPayoff> noCashPayoff = new List<OrderPayoff>();
                    for (int index = 0; index < tempPayoffList.Count; index++)
                    {
                        if (tempPayoffList[index].PayoffType == (int)PayoffWayMode.Cash)
                        {
                            cashPayoff.Add(tempPayoffList[index]);
                        }
                        else
                        {
                            noCashPayoff.Add(tempPayoffList[index]);
                        }
                    }
                    //非现金支付方式按单位价值从高到低排序
                    OrderPayoff[] noCashPayoffArr = noCashPayoff.ToArray();
                    for (int j = 0; j < noCashPayoffArr.Length; j++)
                    {
                        for (int i = noCashPayoffArr.Length - 1; i > j; i--)
                        {
                            if (noCashPayoffArr[j].AsPay < noCashPayoffArr[i].AsPay)
                            {
                                OrderPayoff temp = noCashPayoffArr[j];
                                noCashPayoffArr[j] = noCashPayoffArr[i];
                                noCashPayoffArr[i] = temp;
                            }
                        }
                    }
                    if (noCash - noCashPayoffArr[noCashPayoffArr.Length - 1].AsPay < m_ActualPayMoney)
                    {
                        if (cashPayoff.Count > 0)
                        {
                            MessageBox.Show("现金支付方式多余！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("非现金支付方式金额过多，请重新支付！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    //由全部非现金及部分现金来支付
                    decimal changePay = m_PaidInAmount - m_ActualPayMoney;
                    foreach (KeyValuePair<string, OrderPayoff> item in dic)
                    {
                        if (item.Value.PayoffType == (int)PayoffWayMode.Cash)
                        {
                            item.Value.NeedChangePay = changePay;
                            break;
                        }
                    }
                }
            }
            //计算支付的金额并填充OrderPayoff
            bool IsContainsVIPCard = false;
            decimal paymentMoney = 0;
            decimal needChangePay = 0;
            List<OrderPayoff> orderPayoffList = new List<OrderPayoff>();
            foreach (KeyValuePair<string, OrderPayoff> item in dic)
            {
                if (item.Value.Quantity > 0)
                {
                    if (item.Value.PayoffType == (int)PayoffWayMode.MembershipCard)
                    {
                        IsContainsVIPCard = true;
                    }
                    OrderPayoff orderPayoff = item.Value;
                    paymentMoney += orderPayoff.AsPay * orderPayoff.Quantity;
                    needChangePay += orderPayoff.NeedChangePay;
                    orderPayoffList.Add(orderPayoff);
                }
            }
            bool result = false;
            if (IsContainsVIPCard)
            {
                Dictionary<string, VIPCardPayment> dicCardPayment = new Dictionary<string, VIPCardPayment>();
                Dictionary<string, string> dicCardTradePayNo = new Dictionary<string, string>();
                if (IsVIPCardPaySuccess(ref dicCardPayment, ref dicCardTradePayNo))
                {
                    string strTradePayNo = string.Empty;
                    foreach (KeyValuePair<string, string> item in dicCardTradePayNo)
                    {
                        strTradePayNo += "," + item.Value;
                    }
                    strTradePayNo = strTradePayNo.Substring(1);
                    result = PayForOrder(orderPayoffList, paymentMoney, needChangePay, strTradePayNo);
                    if (!result)
                    {
                        //取消会员卡支付
                        foreach (KeyValuePair<string, VIPCardPayment> item in dicCardPayment)
                        {
                            //将支付成功的会员卡取消支付
                            int returnValue = VIPCardTradeService.GetInstance().RefundVIPCardPayment(item.Value.CardNo, dicCardTradePayNo[item.Value.CardNo]);
                            if (returnValue == 0)
                            {
                                string cardNo = item.Value.CardNo;
                                CardRefundPay cardRefundPay = new CardRefundPay();
                                cardRefundPay.CardNo = cardNo;
                                cardRefundPay.ShopID = ConstantValuePool.CurrentShop.ShopID.ToString();
                                cardRefundPay.TradePayNo = dicCardTradePayNo[cardNo];
                                cardRefundPay.PayAmount = item.Value.PayAmount;
                                cardRefundPay.EmployeeNo = item.Value.EmployeeNo;
                                cardRefundPay.DeviceNo = item.Value.DeviceNo;
                                CardRefundPayService refundPayService = new CardRefundPayService();
                                refundPayService.AddRefundPayInfo(cardRefundPay);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("会员卡支付操作失败，请稍后再试！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                result = PayForOrder(orderPayoffList, paymentMoney, needChangePay, string.Empty);
            }
            if (result)
            {
                //打印小票
                PrintData printData = new PrintData();
                printData.ShopName = ConstantValuePool.CurrentShop.ShopName;
                printData.DeskName = m_SalesOrder.order.DeskName;
                printData.PersonNum = m_SalesOrder.order.PeopleNum.ToString();
                printData.PrintTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                printData.EmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                printData.TranSequence = m_SalesOrder.order.TranSequence.ToString();
                printData.ShopAddress = ConstantValuePool.CurrentShop.RunAddress;
                printData.Telephone = ConstantValuePool.CurrentShop.Telephone;
                printData.ReceivableMoney = m_ActualPayMoney.ToString("f2");
                printData.ServiceFee = "0.00";
                printData.PaidInMoney = paymentMoney.ToString("f2");
                printData.NeedChangePay = needChangePay.ToString("f2");
                printData.GoodsOrderList = new List<GoodsOrder>();
                printData.PayingOrderList = new List<PayingGoodsOrder>();
                foreach (OrderDetails item in m_SalesOrder.orderDetailsList)
                {
                    string strLevelFlag = string.Empty;
                    int levelCount = item.ItemLevel * 2;
                    for (int i = 0; i < levelCount; i++)
                    {
                        strLevelFlag += "-";
                    }
                    GoodsOrder goodsOrder = new GoodsOrder();
                    goodsOrder.GoodsName = strLevelFlag + item.GoodsName;
                    goodsOrder.GoodsNum = item.ItemQty.ToString("f1");
                    goodsOrder.SellPrice = item.SellPrice.ToString("f2");
                    goodsOrder.TotalSellPrice = item.TotalSellPrice.ToString("f2");
                    goodsOrder.TotalDiscount = item.TotalDiscount.ToString("f2");
                    goodsOrder.Unit = item.Unit;
                    printData.GoodsOrderList.Add(goodsOrder);
                }
                foreach (OrderPayoff orderPayoff in orderPayoffList)
                {
                    PayingGoodsOrder payingOrder = new PayingGoodsOrder();
                    payingOrder.PayoffName = orderPayoff.PayoffName;
                    payingOrder.PayoffMoney = (orderPayoff.AsPay * orderPayoff.Quantity).ToString("f2");
                    payingOrder.NeedChangePay = orderPayoff.NeedChangePay.ToString("f2");
                    printData.PayingOrderList.Add(payingOrder);
                }
                string paperWidth = ConstantValuePool.BizSettingConfig.printConfig.PaperWidth;
                if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.DRIVER)
                {
                    string printerName = ConstantValuePool.BizSettingConfig.printConfig.Name;
                    string paperName = ConstantValuePool.BizSettingConfig.printConfig.PaperName;
                    DriverOrderPrint printer = DriverOrderPrint.GetInstance(printerName, paperName, paperWidth);
                    printer.DoPrintPaidOrder(printData);
                }
                m_IsPaidOrder = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("支付账单失败！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_IsPaidOrder = false;
            this.Close();
        }

        private bool PayForOrder(List<OrderPayoff> orderPayoffList, decimal paymentMoney, decimal needChangePay, string tradePayNo)
        {
            //填充Order
            Order order = new Order();
            order.OrderID = m_SalesOrder.order.OrderID;
            order.TotalSellPrice = m_TotalPrice;
            order.ActualSellPrice = m_ActualPayMoney;
            order.DiscountPrice = m_Discount;
            order.CutOffPrice = m_CutOff;
            order.ServiceFee = 0;
            order.PaymentMoney = paymentMoney;
            order.NeedChangePay = needChangePay;
            order.MembershipCard = string.Empty;
            order.MemberDiscount = 0;
            order.PayEmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
            order.PayEmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
            order.CheckoutDeviceNo = ConstantValuePool.BizSettingConfig.DeviceNo;
            order.TradePayNo = tradePayNo;
            PayingOrder payingOrder = new PayingOrder();
            payingOrder.order = order;
            payingOrder.orderPayoffList = orderPayoffList;
            return PayingOrderService.GetInstance().PayForOrder(payingOrder);
        }

        private bool IsVIPCardPaySuccess(ref Dictionary<string, VIPCardPayment> dicCardPayment, ref Dictionary<string, string> dicCardTradePayNo)
        {
            bool IsSuccess = false;
            //key: cardNo
            dicCardPayment = new Dictionary<string, VIPCardPayment>();
            dicCardTradePayNo = new Dictionary<string, string>();
            foreach (KeyValuePair<string, OrderPayoff> item in dic)
            {
                if (item.Value.Quantity > 0)
                {
                    //会员卡支付
                    if (item.Value.PayoffType == (int)PayoffWayMode.MembershipCard)
                    {
                        VIPCardPayment cardPayment = new VIPCardPayment();
                        cardPayment.CardNo = item.Value.CardNo;
                        cardPayment.PayAmount = item.Value.AsPay * item.Value.Quantity - item.Value.NeedChangePay;
                        cardPayment.PayIntegral = 0;
                        cardPayment.OrderNo = m_SalesOrder.order.OrderNo;
                        cardPayment.EmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                        cardPayment.DeviceNo = ConstantValuePool.BizSettingConfig.DeviceNo;
                        string tradePayNo = string.Empty;
                        int result = VIPCardTradeService.GetInstance().AddVIPCardPayment(cardPayment, out tradePayNo);
                        if (result == 1)
                        {
                            //会员充值成功
                            dicCardPayment.Add(item.Value.CardNo, cardPayment);
                            dicCardTradePayNo.Add(item.Value.CardNo, tradePayNo);
                            IsSuccess = true;
                        }
                        else if (result == 2)
                        {
                            MessageBox.Show(string.Format("卡号'{0}'不存在，请确认输入的卡号是否正确！", item.Value.CardNo), "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            IsSuccess = false;
                            break;
                        }
                        else if (result == 3)
                        {
                            MessageBox.Show(string.Format("卡号'{0}'未开通，请先开卡！", item.Value.CardNo), "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            IsSuccess = false;
                            break;
                        }
                        else if (result == 4)
                        {
                            MessageBox.Show(string.Format("卡号'{0}'已挂失，不能充值！", item.Value.CardNo), "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            IsSuccess = false;
                            break;
                        }
                        else if (result == 5)
                        {
                            MessageBox.Show(string.Format("卡号'{0}'已锁卡，不能充值！", item.Value.CardNo), "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            IsSuccess = false;
                            break;
                        }
                        else if (result == 6)
                        {
                            MessageBox.Show(string.Format("卡号'{0}'已作废，不能充值！", item.Value.CardNo), "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            IsSuccess = false;
                            break;
                        }
                        else if (result == 7)
                        {
                            MessageBox.Show(string.Format("卡号'{0}'所属会员组没有储值功能！", item.Value.CardNo), "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            IsSuccess = false;
                            break;
                        }
                        else
                        {
                            MessageBox.Show("服务器出现错误，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            IsSuccess = false;
                            break;
                        }
                    }
                }
            }
            if (IsSuccess)
            {
                return true;
            }
            else
            {
                if (dicCardPayment.Count > 0)
                {
                    //取消会员卡支付
                    foreach (KeyValuePair<string, VIPCardPayment> item in dicCardPayment)
                    {
                        //将支付成功的会员卡取消支付
                        int returnValue = VIPCardTradeService.GetInstance().RefundVIPCardPayment(item.Value.CardNo, dicCardTradePayNo[item.Value.CardNo]);
                        if (returnValue == 0)
                        {
                            string cardNo = item.Value.CardNo;
                            CardRefundPay cardRefundPay = new CardRefundPay();
                            cardRefundPay.CardNo = cardNo;
                            cardRefundPay.ShopID = ConstantValuePool.CurrentShop.ShopID.ToString();
                            cardRefundPay.TradePayNo = dicCardTradePayNo[cardNo];
                            cardRefundPay.PayAmount = item.Value.PayAmount;
                            cardRefundPay.EmployeeNo = item.Value.EmployeeNo;
                            cardRefundPay.DeviceNo = item.Value.DeviceNo;
                            CardRefundPayService refundPayService = new CardRefundPayService();
                            refundPayService.AddRefundPayInfo(cardRefundPay);
                        }
                    }
                }
                return false;
            }
        }
    }
}
