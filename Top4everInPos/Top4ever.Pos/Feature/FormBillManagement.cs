using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;
using Top4ever.Entity.Enum;
using Top4ever.Print;
using Top4ever.Print.Entity;

namespace VechsoftPos.Feature
{
    public partial class FormBillManagement : Form
    {
        /// <summary>
        /// 1:输入台号 2:输入流水号
        /// </summary>
        private int _seachType;
        private int _pageIndex;
        private int _pageSize = 25;
        private SalesOrder _salesOrder;

        public FormBillManagement()
        {
            InitializeComponent();
            ResizeStyle();
        }

        private void FormBillManagement_Load(object sender, EventArgs e)
        {
            this.txtSearchValue.Text = string.Empty;
            this.lbOrderNo.Text = string.Empty;
            this.lbBillType.Text = string.Empty;
            this.lbDeskName.Text = string.Empty;
            this.lbEatType.Text = string.Empty;
            this.lbEmployeeNo.Text = string.Empty;
            this.lbCashier.Text = string.Empty;
            this.lbDeviceNo.Text = string.Empty;
            this.lbPage.Text = "第 1 页";
            this.lbBillIndex.Text = "0/0";
            this.btnPageUp.DisplayColor = this.btnPageUp.BackColor;
            this.btnPageDown.DisplayColor = this.btnPageDown.BackColor;
            this.btnPageUp.Enabled = false;
            this.btnPageUp.BackColor = ConstantValuePool.DisabledColor;
            this.btnPageDown.Enabled = false;
            this.btnPageDown.BackColor = ConstantValuePool.DisabledColor;
            btnBillModify.BackColor = btnBillModify.DisplayColor = Color.SteelBlue;
            btnSingleDelete.BackColor = btnSingleDelete.DisplayColor = Color.SteelBlue;
            btnWholeDelete.BackColor = btnWholeDelete.DisplayColor = Color.SteelBlue;
        }

        private void btnDeskNo_Click(object sender, EventArgs e)
        {
            FormNumericKeypad keyForm = new FormNumericKeypad(false);
            keyForm.DisplayText = "请输入台号";
            keyForm.ShowDialog();
            if (!string.IsNullOrEmpty(keyForm.KeypadValue))
            {
                _seachType = 1;
                this.txtSearchValue.Text = keyForm.KeypadValue;
            }
        }

