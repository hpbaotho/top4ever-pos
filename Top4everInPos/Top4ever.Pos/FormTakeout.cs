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
using Top4ever.Domain.GoodsRelated;
using Top4ever.Entity;
using Top4ever.Entity.Enum;

namespace Top4ever.Pos
{
    public partial class FormTakeout : Form
    {
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
        private decimal m_TotalPrice = 0;
        private decimal m_ActualPayMoney = 0;
        private decimal m_Discount = 0;
        private decimal m_CutOff = 0;
        private CrystalButton prevPressedButton = null;
        private bool m_ShowSilverCode = false;

        public FormTakeout()
        {
            InitializeComponent();
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
            int buttonNum = 8;
            int width = (this.pnlTools.Width - (buttonNum - 1) * space) / buttonNum;
            int offsetX = 0, offsetY = 0;
            btnFuncPanel.Width = width;
            offsetX += width + space;
            btnTakeOut.Width = width;
            btnTakeOut.Location = new Point(offsetX, offsetY);
            offsetX += width + space;
            btnDiscount.Width = width;
            btnDiscount.Location = new Point(offsetX, offsetY);
            offsetX += width + space;
            btnWholeDiscount.Width = width;
            btnWholeDiscount.Location = new Point(offsetX, offsetY);
            offsetX += width + space;
            btnOutsideOrder.Width = width;
            btnOutsideOrder.Location = new Point(offsetX, offsetY);
            offsetX += width + space;
            btnDelivery.Width = width;
            btnDelivery.Location = new Point(offsetX, offsetY);
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
            btnPageUp.DisplayColor = btnPageUp.BackColor;
            btnPageDown.DisplayColor = btnPageDown.BackColor;
            btnHead.DisplayColor = btnHead.BackColor;
            btnBack.DisplayColor = btnBack.BackColor;
            btnPgUp.DisplayColor = btnPgUp.BackColor;
            btnPgDown.DisplayColor = btnPgDown.BackColor;
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
                int buttonNum = 10, space = 2;
                int px = 0, py = space;
                int height = (this.pnlDelivery.Height - this.pnlPage.Height - (buttonNum + 1) * space) / buttonNum;
                for (int i = 0; i < buttonNum; i++)
                {
                    CrystalButton btnDelivery = new CrystalButton();
                    btnDelivery.Name = "btnDelivery" + i;
                    btnDelivery.BackColor = Color.DodgerBlue;
                    btnDelivery.Font = new Font("微软雅黑", 9.75F);
                    btnDelivery.ForeColor = Color.White;
                    btnDelivery.Location = new Point(px, py);
                    btnDelivery.Size = new Size(pnlDelivery.Width - space, height);
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPriceCode_Click(object sender, EventArgs e)
        {
            m_ShowSilverCode = !m_ShowSilverCode;
            DisplayGoodsButton();
        }
    }
}