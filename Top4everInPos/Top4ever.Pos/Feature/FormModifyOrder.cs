using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.Common;
using Top4ever.ClientService;
using Top4ever.CustomControl;
using Top4ever.Domain;
using Top4ever.Domain.Transfer;
using Top4ever.Domain.OrderRelated;
using Top4ever.Entity;
using Top4ever.Entity.Enum;
using Top4ever.Print;
using Top4ever.Print.Entity;

namespace VechsoftPos.Feature
{
    public partial class FormModifyOrder : Form
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
        /// 去服务费
        /// </summary>
        private bool m_CutServiceFee = false;
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

        public FormModifyOrder(SalesOrder salesOrder)
        {
            InitializeComponent();
            m_SalesOrder = salesOrder;
            btnPageUp.BackColor = btnPageUp.DisplayColor = Color.Tomato;
            btnPageDown.BackColor = btnPageDown.DisplayColor = Color.Teal;
        }

        private void FormModifyOrder_Load(object sender, EventArgs e)
        {
            CalculateButtonSize();
            GetPayoffButton();
            DisplayPayoffButton();
            BindGoodsOrderInfo();
            BindPayoffWay();
            txtReceAmount.Text = (m_SalesOrder.order.ActualSellPrice + m_SalesOrder.order.ServiceFee).ToString("f2");
            if (m_SalesOrder.order.ServiceFee == 0)
            {
                btnCutServiceFee.Enabled = false;
                btnCutServiceFee.BackColor = ConstantValuePool.DisabledColor;
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
                    dgvPayoffWay.Rows[index].Cells["ColStatus"].Value = "删除";
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
                    if (item.Value.PayoffType == (int)PayoffWayMode.GiftVoucher || item.Value.PayoffType == (int)PayoffWayMode.Coupon)
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
                this.txtPaidInAmount.Text = realPay.ToString("f2");
                this.txtPayoffWay.Text = strPayoffWay.Substring(1);
            }
            else
            {
                this.txtPaidInAmount.Text = "0.00";
                this.txtPayoffWay.Text = string.Empty;
            }
        }

