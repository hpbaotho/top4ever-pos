using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.CustomControl;
using Top4ever.Common;
using Top4ever.Domain;
using Top4ever.Domain.GoodsRelated;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Promotions;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;
using Top4ever.Entity.Enum;
using VechsoftPos.Feature;
using VechsoftPos.Promotions;
using Top4ever.Print;
using Top4ever.Print.Entity;

namespace VechsoftPos
{
    public partial class FormOrder : Form
    {
        private const int BlankSpace = 2;
        private readonly List<CrystalButton> _btnGroupList = new List<CrystalButton>();
        private readonly List<CrystalButton> _btnItemList = new List<CrystalButton>();
        //品项组列表
        private int _groupPageSize;
        private int _groupPageIndex;
        //品项列表
        private int _itemPageSize;
        private int _itemPageIndex;
        private GoodsGroup _currentGoodsGroup;
        private DetailsGroup _currentDetailsGroup;
        private IList<Guid> _currentDetailsGroupIdList;
        private string _detailsPrefix = string.Empty;
        private bool _goodsOrDetails = true;
        private string _goodsNum = string.Empty;
        /// <summary>
        /// 当前桌子名称
        /// </summary>
        private string _currentDeskName;
        /// <summary>
        /// 提交的订单信息
        /// </summary>
        private SalesOrder _salesOrder;
        /// <summary>
        /// Hide之后重新Show的标志位
        /// </summary>
        private bool _onShow;
        private decimal _totalPrice;
        private decimal _actualPayMoney;
        private decimal _discount;
        private decimal _cutOff;
        private int _personNum;
        private Guid _employeeId = Guid.Empty;
        private string _employeeNo = string.Empty;
        private CrystalButton _prevPressedButton;
        private bool _showSilverCode;

        #region input
        public int PersonNum
        {
            set { _personNum = value; }
        }

        public string CurrentDeskName
        {
            set { _currentDeskName = value; }
        }

        public SalesOrder PlaceSalesOrder
        {
            set { _salesOrder = value; }
        }

        public bool VisibleShow
        {
            set { _onShow = value; }
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

        private void FormOrder_Load(object sender, EventArgs e)
        {
            if (!RightsItemCode.FindRights(RightsItemCode.SPLITBILL))
            {
                btnSplitBill.Enabled = false;
                btnSplitBill.BackColor = ConstantValuePool.DisabledColor;
            }
            if (!RightsItemCode.FindRights(RightsItemCode.REFORM))
            {
                btnTasteRemark.Enabled = false;
                btnTasteRemark.BackColor = ConstantValuePool.DisabledColor;
            }
            if (!RightsItemCode.FindRights(RightsItemCode.PLACEORDER))
            {
                btnPlaceOrder.Enabled = false;
                btnPlaceOrder.BackColor = ConstantValuePool.DisabledColor;
            }
            ResizeSearchPad();
        }

        private void FormOrder_VisibleChanged(object sender, EventArgs e)
        {
            if (_onShow)
            {
                //初始化品项组按钮
                InitializeGroupButton();
                //初始化品项按钮
                InitializeItemButton();
                //初始化
                LoadDefaultGoodsGroupButton();
                _goodsOrDetails = true;
                _goodsNum = string.Empty;
                _detailsPrefix = string.Empty;
                _prevPressedButton = null;
                _showSilverCode = false;
                this.lbTotalPrice.Text = "总金额：";
                this.lbDiscount.Text = "折扣：";
                this.lbNeedPayMoney.Text = "实际应付：";
                this.lbCutOff.Text = "去零：";
                this.btnDeskNo.Text = "桌号：" + _currentDeskName;

                this.txtShopNo.Text = "店铺号：" + ConstantValuePool.CurrentShop.ShopNo;
                this.txtDeviceNo.Text = "设备号：" + ConstantValuePool.BizSettingConfig.DeviceNo;
                this.txtSoftwareProvider.Width += this.Width - 1024;
                this.txtSoftwareProvider.Text = "软件提供商：凡趣科技有限公司";
                this.txtCurrentDateTime.Text = "日期：" + DateTime.Now.ToLongDateString() + " " + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);

                this.dgvGoodsOrder.Rows.Clear();
                if (_salesOrder == null)    //new bill
                {
                    _employeeId = ConstantValuePool.CurrentEmployee.EmployeeID;
                    _employeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                }
                else
                {
                    _personNum = _salesOrder.order.PeopleNum;
                    _employeeId = _salesOrder.order.EmployeeID;
                    _employeeNo = _salesOrder.order.EmployeeNo;
                    BindGoodsOrderInfo();   //绑定订单信息
                    BindOrderInfoSum();
                    //更新第二屏信息
                    if (Screen.AllScreens.Length > 1 && ConstantValuePool.BizSettingConfig.SecondScreenEnabled)
                    {
                        if (ConstantValuePool.SecondScreenForm != null && ConstantValuePool.SecondScreenForm is FormSecondScreen)
                        {
                            ((FormSecondScreen)ConstantValuePool.SecondScreenForm).BindGoodsOrderInfo(dgvGoodsOrder);
                        }
                    }
                }
                this.btnEmployee.Text = "服务员：" + _employeeNo;
                this.btnPersonNum.Text = "人数：" + _personNum;
            }
        }

        #region 初始化

        private void InitializeGroupButton()
        {
            if (_btnGroupList.Count == 0)
            {
                if (ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls != null && ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls.Count > 0)
                {
                    foreach (BizControl control in ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls)
                    {
                        if (control.Name == "Group")
                        {
                            int width = (this.pnlGroup.Width - BlankSpace * (control.ColumnsCount + 1)) / control.ColumnsCount;
                            int height = (this.pnlGroup.Height - BlankSpace * (control.RowsCount + 1)) / control.RowsCount;
                            _groupPageSize = control.ColumnsCount * control.RowsCount - 2;    //扣除向前、向后两个按钮
                            //坐标
                            int px = BlankSpace, py = BlankSpace, times = 0;
                            for (int i = 0; i < _groupPageSize; i++)
                            {
                                CrystalButton btnGroup = new CrystalButton();
                                btnGroup.Name = "btnGroup" + i;
                                btnGroup.BackColor = btnGroup.DisplayColor = Color.DodgerBlue;
                                btnGroup.Location = new Point(px, py);
                                btnGroup.Size = new Size(width, height);
                                btnGroup.Click += new System.EventHandler(this.btnGroup_Click);

                                this.pnlGroup.Controls.Add(btnGroup);
                                _btnGroupList.Add(btnGroup);
                                //计算Button位置
                                times++;
                                px += BlankSpace + width;
                                if (times == control.ColumnsCount)
                                {
                                    px = BlankSpace;
                                    times = 0;
                                    py += BlankSpace + height;
                                }
                            }
                            px = (control.ColumnsCount - 2) * width + (control.ColumnsCount - 2 + 1) * BlankSpace;
                            py = (control.RowsCount - 1) * height + control.RowsCount * BlankSpace;
                            btnPageUp.Width = width;
                            btnPageUp.Height = height;
                            btnPageUp.Location = new Point(px, py);
                            px += width + BlankSpace;
                            btnPageDown.Width = width;
                            btnPageDown.Height = height;
                            btnPageDown.Location = new Point(px, py);
                            //向前
                            this.pnlGroup.Controls.Add(btnPageUp);
                            //向后
                            this.pnlGroup.Controls.Add(btnPageDown);
                            break;
                        }
                    }
                }
            }
        }

