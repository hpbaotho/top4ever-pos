using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Common;
using Top4ever.CustomControl;
using Top4ever.Domain;
using Top4ever.Domain.GoodsRelated;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;
using Top4ever.Entity.Enum;
using Top4ever.Pos.Feature;
using Top4ever.Print;
using Top4ever.Print.Entity;

namespace Top4ever.Pos
{
    public partial class FormCheckOut : Form
    {
        private const int m_Space = 5;
        private int m_Width = 0;
        private int m_Height = 0;
        private int m_ColumnsCount = 0;
        private int m_RowsCount = 0;
        private int m_PageSize = 0;
        private int m_PageIndex = 0;
        private List<CrystalButton> payoffButtonList = new List<CrystalButton>();
        private SalesOrder m_SalesOrder = null;
        private string m_CurrentDeskName = string.Empty;
        private decimal m_TotalPrice = 0;
        private decimal m_ActualPayMoney = 0;
        private decimal m_Discount = 0;
        private decimal m_CutOff = 0;
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
        /// 前一个按钮
        /// </summary>
        private CrystalButton m_PrePayoffButton = null;
        /// <summary>
        /// 去服务费
        /// </summary>
        private bool m_CutServiceFee = false;
        /// <summary>
        /// 服务费
        /// </summary>
        private decimal m_ServiceFee = 0;

        #region output

        private bool m_IsPaidOrder = false;
        public bool IsPaidOrder
        {
            get { return m_IsPaidOrder; }
        }

        #endregion

        public FormCheckOut(SalesOrder salesOrder, string currentDeskName)
        {
            InitializeComponent();
            m_SalesOrder = salesOrder;
            m_CurrentDeskName = currentDeskName;
        }

        private void FormCheckOut_Load(object sender, EventArgs e)
        {
            CalculateButtonSize();
            GetPayoffButton();
            DisplayPayoffButton();
            BindGoodsOrderInfo();
            BindOrderInfoSum();
            ResizeNumericPad();
            if (!RightsItemCode.FindRights(RightsItemCode.PAYMENT))
            {
                btnConfirm.Enabled = false;
                btnConfirm.BackColor = ConstantValuePool.DisabledColor;
            }
            this.btnDeskNo.Text = "桌号：" + m_CurrentDeskName;
            this.btnEmployee.Text = "服务员：" + ConstantValuePool.CurrentEmployee.EmployeeNo;
            this.btnPersonNum.Text = "人数：" + m_SalesOrder.order.PeopleNum;
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
                btn.BackColor = Color.Blue;
                btn.DisplayColor = btn.BackColor;
                btn.Font = new Font("Microsoft YaHei", 12F, FontStyle.Regular);
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
            this.pnlPayoffWay.Controls.Add(btnPageUp);
            px = (m_ColumnsCount - 1) * m_Width + (m_ColumnsCount - 1 + 1) * m_Space;
            py = (m_RowsCount - 1) * m_Height + (m_RowsCount - 1 + 1) * m_Space;
            btnPageDown.Width = m_Width;
            btnPageDown.Height = m_Height;
            btnPageDown.Location = new Point(px, py);
            this.pnlPayoffWay.Controls.Add(btnPageDown);
        }

        private void BindGoodsOrderInfo()
        {
            this.dgvGoodsOrder.Rows.Clear();
            if (m_SalesOrder.orderDetailsList != null && m_SalesOrder.orderDetailsList.Count > 0)
            {
                foreach (OrderDetails orderDetails in m_SalesOrder.orderDetailsList)
                {
                    int index = dgvGoodsOrder.Rows.Add(new DataGridViewRow());
                    dgvGoodsOrder.Rows[index].Cells["ItemID"].Value = orderDetails.GoodsID;
                    dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value = orderDetails.ItemQty;
                    if (orderDetails.ItemType == (int)OrderItemType.Goods)
                    {
                        dgvGoodsOrder.Rows[index].Cells["GoodsName"].Value = orderDetails.GoodsName;
                    }
                    else
                    {
                        if (dgvGoodsOrder.Rows[index - 1].Cells["GoodsName"].Value.ToString().IndexOf('-') >= 0)
                        {
                            int lastIndex = dgvGoodsOrder.Rows[index - 1].Cells["GoodsName"].Value.ToString().LastIndexOf('-');
                            string strDetailFlag = dgvGoodsOrder.Rows[index - 1].Cells["GoodsName"].Value.ToString().Substring(0, lastIndex + 1);
                            dgvGoodsOrder.Rows[index].Cells["GoodsName"].Value = strDetailFlag + "--" + orderDetails.GoodsName;
                        }
                        else
                        {
                            dgvGoodsOrder.Rows[index].Cells["GoodsName"].Value = "--" + orderDetails.GoodsName;
                        }
                    }
                    dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value = orderDetails.TotalSellPrice;
                    dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value = orderDetails.TotalDiscount;
                    dgvGoodsOrder.Rows[index].Cells["OrderDetailsID"].Value = orderDetails.OrderDetailsID;
                    dgvGoodsOrder.Rows[index].Cells["OrderDetailsID"].Tag = orderDetails;
                }
            }
        }

