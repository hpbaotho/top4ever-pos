using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
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
using Top4ever.Print.Entity;
using Top4ever.Print;
using Top4ever.Pos.Feature;

namespace Top4ever.Pos
{
    public partial class FormTakeout : Form
    {
        private const string deskName = "W001";
        private List<CrystalButton> btnDeliveryList = new List<CrystalButton>();
        private const int m_Space = 2;
        private int m_GoodsGroupPageIndex = 0;
        private int m_GoodsPageIndex = 0;
        private int m_DetailsGroupPageIndex = 0;
        private int m_DetailsPageIndex = 0;
        private GoodsGroup m_CurrentGoodsGroup;
        private DetailsGroup m_CurrentDetailsGroup;
        private IList<Guid> m_CurrentDetailsGroupIDList;
        private string m_DetailsPrefix = string.Empty;
        private bool m_GoodsOrDetails = true;
        /// <summary>
        /// 提交的订单信息
        /// </summary>
        private SalesOrder m_SalesOrder;
        private decimal m_TotalPrice = 0;
        private decimal m_ActualPayMoney = 0;
        private decimal m_Discount = 0;
        private decimal m_CutOff = 0;
        private Guid m_EmployeeID = Guid.Empty;
        private string m_EmployeeNo = string.Empty;
        private CrystalButton prevPressedButton = null;
        private bool m_ShowSilverCode = false;
        //外卖单列表
        private const int m_PageCount = 10;
        private int m_PageIndex = 0;

        public FormTakeout()
        {
            InitializeComponent();
            btnPageUp.DisplayColor = btnPageUp.BackColor;
            btnPageDown.DisplayColor = btnPageDown.BackColor;
            btnHead.DisplayColor = btnHead.BackColor;
            btnBack.DisplayColor = btnBack.BackColor;
            btnPgUp.DisplayColor = btnPgUp.BackColor;
            btnPgDown.DisplayColor = btnPgDown.BackColor;
            btnOutsideOrder.DisplayColor = btnOutsideOrder.BackColor;
        }

        private void FormTakeout_Load(object sender, EventArgs e)
        {
            //初始化外卖单按钮
            InitDeliveryButton();
            //初始化菜品组按钮
            InitializeGoodsGroupButton();
            //初始化菜品按钮
            InitializeGoodsButton();
            //初始化细项按钮
            InitializeDetailButton();
            //初始化
            m_GoodsGroupPageIndex = 0;
            m_GoodsPageIndex = 0;
            m_DetailsGroupPageIndex = 0;
            m_DetailsPageIndex = 0;
            LoadDefaultGoodsGroupButton();
            m_GoodsOrDetails = true;
            //Tools Bar Size
            int space = 2;
            int buttonNum = 9;
            int width = (this.pnlTools.Width - (buttonNum - 1) * space) / buttonNum;
            int offsetX = 0, offsetY = 0;
            btnFuncPanel.Width = width;
            offsetX += width + space;
            btnDiscount.Width = width;
            btnDiscount.Location = new Point(offsetX, offsetY);
            offsetX += width + space;
            btnWholeDiscount.Width = width;
            btnWholeDiscount.Location = new Point(offsetX, offsetY);
            offsetX += width + space;
            btnTakeOut.Width = width;
            btnTakeOut.Location = new Point(offsetX, offsetY);
            offsetX += width + space;
            btnBeNew.Width = width;
            btnBeNew.Location = new Point(offsetX, offsetY);
            offsetX += width + space;
            btnOutsideOrder.Width = width;
            btnOutsideOrder.Location = new Point(offsetX, offsetY);
            offsetX += width + space;
            btnDeliveryGoods.Width = width;
            btnDeliveryGoods.Location = new Point(offsetX, offsetY);
            offsetX += width + space;
            btnPrintBill.Width = width;
            btnPrintBill.Location = new Point(offsetX, offsetY);
            offsetX += width + space;
            btnClose.Width = width;
            btnClose.Location = new Point(offsetX, offsetY);
            //编码Size
            int locPX = this.pnlCodeSearch.Width - this.btnCheckout.Width - 5;
            this.btnCheckout.Location = new Point(locPX, 0);
            int distance = this.pnlCodeSearch.Width - 497;
            txtSearch.Width = 200 + distance;
            //外卖Size
            distance = this.pnlCustomerInfo.Width - 1024;
            int locWidth = 325 + distance;
            txtAddress.Width = locWidth;
            locPX = txtAddress.Location.X + locWidth + 8;
            btnRecords.Location = new Point(locPX, btnAddress.Location.Y);
            locPX += btnRecords.Width + 2;
            btnRecentlyCall.Location = new Point(locPX, btnAddress.Location.Y);
            //清除
            txtTelephone.Text = string.Empty;
            txtName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            btnOutsideOrder.Enabled = false;
            btnOutsideOrder.BackColor = ConstantValuePool.DisabledColor;
            //加载外卖单列表
            OrderService orderService = new OrderService();
            IList<DeliveryOrder> deliveryOrderList = orderService.GetDeliveryOrderList();
            if (deliveryOrderList != null)
            {
                DisplayDeliveryOrderButton(deliveryOrderList);
            }
        }

        private void InitDeliveryButton()
        {
            if (btnDeliveryList.Count > 0)
            {
                this.pnlDelivery.Controls.Clear();
                foreach (CrystalButton btnDelivery in btnDeliveryList)
                {
                    this.pnlDelivery.Controls.Add(btnDelivery);
                }
            }
            else
            {
                int space = 2;
                int px = 0, py = space;
                int height = (this.pnlDelivery.Height - this.pnlPage.Height - (m_PageCount + 1) * space) / m_PageCount;
                for (int i = 0; i < m_PageCount; i++)
                {
                    CrystalButton btnDelivery = new CrystalButton();
                    btnDelivery.Name = "btnDelivery" + i;
                    btnDelivery.BackColor = btnDelivery.DisplayColor = Color.DodgerBlue;
                    btnDelivery.Font = new Font("微软雅黑", 9.75F);
                    btnDelivery.ForeColor = Color.White;
                    btnDelivery.Location = new Point(px, py);
                    btnDelivery.Size = new Size(pnlDelivery.Width - space, height);
                    btnDelivery.Click += new System.EventHandler(this.btnDelivery_Click);
                    this.pnlDelivery.Controls.Add(btnDelivery);
                    btnDeliveryList.Add(btnDelivery);
                    py += height + space;
                }
            }
        }

        #region 初始化
        private void InitializeGoodsGroupButton()
        {
            if (ConstantValuePool.GoodsGroupButton == null || ConstantValuePool.GoodsGroupButton.Count == 0)
            {
                if (ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls != null && ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls.Count > 0)
                {
                    List<CrystalButton> goodsGroupButton = new List<CrystalButton>();
                    foreach (BizControl control in ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls)
                    {
                        if (control.Name == "GoodsGroup")
                        {
                            int width = (this.pnlGroup.Width - m_Space * control.ColumnsCount) / control.ColumnsCount;
                            int height = (this.pnlGroup.Height - m_Space * control.RowsCount) / control.RowsCount;
                            int pageSize = control.ColumnsCount * control.RowsCount - 2;    //扣除向前、向后两个按钮
                            //坐标
                            int px = m_Space, py = 0, times = 0, pageCount = 0;
                            foreach (GoodsGroup item in ConstantValuePool.GoodsGroupList)
                            {
                                //实例化Button
                                CrystalButton btn = new CrystalButton();
                                btn.Name = item.GoodsGroupID.ToString();
                                btn.Text = item.GoodsGroupName;
                                btn.Width = width;
                                btn.Height = height;
                                btn.Location = new Point(px, py);
                                btn.Tag = item;
                                foreach (ButtonStyle btnStyle in ConstantValuePool.ButtonStyleList)
                                {
                                    if (item.ButtonStyleID.Equals(btnStyle.ButtonStyleID))
                                    {
                                        float emSize = (float)btnStyle.FontSize;
                                        FontStyle style = FontStyle.Regular;
                                        btn.Font = new Font(btnStyle.FontName, emSize, style);
                                        btn.ForeColor = ColorConvert.RGB(btnStyle.ForeColor);
                                        btn.BackColor = btn.DisplayColor = ColorConvert.RGB(btnStyle.BackColor);
                                        break;
                                    }
                                }
                                goodsGroupButton.Add(btn);
                                //计算Button位置
                                times++;
                                pageCount++;
                                px += m_Space + width;
                                if (times == control.ColumnsCount)
                                {
                                    px = m_Space;
                                    times = 0;
                                    py += m_Space + height;
                                }
                                if (pageCount == pageSize)
                                {
                                    //new page 初始化
                                    px = m_Space;
                                    py = 0;
                                    times = 0;
                                    pageCount = 0;
                                }
                            }
                            break;
                        }
                    }
                    ConstantValuePool.GoodsGroupButton = goodsGroupButton;
                }
            }
        }

