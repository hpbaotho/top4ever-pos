using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.Common;
using Top4ever.CustomControl;
using Top4ever.Domain;
using Top4ever.Domain.Transfer;
using Top4ever.Domain.OrderRelated;
using Top4ever.Entity;
using Top4ever.Entity.Enum;
using Top4ever.Print;
using Top4ever.Print.Entity;
using Top4ever.ClientService;

namespace Top4ever.Pos.Feature
{
    public partial class FormBackGoods : Form
    {
        private const int m_Space = 2;
        private int m_Width = 0;
        private int m_Height = 0;
        private int m_ColumnsCount = 0;
        private int m_RowsCount = 0;
        private int m_PageSize = 0;
        private int m_PageIndex = 0;
        private List<CrystalButton> payoffButtonList = new List<CrystalButton>();
        private SalesOrder m_SalesOrder = null;
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
        /// 服务费
        /// </summary>
        private decimal m_ServiceFee = 0;
        /// <summary>
        /// 账单是否修改
        /// </summary>
        private bool m_IsChanged = false;
        public bool IsChanged
        {
            get { return m_IsChanged; }
        }

        public FormBackGoods(SalesOrder salesOrder)
        {
            InitializeComponent();
            m_SalesOrder = salesOrder;
            btnPageUp.BackColor = btnPageUp.DisplayColor = Color.Tomato;
            btnPageDown.BackColor = btnPageDown.DisplayColor = Color.Teal;
        }

        private void FormBackGoods_Load(object sender, EventArgs e)
        {
            CalculateButtonSize();
            GetPayoffButton();
            DisplayPayoffButton();
            BindGoodsOrderInfo();
            BindPayoffWay();
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
                        m_Width = (this.pnlPayoffWay.Width - m_Space * (control.ColumnsCount - 1)) / control.ColumnsCount;
                        m_Height = (this.pnlPayoffWay.Height - m_Space * (control.RowsCount - 1)) / control.RowsCount;
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
                btn.Font = new Font("Microsoft YaHei", 9.75F, FontStyle.Regular);
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
            int px = 0, py = 0;
            for (int i = startIndex; i < endIndex; i++)
            {
                CrystalButton btn = payoffButtonList[i];
                btn.Location = new Point(px, py);
                this.pnlPayoffWay.Controls.Add(btn);
                if (index % m_ColumnsCount == 0)
                {
                    px = 0;
                    py += m_Height + m_Space;
                }
                else
                {
                    px += m_Width + m_Space;
                }
                index++;
            }
            px = (m_ColumnsCount - 2) * m_Width + (m_ColumnsCount - 2) * m_Space;
            py = (m_RowsCount - 1) * m_Height + (m_RowsCount - 1) * m_Space;
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
            px = (m_ColumnsCount - 1) * m_Width + (m_ColumnsCount - 1) * m_Space;
            py = (m_RowsCount - 1) * m_Height + (m_RowsCount - 1) * m_Space;
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

        private void BindGoodsOrderInfo()
        {
            this.dgvGoodsOrder.Rows.Clear();
            if (m_SalesOrder.orderDetailsList != null && m_SalesOrder.orderDetailsList.Count > 0)
            {
                foreach (OrderDetails orderDetails in m_SalesOrder.orderDetailsList)
                {
                    int index = dgvGoodsOrder.Rows.Add(new DataGridViewRow());
                    dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value = orderDetails.ItemQty;
                    if (orderDetails.ItemType == (int)OrderItemType.Goods)
                    {
                        dgvGoodsOrder.Rows[index].Cells["GoodsName"].Value = orderDetails.GoodsName;
                    }
                    else
                    {
                        string strLevelFlag = string.Empty;
                        int levelCount = orderDetails.ItemLevel * 2;
                        for (int i = 0; i < levelCount; i++)
                        {
                            strLevelFlag += "-";
                        }
                        dgvGoodsOrder.Rows[index].Cells["GoodsName"].Value = strLevelFlag + orderDetails.GoodsName;
                    }
                    dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value = orderDetails.TotalSellPrice;
                    dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value = orderDetails.TotalDiscount;
                    dgvGoodsOrder.Rows[index].Cells["DelFlag"].Value = "-0.0";
                    dgvGoodsOrder.Rows[index].Cells["OrderDetailsID"].Value = orderDetails.OrderDetailsID;
                    dgvGoodsOrder.Rows[index].Cells["OrderDetailsID"].Tag = orderDetails;
                }
            }
        }

        private void BindPayoffWay()
        { 
            this.dgvPayoffWay.Rows.Clear();
            if (m_SalesOrder.orderPayoffList != null && m_SalesOrder.orderPayoffList.Count > 0)
            {
                foreach (OrderPayoff orderPayoff in m_SalesOrder.orderPayoffList)
                {
                    int index = dgvPayoffWay.Rows.Add();
                    dgvPayoffWay.Rows[index].Cells["ColPayoffWay"].Value = orderPayoff.PayoffName;
                    dgvPayoffWay.Rows[index].Cells["ColPayoffPrice"].Value = (orderPayoff.AsPay * orderPayoff.Quantity).ToString("f2");
                    if (orderPayoff.NeedChangePay > 0)
                    {
                        dgvPayoffWay.Rows[index].Cells["ColChangePay"].Value = (-orderPayoff.NeedChangePay).ToString("f2");
                    }
                }
            }
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
                this.txtRealReturnAmount.Text = realPay.ToString("f2");
                this.txtPayoffWay.Text = strPayoffWay.Substring(1);
            }
            else
            {
                this.txtRealReturnAmount.Text = "0.00";
                this.txtPayoffWay.Text = string.Empty;
            }
        }