        private void btnReal_Click(object sender, EventArgs e)
        {
            if (curPayoffWay != null && !string.IsNullOrEmpty(txtReceAmount.Text))
            {
                decimal remainNeedPay = 0;
                if (string.IsNullOrEmpty(txtPaidInAmount.Text))
                {
                    remainNeedPay = decimal.Parse(txtReceAmount.Text);
                }
                else
                {
                    remainNeedPay = decimal.Parse(txtReceAmount.Text) - decimal.Parse(txtPaidInAmount.Text);
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

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.CurrentRow != null)
            {
                //权限验证
                bool hasRights = false;
                if (RightsItemCode.FindRights(RightsItemCode.SINGLEDISCOUNT))
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
                        if (RightsItemCode.FindRights(rightsCodeList, RightsItemCode.SINGLEDISCOUNT))
                        {
                            hasRights = true;
                        }
                    }
                }
                if (!hasRights)
                {
                    return;
                }
                int selectIndex = dgvGoodsOrder.CurrentRow.Index;
                OrderDetails orderDetails = dgvGoodsOrder.Rows[selectIndex].Cells["OrderDetailsID"].Tag as OrderDetails;
                if (orderDetails.ItemType == (int)OrderItemType.Goods)   //主项才能打折
                {
                    if (orderDetails.CanDiscount)
                    {
                        FormDiscount formDiscount = new FormDiscount(DiscountDisplayModel.SingleDiscount, -1, orderDetails.TotalSellPrice);
                        formDiscount.ShowDialog();
                        if (formDiscount.CurrentDiscount != null)
                        {
                            Discount discount = formDiscount.CurrentDiscount;
                            if (discount.DiscountType == (int)DiscountItemType.DiscountRate)
                            {
                                dgvGoodsOrder.Rows[selectIndex].Cells["GoodsDiscount"].Value = -Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsPrice"].Value) * discount.DiscountRate;
                            }
                            else
                            {
                                dgvGoodsOrder.Rows[selectIndex].Cells["GoodsDiscount"].Value = -discount.OffFixPay;
                            }
                            dgvGoodsOrder.Rows[selectIndex].Cells["GoodsDiscount"].Tag = discount;
                            //更新细项
                            if (selectIndex < dgvGoodsOrder.Rows.Count - 1)
                            {
                                for (int index = selectIndex + 1; index < dgvGoodsOrder.Rows.Count; index++)
                                {
                                    orderDetails = dgvGoodsOrder.Rows[index].Cells["OrderDetailsID"].Tag as OrderDetails;
                                    if (orderDetails.ItemType == (int)OrderItemType.Goods)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        if (discount.DiscountType == (int)DiscountItemType.DiscountRate)
                                        {
                                            dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value = -Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value) * discount.DiscountRate;
                                        }
                                        else
                                        {
                                            dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value = -discount.OffFixPay;
                                        }
                                        dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Tag = discount;
                                    }
                                }
                            }
                            //重新计算
                            CalculateOrderPrice();
                            txtReceAmount.Text = (m_ActualPayMoney + m_ServiceFee).ToString("f2");
                        }
                    }
                }
            }
        }

        private void btnWholeDiscount_Click(object sender, EventArgs e)
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
            //计算能打折的总金额
            decimal canDiscountPrice = 0;
            foreach (DataGridViewRow dr in dgvGoodsOrder.Rows)
            {
                OrderDetails orderDetails = dr.Cells["OrderDetailsID"].Tag as OrderDetails;
                if (orderDetails != null)
                {
                    if (orderDetails.CanDiscount)
                    {
                        canDiscountPrice += Convert.ToDecimal(dr.Cells["GoodsPrice"].Value);
                    }
                }
            }
            FormDiscount formDiscount = new FormDiscount(DiscountDisplayModel.WholeDiscount, canDiscountPrice, m_ActualPayMoney);
            formDiscount.ShowDialog();
            if (formDiscount.CurrentDiscount != null)
            {
                Discount discount = formDiscount.CurrentDiscount;
                int firstIndex = -1; //折价索引
                decimal offFixedPay = 0;
                for (int index = 0; index < dgvGoodsOrder.Rows.Count; index++)
                {
                    DataGridViewRow dr = dgvGoodsOrder.Rows[index];
                    OrderDetails orderDetails = dr.Cells["OrderDetailsID"].Tag as OrderDetails;
                    if (orderDetails != null)
                    {
                        if (orderDetails.CanDiscount)
                        {
                            if (discount.DiscountType == (int)DiscountItemType.DiscountRate)
                            {
                                dr.Cells["GoodsDiscount"].Value = -Convert.ToDecimal(dr.Cells["GoodsPrice"].Value) * discount.DiscountRate;
                            }
                            else
                            {
                                if (firstIndex < 0)
                                {
                                    firstIndex = index;
                                }
                                decimal discountPrice = orderDetails.TotalSellPrice / canDiscountPrice * discount.OffFixPay;
                                discountPrice = Math.Round(discountPrice, 2);
                                dr.Cells["GoodsDiscount"].Value = -discountPrice;
                                offFixedPay += discountPrice;
                            }
                            orderDetails.TotalDiscount = Convert.ToDecimal(dr.Cells["GoodsDiscount"].Value);
                            dr.Cells["OrderDetailsID"].Tag = orderDetails;
                            dr.Cells["GoodsDiscount"].Tag = discount;
                        }
                    }
                }
                if (firstIndex >= 0)
                {
                    decimal gap = discount.OffFixPay - offFixedPay;
                    gap = Math.Round(gap, 2);
                    decimal discountPrice = Math.Abs(Convert.ToDecimal(dgvGoodsOrder.Rows[firstIndex].Cells["GoodsDiscount"].Value));
                    discountPrice += gap;
                    dgvGoodsOrder.Rows[firstIndex].Cells["GoodsDiscount"].Value = -discountPrice;
                    OrderDetails orderDetails = dgvGoodsOrder.Rows[firstIndex].Cells["OrderDetailsID"].Tag as OrderDetails;
                    orderDetails.TotalDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[firstIndex].Cells["GoodsDiscount"].Value);
                    dgvGoodsOrder.Rows[firstIndex].Cells["OrderDetailsID"].Tag = orderDetails;
                }
                //重新计算
                CalculateOrderPrice();
                txtReceAmount.Text = (m_ActualPayMoney + m_ServiceFee).ToString("f2");
            }
        }

        private void btnCutServiceFee_Click(object sender, EventArgs e)
        {
            CrystalButton btn = sender as CrystalButton;
            if (m_CutServiceFee)
            {
                m_CutServiceFee = false;
                btn.Text = "去服务费";
            }
            else
            {
                m_CutServiceFee = true;
                btn.Text = "加服务费";
            }
            //重新计算
            CalculateOrderPrice();
            txtReceAmount.Text = (m_ActualPayMoney + m_ServiceFee).ToString("f2");
        }

        private void CalculateOrderPrice()
        {
            decimal totalPrice = 0, totalDiscount = 0;
            for (int i = 0; i < dgvGoodsOrder.Rows.Count; i++)
            {
                OrderDetails orderDetails = dgvGoodsOrder.Rows[i].Cells["OrderDetailsID"].Tag as OrderDetails;
                totalPrice += orderDetails.ItemQty * orderDetails.SellPrice;
                totalDiscount += Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["GoodsDiscount"].Value);
            }
            m_TotalPrice = totalPrice;
            m_Discount = totalDiscount;
            decimal wholePayMoney = totalPrice + totalDiscount;
            decimal actualPayMoney = CutOffDecimal.HandleCutOff(wholePayMoney, ConstantValuePool.SysConfig.IsCutTail, ConstantValuePool.SysConfig.CutTailType, ConstantValuePool.SysConfig.CutTailDigit);
            m_ActualPayMoney = actualPayMoney;
            m_CutOff = wholePayMoney - actualPayMoney;
            if (m_CutServiceFee)
            {
                m_ServiceFee = 0;
            }
            else
            {
                decimal serviceFee = 0;
                DateTime curTime = Convert.ToDateTime(DateTime.Now.ToString("T"));
                //时段1服务费
                if (curTime > Convert.ToDateTime(ConstantValuePool.SysConfig.ServiceFeeBeginTime1) && curTime < Convert.ToDateTime(ConstantValuePool.SysConfig.ServiceFeeEndTime1))
                {
                    if (ConstantValuePool.SysConfig.FixedServiceFee1 > 0)
                    {
                        serviceFee = ConstantValuePool.SysConfig.FixedServiceFee1;
                    }
                    if (ConstantValuePool.SysConfig.ServiceFeePercent1 > 0)
                    {
                        decimal tempServiceFee = actualPayMoney * ConstantValuePool.SysConfig.ServiceFeePercent1 / 100;
                        serviceFee = CutOffDecimal.HandleCutOff(tempServiceFee, ConstantValuePool.SysConfig.IsCutTail, ConstantValuePool.SysConfig.CutTailType, ConstantValuePool.SysConfig.CutTailDigit);
                    }
                }
                //时段2服务费
                if (curTime > Convert.ToDateTime(ConstantValuePool.SysConfig.ServiceFeeBeginTime2) && curTime < Convert.ToDateTime(ConstantValuePool.SysConfig.ServiceFeeEndTime2))
                {
                    if (ConstantValuePool.SysConfig.FixedServiceFee2 > 0)
                    {
                        serviceFee = ConstantValuePool.SysConfig.FixedServiceFee2;
                    }
                    if (ConstantValuePool.SysConfig.ServiceFeePercent2 > 0)
                    {
                        decimal tempServiceFee = actualPayMoney * ConstantValuePool.SysConfig.ServiceFeePercent2 / 100;
                        serviceFee = CutOffDecimal.HandleCutOff(tempServiceFee, ConstantValuePool.SysConfig.IsCutTail, ConstantValuePool.SysConfig.CutTailType, ConstantValuePool.SysConfig.CutTailDigit);
                    }
                }
                m_ServiceFee = serviceFee;
            }
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            decimal receAmount = decimal.Parse(txtReceAmount.Text);
            decimal paidInAmount = 0;
            if (!string.IsNullOrEmpty(txtPaidInAmount.Text))
            {
                paidInAmount = decimal.Parse(txtPaidInAmount.Text);
            }
            if (receAmount == paidInAmount)
            {
                //计算支付的金额并填充OrderPayoff
                decimal paymentMoney = 0;
                decimal needChangePay = 0;
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
                bool result = ModifyForOrder(orderPayoffList, paymentMoney, needChangePay);
                if (result)
                {
                    m_IsChanged = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("账单修改失败，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("应收金额与实收金额不一致，请重新支付！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_IsChanged = false;
            this.Close();
        }

        private bool ModifyForOrder(List<OrderPayoff> orderPayoffList, decimal paymentMoney, decimal needChangePay)
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
            order.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
            //填充OrderDetails\OrderDiscount
            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            List<OrderDiscount> orderDiscountList = new List<OrderDiscount>();
            for (int i = 0; i < dgvGoodsOrder.RowCount; i++)
            {
                Discount itemDiscount = dgvGoodsOrder.Rows[i].Cells["GoodsDiscount"].Tag as Discount;
                if (itemDiscount != null)
                {
                    decimal itemDiscountPrice = Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["GoodsDiscount"].Value);

                    OrderDetails orderDetails = CopyExtension.Clone<OrderDetails>(m_SalesOrder.orderDetailsList[i]);
                    orderDetails.TotalDiscount = itemDiscountPrice;
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
                    orderDiscount.OffPay = Math.Abs(itemDiscountPrice);
                    orderDiscount.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
                    orderDiscountList.Add(orderDiscount);
                }
            }
            ModifiedPaidOrder modifiedPaidOrder = new ModifiedPaidOrder();
            modifiedPaidOrder.order = order;
            modifiedPaidOrder.orderDetailsList = orderDetailsList;
            modifiedPaidOrder.orderDiscountList = orderDiscountList;
            modifiedPaidOrder.orderPayoffList = orderPayoffList;
            return ModifyOrderService.GetInstance().ModifyForOrder(modifiedPaidOrder);
        }
    }
}