        private void InitializeGoodsButton()
        {
            if (ConstantValuePool.DicGoodsButton == null || ConstantValuePool.DicGoodsButton.Count == 0)
            {
                if (ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls != null && ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls.Count > 0)
                {
                    Dictionary<string, List<CrystalButton>> dicGoodsButton = new Dictionary<string, List<CrystalButton>>();
                    foreach (BizControl control in ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls)
                    {
                        if (control.Name == "Goods")
                        {
                            int width = (this.pnlItem.Width - m_Space * control.ColumnsCount) / control.ColumnsCount;
                            int height = (this.pnlItem.Height - m_Space * control.RowsCount) / control.RowsCount;
                            int pageSize = control.ColumnsCount * control.RowsCount - 2;    //扣除向前、向后两个按钮
                            //坐标
                            int px = m_Space, py = 0, times = 0, pageCount = 0;
                            foreach (GoodsGroup goodsGroupItem in ConstantValuePool.GoodsGroupList)
                            {
                                if (goodsGroupItem.GoodsList != null && goodsGroupItem.GoodsList.Count > 0)
                                {
                                    string goodsGroupID = goodsGroupItem.GoodsGroupID.ToString();
                                    List<CrystalButton> btnGoods = new List<CrystalButton>();
                                    foreach (Goods goodsItem in goodsGroupItem.GoodsList)
                                    {
                                        //实例化Button
                                        CrystalButton btn = new CrystalButton();
                                        btn.Name = goodsItem.GoodsID.ToString();
                                        btn.Text = goodsItem.GoodsName;
                                        btn.Width = width;
                                        btn.Height = height;
                                        btn.Location = new Point(px, py);
                                        btn.Tag = goodsItem;
                                        foreach (ButtonStyle btnStyle in ConstantValuePool.ButtonStyleList)
                                        {
                                            if (goodsItem.ButtonStyleID.Equals(btnStyle.ButtonStyleID))
                                            {
                                                float emSize = (float)btnStyle.FontSize;
                                                FontStyle style = FontStyle.Regular;
                                                btn.Font = new Font(btnStyle.FontName, emSize, style);
                                                btn.ForeColor = ColorConvert.RGB(btnStyle.ForeColor);
                                                btn.BackColor = btn.DisplayColor = ColorConvert.RGB(btnStyle.BackColor);
                                                break;
                                            }
                                        }
                                        btnGoods.Add(btn);
                                        //计算Button位置
                                        times++;
                                        pageCount++;
                                        px += m_Space + width;
                                        if (times == control.ColumnsCount)
                                        {
                                            px = m_Space;
                                            times = 0;
                                            py += m_Space + height;
                                        }
                                        if (pageCount == pageSize)
                                        {
                                            //new page 初始化
                                            px = m_Space;
                                            py = 0;
                                            times = 0;
                                            pageCount = 0;
                                        }
                                    }
                                    dicGoodsButton.Add(goodsGroupID, btnGoods);
                                    //new button 初始化
                                    px = m_Space;
                                    py = 0;
                                    times = 0;
                                    pageCount = 0;
                                }
                            }
                            break;
                        }
                    }
                    ConstantValuePool.DicGoodsButton = dicGoodsButton;
                }
            }
        }

        private void InitializeDetailButton()
        {
            if (ConstantValuePool.DicDetailsButton == null || ConstantValuePool.DicDetailsButton.Count == 0)
            {
                if (ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls != null && ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls.Count > 0)
                {
                    Dictionary<string, List<CrystalButton>> DicDetailsButton = new Dictionary<string, List<CrystalButton>>();
                    foreach (BizControl control in ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls)
                    {
                        if (control.Name == "Details")
                        {
                            int width = (this.pnlItem.Width - m_Space * control.ColumnsCount) / control.ColumnsCount;
                            int height = (this.pnlItem.Height - m_Space * control.RowsCount) / control.RowsCount;
                            int pageSize = control.ColumnsCount * control.RowsCount - 2;    //扣除向前、向后两个按钮
                            //坐标
                            int px = m_Space, py = 0, times = 0, pageCount = 0;
                            foreach (DetailsGroup detailsGroupItem in ConstantValuePool.DetailsGroupList)
                            {
                                if (detailsGroupItem.DetailsList != null && detailsGroupItem.DetailsList.Count > 0)
                                {
                                    string detailGroupID = detailsGroupItem.DetailsGroupID.ToString();
                                    List<CrystalButton> btnDetails = new List<CrystalButton>();
                                    foreach (Details detailsItem in detailsGroupItem.DetailsList)
                                    {
                                        //实例化Button
                                        CrystalButton btn = new CrystalButton();
                                        btn.Name = detailsItem.DetailsID.ToString();
                                        btn.Text = detailsItem.DetailsName;
                                        btn.Width = width;
                                        btn.Height = height;
                                        btn.Location = new Point(px, py);
                                        btn.Tag = detailsItem;
                                        foreach (ButtonStyle btnStyle in ConstantValuePool.ButtonStyleList)
                                        {
                                            if (detailsItem.ButtonStyleID.Equals(btnStyle.ButtonStyleID))
                                            {
                                                float emSize = (float)btnStyle.FontSize;
                                                FontStyle style = FontStyle.Regular;
                                                btn.Font = new Font(btnStyle.FontName, emSize, style);
                                                btn.ForeColor = ColorConvert.RGB(btnStyle.ForeColor);
                                                btn.BackColor = btn.DisplayColor = ColorConvert.RGB(btnStyle.BackColor);
                                                break;
                                            }
                                        }
                                        btnDetails.Add(btn);
                                        //计算Button位置
                                        times++;
                                        pageCount++;
                                        px += m_Space + width;
                                        if (times == control.ColumnsCount)
                                        {
                                            px = m_Space;
                                            times = 0;
                                            py += m_Space + height;
                                        }
                                        if (pageCount == pageSize)
                                        {
                                            //new page 初始化
                                            px = m_Space;
                                            py = 0;
                                            times = 0;
                                            pageCount = 0;
                                        }
                                    }
                                    DicDetailsButton.Add(detailGroupID, btnDetails);
                                    //new button 初始化
                                    px = m_Space;
                                    py = 0;
                                    times = 0;
                                    pageCount = 0;
                                }
                            }
                            break;
                        }
                    }
                    ConstantValuePool.DicDetailsButton = DicDetailsButton;
                }
            }
        }
        #endregion

        #region 显示按钮