        private void btnReal_Click(object sender, EventArgs e)
        {
            if (curPayoffWay != null && !string.IsNullOrEmpty(txtRefundAmount.Text))
            {
                decimal remainNeedPay = 0;
                if (string.IsNullOrEmpty(txtRealReturnAmount.Text))
                {
                    remainNeedPay = decimal.Parse(txtRefundAmount.Text);
                }
                else
                {
                    remainNeedPay = decimal.Parse(txtRefundAmount.Text) - decimal.Parse(txtRealReturnAmount.Text);
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

        private void btnDeleteNumber_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.CurrentRow != null)
            {
                int selectIndex = dgvGoodsOrder.CurrentRow.Index;
                OrderDetails orderDetails = dgvGoodsOrder.Rows[selectIndex].Cells["OrderDetailsID"].Tag as OrderDetails;
                if (orderDetails.ItemType == (int)OrderItemType.Details)
                {
                    MessageBox.Show("细项不能单独删除！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (orderDetails.ItemType == (int)OrderItemType.SetMeal)
                {
                    MessageBox.Show("套餐项不能单独删除！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                CrystalButton btn = sender as CrystalButton;
                decimal goodsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                decimal deletedNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["DelFlag"].Value);
                decimal delNum = Math.Abs(decimal.Parse(btn.Text));
                if (delNum > goodsNum + deletedNum)
                {
                    MessageBox.Show("删除的品项数量不能超过剩余数量！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    int goodsTotalCount = 0; //菜品主项数量合计
                    for (int i = 0; i < dgvGoodsOrder.RowCount; i++)
                    {
                        OrderDetails tempOrderDetails = dgvGoodsOrder.Rows[i].Cells["OrderDetailsID"].Tag as OrderDetails;
                        if (tempOrderDetails.ItemType == (int)OrderItemType.Goods)
                        {
                            decimal tempGoodsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["GoodsNum"].Value);
                            decimal tempDeletedNum = Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["DelFlag"].Value);
                            if (tempGoodsNum + tempDeletedNum > 0)
                            {
                                goodsTotalCount++;
                            }
                        }
                    }
                    if (goodsTotalCount == 1 && delNum >= goodsNum + deletedNum)
                    {
                        MessageBox.Show("如果您要进行退单操作，请整单删除！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    FormCancelOrder form = new FormCancelOrder();
                    form.ShowDialog();
                    if (form.CurrentReason != null)
                    {
                        dgvGoodsOrder.Rows[selectIndex].Cells["DelFlag"].Value = deletedNum - delNum;
                        dgvGoodsOrder.Rows[selectIndex].Cells["DelReasonName"].Value = form.CurrentReason.ReasonName;
                        //细项和套餐
                        for (int i = selectIndex + 1; i < dgvGoodsOrder.RowCount; i++)
                        {
                            OrderDetails tempOrderDetails = dgvGoodsOrder.Rows[i].Cells["OrderDetailsID"].Tag as OrderDetails;
                            if (tempOrderDetails.ItemType == (int)OrderItemType.Goods)
                            {
                                break;
                            }
                            else
                            {
                                decimal detailsDeletedNum = Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["DelFlag"].Value);
                                decimal detailsDelNum = tempOrderDetails.ItemQty / goodsNum * delNum;
                                dgvGoodsOrder.Rows[i].Cells["DelFlag"].Value = detailsDeletedNum - detailsDelNum;
                                dgvGoodsOrder.Rows[i].Cells["DelReasonName"].Value = form.CurrentReason.ReasonName;
                            }
                        }
                        CalculateOrderPrice();
                        txtRefundAmount.Text = (m_SalesOrder.order.ActualSellPrice + m_SalesOrder.order.ServiceFee - (m_ActualPayMoney + m_ServiceFee)).ToString("f2");
                    }
                }
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.CurrentRow != null)
            {
                int selectIndex = dgvGoodsOrder.CurrentRow.Index;
                dgvGoodsOrder.Rows[selectIndex].Cells["DelFlag"].Value = "-0.0";
                dgvGoodsOrder.Rows[selectIndex].Cells["DelReasonName"].Value = null;
                //细项和套餐
                for (int i = selectIndex + 1; i < dgvGoodsOrder.RowCount; i++)
                {
                    OrderDetails tempOrderDetails = dgvGoodsOrder.Rows[i].Cells["OrderDetailsID"].Tag as OrderDetails;
                    if (tempOrderDetails.ItemType == (int)OrderItemType.Goods)
                    {
                        break;
                    }
                    else
                    {
                        dgvGoodsOrder.Rows[i].Cells["DelFlag"].Value = "-0.0";
                        dgvGoodsOrder.Rows[i].Cells["DelReasonName"].Value = null;
                    }
                }
                CalculateOrderPrice();
                txtRefundAmount.Text = (m_SalesOrder.order.ActualSellPrice + m_SalesOrder.order.ServiceFee - (m_ActualPayMoney + m_ServiceFee)).ToString("f2");
            }
        }

        private void CalculateOrderPrice()
        {
            decimal totalPrice = 0, totalDiscount = 0;
            for (int i = 0; i < dgvGoodsOrder.Rows.Count; i++)
            {
                decimal itemNum = Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["GoodsNum"].Value);
                decimal deletedNum = Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["DelFlag"].Value);
                OrderDetails orderDetails = dgvGoodsOrder.Rows[i].Cells["OrderDetailsID"].Tag as OrderDetails;
                totalPrice += (itemNum + deletedNum) * orderDetails.SellPrice;
                totalDiscount += Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["GoodsDiscount"].Value) / itemNum * (itemNum + deletedNum);
            }
            m_TotalPrice = totalPrice;
            m_Discount = totalDiscount;
            decimal wholePayMoney = totalPrice + totalDiscount;
            decimal actualPayMoney = CutOffDecimal.HandleCutOff(wholePayMoney, CutOffType.ROUND_OFF, 0);
            m_ActualPayMoney = actualPayMoney;
            m_CutOff = wholePayMoney - actualPayMoney;
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
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            decimal refundAmount = decimal.Parse(txtRefundAmount.Text);
            decimal realReturnAmount = 0;
            if (!string.IsNullOrEmpty(txtRealReturnAmount.Text))
            {
                realReturnAmount = decimal.Parse(txtRealReturnAmount.Text);
            }
            if (refundAmount == realReturnAmount)
            {
                //判断支付方式是否正确
                bool IsPayoffWayRight = true;
                IList<OrderPayoff> _orderPayoffList = new List<OrderPayoff>();
                foreach (KeyValuePair<string, OrderPayoff> item in dic)
                {
                    if (item.Value.Quantity > 0)
                    {
                        _orderPayoffList.Add(item.Value);
                    }
                }
                foreach (OrderPayoff orderPayoff in _orderPayoffList)
                {
                    bool IsContains = false;
                    OrderPayoff temp = null;
                    foreach (OrderPayoff item in m_SalesOrder.orderPayoffList)
                    {
                        if (item.PayoffID.Equals(orderPayoff.PayoffID))
                        {
                            IsContains = true;
                            temp = item;
                            break;
                        }
                    }
                    if (IsContains)
                    {
                        decimal tempAmount = temp.AsPay * temp.Quantity - temp.NeedChangePay;
                        decimal payAmount = orderPayoff.AsPay * orderPayoff.Quantity - orderPayoff.NeedChangePay;
                        if (tempAmount < payAmount)
                        {
                            IsPayoffWayRight = false;
                            break;
                        }
                    }
                    else
                    {
                        IsPayoffWayRight = false;
                        break;
                    }
                }
                if (IsPayoffWayRight)
                {
                    IList<OrderPayoff> orderPayoffList = new List<OrderPayoff>();
                    foreach (OrderPayoff item in m_SalesOrder.orderPayoffList)
                    {
                        bool IsContains = false;
                        foreach (OrderPayoff orderPayoff in _orderPayoffList)
                        {
                            if (item.PayoffID.Equals(orderPayoff.PayoffID))
                            {
                                decimal remainQty = ((item.AsPay * item.Quantity - item.NeedChangePay) - (orderPayoff.AsPay * orderPayoff.Quantity - orderPayoff.NeedChangePay)) / orderPayoff.AsPay;
                                if (remainQty > 0)
                                {
                                    OrderPayoff temp = new OrderPayoff();
                                    temp.OrderPayoffID = Guid.NewGuid();
                                    temp.OrderID = m_SalesOrder.order.OrderID;
                                    temp.PayoffID = item.PayoffID;
                                    temp.PayoffName = item.PayoffName;
                                    temp.PayoffType = item.PayoffType;
                                    temp.AsPay = item.AsPay;
                                    temp.Quantity = remainQty;
                                    temp.NeedChangePay = 0;
                                    temp.CardNo = item.CardNo;
                                    temp.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
                                    orderPayoffList.Add(temp);
                                }
                                IsContains = true;
                                break;
                            }
                        }
                        if (!IsContains)
                        {
                            OrderPayoff temp = CopyExtension.Clone<OrderPayoff>(item);
                            temp.OrderID = m_SalesOrder.order.OrderID;
                            temp.OrderPayoffID = Guid.NewGuid();
                            temp.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
                            orderPayoffList.Add(temp);
                        }
                    }
                    List<DeletedOrderDetails> deletedOrderDetailsList = new List<DeletedOrderDetails>();
                    for (int i = 0; i < dgvGoodsOrder.RowCount; i++)
                    {
                        OrderDetails tempOrderDetails = dgvGoodsOrder.Rows[i].Cells["OrderDetailsID"].Tag as OrderDetails;
                        decimal tempGoodsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["GoodsNum"].Value);
                        decimal tempGoodsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["GoodsDiscount"].Value);
                        decimal tempDeletedNum = Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["DelFlag"].Value);
                        if (Math.Abs(tempDeletedNum) > 0)
                        {
                            decimal remainQty = tempGoodsNum - Math.Abs(tempDeletedNum);
                            DeletedOrderDetails orderDetails = new DeletedOrderDetails();
                            orderDetails.OrderDetailsID = tempOrderDetails.OrderDetailsID;
                            orderDetails.DeletedQuantity = tempDeletedNum;
                            orderDetails.RemainQuantity = remainQty;
                            orderDetails.OffPay = Math.Round(-tempGoodsDiscount / tempGoodsNum * remainQty, 4);
                            orderDetails.AuthorisedManager = ConstantValuePool.CurrentEmployee.EmployeeID;
                            orderDetails.CancelEmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                            orderDetails.CancelReasonName = dgvGoodsOrder.Rows[i].Cells["DelReasonName"].Value.ToString(); ;
                            deletedOrderDetailsList.Add(orderDetails);
                        }
                    }
                    //构造DeletedPaidOrder对象
                    decimal paymentMoney = 0;
                    foreach (OrderPayoff item in orderPayoffList)
                    {
                        paymentMoney += item.AsPay * item.Quantity;
                    }
                    Order order = new Order();
                    order.OrderID = m_SalesOrder.order.OrderID;
                    order.TotalSellPrice = m_TotalPrice;
                    order.ActualSellPrice = m_ActualPayMoney;
                    order.DiscountPrice = m_Discount;
                    order.CutOffPrice = m_CutOff;
                    order.ServiceFee = m_ServiceFee;
                    order.PaymentMoney = paymentMoney;
                    order.NeedChangePay = 0;
                    order.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
                    DeletedPaidOrder deletedPaidOrder = new DeletedPaidOrder();
                    deletedPaidOrder.order = order;
                    deletedPaidOrder.deletedOrderDetailsList = deletedOrderDetailsList;
                    deletedPaidOrder.orderPayoffList = orderPayoffList;
                    DeletedOrderService orderService = new DeletedOrderService();
                    if (orderService.DeletePaidSingleOrder(deletedPaidOrder))
                    {
                        m_IsChanged = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("单品删除失败，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("退款支付方式不属于原结账支付方式，请重新支付！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("应退金额与实退金额不一致，请重新支付！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_IsChanged = false;
            this.Close();
        }
    }
}