        private void InitializeItemButton()
        {
            if (_btnItemList.Count == 0)
            {
                if (ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls != null && ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls.Count > 0)
                {
                    foreach (BizControl control in ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls)
                    {
                        if (control.Name == "Item")
                        {
                            int width = (this.pnlItem.Width - BlankSpace * (control.ColumnsCount + 1)) / control.ColumnsCount;
                            int height = (this.pnlItem.Height - BlankSpace * (control.RowsCount + 1)) / control.RowsCount;
                            _itemPageSize = control.ColumnsCount * control.RowsCount - 2;    //扣除向前、向后两个按钮
                            //坐标
                            int px = BlankSpace, py = BlankSpace, times = 0;
                            for (int i = 0; i < _itemPageSize; i++)
                            {
                                CrystalButton btnItem = new CrystalButton();
                                btnItem.Name = "btnItem" + i;
                                btnItem.BackColor = btnItem.DisplayColor = Color.DodgerBlue;
                                btnItem.Location = new Point(px, py);
                                btnItem.Size = new Size(width, height);
                                btnItem.Click += new System.EventHandler(this.btnItem_Click);

                                this.pnlItem.Controls.Add(btnItem);
                                _btnItemList.Add(btnItem);
                                //计算Button位置
                                times++;
                                px += BlankSpace + width;
                                if (times == control.ColumnsCount)
                                {
                                    px = BlankSpace;
                                    times = 0;
                                    py += BlankSpace + height;
                                }
                            }
                            px = (control.ColumnsCount - 2) * width + (control.ColumnsCount - 2 + 1) * BlankSpace;
                            py = (control.RowsCount - 1) * height + control.RowsCount * BlankSpace;
                            btnHead.Width = width;
                            btnHead.Height = height;
                            btnHead.Location = new Point(px, py);
                            px += width + BlankSpace;
                            btnBack.Width = width;
                            btnBack.Height = height;
                            btnBack.Location = new Point(px, py);
                            //向前
                            this.pnlItem.Controls.Add(btnHead);
                            //向后
                            this.pnlItem.Controls.Add(btnBack);
                            break;
                        }
                    }
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

            int unDisplayNum = 0;
            int startIndex = _groupPageIndex * _groupPageSize;
            int endIndex = (_groupPageIndex + 1) * _groupPageSize;
            if (endIndex > ConstantValuePool.GoodsGroupList.Count)
            {
                unDisplayNum = endIndex - ConstantValuePool.GoodsGroupList.Count;
                endIndex = ConstantValuePool.GoodsGroupList.Count;
            }
            //隐藏没有内容的按钮
            for (int i = _btnGroupList.Count - unDisplayNum; i < _btnGroupList.Count; i++)
            {
                _btnGroupList[i].Visible = false;
            }
            //显示有内容的按钮
            for (int i = 0, j = startIndex; j < endIndex; i++, j++)
            {
                GoodsGroup goodsGroup = ConstantValuePool.GoodsGroupList[j];
                CrystalButton btn = _btnGroupList[i];
                btn.Visible = true;
                btn.Text = goodsGroup.GoodsGroupName;
                btn.Tag = goodsGroup;
                btn.Enabled = IsItemButtonEnabled(goodsGroup.GoodsGroupID, ItemsType.GoodsGroup);
                foreach (ButtonStyle btnStyle in ConstantValuePool.ButtonStyleList)
                {
                    if (goodsGroup.ButtonStyleID.Equals(btnStyle.ButtonStyleID))
                    {
                        float emSize = (float)btnStyle.FontSize;
                        FontStyle style = FontStyle.Regular;
                        btn.Font = new Font(btnStyle.FontName, emSize, style);
                        btn.ForeColor = ColorConvert.RGB(btnStyle.ForeColor);
                        btn.BackColor = btn.DisplayColor = ColorConvert.RGB(btnStyle.BackColor);
                        break;
                    }
                }
            }
            //设置页码按钮的显示
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
            if (endIndex >= ConstantValuePool.GoodsGroupList.Count)
            {
                btnPageDown.Enabled = false;
                btnPageDown.BackColor = ConstantValuePool.DisabledColor;
            }
            else
            {
                btnPageDown.Enabled = true;
                btnPageDown.BackColor = btnPageDown.DisplayColor;
            }

            this.pnlGroup.ResumeLayout(false);
            this.pnlGroup.PerformLayout();
            this.ResumeLayout(false);
        }

        private void DisplayGoodsButton()
        {
            if (_currentGoodsGroup != null)
            {
                if (_currentGoodsGroup.GoodsList == null || _currentGoodsGroup.GoodsList.Count == 0)
                {
                    HideItemButton();
                }
                else
                {
                    //禁止引发Layout事件
                    this.pnlItem.SuspendLayout();
                    this.SuspendLayout();

                    //显示控件
                    int unDisplayNum = 0;
                    int startIndex = _itemPageIndex * _itemPageSize;
                    int endIndex = (_itemPageIndex + 1) * _itemPageSize;
                    if (endIndex > _currentGoodsGroup.GoodsList.Count)
                    {
                        unDisplayNum = endIndex - _currentGoodsGroup.GoodsList.Count;
                        endIndex = _currentGoodsGroup.GoodsList.Count;
                    }
                    //隐藏没有内容的按钮
                    for (int i = _btnItemList.Count - unDisplayNum; i < _btnItemList.Count; i++)
                    {
                        _btnItemList[i].Visible = false;
                    }
                    //显示有内容的按钮
                    for (int i = 0, j = startIndex; j < endIndex; i++, j++)
                    {
                        Goods goods = _currentGoodsGroup.GoodsList[j];
                        CrystalButton btn = _btnItemList[i];
                        btn.Visible = true;
                        if (_showSilverCode)
                        {
                            btn.Text = goods.GoodsName + "\r\n ￥" + goods.SellPrice.ToString("f2");
                        }
                        else
                        {
                            btn.Text = goods.GoodsName;
                        }
                        btn.Tag = goods;
                        btn.Enabled = IsItemButtonEnabled(goods.GoodsID, ItemsType.Goods);
                        foreach (ButtonStyle btnStyle in ConstantValuePool.ButtonStyleList)
                        {
                            if (goods.ButtonStyleID.Equals(btnStyle.ButtonStyleID))
                            {
                                float emSize = (float)btnStyle.FontSize;
                                FontStyle style = FontStyle.Regular;
                                btn.Font = new Font(btnStyle.FontName, emSize, style);
                                btn.ForeColor = ColorConvert.RGB(btnStyle.ForeColor);
                                btn.BackColor = btn.DisplayColor = ColorConvert.RGB(btnStyle.BackColor);
                                break;
                            }
                        }
                    }
                    //设置页码按钮的显示
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
                    if (endIndex >= _currentGoodsGroup.GoodsList.Count)
                    {
                        btnBack.Enabled = false;
                        btnBack.BackColor = ConstantValuePool.DisabledColor;
                    }
                    else
                    {
                        btnBack.Enabled = true;
                        btnBack.BackColor = btnBack.DisplayColor;
                    }

                    this.pnlItem.ResumeLayout(false);
                    this.pnlItem.PerformLayout();
                    this.ResumeLayout(false);
                }
            }
        }

        private void DisplayDetailGroupButton()
        {
            List<DetailsGroup> detailGroupList = new List<DetailsGroup>();
            foreach (DetailsGroup item in ConstantValuePool.DetailsGroupList)
            {
                if (item.IsCommon || _currentDetailsGroupIdList.Contains(item.DetailsGroupID))
                {
                    detailGroupList.Add(item);
                }
            }
            if (detailGroupList.Count > 0)
            {
                //禁止引发Layout事件
                this.pnlGroup.SuspendLayout();
                this.SuspendLayout();

                int unDisplayNum = 0;
                int startIndex = _groupPageIndex * _groupPageSize;
                int endIndex = (_groupPageIndex + 1) * _groupPageSize;
                if (endIndex > detailGroupList.Count)
                {
                    unDisplayNum = endIndex - detailGroupList.Count;
                    endIndex = detailGroupList.Count;
                }
                //隐藏没有内容的按钮
                for (int i = _btnGroupList.Count - unDisplayNum; i < _btnGroupList.Count; i++)
                {
                    _btnGroupList[i].Visible = false;
                }
                //显示有内容的按钮
                for (int i = 0, j = startIndex; j < endIndex; i++, j++)
                {
                    DetailsGroup detailsGroup = detailGroupList[j];
                    CrystalButton btn = _btnGroupList[i];
                    btn.Visible = true;
                    btn.Text = detailsGroup.DetailsGroupName;
                    btn.Tag = detailsGroup;
                    btn.BackColor = btn.DisplayColor = Color.DodgerBlue;
                    foreach (ButtonStyle btnStyle in ConstantValuePool.ButtonStyleList)
                    {
                        if (detailsGroup.ButtonStyleID.Equals(btnStyle.ButtonStyleID))
                        {
                            float emSize = (float)btnStyle.FontSize;
                            FontStyle style = FontStyle.Regular;
                            btn.Font = new Font(btnStyle.FontName, emSize, style);
                            btn.ForeColor = ColorConvert.RGB(btnStyle.ForeColor);
                            btn.BackColor = btn.DisplayColor = ColorConvert.RGB(btnStyle.BackColor);
                            break;
                        }
                    }
                }
                //设置页码按钮的显示
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

                this.pnlGroup.ResumeLayout(false);
                this.pnlGroup.PerformLayout();
                this.ResumeLayout(false);
            }
        }

        private void DisplayDetailButton()
        {
            if (_currentDetailsGroup != null)
            {
                if (_currentDetailsGroup.DetailsList == null || _currentDetailsGroup.DetailsList.Count == 0)
                {
                    HideItemButton();
                }
                else
                {
                    //禁止引发Layout事件
                    this.pnlItem.SuspendLayout();
                    this.SuspendLayout();

                    //显示控件
                    int unDisplayNum = 0;
                    int startIndex = _itemPageIndex * _itemPageSize;
                    int endIndex = (_itemPageIndex + 1) * _itemPageSize;
                    if (endIndex > _currentDetailsGroup.DetailsList.Count)
                    {
                        unDisplayNum = endIndex - _currentDetailsGroup.DetailsList.Count;
                        endIndex = _currentDetailsGroup.DetailsList.Count;
                    }
                    //隐藏没有内容的按钮
                    for (int i = _btnItemList.Count - unDisplayNum; i < _btnItemList.Count; i++)
                    {
                        _btnItemList[i].Visible = false;
                    }
                    //显示有内容的按钮
                    for (int i = 0, j = startIndex; j < endIndex; i++, j++)
                    {
                        Details details = _currentDetailsGroup.DetailsList[j];
                        CrystalButton btn = _btnItemList[i];
                        btn.Visible = true;
                        if (_showSilverCode)
                        {
                            btn.Text = details.DetailsName + "\r\n ￥" + details.SellPrice.ToString("f2");
                        }
                        else
                        {
                            btn.Text = details.DetailsName;
                        }
                        btn.Tag = details;
                        foreach (ButtonStyle btnStyle in ConstantValuePool.ButtonStyleList)
                        {
                            if (details.ButtonStyleID.Equals(btnStyle.ButtonStyleID))
                            {
                                float emSize = (float)btnStyle.FontSize;
                                FontStyle style = FontStyle.Regular;
                                btn.Font = new Font(btnStyle.FontName, emSize, style);
                                btn.ForeColor = ColorConvert.RGB(btnStyle.ForeColor);
                                btn.BackColor = btn.DisplayColor = ColorConvert.RGB(btnStyle.BackColor);
                                break;
                            }
                        }
                    }
                    //设置页码按钮的显示
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
                    if (endIndex >= _currentDetailsGroup.DetailsList.Count)
                    {
                        btnBack.Enabled = false;
                        btnBack.BackColor = ConstantValuePool.DisabledColor;
                    }
                    else
                    {
                        btnBack.Enabled = true;
                        btnBack.BackColor = btnBack.DisplayColor;
                    }
                }
                this.pnlItem.ResumeLayout(false);
                this.pnlItem.PerformLayout();
                this.ResumeLayout(false);
            }
        }

        private void HideItemButton()
        {
            //禁止引发Layout事件
            this.pnlItem.SuspendLayout();
            this.SuspendLayout();

            foreach (CrystalButton btnItem in _btnItemList)
            {
                btnItem.Visible = false;
            }
            btnHead.Enabled = false;
            btnHead.BackColor = ConstantValuePool.DisabledColor;
            btnBack.Enabled = false;
            btnBack.BackColor = ConstantValuePool.DisabledColor;

            this.pnlItem.ResumeLayout(false);
            this.pnlItem.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        #region goods or detail button event

        private void btnGroup_Click(object sender, EventArgs e)
        {
            CrystalButton btnGroup = sender as CrystalButton;
            if(btnGroup == null) return;
            if (btnGroup.Tag is GoodsGroup)
            {
                _currentGoodsGroup = btnGroup.Tag as GoodsGroup;
                Color pressedColor = ConstantValuePool.PressedColor;
                foreach (ButtonStyle btnStyle in ConstantValuePool.ButtonStyleList)
                {
                    if (_currentGoodsGroup.ButtonStyleID.Equals(btnStyle.ButtonStyleID))
                    {
                        pressedColor = ColorConvert.RGB(btnStyle.ClickedBackColor);
                        break;
                    }
                }
                btnGroup.BackColor = pressedColor;
                if (_prevPressedButton == null)
                {
                    _prevPressedButton = btnGroup;
                }
                else
                {
                    if (btnGroup.Text != _prevPressedButton.Text)
                    {
                        _prevPressedButton.BackColor = _prevPressedButton.DisplayColor;
                    }
                    _prevPressedButton = btnGroup;
                }
                _itemPageIndex = 0;
                DisplayGoodsButton();
            }
            if (btnGroup.Tag is DetailsGroup)
            {
                _prevPressedButton.BackColor = _prevPressedButton.DisplayColor;

                DetailsGroup detailsGroup = btnGroup.Tag as DetailsGroup;
                Color pressedColor = ConstantValuePool.PressedColor;
                foreach (ButtonStyle btnStyle in ConstantValuePool.ButtonStyleList)
                {
                    if (detailsGroup.ButtonStyleID.Equals(btnStyle.ButtonStyleID))
                    {
                        pressedColor = ColorConvert.RGB(btnStyle.ClickedBackColor);
                        break;
                    }
                }
                btnGroup.BackColor = pressedColor;
                _prevPressedButton = btnGroup;
                if (detailsGroup.DetailsList != null && detailsGroup.DetailsList.Count > 0)
                {
                    _currentDetailsGroup = detailsGroup;
                    _itemPageIndex = 0;
                    DisplayDetailButton();
                }
            }
        }

        private void btnItem_Click(object sender, EventArgs e)
        {
            CrystalButton btnItem = sender as CrystalButton;
            if(btnItem == null) return;
            if (btnItem.Tag is Goods)
            {
                Goods goods = btnItem.Tag as Goods;
                decimal goodsDiscount = 0;
                Discount _discount = null;
                bool isContainsSalePrice = false;   //是否包含特价

                decimal goodsPrice = goods.SellPrice;
                decimal goodsNum = 1M;
                if (goods.IsCustomQty || goods.IsCustomPrice)
                {
                    decimal sellPrice = OrderDetailsService.GetInstance().GetLastCustomPrice(goods.GoodsID);
                    if (sellPrice > 0)
                    {
                        goodsPrice = sellPrice;
                    }
                    if (goods.IsCustomQty || sellPrice == 0)
                    {
                        FormCustomQty form = new FormCustomQty(goods.IsCustomQty, goodsNum, goods.IsCustomPrice, goodsPrice);
                        form.ShowDialog();
                        if (form.CustomPrice > 0)
                        {
                            goodsPrice = form.CustomPrice;
                        }
                        if (form.CustomQty > 0)
                        {
                            goodsNum = form.CustomQty;
                        }
                    }
                    goods.SellPrice = goodsPrice;
                    btnItem.Tag = goods;
                }
                else
                {
                    #region 判断是否限时特价
                    //所属组特价
                    bool isInGroup = false;
                    foreach (GoodsLimitedTimeSale item in ConstantValuePool.GroupLimitedTimeSaleList)
                    {
                        if (item.ItemID == _currentGoodsGroup.GoodsGroupID)
                        {
                            if (!IsValidDate(item.BeginDate, item.EndDate, item.Week, item.Day, item.Hour, item.Minute))
                            {
                                continue;
                            }
                            _discount = new Discount();
                            _discount.DiscountID = new Guid("66666666-6666-6666-6666-666666666666");
                            _discount.DiscountName = "促销折扣";
                            if (item.DiscountType == (int)DiscountItemType.DiscountRate)
                            {
                                goodsDiscount = goods.SellPrice * item.DiscountRate;
                                _discount.DiscountType = (int)DiscountItemType.DiscountRate;
                                _discount.DiscountRate = item.DiscountRate;
                                _discount.OffFixPay = 0;
                            }
                            if (item.DiscountType == (int)DiscountItemType.OffFixPay)
                            {
                                goodsDiscount = item.OffFixPay;
                                _discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                _discount.DiscountRate = 0;
                                _discount.OffFixPay = item.OffFixPay;
                            }
                            if (item.DiscountType == (int)DiscountItemType.OffSaleTo)
                            {
                                goodsDiscount = goods.SellPrice - item.OffSaleTo;
                                _discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                _discount.DiscountRate = 0;
                                _discount.OffFixPay = goods.SellPrice - item.OffSaleTo;
                            }
                            isInGroup = true;
                            isContainsSalePrice = true;
                            break;
                        }
                    }
                    //品项特价
                    if (!isInGroup)
                    {
                        foreach (GoodsLimitedTimeSale item in ConstantValuePool.GoodsLimitedTimeSaleList)
                        {
                            if (item.ItemID == goods.GoodsID)
                            {
                                if (!IsValidDate(item.BeginDate, item.EndDate, item.Week, item.Day, item.Hour, item.Minute))
                                {
                                    continue;
                                }
                                _discount = new Discount();
                                _discount.DiscountID = new Guid("66666666-6666-6666-6666-666666666666");
                                _discount.DiscountName = "促销折扣";
                                if (item.DiscountType == (int)DiscountItemType.DiscountRate)
                                {
                                    goodsDiscount = goods.SellPrice * item.DiscountRate;
                                    _discount.DiscountType = (int)DiscountItemType.DiscountRate;
                                    _discount.DiscountRate = item.DiscountRate;
                                    _discount.OffFixPay = 0;
                                }
                                if (item.DiscountType == (int)DiscountItemType.OffFixPay)
                                {
                                    goodsDiscount = item.OffFixPay;
                                    _discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                    _discount.DiscountRate = 0;
                                    _discount.OffFixPay = item.OffFixPay;
                                }
                                if (item.DiscountType == (int)DiscountItemType.OffSaleTo)
                                {
                                    goodsDiscount = goods.SellPrice - item.OffSaleTo;
                                    _discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                    _discount.DiscountRate = 0;
                                    _discount.OffFixPay = goods.SellPrice - item.OffSaleTo;
                                }
                                isContainsSalePrice = true;
                                break;
                            }
                        }
                    }
                    #endregion
                }

                int index, selectedIndex;
                index = selectedIndex = dgvGoodsOrder.Rows.Add(new DataGridViewRow());
                dgvGoodsOrder.Rows[index].Cells["ItemID"].Value = goods.GoodsID;
                dgvGoodsOrder.Rows[index].Cells["ItemID"].Tag = goods;
                dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value = goodsNum;
                dgvGoodsOrder.Rows[index].Cells["GoodsName"].Value = goods.GoodsName;
                dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value = goodsPrice * goodsNum;
                dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value = (-goodsDiscount).ToString("f2");
                if (_discount != null)
                {
                    dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Tag = _discount;
                }
                dgvGoodsOrder.Rows[index].Cells["ItemType"].Value = OrderItemType.Goods;
                dgvGoodsOrder.Rows[index].Cells["CanDiscount"].Value = goods.CanDiscount;
                dgvGoodsOrder.Rows[index].Cells["ItemUnit"].Value = goods.Unit;
                
                #region 判断是否套餐
                if (!isContainsSalePrice)
                {
                    IList<GoodsSetMeal> goodsSetMealList = ConstantValuePool.GoodsSetMealList.Where(goodsSetMeal => goodsSetMeal.ParentGoodsID.Equals(goods.GoodsID)).ToList();
                    if (goodsSetMealList.Count > 0)
                    {
                        Dictionary<int, List<GoodsSetMeal>> dicGoodsSetMealByGroup = new Dictionary<int, List<GoodsSetMeal>>();
                        foreach (GoodsSetMeal item in goodsSetMealList)
                        {
                            if (dicGoodsSetMealByGroup.ContainsKey(item.GroupNo))
                            {
                                dicGoodsSetMealByGroup[item.GroupNo].Add(item);
                            }
                            else
                            {
                                List<GoodsSetMeal> temp = new List<GoodsSetMeal>();
                                temp.Add(item);
                                dicGoodsSetMealByGroup.Add(item.GroupNo, temp);
                            }
                        }
                        bool isSingleGoodsSetMeal = false;
                        foreach (KeyValuePair<int, List<GoodsSetMeal>> item in dicGoodsSetMealByGroup)
                        {
                            if (item.Value[0].IsRequired && item.Value[0].LimitedQty == item.Value.Count)
                            {
                                isSingleGoodsSetMeal = true;
                            }
                            else
                            {
                                isSingleGoodsSetMeal = false;
                                break;
                            }
                        }
                        if (isSingleGoodsSetMeal)
                        {
                            foreach (GoodsSetMeal item in goodsSetMealList)
                            {
                                Goods temp = new Goods();
                                temp.GoodsID = item.GoodsID;
                                temp.GoodsNo = item.GoodsNo;
                                temp.GoodsName = item.GoodsName;
                                temp.GoodsName2nd = item.GoodsName2nd;
                                temp.Unit = item.Unit;
                                temp.SellPrice = item.SellPrice;
                                temp.CanDiscount = false;
                                temp.AutoShowDetails = false;
                                temp.PrintSolutionName = item.PrintSolutionName;
                                temp.DepartID = item.DepartID;
                                //更新列表
                                index = dgvGoodsOrder.Rows.Add(new DataGridViewRow());
                                dgvGoodsOrder.Rows[index].Cells["ItemID"].Value = item.GoodsID;
                                dgvGoodsOrder.Rows[index].Cells["ItemID"].Tag = temp;
                                dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value = item.ItemQty;
                                dgvGoodsOrder.Rows[index].Cells["GoodsName"].Value = "--" + item.GoodsName;
                                dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value = item.SellPrice;
                                decimal discount;
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
                                dgvGoodsOrder.Rows[index].Cells["CanDiscount"].Value = false;
                                dgvGoodsOrder.Rows[index].Cells["ItemUnit"].Value = item.Unit;
                            }
                        }
                        else
                        {
                            //需要人工选择
                            if (dicGoodsSetMealByGroup.Count > 0)
                            {
                                FormGoodsSetMeal form = new FormGoodsSetMeal(dicGoodsSetMealByGroup);
                                form.ShowDialog();
                                if (form.DicResultGoodsSetMeal.Count > 0)
                                {
                                    foreach (KeyValuePair<int, List<GoodsSetMeal>> dicItem in form.DicResultGoodsSetMeal)
                                    {
                                        foreach (GoodsSetMeal item in dicItem.Value)
                                        {
                                            Goods temp = new Goods();
                                            temp.GoodsID = item.GoodsID;
                                            temp.GoodsNo = item.GoodsNo;
                                            temp.GoodsName = item.GoodsName;
                                            temp.GoodsName2nd = item.GoodsName2nd;
                                            temp.Unit = item.Unit;
                                            temp.SellPrice = item.SellPrice;
                                            temp.CanDiscount = false;
                                            temp.AutoShowDetails = false;
                                            temp.PrintSolutionName = item.PrintSolutionName;
                                            temp.DepartID = item.DepartID;
                                            //更新列表
                                            index = dgvGoodsOrder.Rows.Add(new DataGridViewRow());
                                            dgvGoodsOrder.Rows[index].Cells["ItemID"].Value = item.GoodsID;
                                            dgvGoodsOrder.Rows[index].Cells["ItemID"].Tag = temp;
                                            dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value = item.ItemQty;
                                            dgvGoodsOrder.Rows[index].Cells["GoodsName"].Value = "--" + item.GoodsName;
                                            dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value = item.SellPrice;
                                            decimal discount;
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
                                            dgvGoodsOrder.Rows[index].Cells["CanDiscount"].Value = false;
                                            dgvGoodsOrder.Rows[index].Cells["ItemUnit"].Value = item.Unit;
                                        }
                                    }
                                }
                                else
                                {
                                    dgvGoodsOrder.Rows.RemoveAt(dgvGoodsOrder.Rows.Count - 1);
                                    return;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region 判断是否自动显示细项组
                if (goods.AutoShowDetails)
                {
                    _groupPageIndex = 0;
                    _itemPageIndex = 0;
                    if (goods.DetailsGroupIDList != null && goods.DetailsGroupIDList.Count > 0)
                    {
                        _goodsOrDetails = false;    //状态为细项
                        _currentDetailsGroupIdList = goods.DetailsGroupIDList;
                        DisplayDetailGroupButton();
                        HideItemButton();
                        _detailsPrefix = "--";
                    }
                }
                #endregion

                //datagridview滚动条定位
                dgvGoodsOrder.Rows[selectedIndex].Selected = true;
                dgvGoodsOrder.CurrentCell = dgvGoodsOrder.Rows[selectedIndex].Cells["GoodsNum"];
                //统计
                BindOrderInfoSum();
            }
            if (btnItem.Tag is Details)
            {
                Details details = btnItem.Tag as Details;
                if (dgvGoodsOrder.CurrentRow != null)
                {
                    int selectIndex = dgvGoodsOrder.CurrentRow.Index;
                    if (_currentDetailsGroup.LimitedNumbers > 0)
                    {
                        object objGroupLimitNum = dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Tag;
                        if (objGroupLimitNum == null)
                        {
                            Dictionary<Guid, int> dicGroupLimitNum = new Dictionary<Guid, int>();
                            dicGroupLimitNum.Add(_currentDetailsGroup.DetailsGroupID, 1);
                            dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Tag = dicGroupLimitNum;
                        }
                        else
                        {
                            Dictionary<Guid, int> dicGroupLimitNum = objGroupLimitNum as Dictionary<Guid, int>;
                            if (dicGroupLimitNum != null)
                            {
                                if (dicGroupLimitNum.ContainsKey(_currentDetailsGroup.DetailsGroupID))
                                {
                                    int selectedNum = dicGroupLimitNum[_currentDetailsGroup.DetailsGroupID];
                                    if (selectedNum >= _currentDetailsGroup.LimitedNumbers)
                                    {
                                        MessageBox.Show("超出细项的数量限制！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                    else
                                    {
                                        selectedNum++;
                                        dicGroupLimitNum[_currentDetailsGroup.DetailsGroupID] = selectedNum;
                                        dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Tag = dicGroupLimitNum;
                                    }
                                }
                                else
                                {
                                    dicGroupLimitNum.Add(_currentDetailsGroup.DetailsGroupID, 1);
                                    dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Tag = dicGroupLimitNum;
                                }
                            }
                        }
                    }
                    //数量
                    decimal itemNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                    DataGridViewRow dgr = dgvGoodsOrder.Rows[0].Clone() as DataGridViewRow;
                    if (dgr != null)
                    {
                        dgr.Cells[0].Value = details.DetailsID;
                        dgr.Cells[0].Tag = details;
                        dgr.Cells[1].Value = itemNum;
                        dgr.Cells[2].Value = _detailsPrefix + details.DetailsName;
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
                            if (Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value) == (int) OrderItemType.Goods)
                            {
                                for (int i = selectIndex + 1; i < dgvGoodsOrder.RowCount; i++)
                                {
                                    int itemType = Convert.ToInt32(dgvGoodsOrder.Rows[i].Cells["ItemType"].Value);
                                    if (itemType == (int) OrderItemType.Goods)
                                    {
                                        break;
                                    }
                                    rowIndex++;
                                }
                            }
                            dgvGoodsOrder.Rows.Insert(rowIndex, dgr);
                        }
                    }
                    //统计
                    BindOrderInfoSum();
                }
            }
            //更新第二屏信息
            if (Screen.AllScreens.Length > 1 && ConstantValuePool.BizSettingConfig.SecondScreenEnabled)
            {
                if (ConstantValuePool.SecondScreenForm != null && ConstantValuePool.SecondScreenForm is FormSecondScreen)
                {
                    ((FormSecondScreen)ConstantValuePool.SecondScreenForm).BindGoodsOrderInfo(dgvGoodsOrder);
                }
            }
        }

        #endregion

        #region 分页事件

        private void btnPageUp_Click(object sender, EventArgs e)
        {
            _groupPageIndex--;
            if (_goodsOrDetails)
            {
                DisplayGoodsGroupButton();
            }
            else
            {
                DisplayDetailGroupButton();
            }
        }

        private void btnPageDown_Click(object sender, EventArgs e)
        {
            _groupPageIndex++;
            if (_goodsOrDetails)
            {
                DisplayGoodsGroupButton();
            }
            else
            {
                DisplayDetailGroupButton();
            }
        }

        private void btnHead_Click(object sender, EventArgs e)
        {
            _itemPageIndex--;
            if (_goodsOrDetails)
            {
                DisplayGoodsButton();
            }
            else
            {
                DisplayDetailButton();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _itemPageIndex++;
            if (_goodsOrDetails)
            {
                DisplayGoodsButton();
            }
            else
            {
                DisplayDetailButton();
            }
        }

        #endregion

        private void BindGoodsOrderInfo()
        {
            this.dgvGoodsOrder.Rows.Clear();
            if (_salesOrder.orderDetailsList != null && _salesOrder.orderDetailsList.Count > 0)
            {                
                foreach (OrderDetails orderDetails in _salesOrder.orderDetailsList)
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
            _totalPrice = totalPrice;
            _discount = totalDiscount;
            this.lbTotalPrice.Text = "总金额：" + totalPrice.ToString("f2");
            this.lbDiscount.Text = "折扣：" + totalDiscount.ToString("f2");
            decimal wholePayMoney = totalPrice + totalDiscount;
            decimal actualPayMoney = CutOffDecimal.HandleCutOff(wholePayMoney, ConstantValuePool.SysConfig.IsCutTail, ConstantValuePool.SysConfig.CutTailType, ConstantValuePool.SysConfig.CutTailDigit);
            _actualPayMoney = actualPayMoney;
            _cutOff = wholePayMoney - actualPayMoney;
            this.lbNeedPayMoney.Text = "实际应付：" + actualPayMoney.ToString("f2");
            this.lbCutOff.Text = "去零：" + (-_cutOff).ToString("f2");
        }

        private void btnShowDetails_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.CurrentRow != null)
            {
                int selectIndex = dgvGoodsOrder.CurrentRow.Index;
                if (dgvGoodsOrder.Rows[selectIndex].Cells["OrderDetailsID"].Value == null)
                {
                    _groupPageIndex = 0;
                    _itemPageIndex = 0;
                    int itemType = Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value);
                    if (itemType == (int)OrderItemType.Goods)
                    {
                        Goods goods = dgvGoodsOrder.Rows[selectIndex].Cells["ItemID"].Tag as Goods;
                        if (goods != null && goods.DetailsGroupIDList != null && goods.DetailsGroupIDList.Count > 0)
                        {
                            _goodsOrDetails = false;    //状态为细项
                            _currentDetailsGroupIdList = goods.DetailsGroupIDList;
                            DisplayDetailGroupButton();
                            HideItemButton();
                            _detailsPrefix = "--";
                        }
                    }
                    else if (itemType == (int)OrderItemType.Details)
                    {
                        Details details = dgvGoodsOrder.Rows[selectIndex].Cells["ItemID"].Tag as Details;
                        if (details != null && details.DetailsGroupIDList != null && details.DetailsGroupIDList.Count > 0)
                        {
                            _goodsOrDetails = false;    //状态为细项
                            _currentDetailsGroupIdList = details.DetailsGroupIDList;
                            DisplayDetailGroupButton();
                            HideItemButton();
                            //detail prefix --
                            string goodsName = dgvGoodsOrder.Rows[selectIndex].Cells["GoodsName"].Value.ToString();
                            if (goodsName.IndexOf('-') >= 0)
                            {
                                int index = goodsName.LastIndexOf('-');
                                _detailsPrefix = goodsName.Substring(0, index + 1);
                                _detailsPrefix += "--";
                            }
                            else
                            {
                                _detailsPrefix = "--";
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
                    CrystalButton btnNumber = sender as CrystalButton;
                    if(btnNumber == null) return;
                    decimal quantity;
                    if (string.IsNullOrEmpty(_goodsNum))
                    {
                        _goodsNum = btnNumber.Text;
                        if (!decimal.TryParse(_goodsNum, out quantity))
                        {
                            MessageBox.Show("输入数量格式不正确！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        if (_goodsNum.IndexOf('.') > 0)
                        {
                            _goodsNum += btnNumber.Text;
                        }
                        else
                        {
                            _goodsNum = btnNumber.Text;
                        }
                        if (!decimal.TryParse(_goodsNum, out quantity))
                        {
                            MessageBox.Show("输入数量格式不正确！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        //初始化
                        _goodsNum = string.Empty;
                    }
                    int itemType = Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value);
                    if (itemType == (int)OrderItemType.Goods)
                    {
                        Goods goods = dgvGoodsOrder.Rows[selectIndex].Cells["ItemID"].Tag as Goods;
                        decimal sellPrice = goods == null ? 0 : goods.SellPrice;
                        decimal originalGoodsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                        decimal originalGoodsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsDiscount"].Value);
                        dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value = quantity;
                        dgvGoodsOrder.Rows[selectIndex].Cells["GoodsPrice"].Value = sellPrice * quantity;
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
                        decimal sellPrice = details == null ? 0 : details.SellPrice;
                        decimal originalDetailsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                        decimal originalDetailsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsDiscount"].Value);
                        dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value = quantity;
                        dgvGoodsOrder.Rows[selectIndex].Cells["GoodsPrice"].Value = sellPrice * quantity;
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
            if (dgvGoodsOrder.CurrentRow != null)
            {
                int selectIndex = dgvGoodsOrder.CurrentRow.Index;
                if (dgvGoodsOrder.Rows[selectIndex].Cells["OrderDetailsID"].Value == null)
                {
                    int itemType = Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value);
                    if (itemType == (int) OrderItemType.Goods || itemType == (int) OrderItemType.Details)
                    {
                        if (string.IsNullOrEmpty(_goodsNum))
                        {
                            _goodsNum = "0.";
                        }
                        else
                        {
                            if (_goodsNum.IndexOf('.') > 0)
                            {
                                return;
                            }
                            else
                            {
                                _goodsNum += ".";
                            }
                        }
                    }
                }
            }
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.CurrentRow != null)
            {
                int selectIndex = dgvGoodsOrder.CurrentRow.Index;
                if (dgvGoodsOrder.Rows[selectIndex].Cells["OrderDetailsID"].Value == null)
                {
                    FormNumericKeypad keyForm = new FormNumericKeypad();
                    keyForm.DisplayText = "请输入品项数量";
                    keyForm.ShowDialog();
                    decimal quantity;
                    if (!string.IsNullOrEmpty(keyForm.KeypadValue) && decimal.TryParse(keyForm.KeypadValue, out quantity))
                    {

                        int itemType = Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value);
                        if (itemType == (int)OrderItemType.Goods)
                        {
                            Goods goods = dgvGoodsOrder.Rows[selectIndex].Cells["ItemID"].Tag as Goods;
                            decimal sellPrice = goods == null ? 0 : goods.SellPrice;
                            decimal originalGoodsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                            decimal originalGoodsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsDiscount"].Value);
                            dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value = quantity;
                            dgvGoodsOrder.Rows[selectIndex].Cells["GoodsPrice"].Value = sellPrice * quantity;
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
                        if (itemType == (int)OrderItemType.Details)
                        {
                            Details details = dgvGoodsOrder.Rows[selectIndex].Cells["ItemID"].Tag as Details;
                            decimal sellPrice = details == null ? 0 : details.SellPrice;
                            decimal originalDetailsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                            decimal originalDetailsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsDiscount"].Value);
                            dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value = quantity;
                            dgvGoodsOrder.Rows[selectIndex].Cells["GoodsPrice"].Value = sellPrice * quantity;
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
                    int itemType = Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value);
                    if (itemType == (int)OrderItemType.Details)
                    {
                        MessageBox.Show("细项不能单独删除！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (itemType == (int)OrderItemType.SetMeal)
                    {
                        MessageBox.Show("套餐项不能单独删除！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    Guid orderDetailsId = new Guid(dgvGoodsOrder.Rows[selectIndex].Cells["OrderDetailsID"].Value.ToString());
                    decimal goodsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                    string goodsName = dgvGoodsOrder.Rows[selectIndex].Cells["GoodsName"].Value.ToString();
                    decimal goodsPrice = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsPrice"].Value);
                    if (!RightsItemCode.FindRights(RightsItemCode.CANCELGOODS))
                    {
                        decimal singleItemPriceSum = goodsPrice / goodsNum;
                        if (itemType == (int)OrderItemType.Goods && selectIndex < dgvGoodsOrder.Rows.Count - 1)
                        {
                            for (int index = selectIndex + 1; index < dgvGoodsOrder.RowCount; index++)
                            {
                                if (Convert.ToInt32(dgvGoodsOrder.Rows[index].Cells["ItemType"].Value) == (int)OrderItemType.Goods)
                                {
                                    break;
                                }
                                singleItemPriceSum += Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value) / Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value);
                            }
                            if (singleItemPriceSum > ConstantValuePool.CurrentEmployee.LimitMoney)
                            {
                                if (DialogResult.Yes == MessageBox.Show("当前用户不具备该权限，并且超过最高退菜限额，是否更换用户？", "信息提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                                {
                                    //权限验证
                                    bool hasRights = false;
                                    FormRightsCode formRightsCode = new FormRightsCode();
                                    formRightsCode.ShowDialog();
                                    if (formRightsCode.ReturnValue)
                                    {
                                        IList<string> rightsCodeList = formRightsCode.RightsCodeList;
                                        if (RightsItemCode.FindRights(rightsCodeList, RightsItemCode.CANCELGOODS))
                                        {
                                            hasRights = true;
                                        }
                                    }
                                    if (!hasRights)
                                    {
                                        return;
                                    }
                                }
                                else
                                {
                                    return;
                                }
                            }
                        }
                    }
                    FormCancelOrder form = new FormCancelOrder(goodsName, goodsNum);
                    form.ShowDialog();
                    if (form.DelItemNum > 0 && form.CurrentReason != null)
                    {
                        if (form.DelItemNum < goodsNum)
                        {
                            //Key:Index, Value:RemainNum
                            Dictionary<int, decimal> dicRemainNum = new Dictionary<int, decimal>();
                            List<DeletedOrderDetails> deletedOrderDetailsList = new List<DeletedOrderDetails>();
                            //主项
                            decimal remainNum = goodsNum - form.DelItemNum;
                            dicRemainNum.Add(selectIndex, remainNum);
                            decimal originalDetailsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                            decimal originalDetailsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsDiscount"].Value);
                            DeletedOrderDetails orderDetails = new DeletedOrderDetails();
                            orderDetails.OrderDetailsID = orderDetailsId;
                            orderDetails.DeletedQuantity = -form.DelItemNum;
                            orderDetails.RemainQuantity = remainNum;
                            orderDetails.OffPay = Math.Round(-originalDetailsDiscount / originalDetailsNum * remainNum, 4);
                            orderDetails.AuthorisedManager = ConstantValuePool.CurrentEmployee.EmployeeID;
                            orderDetails.CancelEmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                            orderDetails.CancelReasonName = form.CurrentReason.ReasonName;
                            deletedOrderDetailsList.Add(orderDetails);
                            //细项
                            if (selectIndex < dgvGoodsOrder.Rows.Count - 1)
                            {
                                for (int index = selectIndex + 1; index < dgvGoodsOrder.RowCount; index++)
                                {
                                    if (Convert.ToInt32(dgvGoodsOrder.Rows[index].Cells["ItemType"].Value) == (int)OrderItemType.Goods)
                                    {
                                        break;
                                    }
                                    orderDetailsId = new Guid(dgvGoodsOrder.Rows[index].Cells["OrderDetailsID"].Value.ToString());
                                    originalDetailsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value);
                                    originalDetailsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value);
                                    decimal delItemNum = originalDetailsNum / goodsNum * form.DelItemNum;
                                    remainNum = originalDetailsNum - delItemNum;
                                    dicRemainNum.Add(index, remainNum);
                                    DeletedOrderDetails item = new DeletedOrderDetails();
                                    item.OrderDetailsID = orderDetailsId;
                                    item.DeletedQuantity = -delItemNum;
                                    item.RemainQuantity = remainNum;
                                    item.OffPay = Math.Round(-originalDetailsDiscount / originalDetailsNum * remainNum, 4);
                                    item.AuthorisedManager = ConstantValuePool.CurrentEmployee.EmployeeID;
                                    item.CancelEmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                                    item.CancelReasonName = form.CurrentReason.ReasonName;
                                    deletedOrderDetailsList.Add(item);
                                }
                            }
                            //计算价格信息
                            decimal totalPrice = 0, totalDiscount = 0;
                            for (int i = 0; i < dgvGoodsOrder.RowCount; i++)
                            {
                                if (dgvGoodsOrder.Rows[i].Cells["OrderDetailsID"].Value != null)
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
                            }
                            decimal wholePayMoney = totalPrice + totalDiscount;
                            decimal actualPayMoney = CutOffDecimal.HandleCutOff(wholePayMoney, ConstantValuePool.SysConfig.IsCutTail, ConstantValuePool.SysConfig.CutTailType, ConstantValuePool.SysConfig.CutTailDigit);
                            //构造DeletedSingleOrder对象
                            DeletedSingleOrder deletedSingleOrder = new DeletedSingleOrder();
                            deletedSingleOrder.OrderID = _salesOrder.order.OrderID;
                            deletedSingleOrder.TotalSellPrice = totalPrice;
                            deletedSingleOrder.ActualSellPrice = actualPayMoney;
                            deletedSingleOrder.DiscountPrice = totalDiscount;
                            deletedSingleOrder.CutOffPrice = wholePayMoney - actualPayMoney;
                            deletedSingleOrder.deletedOrderDetailsList = deletedOrderDetailsList;

                            if (DeletedOrderService.GetInstance().DeleteSingleOrder(deletedSingleOrder))
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
                                MessageBox.Show("删除品项失败！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                            orderDetails.OrderDetailsID = orderDetailsId;
                            orderDetails.DeletedQuantity = -Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                            orderDetails.RemainQuantity = 0;
                            orderDetails.OffPay = 0;
                            orderDetails.AuthorisedManager = ConstantValuePool.CurrentEmployee.EmployeeID;
                            orderDetails.CancelEmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                            orderDetails.CancelReasonName = form.CurrentReason.ReasonName;
                            deletedOrderDetailsList.Add(orderDetails);
                            //细项
                            if (selectIndex < dgvGoodsOrder.RowCount - 1)
                            {
                                for (int index = selectIndex + 1; index < dgvGoodsOrder.RowCount; index++)
                                {
                                    if (Convert.ToInt32(dgvGoodsOrder.Rows[index].Cells["ItemType"].Value) == (int)OrderItemType.Goods)
                                    {
                                        break;
                                    }
                                    deletedIndexList.Add(index);
                                    orderDetailsId = new Guid(dgvGoodsOrder.Rows[index].Cells["OrderDetailsID"].Value.ToString());
                                    DeletedOrderDetails item = new DeletedOrderDetails();
                                    item.OrderDetailsID = orderDetailsId;
                                    item.DeletedQuantity = -Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value);
                                    item.RemainQuantity = 0;
                                    item.OffPay = 0;
                                    item.AuthorisedManager = ConstantValuePool.CurrentEmployee.EmployeeID;
                                    item.CancelEmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                                    item.CancelReasonName = form.CurrentReason.ReasonName;
                                    deletedOrderDetailsList.Add(item);
                                }
                            }
                            //计算价格信息
                            decimal totalPrice = 0, totalDiscount = 0;
                            for (int i = 0; i < dgvGoodsOrder.RowCount; i++)
                            {
                                if (dgvGoodsOrder.Rows[i].Cells["OrderDetailsID"].Value != null)
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
                            }
                            decimal wholePayMoney = totalPrice + totalDiscount;
                            decimal actualPayMoney = CutOffDecimal.HandleCutOff(wholePayMoney, ConstantValuePool.SysConfig.IsCutTail, ConstantValuePool.SysConfig.CutTailType, ConstantValuePool.SysConfig.CutTailDigit);
                            //构造DeletedSingleOrder对象
                            DeletedSingleOrder deletedSingleOrder = new DeletedSingleOrder();
                            deletedSingleOrder.OrderID = _salesOrder.order.OrderID;
                            deletedSingleOrder.TotalSellPrice = totalPrice;
                            deletedSingleOrder.ActualSellPrice = actualPayMoney;
                            deletedSingleOrder.DiscountPrice = totalDiscount;
                            deletedSingleOrder.CutOffPrice = wholePayMoney - actualPayMoney;
                            deletedSingleOrder.deletedOrderDetailsList = deletedOrderDetailsList;

                            if (DeletedOrderService.GetInstance().DeleteSingleOrder(deletedSingleOrder))
                            {
                                for(int i = deletedIndexList.Count - 1; i >= 0; i--)
                                {
                                    dgvGoodsOrder.Rows.RemoveAt(deletedIndexList[i]);
                                }
                            }
                            else
                            {
                                MessageBox.Show("删除品项失败！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    else if (Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value) == (int)OrderItemType.SetMeal)
                    {
                        MessageBox.Show("套餐项不能单独删除！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        dgvGoodsOrder.Rows.RemoveAt(selectIndex);
                        if (selectIndex < dgvGoodsOrder.RowCount - 1)
                        {
                            for (int i = selectIndex; i < dgvGoodsOrder.RowCount; i++)
                            {
                                if (Convert.ToInt32(dgvGoodsOrder.Rows[i].Cells["ItemType"].Value) == (int)OrderItemType.Goods)
                                {
                                    break;
                                }
                                dgvGoodsOrder.Rows.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                }
                //统计
                BindOrderInfoSum();
                //更新第二屏信息
                if (Screen.AllScreens.Length > 1 && ConstantValuePool.BizSettingConfig.SecondScreenEnabled)
                {
                    if (ConstantValuePool.SecondScreenForm != null && ConstantValuePool.SecondScreenForm is FormSecondScreen)
                    {
                        ((FormSecondScreen)ConstantValuePool.SecondScreenForm).BindGoodsOrderInfo(dgvGoodsOrder);
                    }
                }
            }
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.Rows.Count > 0)
            {
                //判断参加限时组合销售
                JoinGoodsCombinedSale(this.dgvGoodsOrder);
                BindOrderInfoSum();
                if (SubmitSalesOrder())
                {
                    //更新桌况为占用状态
                    int status = (int)DeskButtonStatus.OCCUPIED;
                    if (DeskService.GetInstance().UpdateDeskStatus(_currentDeskName, string.Empty, status))
                    {
                        //更新第二屏信息
                        if (Screen.AllScreens.Length > 1 && ConstantValuePool.BizSettingConfig.SecondScreenEnabled)
                        {
                            if (ConstantValuePool.SecondScreenForm != null && ConstantValuePool.SecondScreenForm is FormSecondScreen)
                            {
                                ((FormSecondScreen)ConstantValuePool.SecondScreenForm).ClearGoodsOrderInfo();
                            }
                        }
                        _onShow = false;
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("更新桌况失败！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("请先选择菜品！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (_salesOrder == null)
            {
                //更新桌况为空闲状态
                int status = (int)DeskButtonStatus.IDLE_MODE;
                if (DeskService.GetInstance().UpdateDeskStatus(_currentDeskName, string.Empty, status))
                {
                    //更新第二屏信息
                    if (Screen.AllScreens.Length > 1 && ConstantValuePool.BizSettingConfig.SecondScreenEnabled)
                    {
                        if (ConstantValuePool.SecondScreenForm != null && ConstantValuePool.SecondScreenForm is FormSecondScreen)
                        {
                            ((FormSecondScreen)ConstantValuePool.SecondScreenForm).ClearGoodsOrderInfo();
                        }
                    }
                    _onShow = false;
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("更新桌况失败！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                //更新桌况为占用状态
                int status = (int)DeskButtonStatus.OCCUPIED;
                if (DeskService.GetInstance().UpdateDeskStatus(_currentDeskName, string.Empty, status))
                {
                    //更新第二屏信息
                    if (Screen.AllScreens.Length > 1 && ConstantValuePool.BizSettingConfig.SecondScreenEnabled)
                    {
                        if (ConstantValuePool.SecondScreenForm != null && ConstantValuePool.SecondScreenForm is FormSecondScreen)
                        {
                            ((FormSecondScreen)ConstantValuePool.SecondScreenForm).ClearGoodsOrderInfo();
                        }
                    }
                    _onShow = false;
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("更新桌况失败！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnTurnBack_Click(object sender, EventArgs e)
        {
            if (!_goodsOrDetails)
            {
                _prevPressedButton.BackColor = _prevPressedButton.DisplayColor;
                _goodsOrDetails = true;
                LoadDefaultGoodsGroupButton();
            }
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
                        decimal itemPrice = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsPrice"].Value);
                        FormDiscount formDiscount = new FormDiscount(DiscountDisplayModel.SingleDiscount, -1, itemPrice);
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
                            //统计
                            BindOrderInfoSum();
                            //更新第二屏信息
                            if (Screen.AllScreens.Length > 1 && ConstantValuePool.BizSettingConfig.SecondScreenEnabled)
                            {
                                if (ConstantValuePool.SecondScreenForm != null && ConstantValuePool.SecondScreenForm is FormSecondScreen)
                                {
                                    ((FormSecondScreen)ConstantValuePool.SecondScreenForm).BindGoodsOrderInfo(dgvGoodsOrder);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.Rows.Count > 0)
            {
                //权限验证
                bool hasRights = false;
                if (RightsItemCode.FindRights(RightsItemCode.CHECKOUT))
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
                        if (RightsItemCode.FindRights(rightsCodeList, RightsItemCode.CHECKOUT))
                        {
                            hasRights = true;
                        }
                    }
                }
                if (!hasRights)
                {
                    return;
                }
                //判断参加限时组合销售
                JoinGoodsCombinedSale(this.dgvGoodsOrder);
                BindOrderInfoSum();
                if (SubmitSalesOrder())
                {
                    //转入结账页面
                    SalesOrder newSalesOrder = CopyExtension.Clone<SalesOrder>(_salesOrder);
                    FormCheckOut checkForm = new FormCheckOut(newSalesOrder, _currentDeskName);
                    checkForm.ShowDialog();
                    if (checkForm.IsPaidOrder)
                    {
                        //更新第二屏信息
                        if (Screen.AllScreens.Length > 1 && ConstantValuePool.BizSettingConfig.SecondScreenEnabled)
                        {
                            if (ConstantValuePool.SecondScreenForm != null && ConstantValuePool.SecondScreenForm is FormSecondScreen)
                            {
                                ((FormSecondScreen)ConstantValuePool.SecondScreenForm).ClearGoodsOrderInfo();
                            }
                        }
                        _onShow = false;
                        this.Hide();
                    }
                    else
                    {
                        if (checkForm.IsPreCheckOut)
                        {
                            //更新第二屏信息
                            if (Screen.AllScreens.Length > 1 && ConstantValuePool.BizSettingConfig.SecondScreenEnabled)
                            {
                                if (ConstantValuePool.SecondScreenForm != null && ConstantValuePool.SecondScreenForm is FormSecondScreen)
                                {
                                    ((FormSecondScreen)ConstantValuePool.SecondScreenForm).ClearGoodsOrderInfo();
                                }
                            }
                            _onShow = false;
                            this.Hide();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("请先选择菜品！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);;
            }
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            if (_salesOrder != null)
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

                    if (!DeletedOrderService.GetInstance().DeleteWholeOrder(deletedOrder))
                    {
                        MessageBox.Show("删除账单失败！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);;
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
            if (DeskService.GetInstance().UpdateDeskStatus(_currentDeskName, string.Empty, status))
            {
                //更新第二屏信息
                if (Screen.AllScreens.Length > 1 && ConstantValuePool.BizSettingConfig.SecondScreenEnabled)
                {
                    if (ConstantValuePool.SecondScreenForm != null && ConstantValuePool.SecondScreenForm is FormSecondScreen)
                    {
                        ((FormSecondScreen)ConstantValuePool.SecondScreenForm).ClearGoodsOrderInfo();
                    }
                }
                _onShow = false;
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
            if (dgvGoodsOrder.Rows.Count <= 0)
            {
                MessageBox.Show("请先选择菜品！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (DataGridViewRow dr in dgvGoodsOrder.Rows)
            {
                if (dr.Cells["OrderDetailsID"].Value == null)
                {
                    MessageBox.Show("存在新单，不能印单！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (ConstantValuePool.BizSettingConfig.printConfig.Enabled)
            {
                //打印
                PrintData printData = new PrintData();
                printData.ShopName = ConstantValuePool.CurrentShop.ShopName;
                printData.DeskName = _currentDeskName;
                printData.PersonNum = _personNum.ToString();
                printData.PrintTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                printData.EmployeeNo = _employeeNo;
                printData.TranSequence = _salesOrder.order.TranSequence.ToString();
                printData.ShopAddress = ConstantValuePool.CurrentShop.RunAddress;
                printData.Telephone = ConstantValuePool.CurrentShop.Telephone;
                printData.GoodsOrderList = new List<GoodsOrder>();
                foreach (DataGridViewRow dr in dgvGoodsOrder.Rows)
                {
                    GoodsOrder goodsOrder = new GoodsOrder();
                    goodsOrder.GoodsName = dr.Cells["GoodsName"].Value.ToString();
                    decimal itemQty = Convert.ToDecimal(dr.Cells["GoodsNum"].Value);
                    decimal totalSellPrice = Convert.ToDecimal(dr.Cells["GoodsPrice"].Value);
                    goodsOrder.GoodsNum = itemQty.ToString("f1");
                    goodsOrder.SellPrice = (totalSellPrice / itemQty).ToString("f2");
                    goodsOrder.TotalSellPrice = totalSellPrice.ToString("f2");
                    goodsOrder.TotalDiscount = Convert.ToDecimal(dr.Cells["GoodsDiscount"].Value).ToString("f2");
                    goodsOrder.Unit = dr.Cells["ItemUnit"].Value.ToString();
                    printData.GoodsOrderList.Add(goodsOrder);
                }
                int copies = ConstantValuePool.BizSettingConfig.printConfig.Copies;
                string paperWidth = ConstantValuePool.BizSettingConfig.printConfig.PaperWidth;
                if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.DRIVER)
                {
                    string printerName = ConstantValuePool.BizSettingConfig.printConfig.Name;
                    string paperName = ConstantValuePool.BizSettingConfig.printConfig.PaperName;
                    DriverOrderPrint printer = DriverOrderPrint.GetInstance(printerName, paperName, paperWidth);
                    for (int i = 0; i < copies; i++)
                    {
                        printer.DoPrintOrder(printData);
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
                                printer.DoPrintOrder(printData);
                            }
                        }
                    }
                }
                if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.ETHERNET)
                {
                    string ipAddress = ConstantValuePool.BizSettingConfig.printConfig.Name;
                    InstructionOrderPrint printer = new InstructionOrderPrint(ipAddress, 9100, paperWidth);
                    for (int i = 0; i < copies; i++)
                    {
                        printer.DoPrintOrder(printData);
                    }
                }
                if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.USB)
                {
                    string vid = ConstantValuePool.BizSettingConfig.printConfig.VID;
                    string pid = ConstantValuePool.BizSettingConfig.printConfig.PID;
                    string endpointId = ConstantValuePool.BizSettingConfig.printConfig.EndpointID;
                    InstructionOrderPrint printer = new InstructionOrderPrint(vid, pid, endpointId, paperWidth);
                    for (int i = 0; i < copies; i++)
                    {
                        printer.DoPrintOrder(printData);
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
                _personNum = int.Parse(keyForm.KeypadValue);
                this.btnPersonNum.Text = "人数：" + _personNum;
            }
        }

        private void dgvGoodsOrder_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_goodsOrDetails)
            {
                _prevPressedButton.BackColor = _prevPressedButton.DisplayColor;
                _goodsOrDetails = true;
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
                        if (goods != null)
                        {
                            dgr.Cells["GoodsName"].Value = "*" + goods.GoodsName;
                        }
                        dgr.Cells["Wait"].Value = 1;    //挂单
                        //细项
                        for (int index = seletedIndex + 1; index < dgvGoodsOrder.Rows.Count; index++)
                        {
                            itemType = Convert.ToInt32(dgvGoodsOrder.Rows[index].Cells["ItemType"].Value);
                            if (itemType == (int)OrderItemType.Goods)
                            {
                                break;
                            }
                            string itemName = dgvGoodsOrder.Rows[index].Cells["GoodsName"].Value.ToString();
                            int spIndex = itemName.LastIndexOf('-');
                            string goodsName = itemName.Substring(spIndex + 1);
                            dgvGoodsOrder.Rows[index].Cells["GoodsName"].Value = itemName.Substring(0, spIndex + 1) + "*" + goodsName;
                            dgvGoodsOrder.Rows[index].Cells["Wait"].Value = 1;
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
                            orderDetails = new OrderDetails();
                            orderDetails.OrderDetailsID = new Guid(dgvGoodsOrder.Rows[index].Cells["OrderDetailsID"].Value.ToString());
                            orderDetails.Wait = 0;
                            orderDetailsList.Add(orderDetails);
                        }
                        if (OrderDetailsService.GetInstance().LadeOrderDetails(orderDetailsList))
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
                    if (OrderDetailsService.GetInstance().LadeOrderDetails(orderDetailsList))
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
                    MessageBox.Show("没有需要提单的品项！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("有新单，不能整单提单！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.exTabControl1.SelectedIndex = 0;
        }

        private void btnSplitBill_Click(object sender, EventArgs e)
        {
            if (_salesOrder == null)
            {
                MessageBox.Show("账单未落单，不允许分单！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            foreach (DataGridViewRow dr in dgvGoodsOrder.Rows)
            {
                if (dr.Cells["OrderDetailsID"].Value == null)
                {
                    MessageBox.Show("账单中包含未落单品项，不允许分单！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            FormSplitBill form = new FormSplitBill(_salesOrder);
            form.ShowDialog();
            if (form.SplitOrderSuccess)
            {
                //重新加载
                _salesOrder = SalesOrderService.GetInstance().GetSalesOrder(_salesOrder.order.OrderID);
                BindGoodsOrderInfo();   //绑定订单信息
                BindOrderInfoSum();
            }
            this.exTabControl1.SelectedIndex = 0;
        }

        private void btnTasteRemark_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.CurrentRow != null)
            {
                //权限验证
                bool hasRights = false;
                if (RightsItemCode.FindRights(RightsItemCode.CUSTOMTASTE))
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
                        if (RightsItemCode.FindRights(rightsCodeList, RightsItemCode.CUSTOMTASTE))
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
                if (dgvGoodsOrder.Rows[selectIndex].Cells["OrderDetailsID"].Value == null)
                {
                    string goodsName = dgvGoodsOrder.Rows[selectIndex].Cells["GoodsName"].Value.ToString();
                    FormCustomDetails form = new FormCustomDetails(goodsName);
                    form.ShowDialog();
                    if (!string.IsNullOrEmpty(form.CustomTasteName))
                    {
                        string printSolutionName = string.Empty;
                        Guid departId = Guid.Empty;
                        int itemType = Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value);
                        if (itemType == (int)OrderItemType.Goods)
                        {
                            Goods goods = dgvGoodsOrder.Rows[selectIndex].Cells["ItemID"].Tag as Goods;
                            if (goods != null)
                            {
                                printSolutionName = goods.PrintSolutionName;
                                departId = goods.DepartID;
                            }
                        }
                        else if (itemType == (int)OrderItemType.Details)
                        {
                            Details detail = dgvGoodsOrder.Rows[selectIndex].Cells["ItemID"].Tag as Details;
                            if (detail != null)
                            {
                                printSolutionName = detail.PrintSolutionName;
                                departId = detail.DepartID;
                            }
                        }
                        Details details = new Details();
                        details.DetailsID = new Guid("77777777-7777-7777-7777-777777777777");
                        details.DetailsNo = "7777";
                        details.DetailsName = details.DetailsName2nd = form.CustomTasteName.Replace("-", "");
                        details.SellPrice = 0;
                        details.CanDiscount = false;
                        details.AutoShowDetails = false;
                        details.PrintSolutionName = printSolutionName;
                        details.DepartID = departId;
                        //数量
                        decimal itemNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                        DataGridViewRow dgr = dgvGoodsOrder.Rows[0].Clone() as DataGridViewRow;
                        if (dgr != null)
                        {
                            dgr.Cells[0].Value = details.DetailsID;
                            dgr.Cells[0].Tag = details;
                            dgr.Cells[1].Value = itemNum;
                            dgr.Cells[2].Value = form.CustomTasteName;
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
                                if (Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value) == (int) OrderItemType.Goods)
                                {
                                    for (int i = selectIndex + 1; i < dgvGoodsOrder.RowCount; i++)
                                    {
                                        itemType = Convert.ToInt32(dgvGoodsOrder.Rows[i].Cells["ItemType"].Value);
                                        if (itemType == (int) OrderItemType.Goods)
                                        {
                                            break;
                                        }
                                        rowIndex++;
                                    }
                                }
                                dgvGoodsOrder.Rows.Insert(rowIndex, dgr);
                            }
                        }
                        //统计
                        BindOrderInfoSum();
                    }
                }
                this.exTabControl1.SelectedIndex = 0;
            }
        }

        private void btnPriceCode_Click(object sender, EventArgs e)
        {
            _showSilverCode = !_showSilverCode;
            DisplayGoodsButton();
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
                            Guid orderDetailsId = new Guid(dgr.Cells["OrderDetailsID"].Value.ToString());
                            orderDetailsIdList.Add(orderDetailsId);
                            if (seletedIndex < dgvGoodsOrder.Rows.Count - 1)
                            {
                                for (int index = seletedIndex + 1; index < dgvGoodsOrder.Rows.Count; index++)
                                {
                                    if (Convert.ToInt32(dgvGoodsOrder.Rows[index].Cells["ItemType"].Value) == (int)OrderItemType.Goods)
                                    {
                                        break;
                                    }
                                    orderDetailsId = new Guid(dgvGoodsOrder.Rows[index].Cells["OrderDetailsID"].Value.ToString());
                                    orderDetailsIdList.Add(orderDetailsId);
                                }
                            }
                            ReminderOrder reminder = new ReminderOrder();
                            reminder.OrderID = _salesOrder.order.OrderID;
                            reminder.OrderDetailsIDList = orderDetailsIdList;
                            reminder.ReasonName = form.CurrentReason.ReasonName;
                            reminder.EmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                            bool result = ReminderService.GetInstance().CreateReminderOrder(reminder);
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
            //1:交班 2:日结
            const int modelType = 1;
            FormSalesReport formReport = new FormSalesReport(modelType);
            formReport.ShowDialog();
            if (formReport.HandleSuccess)
            {
                this.Close();
                ConstantValuePool.DeskForm.Close();
            }
        }

        private void btnDailyStatement_Click(object sender, EventArgs e)
        {
            //1:交班 2:日结
            const int modelType = 2;
            FormSalesReport formReport = new FormSalesReport(modelType);
            formReport.ShowDialog();
            if (formReport.HandleSuccess)
            {
                this.Close();
                ConstantValuePool.DeskForm.Close();
            }
        }

        private void btnPromotion_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.Rows.Count <= 0)
            {
                MessageBox.Show("请先选择菜品！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ConstantValuePool.PromotionList == null || ConstantValuePool.PromotionList.Count <= 0)
            {
                MessageBox.Show("未找到有效的促销条件，请检查是否在后台添加。", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            IList<Promotion> promotionList;
            if (ConstantValuePool.PromotionCronTriggerList != null && ConstantValuePool.PromotionCronTriggerList.Count > 0)
            {
                promotionList = new List<Promotion>();
                foreach (Promotion promotion in ConstantValuePool.PromotionList)
                {
                    foreach (PromotionCronTrigger cronTrigger in ConstantValuePool.PromotionCronTriggerList)
                    {
                        if (promotion.ActivityNo == cronTrigger.ActivityNo)
                        {
                            PromotionCronTriggerService cronTriggerService = new PromotionCronTriggerService(cronTrigger);
                            if (cronTriggerService.IsPromotionInTime())
                            {
                                promotionList.Add(promotion);
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                promotionList = ConstantValuePool.PromotionList;
            }
            if (promotionList == null || promotionList.Count <= 0)
            {
                MessageBox.Show("未找到符合条件的促销规则", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            IList<OrderDetails> orderDetailsList = new List<OrderDetails>();
            //组装OrderDetails
            foreach (DataGridViewRow dr in dgvGoodsOrder.Rows)
            {
                //填充OrderDetails
                OrderDetails orderDetails = new OrderDetails();
                orderDetails.GoodsID = new Guid(dr.Cells["ItemID"].Value.ToString());
                orderDetails.TotalSellPrice = Convert.ToDecimal(dr.Cells["GoodsPrice"].Value);
                orderDetails.TotalDiscount = Convert.ToDecimal(dr.Cells["GoodsDiscount"].Value);
                orderDetails.ItemQty = Convert.ToDecimal(dr.Cells["GoodsNum"].Value);
                orderDetails.SellPrice = orderDetails.TotalSellPrice/orderDetails.ItemQty;
                orderDetailsList.Add(orderDetails);
            }
            foreach (Promotion promotion in promotionList)
            {
                IList<PromotionCondition> promotionConditionList = null;
                if (ConstantValuePool.PromotionConditionList != null && ConstantValuePool.PromotionConditionList.Count > 0)
                {
                    promotionConditionList = ConstantValuePool.PromotionConditionList.Where(promotionCondition => promotion.ActivityNo == promotionCondition.ActivityNo).ToList();
                }
                PromotionConditionService conditionService = new PromotionConditionService(promotionConditionList);
                bool result = conditionService.IsItemEligible(orderDetailsList, promotion.IsIncluded, promotion.AndOr);
                if (result)
                {
                    //totalQuantity
                    decimal totalQuantity = 0M;
                    foreach (DataGridViewRow dr in dgvGoodsOrder.Rows)
                    {
                        int itemType = Convert.ToInt32(dr.Cells["ItemType"].Value);
                        if (itemType == (int) OrderItemType.Goods)
                        {
                            totalQuantity += Convert.ToDecimal(dr.Cells["GoodsNum"].Value);
                        }
                    }
                    //PromotionPresent
                    IList<PromotionPresent> promotionPresentList = new List<PromotionPresent>();
                    foreach (PromotionPresent promotionPresent in ConstantValuePool.PromotionPresentList)
                    {
                        if (promotion.ActivityNo == promotionPresent.ActivityNo)
                        {
                            if (promotionPresent.TotalMoney <= _actualPayMoney && promotionPresent.TotalQuantity <= totalQuantity)
                            {
                                promotionPresentList.Add(promotionPresent);
                            }
                        }
                    }
                    if (promotionPresentList.Count > 0)
                    {
                        PromotionPresentService presentService;
                        if (promotion.PresentType == 1)
                        {
                            presentService = new PromotionPresentCommonService(_actualPayMoney, totalQuantity, promotionPresentList);
                            presentService.GetPromotionPresents(dgvGoodsOrder);
                        }
                        if (promotion.PresentType == 2)
                        {
                            presentService = new PromotionPresentMultipleService(_actualPayMoney, totalQuantity, promotionPresentList);
                            presentService.GetPromotionPresents(dgvGoodsOrder);
                        }
                        if (promotion.PresentType == 3 || promotion.PresentType == 4)
                        {

                        }
                        if (!promotion.WithOtherPromotion) break;
                    }
                }
            }
        }

        #region private method

        private bool SubmitSalesOrder()
        {
            Guid orderId = _salesOrder == null ? Guid.NewGuid() : _salesOrder.order.OrderID;
            IList<GoodsCheckStock> temp = new List<GoodsCheckStock>();
            IList<OrderDetails> newOrderDetailsList = new List<OrderDetails>();
            IList<OrderDiscount> newOrderDiscountList = new List<OrderDiscount>();
            foreach (DataGridViewRow dr in dgvGoodsOrder.Rows)
            {
                if (dr.Cells["OrderDetailsID"].Value == null)
                {
                    Guid orderDetailsId = Guid.NewGuid();
                    int itemType = Convert.ToInt32(dr.Cells["ItemType"].Value);
                    string goodsName = dr.Cells["GoodsName"].Value.ToString();
                    //填充OrderDetails
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.OrderDetailsID = orderDetailsId;
                    orderDetails.OrderID = orderId;
                    orderDetails.DeviceNo = ConstantValuePool.BizSettingConfig.DeviceNo;
                    orderDetails.TotalSellPrice = Convert.ToDecimal(dr.Cells["GoodsPrice"].Value);
                    orderDetails.TotalDiscount = Convert.ToDecimal(dr.Cells["GoodsDiscount"].Value);
                    orderDetails.ItemQty = Convert.ToDecimal(dr.Cells["GoodsNum"].Value);
                    orderDetails.EmployeeID = _employeeId;
                    if (dr.Cells["Wait"].Value != null)
                    {
                        orderDetails.Wait = Convert.ToInt32(dr.Cells["Wait"].Value);
                    }
                    if (itemType == (int)OrderItemType.Goods)
                    {
                        orderDetails.ItemType = (int)OrderItemType.Goods;
                        Goods goods = dr.Cells["ItemID"].Tag as Goods;
                        if (goods != null)
                        {
                            orderDetails.GoodsID = goods.GoodsID;
                            orderDetails.GoodsNo = goods.GoodsNo;
                            orderDetails.GoodsName = goods.GoodsName;
                            orderDetails.Unit = goods.Unit;
                            orderDetails.CanDiscount = goods.CanDiscount;
                            orderDetails.SellPrice = goods.SellPrice;
                            orderDetails.PrintSolutionName = goods.PrintSolutionName;
                            orderDetails.DepartID = goods.DepartID;
                            if (goods.IsCheckStock)
                            {
                                GoodsCheckStock goodsCheckStock = new GoodsCheckStock();
                                goodsCheckStock.GoodsID = goods.GoodsID;
                                goodsCheckStock.GoodsName = goods.GoodsName;
                                goodsCheckStock.ReducedQuantity = orderDetails.ItemQty;
                                temp.Add(goodsCheckStock);
                            }
                        }
                    }
                    else if (itemType == (int)OrderItemType.Details)
                    {
                        orderDetails.ItemType = (int)OrderItemType.Details;
                        Details details = dr.Cells["ItemID"].Tag as Details;
                        if (details != null)
                        {
                            orderDetails.GoodsID = details.DetailsID;
                            orderDetails.GoodsNo = details.DetailsNo;
                            orderDetails.GoodsName = details.DetailsName;
                            orderDetails.CanDiscount = details.CanDiscount;
                            orderDetails.Unit = ""; //
                            orderDetails.SellPrice = details.SellPrice;
                            orderDetails.PrintSolutionName = details.PrintSolutionName;
                            orderDetails.DepartID = details.DepartID;
                        }
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
                            orderDiscount.OrderID = orderId;
                            orderDiscount.OrderDetailsID = orderDetailsId;
                            orderDiscount.DiscountID = discount.DiscountID;
                            orderDiscount.DiscountName = discount.DiscountName;
                            orderDiscount.DiscountType = discount.DiscountType;
                            orderDiscount.DiscountRate = discount.DiscountRate;
                            orderDiscount.OffFixPay = discount.OffFixPay;
                            orderDiscount.OffPay = offPay;
                            orderDiscount.EmployeeID = _employeeId;
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
                        Guid orderDetailsId = new Guid(dr.Cells["OrderDetailsID"].Value.ToString());
                        //填充OrderDetails
                        OrderDetails orderDetails = new OrderDetails();
                        orderDetails.OrderDetailsID = orderDetailsId;
                        orderDetails.OrderID = orderId;
                        orderDetails.DeviceNo = ConstantValuePool.BizSettingConfig.DeviceNo;
                        orderDetails.ItemQty = Convert.ToDecimal(dr.Cells["GoodsNum"].Value);
                        orderDetails.ItemType = Convert.ToInt32(dr.Cells["ItemType"].Value);
                        orderDetails.GoodsName = dr.Cells["GoodsName"].Value.ToString();
                        orderDetails.TotalSellPrice = Convert.ToDecimal(dr.Cells["GoodsPrice"].Value);
                        orderDetails.TotalDiscount = Convert.ToDecimal(dr.Cells["GoodsDiscount"].Value);
                        orderDetails.Unit = dr.Cells["ItemUnit"].Value.ToString();
                        orderDetails.EmployeeID = _employeeId;
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
                            orderDiscount.OrderID = orderId;
                            orderDiscount.OrderDetailsID = orderDetailsId;
                            orderDiscount.DiscountID = discount.DiscountID;
                            orderDiscount.DiscountName = discount.DiscountName;
                            orderDiscount.DiscountType = discount.DiscountType;
                            orderDiscount.DiscountRate = discount.DiscountRate;
                            orderDiscount.OffFixPay = discount.OffFixPay;
                            orderDiscount.OffPay = offPay;
                            orderDiscount.EmployeeID = _employeeId;
                            newOrderDiscountList.Add(orderDiscount);
                        }
                    }
                }
            }
            //品项沽清
            if (temp.Count > 0)
            {
                IList<GoodsCheckStock> tempGoodsStockList = GoodsService.GetInstance().GetGoodsCheckStock();
                if (tempGoodsStockList != null && tempGoodsStockList.Count > 0)
                {
                    IList<GoodsCheckStock> goodsCheckStockList = new List<GoodsCheckStock>();
                    foreach (GoodsCheckStock item in temp)
                    {
                        bool isContains = tempGoodsStockList.Any(tempGoodsStock => item.GoodsID.Equals(tempGoodsStock.GoodsID));
                        if (isContains)
                        {
                            goodsCheckStockList.Add(item);
                        }
                    }
                    if (goodsCheckStockList.Count > 0)
                    {
                        string goodsName = GoodsService.GetInstance().UpdateReducedGoodsQty(goodsCheckStockList);
                        if (!string.IsNullOrEmpty(goodsName))
                        {
                            MessageBox.Show(string.Format("<{0}> 的剩余数量不足！", goodsName), "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }
                }
            }
            if (_salesOrder == null)    //新增的菜单
            {
                Order order = new Order();
                order.OrderID = orderId;
                order.TotalSellPrice = _totalPrice;
                order.ActualSellPrice = _actualPayMoney;
                order.DiscountPrice = _discount;
                order.CutOffPrice = _cutOff;
                order.ServiceFee = 0;
                order.DeviceNo = ConstantValuePool.BizSettingConfig.DeviceNo;
                order.DeskName = _currentDeskName;
                order.EatType = (int)EatWayType.DineIn;
                order.Status = 0;
                order.PeopleNum = _personNum;
                order.EmployeeID = _employeeId;
                order.EmployeeNo = _employeeNo;

                SalesOrder salesOrder = new SalesOrder();
                salesOrder.order = order;
                salesOrder.orderDetailsList = newOrderDetailsList;
                salesOrder.orderDiscountList = newOrderDiscountList;
                int tranSequence = SalesOrderService.GetInstance().CreateSalesOrder(salesOrder);
                if (tranSequence > 0)
                {
                    //重新加载
                    _salesOrder = SalesOrderService.GetInstance().GetSalesOrder(orderId);
                    BindGoodsOrderInfo();   //绑定订单信息
                    BindOrderInfoSum();
                }
                else
                {
                    MessageBox.Show("结账提交失败！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                if (newOrderDetailsList.Count > 0)
                {
                    Order order = new Order();
                    order.OrderID = orderId;
                    order.TotalSellPrice = _totalPrice;
                    order.ActualSellPrice = _actualPayMoney;
                    order.DiscountPrice = _discount;
                    order.CutOffPrice = _cutOff;
                    order.ServiceFee = 0;
                    order.DeviceNo = ConstantValuePool.BizSettingConfig.DeviceNo;
                    order.DeskName = _currentDeskName;
                    order.PeopleNum = _personNum;
                    order.EmployeeID = _employeeId;
                    order.EmployeeNo = _employeeNo;

                    SalesOrder salesOrder = new SalesOrder();
                    salesOrder.order = order;
                    salesOrder.orderDetailsList = newOrderDetailsList;
                    salesOrder.orderDiscountList = newOrderDiscountList;
                    Int32 result = SalesOrderService.GetInstance().UpdateSalesOrder(salesOrder);
                    if (result == 1)
                    {
                        //重新加载
                        _salesOrder = SalesOrderService.GetInstance().GetSalesOrder(orderId);
                        BindGoodsOrderInfo();   //绑定订单信息
                        BindOrderInfoSum();
                    }
                    else if (result == 2)
                    {
                        MessageBox.Show("当前桌号被其他设备占用，请退出后重试！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);;
                        return false;
                    }
                    else
                    {
                        MessageBox.Show("结账提交失败，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);;
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
                    printData.DeskName = _currentDeskName;
                    printData.PersonNum = _personNum.ToString();
                    printData.PrintTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    printData.EmployeeNo = _employeeNo;
                    printData.TranSequence = _salesOrder.order.TranSequence.ToString();
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
                    if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.DRIVER)
                    {
                        string printerName = ConstantValuePool.BizSettingConfig.printConfig.Name;
                        string paperName = ConstantValuePool.BizSettingConfig.printConfig.PaperName;
                        DriverOrderPrint printer = DriverOrderPrint.GetInstance(printerName, paperName, paperWidth);
                        for (int i = 0; i < copies; i++)
                        {
                            printer.DoPrintOrder(printData);
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
                                    printer.DoPrintOrder(printData);
                                }
                            }
                        }
                    }
                    if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.ETHERNET)
                    {
                        string ipAddress = ConstantValuePool.BizSettingConfig.printConfig.Name;
                        InstructionOrderPrint printer = new InstructionOrderPrint(ipAddress, 9100, paperWidth);
                        for (int i = 0; i < copies; i++)
                        {
                            printer.DoPrintOrder(printData);
                        }
                    }
                    if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.USB)
                    {
                        string vid = ConstantValuePool.BizSettingConfig.printConfig.VID;
                        string pid = ConstantValuePool.BizSettingConfig.printConfig.PID;
                        string endpointId = ConstantValuePool.BizSettingConfig.printConfig.EndpointID;
                        InstructionOrderPrint printer = new InstructionOrderPrint(vid, pid, endpointId, paperWidth);
                        for (int i = 0; i < copies; i++)
                        {
                            printer.DoPrintOrder(printData);
                        }
                    }
                }
            }
            return true;
        }

        private void LoadDefaultGoodsGroupButton()
        {
            _groupPageIndex = 0;
            _itemPageIndex = 0;
            DisplayGoodsGroupButton();
            HideItemButton();
        }

        private void ResizeSearchPad()
        {
            if (this.Width > 1024 && pnlSearch.Visible)
            {
                double widthRate = Convert.ToDouble(this.Width - this.pnlLeft.Width) / 504;
                const double heightRate = 1;
                foreach (Control c in this.pnlSearch.Controls)
                {
                    SetControlSize(c, widthRate, heightRate);
                }
            }
        }

        private void SetControlSize(Control ctl, double widthRate, double heightRate)
        {
            ctl.Width = Convert.ToInt32(ctl.Width * widthRate);
            ctl.Height = Convert.ToInt32(ctl.Height * heightRate);
            ctl.Location = new Point(Convert.ToInt32(ctl.Location.X * widthRate), Convert.ToInt32(ctl.Location.Y * heightRate));
        }

        private bool IsItemButtonEnabled(Guid itemId, ItemsType itemType)
        {
            bool isEnabled = true;
            foreach (GoodsCronTrigger trigger in ConstantValuePool.GoodsCronTriggerList)
            {
                if (itemId == trigger.ItemID && (int)itemType == trigger.ItemType)
                {
                    isEnabled = IsValidDate(trigger.BeginDate, trigger.EndDate, trigger.Week, trigger.Day, trigger.Hour, trigger.Minute);
                    break;
                }
            }
            return isEnabled;
        }

        private bool IsValidDate(string beginDate, string endDate, string week, string day, string hour, string minute)
        {
            bool isValid = true;
            if (DateTime.Now >= DateTime.Parse(beginDate) && DateTime.Now <= DateTime.Parse(endDate))
            {
                DayOfWeek curWeek = DateTime.Now.DayOfWeek;
                string curDay = DateTime.Now.Day.ToString();
                int curHour = DateTime.Now.Hour;
                int curMinute = DateTime.Now.Minute;
                //判断周或者日
                if (week == "?")
                {
                    //判断是否包含当日
                    if (day != "*")
                    {
                        bool isContainsDay = day.Split(',').Any(item => curDay == item);
                        if (!isContainsDay)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //判断是否包含周几
                    //判断包含# 例:当月第几周星期几
                    if (week.IndexOf('#') > 0)
                    {
                        string weekIndex = week.Split('#')[0];
                        string weekDay = week.Split('#')[1];
                        //计算当日是当月的第几周
                        DateTime firstofMonth = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + "01");
                        int i = (int)firstofMonth.Date.DayOfWeek;
                        if (i == 0)
                        {
                            i = 7;
                        }
                        int curWeekIndex = (DateTime.Now.Day + i - 1) / 7;
                        if (curWeekIndex != int.Parse(weekIndex))
                        {
                            return false;
                        }
                        if ((int)curWeek != int.Parse(weekDay))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //不包含# 例:当月每个星期几
                        bool isContainsWeek = week.Split(',').Any(item => (int)curWeek == int.Parse(item));
                        if (!isContainsWeek)
                        {
                            return false;
                        }
                    }
                }
                //判断时
                if (hour != "*")
                {
                    if (hour.IndexOf('-') > 0)
                    {
                        string hourMinute = curHour.ToString().PadLeft(2, '0') + ":" + curMinute.ToString().PadLeft(2, '0');
                        if (hour.IndexOf(',') > 0) //多个小时时间段
                        {
                            string[] hourArr = hour.Split(',');
                            bool isContainHour = false;
                            foreach (string item in hourArr)
                            {
                                string beginHour = item.Split('-')[0].Trim();
                                string endHour = item.Split('-')[1].Trim();
                                if (String.Compare(hourMinute, beginHour, StringComparison.OrdinalIgnoreCase) >= 0 && String.Compare(hourMinute, endHour, StringComparison.OrdinalIgnoreCase) <= 0)
                                {
                                    isContainHour = true;
                                    break;
                                }
                            }
                            if (!isContainHour)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            string beginHour = hour.Split('-')[0].Trim();
                            string endHour = hour.Split('-')[1].Trim();
                            if (String.Compare(hourMinute, beginHour, StringComparison.OrdinalIgnoreCase) < 0 || String.Compare(hourMinute, endHour, StringComparison.OrdinalIgnoreCase) > 0)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (hour.IndexOf(',') > 0) //多个小时
                        {
                            bool isContainsHour = hour.Split(',').Any(item => curHour == int.Parse(item));
                            if (!isContainsHour)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (curHour != int.Parse(hour))
                            {
                                return false;
                            }
                        }
                    }
                }
                //判断分
                if (minute != "*")
                {
                    if (minute.IndexOf('-') > 0)
                    {
                        if (minute.IndexOf(',') > 0) //多个分钟时间段
                        {
                            string[] minuteArr = minute.Split(',');
                            bool isContainMinute = false;
                            foreach (string item in minuteArr)
                            {
                                string beginMinute = item.Split('-')[0];
                                string endMinute = item.Split('-')[1];
                                if (curMinute >= int.Parse(beginMinute) && curMinute <= int.Parse(endMinute))
                                {
                                    isContainMinute = true;
                                    break;
                                }
                            }
                            if (!isContainMinute)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            string beginMinute = minute.Split('-')[0];
                            string endMinute = minute.Split('-')[1];
                            if (curMinute < int.Parse(beginMinute) || curMinute > int.Parse(endMinute))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (minute.IndexOf(',') > 0) //多个分钟
                        {
                            bool isContainsMinute = minute.Split(',').Any(item => curMinute == int.Parse(item));
                            if (!isContainsMinute)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (curMinute != int.Parse(minute))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            else
            {
                isValid = false;
            }
            return isValid;
        }

        /// <summary>
        /// 判断是否存在限时组合销售
        /// </summary>
        /// <param name="goodsOrderGridView"></param>
        private void JoinGoodsCombinedSale(DataGridView goodsOrderGridView)
        {
            //获取符合时间条件的品项组组合销售
            if (ConstantValuePool.GroupCombinedSaleList == null || ConstantValuePool.GroupCombinedSaleList.Count <= 0) return;
            var groupCombinedSaleList = ConstantValuePool.GroupCombinedSaleList.Where(item => IsValidDate(item.BeginDate, item.EndDate, item.Week, item.Day, item.Hour, item.Minute)).ToList();
            //获取符合时间条件的品项组合销售
            if (ConstantValuePool.GoodsCombinedSaleList == null || ConstantValuePool.GoodsCombinedSaleList.Count <= 0) return;
            var goodsCombinedSaleList = ConstantValuePool.GoodsCombinedSaleList.Where(item => IsValidDate(item.BeginDate, item.EndDate, item.Week, item.Day, item.Hour, item.Minute)).ToList();

            List<Guid> hasExistGoodsId = new List<Guid>();
            for (int index = 0; index < goodsOrderGridView.Rows.Count; index++)
            {
                DataGridViewRow dr = goodsOrderGridView.Rows[index];
                int itemType = Convert.ToInt32(dr.Cells["ItemType"].Value);
                //限时组合销售只针对主品项
                if (itemType == (int)OrderItemType.Goods)
                {
                    Guid goodsId = new Guid(dr.Cells["ItemID"].Value.ToString());
                    decimal goodsQty = Convert.ToDecimal(dr.Cells["GoodsNum"].Value);
                    decimal totalSellPrice = Convert.ToDecimal(dr.Cells["GoodsPrice"].Value);
                    decimal totalDiscount = Convert.ToDecimal(dr.Cells["GoodsDiscount"].Value);
                    if (hasExistGoodsId.Exists(p => p == goodsId) || hasExistGoodsId.Exists(p => GoodsUtil.IsGoodsInGroup(goodsId, p)))
                    {
                        continue;
                    }
                    //在品项中是否存在
                    bool isInItem = false;
                    foreach (GoodsCombinedSale item in goodsCombinedSaleList)
                    {
                        if (item.ItemID == goodsId)
                        {
                            //组合优惠
                            decimal discountRate = Math.Abs(totalDiscount) / totalSellPrice;
                            bool isFit = goodsQty >= item.Quantity;
                            if (isFit)
                            {
                                if (item.MoreOrLess == 1)
                                {
                                    isFit = totalSellPrice/goodsQty > item.SellPrice;
                                }
                                else if (item.MoreOrLess == 2)
                                {
                                    isFit = totalSellPrice/goodsQty == item.SellPrice;
                                }
                                else if (item.MoreOrLess == 3)
                                {
                                    isFit = totalSellPrice/goodsQty < item.SellPrice;
                                }
                            }
                            if (isFit)
                            {
                                isFit = discountRate <= item.LeastDiscountRate;
                            }
                            if (isFit)
                            {
                                //优惠区间 第二杯半价
                                if (item.PreferentialInterval == 2)
                                {
                                    int count = 0;
                                    for (int m = index + 1; m < goodsOrderGridView.Rows.Count; m++)
                                    {
                                        //限时组合销售只针对主品项
                                        if (Convert.ToInt32(goodsOrderGridView.Rows[m].Cells["ItemType"].Value) == (int)OrderItemType.Goods)
                                        {
                                            Guid tempGoodsId = new Guid(goodsOrderGridView.Rows[m].Cells["ItemID"].Value.ToString());
                                            decimal tempTotalPrice = Convert.ToDecimal(goodsOrderGridView.Rows[m].Cells["GoodsPrice"].Value);
                                            if (tempGoodsId == item.ItemID)
                                            {
                                                if (count % 2 == 1)
                                                {
                                                    count++;
                                                    continue;
                                                }
                                                Discount discount = new Discount();
                                                discount.DiscountID = new Guid("66666666-6666-6666-6666-666666666666");
                                                discount.DiscountName = "促销折扣";
                                                decimal goodsDiscount = 0;
                                                if (item.DiscountType2 == (int)DiscountItemType.DiscountRate)
                                                {
                                                    goodsDiscount = tempTotalPrice * item.DiscountRate2;
                                                    discount.DiscountType = (int)DiscountItemType.DiscountRate;
                                                    discount.DiscountRate = item.DiscountRate2;
                                                    discount.OffFixPay = 0;
                                                }
                                                if (item.DiscountType2 == (int)DiscountItemType.OffFixPay)
                                                {
                                                    goodsDiscount = item.OffFixPay2;
                                                    discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                    discount.DiscountRate = 0;
                                                    discount.OffFixPay = item.OffFixPay2;
                                                }
                                                if (item.DiscountType2 == (int)DiscountItemType.OffSaleTo)
                                                {
                                                    goodsDiscount = tempTotalPrice - item.OffSaleTo2;
                                                    discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                    discount.DiscountRate = 0;
                                                    discount.OffFixPay = tempTotalPrice - item.OffSaleTo2;
                                                }
                                                goodsOrderGridView.Rows[m].Cells["GoodsDiscount"].Value = (-goodsDiscount).ToString("f2");
                                                goodsOrderGridView.Rows[m].Cells["GoodsDiscount"].Tag = discount;
                                                if (item.IsMultiple)
                                                {
                                                    count++;
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                //优惠区间 第二件半价 第三件免费
                                if (item.PreferentialInterval == 3)
                                {
                                    int count = 1;
                                    for (int m = index + 1; m < goodsOrderGridView.Rows.Count; m++)
                                    {
                                        //限时组合销售只针对主品项
                                        if (Convert.ToInt32(goodsOrderGridView.Rows[m].Cells["ItemType"].Value) == (int)OrderItemType.Goods)
                                        {
                                            Guid tempGoodsId = new Guid(goodsOrderGridView.Rows[m].Cells["ItemID"].Value.ToString());
                                            decimal tempTotalPrice = Convert.ToDecimal(goodsOrderGridView.Rows[m].Cells["GoodsPrice"].Value);
                                            if (tempGoodsId == item.ItemID)
                                            {
                                                if (count % 3 == 0)
                                                {
                                                    count++;
                                                    continue;
                                                }
                                                if (count % 3 == 1)
                                                {
                                                    Discount discount = new Discount();
                                                    discount.DiscountID = new Guid("66666666-6666-6666-6666-666666666666");
                                                    discount.DiscountName = "促销折扣";
                                                    decimal goodsDiscount = 0;
                                                    if (item.DiscountType2 == (int)DiscountItemType.DiscountRate)
                                                    {
                                                        goodsDiscount = tempTotalPrice * item.DiscountRate2;
                                                        discount.DiscountType = (int)DiscountItemType.DiscountRate;
                                                        discount.DiscountRate = item.DiscountRate2;
                                                        discount.OffFixPay = 0;
                                                    }
                                                    if (item.DiscountType2 == (int)DiscountItemType.OffFixPay)
                                                    {
                                                        goodsDiscount = item.OffFixPay2;
                                                        discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                        discount.DiscountRate = 0;
                                                        discount.OffFixPay = item.OffFixPay2;
                                                    }
                                                    if (item.DiscountType2 == (int)DiscountItemType.OffSaleTo)
                                                    {
                                                        goodsDiscount = tempTotalPrice - item.OffSaleTo2;
                                                        discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                        discount.DiscountRate = 0;
                                                        discount.OffFixPay = tempTotalPrice - item.OffSaleTo2;
                                                    }
                                                    goodsOrderGridView.Rows[m].Cells["GoodsDiscount"].Value = (-goodsDiscount).ToString("f2");
                                                    goodsOrderGridView.Rows[m].Cells["GoodsDiscount"].Tag = discount;
                                                    count++;
                                                    continue;
                                                }
                                                if (count % 3 == 2)
                                                {
                                                    Discount discount = new Discount();
                                                    discount.DiscountID = new Guid("66666666-6666-6666-6666-666666666666");
                                                    discount.DiscountName = "促销折扣";
                                                    decimal goodsDiscount = 0;
                                                    if (item.DiscountType3 == (int)DiscountItemType.DiscountRate)
                                                    {
                                                        goodsDiscount = tempTotalPrice * item.DiscountRate3;
                                                        discount.DiscountType = (int)DiscountItemType.DiscountRate;
                                                        discount.DiscountRate = item.DiscountRate3;
                                                        discount.OffFixPay = 0;
                                                    }
                                                    if (item.DiscountType3 == (int)DiscountItemType.OffFixPay)
                                                    {
                                                        goodsDiscount = item.OffFixPay3;
                                                        discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                        discount.DiscountRate = 0;
                                                        discount.OffFixPay = item.OffFixPay3;
                                                    }
                                                    if (item.DiscountType3 == (int)DiscountItemType.OffSaleTo)
                                                    {
                                                        goodsDiscount = tempTotalPrice - item.OffSaleTo3;
                                                        discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                        discount.DiscountRate = 0;
                                                        discount.OffFixPay = tempTotalPrice - item.OffSaleTo3;
                                                    }
                                                    goodsOrderGridView.Rows[m].Cells["GoodsDiscount"].Value = (-goodsDiscount).ToString("f2");
                                                    goodsOrderGridView.Rows[m].Cells["GoodsDiscount"].Tag = discount;
                                                    if (item.IsMultiple)
                                                    {
                                                        count++;
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            isInItem = true;
                            hasExistGoodsId.Add(goodsId);
                            break;
                        }
                    }
                    if (!isInItem)
                    {
                        //在品项组中是否存在
                        foreach (GoodsCombinedSale item in groupCombinedSaleList)
                        {
                            if (GoodsUtil.IsGoodsInGroup(goodsId, item.ItemID))
                            {
                                //组合优惠
                                decimal discountRate = Math.Abs(totalDiscount) / totalSellPrice;
                                bool isFit = goodsQty >= item.Quantity;
                                if (isFit)
                                {
                                    if (item.MoreOrLess == 1)
                                    {
                                        isFit = totalSellPrice/goodsQty > item.SellPrice;
                                    }
                                    else if (item.MoreOrLess == 2)
                                    {
                                        isFit = totalSellPrice/goodsQty == item.SellPrice;
                                    }
                                    else if (item.MoreOrLess == 3)
                                    {
                                        isFit = totalSellPrice/goodsQty < item.SellPrice;
                                    }
                                }
                                if (isFit)
                                {
                                    isFit = discountRate <= item.LeastDiscountRate;
                                }
                                if (isFit)
                                {
                                    //优惠区间 第二杯半价
                                    if (item.PreferentialInterval == 2)
                                    {
                                        int count = 0;
                                        for (int m = index + 1; m < goodsOrderGridView.Rows.Count; m++)
                                        {
                                            //限时组合销售只针对主品项
                                            if (Convert.ToInt32(goodsOrderGridView.Rows[m].Cells["ItemType"].Value) == (int)OrderItemType.Goods)
                                            {
                                                Guid tempGoodsId = new Guid(goodsOrderGridView.Rows[m].Cells["ItemID"].Value.ToString());
                                                decimal tempTotalPrice = Convert.ToDecimal(goodsOrderGridView.Rows[m].Cells["GoodsPrice"].Value);
                                                if (GoodsUtil.IsGoodsInGroup(tempGoodsId, item.ItemID))
                                                {
                                                    if (count % 2 == 1)
                                                    {
                                                        count++;
                                                        continue;
                                                    }
                                                    Discount discount = new Discount();
                                                    discount.DiscountID = new Guid("66666666-6666-6666-6666-666666666666");
                                                    discount.DiscountName = "促销折扣";
                                                    decimal goodsDiscount = 0;
                                                    if (item.DiscountType2 == (int)DiscountItemType.DiscountRate)
                                                    {
                                                        goodsDiscount = tempTotalPrice * item.DiscountRate2;
                                                        discount.DiscountType = (int)DiscountItemType.DiscountRate;
                                                        discount.DiscountRate = item.DiscountRate2;
                                                        discount.OffFixPay = 0;
                                                    }
                                                    if (item.DiscountType2 == (int)DiscountItemType.OffFixPay)
                                                    {
                                                        goodsDiscount = item.OffFixPay2;
                                                        discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                        discount.DiscountRate = 0;
                                                        discount.OffFixPay = item.OffFixPay2;
                                                    }
                                                    if (item.DiscountType2 == (int)DiscountItemType.OffSaleTo)
                                                    {
                                                        goodsDiscount = tempTotalPrice - item.OffSaleTo2;
                                                        discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                        discount.DiscountRate = 0;
                                                        discount.OffFixPay = tempTotalPrice - item.OffSaleTo2;
                                                    }
                                                    goodsOrderGridView.Rows[m].Cells["GoodsDiscount"].Value = (-goodsDiscount).ToString("f2");
                                                    goodsOrderGridView.Rows[m].Cells["GoodsDiscount"].Tag = discount;
                                                    if (item.IsMultiple)
                                                    {
                                                        count++;
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //优惠区间 第二件半价 第三件免费
                                    if (item.PreferentialInterval == 3)
                                    {
                                        int count = 1;
                                        for (int m = index + 1; m < goodsOrderGridView.Rows.Count; m++)
                                        {
                                            //限时组合销售只针对主品项
                                            if (Convert.ToInt32(goodsOrderGridView.Rows[m].Cells["ItemType"].Value) == (int)OrderItemType.Goods)
                                            {
                                                Guid tempGoodsId = new Guid(goodsOrderGridView.Rows[m].Cells["ItemID"].Value.ToString());
                                                decimal tempTotalPrice = Convert.ToDecimal(goodsOrderGridView.Rows[m].Cells["GoodsPrice"].Value);
                                                if (GoodsUtil.IsGoodsInGroup(tempGoodsId, item.ItemID))
                                                {
                                                    if (count % 3 == 0)
                                                    {
                                                        count++;
                                                        continue;
                                                    }
                                                    if (count % 3 == 1)
                                                    {
                                                        Discount discount = new Discount();
                                                        discount.DiscountID = new Guid("66666666-6666-6666-6666-666666666666");
                                                        discount.DiscountName = "促销折扣";
                                                        decimal goodsDiscount = 0;
                                                        if (item.DiscountType2 == (int)DiscountItemType.DiscountRate)
                                                        {
                                                            goodsDiscount = tempTotalPrice * item.DiscountRate2;
                                                            discount.DiscountType = (int)DiscountItemType.DiscountRate;
                                                            discount.DiscountRate = item.DiscountRate2;
                                                            discount.OffFixPay = 0;
                                                        }
                                                        if (item.DiscountType2 == (int)DiscountItemType.OffFixPay)
                                                        {
                                                            goodsDiscount = item.OffFixPay2;
                                                            discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                            discount.DiscountRate = 0;
                                                            discount.OffFixPay = item.OffFixPay2;
                                                        }
                                                        if (item.DiscountType2 == (int)DiscountItemType.OffSaleTo)
                                                        {
                                                            goodsDiscount = tempTotalPrice - item.OffSaleTo2;
                                                            discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                            discount.DiscountRate = 0;
                                                            discount.OffFixPay = tempTotalPrice - item.OffSaleTo2;
                                                        }
                                                        goodsOrderGridView.Rows[m].Cells["GoodsDiscount"].Value = (-goodsDiscount).ToString("f2");
                                                        goodsOrderGridView.Rows[m].Cells["GoodsDiscount"].Tag = discount;
                                                        count++;
                                                        continue;
                                                    }
                                                    if (count % 3 == 2)
                                                    {
                                                        Discount discount = new Discount();
                                                        discount.DiscountID = new Guid("66666666-6666-6666-6666-666666666666");
                                                        discount.DiscountName = "促销折扣";
                                                        decimal goodsDiscount = 0;
                                                        if (item.DiscountType3 == (int)DiscountItemType.DiscountRate)
                                                        {
                                                            goodsDiscount = tempTotalPrice * item.DiscountRate3;
                                                            discount.DiscountType = (int)DiscountItemType.DiscountRate;
                                                            discount.DiscountRate = item.DiscountRate3;
                                                            discount.OffFixPay = 0;
                                                        }
                                                        if (item.DiscountType3 == (int)DiscountItemType.OffFixPay)
                                                        {
                                                            goodsDiscount = item.OffFixPay3;
                                                            discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                            discount.DiscountRate = 0;
                                                            discount.OffFixPay = item.OffFixPay3;
                                                        }
                                                        if (item.DiscountType3 == (int)DiscountItemType.OffSaleTo)
                                                        {
                                                            goodsDiscount = tempTotalPrice - item.OffSaleTo3;
                                                            discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                            discount.DiscountRate = 0;
                                                            discount.OffFixPay = tempTotalPrice - item.OffSaleTo3;
                                                        }
                                                        goodsOrderGridView.Rows[m].Cells["GoodsDiscount"].Value = (-goodsDiscount).ToString("f2");
                                                        goodsOrderGridView.Rows[m].Cells["GoodsDiscount"].Tag = discount;
                                                        if (item.IsMultiple)
                                                        {
                                                            count++;
                                                        }
                                                        else
                                                        {
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    hasExistGoodsId.Add(item.ItemID);
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
