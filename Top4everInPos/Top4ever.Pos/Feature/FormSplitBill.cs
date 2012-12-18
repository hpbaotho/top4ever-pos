using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Common;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;
using Top4ever.Entity.Enum;

namespace Top4ever.Pos.Feature
{
    public partial class FormSplitBill : Form
    {
        private SalesOrder m_SalesOrder;
        private bool m_HeadOrBack;
        private Dictionary<string, decimal> dicDiscount = new Dictionary<string, decimal>();
        private bool m_SplitOrderSuccess = false;

        public bool SplitOrderSuccess
        {
            get { return m_SplitOrderSuccess; }
        }

        public FormSplitBill(SalesOrder salesOrder)
        {
            m_SalesOrder = salesOrder;
            InitializeComponent();
            this.btnSave.DisplayColor = this.btnSave.BackColor;
        }

        private void FormSplitBill_Load(object sender, EventArgs e)
        {
            this.btnDeskNo.Text = this.btnDeskNo2.Text = "桌号：" + m_SalesOrder.order.DeskName;
            this.btnEmployee.Text = "服务员：" + m_SalesOrder.order.EmployeeNo;
            this.btnEmployee2.Text = "服务员：" + ConstantValuePool.CurrentEmployee.EmployeeNo;
            this.btnPersonNum.Text = "人数:" + m_SalesOrder.order.PeopleNum;
            BindGoodsOrderInfo(m_SalesOrder);
            BindOrderInfoSum(m_SalesOrder);
            SetSaveButtonEnabled();
        }