        private void DisplayGoodsGroupButton()
        {
            //禁止引发Layout事件
            this.pnlGroup.SuspendLayout();
            this.SuspendLayout();
            //清除pnlGroup内所有控件
            this.pnlGroup.Controls.Clear();
            //显示控件
            if (ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls != null && ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls.Count > 0)
            {
                int width = 0, height = 0, columnsCount = 0, rowsCount = 0, pageSize = 0;
                foreach (BizControl control in ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls)
                {
                    if (control.Name == "GoodsGroup")
                    {
                        width = (this.pnlGroup.Width - m_Space * control.ColumnsCount) / control.ColumnsCount;
                        height = (this.pnlGroup.Height - m_Space * control.RowsCount) / control.RowsCount;
                        columnsCount = control.ColumnsCount;
                        rowsCount = control.RowsCount;
                        pageSize = control.ColumnsCount * control.RowsCount - 2;    //扣除向前、向后两个按钮
                        break;
                    }
                }
                int startIndex = m_GoodsGroupPageIndex * pageSize;
                int endIndex = (m_GoodsGroupPageIndex + 1) * pageSize;
                if (endIndex > ConstantValuePool.GoodsGroupButton.Count)
                {
                    endIndex = ConstantValuePool.GoodsGroupButton.Count;
                }
                for (int i = startIndex; i < endIndex; i++)
                {
                    CrystalButton btnGoodsGroup = ConstantValuePool.GoodsGroupButton[i];
                    btnGoodsGroup.BackColor = btnGoodsGroup.DisplayColor;
                    btnGoodsGroup.Click -= new System.EventHandler(this.btnGoodsGroup_Click);
                    btnGoodsGroup.Click += new System.EventHandler(this.btnGoodsGroup_Click);
                    this.pnlGroup.Controls.Add(btnGoodsGroup);
                }
                //设置页码按钮的位置
                int px = (columnsCount - 2) * width + (columnsCount - 2 + 1) * m_Space;
                int py = (rowsCount - 1) * (height + m_Space);
                btnPageUp.Width = width;
                btnPageUp.Height = height;
                btnPageUp.Location = new Point(px, py);
                if (startIndex <= 0)
                {
                    btnPageUp.Enabled = false;
                    btnPageUp.BackColor = ConstantValuePool.DisabledColor;
                }
                else
                {
                    btnPageUp.Enabled = true;
                    btnPageUp.BackColor = btnPageUp.DisplayColor;
                }
                px = (columnsCount - 1) * width + (columnsCount - 1 + 1) * m_Space;
                py = (rowsCount - 1) * (height + m_Space);
                btnPageDown.Width = width;
                btnPageDown.Height = height;
                btnPageDown.Location = new Point(px, py);
                if (endIndex >= ConstantValuePool.GoodsGroupButton.Count)
                {
                    btnPageDown.Enabled = false;
                    btnPageDown.BackColor = ConstantValuePool.DisabledColor;
                }
                else
                {
                    btnPageDown.Enabled = true;
                    btnPageDown.BackColor = btnPageDown.DisplayColor;
                }
                //向前
                this.pnlGroup.Controls.Add(btnPageUp);
                //向后
                this.pnlGroup.Controls.Add(btnPageDown);
            }
            this.pnlGroup.ResumeLayout(false);
            this.pnlGroup.PerformLayout();
            this.ResumeLayout(false);
        }

        private void DisplayGoodsButton()
        {
            //禁止引发Layout事件
            this.pnlItem.SuspendLayout();
            this.SuspendLayout();
            //清除pnlItem内所有控件
            this.pnlItem.Controls.Clear();
            if (m_CurrentGoodsGroup != null)
            {
                string goodsGroupID = m_CurrentGoodsGroup.GoodsGroupID.ToString();
                if (ConstantValuePool.DicGoodsButton.ContainsKey(goodsGroupID))
                {
                    List<CrystalButton> btnGoodsList = ConstantValuePool.DicGoodsButton[goodsGroupID];
                    if (btnGoodsList != null && btnGoodsList.Count > 0)
                    {
                        if (ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls != null && ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls.Count > 0)
                        {
                            int width = 0, height = 0, columnsCount = 0, rowsCount = 0, pageSize = 0;
                            foreach (BizControl control in ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls)
                            {
                                if (control.Name == "Goods")
                                {
                                    width = (this.pnlItem.Width - m_Space * control.ColumnsCount) / control.ColumnsCount;
                                    height = (this.pnlItem.Height - m_Space * control.RowsCount) / control.RowsCount;
                                    columnsCount = control.ColumnsCount;
                                    rowsCount = control.RowsCount;
                                    pageSize = control.ColumnsCount * control.RowsCount - 2;    //扣除向前、向后两个按钮
                                    break;
                                }
                            }
                            //显示控件
                            int startIndex = m_GoodsPageIndex * pageSize;
                            int endIndex = (m_GoodsPageIndex + 1) * pageSize;
                            if (endIndex > btnGoodsList.Count)
                            {
                                endIndex = btnGoodsList.Count;
                            }
                            for (int i = startIndex; i < endIndex; i++)
                            {
                                CrystalButton btnGoods = btnGoodsList[i];
                                btnGoods.Click -= new System.EventHandler(this.btnGoods_Click);
                                btnGoods.Click += new System.EventHandler(this.btnGoods_Click);
                                if (m_ShowSilverCode)
                                {
                                    if (btnGoods.Text.IndexOf("￥") > 0)
                                    {
                                        //do nothing
                                    }
                                    else
                                    {
                                        Goods temp = btnGoods.Tag as Goods;
                                        btnGoods.Text += "\r\n ￥" + temp.SellPrice.ToString("f2");
                                    }
                                }
                                else
                                {
                                    if (btnGoods.Text.IndexOf("￥") > 0)
                                    {
                                        btnGoods.Text = btnGoods.Text.Substring(0, btnGoods.Text.IndexOf("￥") - 3);
                                    }
                                    else
                                    {
                                        //do nothing
                                    }
                                }
                                this.pnlItem.Controls.Add(btnGoods);
                            }
                            //设置页码按钮的位置
                            int px = (columnsCount - 2) * width + (columnsCount - 2 + 1) * m_Space;
                            int py = (rowsCount - 1) * (height + m_Space);
                            btnHead.Width = width;
                            btnHead.Height = height;
                            btnHead.Location = new Point(px, py);
                            if (startIndex <= 0)
                            {
                                btnHead.Enabled = false;
                                btnHead.BackColor = ConstantValuePool.DisabledColor;
                            }
                            else
                            {
                                btnHead.Enabled = true;
                                btnHead.BackColor = btnHead.DisplayColor;
                            }
                            px = (columnsCount - 1) * width + (columnsCount - 1 + 1) * m_Space;
                            py = (rowsCount - 1) * (height + m_Space);
                            btnBack.Width = width;
                            btnBack.Height = height;
                            btnBack.Location = new Point(px, py);
                            if (endIndex >= btnGoodsList.Count)
                            {
                                btnBack.Enabled = false;
                                btnBack.BackColor = ConstantValuePool.DisabledColor;
                            }
                            else
                            {
                                btnBack.Enabled = true;
                                btnBack.BackColor = btnBack.DisplayColor;
                            }
                            //向前
                            this.pnlItem.Controls.Add(btnHead);
                            //向后
                            this.pnlItem.Controls.Add(btnBack);
                        }
                    }
                }
            }
            this.pnlItem.ResumeLayout(false);
            this.pnlItem.PerformLayout();
            this.ResumeLayout(false);
        }

