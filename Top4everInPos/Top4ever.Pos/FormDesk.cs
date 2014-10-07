using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Common;
using Top4ever.CustomControl;
using Top4ever.Domain;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;
using Top4ever.Entity.Enum;

namespace VechsoftPos
{
    public partial class FormDesk : Form
    {
        private readonly FormOrder _formOrder;
        private readonly FormTakeout _formTakeout;

        private Guid _currentRegionId = Guid.Empty;
        private ButtonOperateType _operateType = ButtonOperateType.NONE;
        private CrystalButton _prevPressedButton;
        private CrystalButton _prevRegionButton;
        private bool _currentFormActivate;
        private const int ThreadSleepTime = 1000;
        private readonly Dictionary<Guid, List<CrystalButton>> _dicDeskInRegion = new Dictionary<Guid, List<CrystalButton>>();
        private readonly bool _haveDailyClose;

        #region 转台局部变量
        private string _deskName1St = string.Empty;
        private Guid _orderId1St = Guid.Empty;
        private bool _firstDeskSingleOrder;
        #endregion

        public FormDesk(bool haveDailyClose)
        {
            this._haveDailyClose = haveDailyClose;
            _formOrder = new FormOrder();
            if (ConstantValuePool.BizSettingConfig.SaleType == ShopSaleType.DineInAndTakeout)
            {
                _formTakeout = new FormTakeout(haveDailyClose);
            }
            InitializeComponent();
            if (!string.IsNullOrEmpty(ConstantValuePool.BizSettingConfig.DeskImagePath) && File.Exists(ConstantValuePool.BizSettingConfig.DeskImagePath))
            {
                pnlDesk.BackgroundImage = Image.FromFile(ConstantValuePool.BizSettingConfig.DeskImagePath);
                pnlDesk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            }
        }

