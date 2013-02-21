using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Top4ever.Entity;
using Top4ever.Domain.OrderRelated;
using Top4ever.CustomControl;
using Top4ever.Entity.Enum;
using Top4ever.Domain.Transfer;
using Top4ever.Common;

namespace Top4ever.Pos.Feature
{
    public partial class FormBackGoods : Form
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

        public FormBackGoods(SalesOrder salesOrder)
        {
            InitializeComponent();
            m_SalesOrder = salesOrder;
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
                    dgvGoodsOrder.Rows[index].Cells["ColQty"].Value = orderDetails.ItemQty;
                    if (orderDetails.ItemType == (int)OrderItemType.Goods)
                    {
                        dgvGoodsOrder.Rows[index].Cells["ColGoodsName"].Value = orderDetails.GoodsName;
                    }
                    else
                    {
                        if (dgvGoodsOrder.Rows[index - 1].Cells["ColGoodsName"].Value.ToString().IndexOf('-') >= 0)
                        {
                            int lastIndex = dgvGoodsOrder.Rows[index - 1].Cells["ColGoodsName"].Value.ToString().LastIndexOf('-');
                            string strDetailFlag = dgvGoodsOrder.Rows[index - 1].Cells["ColGoodsName"].Value.ToString().Substring(0, lastIndex + 1);
                            dgvGoodsOrder.Rows[index].Cells["ColGoodsName"].Value = strDetailFlag + "--" + orderDetails.GoodsName;
                        }
                        else
                        {
                            dgvGoodsOrder.Rows[index].Cells["ColGoodsName"].Value = "--" + orderDetails.GoodsName;
                        }
                    }
                    dgvGoodsOrder.Rows[index].Cells["ColPrice"].Value = orderDetails.TotalSellPrice;
                    dgvGoodsOrder.Rows[index].Cells["ColDiscount"].Value = orderDetails.TotalDiscount;
                    dgvGoodsOrder.Rows[index].Cells["ColFlag"].Value = "-0.0";
                    dgvGoodsOrder.Rows[index].Cells["ColOrderDetailsID"].Value = orderDetails.OrderDetailsID;
                    dgvGoodsOrder.Rows[index].Cells["ColOrderDetailsID"].Tag = orderDetails;
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
                this.txtRealPay.Text = realPay.ToString("f2");
                this.txtPayoffWay.Text = strPayoffWay.Substring(1);
            }
            else
            {
                this.txtRealPay.Text = "0.00";
                this.txtPayoffWay.Text = string.Empty;
            }
        }

        private void btnReal_Click(object sender, EventArgs e)
        {
            if (curPayoffWay != null && !string.IsNullOrEmpty(txtNeedPay.Text))
            {
                decimal remainNeedPay = 0;
                if (string.IsNullOrEmpty(txtRealPay.Text))
                {
                    remainNeedPay = decimal.Parse(txtNeedPay.Text) + m_ServiceFee;
                }
                else
                {
                    remainNeedPay = decimal.Parse(txtNeedPay.Text) + m_ServiceFee - decimal.Parse(txtRealPay.Text);
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
                OrderDetails orderDetails = dgvGoodsOrder.Rows[selectIndex].Cells["ColOrderDetailsID"].Tag as OrderDetails;
                if (orderDetails.ItemType == (int)OrderItemType.Goods)   //主项才能打折
                {
                    if (orderDetails.CanDiscount)
                    {
                        FormDiscount formDiscount = new FormDiscount(DiscountDisplayModel.SingleDiscount);
                        formDiscount.ShowDialog();
                        if (formDiscount.CurrentDiscount != null)
                        {
                            Discount discount = formDiscount.CurrentDiscount;
                            if (discount.DiscountType == (int)DiscountItemType.DiscountRate)
                            {
                                dgvGoodsOrder.Rows[selectIndex].Cells["ColDiscount"].Value = -Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["ColPrice"].Value) * discount.DiscountRate;
                            }
                            else
                            {
                                dgvGoodsOrder.Rows[selectIndex].Cells["ColDiscount"].Value = -discount.OffFixPay;
                            }
                            dgvGoodsOrder.Rows[selectIndex].Cells["ColDiscount"].Tag = discount;
                            //更新细项
                            if (selectIndex < dgvGoodsOrder.Rows.Count - 1)
                            {
                                for (int index = selectIndex + 1; index < dgvGoodsOrder.Rows.Count; index++)
                                {
                                    orderDetails = dgvGoodsOrder.Rows[index].Cells["ColOrderDetailsID"].Tag as OrderDetails;
                                    if (orderDetails.ItemType == (int)OrderItemType.Goods)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        if (discount.DiscountType == (int)DiscountItemType.DiscountRate)
                                        {
                                            dgvGoodsOrder.Rows[index].Cells["ColDiscount"].Value = -Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["ColPrice"].Value) * discount.DiscountRate;
                                        }
                                        else
                                        {
                                            dgvGoodsOrder.Rows[index].Cells["ColDiscount"].Value = -discount.OffFixPay;
                                        }
                                        dgvGoodsOrder.Rows[index].Cells["ColDiscount"].Tag = discount;
                                    }
                                }
                            }
                            //重新计算
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
            FormDiscount formDiscount = new FormDiscount(DiscountDisplayModel.WholeDiscount);
            formDiscount.ShowDialog();
            if (formDiscount.CurrentDiscount != null)
            {
                Discount discount = formDiscount.CurrentDiscount;
                foreach (DataGridViewRow dr in dgvGoodsOrder.Rows)
                {
                    OrderDetails orderDetails = dr.Cells["ColOrderDetailsID"].Tag as OrderDetails;
                    if (orderDetails != null)
                    {
                        if (orderDetails.CanDiscount)
                        {
                            if (discount.DiscountType == (int)DiscountItemType.DiscountRate)
                            {
                                dr.Cells["ColDiscount"].Value = -Convert.ToDecimal(dr.Cells["ColPrice"].Value) * discount.DiscountRate;
                            }
                            else
                            {
                                dr.Cells["ColDiscount"].Value = -discount.OffFixPay;
                            }
                            orderDetails.TotalDiscount = Convert.ToDecimal(dr.Cells["ColDiscount"].Value);
                            dr.Cells["ColOrderDetailsID"].Tag = orderDetails;
                            dr.Cells["ColDiscount"].Tag = discount;
                        }
                    }
                }
                //重新计算
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
        }
    }
}
