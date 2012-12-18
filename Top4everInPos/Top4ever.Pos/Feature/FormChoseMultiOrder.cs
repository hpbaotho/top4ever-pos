using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.CustomControl;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;
using Top4ever.Entity.Enum;

namespace Top4ever.Pos.Feature
{
    public partial class FormChoseMultiOrder : Form
    {
        private CrystalButton m_PreviousBtn;
        private IList<Order> m_OrderList;
        private DeskChange m_DeskChange;
        private Order m_Order;

        public Order SelectedOrder
        {
            get { return m_Order; }
        }

        public FormChoseMultiOrder(IList<Order> orderList)
        {
            m_OrderList = orderList;
            InitializeComponent();
        }

        public FormChoseMultiOrder(IList<Order> orderList, DeskChange deskChange)
        {
            m_OrderList = orderList;
            m_DeskChange = deskChange;
            InitializeComponent();
        }

        private void FormChoseMultiOrder_Load(object sender, EventArgs e)
        {
            if (m_OrderList != null && m_OrderList.Count > 0)
            {
                InitialOrderButton(m_OrderList);
            }
            if (m_DeskChange != null)
            {
                pnlBottom.Controls.Clear();
                int px = 0, py = 5;
                int space = 10;
                int width = (this.pnlBottom.Width - 2 * space) / 3;
                int height = 43;

                CrystalButton btnMoveBill = new CrystalButton();
                btnMoveBill.BackColor = Color.Teal;
                btnMoveBill.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular);
                btnMoveBill.ForeColor = System.Drawing.Color.White;
                btnMoveBill.Location = new Point(px, py);
                btnMoveBill.Name = "btnMoveBill";
                btnMoveBill.Size = new Size(width, height);
                btnMoveBill.Text = "转台";
                btnMoveBill.Click += new System.EventHandler(this.btnMoveBill_Click);
                this.pnlBottom.Controls.Add(btnMoveBill);
                px += width + space; 
                CrystalButton btnMerge = new CrystalButton();
                btnMerge.BackColor = Color.Teal;
                btnMerge.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular);
                btnMerge.ForeColor = System.Drawing.Color.White;
                btnMerge.Location = new Point(px, py);
                btnMerge.Name = "btnMerge";
                btnMerge.Size = new Size(width, height);
                btnMerge.Text = "合并";
                btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
                this.pnlBottom.Controls.Add(btnMerge);
                px += width + space;
                btnCancel.Location = new Point(px, py);
                btnCancel.Size = new Size(width, height);
                this.pnlBottom.Controls.Add(btnCancel);
            }
        }

        private void InitialOrderButton(IList<Order> orderList)
        {
            int maxColumn = 0;
            if (orderList.Count <= 5)
            {
                maxColumn = 1;
            }
            else if (orderList.Count > 5 && orderList.Count <= 10)
            {
                maxColumn = 2;
            }
            else
            {
                maxColumn = 3;
            }
            int width = (this.pnlContainer.Width - 12) / maxColumn;
            int height = Convert.ToInt32((this.pnlContainer.Height - 12) / Math.Ceiling((decimal)orderList.Count / maxColumn));

            int count = 1;
            int px = 6, py = 6;
            foreach (Order order in orderList)
            {
                StringBuilder sbOrderInfo = new StringBuilder();
                sbOrderInfo.Append("桌号:" + order.DeskName + "-" + order.SubOrderNo);
                sbOrderInfo.Append("\n");
                sbOrderInfo.Append("人数:" + order.PeopleNum);
                sbOrderInfo.Append("\n");
                sbOrderInfo.Append("消费金额:" + order.ActualSellPrice.ToString("N"));
                CrystalButton btnOrder = new CrystalButton();
                btnOrder.BackColor = btnOrder.DisplayColor = Color.Olive;
                btnOrder.ForeColor = Color.White;
                btnOrder.Name = order.OrderID.ToString();
                btnOrder.Text = sbOrderInfo.ToString();
                btnOrder.Tag = order;
                btnOrder.Width = width;
                btnOrder.Height = height;
                btnOrder.Location = new Point(px, py);
                btnOrder.Click += new EventHandler(btnOrder_Click);
                this.pnlContainer.Controls.Add(btnOrder);
                count++;

                if (count > maxColumn)
                {
                    px = 6;
                    py += height;
                    count = 1;
                    continue;
                }
                px += width;
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            CrystalButton btnOrder = sender as CrystalButton;
            m_Order = btnOrder.Tag as Order;
            if (m_PreviousBtn == null)
            {
                btnOrder.BackColor = ConstantValuePool.PressedColor;
                m_PreviousBtn = btnOrder;
            }
            else
            {
                m_PreviousBtn.BackColor = m_PreviousBtn.DisplayColor;
                btnOrder.BackColor = ConstantValuePool.PressedColor;
                m_PreviousBtn = btnOrder;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (m_Order != null)
            {
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_Order = null;
            this.Close();
        }

        private void btnMoveBill_Click(object sender, EventArgs e)
        {
            OrderService orderService = new OrderService();
            if (orderService.OrderDeskOperate(m_DeskChange))
            {
                //通过m_Order是否为null判断转台是否成功
                m_Order = new Order();
                this.Close();
            }
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            if (m_OrderList.Count == 1)
            {
                m_Order = m_OrderList[0];
            }
            if (m_Order != null)
            {
                m_DeskChange.OrderID2nd = m_Order.OrderID;
                OrderService orderService = new OrderService();
                if (orderService.OrderDeskOperate(m_DeskChange))
                {
                    this.Close();
                }
            }
        }
    }
}
