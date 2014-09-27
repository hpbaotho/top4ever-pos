using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.ClientService.Enum;
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
        private FormOrder m_FormOrder = null;
        private FormTakeout m_FormTakeout = null;

        private Guid m_CurrentRegionID = Guid.Empty;
        private ButtonOperateType m_OperateType = ButtonOperateType.NONE;
        private CrystalButton prevPressedButton = null;
        private CrystalButton prevRegionButton = null;
        private bool currentFormActivate = false;
        private int threadSleepTime = 1000;
        private Dictionary<Guid, List<CrystalButton>> dicDeskInRegion = new Dictionary<Guid, List<CrystalButton>>();
        private bool haveDailyClose;

        #region 转台局部变量
        private string deskName1st = string.Empty;
        private Guid orderID1st = Guid.Empty;
        private Guid orderID2nd = Guid.Empty;
        private bool firstDeskSingleOrder = false;
        #endregion

        public FormDesk(bool haveDailyClose)
        {
            this.haveDailyClose = haveDailyClose;
            m_FormOrder = new FormOrder();
            if (ConstantValuePool.BizSettingConfig.SaleType == ShopSaleType.DineInAndTakeout)
            {
                m_FormTakeout = new FormTakeout(haveDailyClose);
            }
            InitializeComponent();
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
                if (haveDailyClose)
                {
                    btnOrder.BackColor = ConstantValuePool.PressedColor;
                    m_OperateType = ButtonOperateType.ORDER;
                    prevPressedButton = btnOrder;
                }
                else
                {
                    btnOrder.Enabled = false;
                    btnOrder.BackColor = ConstantValuePool.DisabledColor;
                    m_OperateType = ButtonOperateType.NONE;
                }
            }
            else
            {
                btnOrder.Enabled = false;
                btnOrder.BackColor = ConstantValuePool.DisabledColor;
                m_OperateType = ButtonOperateType.NONE;
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

            Thread backWork = new Thread(new ThreadStart(doWork));
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
                        float emSize = (float)btnStyle.FontSize;
                        FontStyle style = FontStyle.Regular;
                        btn.Font = new Font(btnStyle.FontName, emSize, style);
                        btn.ForeColor = ColorConvert.RGB(btnStyle.ForeColor);
                        btn.BackColor = btn.DisplayColor = ColorConvert.RGB(btnStyle.BackColor);
                        break;
                    }
                }
                btn.Click += new System.EventHandler(this.btnRegion_Click);
                if (prevRegionButton == null)
                {
                    prevRegionButton = btn;
                    prevRegionButton.BackColor = ConstantValuePool.PressedColor;
                }
                this.pnlRegion.Controls.Add(btn);
            }
            //动态加载第一区域的桌况信息
            BizRegion firstRegion =  ConstantValuePool.RegionList[0];
            m_CurrentRegionID = firstRegion.RegionID;
            if (!dicDeskInRegion.ContainsKey(m_CurrentRegionID))
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
                    btn.Font = new Font("Arial", 14.25F, FontStyle.Regular);
                    btn.ForeColor = Color.White;
                    btn.BackColor = GetColorByStatus(desk.Status, desk.DeviceNo);
                    btn.Click += new System.EventHandler(this.btnDesk_Click);
                    btnList.Add(btn);
                }
                dicDeskInRegion.Add(firstRegion.RegionID, btnList);
            }
            foreach (CrystalButton btn in dicDeskInRegion[firstRegion.RegionID])
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
            currentFormActivate = true;
            base.OnActivated(e);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            currentFormActivate = false;
            base.OnDeactivate(e);
        }

        private void doWork()
        {
            while (true)
            {
                if (currentFormActivate)
                {
                    if (dicDeskInRegion.ContainsKey(m_CurrentRegionID))
                    {
                        IList<DeskRealTimeInfo> deskInfoList = DeskService.GetInstance().GetDeskRealTimeInfo(m_CurrentRegionID.ToString());
                        List<CrystalButton> btnDeskList = dicDeskInRegion[m_CurrentRegionID];
                        foreach (CrystalButton btnDesk in btnDeskList)
                        {
                            BizDesk desk = btnDesk.Tag as BizDesk;
                            bool IsContains = false;
                            foreach (DeskRealTimeInfo deskInfo in deskInfoList)
                            {
                                if (desk.DeskName == deskInfo.DeskName)
                                {
                                    IsContains = true;
                                    //更新状态
                                    desk.Status = deskInfo.DeskStatus;
                                    desk.DeviceNo = deskInfo.DeviceNo;
                                    UpdateDeskButtonInfo(btnDesk, deskInfo);
                                    break;
                                }
                            }
                            if (!IsContains)
                            {
                                UpdateDeskButtonInfo(btnDesk, null);
                            }
                        }
                    }
                }
                Thread.Sleep(threadSleepTime);
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
                if (currentFormActivate)
                {
                    if (deskInfo == null)
                    {
                        BizDesk desk = btnDesk.Tag as BizDesk;
                        desk.Status = (int)DeskButtonStatus.IDLE_MODE;
                        desk.DeviceNo = string.Empty;
                        btnDesk.BackColor = GetColorByStatus(desk.Status, desk.DeviceNo);
                        btnDesk.Text = desk.DeskName;
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
            currentFormActivate = false;
            lock (dicDeskInRegion)
            {
                //禁止引发Layout事件
                this.pnlDesk.SuspendLayout();
                this.SuspendLayout();
                //清除pnlDesk内所有控件
                this.pnlDesk.Controls.Clear();
                //获取Desk List
                CrystalButton btnRegion = sender as CrystalButton;
                prevRegionButton.BackColor = prevRegionButton.DisplayColor;
                btnRegion.BackColor = ConstantValuePool.PressedColor;
                prevRegionButton = btnRegion;
                BizRegion region = btnRegion.Tag as BizRegion;
                m_CurrentRegionID = region.RegionID;
                if (!dicDeskInRegion.ContainsKey(m_CurrentRegionID))
                {
                    List<CrystalButton> btnList = new List<CrystalButton>();
                    foreach (BizDesk desk in region.BizDeskList)
                    {
                        CrystalButton btn = new CrystalButton();
                        btn.Name = desk.DeskID.ToString();
                        btn.Text = desk.DeskName;
                        btn.Width = desk.Width;
                        btn.Height = desk.Height;
                        btn.Location = new Point(desk.PX, desk.PY);
                        btn.Tag = desk;
                        btn.Font = new Font("Arial", 14.25F, FontStyle.Regular);
                        btn.ForeColor = Color.White;
                        btn.BackColor = GetColorByStatus(desk.Status, desk.DeviceNo);
                        btn.Click += new System.EventHandler(this.btnDesk_Click);
                        btnList.Add(btn);
                    }
                    dicDeskInRegion.Add(region.RegionID, btnList);
                }
                foreach (CrystalButton btn in dicDeskInRegion[region.RegionID])
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
            currentFormActivate = true;
        }

        private void btnDesk_Click(object sender, EventArgs e)
        {
            if (m_OperateType == ButtonOperateType.NONE)
            {
                return;
            }
            currentFormActivate = false;
            CrystalButton btnDesk = sender as CrystalButton;
            BizDesk tempDesk = btnDesk.Tag as BizDesk;
            //重新获取Desk信息
            BizDesk desk = DeskService.GetInstance().GetBizDeskByName(tempDesk.DeskName);
            if (m_OperateType == ButtonOperateType.ORDER)
            {
                if (desk.Status == (int)DeskButtonStatus.IDLE_MODE)
                {
                    //人数
                    Feature.FormNumericKeypad keyForm = new Feature.FormNumericKeypad();
                    keyForm.DisplayText = "请输入就餐人数";
                    keyForm.ShowDialog();
                    if (!string.IsNullOrEmpty(keyForm.KeypadValue) && keyForm.KeypadValue != "0" && keyForm.KeypadValue.IndexOf('.') == -1)
                    {
                        m_FormOrder.PersonNum = int.Parse(keyForm.KeypadValue);
                    }
                    else
                    {
                        return;
                    }
                    //更新桌况为占用状态
                    int status = (int)DeskButtonStatus.OCCUPIED;
                    if (DeskService.GetInstance().UpdateDeskStatus(desk.DeskName, ConstantValuePool.BizSettingConfig.DeviceNo, status))
                    {
                        desk.Status = status;
                        btnDesk.BackColor = GetColorByStatus(status, ConstantValuePool.BizSettingConfig.DeviceNo);
                        m_FormOrder.CurrentDeskName = desk.DeskName;
                        m_FormOrder.PlaceSalesOrder = null;
                        m_FormOrder.VisibleShow = true;
                        m_FormOrder.Show();
                    }
                }
                else if (desk.Status == (int)DeskButtonStatus.OCCUPIED)
                {
                    if (desk.DeviceNo == ConstantValuePool.BizSettingConfig.DeviceNo || string.IsNullOrEmpty(desk.DeviceNo))
                    {
                        //更新桌况为占用状态
                        int status = (int)DeskButtonStatus.OCCUPIED;
                        if (DeskService.GetInstance().UpdateDeskStatus(desk.DeskName, ConstantValuePool.BizSettingConfig.DeviceNo, status))
                        {
                            //获取桌子的订单列表
                            IList<Order> orderList = OrderService.GetInstance().GetOrderList(desk.DeskName);
                            if (orderList != null && orderList.Count > 0)
                            {
                                SalesOrder salesOrder = null;
                                if (orderList.Count == 1)
                                {
                                    Guid orderID = orderList[0].OrderID;
                                    salesOrder = SalesOrderService.GetInstance().GetSalesOrder(orderID);
                                }
                                else
                                {
                                    Feature.FormChoseMultiOrder form = new Feature.FormChoseMultiOrder(orderList);
                                    form.ShowDialog();
                                    if (form.SelectedOrder != null)
                                    {
                                        Guid orderID = form.SelectedOrder.OrderID;
                                        salesOrder = SalesOrderService.GetInstance().GetSalesOrder(orderID);
                                    }
                                }
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
                                        m_FormOrder.CurrentDeskName = desk.DeskName;
                                        m_FormOrder.PlaceSalesOrder = salesOrder;
                                        m_FormOrder.VisibleShow = true;
                                        m_FormOrder.Show();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (m_OperateType == ButtonOperateType.CLEAR)
            {
                if (desk.Status == (int)DeskButtonStatus.OCCUPIED && !string.IsNullOrEmpty(desk.DeviceNo))
                {
                    //更新桌况为非占用状态
                    int status = (int)DeskButtonStatus.OCCUPIED;
                    if (DeskService.GetInstance().UpdateDeskStatus(desk.DeskName, string.Empty, status))
                    {
                        btnDesk.BackColor = GetColorByStatus(status, string.Empty);
                    }
                }
            }
            else if (m_OperateType == ButtonOperateType.CHANGE_DESK)
            {
                if (string.IsNullOrEmpty(deskName1st))
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
                                deskName1st = desk.DeskName;
                                orderID1st = form.SelectedOrder.OrderID;
                                firstDeskSingleOrder = false;
                            }
                            else
                            {
                                currentFormActivate = true; //使线程重新活跃
                                return;
                            }
                        }
                        else
                        {
                            deskName1st = desk.DeskName;
                            orderID1st = orderList[0].OrderID;
                            firstDeskSingleOrder = true;
                        }
                    }
                    else
                    {
                        currentFormActivate = true; //使线程重新活跃
                        return; //空桌
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(deskName1st))
                    {
                        if (desk.DeskName == deskName1st)
                        {
                            currentFormActivate = true; //使线程重新活跃
                            return; //点击相同的第一张桌子
                        }
                        //获取桌子的订单列表
                        IList<Order> orderList = OrderService.GetInstance().GetOrderList(desk.DeskName);
                        if (orderList != null && orderList.Count > 0)
                        {
                            DeskChange deskChange = new DeskChange();
                            deskChange.DeskName = desk.DeskName;
                            deskChange.OrderID1st = orderID1st;
                            deskChange.OrderID2nd = Guid.Empty;
                            Feature.FormChoseMultiOrder form = new Feature.FormChoseMultiOrder(orderList, deskChange);
                            form.ShowDialog();
                            if (form.SelectedOrder != null)
                            {
                                int status = 0;
                                if (firstDeskSingleOrder)
                                {
                                    //更新桌况为空闲状态
                                    status = (int)DeskButtonStatus.IDLE_MODE;
                                    if (!DeskService.GetInstance().UpdateDeskStatus(deskName1st, string.Empty, status))
                                    {
                                        MessageBox.Show("更新桌况失败！");
                                    }
                                }
                                status = (int)DeskButtonStatus.OCCUPIED;
                                if (!DeskService.GetInstance().UpdateDeskStatus(desk.DeskName, ConstantValuePool.BizSettingConfig.DeviceNo, status))
                                {
                                    MessageBox.Show("更新桌况失败！");
                                }

                                deskName1st = string.Empty;
                                orderID1st = Guid.Empty;
                                orderID2nd = Guid.Empty;
                                firstDeskSingleOrder = false;
                            }
                            else
                            {
                                deskName1st = string.Empty;
                                orderID1st = Guid.Empty;
                                orderID2nd = Guid.Empty;
                                firstDeskSingleOrder = false;
                            }
                        }
                        else
                        {
                            //直接转台
                            DeskChange deskChange = new DeskChange();
                            deskChange.DeskName = desk.DeskName;
                            deskChange.OrderID1st = orderID1st;
                            deskChange.OrderID2nd = Guid.Empty;
                            if (OrderService.GetInstance().OrderDeskOperate(deskChange))
                            {
                                int status = 0;
                                if (firstDeskSingleOrder)
                                {
                                    //更新桌况为空闲状态
                                    status = (int)DeskButtonStatus.IDLE_MODE;
                                    if (!DeskService.GetInstance().UpdateDeskStatus(deskName1st, string.Empty, status))
                                    {
                                        MessageBox.Show("更新桌况失败！");
                                    }
                                }
                                status = (int)DeskButtonStatus.OCCUPIED;
                                if (!DeskService.GetInstance().UpdateDeskStatus(desk.DeskName, ConstantValuePool.BizSettingConfig.DeviceNo, status))
                                {
                                    MessageBox.Show("更新桌况失败！");
                                }

                                deskName1st = string.Empty;
                                orderID1st = Guid.Empty;
                                orderID2nd = Guid.Empty;
                                firstDeskSingleOrder = false;
                            }
                        }
                    }
                }
            }
            else if (m_OperateType == ButtonOperateType.CHECKOUT)
            {
                if (desk.Status == (int)DeskButtonStatus.OCCUPIED)
                {
                    if (desk.DeviceNo == ConstantValuePool.BizSettingConfig.DeviceNo || string.IsNullOrEmpty(desk.DeviceNo))
                    {
                        //更新桌况为占用状态
                        int status = (int)DeskButtonStatus.OCCUPIED;
                        if (DeskService.GetInstance().UpdateDeskStatus(desk.DeskName, ConstantValuePool.BizSettingConfig.DeviceNo, status))
                        {
                            //获取桌子的订单列表
                            IList<Order> orderList = OrderService.GetInstance().GetOrderList(desk.DeskName);
                            if (orderList != null && orderList.Count > 0)
                            {
                                SalesOrder salesOrder = null;
                                if (orderList.Count == 1)
                                {
                                    Guid orderID = orderList[0].OrderID;
                                    salesOrder = SalesOrderService.GetInstance().GetSalesOrder(orderID);
                                }
                                else
                                {
                                    Feature.FormChoseMultiOrder form = new Feature.FormChoseMultiOrder(orderList);
                                    form.ShowDialog();
                                    if (form.SelectedOrder != null)
                                    {
                                        Guid orderID = form.SelectedOrder.OrderID;
                                        salesOrder = SalesOrderService.GetInstance().GetSalesOrder(orderID);
                                    }
                                }
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
            currentFormActivate = true;
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
            if (prevPressedButton != null)
            {
                prevPressedButton.BackColor = prevPressedButton.DisplayColor;
            }
            CrystalButton btn = sender as CrystalButton;
            btn.BackColor = ConstantValuePool.PressedColor;
            m_OperateType = ButtonOperateType.ORDER;
            prevPressedButton = btn;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (prevPressedButton != null)
            {
                prevPressedButton.BackColor = prevPressedButton.DisplayColor;
            }
            CrystalButton btn = sender as CrystalButton;
            btn.BackColor = ConstantValuePool.PressedColor;
            m_OperateType = ButtonOperateType.CLEAR;
            prevPressedButton = btn;
        }

        private void btnTurnTable_Click(object sender, EventArgs e)
        {
            deskName1st = string.Empty;
            orderID1st = Guid.Empty;
            orderID2nd = Guid.Empty;
            firstDeskSingleOrder = false;
            if (prevPressedButton != null)
            {
                prevPressedButton.BackColor = prevPressedButton.DisplayColor;
            }
            CrystalButton btn = sender as CrystalButton;
            btn.BackColor = ConstantValuePool.PressedColor;
            m_OperateType = ButtonOperateType.CHANGE_DESK;
            prevPressedButton = btn;
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            if (prevPressedButton != null)
            {
                prevPressedButton.BackColor = prevPressedButton.DisplayColor;
            }
            CrystalButton btn = sender as CrystalButton;
            btn.BackColor = ConstantValuePool.PressedColor;
            m_OperateType = ButtonOperateType.CHECKOUT;
            prevPressedButton = btn;
        }

        private void btnTakeOut_Click(object sender, EventArgs e)
        {
            m_FormTakeout.VisibleShow = true;
            m_FormTakeout.Show();
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