        private void DisplayDetailGroupButton()
        {
            List<DetailsGroup> detailGroupList = new List<DetailsGroup>();
            foreach (DetailsGroup item in ConstantValuePool.DetailsGroupList)
            {
                if (m_CurrentDetailsGroupIDList.Contains(item.DetailsGroupID))
                {
                    detailGroupList.Add(item);
                }
            }
            if (detailGroupList.Count > 0)
            {
                //禁止引发Layout事件
                this.pnlGroup.SuspendLayout();
                this.SuspendLayout();
                //清除pnlGroup内所有控件
                this.pnlGroup.Controls.Clear();
                //显示控件
                if (ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls != null && ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls.Count > 0)
                {
                    int width = 0, height = 0, columnsCount = 0, rowsCount = 0, pageSize = 0;
                    foreach (BizControl control in ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls)
                    {
                        if (control.Name == "DetailsGroup")
                        {
                            width = (this.pnlGroup.Width - m_Space * control.ColumnsCount) / control.ColumnsCount;
                            height = (this.pnlGroup.Height - m_Space * control.RowsCount) / control.RowsCount;
                            columnsCount = control.ColumnsCount;
                            rowsCount = control.RowsCount;
                            pageSize = control.ColumnsCount * control.RowsCount - 2;    //扣除向前、向后两个按钮
                            break;
                        }
                    }
                    int startIndex = m_DetailsGroupPageIndex * pageSize;
                    int endIndex = (m_DetailsGroupPageIndex + 1) * pageSize;
                    if (endIndex > detailGroupList.Count)
                    {
                        endIndex = detailGroupList.Count;
                    }
                    //坐标
                    int px = m_Space, py = 0, times = 0;
                    for (int i = startIndex; i < endIndex; i++)
                    {
                        //实例化Button
                        CrystalButton btn = new CrystalButton();
                        btn.Name = detailGroupList[i].DetailsGroupID.ToString();
                        btn.Text = detailGroupList[i].DetailsGroupName;
                        btn.Width = width;
                        btn.Height = height;
                        btn.Location = new Point(px, py);
                        btn.Tag = detailGroupList[i];
                        btn.Click += new System.EventHandler(this.btnDetailGroup_Click);
                        foreach (ButtonStyle btnStyle in ConstantValuePool.ButtonStyleList)
                        {
                            if (detailGroupList[i].ButtonStyleID.Equals(btnStyle.ButtonStyleID))
                            {
                                float emSize = (float)btnStyle.FontSize;
                                FontStyle style = FontStyle.Regular;
                                btn.Font = new Font(btnStyle.FontName, emSize, style);
                                btn.ForeColor = ColorConvert.RGB(btnStyle.ForeColor);
                                btn.BackColor = btn.DisplayColor = ColorConvert.RGB(btnStyle.BackColor);
                                break;
                            }
                        }
                        this.pnlGroup.Controls.Add(btn);
                        //计算Button位置
                        times++;
                        px += m_Space + width;
                        if (times == columnsCount)
                        {
                            px = m_Space;
                            times = 0;
                            py += m_Space + height;
                        }
                    }
                    //设置页码按钮的位置
                    px = (columnsCount - 2) * width + (columnsCount - 2 + 1) * m_Space;
                    py = (rowsCount - 1) * (height + m_Space);
                    btnPageUp.Width = width;
                    btnPageUp.Height = height;
                    btnPageUp.Location = new Point(px, py);
                    if (startIndex <= 0)
                    {
                        btnPageUp.Enabled = false;
                        btnPageUp.BackColor = ConstantValuePool.DisabledColor;
                    }
                    else
                    {
                        btnPageUp.Enabled = true;
                        btnPageUp.BackColor = btnPageUp.DisplayColor;
                    }
                    px = (columnsCount - 1) * width + (columnsCount - 1 + 1) * m_Space;
                    py = (rowsCount - 1) * (height + m_Space);
                    btnPageDown.Width = width;
                    btnPageDown.Height = height;
                    btnPageDown.Location = new Point(px, py);
                    if (endIndex >= detailGroupList.Count)
                    {
                        btnPageDown.Enabled = false;
                        btnPageDown.BackColor = ConstantValuePool.DisabledColor;
                    }
                    else
                    {
                        btnPageDown.Enabled = true;
                        btnPageDown.BackColor = btnPageDown.DisplayColor;
                    }
                    //向前
                    this.pnlGroup.Controls.Add(btnPageUp);
                    //向后
                    this.pnlGroup.Controls.Add(btnPageDown);
                }
                this.pnlGroup.ResumeLayout(false);
                this.pnlGroup.PerformLayout();
                this.ResumeLayout(false);
            }
        }

