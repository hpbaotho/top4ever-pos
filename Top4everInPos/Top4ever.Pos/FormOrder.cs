using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.CustomControl;
using Top4ever.Common;
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
    public partial class FormOrder : Form
    {
        private int m_GoodsGroupPageIndex = 0;
        private int m_GoodsPageIndex = 0;
        private int m_DetailsGroupPageIndex = 0;
        private int m_DetailsPageIndex = 0;
        private GoodsGroup m_CurrentGoodsGroup;
        private DetailsGroup m_CurrentDetailsGroup;
        private IList<Guid> m_CurrentDetailsGroupIDList;
        private string m_DetailsPrefix = string.Empty;
        private bool m_GoodsOrDetails = true;
        private string m_GoodsNum = string.Empty;
        /// <summary>
        /// 当前桌子名称
        /// </summary>
        private string m_CurrentDeskName;
        /// <summary>
        /// 提交的订单信息
        /// </summary>
        private SalesOrder m_SalesOrder;
        /// <summary>
        /// Hide之后重新Show的标志位
        /// </summary>
        private bool m_OnShow = false;
        private int m_Duration = 0; //计时器
        private decimal m_TotalPrice = 0;
        private decimal m_ActualPayMoney = 0;
        private decimal m_Discount = 0;
        private decimal m_CutOff = 0;
        private int m_PersonNum = 0;
        private Guid m_EmployeeID = Guid.Empty;
        private string m_EmployeeNo = string.Empty;
        private CrystalButton prevPressedButton = null;

        #region input
        public int PersonNum
        {
            set { m_PersonNum = value; }
        }

        public string CurrentDeskName
        {
            set { m_CurrentDeskName = value; }
        }

        public SalesOrder PlaceSalesOrder
        {
            set { m_SalesOrder = value; }
        }

        public bool VisibleShow
        {
            set { m_OnShow = value; }
        }
        #endregion

        public FormOrder()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();

            btnPageUp.DisplayColor = btnPageUp.BackColor;
            btnPageDown.DisplayColor = btnPageDown.BackColor;
            btnHead.DisplayColor = btnHead.BackColor;
            btnBack.DisplayColor = btnBack.BackColor;
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
                            int width = this.pnlGroup.Width / control.ColumnsCount;
                            int height = this.pnlGroup.Height / control.RowsCount;
                            int pageSize = control.ColumnsCount * control.RowsCount - 2;    //扣除向前、向后两个按钮
                            //坐标
                            int px = 0, py = 0, times = 0, pageCount = 0;
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
                                    if(item.ButtonStyleID.Equals(btnStyle.ButtonStyleID))
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
                                px += width;
                                if (times == control.ColumnsCount)
                                {
                                    px = 0;
                                    times = 0;
                                    py += height;
                                }
                                if (pageCount == pageSize)
                                {
                                    //new page 初始化
                                    px = 0;
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
                            int width = this.pnlItem.Width / control.ColumnsCount;
                            int height = this.pnlItem.Height / control.RowsCount;
                            int pageSize = control.ColumnsCount * control.RowsCount - 2;    //扣除向前、向后两个按钮
                            //坐标
                            int px = 0, py = 0, times = 0, pageCount = 0;
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
                                        px += width;
                                        if (times == control.ColumnsCount)
                                        {
                                            px = 0;
                                            times = 0;
                                            py += height;
                                        }
                                        if (pageCount == pageSize)
                                        {
                                            //new page 初始化
                                            px = 0;
                                            py = 0;
                                            times = 0;
                                            pageCount = 0;
                                        }
                                    }
                                    dicGoodsButton.Add(goodsGroupID, btnGoods);
                                    //new button 初始化
                                    px = 0;
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
                            int width = this.pnlItem.Width / control.ColumnsCount;
                            int height = this.pnlItem.Height / control.RowsCount;
                            int pageSize = control.ColumnsCount * control.RowsCount - 2;    //扣除向前、向后两个按钮
                            //坐标
                            int px = 0, py = 0, times = 0, pageCount = 0;
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
                                        px += width;
                                        if (times == control.ColumnsCount)
                                        {
                                            px = 0;
                                            times = 0;
                                            py += height;
                                        }
                                        if (pageCount == pageSize)
                                        {
                                            //new page 初始化
                                            px = 0;
                                            py = 0;
                                            times = 0;
                                            pageCount = 0;
                                        }
                                    }
                                    DicDetailsButton.Add(detailGroupID, btnDetails);
                                    //new button 初始化
                                    px = 0;
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
                        width = this.pnlGroup.Width / control.ColumnsCount;
                        height = this.pnlGroup.Height / control.RowsCount;
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
                    btnGoodsGroup.Click -= new System.EventHandler(this.btnGoodsGroup_Click);
                    btnGoodsGroup.Click += new System.EventHandler(this.btnGoodsGroup_Click);
                    this.pnlGroup.Controls.Add(btnGoodsGroup);
                }
                //设置页码按钮的位置
                int px = (columnsCount - 2) * width;
                int py = (rowsCount - 1) * height;
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
                px = (columnsCount - 1) * width;
                py = (rowsCount - 1) * height;
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
                                width = this.pnlItem.Width / control.ColumnsCount;
                                height = this.pnlItem.Height / control.RowsCount;
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
                            this.pnlItem.Controls.Add(btnGoods);
                        }
                        //设置页码按钮的位置
                        int px = (columnsCount - 2) * width;
                        int py = (rowsCount - 1) * height;
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
                        px = (columnsCount - 1) * width;
                        py = (rowsCount - 1) * height;
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
                            width = this.pnlGroup.Width / control.ColumnsCount;
                            height = this.pnlGroup.Height / control.RowsCount;
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
                    int px = 0, py = 0, times = 0;
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
                        px += width;
                        if (times == columnsCount)
                        {
                            px = 0;
                            times = 0;
                            py += height;
                        }
                    }
                    //设置页码按钮的位置
                    px = (columnsCount - 2) * width;
                    py = (rowsCount - 1) * height;
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
                    px = (columnsCount - 1) * width;
                    py = (rowsCount - 1) * height;
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
                                width = this.pnlItem.Width / control.ColumnsCount;
                                height = this.pnlItem.Height / control.RowsCount;
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
                        int px = (columnsCount - 2) * width;
                        int py = (rowsCount - 1) * height;
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
                        px = (columnsCount - 1) * width;
                        py = (rowsCount - 1) * height;
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

        #region goods or detail button event

        private void btnGoodsGroup_Click(object sender, EventArgs e)
        {
            prevPressedButton.BackColor = prevPressedButton.DisplayColor;

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
            prevPressedButton = btnGoodsGroup;

            m_GoodsPageIndex = 0;
            DisplayGoodsButton();
        }

        private void btnGoods_Click(object sender, EventArgs e)
        {
            CrystalButton btnGoods = sender as CrystalButton;
            Goods goods = btnGoods.Tag as Goods;
            int index = dgvGoodsOrder.Rows.Add(new DataGridViewRow());
            dgvGoodsOrder.Rows[index].Cells["ItemID"].Value = goods.GoodsID;
            dgvGoodsOrder.Rows[index].Cells["ItemID"].Tag = goods;
            dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value = 1;
            dgvGoodsOrder.Rows[index].Cells["GoodsName"].Value = goods.GoodsName;
            dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value = goods.SellPrice;
            dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value = 0;
            dgvGoodsOrder.Rows[index].Cells["ItemType"].Value = OrderItemType.Goods;
            dgvGoodsOrder.Rows[index].Cells["CanDiscount"].Value = goods.CanDiscount;
            dgvGoodsOrder.Rows[index].Cells["ItemUnit"].Value = goods.Unit;
            //datagridview滚动条定位
            dgvGoodsOrder.Rows[index].Selected = true;
            dgvGoodsOrder.CurrentCell = dgvGoodsOrder.Rows[index].Cells["GoodsNum"];
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

        #endregion

        private void FormOrder_VisibleChanged(object sender, EventArgs e)
        {
            if (m_OnShow)
            {
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
                m_GoodsNum = string.Empty;
                this.lbTotalPrice.Text = "总金额：";
                this.lbDiscount.Text = "折扣：";
                this.lbNeedPayMoney.Text = "实际应付：";
                this.lbCutOff.Text = "去零：";
                this.btnDeskNo.Text = "桌号：" + m_CurrentDeskName;

                m_Duration = 0;
                this.txtDeviceNo.Text = "设备号：" + ConstantValuePool.BizSettingConfig.DeviceNo;
                this.txtServiceTime.Text = "服务时间：00:00:00";
                timer1.Start();
                this.txtSoftwareProvider.Width += this.Width - 1024;
                this.txtSoftwareProvider.Text = "软件提供商：上海XX科技有限公司";
                this.txtCurrentDateTime.Text = "日期：" + DateTime.Now.ToLongDateString() + " " + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);

                this.dgvGoodsOrder.Rows.Clear();
                if (m_SalesOrder == null)    //new bill
                {
                    m_EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
                    m_EmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                }
                else
                {
                    m_PersonNum = m_SalesOrder.order.PeopleNum;
                    m_EmployeeID = m_SalesOrder.order.EmployeeID;
                    m_EmployeeNo = m_SalesOrder.order.EmployeeNo;
                    BindGoodsOrderInfo();   //绑定订单信息
                    BindOrderInfoSum();
                }
                this.btnEmployee.Text = "服务员：" + m_EmployeeNo;
                this.btnPersonNum.Text = "人数：" + m_PersonNum;
            }
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
            this.lbCutOff.Text = "去零：" + m_CutOff.ToString("f2");
        }

        private void btnShowDetails_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.CurrentRow != null)
            {
                int selectIndex = dgvGoodsOrder.CurrentRow.Index;
                if (dgvGoodsOrder.Rows[selectIndex].Cells["OrderDetailsID"].Value == null)
                {
                    m_DetailsGroupPageIndex = 0;
                    m_DetailsPageIndex = 0;
                    int itemType = Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value);
                    if (itemType == (int)OrderItemType.Goods)
                    {
                        Goods goods = dgvGoodsOrder.Rows[selectIndex].Cells["ItemID"].Tag as Goods;
                        if (goods.DetailsGroupIDList != null && goods.DetailsGroupIDList.Count > 0)
                        {
                            m_GoodsOrDetails = false;    //状态为细项
                            m_CurrentDetailsGroupIDList = goods.DetailsGroupIDList;
                            DisplayDetailGroupButton();
                            this.pnlItem.Controls.Clear();
                            m_DetailsPrefix = "--";
                        }
                    }
                    else if (itemType == (int)OrderItemType.Details)
                    {
                        Details details = dgvGoodsOrder.Rows[selectIndex].Cells["ItemID"].Tag as Details;
                        if (details.DetailsGroupIDList != null && details.DetailsGroupIDList.Count > 0)
                        {
                            m_GoodsOrDetails = false;    //状态为细项
                            m_CurrentDetailsGroupIDList = details.DetailsGroupIDList;
                            DisplayDetailGroupButton();
                            this.pnlItem.Controls.Clear();
                            //detail prefix --
                            string goodsName = dgvGoodsOrder.Rows[selectIndex].Cells["GoodsName"].Value.ToString();
                            if (goodsName.IndexOf('-') >= 0)
                            {
                                int index = goodsName.LastIndexOf('-');
                                m_DetailsPrefix = goodsName.Substring(0, index + 1);
                                m_DetailsPrefix += "--";
                            }
                            else
                            {
                                m_DetailsPrefix = "--";
                            }
                        }
                    }
                }
            }
        }

        private void btnAddNumber_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.CurrentRow != null)
            {
                int selectIndex = dgvGoodsOrder.CurrentRow.Index;
                if (dgvGoodsOrder.Rows[selectIndex].Cells["OrderDetailsID"].Value == null)
                {
                    decimal quantity = 0;
                    CrystalButton btnNumber = sender as CrystalButton;
                    if (string.IsNullOrEmpty(m_GoodsNum))
                    {
                        m_GoodsNum = btnNumber.Text;
                        if (!decimal.TryParse(m_GoodsNum, out quantity))
                        {
                            MessageBox.Show("输入数量格式不正确！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        if (m_GoodsNum.IndexOf('.') > 0)
                        {
                            m_GoodsNum += btnNumber.Text;
                        }
                        else
                        {
                            m_GoodsNum = btnNumber.Text;
                        }
                        if (!decimal.TryParse(m_GoodsNum, out quantity))
                        {
                            MessageBox.Show("输入数量格式不正确！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        //初始化
                        m_GoodsNum = string.Empty;
                    }
                    int itemType = Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value);
                    if (itemType == (int)OrderItemType.Goods)
                    {
                        Goods goods = dgvGoodsOrder.Rows[selectIndex].Cells["ItemID"].Tag as Goods;
                        decimal originalGoodsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                        decimal originalGoodsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsDiscount"].Value);
                        dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value = quantity;
                        dgvGoodsOrder.Rows[selectIndex].Cells["GoodsPrice"].Value = goods.SellPrice * quantity;
                        if (Math.Abs(originalGoodsDiscount) > 0 && originalGoodsNum > 0)
                        {
                            dgvGoodsOrder.Rows[selectIndex].Cells["GoodsDiscount"].Value = originalGoodsDiscount / originalGoodsNum * quantity;
                        }
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
                                    decimal originalDetailsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value);
                                    decimal originalDetailsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value);
                                    decimal currentQty = originalDetailsNum / originalGoodsNum * quantity;
                                    dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value = currentQty;
                                    if (Math.Abs(originalDetailsDiscount) > 0 && originalDetailsNum > 0)
                                    {
                                        dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value = originalDetailsDiscount / originalDetailsNum * currentQty;
                                    }
                                    object item = dgvGoodsOrder.Rows[index].Cells["ItemID"].Tag;
                                    if (item is Goods)
                                    {
                                        Goods subGoods = item as Goods;
                                        dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value = subGoods.SellPrice * currentQty;
                                    }
                                    if (item is Details)
                                    {
                                        Details details = item as Details;
                                        dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value = details.SellPrice * currentQty;
                                    }
                                }
                            }
                        }
                    }
                    if (itemType == (int)OrderItemType.Details)
                    {
                        Details details = dgvGoodsOrder.Rows[selectIndex].Cells["ItemID"].Tag as Details;
                        decimal originalDetailsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                        decimal originalDetailsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsDiscount"].Value);
                        dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value = quantity;
                        dgvGoodsOrder.Rows[selectIndex].Cells["GoodsPrice"].Value = details.SellPrice * quantity;
                        if (Math.Abs(originalDetailsDiscount) > 0 && originalDetailsNum > 0)
                        {
                            dgvGoodsOrder.Rows[selectIndex].Cells["GoodsDiscount"].Value = originalDetailsDiscount / originalDetailsNum * quantity;
                        }
                    }
                    //统计
                    BindOrderInfoSum();
                }
            }
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            int selectIndex = dgvGoodsOrder.CurrentRow.Index;
            if (dgvGoodsOrder.Rows[selectIndex].Cells["OrderDetailsID"].Value == null)
            {
                int itemType = Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value);
                if (itemType == (int)OrderItemType.Goods || itemType == (int)OrderItemType.Details)
                {
                    if (string.IsNullOrEmpty(m_GoodsNum))
                    {
                        m_GoodsNum = "0.";
                    }
                    else
                    {
                        if (m_GoodsNum.IndexOf('.') > 0)
                        {
                            return;
                        }
                        else
                        {
                            m_GoodsNum += ".";
                        }
                    }
                }
            }
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            Feature.FormNumericKeypad keyForm = new Feature.FormNumericKeypad();
            keyForm.DisplayText = "请输入品项数量";
            keyForm.ShowDialog();
            decimal quantity = 0;
            if (!string.IsNullOrEmpty(keyForm.KeypadValue) && decimal.TryParse(keyForm.KeypadValue, out quantity))
            {
                if (dgvGoodsOrder.CurrentRow != null)
                {
                    int selectIndex = dgvGoodsOrder.CurrentRow.Index;
                    if (dgvGoodsOrder.Rows[selectIndex].Cells["OrderDetailsID"].Value == null)
                    {
                        int itemType = Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value);
                        if (itemType == (int)OrderItemType.Goods)
                        {
                            Goods goods = dgvGoodsOrder.Rows[selectIndex].Cells["ItemID"].Tag as Goods;
                            decimal originalGoodsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                            decimal originalGoodsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsDiscount"].Value);
                            dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value = quantity;
                            dgvGoodsOrder.Rows[selectIndex].Cells["GoodsPrice"].Value = goods.SellPrice * quantity;
                            if (Math.Abs(originalGoodsDiscount) > 0 && originalGoodsNum > 0)
                            {
                                dgvGoodsOrder.Rows[selectIndex].Cells["GoodsDiscount"].Value = originalGoodsDiscount / originalGoodsNum * quantity;
                            }
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
                                        decimal originalDetailsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value);
                                        decimal originalDetailsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value);
                                        decimal currentQty = originalDetailsNum / originalGoodsNum * quantity;
                                        dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value = currentQty;
                                        if (Math.Abs(originalDetailsDiscount) > 0 && originalDetailsNum > 0)
                                        {
                                            dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value = originalDetailsDiscount / originalDetailsNum * currentQty;
                                        }
                                        object item = dgvGoodsOrder.Rows[index].Cells["ItemID"].Tag;
                                        if (item is Goods)
                                        {
                                            Goods subGoods = item as Goods;
                                            dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value = subGoods.SellPrice * currentQty;
                                        }
                                        if (item is Details)
                                        {
                                            Details details = item as Details;
                                            dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value = details.SellPrice * currentQty;
                                        }
                                    }
                                }
                            }
                        }
                        if (itemType == (int)OrderItemType.Details)
                        {
                            Details details = dgvGoodsOrder.Rows[selectIndex].Cells["ItemID"].Tag as Details;
                            decimal originalDetailsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                            decimal originalDetailsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsDiscount"].Value);
                            dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value = quantity;
                            dgvGoodsOrder.Rows[selectIndex].Cells["GoodsPrice"].Value = details.SellPrice * quantity;
                            if (Math.Abs(originalDetailsDiscount) > 0 && originalDetailsNum > 0)
                            {
                                dgvGoodsOrder.Rows[selectIndex].Cells["GoodsDiscount"].Value = originalDetailsDiscount / originalDetailsNum * quantity;
                            }
                        }
                        //统计
                        BindOrderInfoSum();
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.CurrentRow != null)
            {
                int selectIndex = dgvGoodsOrder.CurrentRow.Index;
                if (dgvGoodsOrder.Rows[selectIndex].Cells["OrderDetailsID"].Value != null)
                {
                    if (Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value) == (int)OrderItemType.Details)
                    {
                        MessageBox.Show("细项不能单独删除！");
                        return;
                    }
                    Guid orderDetailsID = new Guid(dgvGoodsOrder.Rows[selectIndex].Cells["OrderDetailsID"].Value.ToString());
                    decimal itemNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                    string itemName = dgvGoodsOrder.Rows[selectIndex].Cells["GoodsName"].Value.ToString();
                    FormCancelOrder form = new FormCancelOrder(itemName, itemNum);
                    form.ShowDialog();
                    if (form.DelItemNum > 0 && form.CurrentReason != null)
                    {
                        if (form.DelItemNum < itemNum)
                        {
                            //Key:Index, Value:RemainNum
                            Dictionary<int, decimal> dicRemainNum = new Dictionary<int, decimal>();
                            List<DeletedOrderDetails> deletedOrderDetailsList = new List<DeletedOrderDetails>();
                            //主项
                            decimal remainNum = itemNum - form.DelItemNum;
                            dicRemainNum.Add(selectIndex, remainNum);
                            decimal originalDetailsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                            decimal originalDetailsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsDiscount"].Value);
                            DeletedOrderDetails orderDetails = new DeletedOrderDetails();
                            orderDetails.OrderDetailsID = orderDetailsID;
                            orderDetails.RemainQuantity = remainNum;
                            orderDetails.OffPay = Math.Round(-originalDetailsDiscount / originalDetailsNum * remainNum, 4);
                            orderDetails.AuthorisedManager = ConstantValuePool.CurrentEmployee.EmployeeID;
                            orderDetails.CancelEmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                            orderDetails.CancelReasonName = form.CurrentReason.ReasonName;
                            deletedOrderDetailsList.Add(orderDetails);
                            //细项
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
                                        dicRemainNum.Add(index, remainNum);
                                        orderDetailsID = new Guid(dgvGoodsOrder.Rows[index].Cells["OrderDetailsID"].Value.ToString());
                                        originalDetailsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value);
                                        originalDetailsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value);
                                        DeletedOrderDetails item = new DeletedOrderDetails();
                                        item.OrderDetailsID = orderDetailsID;
                                        item.RemainQuantity = remainNum;
                                        item.OffPay = Math.Round(-originalDetailsDiscount / originalDetailsNum * remainNum, 4);
                                        item.AuthorisedManager = ConstantValuePool.CurrentEmployee.EmployeeID;
                                        item.CancelEmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                                        item.CancelReasonName = form.CurrentReason.ReasonName;
                                        deletedOrderDetailsList.Add(item);
                                    }
                                }
                            }
                            //计算价格信息
                            decimal totalPrice = 0, totalDiscount = 0;
                            for (int i = 0; i < dgvGoodsOrder.Rows.Count; i++)
                            {
                                if (dicRemainNum.ContainsKey(i))
                                {
                                    decimal originalDetailsPrice = Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["GoodsPrice"].Value);
                                    originalDetailsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["GoodsNum"].Value);
                                    originalDetailsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["GoodsDiscount"].Value);
                                    totalPrice += originalDetailsPrice / originalDetailsNum * dicRemainNum[i];
                                    totalDiscount += originalDetailsDiscount / originalDetailsNum * dicRemainNum[i];
                                }
                                else
                                {
                                    totalPrice += Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["GoodsPrice"].Value);
                                    totalDiscount += Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["GoodsDiscount"].Value);
                                }
                            }
                            decimal wholePayMoney = totalPrice + totalDiscount;
                            decimal actualPayMoney = CutOffDecimal.HandleCutOff(wholePayMoney, CutOffType.ROUND_OFF, 0);
                            //构造DeletedSingleOrder对象
                            DeletedSingleOrder deletedSingleOrder = new DeletedSingleOrder();
                            deletedSingleOrder.OrderID = m_SalesOrder.order.OrderID;
                            deletedSingleOrder.TotalSellPrice = totalPrice;
                            deletedSingleOrder.ActualSellPrice = actualPayMoney;
                            deletedSingleOrder.DiscountPrice = totalDiscount;
                            deletedSingleOrder.CutOffPrice = wholePayMoney - actualPayMoney;
                            deletedSingleOrder.deletedOrderDetailsList = deletedOrderDetailsList;

                            DeletedOrderService orderService = new DeletedOrderService();
                            if (orderService.DeleteSingleOrder(deletedSingleOrder))
                            {
                                foreach (KeyValuePair<int, decimal> item in dicRemainNum)
                                {
                                    int index = item.Key;
                                    decimal remainsNum = item.Value;
                                    decimal detailsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value);
                                    decimal detailsPrice = Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value);
                                    decimal detailsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value);
                                    dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value = remainsNum;
                                    dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value = detailsPrice / detailsNum * remainsNum;
                                    dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value = detailsDiscount / detailsNum * remainsNum;
                                }
                            }
                            else
                            {
                                MessageBox.Show("删除品项失败！");
                                return;
                            }
                        }
                        else
                        {
                            List<int> deletedIndexList = new List<int>();
                            List<DeletedOrderDetails> deletedOrderDetailsList = new List<DeletedOrderDetails>();
                            //主项
                            deletedIndexList.Add(selectIndex);
                            DeletedOrderDetails orderDetails = new DeletedOrderDetails();
                            orderDetails.OrderDetailsID = orderDetailsID;
                            orderDetails.RemainQuantity = 0;
                            orderDetails.OffPay = 0;
                            orderDetails.AuthorisedManager = ConstantValuePool.CurrentEmployee.EmployeeID;
                            orderDetails.CancelEmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                            orderDetails.CancelReasonName = form.CurrentReason.ReasonName;
                            deletedOrderDetailsList.Add(orderDetails);
                            //细项
                            if (selectIndex < dgvGoodsOrder.Rows.Count - 1)
                            {
                                for (int index = selectIndex + 1; index < dgvGoodsOrder.Rows.Count; index++)
                                {
                                    if (Convert.ToInt32(dgvGoodsOrder.Rows[index].Cells["ItemType"].Value) == (int)OrderItemType.Details)
                                    {
                                        deletedIndexList.Add(index);
                                        orderDetailsID = new Guid(dgvGoodsOrder.Rows[index].Cells["OrderDetailsID"].Value.ToString());
                                        DeletedOrderDetails item = new DeletedOrderDetails();
                                        item.OrderDetailsID = orderDetailsID;
                                        item.RemainQuantity = 0;
                                        item.OffPay = 0;
                                        item.AuthorisedManager = ConstantValuePool.CurrentEmployee.EmployeeID;
                                        item.CancelEmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                                        item.CancelReasonName = form.CurrentReason.ReasonName;
                                        deletedOrderDetailsList.Add(item);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            //计算价格信息
                            decimal totalPrice = 0, totalDiscount = 0;
                            for (int i = 0; i < dgvGoodsOrder.Rows.Count; i++)
                            {
                                bool hasDeleted = false;
                                foreach (int deletedIndex in deletedIndexList)
                                {
                                    if (i == deletedIndex)
                                    {
                                        hasDeleted = true;
                                        break;
                                    }
                                }
                                if (hasDeleted) continue;
                                totalPrice += Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["GoodsPrice"].Value);
                                totalDiscount += Convert.ToDecimal(dgvGoodsOrder.Rows[i].Cells["GoodsDiscount"].Value);
                            }
                            decimal wholePayMoney = totalPrice + totalDiscount;
                            decimal actualPayMoney = CutOffDecimal.HandleCutOff(wholePayMoney, CutOffType.ROUND_OFF, 0);
                            //构造DeletedSingleOrder对象
                            DeletedSingleOrder deletedSingleOrder = new DeletedSingleOrder();
                            deletedSingleOrder.OrderID = m_SalesOrder.order.OrderID;
                            deletedSingleOrder.TotalSellPrice = totalPrice;
                            deletedSingleOrder.ActualSellPrice = actualPayMoney;
                            deletedSingleOrder.DiscountPrice = totalDiscount;
                            deletedSingleOrder.CutOffPrice = wholePayMoney - actualPayMoney;
                            deletedSingleOrder.deletedOrderDetailsList = deletedOrderDetailsList;

                            DeletedOrderService orderService = new DeletedOrderService();
                            if (orderService.DeleteSingleOrder(deletedSingleOrder))
                            {
                                for(int i = deletedIndexList.Count - 1; i >= 0; i--)
                                {
                                    dgvGoodsOrder.Rows.RemoveAt(deletedIndexList[i]);
                                }
                            }
                            else
                            {
                                MessageBox.Show("删除品项失败！");
                                return;
                            }
                        }
                    }
                }
                else
                {
                    if (Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value) == (int)OrderItemType.Details)
                    {
                        dgvGoodsOrder.Rows.RemoveAt(selectIndex);
                    }
                    else
                    {
                        List<int> deletedIndexList = new List<int>();
                        deletedIndexList.Add(selectIndex);
                        //细项
                        if (selectIndex < dgvGoodsOrder.Rows.Count - 1)
                        {
                            for (int index = selectIndex + 1; index < dgvGoodsOrder.Rows.Count; index++)
                            {
                                if (Convert.ToInt32(dgvGoodsOrder.Rows[index].Cells["ItemType"].Value) == (int)OrderItemType.Details)
                                {
                                    deletedIndexList.Add(index);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        for (int i = deletedIndexList.Count - 1; i >= 0; i--)
                        {
                            dgvGoodsOrder.Rows.RemoveAt(deletedIndexList[i]);
                        }
                    }
                }
                //统计
                BindOrderInfoSum();
            }
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.Rows.Count > 0)
            {
                if (SubmitSalesOrder())
                {
                    //更新桌况为占用状态
                    int status = (int)DeskButtonStatus.OCCUPIED;
                    DeskService deskService = new DeskService();
                    if (deskService.UpdateDeskStatus(m_CurrentDeskName, string.Empty, status))
                    {
                        m_OnShow = false;
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("更新桌况失败！");
                    }
                }
            }
            else
            {
                MessageBox.Show("请先选择菜品！");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (m_SalesOrder == null)
            {
                //更新桌况为空闲状态
                int status = (int)DeskButtonStatus.IDLE_MODE;
                DeskService deskService = new DeskService();
                if(deskService.UpdateDeskStatus(m_CurrentDeskName, string.Empty, status))
                {
                    m_OnShow = false;
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("更新桌况失败！");
                }
            }
            else
            {
                //更新桌况为占用状态
                int status = (int)DeskButtonStatus.OCCUPIED;
                DeskService deskService = new DeskService();
                if (deskService.UpdateDeskStatus(m_CurrentDeskName, string.Empty, status))
                {
                    m_OnShow = false;
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("更新桌况失败！");
                }
            }
        }

        private void btnTurnBack_Click(object sender, EventArgs e)
        {
            if (!m_GoodsOrDetails)
            {
                prevPressedButton.BackColor = prevPressedButton.DisplayColor;
                m_GoodsOrDetails = true;
                LoadDefaultGoodsGroupButton();
            }
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.Rows.Count > 0)
            {
                int selectIndex = dgvGoodsOrder.SelectedRows[0].Index;
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

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.Rows.Count > 0)
            {
                if (SubmitSalesOrder())
                {
                    //转入结账页面
                    SalesOrder newSalesOrder = CopyExtension.Clone<SalesOrder>(m_SalesOrder);
                    FormCheckOut checkForm = new FormCheckOut(newSalesOrder, m_CurrentDeskName);
                    checkForm.ShowDialog();
                    if (checkForm.IsPaidOrder)
                    {
                        m_OnShow = false;
                        this.Hide();
                    }
                }
            }
            else
            {
                MessageBox.Show("请先选择菜品！");
            }
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            if (m_SalesOrder != null)
            {
                FormCancelOrder form = new FormCancelOrder();
                form.ShowDialog();
                if (form.CurrentReason != null)
                {
                    //删除订单
                    DeletedOrder deletedOrder = new DeletedOrder();
                    deletedOrder.OrderID = m_SalesOrder.order.OrderID;
                    deletedOrder.AuthorisedManager = ConstantValuePool.CurrentEmployee.EmployeeID;
                    deletedOrder.CancelEmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                    deletedOrder.CancelReasonName = form.CurrentReason.ReasonName;

                    DeletedOrderService orderService = new DeletedOrderService();
                    if (!orderService.DeleteWholeOrder(deletedOrder))
                    {
                        MessageBox.Show("删除账单失败！");
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            //更新桌况为空闲状态
            int status = (int)DeskButtonStatus.IDLE_MODE;
            DeskService deskService = new DeskService();
            if (deskService.UpdateDeskStatus(m_CurrentDeskName, string.Empty, status))
            {
                m_OnShow = false;
                this.Hide();
            }
            else
            {
                MessageBox.Show("更新桌况状态错误 !");
                return;
            }
        }

        private void btnPrintBill_Click(object sender, EventArgs e)
        {
            bool IsContainsNewItem = false;
            foreach (DataGridViewRow dr in dgvGoodsOrder.Rows)
            {
                if (dr.Cells["OrderDetailsID"].Value == null)
                {
                    IsContainsNewItem = true;
                    break;
                }
            }
            if (IsContainsNewItem)
            {
                MessageBox.Show("存在新单，不能印单！");
            }
            else
            {
                if (ConstantValuePool.BizSettingConfig.printConfig.Enabled)
                {
                    //打印
                    PrintData printData = new PrintData();
                    printData.ShopName = ConstantValuePool.CurrentShop.ShopName;
                    printData.DeskName = m_CurrentDeskName;
                    printData.PersonNum = m_PersonNum.ToString();
                    printData.PrintTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    printData.EmployeeNo = m_EmployeeNo;
                    printData.TranSequence = m_SalesOrder.order.TranSequence.ToString();
                    printData.ShopAddress = ConstantValuePool.CurrentShop.RunAddress;
                    printData.Telephone = ConstantValuePool.CurrentShop.Telephone;
                    printData.GoodsOrderList = new List<GoodsOrder>();
                    foreach (DataGridViewRow dr in dgvGoodsOrder.Rows)
                    {
                        GoodsOrder goodsOrder = new GoodsOrder();
                        goodsOrder.GoodsName = dr.Cells["GoodsName"].Value.ToString();
                        decimal ItemQty = Convert.ToDecimal(dr.Cells["GoodsNum"].Value);
                        decimal totalSellPrice = Convert.ToDecimal(dr.Cells["GoodsPrice"].Value);
                        goodsOrder.GoodsNum = ItemQty.ToString("f1");
                        goodsOrder.SellPrice = (totalSellPrice / ItemQty).ToString("f2");
                        goodsOrder.TotalSellPrice = totalSellPrice.ToString("f2");
                        goodsOrder.TotalDiscount = Convert.ToDecimal(dr.Cells["GoodsDiscount"].Value).ToString("f2");
                        goodsOrder.Unit = dr.Cells["ItemUnit"].Value.ToString();
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
        }

        private void btnPersonNum_Click(object sender, EventArgs e)
        {
            Feature.FormNumericKeypad keyForm = new Feature.FormNumericKeypad();
            keyForm.DisplayText = "请输入就餐人数";
            keyForm.ShowDialog();
            if (!string.IsNullOrEmpty(keyForm.KeypadValue) && keyForm.KeypadValue != "0" && keyForm.KeypadValue.IndexOf('.') == -1)
            {
                m_PersonNum = int.Parse(keyForm.KeypadValue);
                this.btnPersonNum.Text = "人数：" + m_PersonNum;
            }
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
        
        private void btnRestGoods_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.CurrentRow != null)
            {
                int seletedIndex = dgvGoodsOrder.CurrentRow.Index;
                DataGridViewRow dgr = dgvGoodsOrder.Rows[seletedIndex];
                if (dgr.Cells["OrderDetailsID"].Value == null)
                {
                    int itemType = Convert.ToInt32(dgr.Cells["ItemType"].Value);
                    if ((dgr.Cells["Wait"].Value == null || Convert.ToInt32(dgr.Cells["Wait"].Value) == 0) && itemType == (int)OrderItemType.Goods)
                    {
                        Goods goods = dgr.Cells["ItemID"].Tag as Goods;
                        dgr.Cells["GoodsName"].Value = "*" + goods.GoodsName;
                        dgr.Cells["Wait"].Value = 1;    //挂单
                        //细项
                        for (int index = seletedIndex + 1; index < dgvGoodsOrder.Rows.Count; index++)
                        {
                            itemType = Convert.ToInt32(dgvGoodsOrder.Rows[index].Cells["ItemType"].Value);
                            if (itemType == (int)OrderItemType.Goods)
                            {
                                break;
                            }
                            else
                            {
                                string itemName = dgvGoodsOrder.Rows[index].Cells["GoodsName"].Value.ToString();
                                int spIndex = itemName.LastIndexOf('-');
                                string goodsName = itemName.Substring(spIndex + 1);
                                dgvGoodsOrder.Rows[index].Cells["GoodsName"].Value = itemName.Substring(0, spIndex + 1) + "*" + goodsName;
                                dgvGoodsOrder.Rows[index].Cells["Wait"].Value = 1;
                            }
                        }
                    }
                }
            }
            this.exTabControl1.SelectedIndex = 0;
        }

        private void btnLadeGoods_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.CurrentRow != null)
            {
                int seletedIndex = dgvGoodsOrder.CurrentRow.Index;
                DataGridViewRow dgr = dgvGoodsOrder.Rows[seletedIndex];
                if (dgr.Cells["OrderDetailsID"].Value != null)
                {
                    int itemType = Convert.ToInt32(dgr.Cells["ItemType"].Value);
                    if ((dgr.Cells["Wait"].Value != null && Convert.ToInt32(dgr.Cells["Wait"].Value) == 1) && itemType == (int)OrderItemType.Goods)
                    {
                        List<OrderDetails> orderDetailsList = new List<OrderDetails>();
                        OrderDetails orderDetails = new OrderDetails();
                        orderDetails.OrderDetailsID = new Guid(dgr.Cells["OrderDetailsID"].Value.ToString());
                        orderDetails.Wait = 0;
                        orderDetailsList.Add(orderDetails);
                        //细项
                        for (int index = seletedIndex + 1; index < dgvGoodsOrder.Rows.Count; index++)
                        {
                            itemType = Convert.ToInt32(dgvGoodsOrder.Rows[index].Cells["ItemType"].Value);
                            if (itemType == (int)OrderItemType.Goods)
                            {
                                break;
                            }
                            else
                            {
                                orderDetails = new OrderDetails();
                                orderDetails.OrderDetailsID = new Guid(dgvGoodsOrder.Rows[index].Cells["OrderDetailsID"].Value.ToString());
                                orderDetails.Wait = 0;
                                orderDetailsList.Add(orderDetails);
                            }
                        }
                        OrderDetailsService orderDetailsService = new OrderDetailsService();
                        if (orderDetailsService.LadeOrderDetails(orderDetailsList))
                        {
                            //更新GridView
                            for (int i = 0; i < orderDetailsList.Count; i++)
                            {
                                string goodsName = dgvGoodsOrder.Rows[seletedIndex + i].Cells["GoodsName"].Value.ToString();
                                dgvGoodsOrder.Rows[seletedIndex + i].Cells["GoodsName"].Value = goodsName.Replace("*", "");
                                dgvGoodsOrder.Rows[seletedIndex + i].Cells["Wait"].Value = orderDetailsList[i].Wait;
                            }
                        }
                    }
                }
            }
            this.exTabControl1.SelectedIndex = 0;
        }

        private void btnRestOrder_Click(object sender, EventArgs e)
        {
            bool canRestOrder = true;
            foreach (DataGridViewRow dr in dgvGoodsOrder.Rows)
            {
                if (dr.Cells["OrderDetailsID"].Value != null)
                {
                    canRestOrder = false;
                    break;
                }
            }
            if (canRestOrder)
            {
                foreach (DataGridViewRow dgr in dgvGoodsOrder.Rows)
                {
                    int itemType = Convert.ToInt32(dgr.Cells["ItemType"].Value);
                    if ((dgr.Cells["Wait"].Value == null || Convert.ToInt32(dgr.Cells["Wait"].Value) == 0))
                    {
                        string itemName = dgr.Cells["GoodsName"].Value.ToString();
                        if (itemType == (int)OrderItemType.Goods)
                        {
                            dgr.Cells["GoodsName"].Value = "*" + itemName;
                            dgr.Cells["Wait"].Value = 1;    //挂单
                        }
                        else
                        {
                            int spIndex = itemName.LastIndexOf('-');
                            string goodsName = itemName.Substring(spIndex + 1);
                            dgr.Cells["GoodsName"].Value = itemName.Substring(0, spIndex + 1) + "*" + goodsName;
                            dgr.Cells["Wait"].Value = 1;    //挂单
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("不能整单挂单");
            }
            this.exTabControl1.SelectedIndex = 0;
        }

        private void btnLadeOrder_Click(object sender, EventArgs e)
        {
            bool canLadeOrder = true;
            foreach (DataGridViewRow dr in dgvGoodsOrder.Rows)
            {
                if (dr.Cells["OrderDetailsID"].Value == null)
                {
                    canLadeOrder = false;
                    break;
                }
            }
            if (canLadeOrder)
            {
                List<OrderDetails> orderDetailsList = new List<OrderDetails>();
                foreach (DataGridViewRow dgr in dgvGoodsOrder.Rows)
                {
                    if (dgr.Cells["Wait"].Value != null && Convert.ToInt32(dgr.Cells["Wait"].Value) == 1)
                    {
                        OrderDetails orderDetails = new OrderDetails();
                        orderDetails.OrderDetailsID = new Guid(dgr.Cells["OrderDetailsID"].Value.ToString());
                        orderDetails.Wait = 0;
                        orderDetailsList.Add(orderDetails);
                    }
                }
                if (orderDetailsList.Count > 0)
                {
                    OrderDetailsService orderDetailsService = new OrderDetailsService();
                    if (orderDetailsService.LadeOrderDetails(orderDetailsList))
                    {
                        //更新GridView
                        foreach (DataGridViewRow dgr in dgvGoodsOrder.Rows)
                        {
                            if (dgr.Cells["Wait"].Value != null && Convert.ToInt32(dgr.Cells["Wait"].Value) == 1)
                            {
                                string goodsName = dgr.Cells["GoodsName"].Value.ToString();
                                dgr.Cells["GoodsName"].Value = goodsName.Replace("*", "");
                                dgr.Cells["Wait"].Value = 0;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("没有需要提单的品项！");
                }
            }
            else
            {
                MessageBox.Show("有新单，不能整单提单！");
            }
            this.exTabControl1.SelectedIndex = 0;
        }

        private void btnSplitBill_Click(object sender, EventArgs e)
        {
            FormSplitBill form = new FormSplitBill(m_SalesOrder);
            form.ShowDialog();
            if (form.SplitOrderSuccess)
            {
                //重新加载
                SalesOrderService salesOrderService = new SalesOrderService();
                m_SalesOrder = salesOrderService.GetSalesOrder(m_SalesOrder.order.OrderID);
                BindGoodsOrderInfo();   //绑定订单信息
                BindOrderInfoSum();
            }
            this.exTabControl1.SelectedIndex = 0;
        }

        private void btnReform_Click(object sender, EventArgs e)
        {
            this.exTabControl1.SelectedIndex = 0;
        }

        private void btnPriceCode_Click(object sender, EventArgs e)
        {
            this.exTabControl1.SelectedIndex = 0;
        }

        private void btnReminder_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.CurrentRow != null)
            {
                int seletedIndex = dgvGoodsOrder.CurrentRow.Index;
                DataGridViewRow dgr = dgvGoodsOrder.Rows[seletedIndex];
                if (dgr.Cells["OrderDetailsID"].Value != null)
                {
                    int itemType = Convert.ToInt32(dgr.Cells["ItemType"].Value);
                    if ((dgr.Cells["Wait"].Value == null || Convert.ToInt32(dgr.Cells["Wait"].Value) == 0) && itemType == (int)OrderItemType.Goods)
                    {
                        FormReminder form = new FormReminder();
                        form.ShowDialog();
                        if (form.CurrentReason != null)
                        { 
                            //催单
                            IList<Guid> orderDetailsIdList = new List<Guid>();
                            Guid orderDetailsID = new Guid(dgr.Cells["OrderDetailsID"].Value.ToString());
                            orderDetailsIdList.Add(orderDetailsID);
                            if (seletedIndex < dgvGoodsOrder.Rows.Count - 1)
                            {
                                for (int index = seletedIndex + 1; index < dgvGoodsOrder.Rows.Count; index++)
                                {
                                    if (Convert.ToInt32(dgvGoodsOrder.Rows[index].Cells["ItemType"].Value) == (int)OrderItemType.Goods)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        orderDetailsID = new Guid(dgvGoodsOrder.Rows[index].Cells["OrderDetailsID"].Value.ToString());
                                        orderDetailsIdList.Add(orderDetailsID);
                                    }
                                }
                            }
                            ReminderOrder reminder = new ReminderOrder();
                            reminder.OrderID = m_SalesOrder.order.OrderID;
                            reminder.OrderDetailsIDList = orderDetailsIdList;
                            reminder.ReasonName = form.CurrentReason.ReasonName;
                            reminder.EmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                            ReminderService reminderService = new ReminderService();
                            bool result = reminderService.CreateReminderOrder(reminder);
                            if (!result)
                            {
                                MessageBox.Show("催单失败，请重新操作！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                }
            }
            this.exTabControl1.SelectedIndex = 0;
        }

        private void btnHandover_Click(object sender, EventArgs e)
        {
            HandoverInfo handover = new HandoverInfo();
            HandoverService handoverService = new HandoverService();
            bool result = handoverService.CreateHandover(handover);
            if (result)
            {
                MessageBox.Show("交班成功！");
            }
            else
            {
                MessageBox.Show("出现异常错误，请重新交班！");
            }
        }

        private void btnDailyStatement_Click(object sender, EventArgs e)
        {
            Feature.FormSalesReport formReport = new FormSalesReport();
            formReport.ShowDialog();
            //DailyBalance dailyBalance = new DailyBalance();
            //DailyBalanceService dailyBalanceService = new DailyBalanceService();
            //int result = dailyBalanceService.CreateDailyBalance(dailyBalance);
            //if (result == 1)
            //{
            //    MessageBox.Show("日结成功！");
            //}
            //else if (result == 2)
            //{
            //    MessageBox.Show("存在未结账单据，请先结完账！");
            //}
            //else if (result == 3)
            //{
            //    MessageBox.Show("存在未交班的POS，请先交班！");
            //}
            //else
            //{
            //    MessageBox.Show("出现异常错误，请重新日结！");
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            m_Duration++;
            int hour = m_Duration / 60 / 60;
            int minute = (m_Duration - hour * 60 * 60) / 60;
            int second = m_Duration - hour * 60 * 60 - minute * 60;
            this.txtServiceTime.Text = "服务时间：" + formateTime(hour) + ":" + formateTime(minute) + ":" + formateTime(second);
        }

        #region private method

        private bool SubmitSalesOrder()
        {
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
                        OrderDiscount orderDiscount = new OrderDiscount();
                        orderDiscount.OrderDiscountID = Guid.NewGuid();
                        orderDiscount.OrderID = orderID;
                        orderDiscount.OrderDetailsID = orderDetailsID;
                        orderDiscount.DiscountID = discount.DiscountID;
                        orderDiscount.DiscountName = discount.DiscountName;
                        orderDiscount.DiscountType = discount.DiscountType;
                        orderDiscount.DiscountRate = discount.DiscountRate;
                        orderDiscount.OffFixPay = discount.OffFixPay;
                        orderDiscount.OffPay = Math.Abs(Convert.ToDecimal(dr.Cells["GoodsDiscount"].Value));
                        orderDiscount.EmployeeID = m_EmployeeID;
                        newOrderDiscountList.Add(orderDiscount);
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
                        OrderDiscount orderDiscount = new OrderDiscount();
                        orderDiscount.OrderDiscountID = Guid.NewGuid();
                        orderDiscount.OrderID = orderID;
                        orderDiscount.OrderDetailsID = orderDetailsID;
                        orderDiscount.DiscountID = discount.DiscountID;
                        orderDiscount.DiscountName = discount.DiscountName;
                        orderDiscount.DiscountType = discount.DiscountType;
                        orderDiscount.DiscountRate = discount.DiscountRate;
                        orderDiscount.OffFixPay = discount.OffFixPay;
                        orderDiscount.OffPay = Math.Abs(Convert.ToDecimal(dr.Cells["GoodsDiscount"].Value));
                        orderDiscount.EmployeeID = m_EmployeeID;
                        newOrderDiscountList.Add(orderDiscount);
                    }
                }
            }
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
                order.DeskName = m_CurrentDeskName;
                order.EatType = (int)EatWayType.DineIn;
                order.Status = 0;
                order.PeopleNum = m_PersonNum;
                order.EmployeeID = m_EmployeeID;
                order.EmployeeNo = m_EmployeeNo;
                order.OrderLastTime = m_Duration;

                SalesOrder salesOrder = new SalesOrder();
                salesOrder.order = order;
                salesOrder.orderDetailsList = newOrderDetailsList;
                salesOrder.orderDiscountList = newOrderDiscountList;
                SalesOrderService orderService = new SalesOrderService();
                bool result = orderService.CreateSalesOrder(salesOrder);
                if (result)
                {
                    //重新加载
                    SalesOrderService salesOrderService = new SalesOrderService();
                    m_SalesOrder = salesOrderService.GetSalesOrder(orderID);
                    BindGoodsOrderInfo();   //绑定订单信息
                    BindOrderInfoSum();
                }
                else
                {
                    MessageBox.Show("结账提交失败！");
                    return false;
                }
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
                    order.DeskName = m_CurrentDeskName;
                    order.PeopleNum = m_PersonNum;
                    order.EmployeeID = m_EmployeeID;
                    order.EmployeeNo = m_EmployeeNo;
                    order.OrderLastTime = m_Duration;

                    SalesOrder salesOrder = new SalesOrder();
                    salesOrder.order = order;
                    salesOrder.orderDetailsList = newOrderDetailsList;
                    salesOrder.orderDiscountList = newOrderDiscountList;
                    SalesOrderService orderService = new SalesOrderService();
                    bool result = orderService.UpdateSalesOrder(salesOrder);
                    if (result)
                    {
                        //重新加载
                        SalesOrderService salesOrderService = new SalesOrderService();
                        m_SalesOrder = salesOrderService.GetSalesOrder(orderID);
                        BindGoodsOrderInfo();   //绑定订单信息
                        BindOrderInfoSum();
                    }
                    else
                    {
                        MessageBox.Show("结账提交失败！");
                        return false;
                    }
                }
            }
            if (newOrderDetailsList.Count > 0)
            {
                if (ConstantValuePool.BizSettingConfig.printConfig.Enabled)
                {
                    //打印
                    PrintData printData = new PrintData();
                    printData.ShopName = ConstantValuePool.CurrentShop.ShopName;
                    printData.DeskName = m_CurrentDeskName;
                    printData.PersonNum = m_PersonNum.ToString();
                    printData.PrintTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    printData.EmployeeNo = m_EmployeeNo;
                    printData.TranSequence = m_SalesOrder.order.TranSequence.ToString();
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
            return true;
        }

        private void LoadDefaultGoodsGroupButton()
        {
            m_GoodsGroupPageIndex = 0;
            m_GoodsPageIndex = 0;
            DisplayGoodsGroupButton();
            m_CurrentGoodsGroup = ConstantValuePool.GoodsGroupList[0];
            // begin 设置颜色
            CrystalButton btnGoodsGroup = ConstantValuePool.GoodsGroupButton[0];
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
            prevPressedButton = btnGoodsGroup;
            // end
            DisplayGoodsButton();
        }

        private string formateTime(int time)
        {
            string result = string.Empty;
            if (time.ToString().Length == 1)
            {
                result = "0" + time.ToString();
            }
            else
            {
                result = time.ToString();
            }
            return result;
        }

        #endregion
    }
}