        private void FormSplitBill_Shown(object sender, EventArgs e)
        {
            //输入分单人数
            FormNumericKeypad form = new FormNumericKeypad();
            form.DisplayText = "请输入用餐人数";
            form.ShowDialog();
        GotoInputNum:
            if (string.IsNullOrEmpty(form.KeypadValue))
            {
                this.Close();
            }
            else
            {
                if (form.KeypadValue.IndexOf('.') >= 0)
                {
                    MessageBox.Show("请输入整数!");
                    form.ShowDialog();
                    goto GotoInputNum;
                }
                else
                {
                    if (int.Parse(form.KeypadValue) >= m_SalesOrder.order.PeopleNum)
                    {
                        if (DialogResult.Yes == MessageBox.Show("输入的人数超过原单的人数，是否继续？", "分单人数", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        {
                            this.btnPeopleNum.Text = "人数:" + form.KeypadValue;
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        this.btnPeopleNum.Text = "人数:" + form.KeypadValue;
                    }
                }
            }
        }

        private void btnHead_Click(object sender, EventArgs e)
        {
            m_HeadOrBack = true;
            if (TurnSrcOrderToNew(dgvGoodsOrder, dgvGoodsOrder2))
            {
                AmountdgOrderInfoSum(dgvGoodsOrder, dgvGoodsOrderSum);
                AmountdgOrderInfoSum(dgvGoodsOrder2, dgvGoodsOrderSum2);
            }
            SetSaveButtonEnabled();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            m_HeadOrBack = false;
            if (RollbackNewOrderToSrc(dgvGoodsOrder2, dgvGoodsOrder))
            {
                AmountdgOrderInfoSum(dgvGoodsOrder, dgvGoodsOrderSum);
                AmountdgOrderInfoSum(dgvGoodsOrder2, dgvGoodsOrderSum2);
            }
            SetSaveButtonEnabled();
        }

        private void btnPeopleNum_Click(object sender, EventArgs e)
        {
            FormNumericKeypad form = new FormNumericKeypad();
            form.DisplayText = "请输入用餐人数";
            form.ShowDialog();
        GotoInputNum:
            if (string.IsNullOrEmpty(form.KeypadValue))
            {
                this.Close();
            }
            else
            {
                if (form.KeypadValue.IndexOf('.') >= 0)
                {
                    MessageBox.Show("请输入整数!");
                    form.ShowDialog();
                    goto GotoInputNum;
                }
                else
                {
                    if (int.Parse(form.KeypadValue) >= m_SalesOrder.order.PeopleNum)
                    {
                        if (DialogResult.Yes == MessageBox.Show("输入的人数超过原单的人数，是否继续？", "分单人数", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        {
                            this.btnPeopleNum.Text = "人数:" + form.KeypadValue;
                        }
                    }
                    else
                    {
                        this.btnPeopleNum.Text = "人数:" + form.KeypadValue;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder2.Rows.Count > 0)
            {
                string personNum = this.btnPeopleNum.Text;
                if (personNum.IndexOf(':') > 0)
                {
                    personNum = personNum.Substring(personNum.IndexOf(':') + 1);
                }
                //原先单子的价格
                decimal totalPrice, actualPayMoney, discountPrice, cutOff;
                if (dgvGoodsOrderSum.Rows.Count == 2)
                {
                    totalPrice = Convert.ToDecimal(dgvGoodsOrderSum.Rows[0].Cells[1].Value);
                    actualPayMoney = Convert.ToDecimal(dgvGoodsOrderSum.Rows[1].Cells[1].Value);
                    discountPrice = 0;
                    cutOff = totalPrice - actualPayMoney - discountPrice;
                }
                else
                {
                    totalPrice = Convert.ToDecimal(dgvGoodsOrderSum.Rows[0].Cells[1].Value);
                    discountPrice = Convert.ToDecimal(dgvGoodsOrderSum.Rows[1].Cells[1].Value);
                    actualPayMoney = Convert.ToDecimal(dgvGoodsOrderSum.Rows[2].Cells[1].Value);
                    cutOff = totalPrice - actualPayMoney - Math.Abs(discountPrice);
                }
                Order originalOrder = new Order();
                originalOrder.OrderID = m_SalesOrder.order.OrderID;
                originalOrder.TotalSellPrice = totalPrice;
                originalOrder.ActualSellPrice = actualPayMoney;
                originalOrder.DiscountPrice = discountPrice;
                originalOrder.CutOffPrice = cutOff;
                int remainPeopleNum = m_SalesOrder.order.PeopleNum - int.Parse(personNum);
                if (remainPeopleNum <= 0) remainPeopleNum = 1;
                originalOrder.PeopleNum = remainPeopleNum;
                originalOrder.OrderLastTime = m_SalesOrder.order.OrderLastTime / 2;
                List<OrderDetails> subOrderDetailsList = new List<OrderDetails>();
                foreach (DataGridViewRow dr in dgvGoodsOrder2.Rows)
                {
                    string orderDetailsID = dr.Cells[4].Value.ToString();
                    //填充OrderDetails
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.OrderDetailsID = new Guid(orderDetailsID);
                    orderDetails.TotalSellPrice = Convert.ToDecimal(dr.Cells[2].Value);
                    orderDetails.TotalDiscount = GetDiscountFromDic(orderDetailsID);
                    orderDetails.ItemQty = Convert.ToDecimal(dr.Cells[0].Value);
                    subOrderDetailsList.Add(orderDetails);
                }
                //新单子
                decimal newTotalPrice, newActualPayMoney, newDiscountPrice, newCutOff;
                if (dgvGoodsOrderSum2.Rows.Count == 2)
                {
                    newTotalPrice = Convert.ToDecimal(dgvGoodsOrderSum2.Rows[0].Cells[1].Value);
                    newActualPayMoney = Convert.ToDecimal(dgvGoodsOrderSum2.Rows[1].Cells[1].Value);
                    newDiscountPrice = 0;
                    newCutOff = newTotalPrice - newActualPayMoney - newDiscountPrice;
                }
                else
                {
                    newTotalPrice = Convert.ToDecimal(dgvGoodsOrderSum2.Rows[0].Cells[1].Value);
                    newDiscountPrice = Convert.ToDecimal(dgvGoodsOrderSum2.Rows[1].Cells[1].Value);
                    newActualPayMoney = Convert.ToDecimal(dgvGoodsOrderSum2.Rows[2].Cells[1].Value);
                    newCutOff = newTotalPrice - newActualPayMoney - Math.Abs(newDiscountPrice);
                }
                Order newOrder = new Order();
                newOrder.OrderID = Guid.NewGuid();
                newOrder.TotalSellPrice = newTotalPrice;
                newOrder.ActualSellPrice = newActualPayMoney;
                newOrder.DiscountPrice = newDiscountPrice;
                newOrder.CutOffPrice = newCutOff;
                newOrder.ServiceFee = 0;
                newOrder.DeviceNo = ConstantValuePool.BizSettingConfig.DeviceNo;
                newOrder.DeskName = m_SalesOrder.order.DeskName;
                newOrder.EatType = m_SalesOrder.order.EatType;
                newOrder.Status = 0;
                newOrder.PeopleNum = int.Parse(personNum);
                newOrder.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
                newOrder.EmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                newOrder.OrderLastTime = m_SalesOrder.order.OrderLastTime / 2;
                List<OrderDetails> newOrderDetailsList = new List<OrderDetails>();
                foreach (DataGridViewRow dr in dgvGoodsOrder2.Rows)
                {
                    string goodsName = dr.Cells[1].Value.ToString();
                    string orderDetailsID = dr.Cells[4].Value.ToString();
                    int itemType = Convert.ToInt32(dr.Cells[5].Value);
                    //填充OrderDetails
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.OrderDetailsID = Guid.NewGuid();
                    orderDetails.OrderID = newOrder.OrderID;
                    orderDetails.DeviceNo = ConstantValuePool.BizSettingConfig.DeviceNo;
                    orderDetails.TotalSellPrice = Convert.ToDecimal(dr.Cells[2].Value);
                    orderDetails.TotalDiscount = 0;
                    orderDetails.ItemQty = Convert.ToDecimal(dr.Cells[0].Value);
                    orderDetails.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
                    foreach (OrderDetails item in m_SalesOrder.orderDetailsList)
                    {
                        if (item.OrderDetailsID.ToString() == orderDetailsID)
                        {
                            orderDetails.Wait = item.Wait;
                            orderDetails.ItemType = item.ItemType;
                            orderDetails.GoodsID = item.GoodsID;
                            orderDetails.GoodsNo = item.GoodsNo;
                            orderDetails.GoodsName = item.GoodsName;
                            orderDetails.CanDiscount = item.CanDiscount;
                            orderDetails.Unit = item.Unit;
                            orderDetails.SellPrice = item.SellPrice;
                            orderDetails.PrintSolutionName = item.PrintSolutionName;
                            orderDetails.DepartID = item.DepartID;
                            orderDetails.ItemLevel = item.ItemLevel;
                            break;
                        }
                    }
                    newOrderDetailsList.Add(orderDetails);
                }
                SalesSplitOrder salesSplitOrder = new SalesSplitOrder();
                salesSplitOrder.OriginalOrder = originalOrder;
                salesSplitOrder.SubOrderDetailsList = subOrderDetailsList;
                salesSplitOrder.NewOrder = newOrder;
                salesSplitOrder.NewOrderDetailsList = newOrderDetailsList;
                SalesOrderService salesOrderService = new SalesOrderService();
                if (salesOrderService.SplitSalesOrder(salesSplitOrder))
                {
                    m_SplitOrderSuccess = true;
                    this.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetSaveButtonEnabled()
        {
            if (dgvGoodsOrder2.Rows.Count > 0)
            {
                this.btnSave.Enabled = true;
                this.btnSave.BackColor = this.btnSave.DisplayColor;
            }
            else
            {
                this.btnSave.Enabled = false;
                this.btnSave.BackColor = ConstantValuePool.DisabledColor;
            }
        }

        #region 私有方法

        private void BindGoodsOrderInfo(SalesOrder salesOrder)
        {
            if (salesOrder.orderDetailsList != null && salesOrder.orderDetailsList.Count > 0)
            {
                foreach (OrderDetails orderDetails in salesOrder.orderDetailsList)
                {
                    int index = dgvGoodsOrder.Rows.Add(new DataGridViewRow());
                    dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value = orderDetails.ItemQty;
                    string restOrderFlag = string.Empty;
                    if (orderDetails.Wait == 1)
                    {
                        restOrderFlag = "*";
                    }
                    if (orderDetails.ItemType == (int)OrderItemType.Goods)
                    {
                        dgvGoodsOrder.Rows[index].Cells["GoodsName"].Value = restOrderFlag + orderDetails.GoodsName;
                    }
                    else
                    {
                        string strLevelFlag = string.Empty;
                        int levelCount = orderDetails.ItemLevel * 2;
                        for (int i = 0; i < levelCount; i++)
                        {
                            strLevelFlag += "-";
                        }
                        dgvGoodsOrder.Rows[index].Cells["GoodsName"].Value = strLevelFlag + restOrderFlag + orderDetails.GoodsName;
                    }
                    dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value = orderDetails.TotalSellPrice;
                    dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value = orderDetails.TotalDiscount;
                    dgvGoodsOrder.Rows[index].Cells["OrderDetailsID"].Value = orderDetails.OrderDetailsID;
                    dgvGoodsOrder.Rows[index].Cells["ItemType"].Value = orderDetails.ItemType;
                }
            }
        }

        private void BindOrderInfoSum(SalesOrder salesOrder)
        {
            int index = (int)dgvGoodsOrderSum.Rows.Add(new DataGridViewRow());
            dgvGoodsOrderSum.Rows[index].Cells[0].Value = "金额合计";
            dgvGoodsOrderSum.Rows[index].Cells[1].Value = salesOrder.order.TotalSellPrice;
            if (Math.Abs(salesOrder.order.DiscountPrice) > 0)
            {
                index = (int)dgvGoodsOrderSum.Rows.Add(new DataGridViewRow());
                dgvGoodsOrderSum.Rows[index].Cells[0].Value = "折扣合计";
                dgvGoodsOrderSum.Rows[index].Cells[1].Value = salesOrder.order.DiscountPrice;
            }
            index = (int)dgvGoodsOrderSum.Rows.Add(new DataGridViewRow());
            dgvGoodsOrderSum.Rows[index].Cells[0].Value = "实际应付";
            dgvGoodsOrderSum.Rows[index].Cells[1].Value = salesOrder.order.ActualSellPrice;
        }

        private void AmountdgOrderInfoSum(DataGridView dgvGoodsOrder, DataGridView dgvGoodsOrderSum)
        {
            dgvGoodsOrderSum.Rows.Clear();
            decimal totalPrice = 0, totalDiscount = 0;
            for (int i = 0; i < dgvGoodsOrder.Rows.Count; i++)
            {
                totalPrice += Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells[2].Value);
                totalDiscount += Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells[3].Value);
            }
            int rowIndex = dgvGoodsOrderSum.Rows.Add(new DataGridViewRow());
            dgvGoodsOrderSum.Rows[rowIndex].Cells[0].Value = "金额合计";
            dgvGoodsOrderSum.Rows[rowIndex].Cells[1].Value = totalPrice;
            if (Math.Abs(totalDiscount) > 0)
            {
                rowIndex = dgvGoodsOrderSum.Rows.Add(new DataGridViewRow());
                dgvGoodsOrderSum.Rows[rowIndex].Cells[0].Value = "折扣合计";
                dgvGoodsOrderSum.Rows[rowIndex].Cells[1].Value = totalDiscount;
            }
            rowIndex = dgvGoodsOrderSum.Rows.Add(new DataGridViewRow());
            dgvGoodsOrderSum.Rows[rowIndex].Cells[0].Value = "实际应付";
            decimal actualSellPrice = CutOffDecimal.HandleCutOff(totalPrice + totalDiscount, CutOffType.ROUND_OFF, 0);
            dgvGoodsOrderSum.Rows[rowIndex].Cells[1].Value = actualSellPrice;
        }

        /// <summary>
        /// 将原来的菜单分到新单上
        /// </summary>
        private bool TurnSrcOrderToNew(DataGridView gridView1, DataGridView gridView2)
        {
            if (gridView1.CurrentRow != null && gridView1.CurrentRow.Index >= 0 && gridView1.Rows.Count > 1)
            {
                int selectedIndex = gridView1.CurrentRow.Index;
                int goodsType = Convert.ToInt32(gridView1.Rows[selectedIndex].Cells[5].Value);
                if (goodsType != (int)OrderItemType.Goods)
                {
                    return false;
                }
                decimal goodsNum = Convert.ToDecimal(gridView1.Rows[selectedIndex].Cells[0].Value);
                string goodsName = gridView1.Rows[selectedIndex].Cells[1].Value.ToString();
                decimal goodsPrice = Convert.ToDecimal(gridView1.Rows[selectedIndex].Cells[2].Value);
                decimal goodsDiscount = Convert.ToDecimal(gridView1.Rows[selectedIndex].Cells[3].Value);
                string orderDetailsID = gridView1.Rows[selectedIndex].Cells[4].Value.ToString();
                decimal splitOrderQty = 0;
                //更新gridView1中的数据
                if (goodsNum > 1)
                {
                    FormNumericKeypad form = new FormNumericKeypad();
                    form.DisplayText = "分单份数";
                    form.ShowDialog();
                    if (!string.IsNullOrEmpty(form.KeypadValue))
                    {
                        splitOrderQty = decimal.Parse(form.KeypadValue);
                    }
                    while (splitOrderQty > goodsNum)
                    {
                        MessageBox.Show("选择分单数量有误！");
                        splitOrderQty = 0;
                        form.ShowDialog();
                        if (!string.IsNullOrEmpty(form.KeypadValue))
                        {
                            splitOrderQty = decimal.Parse(form.KeypadValue);
                        }
                    }
                    if (splitOrderQty > 0)
                    {
                        if (splitOrderQty == goodsNum)
                        {
                            gridView1.Rows.RemoveAt(selectedIndex);
                        }
                        else
                        {
                            gridView1.Rows[selectedIndex].Cells[0].Value = goodsNum - splitOrderQty;
                            gridView1.Rows[selectedIndex].Cells[2].Value = goodsPrice - (goodsPrice / goodsNum) * splitOrderQty;
                            gridView1.Rows[selectedIndex].Cells[3].Value = goodsDiscount - (goodsDiscount / goodsNum) * splitOrderQty;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    gridView1.Rows.RemoveAt(selectedIndex);
                }
                //更新gridView2中的数据
                int containIndex = -1;
                for (int index = 0; index < gridView2.Rows.Count; index++)
                {
                    if (gridView2.Rows[index].Cells[4].Value.ToString() == orderDetailsID)
                    {
                        containIndex = index;
                        break;
                    }
                }
                if (containIndex != -1)
                {
                    decimal goodsNum2 = Convert.ToDecimal(gridView2.Rows[containIndex].Cells[0].Value);
                    decimal goodsPrice2 = Convert.ToDecimal(gridView2.Rows[containIndex].Cells[2].Value);
                    if (goodsNum > 1)
                    {
                        gridView2.Rows[containIndex].Cells[0].Value = goodsNum2 + splitOrderQty;
                        gridView2.Rows[containIndex].Cells[2].Value = goodsPrice2 + (goodsPrice / goodsNum) * splitOrderQty;
                        AppendDiscountToDic(orderDetailsID, goodsDiscount / goodsNum * splitOrderQty);
                    }
                    else
                    {
                        gridView2.Rows[containIndex].Cells[0].Value = goodsNum2 + goodsNum;
                        gridView2.Rows[containIndex].Cells[2].Value = goodsPrice2 + goodsPrice;
                        AppendDiscountToDic(orderDetailsID, goodsDiscount);
                    }
                }
                else
                {
                    int rowIndex = gridView2.Rows.Add(new DataGridViewRow());
                    if (goodsNum > 1)
                    {
                        gridView2.Rows[rowIndex].Cells[0].Value = splitOrderQty;
                        gridView2.Rows[rowIndex].Cells[1].Value = goodsName;
                        gridView2.Rows[rowIndex].Cells[2].Value = goodsPrice / goodsNum * splitOrderQty;
                        gridView2.Rows[rowIndex].Cells[3].Value = 0;
                        gridView2.Rows[rowIndex].Cells[4].Value = orderDetailsID;
                        gridView2.Rows[rowIndex].Cells[5].Value = goodsType;
                        AppendDiscountToDic(orderDetailsID, (goodsDiscount / goodsNum) * splitOrderQty);
                    }
                    else
                    {
                        gridView2.Rows[rowIndex].Cells[0].Value = goodsNum;
                        gridView2.Rows[rowIndex].Cells[1].Value = goodsName;
                        gridView2.Rows[rowIndex].Cells[2].Value = goodsPrice;
                        gridView2.Rows[rowIndex].Cells[3].Value = 0;
                        gridView2.Rows[rowIndex].Cells[4].Value = orderDetailsID;
                        gridView2.Rows[rowIndex].Cells[5].Value = goodsType;
                        AppendDiscountToDic(orderDetailsID, goodsDiscount);
                    }
                }
                //同时移动细项
                if (goodsNum > 1)
                {
                    if (goodsNum > splitOrderQty)
                    {
                        RemoveDetailItem(gridView1, gridView2, selectedIndex, containIndex, splitOrderQty);
                    }
                    else
                    {
                        RemoveDetailItem(gridView1, gridView2, selectedIndex, containIndex);
                    }
                }
                else
                {
                    RemoveDetailItem(gridView1, gridView2, selectedIndex, containIndex);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 将新单回滚到原来单子上
        /// </summary>
        private bool RollbackNewOrderToSrc(DataGridView gridView1, DataGridView gridView2)
        {
            if (gridView1.CurrentRow != null && gridView1.CurrentRow.Index >= 0)
            {
                int selectedIndex = gridView1.CurrentRow.Index;
                int goodsType = Convert.ToInt32(gridView1.Rows[selectedIndex].Cells[5].Value);
                if (goodsType != (int)OrderItemType.Goods)
                {
                    return false;
                }
                decimal goodsNum = Convert.ToDecimal(gridView1.Rows[selectedIndex].Cells[0].Value);
                string goodsName = gridView1.Rows[selectedIndex].Cells[1].Value.ToString();
                decimal goodsPrice = Convert.ToDecimal(gridView1.Rows[selectedIndex].Cells[2].Value);
                string orderDetailsID = gridView1.Rows[selectedIndex].Cells[4].Value.ToString();
                decimal splitOrderQty = 0;
                //更新gridView1中的数据
                if (goodsNum > 1)
                {
                    FormNumericKeypad form = new FormNumericKeypad();
                    form.DisplayText = "请输入分单份数";
                    form.ShowDialog();
                    if (!string.IsNullOrEmpty(form.KeypadValue))
                    {
                        splitOrderQty = decimal.Parse(form.KeypadValue);
                    }
                    while (splitOrderQty > goodsNum)
                    {
                        MessageBox.Show("选择分单数量有误！");
                        splitOrderQty = 0;
                        form.ShowDialog();
                        if (!string.IsNullOrEmpty(form.KeypadValue))
                        {
                            splitOrderQty = decimal.Parse(form.KeypadValue);
                        }
                    }
                    if (splitOrderQty > 0)
                    {
                        if (splitOrderQty == goodsNum)
                        {
                            gridView1.Rows.RemoveAt(selectedIndex);
                        }
                        else
                        {
                            gridView1.Rows[selectedIndex].Cells[0].Value = goodsNum - splitOrderQty;
                            gridView1.Rows[selectedIndex].Cells[2].Value = goodsPrice - goodsPrice / goodsNum * splitOrderQty;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    gridView1.Rows.RemoveAt(selectedIndex);
                }

                //更新gridView2中的数据
                int containIndex = -1;
                for (int index = 0; index < gridView2.Rows.Count; index++)
                {
                    if (gridView2.Rows[index].Cells[4].Value.ToString() == orderDetailsID)
                    {
                        containIndex = index;
                        break;
                    }
                }
                if (containIndex != -1)
                {
                    decimal goodsNum2 = Convert.ToDecimal(gridView2.Rows[containIndex].Cells[0].Value);
                    decimal goodsPrice2 = Convert.ToDecimal(gridView2.Rows[containIndex].Cells[2].Value);
                    decimal goodsDiscount2 = Convert.ToDecimal(gridView2.Rows[containIndex].Cells[3].Value);

                    if (goodsNum > 1)
                    {
                        gridView2.Rows[containIndex].Cells[0].Value = goodsNum2 + splitOrderQty;
                        gridView2.Rows[containIndex].Cells[2].Value = goodsPrice2 + goodsPrice / goodsNum * splitOrderQty;
                        decimal totalDiscount = GetDiscountFromDic(orderDetailsID);
                        gridView2.Rows[containIndex].Cells[3].Value = goodsDiscount2 + totalDiscount / goodsNum * splitOrderQty;
                        SubtractDiscountToDic(orderDetailsID, totalDiscount / goodsNum * splitOrderQty);
                    }
                    else
                    {
                        gridView2.Rows[containIndex].Cells[0].Value = goodsNum2 + goodsNum;
                        gridView2.Rows[containIndex].Cells[2].Value = goodsPrice2 + goodsPrice;
                        decimal totalDiscount = GetDiscountFromDic(orderDetailsID);
                        gridView2.Rows[containIndex].Cells[3].Value = goodsDiscount2 + totalDiscount;
                        SubtractDiscountToDic(orderDetailsID, totalDiscount);
                    }
                }
                else
                {
                    int rowIndex = gridView2.Rows.Add(new DataGridViewRow());
                    if (goodsNum > 1)
                    {
                        gridView2.Rows[rowIndex].Cells[0].Value = splitOrderQty;
                        gridView2.Rows[rowIndex].Cells[1].Value = goodsName;
                        gridView2.Rows[rowIndex].Cells[2].Value = goodsPrice / goodsNum * splitOrderQty;
                        decimal totalDiscount = GetDiscountFromDic(orderDetailsID);
                        gridView2.Rows[rowIndex].Cells[3].Value = totalDiscount / goodsNum * splitOrderQty;
                        gridView2.Rows[rowIndex].Cells[4].Value = orderDetailsID;
                        gridView2.Rows[rowIndex].Cells[5].Value = goodsType;
                        SubtractDiscountToDic(orderDetailsID, totalDiscount / goodsNum * splitOrderQty);
                    }
                    else
                    {
                        gridView2.Rows[rowIndex].Cells[0].Value = goodsNum;
                        gridView2.Rows[rowIndex].Cells[1].Value = goodsName;
                        gridView2.Rows[rowIndex].Cells[2].Value = goodsPrice;
                        decimal totalDiscount = GetDiscountFromDic(orderDetailsID);
                        gridView2.Rows[rowIndex].Cells[3].Value = totalDiscount;
                        gridView2.Rows[rowIndex].Cells[4].Value = orderDetailsID;
                        gridView2.Rows[rowIndex].Cells[5].Value = goodsType;
                        SubtractDiscountToDic(orderDetailsID, totalDiscount);
                    }
                }
                //同时移动细项
                if (goodsNum > 1)
                {
                    if (goodsNum > splitOrderQty)
                    {
                        RemoveDetailItem(gridView1, gridView2, selectedIndex, containIndex, splitOrderQty);
                    }
                    else
                    {
                        RemoveDetailItem(gridView1, gridView2, selectedIndex, containIndex);
                    }
                }
                else
                {
                    RemoveDetailItem(gridView1, gridView2, selectedIndex, containIndex);
                }
                //设置最后一行选中
                if (gridView1.Rows.Count > 0)
                {
                    gridView1.Rows[gridView1.Rows.Count - 1].Selected = true;
                    gridView1.CurrentCell = gridView1[0, gridView1.Rows.Count - 1];
                }
                if (gridView2.Rows.Count > 0)
                {
                    gridView2.Rows[gridView2.Rows.Count - 1].Selected = true;
                    gridView2.CurrentCell = gridView2[0, gridView2.Rows.Count - 1];
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void AppendDiscountToDic(string orderDetailsID, decimal goodsDiscount)
        {
            bool isContain = false;
            foreach (KeyValuePair<string, decimal> item in dicDiscount)
            {
                if (item.Key.Equals(orderDetailsID))
                {
                    isContain = true;
                    dicDiscount[item.Key] = item.Value + goodsDiscount;
                    break;
                }
            }
            if (!isContain)
            {
                dicDiscount.Add(orderDetailsID, goodsDiscount);
            }
        }

        private void SubtractDiscountToDic(string orderDetailsID, decimal goodsDiscount)
        {
            foreach (KeyValuePair<string, decimal> item in dicDiscount)
            {
                if (item.Key.Equals(orderDetailsID))
                {
                    dicDiscount[item.Key] = item.Value - goodsDiscount;
                    break;
                }
            }
        }

        private decimal GetDiscountFromDic(string orderDetailsID)
        {
            decimal totalDiscount = 0;
            if (dicDiscount.ContainsKey(orderDetailsID))
            {
                totalDiscount = dicDiscount[orderDetailsID];
            }
            return totalDiscount;
        }

        //移动全部的细项或子项
        private void RemoveDetailItem(DataGridView gridView1, DataGridView gridView2, int selectedIndex, int containIndex)
        {
            if (selectedIndex >= gridView1.Rows.Count) return;
            //因为主项被全部移动，所以selectedIndex不需要+1
            int goodsType = Convert.ToInt32(gridView1.Rows[selectedIndex].Cells[5].Value);
            while (goodsType != (int)OrderItemType.Goods)
            {
                //更新gridView2中的数据
                if (containIndex != -1)
                {
                    containIndex++;
                    decimal goodsNum = Convert.ToDecimal(gridView1.Rows[selectedIndex].Cells[0].Value);
                    decimal goodsPrice = Convert.ToDecimal(gridView1.Rows[selectedIndex].Cells[2].Value);
                    decimal goodsDiscount = 0;
                    decimal goodsNum2 = Convert.ToDecimal(gridView2.Rows[containIndex].Cells[0].Value);
                    decimal goodsPrice2 = Convert.ToDecimal(gridView2.Rows[containIndex].Cells[2].Value);
                    decimal goodsDiscount2 = 0;
                    gridView2.Rows[containIndex].Cells[0].Value = goodsNum2 + goodsNum;
                    gridView2.Rows[containIndex].Cells[2].Value = goodsPrice2 + goodsPrice;

                    if (m_HeadOrBack)
                    {
                        goodsDiscount = Convert.ToDecimal(gridView1.Rows[selectedIndex].Cells[3].Value);
                        goodsDiscount2 = Convert.ToDecimal(gridView2.Rows[containIndex].Cells[3].Tag);
                        gridView2.Rows[containIndex].Cells[3].Tag = goodsDiscount2 + goodsDiscount / goodsNum;
                    }
                    else
                    {
                        goodsDiscount = Convert.ToDecimal(gridView1.Rows[selectedIndex].Cells[3].Tag);
                        goodsDiscount2 = Convert.ToDecimal(gridView2.Rows[containIndex].Cells[3].Value);
                        gridView2.Rows[containIndex].Cells[3].Value = goodsDiscount2 + goodsDiscount / goodsNum;
                    }
                }
                else
                {
                    int rowIndex = gridView2.Rows.Add(new DataGridViewRow());
                    gridView2.Rows[rowIndex].Cells[0].Value = gridView1.Rows[selectedIndex].Cells[0].Value;
                    gridView2.Rows[rowIndex].Cells[1].Value = gridView1.Rows[selectedIndex].Cells[1].Value;
                    gridView2.Rows[rowIndex].Cells[2].Value = gridView1.Rows[selectedIndex].Cells[2].Value;
                    if (m_HeadOrBack)
                    {
                        gridView2.Rows[rowIndex].Cells[3].Tag = gridView1.Rows[selectedIndex].Cells[3].Value;
                    }
                    else
                    {
                        gridView2.Rows[rowIndex].Cells[3].Value = gridView1.Rows[selectedIndex].Cells[3].Tag;
                    }
                    gridView2.Rows[rowIndex].Cells[4].Value = gridView1.Rows[selectedIndex].Cells[4].Value;
                    gridView2.Rows[rowIndex].Cells[5].Value = gridView1.Rows[selectedIndex].Cells[5].Value;
                }
                gridView1.Rows.RemoveAt(selectedIndex);
                //因为该细项被全部移动，所以selectedIndex不需要+1
                if (selectedIndex < gridView1.Rows.Count)
                {
                    goodsType = Convert.ToInt32(gridView1.Rows[selectedIndex].Cells[5].Value);
                }
                else
                {
                    break;
                }
            }
        }

        //移除部分的细项
        private void RemoveDetailItem(DataGridView gridView1, DataGridView gridView2, int selectedIndex, int containIndex, decimal splitOrderQty)
        {
            int goodsType = -1;
            while (true)
            {
                selectedIndex++;
                if (selectedIndex >= gridView1.Rows.Count)
                {
                    break;
                }
                else
                {
                    goodsType = Convert.ToInt32(gridView1.Rows[selectedIndex].Cells[5].Value);
                    if (goodsType == (int)OrderItemType.Goods)
                    {
                        break;
                    }
                    else
                    {
                        decimal goodsNum = Convert.ToDecimal(gridView1.Rows[selectedIndex].Cells[0].Value);
                        decimal goodsPrice = Convert.ToDecimal(gridView1.Rows[selectedIndex].Cells[2].Value);
                        decimal goodsDiscount = 0;
                        //更新gridView2中的数据
                        if (containIndex != -1)
                        {
                            decimal goodsNum2 = Convert.ToDecimal(gridView2.Rows[containIndex].Cells[0].Value);
                            decimal goodsPrice2 = Convert.ToDecimal(gridView2.Rows[containIndex].Cells[2].Value);
                            decimal goodsDiscount2 = 0;
                            gridView2.Rows[containIndex].Cells[0].Value = goodsNum2 + splitOrderQty;
                            gridView2.Rows[containIndex].Cells[2].Value = goodsPrice2 + goodsPrice / goodsNum * splitOrderQty;
                            if (m_HeadOrBack)
                            {
                                goodsDiscount2 = Convert.ToDecimal(gridView2.Rows[containIndex].Cells[3].Tag);
                                gridView2.Rows[containIndex].Cells[3].Tag = goodsDiscount2 + goodsDiscount2 / goodsNum * splitOrderQty;
                            }
                            else
                            {
                                goodsDiscount2 = Convert.ToDecimal(gridView2.Rows[containIndex].Cells[3].Value);
                                gridView2.Rows[containIndex].Cells[3].Value = goodsDiscount2 + goodsDiscount2 / goodsNum * splitOrderQty;
                            }
                        }
                        else
                        {
                            int rowIndex = gridView2.Rows.Add(new DataGridViewRow());
                            gridView2.Rows[rowIndex].Cells[0].Value = splitOrderQty;
                            gridView2.Rows[rowIndex].Cells[1].Value = gridView1.Rows[selectedIndex].Cells[1].Value;
                            gridView2.Rows[rowIndex].Cells[2].Value = goodsPrice / goodsNum * splitOrderQty;
                            if (m_HeadOrBack)
                            {
                                goodsDiscount = Convert.ToDecimal(gridView1.Rows[selectedIndex].Cells[3].Value);
                                gridView2.Rows[rowIndex].Cells[3].Tag = goodsDiscount / goodsNum * splitOrderQty;
                                gridView1.Rows[selectedIndex].Cells[3].Value = goodsDiscount - goodsDiscount / goodsNum * splitOrderQty;
                            }
                            else
                            {
                                goodsDiscount = Convert.ToDecimal(gridView1.Rows[selectedIndex].Cells[3].Tag);
                                gridView2.Rows[rowIndex].Cells[3].Value = goodsDiscount - goodsDiscount / goodsNum * splitOrderQty;
                                gridView1.Rows[selectedIndex].Cells[3].Tag = goodsDiscount - goodsDiscount / goodsNum * splitOrderQty;
                            }
                            gridView2.Rows[rowIndex].Cells[4].Value = gridView1.Rows[selectedIndex].Cells[4].Value;
                            gridView2.Rows[rowIndex].Cells[5].Value = gridView1.Rows[selectedIndex].Cells[5].Value;
                        }
                        //更新gridView1中的数据
                        gridView1.Rows[selectedIndex].Cells[0].Value = goodsNum - splitOrderQty;
                        gridView1.Rows[selectedIndex].Cells[2].Value = goodsPrice - goodsPrice / goodsNum * splitOrderQty;
                    }
                }
            }
        }

        #endregion
    }
}