        private void DisplayDetailButton()
        {
            //禁止引发Layout事件
            this.pnlItem.SuspendLayout();
            this.SuspendLayout();
            //清除pnlItem内所有控件
            this.pnlItem.Controls.Clear();
            string detailGroupID = m_CurrentDetailsGroup.DetailsGroupID.ToString();
            if (ConstantValuePool.DicDetailsButton.ContainsKey(detailGroupID))
            {
                List<CrystalButton> btnDetailList = ConstantValuePool.DicDetailsButton[detailGroupID];
                if (btnDetailList != null && btnDetailList.Count > 0)
                {
                    if (ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls != null && ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls.Count > 0)
                    {
                        int width = 0, height = 0, columnsCount = 0, rowsCount = 0, pageSize = 0;
                        foreach (BizControl control in ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls)
                        {
                            if (control.Name == "Details")
                            {
                                width = (this.pnlItem.Width - m_Space * control.ColumnsCount) / control.ColumnsCount;
                                height = (this.pnlItem.Height - m_Space * control.RowsCount) / control.RowsCount;
                                columnsCount = control.ColumnsCount;
                                rowsCount = control.RowsCount;
                                pageSize = control.ColumnsCount * control.RowsCount - 2;    //扣除向前、向后两个按钮
                                break;
                            }
                        }
                        //显示控件
                        int startIndex = m_DetailsPageIndex * pageSize;
                        int endIndex = (m_DetailsPageIndex + 1) * pageSize;
                        if (endIndex > btnDetailList.Count)
                        {
                            endIndex = btnDetailList.Count;
                        }
                        for (int i = startIndex; i < endIndex; i++)
                        {
                            CrystalButton btnDetails = btnDetailList[i];
                            btnDetails.Click -= new System.EventHandler(this.btnDetails_Click);
                            btnDetails.Click += new System.EventHandler(this.btnDetails_Click);
                            this.pnlItem.Controls.Add(btnDetails);
                        }
                        //设置页码按钮的位置
                        int px = (columnsCount - 2) * width + (columnsCount - 2 + 1) * m_Space;
                        int py = (rowsCount - 1) * (height + m_Space);
                        btnHead.Width = width;
                        btnHead.Height = height;
                        btnHead.Location = new Point(px, py);
                        if (startIndex <= 0)
                        {
                            btnHead.Enabled = false;
                            btnHead.BackColor = ConstantValuePool.DisabledColor;
                        }
                        else
                        {
                            btnHead.Enabled = true;
                            btnHead.BackColor = btnHead.DisplayColor;
                        }
                        px = (columnsCount - 1) * width + (columnsCount - 1 + 1) * m_Space;
                        py = (rowsCount - 1) * (height + m_Space);
                        btnBack.Width = width;
                        btnBack.Height = height;
                        btnBack.Location = new Point(px, py);
                        if (endIndex >= btnDetailList.Count)
                        {
                            btnBack.Enabled = false;
                            btnBack.BackColor = ConstantValuePool.DisabledColor;
                        }
                        else
                        {
                            btnBack.Enabled = true;
                            btnBack.BackColor = btnBack.DisplayColor;
                        }
                        //向前
                        this.pnlItem.Controls.Add(btnHead);
                        //向后
                        this.pnlItem.Controls.Add(btnBack);
                    }
                }
            }
            this.pnlItem.ResumeLayout(false);
            this.pnlItem.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private void LoadDefaultGoodsGroupButton()
        {
            m_GoodsGroupPageIndex = 0;
            m_GoodsPageIndex = 0;
            DisplayGoodsGroupButton();
            this.pnlItem.Controls.Clear();
        }

        #region goods or detail button event

        private void btnGoodsGroup_Click(object sender, EventArgs e)
        {
            CrystalButton btnGoodsGroup = sender as CrystalButton;
            m_CurrentGoodsGroup = btnGoodsGroup.Tag as GoodsGroup;
            Color pressedColor = ConstantValuePool.PressedColor;
            foreach (ButtonStyle btnStyle in ConstantValuePool.ButtonStyleList)
            {
                if (m_CurrentGoodsGroup.ButtonStyleID.Equals(btnStyle.ButtonStyleID))
                {
                    pressedColor = ColorConvert.RGB(btnStyle.ClickedBackColor);
                    break;
                }
            }
            btnGoodsGroup.BackColor = pressedColor;
            if (prevPressedButton == null)
            {
                prevPressedButton = btnGoodsGroup;
            }
            else
            {
                prevPressedButton.BackColor = prevPressedButton.DisplayColor;
                prevPressedButton = btnGoodsGroup;
            }

            m_GoodsPageIndex = 0;
            DisplayGoodsButton();
        }

        private void btnGoods_Click(object sender, EventArgs e)
        {
            CrystalButton btnGoods = sender as CrystalButton;
            Goods goods = btnGoods.Tag as Goods;
            int index, selectedIndex;
            index = selectedIndex = dgvGoodsOrder.Rows.Add(new DataGridViewRow());
            dgvGoodsOrder.Rows[index].Cells["ItemID"].Value = goods.GoodsID;
            dgvGoodsOrder.Rows[index].Cells["ItemID"].Tag = goods;
            dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value = 1;
            dgvGoodsOrder.Rows[index].Cells["GoodsName"].Value = goods.GoodsName;
            dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value = goods.SellPrice;
            dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value = 0;
            dgvGoodsOrder.Rows[index].Cells["ItemType"].Value = OrderItemType.Goods;
            dgvGoodsOrder.Rows[index].Cells["CanDiscount"].Value = goods.CanDiscount;
            dgvGoodsOrder.Rows[index].Cells["ItemUnit"].Value = goods.Unit;

            #region 判断是否套餐
            bool haveCirculate = false;
            foreach (GoodsSetMeal item in ConstantValuePool.GoodsSetMealList)
            {
                if (item.ParentGoodsID.Equals(goods.GoodsID))
                {
                    Goods temp = new Goods();
                    temp.GoodsID = item.ItemID;
                    temp.GoodsNo = item.ItemNo;
                    temp.GoodsName = item.ItemName;
                    temp.GoodsName2nd = item.ItemName2nd;
                    temp.Unit = item.Unit;
                    temp.SellPrice = item.SellPrice;
                    temp.CanDiscount = item.CanDiscount;
                    temp.AutoShowDetails = item.AutoShowDetails;
                    temp.PrintSolutionName = item.PrintSolutionName;
                    temp.DepartID = item.DepartID;
                    //更新列表
                    index = dgvGoodsOrder.Rows.Add(new DataGridViewRow());
                    dgvGoodsOrder.Rows[index].Cells["ItemID"].Value = item.ItemID;
                    dgvGoodsOrder.Rows[index].Cells["ItemID"].Tag = temp;
                    dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value = item.ItemQty;
                    dgvGoodsOrder.Rows[index].Cells["GoodsName"].Value = "--" + item.ItemName;
                    dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value = item.SellPrice;
                    decimal discount = 0;
                    if (item.DiscountRate > 0)
                    {
                        discount = item.SellPrice * item.ItemQty * item.DiscountRate;
                    }
                    else
                    {
                        discount = item.OffFixPay;
                    }
                    dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value = (-discount).ToString("f2");
                    dgvGoodsOrder.Rows[index].Cells["ItemType"].Value = OrderItemType.SetMeal;
                    dgvGoodsOrder.Rows[index].Cells["CanDiscount"].Value = item.CanDiscount;
                    dgvGoodsOrder.Rows[index].Cells["ItemUnit"].Value = item.Unit;

                    #region 判断套餐项是否含细项
                    bool haveDetails = false;
                    foreach (GoodsSetMeal detailsSetMeal in ConstantValuePool.DetailsSetMealList)
                    {
                        if (detailsSetMeal.ParentGoodsID.Equals(temp.GoodsID))
                        {
                            Details details = new Details();
                            details.DetailsID = detailsSetMeal.ItemID;
                            details.DetailsNo = detailsSetMeal.ItemNo;
                            details.DetailsName = detailsSetMeal.ItemName;
                            details.DetailsName2nd = detailsSetMeal.ItemName2nd;
                            details.SellPrice = detailsSetMeal.SellPrice;
                            details.CanDiscount = detailsSetMeal.CanDiscount;
                            details.AutoShowDetails = detailsSetMeal.AutoShowDetails;
                            details.PrintSolutionName = detailsSetMeal.PrintSolutionName;
                            details.DepartID = detailsSetMeal.DepartID;
                            //更新列表
                            index = dgvGoodsOrder.Rows.Add(new DataGridViewRow());
                            dgvGoodsOrder.Rows[index].Cells["ItemID"].Value = detailsSetMeal.ItemID;
                            dgvGoodsOrder.Rows[index].Cells["ItemID"].Tag = details;
                            dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value = detailsSetMeal.ItemQty;
                            dgvGoodsOrder.Rows[index].Cells["GoodsName"].Value = "----" + detailsSetMeal.ItemName;
                            dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value = detailsSetMeal.SellPrice;
                            decimal detailsDiscount = 0;
                            if (detailsSetMeal.DiscountRate > 0)
                            {
                                detailsDiscount = detailsSetMeal.SellPrice * detailsSetMeal.ItemQty * detailsSetMeal.DiscountRate;
                            }
                            else
                            {
                                detailsDiscount = detailsSetMeal.OffFixPay;
                            }
                            dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value = (-detailsDiscount).ToString("f2");
                            dgvGoodsOrder.Rows[index].Cells["ItemType"].Value = OrderItemType.SetMeal;
                            dgvGoodsOrder.Rows[index].Cells["CanDiscount"].Value = detailsSetMeal.CanDiscount;
                            haveDetails = true;
                        }
                        else
                        {
                            if (haveDetails) break;
                        }
                    }
                    #endregion

                    haveCirculate = true;
                }
                else
                {
                    if (haveCirculate) break;
                }
            }
            #endregion

            #region 判断是否自动显示细项组
            if (goods.AutoShowDetails)
            {
                m_DetailsGroupPageIndex = 0;
                m_DetailsPageIndex = 0;
                if (goods.DetailsGroupIDList != null && goods.DetailsGroupIDList.Count > 0)
                {
                    m_GoodsOrDetails = false;    //状态为细项
                    m_CurrentDetailsGroupIDList = goods.DetailsGroupIDList;
                    DisplayDetailGroupButton();
                    this.pnlItem.Controls.Clear();
                    m_DetailsPrefix = "--";
                }
            }
            #endregion

            //datagridview滚动条定位
            dgvGoodsOrder.Rows[selectedIndex].Selected = true;
            dgvGoodsOrder.CurrentCell = dgvGoodsOrder.Rows[selectedIndex].Cells["GoodsNum"];
            //统计
            BindOrderInfoSum();
        }

        private void btnDetailGroup_Click(object sender, EventArgs e)
        {
            prevPressedButton.BackColor = prevPressedButton.DisplayColor;

            CrystalButton btnDetailGroup = sender as CrystalButton;
            DetailsGroup detailsGroup = btnDetailGroup.Tag as DetailsGroup;
            Color pressedColor = ConstantValuePool.PressedColor;
            foreach (ButtonStyle btnStyle in ConstantValuePool.ButtonStyleList)
            {
                if (detailsGroup.ButtonStyleID.Equals(btnStyle.ButtonStyleID))
                {
                    pressedColor = ColorConvert.RGB(btnStyle.ClickedBackColor);
                    break;
                }
            }
            btnDetailGroup.BackColor = pressedColor;
            prevPressedButton = btnDetailGroup;
            if (detailsGroup.DetailsList != null && detailsGroup.DetailsList.Count > 0)
            {
                m_CurrentDetailsGroup = detailsGroup;
                m_DetailsPageIndex = 0;
                DisplayDetailButton();
            }
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.CurrentRow != null)
            {
                int selectIndex = dgvGoodsOrder.CurrentRow.Index;
                //数量
                decimal itemNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                CrystalButton btnDetails = sender as CrystalButton;
                Details details = btnDetails.Tag as Details;
                DataGridViewRow dgr = dgvGoodsOrder.Rows[0].Clone() as DataGridViewRow;
                dgr.Cells[0].Value = details.DetailsID;
                dgr.Cells[0].Tag = details;
                dgr.Cells[1].Value = itemNum;
                dgr.Cells[2].Value = m_DetailsPrefix + details.DetailsName;
                dgr.Cells[3].Value = details.SellPrice;
                dgr.Cells[4].Value = 0;
                dgr.Cells[5].Value = OrderItemType.Details;
                dgr.Cells[6].Value = details.CanDiscount;
                int rowIndex = selectIndex + 1;
                if (rowIndex == dgvGoodsOrder.Rows.Count)
                {
                    dgvGoodsOrder.Rows.Add(dgr);
                }
                else
                {
                    if (Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value) == (int)OrderItemType.Goods)
                    {
                        for (int i = selectIndex + 1; i < dgvGoodsOrder.RowCount; i++)
                        {
                            int itemType = Convert.ToInt32(dgvGoodsOrder.Rows[i].Cells["ItemType"].Value);
                            if (itemType == (int)OrderItemType.Goods)
                            {
                                break;
                            }
                            else
                            {
                                rowIndex++;
                            }
                        }
                    }
                    dgvGoodsOrder.Rows.Insert(rowIndex, dgr);
                }
                //统计
                BindOrderInfoSum();
            }
        }