        private void BindOrderInfoSum()
        {
            decimal totalPrice = 0, totalDiscount = 0;
            for (int i = 0; i < dgvGoodsOrder.Rows.Count; i++)
            {
                totalPrice += Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["GoodsPrice"].Value);
                totalDiscount += Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["GoodsDiscount"].Value);
            }
            m_TotalPrice = totalPrice;
            m_Discount = totalDiscount;
            this.lbTotalPrice.Text = "总金额：" + totalPrice.ToString("f2");
            this.lbDiscount.Text = "折扣：" + totalDiscount.ToString("f2");
            decimal wholePayMoney = totalPrice + totalDiscount;
            decimal actualPayMoney = CutOffDecimal.HandleCutOff(wholePayMoney, CutOffType.ROUND_OFF, 0);
            m_ActualPayMoney = actualPayMoney;
            m_CutOff = wholePayMoney - actualPayMoney;
            this.lbNeedPayMoney.Text = "实际应付：" + actualPayMoney.ToString("f2");
            this.lbReceMoney.Text = actualPayMoney.ToString("f2");
            this.lbCutOff.Text = "去零：" + (-m_CutOff).ToString("f2");
            if (m_CutServiceFee)
            {
                m_ServiceFee = 0;
                this.lbServiceFee.Text = "0.00";
            }
            else
            {
                decimal serviceFee = ConstantValuePool.SysConfig.FixedServiceFee;
                if (serviceFee == 0)
                {
                    decimal serviceFeePercent = 0;
                    if (string.IsNullOrEmpty(ConstantValuePool.SysConfig.ServiceFeeBeginTime1) && string.IsNullOrEmpty(ConstantValuePool.SysConfig.ServiceFeeEndTime1)
                        && string.IsNullOrEmpty(ConstantValuePool.SysConfig.ServiceFeeBeginTime2) && string.IsNullOrEmpty(ConstantValuePool.SysConfig.ServiceFeeEndTime2))
                    {
                        serviceFeePercent = ConstantValuePool.SysConfig.ServiceFeePercent;
                    }
                    else
                    {
                        DateTime curTime = Convert.ToDateTime(DateTime.Now.ToString("T"));
                        if (!string.IsNullOrEmpty(ConstantValuePool.SysConfig.ServiceFeeBeginTime1) && !string.IsNullOrEmpty(ConstantValuePool.SysConfig.ServiceFeeEndTime1))
                        {
                            if (curTime > Convert.ToDateTime(ConstantValuePool.SysConfig.ServiceFeeBeginTime1) && curTime < Convert.ToDateTime(ConstantValuePool.SysConfig.ServiceFeeEndTime1))
                            {
                                serviceFeePercent = ConstantValuePool.SysConfig.ServiceFeePercent;
                            }
                        }
                        if (!string.IsNullOrEmpty(ConstantValuePool.SysConfig.ServiceFeeBeginTime2) && !string.IsNullOrEmpty(ConstantValuePool.SysConfig.ServiceFeeEndTime2))
                        {
                            if (curTime > Convert.ToDateTime(ConstantValuePool.SysConfig.ServiceFeeBeginTime2) && curTime < Convert.ToDateTime(ConstantValuePool.SysConfig.ServiceFeeEndTime2))
                            {
                                serviceFeePercent = ConstantValuePool.SysConfig.ServiceFeePercent;
                            }
                        }
                    }
                    decimal tempServiceFee = actualPayMoney * serviceFeePercent / 100;
                    serviceFee = CutOffDecimal.HandleCutOff(tempServiceFee, CutOffType.ROUND_OFF, 0);
                }
                m_ServiceFee = serviceFee;
                this.lbServiceFee.Text = serviceFee.ToString("f2");
            }
        }

        private void btnPayoff_Click(object sender, EventArgs e)
        {
            CrystalButton btn = sender as CrystalButton;
            if (m_PrePayoffButton != null)
            {
                m_PrePayoffButton.BackColor = m_PrePayoffButton.DisplayColor;
            }
            btn.BackColor = ConstantValuePool.PressedColor;
            m_PrePayoffButton = btn;
            curPayoffWay = btn.Tag as PayoffWay;
            this.txtPayoff.Text = curPayoffWay.PayoffName + "(1:" + curPayoffWay.AsPay.ToString("f2") + ")";
            if (dic.ContainsKey(curPayoffWay.PayoffID.ToString()))
            {
                OrderPayoff orderPayoff = dic[curPayoffWay.PayoffID.ToString()];
                decimal totalPrice = orderPayoff.Quantity * orderPayoff.AsPay;
                if (curPayoffWay.PayoffType == (int)PayoffWayMode.GiftVoucher)
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
                if (curPayoffWay.PayoffType == (int)PayoffWayMode.GiftVoucher)
                {
                    this.txtAmount.Text = string.Format("{0} 张(合 {1} 元)", "0", "0.00");
                }
                else
                {
                    this.txtAmount.Text = string.Format("{0} 元", "0.00");
                }
            }
            m_InputNumber = "0";
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            m_IsPaidOrder = false;
            this.Close();
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (curPayoffWay != null)
            {
                dic.Remove(curPayoffWay.PayoffID.ToString());
                if (curPayoffWay.PayoffType == (int)PayoffWayMode.GiftVoucher)
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
            if (curPayoffWay.PayoffType == (int)PayoffWayMode.GiftVoucher)
            {
                this.txtAmount.Text = string.Format("{0} 张(合 {1} 元)", m_InputNumber, totalPrice.ToString("f2"));
            }
            else
            {
                this.txtAmount.Text = string.Format("{0} 元", totalPrice.ToString("f2"));
            }
            DisplayPayoffWay();
        }

        private void DisplayPayoffWay()
        {
            decimal realPay = 0;
            string strPayoffWay = string.Empty;
            foreach (KeyValuePair<string, OrderPayoff> item in dic)
            {
                if (item.Value.Quantity > 0)
                {
                    decimal totalPrice = item.Value.Quantity * item.Value.AsPay;
                    realPay += totalPrice;
                    string singlePay = string.Empty;
                    if (item.Value.PayoffType == (int)PayoffWayMode.GiftVoucher)
                    {
                        singlePay = string.Format("[{0} : {1} 张(合 {2} 元)]", item.Value.PayoffName, item.Value.Quantity, totalPrice.ToString("f2"));
                    }
                    else
                    {
                        singlePay = string.Format("[{0} : {1} 元]", item.Value.PayoffName, totalPrice.ToString("f2"));
                    }
                    strPayoffWay = strPayoffWay + "," + singlePay;
                }
            }
            if (!string.IsNullOrEmpty(strPayoffWay))
            {
                this.lbPaidInMoney.Text = realPay.ToString("f2");
                this.txtPayoffWay.Text = strPayoffWay.Substring(1);
            }
            else
            {
                this.lbPaidInMoney.Text = "0.00";
                this.txtPayoffWay.Text = string.Empty;
            }
            decimal unPaidPrice = m_ActualPayMoney + m_ServiceFee - realPay;
            if (unPaidPrice > 0)
            {
                this.lbUnpaidAmount.Text = unPaidPrice.ToString("f2");
            }
            else
            {
                this.lbUnpaidAmount.Text = "0.00";
            }
        }

        private void btnReal_Click(object sender, EventArgs e)
        {
            if (curPayoffWay != null && !string.IsNullOrEmpty(lbReceMoney.Text))
            {
                decimal remainNeedPay = 0;
                if (string.IsNullOrEmpty(lbPaidInMoney.Text))
                {
                    remainNeedPay = decimal.Parse(lbReceMoney.Text) + m_ServiceFee;
                }
                else
                {
                    remainNeedPay = decimal.Parse(lbReceMoney.Text) + m_ServiceFee - decimal.Parse(lbPaidInMoney.Text);
                }
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

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.Rows.Count > 0)
            {
                //权限验证
                bool hasRights = false;
                if (RightsItemCode.FindRights(RightsItemCode.WHOLEDISCOUNT))
                {
                    hasRights = true;
                }
                else
                {
                    FormRightsCode form = new FormRightsCode();
                    form.ShowDialog();
                    if (form.ReturnValue)
                    {
                        IList<string> rightsCodeList = form.RightsCodeList;
                        if (RightsItemCode.FindRights(rightsCodeList, RightsItemCode.WHOLEDISCOUNT))
                        {
                            hasRights = true;
                        }
                    }
                }
                if (!hasRights)
                {
                    return;
                }
                FormDiscount formDiscount = new FormDiscount(DiscountDisplayModel.WholeDiscount);
                formDiscount.ShowDialog();
                if (formDiscount.CurrentDiscount != null)
                {
                    Discount discount = formDiscount.CurrentDiscount;
                    foreach (DataGridViewRow dr in dgvGoodsOrder.Rows)
                    {
                        OrderDetails orderDetails = dr.Cells["OrderDetailsID"].Tag as OrderDetails;
                        if (orderDetails != null)
                        {
                            Discount itemDiscount = dr.Cells["GoodsDiscount"].Tag as Discount;
                            decimal itemDiscountPrice = Convert.ToDecimal(dr.Cells["GoodsDiscount"].Value);
                            if (orderDetails.CanDiscount && (itemDiscount != null || itemDiscountPrice == 0))
                            {                                
                                if (discount.DiscountType == (int)DiscountItemType.DiscountRate)
                                {
                                    dr.Cells["GoodsDiscount"].Value = -Convert.ToDecimal(dr.Cells["GoodsPrice"].Value) * discount.DiscountRate;
                                }
                                else
                                {
                                    dr.Cells["GoodsDiscount"].Value = -discount.OffFixPay;
                                }
                                orderDetails.TotalDiscount = Convert.ToDecimal(dr.Cells["GoodsDiscount"].Value);
                                dr.Cells["OrderDetailsID"].Tag = orderDetails;
                                dr.Cells["GoodsDiscount"].Tag = discount;
                            }
                        }
                    }
                    //统计
                    BindOrderInfoSum();                    
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            decimal needChangePay = 0;  //找零
            decimal receMoney = decimal.Parse(lbReceMoney.Text);    //应收
            decimal paidInMoney = decimal.Parse(lbPaidInMoney.Text);    //实收
            if (receMoney + m_ServiceFee < paidInMoney)
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
                if (noCash > receMoney + m_ServiceFee)
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
                    if (noCash - noCashPayoffArr[noCashPayoffArr.Length - 1].AsPay < receMoney + m_ServiceFee)
                    {
                        if (cashPayoff.Count > 0)
                        {
                            MessageBox.Show("现金支付方式多余！");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("非现金支付方式金额过多，请重新支付！");
                        return;
                    }
                }
                else
                {
                    //由全部非现金及部分现金来支付
                    needChangePay = paidInMoney - (receMoney + m_ServiceFee);
                    foreach (KeyValuePair<string, OrderPayoff> item in dic)
                    {
                        if (item.Value.PayoffType == (int)PayoffWayMode.Cash)
                        {
                            item.Value.NeedChangePay = needChangePay;
                            break;
                        }
                    }
                }
            }
            if (receMoney + m_ServiceFee > paidInMoney)
            {
                // 支付的金额不足, 现金补足
                foreach (PayoffWay temp in ConstantValuePool.PayoffWayList)
                {
                    if (temp.PayoffType == (int)PayoffWayMode.Cash)
                    {
                        if (dic.ContainsKey(temp.PayoffID.ToString()))
                        {
                            OrderPayoff orderPayoff = dic[temp.PayoffID.ToString()];
                            orderPayoff.Quantity += (receMoney + m_ServiceFee - paidInMoney) / temp.AsPay;
                        }
                        else
                        {
                            OrderPayoff orderPayoff = new OrderPayoff();
                            orderPayoff.OrderPayoffID = Guid.NewGuid();
                            orderPayoff.OrderID = m_SalesOrder.order.OrderID;
                            orderPayoff.PayoffID = temp.PayoffID;
                            orderPayoff.PayoffName = temp.PayoffName;
                            orderPayoff.PayoffType = temp.PayoffType;
                            orderPayoff.AsPay = temp.AsPay;
                            orderPayoff.Quantity = (receMoney + m_ServiceFee - paidInMoney) / temp.AsPay;
                            orderPayoff.CardNo = "";
                            orderPayoff.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
                            dic.Add(temp.PayoffID.ToString(), orderPayoff);
                        }
                        break;
                    }
                }
            }
            
            //计算支付的金额并填充OrderPayoff
            decimal paymentMoney = 0;
            List<OrderPayoff> orderPayoffList = new List<OrderPayoff>();
            foreach (KeyValuePair<string, OrderPayoff> item in dic)
            {
                if (item.Value.Quantity > 0)
                {
                    OrderPayoff orderPayoff = item.Value;
                    paymentMoney += orderPayoff.AsPay * orderPayoff.Quantity;
                    orderPayoffList.Add(orderPayoff);
                }
            }
            bool result = PayForOrder(orderPayoffList, paymentMoney, needChangePay);
            if (result)
            {
                //打印小票
                PrintData printData = new PrintData();
                printData.ShopName = ConstantValuePool.CurrentShop.ShopName;
                printData.DeskName = m_CurrentDeskName;
                printData.PersonNum = m_SalesOrder.order.PeopleNum.ToString();
                printData.PrintTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                printData.EmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                printData.TranSequence = m_SalesOrder.order.TranSequence.ToString();
                printData.ShopAddress = ConstantValuePool.CurrentShop.RunAddress;
                printData.Telephone = ConstantValuePool.CurrentShop.Telephone;
                printData.ReceivableMoney = this.lbReceMoney.Text;
                printData.ServiceFee = m_ServiceFee.ToString("f2");
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
                string configPath = @"PrintConfig\PrintOrderSetting.config";
                string layoutPath = @"PrintConfig\PrintPaidOrder.ini";
                DriverOrderPrint printer = new DriverOrderPrint("Microsoft XPS Document Writer", "76mm", "SpecimenLabel");
                printer.DoPrint(printData, layoutPath, configPath);

                m_IsPaidOrder = true;
                //更新桌况为空闲状态
                int status = (int)DeskButtonStatus.IDLE_MODE;
                DeskService deskService = new DeskService();
                deskService.UpdateDeskStatus(m_CurrentDeskName, string.Empty, status);
                FormConfirm form = new FormConfirm(receMoney + m_ServiceFee, paidInMoney, needChangePay, orderPayoffList);
                form.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("支付账单失败！");
                return;
            }
        }

        private bool PayForOrder(List<OrderPayoff> orderPayoffList, decimal paymentMoney, decimal needChangePay)
        {
            //填充Order
            Order order = new Order();
            order.OrderID = m_SalesOrder.order.OrderID;
            order.TotalSellPrice = m_TotalPrice;
            order.ActualSellPrice = m_ActualPayMoney;
            order.DiscountPrice = m_Discount;
            order.CutOffPrice = m_CutOff;
            order.ServiceFee = m_ServiceFee;
            order.PaymentMoney = paymentMoney;
            order.NeedChangePay = needChangePay;
            order.MembershipCard = string.Empty;
            order.MemberDiscount = 0;
            order.PayEmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
            order.PayEmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
            order.PayLastTime = 0;
            order.CheckoutDeviceNo = ConstantValuePool.BizSettingConfig.DeviceNo;
            //填充OrderDetails\OrderDiscount
            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            List<OrderDiscount> newOrderDiscountList = new List<OrderDiscount>();
            foreach (DataGridViewRow dr in dgvGoodsOrder.Rows)
            {
                OrderDetails orderDetails = dr.Cells["OrderDetailsID"].Tag as OrderDetails;
                if (orderDetails != null)
                {
                    Discount itemDiscount = dr.Cells["GoodsDiscount"].Tag as Discount;
                    decimal itemDiscountPrice = Convert.ToDecimal(dr.Cells["GoodsDiscount"].Value);
                    if (orderDetails.CanDiscount && itemDiscount != null && Math.Abs(itemDiscountPrice) > 0)
                    {
                        orderDetailsList.Add(orderDetails);
                        //OrderDiscount
                        OrderDiscount orderDiscount = new OrderDiscount();
                        orderDiscount.OrderDiscountID = Guid.NewGuid();
                        orderDiscount.OrderID = m_SalesOrder.order.OrderID;
                        orderDiscount.OrderDetailsID = orderDetails.OrderDetailsID;
                        orderDiscount.DiscountID = itemDiscount.DiscountID;
                        orderDiscount.DiscountName = itemDiscount.DiscountName;
                        orderDiscount.DiscountType = itemDiscount.DiscountType;
                        orderDiscount.DiscountRate = itemDiscount.DiscountRate;
                        orderDiscount.OffFixPay = itemDiscount.OffFixPay;
                        orderDiscount.OffPay = Math.Abs(Convert.ToDecimal(dr.Cells["GoodsDiscount"].Value));
                        orderDiscount.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
                        newOrderDiscountList.Add(orderDiscount);
                    }
                }
            }
            PayingOrder payingOrder = new PayingOrder();
            payingOrder.order = order;
            payingOrder.orderDetailsList = orderDetailsList;
            payingOrder.orderDiscountList = newOrderDiscountList;
            payingOrder.orderPayoffList = orderPayoffList;
            PayingOrderService payingOrderService = new PayingOrderService();
            return payingOrderService.PayForOrder(payingOrder);
        }

        private void ResizeNumericPad()
        {
            if (this.Width > 1024)
            {
                double widthRate = Convert.ToDouble(this.Width - this.pnlLeft.Width - this.panel2.Width - this.panel4.Width) / 526;
                double heightRate = 1;
                foreach (Control c in this.pnlNumericPad.Controls)
                {
                    SetControlSize(c, widthRate, heightRate);
                }
                foreach (Control c in this.pnlBigNumeric.Controls)
                {
                    SetControlSize(c, widthRate, heightRate);
                }
                foreach (Control c in this.pnlNumeric.Controls)
                {
                    SetControlSize(c, widthRate, heightRate);
                }
            }
        }

        private void SetControlSize(Control ctl, double widthRate, double heightRate)
        {
            ctl.Width = Convert.ToInt32(ctl.Width * widthRate);
            ctl.Height = Convert.ToInt32(ctl.Height * heightRate);
            ctl.Location = new Point(Convert.ToInt32(ctl.Location.X * widthRate), Convert.ToInt32(ctl.Location.Y * heightRate));
        }

        private void btnPreCheck_Click(object sender, EventArgs e)
        {
            //权限验证
            bool hasRights = false;
            if (RightsItemCode.FindRights(RightsItemCode.PRECHECKOUT))
            {
                hasRights = true;
            }
            else
            {
                FormRightsCode form = new FormRightsCode();
                form.ShowDialog();
                if (form.ReturnValue)
                {
                    IList<string> rightsCodeList = form.RightsCodeList;
                    if (RightsItemCode.FindRights(rightsCodeList, RightsItemCode.PRECHECKOUT))
                    {
                        hasRights = true;
                    }
                }
            }
            if (!hasRights)
            {
                return;
            }
        }

        private void btnMember_Click(object sender, EventArgs e)
        {
            //权限验证
            bool hasRights = false;
            if (RightsItemCode.FindRights(RightsItemCode.MEMBERDISCOUNT))
            {
                hasRights = true;
            }
            else
            {
                FormRightsCode form = new FormRightsCode();
                form.ShowDialog();
                if (form.ReturnValue)
                {
                    IList<string> rightsCodeList = form.RightsCodeList;
                    if (RightsItemCode.FindRights(rightsCodeList, RightsItemCode.MEMBERDISCOUNT))
                    {
                        hasRights = true;
                    }
                }
            }
            if (!hasRights)
            {
                return;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (m_CutServiceFee)
            {
                m_CutServiceFee = false;
                this.pictureBox1.Image = Properties.Resources.cut;
            }
            else
            {
                m_CutServiceFee = true;
                this.pictureBox1.Image = Properties.Resources.add;
            }
            BindOrderInfoSum();
        }
    }
}