        private void FormDesk_Load(object sender, EventArgs e)
        {
            InitializeRegionDeskButton();
            const int space = 5;
            const int width = 75;
            const int height = 28;
            int px = this.pnlDesk.Width - 3 * (width + space);
            int py = this.pnlDesk.Height - height - space;
            this.btnFreeColor.Width = width;
            this.btnFreeColor.Height = height;
            this.btnFreeColor.Location = new Point(px, py);
            px += width + space;
            this.btnTakeColor.Width = width;
            this.btnTakeColor.Height = height;
            this.btnTakeColor.Location = new Point(px, py);
            px += width + space;
            this.btnLookColor.Width = width;
            this.btnLookColor.Height = height;
            this.btnLookColor.Location = new Point(px, py);

            this.pnlSysTools.Width = this.scrollingText1.Width = this.pnlToolBar.Width;
            int toolsBarNum = 6;
            if (ConstantValuePool.BizSettingConfig.SaleType == ShopSaleType.DineInAndTakeout)
            {
                toolsBarNum++;
            }
            else
            {
                btnTakeOut.Visible = false;
            }
            int btnWidth = this.pnlSysTools.Width / toolsBarNum;
            int btnHeight = this.pnlSysTools.Height;
            px = 0;
            py = 0;
            btnManager.Size = new System.Drawing.Size(btnWidth, btnHeight);
            btnManager.Location = new Point(px, py);
            px += btnWidth;
            btnOrder.Size = new System.Drawing.Size(btnWidth, btnHeight);
            btnOrder.Location = new Point(px, py);
            px += btnWidth;
            btnClear.Size = new System.Drawing.Size(btnWidth, btnHeight);
            btnClear.Location = new Point(px, py);
            px += btnWidth;
            btnTurnTable.Size = new System.Drawing.Size(btnWidth, btnHeight);
            btnTurnTable.Location = new Point(px, py);
            px += btnWidth;
            btnCheckOut.Size = new System.Drawing.Size(btnWidth, btnHeight);
            btnCheckOut.Location = new Point(px, py);
            if (ConstantValuePool.BizSettingConfig.SaleType == ShopSaleType.DineInAndTakeout)
            {
                px += btnWidth;
                btnTakeOut.Size = new System.Drawing.Size(btnWidth, btnHeight);
                btnTakeOut.Location = new Point(px, py);
            }
            px += btnWidth;
            btnExit.Size = new System.Drawing.Size(this.pnlSysTools.Width - px, btnHeight);
            btnExit.Location = new Point(px, py);
            btnOrder.DisplayColor = btnOrder.BackColor;
            btnClear.DisplayColor = btnClear.BackColor;
            btnTurnTable.DisplayColor = btnTurnTable.BackColor;
            btnCheckOut.DisplayColor = btnCheckOut.BackColor;

            if (RightsItemCode.FindRights(RightsItemCode.TAKEORDER))
            {
                if (_haveDailyClose)
                {
                    btnOrder.BackColor = ConstantValuePool.PressedColor;
                    _operateType = ButtonOperateType.ORDER;
                    _prevPressedButton = btnOrder;
                }
                else
                {
                    btnOrder.Enabled = false;
                    btnOrder.BackColor = ConstantValuePool.DisabledColor;
                    _operateType = ButtonOperateType.NONE;
                }
            }
            else
            {
                btnOrder.Enabled = false;
                btnOrder.BackColor = ConstantValuePool.DisabledColor;
                _operateType = ButtonOperateType.NONE;
            }
            if (!RightsItemCode.FindRights(RightsItemCode.CLEARDESK))
            {
                btnClear.Enabled = false;
                btnClear.BackColor = ConstantValuePool.DisabledColor;
            }
            if (!RightsItemCode.FindRights(RightsItemCode.TURNTABLE))
            {
                btnTurnTable.Enabled = false;
                btnTurnTable.BackColor = ConstantValuePool.DisabledColor;
            }
            if (!RightsItemCode.FindRights(RightsItemCode.CHECKOUT))
            {
                btnCheckOut.Enabled = false;
                btnCheckOut.BackColor = ConstantValuePool.DisabledColor;
            }
            string strNotice = string.Empty;
            foreach (Notice item in ConstantValuePool.NoticeList)
            {
                strNotice += item.NoticeContent + "\t\t\t";
            }
            if (!string.IsNullOrEmpty(strNotice))
            {
                scrollingText1.ScrollText = strNotice;
            }

            Thread backWork = new Thread(new ThreadStart(DoWork));
            backWork.IsBackground = true;
            backWork.Start();
        }

        private void InitializeRegionDeskButton()
        {
            //禁止引发Layout事件
            this.pnlRegion.SuspendLayout();
            this.pnlDesk.SuspendLayout();
            this.SuspendLayout();
            //动态加载区域控件
            foreach (BizRegion region in ConstantValuePool.RegionList)
            {
                CrystalButton btn = new CrystalButton();
                btn.Name = region.RegionID.ToString();
                btn.Text = region.RegionName;
                btn.Width = region.Width;
                btn.Height = region.Height;
                btn.Location = new Point(region.PX, region.PY);
                btn.Tag = region;
                foreach (ButtonStyle btnStyle in ConstantValuePool.ButtonStyleList)
                {
                    if (region.ButtonStyleID.Equals(btnStyle.ButtonStyleID))
                    {
                        btn.Font = new Font(btnStyle.FontName, btnStyle.FontSize, FontStyle.Regular);
                        btn.ForeColor = ColorConvert.RGB(btnStyle.ForeColor);
                        btn.BackColor = btn.DisplayColor = ColorConvert.RGB(btnStyle.BackColor);
                        break;
                    }
                }
                btn.Click += new System.EventHandler(this.btnRegion_Click);
                if (_prevRegionButton == null)
                {
                    _prevRegionButton = btn;
                    _prevRegionButton.BackColor = ConstantValuePool.PressedColor;
                }
                this.pnlRegion.Controls.Add(btn);
            }
            //动态加载第一区域的桌况信息
            BizRegion firstRegion =  ConstantValuePool.RegionList[0];
            _currentRegionId = firstRegion.RegionID;
            if (!_dicDeskInRegion.ContainsKey(_currentRegionId))
            {
                List<CrystalButton> btnList = new List<CrystalButton>();
                foreach (BizDesk desk in firstRegion.BizDeskList)
                {
                    CrystalButton btn = new CrystalButton();
                    btn.Name = desk.DeskID.ToString();
                    btn.Text = desk.DeskName;
                    btn.Width = desk.Width;
                    btn.Height = desk.Height;
                    btn.Location = new Point(desk.PX, desk.PY);
                    btn.Tag = desk;
                    btn.Font = new Font("Arial", ConstantValuePool.BizSettingConfig.FontSize, FontStyle.Regular);
                    btn.ForeColor = Color.White;
                    btn.BackColor = GetColorByStatus(desk.Status, desk.DeviceNo);
                    btn.Click += new System.EventHandler(this.btnDesk_Click);
                    btnList.Add(btn);
                }
                _dicDeskInRegion.Add(firstRegion.RegionID, btnList);
            }
            foreach (CrystalButton btn in _dicDeskInRegion[firstRegion.RegionID])
            {
                this.pnlDesk.Controls.Add(btn);
            }
            //恢复引发Layout事件
            this.pnlRegion.ResumeLayout(false);
            this.pnlRegion.PerformLayout();
            this.pnlDesk.ResumeLayout(false);
            this.pnlDesk.PerformLayout();
            this.ResumeLayout(false);
        }