        #endregion

        #region 分页事件

        private void btnPageUp_Click(object sender, EventArgs e)
        {
            if (m_GoodsOrDetails)
            {
                m_GoodsGroupPageIndex--;
                DisplayGoodsGroupButton();
            }
            else
            {
                m_DetailsGroupPageIndex--;
                DisplayDetailGroupButton();
            }
        }

        private void btnPageDown_Click(object sender, EventArgs e)
        {
            if (m_GoodsOrDetails)
            {
                m_GoodsGroupPageIndex++;
                DisplayGoodsGroupButton();
            }
            else
            {
                m_DetailsGroupPageIndex++;
                DisplayDetailGroupButton();
            }
        }

        private void btnHead_Click(object sender, EventArgs e)
        {
            if (m_GoodsOrDetails)
            {
                m_GoodsPageIndex--;
                DisplayGoodsButton();
            }
            else
            {
                m_DetailsPageIndex--;
                DisplayDetailButton();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (m_GoodsOrDetails)
            {
                m_GoodsPageIndex++;
                DisplayGoodsButton();
            }
            else
            {
                m_DetailsPageIndex++;
                DisplayDetailButton();
            }
        }

        private void btnPgUp_Click(object sender, EventArgs e)
        {

        }

        private void btnPgDown_Click(object sender, EventArgs e)
        {

        }
        #endregion

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
                    dgvGoodsOrder.Rows[index].Cells["ItemType"].Value = orderDetails.ItemType;
                    dgvGoodsOrder.Rows[index].Cells["CanDiscount"].Value = orderDetails.CanDiscount;
                    dgvGoodsOrder.Rows[index].Cells["ItemUnit"].Value = orderDetails.Unit;
                    dgvGoodsOrder.Rows[index].Cells["Wait"].Value = orderDetails.Wait;
                    dgvGoodsOrder.Rows[index].Cells["OrderDetailsID"].Value = orderDetails.OrderDetailsID;
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
            this.lbCutOff.Text = "去零：" + (-m_CutOff).ToString("f2");
        }

        private void dgvGoodsOrder_MouseDown(object sender, MouseEventArgs e)
        {
            if (!m_GoodsOrDetails)
            {
                prevPressedButton.BackColor = prevPressedButton.DisplayColor;
                m_GoodsOrDetails = true;
                LoadDefaultGoodsGroupButton();
            }
        }

