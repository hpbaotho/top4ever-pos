using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Domain.Transfer;
using Top4ever.Domain.OrderRelated;
using Top4ever.Entity;
using Top4ever.Entity.Enum;

namespace Top4ever.Pos.TakeawayCall
{
    public partial class FormTakeGoods : Form
    {
        private SalesOrder _salesOrder;
        private bool _hasDeliveried = false;
        public bool HasDeliveried
        {
            get { return _hasDeliveried; }
        }

        public FormTakeGoods(SalesOrder salesOrder)
        {
            _salesOrder = salesOrder;
            InitializeComponent();
        }

        private void FormTakeGoods_Load(object sender, EventArgs e)
        {
            if (_salesOrder != null)
            {
                this.txtTranSeq.Text = Convert.ToString(_salesOrder.order.TranSequence);
                this.txtEmployeeNo.Text = _salesOrder.order.EmployeeNo;
                this.txtOrderAmount.Text = Convert.ToString(_salesOrder.order.ActualSellPrice + _salesOrder.order.ServiceFee);
                
                IList<OrderDetails> orderDetailsList = _salesOrder.orderDetailsList;
                if (orderDetailsList != null && orderDetailsList.Count > 0)
                {
                    foreach (OrderDetails orderDetails in orderDetailsList)
                    {
                        int index = dgvGoodsOrder.Rows.Add();
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
                        dgvGoodsOrder.Rows[index].Cells["ItemQty"].Value = orderDetails.ItemQty;
                        dgvGoodsOrder.Rows[index].Cells["SellPrice"].Value = orderDetails.TotalSellPrice.ToString("f2");
                        dgvGoodsOrder.Rows[index].Cells["Discount"].Value = orderDetails.TotalDiscount.ToString("f2");
                    }
                }

                IList<OrderPayoff> orderPayoffList = _salesOrder.orderPayoffList;
                if (orderPayoffList != null && orderPayoffList.Count > 0)
                {
                    //支付金额
                    int index = dgvPayoffWay.Rows.Add();
                    dgvPayoffWay.Rows[index].Cells[0].Value = "支付金额";
                    decimal moreOrLess = _salesOrder.order.ActualSellPrice + _salesOrder.order.ServiceFee - (_salesOrder.order.PaymentMoney - _salesOrder.order.NeedChangePay);
                    if (Math.Abs(moreOrLess) > 0)
                    {
                        dgvPayoffWay.Rows[index].Cells[2].Value = (moreOrLess).ToString("f2");
                    }
                    dgvPayoffWay.Rows[index].Cells[3].Value = (_salesOrder.order.PaymentMoney - _salesOrder.order.NeedChangePay).ToString("f2");
                    //空行
                    dgvPayoffWay.Rows.Add();
                    //支付方式明细
                    foreach (OrderPayoff orderPayoff in orderPayoffList)
                    {
                        index = dgvPayoffWay.Rows.Add();
                        dgvPayoffWay.Rows[index].Cells[0].Value = orderPayoff.PayoffName;
                        if (orderPayoff.PayoffType == (int)PayoffWayMode.GiftVoucher)
                        {
                            dgvPayoffWay.Rows[index].Cells[1].Value = string.Format("{0} 张", orderPayoff.Quantity.ToString("f1"));
                        }
                        else
                        {
                            dgvPayoffWay.Rows[index].Cells[1].Value = (orderPayoff.AsPay * orderPayoff.Quantity).ToString("f2");
                        }
                        if (orderPayoff.NeedChangePay > 0)
                        {
                            dgvPayoffWay.Rows[index].Cells[2].Value = (-orderPayoff.NeedChangePay).ToString("f2");
                        }
                        dgvPayoffWay.Rows[index].Cells[3].Value = (orderPayoff.AsPay * orderPayoff.Quantity - orderPayoff.NeedChangePay).ToString("f2");
                    }
                }
            }
        }

        private void btnTakeGoods_Click(object sender, EventArgs e)
        {
            OrderService orderService = new OrderService();
            if (orderService.DeliveryTakeoutOrder(_salesOrder.order.OrderID, ConstantValuePool.CurrentEmployee.EmployeeID))
            {
                _hasDeliveried = true;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