        private void btnTranSequence_Click(object sender, EventArgs e)
        {
            FormNumericKeypad keyForm = new FormNumericKeypad(false);
            keyForm.DisplayText = "请输入流水号";
            keyForm.ShowDialog();
            if (!string.IsNullOrEmpty(keyForm.KeypadValue))
            {
                _seachType = 2;
                this.txtSearchValue.Text = keyForm.KeypadValue;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            _pageIndex = 0;
            SearchSalesOrder();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _pageIndex = 0;
            _seachType = 0;
            this.txtSearchValue.Text = string.Empty;
            this.lbOrderNo.Text = string.Empty;
            this.lbBillType.Text = string.Empty;
            this.lbDeskName.Text = string.Empty;
            this.lbEatType.Text = string.Empty;
            this.lbEmployeeNo.Text = string.Empty;
            this.lbCashier.Text = string.Empty;
            this.lbDeviceNo.Text = string.Empty;
            this.lbPage.Text = "第 1 页";
            this.lbBillIndex.Text = "0/0";
            this.btnPageUp.Enabled = false;
            this.btnPageUp.BackColor = ConstantValuePool.DisabledColor;
            this.btnPageDown.Enabled = false;
            this.btnPageDown.BackColor = ConstantValuePool.DisabledColor;
            this.dataGridView1.SelectionChanged -= new System.EventHandler(this.dataGridView1_SelectionChanged);
            this.dataGridView1.Rows.Clear();
            this.dataGridView2.Rows.Clear();
            this.dataGridView3.Rows.Clear();
        }

        private void SearchSalesOrder()
        {
            string strWhere = string.Empty;
            if (_seachType == 1)
            {
                strWhere = " DeskName = '" + this.txtSearchValue.Text + "'";
            }
            if (_seachType == 2)
            {
                strWhere = " TranSequence = " + this.txtSearchValue.Text;
                //string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                //strWhere += " AND (PayTime >= '" + currentDate + "' AND PayTime < DATEADD(DAY,1,'" + currentDate + "') )";
            }
            IList<Order> orderList = OrderService.GetInstance().GetOrderListBySearch(strWhere, string.Empty, _pageIndex, _pageSize);
            if (_pageIndex > 0)
            {
                this.btnPageUp.Enabled = true;
                this.btnPageUp.BackColor = this.btnPageUp.DisplayColor;
            }
            else
            {
                this.btnPageUp.Enabled = false;
                this.btnPageUp.BackColor = ConstantValuePool.DisabledColor;
            }
            if (orderList == null || orderList.Count < _pageSize)
            {
                this.btnPageDown.Enabled = false;
                this.btnPageDown.BackColor = ConstantValuePool.DisabledColor;
            }
            else
            {
                this.btnPageDown.Enabled = true;
                this.btnPageDown.BackColor = this.btnPageDown.DisplayColor;
            }
            if (orderList != null && orderList.Count > 0)
            {
                this.lbPage.Text = string.Format("第 {0} 页", _pageIndex + 1);
                int startIndex = _pageIndex * _pageSize + 1;
                int endIndex = (_pageIndex + 1) * _pageSize;
                if (orderList.Count < _pageSize)
                {
                    endIndex = endIndex - (_pageSize - orderList.Count);
                }
                this.lbBillIndex.Text = startIndex + "/" + endIndex;
                BindDataGridView1(orderList);
            }
            else
            {
                this.dataGridView1.SelectionChanged -= new System.EventHandler(this.dataGridView1_SelectionChanged);
                this.dataGridView1.Rows.Clear();
                this.dataGridView2.Rows.Clear();
                this.dataGridView3.Rows.Clear();
                this.lbOrderNo.Text = string.Empty;
                this.lbBillType.Text = string.Empty;
                this.lbDeskName.Text = string.Empty;
                this.lbEatType.Text = string.Empty;
                this.lbEmployeeNo.Text = string.Empty;
                this.lbCashier.Text = string.Empty;
                this.lbDeviceNo.Text = string.Empty;
                this.lbPage.Text = "第 1 页";
                this.lbBillIndex.Text = "0/0";
            }
        }

        private void BindDataGridView1(IList<Order> orderList)
        {
            this.dataGridView1.SelectionChanged -= new System.EventHandler(this.dataGridView1_SelectionChanged);
            this.dataGridView1.Rows.Clear();
            if (orderList != null && orderList.Count > 0)
            {
                foreach (Order order in orderList)
                {
                    string billType = string.Empty;
                    if (order.Status == 0)
                    {
                        billType = "未结账";
                    }
                    else if (order.Status == 1)
                    {
                        billType = "已结账";
                    }
                    else if (order.Status == 2)
                    {
                        billType = "已删除";
                    }
                    else if (order.Status == 3)
                    {
                        billType = "已预结";
                    }
                    else if (order.Status == 4)
                    {
                        billType = "已并桌";
                    }
                    int index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells["TranSequence"].Value = order.TranSequence.ToString();
                    dataGridView1.Rows[index].Cells["BillType"].Value = billType;
                    dataGridView1.Rows[index].Cells["TotalSellPrice"].Value = order.TotalSellPrice.ToString("f2");
                    dataGridView1.Rows[index].Cells["ActualSellPrice"].Value = order.ActualSellPrice.ToString("f2");
                    dataGridView1.Rows[index].Cells["DiscountPrice"].Value = order.DiscountPrice.ToString("f2");
                    dataGridView1.Rows[index].Cells["CutOffPrice"].Value = (-order.CutOffPrice).ToString("f2");
                    dataGridView1.Rows[index].Cells["ServiceFee"].Value = order.ServiceFee.ToString("f2");
                    dataGridView1.Rows[index].Cells["PaymentMoney"].Value = order.PaymentMoney.ToString("f2");
                    dataGridView1.Rows[index].Cells["NeedChangePay"].Value = order.NeedChangePay.ToString("f2");
                    dataGridView1.Rows[index].Cells["MoreOrLess"].Value = (order.ActualSellPrice + order.ServiceFee - (order.PaymentMoney - order.NeedChangePay)).ToString("f2");
                    dataGridView1.Rows[index].Cells["OrderID"].Value = order.OrderID;
                }
                //设置第一行选中
                int selectedIndex = 0;
                if (dataGridView1.RowCount > 0)
                {
                    dataGridView1.Rows[selectedIndex].Selected = true;
                }
                this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
                //默认加载第一行数据
                Guid orderID = new Guid(dataGridView1.Rows[selectedIndex].Cells["OrderID"].Value.ToString());
                SalesOrder salesOrder = SalesOrderService.GetInstance().GetSalesOrderByBillSearch(orderID);
                _salesOrder = salesOrder;
                //更新账单信息
                this.lbOrderNo.Text = salesOrder.order.OrderNo;
                this.lbDeskName.Text = salesOrder.order.DeskName;
                this.lbBillType.Text = dataGridView1.Rows[selectedIndex].Cells["BillType"].Value.ToString();
                string eatType = string.Empty;
                if (salesOrder.order.EatType == (int)EatWayType.DineIn)
                {
                    eatType = "堂食";
                }
                else if (salesOrder.order.EatType == (int)EatWayType.Takeout)
                {
                    eatType = "外卖";
                }
                else if (salesOrder.order.EatType == (int)EatWayType.OutsideOrder)
                {
                    eatType = "外送";
                }
                this.lbEatType.Text = eatType;
                this.lbEmployeeNo.Text = salesOrder.order.EmployeeNo;
                this.lbCashier.Text = salesOrder.order.PayEmployeeNo;
                this.lbDeviceNo.Text = salesOrder.order.DeviceNo;
                BindDataGridView2(salesOrder);
                BindDataGridView3(salesOrder);
            }
        }

        private void BindDataGridView2(SalesOrder salesOrder)
        {
            this.dataGridView2.Rows.Clear();
            IList<OrderDetails> orderDetailsList = salesOrder.orderDetailsList;
            if (orderDetailsList != null && orderDetailsList.Count > 0)
            {
                foreach (OrderDetails orderDetails in orderDetailsList)
                {
                    int index = dataGridView2.Rows.Add();
                    if (orderDetails.ItemType == (int)OrderItemType.Goods)
                    {
                        dataGridView2.Rows[index].Cells["GoodsName"].Value = orderDetails.GoodsName;
                    }
                    else
                    {
                        string strLevelFlag = string.Empty;
                        int levelCount = orderDetails.ItemLevel * 2;
                        for (int i = 0; i < levelCount; i++)
                        {
                            strLevelFlag += "-";
                        }
                        dataGridView2.Rows[index].Cells["GoodsName"].Value = strLevelFlag + orderDetails.GoodsName;
                    }
                    dataGridView2.Rows[index].Cells["ItemQty"].Value = orderDetails.ItemQty;
                    dataGridView2.Rows[index].Cells["SellPrice"].Value = orderDetails.TotalSellPrice.ToString("f2");
                    dataGridView2.Rows[index].Cells["Discount"].Value = orderDetails.TotalDiscount.ToString("f2");
                }
            }
        }

        private void BindDataGridView3(SalesOrder salesOrder)
        {
            this.dataGridView3.Rows.Clear();
            Order order = salesOrder.order;
            //售价总计
            int index = dataGridView3.Rows.Add();
            dataGridView3.Rows[index].Cells[0].Value = "售价总计";
            dataGridView3.Rows[index].Cells[1].Value = string.Empty;
            dataGridView3.Rows[index].Cells[2].Value = string.Empty;
            dataGridView3.Rows[index].Cells[3].Value = order.TotalSellPrice.ToString("f2");
            //去零金额
            if (Math.Abs(order.CutOffPrice) > 0)
            {
                index = dataGridView3.Rows.Add();
                dataGridView3.Rows[index].Cells[0].Value = "去零金额";
                dataGridView3.Rows[index].Cells[1].Value = string.Empty;
                dataGridView3.Rows[index].Cells[2].Value = string.Empty;
                dataGridView3.Rows[index].Cells[3].Value = string.Empty;
                dataGridView3.Rows[index].Cells[4].Value = (-order.CutOffPrice).ToString("f2");
            }
            //折扣金额
            if (Math.Abs(order.DiscountPrice) > 0)
            {
                index = dataGridView3.Rows.Add();
                dataGridView3.Rows[index].Cells[0].Value = "折扣金额";
                dataGridView3.Rows[index].Cells[1].Value = string.Empty;
                dataGridView3.Rows[index].Cells[2].Value = string.Empty;
                dataGridView3.Rows[index].Cells[3].Value = order.DiscountPrice.ToString("f2");
                dataGridView3.Rows[index].Cells[4].Value = string.Empty;
            }
            //实际金额
            index = dataGridView3.Rows.Add();
            dataGridView3.Rows[index].Cells[0].Value = "实际金额";
            dataGridView3.Rows[index].Cells[1].Value = string.Empty;
            dataGridView3.Rows[index].Cells[2].Value = string.Empty;
            dataGridView3.Rows[index].Cells[3].Value = order.ActualSellPrice.ToString("f2");
            dataGridView3.Rows[index].Cells[4].Value = string.Empty;
            //服务费
            if (order.ServiceFee > 0)
            {
                index = dataGridView3.Rows.Add();
                dataGridView3.Rows[index].Cells[0].Value = "服务费";
                dataGridView3.Rows[index].Cells[1].Value = string.Empty;
                dataGridView3.Rows[index].Cells[2].Value = string.Empty;
                dataGridView3.Rows[index].Cells[3].Value = order.ServiceFee.ToString("f2");
                dataGridView3.Rows[index].Cells[4].Value = string.Empty;
            }
            IList<OrderPayoff> orderPayoffList = salesOrder.orderPayoffList;
            if (orderPayoffList != null && orderPayoffList.Count > 0)
            {
                //空行
                dataGridView3.Rows.Add();
                //支付金额
                index = dataGridView3.Rows.Add();
                dataGridView3.Rows[index].Cells[0].Value = "支付金额";
                dataGridView3.Rows[index].Cells[1].Value = (order.ActualSellPrice + order.ServiceFee).ToString("f2");
                dataGridView3.Rows[index].Cells[3].Value = (order.PaymentMoney - order.NeedChangePay).ToString("f2");
                decimal moreOrLess = order.ActualSellPrice + order.ServiceFee - (order.PaymentMoney - order.NeedChangePay);
                if (Math.Abs(moreOrLess) > 0)
                {
                    dataGridView3.Rows[index].Cells[4].Value = (moreOrLess).ToString("f2");
                }
                //空行
                dataGridView3.Rows.Add();
                //支付方式明细
                foreach (OrderPayoff orderPayoff in orderPayoffList)
                {
                    index = dataGridView3.Rows.Add();
                    dataGridView3.Rows[index].Cells[0].Value = orderPayoff.PayoffName;
                    if (orderPayoff.PayoffType == (int)PayoffWayMode.GiftVoucher || orderPayoff.PayoffType == (int)PayoffWayMode.Coupon)
                    {
                        dataGridView3.Rows[index].Cells[1].Value = string.Format("{0} 张", orderPayoff.Quantity.ToString("f1"));
                    }
                    else
                    {
                        dataGridView3.Rows[index].Cells[1].Value = (orderPayoff.AsPay * orderPayoff.Quantity).ToString("f2");
                    }
                    if (orderPayoff.NeedChangePay > 0)
                    {
                        dataGridView3.Rows[index].Cells[2].Value = (-orderPayoff.NeedChangePay).ToString("f2");
                    }
                    dataGridView3.Rows[index].Cells[3].Value = (orderPayoff.AsPay * orderPayoff.Quantity - orderPayoff.NeedChangePay).ToString("f2");
                    dataGridView3.Rows[index].Cells[4].Value = string.Empty;
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int selectedIndex = dataGridView1.CurrentRow.Index;
                if (dataGridView1.Rows[selectedIndex].Cells["OrderID"].Value != null)
                {
                    Guid orderId = new Guid(dataGridView1.Rows[selectedIndex].Cells["OrderID"].Value.ToString());
                    SalesOrder salesOrder = SalesOrderService.GetInstance().GetSalesOrderByBillSearch(orderId);
                    if (salesOrder != null)
                    {
                        _salesOrder = salesOrder;
                        //更新账单信息
                        Order order = salesOrder.order;
                        this.lbOrderNo.Text = order.OrderNo;
                        this.lbDeskName.Text = order.DeskName;
                        string billType = string.Empty;
                        if (order.Status == 0 || order.Status == 3)
                        {
                            billType = "未结账";
                        }
                        else if (order.Status == 1)
                        {
                            billType = "已结账";
                        }
                        else if (order.Status == 2)
                        {
                            billType = "已删除";
                        }
                        this.lbBillType.Text = billType;
                        string eatType = string.Empty;
                        if (order.EatType == (int)EatWayType.DineIn)
                        {
                            eatType = "堂食";
                        }
                        else if (order.EatType == (int)EatWayType.Takeout)
                        {
                            eatType = "外卖";
                        }
                        else if (order.EatType == (int)EatWayType.OutsideOrder)
                        {
                            eatType = "外送";
                        }
                        this.lbEatType.Text = eatType;
                        this.lbEmployeeNo.Text = order.EmployeeNo;
                        this.lbCashier.Text = order.PayEmployeeNo;
                        this.lbDeviceNo.Text = order.DeviceNo;
                        int startIndex = selectedIndex + 1;
                        int index = this.lbBillIndex.Text.IndexOf('/');
                        this.lbBillIndex.Text = startIndex + this.lbBillIndex.Text.Substring(index);
                        BindDataGridView2(salesOrder);
                        BindDataGridView3(salesOrder);
                    }
                }
                else
                {
                    _salesOrder = null;
                    this.lbOrderNo.Text = string.Empty;
                    this.lbBillType.Text = string.Empty;
                    this.lbDeskName.Text = string.Empty;
                    this.lbEatType.Text = string.Empty;
                    this.lbEmployeeNo.Text = string.Empty;
                    this.lbCashier.Text = string.Empty;
                    this.lbDeviceNo.Text = string.Empty;
                    this.dataGridView2.Rows.Clear();
                    this.dataGridView3.Rows.Clear();
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && _salesOrder != null && _salesOrder.order != null)
            {
                if (_salesOrder.order.Status == 1)
                {
                    int selectedIndex = dataGridView1.CurrentRow.Index;
                    if (dataGridView1.Rows[selectedIndex].Cells["OrderID"].Value != null)
                    {
                        Order order = _salesOrder.order;
                        //打印小票
                        PrintData printData = new PrintData();
                        printData.ShopName = ConstantValuePool.CurrentShop.ShopName;
                        printData.DeskName = order.DeskName;
                        printData.PersonNum = order.PeopleNum.ToString();
                        printData.PrintTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        printData.EmployeeNo = order.EmployeeNo;
                        printData.TranSequence = order.TranSequence.ToString();
                        printData.ShopAddress = ConstantValuePool.CurrentShop.RunAddress;
                        printData.Telephone = ConstantValuePool.CurrentShop.Telephone;
                        printData.ReceivableMoney = order.ActualSellPrice.ToString("f2");
                        printData.ServiceFee = order.ServiceFee.ToString("f2");
                        printData.PaidInMoney = order.PaymentMoney.ToString("f2");
                        printData.NeedChangePay = order.NeedChangePay.ToString("f2");
                        printData.GoodsOrderList = new List<GoodsOrder>();
                        printData.PayingOrderList = new List<PayingGoodsOrder>();
                        foreach (OrderDetails item in _salesOrder.orderDetailsList)
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
                        foreach (OrderPayoff orderPayoff in _salesOrder.orderPayoffList)
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
                        if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.COM)
                        {
                            string port = ConstantValuePool.BizSettingConfig.printConfig.Name;
                            if (port.Length > 3)
                            {
                                if (port.Substring(0, 3).ToUpper() == "COM")
                                {
                                    string portName = port.Substring(0, 4).ToUpper();
                                    InstructionOrderPrint printer = new InstructionOrderPrint(portName, 9600, Parity.None, 8, StopBits.One, paperWidth);
                                    printer.DoPrintPaidOrder(printData);
                                }
                            }
                        }
                        if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.ETHERNET)
                        {
                            string ipAddress = ConstantValuePool.BizSettingConfig.printConfig.Name;
                            InstructionOrderPrint printer = new InstructionOrderPrint(ipAddress, 9100, paperWidth);
                            printer.DoPrintPaidOrder(printData);
                        }
                        if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.USB)
                        {
                            string vid = ConstantValuePool.BizSettingConfig.printConfig.VID;
                            string pid = ConstantValuePool.BizSettingConfig.printConfig.PID;
                            string endpointId = ConstantValuePool.BizSettingConfig.printConfig.EndpointID;
                            InstructionOrderPrint printer = new InstructionOrderPrint(vid, pid, endpointId, paperWidth);
                            printer.DoPrintPaidOrder(printData);
                        }
                    }
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPageDown_Click(object sender, EventArgs e)
        {
            _pageIndex++;
            SearchSalesOrder();
        }

        private void btnPageUp_Click(object sender, EventArgs e)
        {
            _pageIndex--;
            SearchSalesOrder();
        }

        private void btnWholeDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && _salesOrder != null && _salesOrder.order != null)
            {
                if (_salesOrder.order.Status == 1)
                {
                    int selectedIndex = dataGridView1.CurrentRow.Index;
                    if (dataGridView1.Rows[selectedIndex].Cells["OrderID"].Value != null)
                    {
                        //权限验证
                        bool hasRights = false;
                        if (RightsItemCode.FindRights(RightsItemCode.CANCELBILL))
                        {
                            hasRights = true;
                        }
                        else
                        {
                            FormRightsCode formRightsCode = new FormRightsCode();
                            formRightsCode.ShowDialog();
                            if (formRightsCode.ReturnValue)
                            {
                                IList<string> rightsCodeList = formRightsCode.RightsCodeList;
                                if (RightsItemCode.FindRights(rightsCodeList, RightsItemCode.CANCELBILL))
                                {
                                    hasRights = true;
                                }
                            }
                        }
                        if (!hasRights)
                        {
                            return;
                        }
                        FormCancelOrder form = new FormCancelOrder();
                        form.ShowDialog();
                        if (form.CurrentReason != null)
                        {
                            //删除订单
                            DeletedOrder deletedOrder = new DeletedOrder();
                            deletedOrder.OrderID = _salesOrder.order.OrderID;
                            deletedOrder.AuthorisedManager = ConstantValuePool.CurrentEmployee.EmployeeID;
                            deletedOrder.CancelEmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                            deletedOrder.CancelReasonName = form.CurrentReason.ReasonName;

                            if (DeletedOrderService.GetInstance().DeletePaidWholeOrder(deletedOrder))
                            {
                                dataGridView1.Rows[selectedIndex].Cells["BillType"].Value = "已删除";
                                _salesOrder.order.Status = 2;
                            }
                            else
                            {
                                MessageBox.Show("删除账单失败！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }

        private void btnSingleDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && _salesOrder != null && _salesOrder.order != null)
            {
                if (_salesOrder.order.Status == 1)
                {
                    int selectedIndex = dataGridView1.CurrentRow.Index;
                    FormBackGoods form = new FormBackGoods(_salesOrder);
                    form.ShowDialog();
                    if (form.IsChanged)
                    {
                        Guid orderId = new Guid(dataGridView1.Rows[selectedIndex].Cells["OrderID"].Value.ToString());
                        SalesOrder salesOrder = SalesOrderService.GetInstance().GetSalesOrderByBillSearch(orderId);
                        _salesOrder = salesOrder;
                        //更新账单信息
                        dataGridView1.Rows[selectedIndex].Cells["TotalSellPrice"].Value = salesOrder.order.TotalSellPrice.ToString("f2");
                        dataGridView1.Rows[selectedIndex].Cells["ActualSellPrice"].Value = salesOrder.order.ActualSellPrice.ToString("f2");
                        dataGridView1.Rows[selectedIndex].Cells["DiscountPrice"].Value = salesOrder.order.DiscountPrice.ToString("f2");
                        dataGridView1.Rows[selectedIndex].Cells["CutOffPrice"].Value = salesOrder.order.CutOffPrice.ToString("f2");
                        dataGridView1.Rows[selectedIndex].Cells["ServiceFee"].Value = salesOrder.order.ServiceFee.ToString("f2");
                        dataGridView1.Rows[selectedIndex].Cells["PaymentMoney"].Value = salesOrder.order.PaymentMoney.ToString("f2");
                        dataGridView1.Rows[selectedIndex].Cells["NeedChangePay"].Value = salesOrder.order.NeedChangePay.ToString("f2");
                        dataGridView1.Rows[selectedIndex].Cells["MoreOrLess"].Value = (salesOrder.order.ActualSellPrice + salesOrder.order.ServiceFee - (salesOrder.order.PaymentMoney - salesOrder.order.NeedChangePay)).ToString("f2");
                        BindDataGridView2(salesOrder);
                        BindDataGridView3(salesOrder);
                    }
                }
            }
        }

        private void btnBillModify_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && _salesOrder != null && _salesOrder.order != null)
            {
                if (_salesOrder.order.Status == 1)
                {
                    int selectedIndex = dataGridView1.CurrentRow.Index;
                    FormModifyOrder form = new FormModifyOrder(_salesOrder);
                    form.ShowDialog();
                    if (form.IsChanged)
                    {
                        Guid orderId = new Guid(dataGridView1.Rows[selectedIndex].Cells["OrderID"].Value.ToString());
                        SalesOrder salesOrder = SalesOrderService.GetInstance().GetSalesOrderByBillSearch(orderId);
                        _salesOrder = salesOrder;
                        //更新账单信息
                        dataGridView1.Rows[selectedIndex].Cells["TotalSellPrice"].Value = salesOrder.order.TotalSellPrice.ToString("f2");
                        dataGridView1.Rows[selectedIndex].Cells["ActualSellPrice"].Value = salesOrder.order.ActualSellPrice.ToString("f2");
                        dataGridView1.Rows[selectedIndex].Cells["DiscountPrice"].Value = salesOrder.order.DiscountPrice.ToString("f2");
                        dataGridView1.Rows[selectedIndex].Cells["CutOffPrice"].Value = salesOrder.order.CutOffPrice.ToString("f2");
                        dataGridView1.Rows[selectedIndex].Cells["ServiceFee"].Value = salesOrder.order.ServiceFee.ToString("f2");
                        dataGridView1.Rows[selectedIndex].Cells["PaymentMoney"].Value = salesOrder.order.PaymentMoney.ToString("f2");
                        dataGridView1.Rows[selectedIndex].Cells["NeedChangePay"].Value = salesOrder.order.NeedChangePay.ToString("f2");
                        dataGridView1.Rows[selectedIndex].Cells["MoreOrLess"].Value = (salesOrder.order.ActualSellPrice + salesOrder.order.ServiceFee - (salesOrder.order.PaymentMoney - salesOrder.order.NeedChangePay)).ToString("f2");
                        BindDataGridView2(salesOrder);
                        BindDataGridView3(salesOrder);
                    }
                }
            }
        }

        #region 重算坐标
        private void ResizeStyle()
        {
            Rectangle screenArea = Screen.GetWorkingArea(this);
            if (screenArea.Width > 1280)
            {
                decimal widthRate = Convert.ToDecimal(screenArea.Width) / 1024;
                decimal heightRate = Convert.ToDecimal(screenArea.Height) / 771;
                SetBounds(0, 0, screenArea.Width, screenArea.Height);

                foreach (Control c in this.Controls)
                {
                    SetControlSize(c, widthRate, heightRate);
                }
                foreach (Control c in this.pnlInformation.Controls)
                {
                    SetControlSize(c, widthRate, heightRate);
                }
                foreach (Control c in this.pnlTop.Controls)
                {
                    SetControlSize(c, widthRate, heightRate);
                }
            }
        }

        private void SetControlSize(Control ctl, decimal widthRate, decimal heightRate)
        {
            ctl.Width = Convert.ToInt32(ctl.Width * widthRate);
            ctl.Height = Convert.ToInt32(ctl.Height * heightRate);
            ctl.Location = new Point(Convert.ToInt32(ctl.Location.X * widthRate), Convert.ToInt32(ctl.Location.Y * heightRate));
        }
        #endregion
    }
}