        private void btnFuncPanel_Click(object sender, EventArgs e)
        {
            Feature.FormFunctionPanel form = new Feature.FormFunctionPanel();
            form.ShowDialog();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.Rows.Count > 0 && dgvGoodsOrder.CurrentRow != null)
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
                int itemType = Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value);
                if (itemType == (int)OrderItemType.Goods)   //主项才能打折
                {
                    bool canDiscount = Convert.ToBoolean(dgvGoodsOrder.Rows[selectIndex].Cells["CanDiscount"].Value);
                    if (canDiscount)
                    {
                        FormDiscount formDiscount = new FormDiscount(DiscountDisplayModel.SingleDiscount);
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
                                    if (Convert.ToInt32(dgvGoodsOrder.Rows[index].Cells["ItemType"].Value) == (int)OrderItemType.Goods)
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
                            //统计
                            BindOrderInfoSum();
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
                    bool canDiscount = Convert.ToBoolean(dr.Cells["CanDiscount"].Value);
                    if (canDiscount)
                    {
                        if (discount.DiscountType == (int)DiscountItemType.DiscountRate)
                        {
                            dr.Cells["GoodsDiscount"].Value = -Convert.ToDecimal(dr.Cells["GoodsPrice"].Value) * discount.DiscountRate;
                        }
                        else
                        {
                            dr.Cells["GoodsDiscount"].Value = -discount.OffFixPay;
                        }
                        dr.Cells["GoodsDiscount"].Tag = discount;
                    }
                }
                //统计
                BindOrderInfoSum();
            }
        }

        private void btnTakeOut_Click(object sender, EventArgs e)
        {
            if (SubmitSalesOrder(deskName, EatWayType.Takeout))
            {
                this.lbTotalPrice.Text = "总金额：";
                this.lbDiscount.Text = "折扣：";
                this.lbNeedPayMoney.Text = "实际应付：";
                this.lbCutOff.Text = "去零：";
                dgvGoodsOrder.Rows.Clear();
                //加载外卖单列表
                m_PageIndex = 0;
                CleanDeliveryOrderButton();
                OrderService orderService = new OrderService();
                IList<DeliveryOrder> deliveryOrderList = orderService.GetDeliveryOrderList();
                if (deliveryOrderList != null)
                {
                    DisplayDeliveryOrderButton(deliveryOrderList);
                }
            }
            else
            {
                MessageBox.Show("外带提交失败，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBeNew_Click(object sender, EventArgs e)
        {
            this.lbTotalPrice.Text = "总金额：";
            this.lbDiscount.Text = "折扣：";
            this.lbNeedPayMoney.Text = "实际应付：";
            this.lbCutOff.Text = "去零：";
            dgvGoodsOrder.Rows.Clear();
        }

        private void btnOutsideOrder_Click(object sender, EventArgs e)
        {
            if (SubmitSalesOrder(deskName, EatWayType.OutsideOrder))
            {
                this.lbTotalPrice.Text = "总金额：";
                this.lbDiscount.Text = "折扣：";
                this.lbNeedPayMoney.Text = "实际应付：";
                this.lbCutOff.Text = "去零：";
                dgvGoodsOrder.Rows.Clear();
                //加载外卖单列表
                m_PageIndex = 0;
                CleanDeliveryOrderButton();
                OrderService orderService = new OrderService();
                IList<DeliveryOrder> deliveryOrderList = orderService.GetDeliveryOrderList();
                if (deliveryOrderList != null)
                {
                    DisplayDeliveryOrderButton(deliveryOrderList);
                }
            }
            else
            {
                MessageBox.Show("外送提交失败，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeliveryGoods_Click(object sender, EventArgs e)
        {

        }

        private void btnPrintBill_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPriceCode_Click(object sender, EventArgs e)
        {
            m_ShowSilverCode = !m_ShowSilverCode;
            DisplayGoodsButton();
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            CrystalButton btnDelivery = sender as CrystalButton;
            DeliveryOrder deliveryOrder = btnDelivery.Tag as DeliveryOrder;
            if (deliveryOrder.PayTime == null)
            {
                SalesOrderService salesOrderService = new SalesOrderService();
                SalesOrder _salesOrder = salesOrderService.GetSalesOrder(deliveryOrder.OrderID);
                if (_salesOrder != null)
                {
                    m_SalesOrder = _salesOrder;
                    BindGoodsOrderInfo();   //绑定订单信息
                    BindOrderInfoSum();
                }
                else
                {
                    MessageBox.Show("获取账单信息失败，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            { 
                SalesOrderService salesOrderService = new SalesOrderService();
                SalesOrder _salesOrder = salesOrderService.GetSalesOrder(deliveryOrder.OrderID);
                if (_salesOrder != null)
                {
                    FormTakeGoods form = new FormTakeGoods(_salesOrder);
                    form.ShowDialog();
                    if (form.HasDeliveried)
                    {
                        btnDelivery.Enabled = false;
                        btnDelivery.BackColor = ConstantValuePool.DisabledColor;
                    }
                }
            }
        }

        private bool SubmitSalesOrder(string deskName, EatWayType eatType)
        {
            bool result = false;
            Guid orderID = Guid.Empty;
            if (m_SalesOrder == null)    //新增的菜单
            {
                orderID = Guid.NewGuid();
            }
            else
            {
                orderID = m_SalesOrder.order.OrderID;
            }
            List<OrderDetails> newOrderDetailsList = new List<OrderDetails>();
            List<OrderDiscount> newOrderDiscountList = new List<OrderDiscount>();
            foreach (DataGridViewRow dr in dgvGoodsOrder.Rows)
            {
                if (dr.Cells["OrderDetailsID"].Value == null)
                {
                    Guid orderDetailsID = Guid.NewGuid();
                    int itemType = Convert.ToInt32(dr.Cells["ItemType"].Value);
                    string goodsName = dr.Cells["GoodsName"].Value.ToString();
                    //填充OrderDetails
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.OrderDetailsID = orderDetailsID;
                    orderDetails.OrderID = orderID;
                    orderDetails.DeviceNo = ConstantValuePool.BizSettingConfig.DeviceNo;
                    orderDetails.TotalSellPrice = Convert.ToDecimal(dr.Cells["GoodsPrice"].Value);
                    orderDetails.TotalDiscount = Convert.ToDecimal(dr.Cells["GoodsDiscount"].Value);
                    orderDetails.ItemQty = Convert.ToDecimal(dr.Cells["GoodsNum"].Value);
                    orderDetails.EmployeeID = m_EmployeeID;
                    if (dr.Cells["Wait"].Value != null)
                    {
                        orderDetails.Wait = Convert.ToInt32(dr.Cells["Wait"].Value);
                    }
                    if (itemType == (int)OrderItemType.Goods)
                    {
                        orderDetails.ItemType = (int)OrderItemType.Goods;
                        Goods goods = dr.Cells["ItemID"].Tag as Goods;
                        orderDetails.GoodsID = goods.GoodsID;
                        orderDetails.GoodsNo = goods.GoodsNo;
                        orderDetails.GoodsName = goods.GoodsName;
                        orderDetails.Unit = goods.Unit;
                        orderDetails.CanDiscount = goods.CanDiscount;
                        orderDetails.SellPrice = goods.SellPrice;
                        orderDetails.PrintSolutionName = goods.PrintSolutionName;
                        orderDetails.DepartID = goods.DepartID;
                    }
                    else if (itemType == (int)OrderItemType.Details)
                    {
                        orderDetails.ItemType = (int)OrderItemType.Details;
                        Details details = dr.Cells["ItemID"].Tag as Details;
                        orderDetails.GoodsID = details.DetailsID;
                        orderDetails.GoodsNo = details.DetailsNo;
                        orderDetails.GoodsName = details.DetailsName;
                        orderDetails.CanDiscount = details.CanDiscount;
                        orderDetails.Unit = "";  //
                        orderDetails.SellPrice = details.SellPrice;
                        orderDetails.PrintSolutionName = details.PrintSolutionName;
                        orderDetails.DepartID = details.DepartID;

                        int index = goodsName.LastIndexOf('-');
                        string itemPrefix = goodsName.Substring(0, index + 1);
                        orderDetails.ItemLevel = itemPrefix.Length / 2;
                    }
                    else if (itemType == (int)OrderItemType.SetMeal)
                    {
                        orderDetails.ItemType = (int)OrderItemType.SetMeal;
                        int index = goodsName.LastIndexOf('-');
                        string itemPrefix = goodsName.Substring(0, index + 1);
                        orderDetails.ItemLevel = itemPrefix.Length / 2;

                        object item = dr.Cells["ItemID"].Tag;
                        if (item is Goods)
                        {
                            Goods goods = item as Goods;
                            orderDetails.GoodsID = goods.GoodsID;
                            orderDetails.GoodsNo = goods.GoodsNo;
                            orderDetails.GoodsName = goods.GoodsName;
                            orderDetails.CanDiscount = goods.CanDiscount;
                            orderDetails.Unit = goods.Unit;
                            orderDetails.SellPrice = goods.SellPrice;
                            orderDetails.PrintSolutionName = goods.PrintSolutionName;
                            orderDetails.DepartID = goods.DepartID;
                        }
                        else if (item is Details)
                        {
                            Details details = item as Details;
                            orderDetails.GoodsID = details.DetailsID;
                            orderDetails.GoodsNo = details.DetailsNo;
                            orderDetails.GoodsName = details.DetailsName;
                            orderDetails.CanDiscount = details.CanDiscount;
                            orderDetails.Unit = "";  //
                            orderDetails.SellPrice = details.SellPrice;
                            orderDetails.PrintSolutionName = details.PrintSolutionName;
                            orderDetails.DepartID = details.DepartID;
                        }
                    }
                    newOrderDetailsList.Add(orderDetails);
                    //填充OrderDiscount
                    Discount discount = dr.Cells["GoodsDiscount"].Tag as Discount;
                    if (discount != null)
                    {
                        decimal offPay = Math.Abs(Convert.ToDecimal(dr.Cells["GoodsDiscount"].Value));
                        if (offPay > 0)
                        {
                            OrderDiscount orderDiscount = new OrderDiscount();
                            orderDiscount.OrderDiscountID = Guid.NewGuid();
                            orderDiscount.OrderID = orderID;
                            orderDiscount.OrderDetailsID = orderDetailsID;
                            orderDiscount.DiscountID = discount.DiscountID;
                            orderDiscount.DiscountName = discount.DiscountName;
                            orderDiscount.DiscountType = discount.DiscountType;
                            orderDiscount.DiscountRate = discount.DiscountRate;
                            orderDiscount.OffFixPay = discount.OffFixPay;
                            orderDiscount.OffPay = offPay;
                            orderDiscount.EmployeeID = m_EmployeeID;
                            newOrderDiscountList.Add(orderDiscount);
                        }
                    }
                }
                else
                {
                    //修改过折扣的账单
                    Discount discount = dr.Cells["GoodsDiscount"].Tag as Discount;
                    if (discount != null)
                    {
                        Guid orderDetailsID = new Guid(dr.Cells["OrderDetailsID"].Value.ToString());
                        int itemType = Convert.ToInt32(dr.Cells["ItemType"].Value);
                        string goodsName = dr.Cells["GoodsName"].Value.ToString();
                        //填充OrderDetails
                        OrderDetails orderDetails = new OrderDetails();
                        orderDetails.OrderDetailsID = orderDetailsID;
                        orderDetails.OrderID = orderID;
                        orderDetails.DeviceNo = ConstantValuePool.BizSettingConfig.DeviceNo;
                        orderDetails.ItemQty = Convert.ToDecimal(dr.Cells["GoodsNum"].Value);
                        orderDetails.GoodsName = dr.Cells["GoodsName"].Value.ToString();
                        orderDetails.TotalSellPrice = Convert.ToDecimal(dr.Cells["GoodsPrice"].Value);
                        orderDetails.TotalDiscount = Convert.ToDecimal(dr.Cells["GoodsDiscount"].Value);
                        orderDetails.Unit = dr.Cells["ItemUnit"].Value.ToString();
                        orderDetails.EmployeeID = m_EmployeeID;
                        if (dr.Cells["Wait"].Value != null)
                        {
                            orderDetails.Wait = Convert.ToInt32(dr.Cells["Wait"].Value);
                        }
                        newOrderDetailsList.Add(orderDetails);
                        //填充OrderDiscount
                        decimal offPay = Math.Abs(Convert.ToDecimal(dr.Cells["GoodsDiscount"].Value));
                        if (offPay > 0)
                        {
                            OrderDiscount orderDiscount = new OrderDiscount();
                            orderDiscount.OrderDiscountID = Guid.NewGuid();
                            orderDiscount.OrderID = orderID;
                            orderDiscount.OrderDetailsID = orderDetailsID;
                            orderDiscount.DiscountID = discount.DiscountID;
                            orderDiscount.DiscountName = discount.DiscountName;
                            orderDiscount.DiscountType = discount.DiscountType;
                            orderDiscount.DiscountRate = discount.DiscountRate;
                            orderDiscount.OffFixPay = discount.OffFixPay;
                            orderDiscount.OffPay = offPay;
                            orderDiscount.EmployeeID = m_EmployeeID;
                            newOrderDiscountList.Add(orderDiscount);
                        }
                    }
                }
            }
            int _tranSeq = 0;
            if (m_SalesOrder == null)    //新增的菜单
            {
                Order order = new Order();
                order.OrderID = orderID;
                order.TotalSellPrice = m_TotalPrice;
                order.ActualSellPrice = m_ActualPayMoney;
                order.DiscountPrice = m_Discount;
                order.CutOffPrice = m_CutOff;
                order.ServiceFee = 0;
                order.DeviceNo = ConstantValuePool.BizSettingConfig.DeviceNo;
                order.DeskName = deskName;
                order.EatType = (int)eatType;
                order.Status = 0;
                order.PeopleNum = 1;
                order.EmployeeID = m_EmployeeID;
                order.EmployeeNo = m_EmployeeNo;
                order.OrderLastTime = 0;

                SalesOrder salesOrder = new SalesOrder();
                salesOrder.order = order;
                salesOrder.orderDetailsList = newOrderDetailsList;
                salesOrder.orderDiscountList = newOrderDiscountList;
                SalesOrderService orderService = new SalesOrderService();
                _tranSeq = orderService.CreateSalesOrder(salesOrder);
                result = _tranSeq > 0;
            }
            else
            {
                if (newOrderDetailsList.Count > 0)
                {
                    Order order = new Order();
                    order.OrderID = orderID;
                    order.TotalSellPrice = m_TotalPrice;
                    order.ActualSellPrice = m_ActualPayMoney;
                    order.DiscountPrice = m_Discount;
                    order.CutOffPrice = m_CutOff;
                    order.ServiceFee = 0;
                    order.DeviceNo = ConstantValuePool.BizSettingConfig.DeviceNo;
                    order.DeskName = deskName;
                    order.PeopleNum = 1;
                    order.EmployeeID = m_EmployeeID;
                    order.EmployeeNo = m_EmployeeNo;
                    order.OrderLastTime = 0;
                    SalesOrder salesOrder = new SalesOrder();
                    salesOrder.order = order;
                    salesOrder.orderDetailsList = newOrderDetailsList;
                    salesOrder.orderDiscountList = newOrderDiscountList;
                    SalesOrderService orderService = new SalesOrderService();
                    result = orderService.UpdateSalesOrder(salesOrder) == 1;

                    _tranSeq = m_SalesOrder.order.TranSequence;
                }
            }
            if (result)
            {
                if (ConstantValuePool.BizSettingConfig.printConfig.Enabled)
                {
                    //打印
                    PrintData printData = new PrintData();
                    printData.ShopName = ConstantValuePool.CurrentShop.ShopName;
                    printData.DeskName = deskName;
                    printData.PersonNum = "1";
                    printData.PrintTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    printData.EmployeeNo = m_EmployeeNo;
                    printData.TranSequence = _tranSeq.ToString();
                    printData.ShopAddress = ConstantValuePool.CurrentShop.RunAddress;
                    printData.Telephone = ConstantValuePool.CurrentShop.Telephone;
                    printData.GoodsOrderList = new List<GoodsOrder>();
                    foreach (OrderDetails item in newOrderDetailsList)
                    {
                        string strLevelFlag = string.Empty;
                        if (item.ItemLevel > 0)
                        {
                            int levelCount = item.ItemLevel * 2;
                            for (int i = 0; i < levelCount; i++)
                            {
                                strLevelFlag += "-";
                            }
                        }
                        GoodsOrder goodsOrder = new GoodsOrder();
                        goodsOrder.GoodsName = strLevelFlag + item.GoodsName;
                        goodsOrder.GoodsNum = item.ItemQty.ToString("f1");
                        goodsOrder.SellPrice = (item.TotalSellPrice / item.ItemQty).ToString("f2");
                        goodsOrder.TotalSellPrice = item.TotalSellPrice.ToString("f2");
                        goodsOrder.TotalDiscount = item.TotalDiscount.ToString("f2");
                        goodsOrder.Unit = item.Unit;
                        printData.GoodsOrderList.Add(goodsOrder);
                    }
                    int copies = ConstantValuePool.BizSettingConfig.printConfig.Copies;
                    string paperWidth = ConstantValuePool.BizSettingConfig.printConfig.PaperWidth;
                    string configPath = @"PrintConfig\InstructionPrintOrderSetting.config";
                    string layoutPath = @"PrintConfig\PrintOrder.ini";
                    if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.DRIVER)
                    {
                        configPath = @"PrintConfig\PrintOrderSetting.config";
                        string printerName = ConstantValuePool.BizSettingConfig.printConfig.Name;
                        DriverOrderPrint printer = new DriverOrderPrint(printerName, paperWidth, "SpecimenLabel");
                        for (int i = 0; i < copies; i++)
                        {
                            printer.DoPrint(printData, layoutPath, configPath);
                        }
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
                                for (int i = 0; i < copies; i++)
                                {
                                    printer.DoPrint(printData, layoutPath, configPath);
                                }
                            }
                        }
                    }
                    if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.ETHERNET)
                    {
                        string IPAddress = ConstantValuePool.BizSettingConfig.printConfig.Name;
                        InstructionOrderPrint printer = new InstructionOrderPrint(IPAddress, 9100, paperWidth);
                        for (int i = 0; i < copies; i++)
                        {
                            printer.DoPrint(printData, layoutPath, configPath);
                        }
                    }
                    if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.USB)
                    {
                        string VID = ConstantValuePool.BizSettingConfig.printConfig.VID;
                        string PID = ConstantValuePool.BizSettingConfig.printConfig.PID;
                        InstructionOrderPrint printer = new InstructionOrderPrint(VID, PID, paperWidth);
                        for (int i = 0; i < copies; i++)
                        {
                            printer.DoPrint(printData, layoutPath, configPath);
                        }
                    }
                }
            }
            return result;
        }

        private void CleanDeliveryOrderButton()
        {
            foreach (CrystalButton btn in btnDeliveryList)
            {
                btn.Tag = null;
                btn.Text = string.Empty;
                btn.BackColor = btn.DisplayColor;
                btn.Enabled = true;
            }
        }

        private void DisplayDeliveryOrderButton(IList<DeliveryOrder> deliveryOrderList)
        {
            if (m_PageIndex <= 0)
            {
                this.btnPgUp.Enabled = false;
                this.btnPgUp.BackColor = ConstantValuePool.DisabledColor;
            }
            else
            {
                this.btnPgUp.Enabled = true;
                this.btnPgUp.BackColor = this.btnPgUp.DisplayColor;
            }
            if (deliveryOrderList.Count > (m_PageCount * (m_PageIndex + 1)))
            {
                this.btnPgDown.Enabled = true;
                this.btnPgDown.BackColor = this.btnPgDown.DisplayColor;
            }
            else
            {
                this.btnPgDown.Enabled = false;
                this.btnPgDown.BackColor = ConstantValuePool.DisabledColor;
            }
            int pageNum = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(deliveryOrderList.Count) / m_PageCount));
            int startIndex = 0;
            int endIndex = 0;
            if ((m_PageIndex + 1) < pageNum)
            {
                startIndex = m_PageIndex * m_PageCount;
                endIndex = (m_PageIndex + 1) * m_PageCount;
            }
            else
            {
                startIndex = m_PageIndex * m_PageCount;
                endIndex = deliveryOrderList.Count;
            }
            for (int i = startIndex, j = 0; i < endIndex; i++, j++)
            {
                btnDeliveryList[j].Tag = deliveryOrderList[i];
                if (deliveryOrderList[i].PayTime == null)
                {
                    btnDeliveryList[j].Text = deliveryOrderList[i].TranSequence.ToString();
                    btnDeliveryList[j].BackColor = Color.Red;
                }
                else
                {
                    btnDeliveryList[j].Text = deliveryOrderList[i].TranSequence + "\r\n " + Convert.ToDateTime(deliveryOrderList[i].PayTime).ToString("MM-dd HH:mm");
                    btnDeliveryList[j].BackColor = Color.Green;
                }
            }
        }
    }
}