        protected override void OnActivated(EventArgs e)
        {
            _currentFormActivate = true;
            base.OnActivated(e);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            _currentFormActivate = false;
            base.OnDeactivate(e);
        }

        private void DoWork()
        {
            while (true)
            {
                if (_currentFormActivate)
                {
                    if (_dicDeskInRegion.ContainsKey(_currentRegionId))
                    {
                        IList<DeskRealTimeInfo> deskInfoList = DeskService.GetInstance().GetDeskRealTimeInfo(_currentRegionId.ToString());
                        List<CrystalButton> btnDeskList = _dicDeskInRegion[_currentRegionId];
                        foreach (CrystalButton btnDesk in btnDeskList)
                        {
                            BizDesk desk = btnDesk.Tag as BizDesk;
                            if(desk == null) continue;
                            bool isContains = false;
                            foreach (DeskRealTimeInfo deskInfo in deskInfoList)
                            {
                                if (desk.DeskName.Equals(deskInfo.DeskName, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    isContains = true;
                                    //更新状态
                                    desk.Status = deskInfo.DeskStatus;
                                    desk.DeviceNo = deskInfo.DeviceNo;
                                    UpdateDeskButtonInfo(btnDesk, deskInfo);
                                    break;
                                }
                            }
                            if (!isContains)
                            {
                                UpdateDeskButtonInfo(btnDesk, null);
                            }
                        }
                    }
                }
                Thread.Sleep(ThreadSleepTime);
            }
        }

        public delegate void DelegateUpdateDeskButton(CrystalButton btnDesk, DeskRealTimeInfo deskInfo);
        private void UpdateDeskButtonInfo(CrystalButton btnDesk, DeskRealTimeInfo deskInfo)
        {
            if (btnDesk.InvokeRequired)
            {
                DelegateUpdateDeskButton myDelegate = new DelegateUpdateDeskButton(UpdateDeskButtonInfo);
                btnDesk.Invoke(myDelegate, new object[] { btnDesk, deskInfo });
            }
            else
            {
                if (_currentFormActivate)
                {
                    if (deskInfo == null)
                    {
                        BizDesk desk = btnDesk.Tag as BizDesk;
                        if (desk != null)
                        {
                            desk.Status = (int) DeskButtonStatus.IDLE_MODE;
                            desk.DeviceNo = string.Empty;
                            btnDesk.BackColor = GetColorByStatus(desk.Status, desk.DeviceNo);
                            btnDesk.Text = desk.DeskName;
                        }
                    }
                    else
                    {
                        btnDesk.BackColor = GetColorByStatus(deskInfo.DeskStatus, deskInfo.DeviceNo);
                        if (deskInfo.IsSplitOrder)
                        {
                            btnDesk.Text = "**";
                        }
                        else
                        {
                            btnDesk.Text = deskInfo.DeskName + "\n" + deskInfo.PeopleNum + "\n" + deskInfo.ConsumptionMoney.ToString("N");
                        }
                    }
                }
            }
        }

        private void btnRegion_Click(object sender, EventArgs e)
        {
            CrystalButton btnRegion = sender as CrystalButton;
            if (btnRegion == null) return;
            BizRegion region = btnRegion.Tag as BizRegion;
            if (region == null) return;

            _currentFormActivate = false;
            lock (_dicDeskInRegion)
            {
                //禁止引发Layout事件
                this.pnlDesk.SuspendLayout();
                this.SuspendLayout();
                //清除pnlDesk内所有控件
                this.pnlDesk.Controls.Clear();
                //获取Desk List
                _prevRegionButton.BackColor = _prevRegionButton.DisplayColor;
                btnRegion.BackColor = ConstantValuePool.PressedColor;
                _prevRegionButton = btnRegion;

                _currentRegionId = region.RegionID;
                if (!_dicDeskInRegion.ContainsKey(_currentRegionId))
                {
                    List<CrystalButton> btnList = new List<CrystalButton>();
                    if (region.BizDeskList != null && region.BizDeskList.Count > 0)
                    {
                        foreach (BizDesk desk in region.BizDeskList)
                        {
                            CrystalButton btn = new CrystalButton();
                            btn.Name = desk.DeskID.ToString();
                            btn.Text = desk.DeskName;
                            btn.Width = desk.Width;
                            btn.Height = desk.Height;
                            btn.Location = new Point(desk.PX, desk.PY);
                            btn.Tag = desk;
                            btn.Font = new Font("Arial", ConstantValuePool.BizSettingConfig.FontSize, FontStyle.Regular);
                            btn.ForeColor = Color.White;
                            btn.BackColor = GetColorByStatus(desk.Status, desk.DeviceNo);
                            btn.Click += new System.EventHandler(this.btnDesk_Click);
                            btnList.Add(btn);
                        }
                    }
                    _dicDeskInRegion.Add(region.RegionID, btnList);
                }
                foreach (CrystalButton btn in _dicDeskInRegion[region.RegionID])
                {
                    this.pnlDesk.Controls.Add(btn);
                }
                this.pnlDesk.Controls.Add(btnFreeColor);
                this.pnlDesk.Controls.Add(btnTakeColor);
                this.pnlDesk.Controls.Add(btnLookColor);
                //恢复引发Layout事件
                this.pnlDesk.ResumeLayout(false);
                this.pnlDesk.PerformLayout();
                this.ResumeLayout(false);
            }
            _currentFormActivate = true;
        }

        private void btnDesk_Click(object sender, EventArgs e)
        {
            if (_operateType == ButtonOperateType.NONE)
            {
                return;
            }
            CrystalButton btnDesk = sender as CrystalButton;
            if (btnDesk == null) return;
            BizDesk tempDesk = btnDesk.Tag as BizDesk;
            if (tempDesk == null) return;

            _currentFormActivate = false;
            //重新获取Desk信息
            BizDesk desk = DeskService.GetInstance().GetBizDeskByName(tempDesk.DeskName);
            if (_operateType == ButtonOperateType.ORDER)
            {
                if (desk.Status == (int)DeskButtonStatus.IDLE_MODE)
                {
                    //人数
                    Feature.FormNumericKeypad keyForm = new Feature.FormNumericKeypad();
                    keyForm.DisplayText = "请输入就餐人数";
                    keyForm.ShowDialog();
                    if (!string.IsNullOrEmpty(keyForm.KeypadValue) && keyForm.KeypadValue != "0" && keyForm.KeypadValue.IndexOf('.') == -1)
                    {
                        _formOrder.PersonNum = int.Parse(keyForm.KeypadValue);
                    }
                    else
                    {
                        return;
                    }
                    //更新桌况为占用状态
                    const int status = (int)DeskButtonStatus.OCCUPIED;
                    if (DeskService.GetInstance().UpdateDeskStatus(desk.DeskName, ConstantValuePool.BizSettingConfig.DeviceNo, status))
                    {
                        desk.Status = status;
                        btnDesk.BackColor = GetColorByStatus(status, ConstantValuePool.BizSettingConfig.DeviceNo);
                        _formOrder.CurrentDeskName = desk.DeskName;
                        _formOrder.PlaceSalesOrder = null;
                        _formOrder.VisibleShow = true;
                        _formOrder.Show();
                    }
                }
                else if (desk.Status == (int)DeskButtonStatus.OCCUPIED)
                {
                    if (string.IsNullOrEmpty(desk.DeviceNo) || desk.DeviceNo == ConstantValuePool.BizSettingConfig.DeviceNo)
                    {
                        //更新桌况为占用状态
                        const int status = (int)DeskButtonStatus.OCCUPIED;
                        if (DeskService.GetInstance().UpdateDeskStatus(desk.DeskName, ConstantValuePool.BizSettingConfig.DeviceNo, status))
                        {
                            //获取桌子的订单列表
                            IList<Order> orderList = OrderService.GetInstance().GetOrderList(desk.DeskName);
                            if (orderList != null && orderList.Count > 0)
                            {
                                Guid orderId = Guid.Empty;
                                if (orderList.Count == 1)
                                {
                                    orderId = orderList[0].OrderID;
                                }
                                else
                                {
                                    Feature.FormChoseMultiOrder form = new Feature.FormChoseMultiOrder(orderList);
                                    form.ShowDialog();
                                    if (form.SelectedOrder != null)
                                    {
                                        orderId = form.SelectedOrder.OrderID;
                                    }
                                }
                                SalesOrder salesOrder = SalesOrderService.GetInstance().GetSalesOrder(orderId);
                                if (salesOrder != null)
                                {
                                    if (salesOrder.order.Status == 3)   //已预结
                                    {
                                        //open check out form
                                        FormCheckOut checkForm = new FormCheckOut(salesOrder, desk.DeskName);
                                        checkForm.ShowDialog();
                                    }
                                    else
                                    {
                                        //open order form
                                        _formOrder.CurrentDeskName = desk.DeskName;
                                        _formOrder.PlaceSalesOrder = salesOrder;
                                        _formOrder.VisibleShow = true;
                                        _formOrder.Show();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (_operateType == ButtonOperateType.CLEAR)
            {
                if (desk.Status == (int)DeskButtonStatus.OCCUPIED && !string.IsNullOrEmpty(desk.DeviceNo))
                {
                    //更新桌况为非占用状态
                    const int status = (int)DeskButtonStatus.OCCUPIED;
                    if (DeskService.GetInstance().UpdateDeskStatus(desk.DeskName, string.Empty, status))
                    {
                        btnDesk.BackColor = GetColorByStatus(status, string.Empty);
                    }
                }
            }
            else if (_operateType == ButtonOperateType.CHANGE_DESK)
            {
                if (string.IsNullOrEmpty(_deskName1St))
                {
                    //获取桌子的订单列表
                    IList<Order> orderList = OrderService.GetInstance().GetOrderList(desk.DeskName);
                    if (orderList != null && orderList.Count > 0)
                    {
                        if (orderList.Count > 1)
                        {
                            Feature.FormChoseMultiOrder form = new Feature.FormChoseMultiOrder(orderList);
                            form.ShowDialog();
                            if (form.SelectedOrder != null)
                            {
                                _deskName1St = desk.DeskName;
                                _orderId1St = form.SelectedOrder.OrderID;
                                _firstDeskSingleOrder = false;
                            }
                            else
                            {
                                _currentFormActivate = true; //使线程重新活跃
                                return;
                            }
                        }
                        else
                        {
                            _deskName1St = desk.DeskName;
                            _orderId1St = orderList[0].OrderID;
                            _firstDeskSingleOrder = true;
                        }
                    }
                    else
                    {
                        _currentFormActivate = true; //使线程重新活跃
                        return; //空桌
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(_deskName1St))
                    {
                        if (desk.DeskName == _deskName1St)
                        {
                            _currentFormActivate = true; //使线程重新活跃
                            return; //点击相同的第一张桌子
                        }
                        //获取桌子的订单列表
                        IList<Order> orderList = OrderService.GetInstance().GetOrderList(desk.DeskName);
                        if (orderList != null && orderList.Count > 0)
                        {
                            DeskChange deskChange = new DeskChange();
                            deskChange.DeskName = desk.DeskName;
                            deskChange.OrderID1st = _orderId1St;
                            deskChange.OrderID2nd = Guid.Empty;
                            Feature.FormChoseMultiOrder form = new Feature.FormChoseMultiOrder(orderList, deskChange);
                            form.ShowDialog();
                            if (form.SelectedOrder != null)
                            {
                                int status = 0;
                                if (_firstDeskSingleOrder)
                                {
                                    //更新桌况为空闲状态
                                    status = (int)DeskButtonStatus.IDLE_MODE;
                                    if (!DeskService.GetInstance().UpdateDeskStatus(_deskName1St, string.Empty, status))
                                    {
                                        MessageBox.Show("更新桌况失败！");
                                    }
                                }
                                status = (int)DeskButtonStatus.OCCUPIED;
                                if (!DeskService.GetInstance().UpdateDeskStatus(desk.DeskName, ConstantValuePool.BizSettingConfig.DeviceNo, status))
                                {
                                    MessageBox.Show("更新桌况失败！");
                                }

                                _deskName1St = string.Empty;
                                _orderId1St = Guid.Empty;
                                _firstDeskSingleOrder = false;
                            }
                            else
                            {
                                _deskName1St = string.Empty;
                                _orderId1St = Guid.Empty;
                                _firstDeskSingleOrder = false;
                            }
                        }
                        else
                        {
                            //直接转台
                            DeskChange deskChange = new DeskChange();
                            deskChange.DeskName = desk.DeskName;
                            deskChange.OrderID1st = _orderId1St;
                            deskChange.OrderID2nd = Guid.Empty;
                            if (OrderService.GetInstance().OrderDeskOperate(deskChange))
                            {
                                int status = 0;
                                if (_firstDeskSingleOrder)
                                {
                                    //更新桌况为空闲状态
                                    status = (int)DeskButtonStatus.IDLE_MODE;
                                    if (!DeskService.GetInstance().UpdateDeskStatus(_deskName1St, string.Empty, status))
                                    {
                                        MessageBox.Show("更新桌况失败！");
                                    }
                                }
                                status = (int)DeskButtonStatus.OCCUPIED;
                                if (!DeskService.GetInstance().UpdateDeskStatus(desk.DeskName, ConstantValuePool.BizSettingConfig.DeviceNo, status))
                                {
                                    MessageBox.Show("更新桌况失败！");
                                }

                                _deskName1St = string.Empty;
                                _orderId1St = Guid.Empty;
                                _firstDeskSingleOrder = false;
                            }
                        }
                    }
                }
            }
            else if (_operateType == ButtonOperateType.CHECKOUT)
            {
                if (desk.Status == (int)DeskButtonStatus.OCCUPIED)
                {
                    if (string.IsNullOrEmpty(desk.DeviceNo) || desk.DeviceNo == ConstantValuePool.BizSettingConfig.DeviceNo)
                    {
                        //更新桌况为占用状态
                        const int status = (int)DeskButtonStatus.OCCUPIED;
                        if (DeskService.GetInstance().UpdateDeskStatus(desk.DeskName, ConstantValuePool.BizSettingConfig.DeviceNo, status))
                        {
                            //获取桌子的订单列表
                            IList<Order> orderList = OrderService.GetInstance().GetOrderList(desk.DeskName);
                            if (orderList != null && orderList.Count > 0)
                            {
                                Guid orderId = Guid.Empty;
                                if (orderList.Count == 1)
                                {
                                    orderId = orderList[0].OrderID;
                                }
                                else
                                {
                                    Feature.FormChoseMultiOrder form = new Feature.FormChoseMultiOrder(orderList);
                                    form.ShowDialog();
                                    if (form.SelectedOrder != null)
                                    {
                                        orderId = form.SelectedOrder.OrderID;

                                    }
                                }
                                SalesOrder salesOrder = SalesOrderService.GetInstance().GetSalesOrder(orderId);
                                if (salesOrder != null)
                                {
                                    FormCheckOut checkForm = new FormCheckOut(salesOrder, desk.DeskName);
                                    checkForm.ShowDialog();
                                }
                            }
                        }
                    }
                }
            }
            _currentFormActivate = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnManager_Click(object sender, EventArgs e)
        {
            Feature.FormFunctionPanel form = new Feature.FormFunctionPanel();
            form.ShowDialog();
            if (form.IsNeedExist)
            {
                this.Close();
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            if (_prevPressedButton != null)
            {
                _prevPressedButton.BackColor = _prevPressedButton.DisplayColor;
            }
            CrystalButton btn = sender as CrystalButton;
            if(btn == null) return;
            btn.BackColor = ConstantValuePool.PressedColor;
            _operateType = ButtonOperateType.ORDER;
            _prevPressedButton = btn;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (_prevPressedButton != null)
            {
                _prevPressedButton.BackColor = _prevPressedButton.DisplayColor;
            }
            CrystalButton btn = sender as CrystalButton;
            if (btn == null) return;
            btn.BackColor = ConstantValuePool.PressedColor;
            _operateType = ButtonOperateType.CLEAR;
            _prevPressedButton = btn;
        }

        private void btnTurnTable_Click(object sender, EventArgs e)
        {
            _deskName1St = string.Empty;
            _orderId1St = Guid.Empty;
            _firstDeskSingleOrder = false;
            if (_prevPressedButton != null)
            {
                _prevPressedButton.BackColor = _prevPressedButton.DisplayColor;
            }
            CrystalButton btn = sender as CrystalButton;
            if (btn == null) return;
            btn.BackColor = ConstantValuePool.PressedColor;
            _operateType = ButtonOperateType.CHANGE_DESK;
            _prevPressedButton = btn;
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            if (_prevPressedButton != null)
            {
                _prevPressedButton.BackColor = _prevPressedButton.DisplayColor;
            }
            CrystalButton btn = sender as CrystalButton;
            if (btn == null) return;
            btn.BackColor = ConstantValuePool.PressedColor;
            _operateType = ButtonOperateType.CHECKOUT;
            _prevPressedButton = btn;
        }

        private void btnTakeOut_Click(object sender, EventArgs e)
        {
            _formTakeout.VisibleShow = true;
            _formTakeout.Show();
        }

        private Color GetColorByStatus(int deskStatus, string deviceNo)
        {
            Color backColor = Color.White;
            if (deskStatus == (int)DeskButtonStatus.IDLE_MODE && string.IsNullOrEmpty(deviceNo))
            {
                backColor = btnFreeColor.BackColor;
            }
            else
            {
                if (deskStatus == (int)DeskButtonStatus.OCCUPIED)
                {
                    if (ConstantValuePool.BizSettingConfig.DeviceNo == deviceNo || string.IsNullOrEmpty(deviceNo))
                    {
                        backColor = btnTakeColor.BackColor;
                    }
                    else
                    {
                        backColor = btnLookColor.BackColor;
                    }
                }
            }
            return backColor;
        }

        private void scrollingText1_TextClicked(object sender, EventArgs args)
        {

        }
    }
}
