using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Common;
using Top4ever.CustomControl;
using Top4ever.Domain;
using Top4ever.Domain.Customers;
using Top4ever.Domain.GoodsRelated;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;
using Top4ever.Entity.Enum;
using Top4ever.Print.Entity;
using Top4ever.Print;
using VechsoftPos.Feature;
using VechsoftPos.TakeawayCall;

namespace VechsoftPos
{
    public partial class FormTakeout : Form
    {
        private const int m_Space = 2;
        private List<CrystalButton> btnDeliveryList = new List<CrystalButton>();
        private List<CrystalButton> btnGroupList = new List<CrystalButton>();
        private List<CrystalButton> btnItemList = new List<CrystalButton>();
        //外卖单列表
        private const int m_PageSize = 10;
        private int m_PageIndex = 0;
        //品项组列表
        private int m_GroupPageSize = 0;
        private int m_GroupPageIndex = 0;
        //品项列表
        private int m_ItemPageSize = 0;
        private int m_ItemPageIndex = 0;
        private CrystalButton prevDeliveryButton = null;
        private IList<DeliveryOrder> m_DeliveryOrderList = new List<DeliveryOrder>();
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
        private CrystalButton prevPressedButton = null;
        private bool m_ShowSilverCode = false;
        private bool haveDailyClose;
        private bool currentFormActivate = false;
        private Thread backWork = null;

        private bool m_OnShow = false;
        public bool VisibleShow
        {
            set { m_OnShow = value; }
        }

        public FormTakeout(bool haveDailyClose)
        {
            this.haveDailyClose = haveDailyClose;
            InitializeComponent();
            btnPageUp.DisplayColor = btnPageUp.BackColor;
            btnPageDown.DisplayColor = btnPageDown.BackColor;
            btnHead.DisplayColor = btnHead.BackColor;
            btnBack.DisplayColor = btnBack.BackColor;
            btnPgUp.DisplayColor = btnPgUp.BackColor;
            btnPgDown.DisplayColor = btnPgDown.BackColor;
            btnDeliveryGoods.DisplayColor = btnDeliveryGoods.BackColor;
            btnOutsideOrder.DisplayColor = btnOutsideOrder.BackColor;
            btnDiscount.DisplayColor = btnDiscount.BackColor;
            btnWholeDiscount.DisplayColor = btnWholeDiscount.BackColor;
            btnTakeOut.DisplayColor = btnTakeOut.BackColor;
        }

        private void FormTakeout_Load(object sender, EventArgs e)
        {
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
            if (ConstantValuePool.BizSettingConfig.telCallConfig.Enabled && ConstantValuePool.IsTelCallWorking)
            {
                backWork = new Thread(new ThreadStart(doWork));
                backWork.IsBackground = true;
                backWork.Start();
            }
        }

        private void doWork()
        {
            if (ConstantValuePool.BizSettingConfig.telCallConfig.Enabled && ConstantValuePool.IsTelCallWorking)
            {
                while (true)
                {
                    if (currentFormActivate)
                    {
                        if (ConstantValuePool.IsTelCallWorking)
                        {
                            if (ConstantValuePool.BizSettingConfig.telCallConfig.Model == 0 || ConstantValuePool.BizSettingConfig.telCallConfig.Model == 1)
                            {
                                if (LDT.Check_State(ConstantValuePool.TelCallID) == 255)
                                {
                                    string strPhoneNo = LDT.GetNumber_Tel(1).ToString();
                                    if (strPhoneNo.Length > 0)
                                    {
                                        //创建通话记录
                                        CallRecord callRecord = new CallRecord();
                                        callRecord.CallRecordID = Guid.NewGuid();
                                        callRecord.Telephone = strPhoneNo;
                                        callRecord.CallTime = DateTime.Now;
                                        callRecord.Status = 0;
                                        CustomersService.GetInstance().CreateOrUpdateCallRecord(callRecord);
                                        //委托弹出窗口
                                        SetIncomingCall(strPhoneNo);
                                    }
                                }
                            }
                        }
                        Thread.Sleep(100);
                        if (ConstantValuePool.BizSettingConfig.telCallConfig.Model == 0)
                        {
                            ConstantValuePool.IsTelCallWorking = LDT.Plugin_Tel(ConstantValuePool.TelCallID);
                        }
                    }
                    Thread.Sleep(500);
                }
            }
        }

        public delegate void IncomingCallMessage(string strPhoneNo);
        private void SetIncomingCall(string strPhoneNo)
        {
            if (txtTelephone.InvokeRequired)
            {
                IncomingCallMessage _myInvoke = new IncomingCallMessage(SetIncomingCall);
                this.Invoke(_myInvoke, new object[] { strPhoneNo });
            }
            else
            {
                int callType = 1;  //程序
                FormIncomingCall form = new FormIncomingCall(strPhoneNo, string.Empty, callType);
                form.ShowDialog();
                if (!string.IsNullOrEmpty(form.IncomingPhoneNo))
                {
                    txtTelephone.Text = form.IncomingPhoneNo;
                }
                if (!string.IsNullOrEmpty(form.SelectedAddress))
                {
                    txtAddress.Text = form.SelectedAddress;
                }
                if (form.CurCustomerInfo != null)
                {
                    txtTelephone.Text = form.CurCustomerInfo.Telephone;
                    txtName.Text = form.CurCustomerInfo.CustomerName;
                }
            }
        }

        private void FormTakeout_VisibleChanged(object sender, EventArgs e)
        {
            if (m_OnShow)
            {
                this.dgvGoodsOrder.Rows.Clear();
                //初始化外卖单按钮
                InitDeliveryButton();
                //初始化品项组按钮
                InitializeGroupButton();
                //初始化品项按钮
                InitializeItemButton();
                //初始化
                LoadDefaultGoodsGroupButton();
                m_GoodsOrDetails = true;
                m_DetailsPrefix = string.Empty;
                prevDeliveryButton = null;
                prevPressedButton = null;
                m_ShowSilverCode = false;
                //清除
                txtTelephone.Text = string.Empty;
                txtName.Text = string.Empty;
                txtAddress.Text = string.Empty;
                btnDeliveryGoods.Enabled = false;
                btnDeliveryGoods.BackColor = ConstantValuePool.DisabledColor;
                //加载外卖单列表
                IList<DeliveryOrder> deliveryOrderList = OrderService.GetInstance().GetDeliveryOrderList();
                if (deliveryOrderList != null)
                {
                    m_PageIndex = 0;
                    m_DeliveryOrderList = deliveryOrderList;
                    DisplayDeliveryOrderButton();
                }
            }
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

        #region 初始化

        private void InitDeliveryButton()
        {
            if (btnDeliveryList.Count == 0)
            {
                int space = 2;
                int px = 0, py = space;
                int height = (this.pnlDelivery.Height - this.pnlPage.Height - (m_PageSize + 1) * space) / m_PageSize;
                for (int i = 0; i < m_PageSize; i++)
                {
                    CrystalButton btnDelivery = new CrystalButton();
                    btnDelivery.Name = "btnDelivery" + i;
                    btnDelivery.BackColor = btnDelivery.DisplayColor = Color.DodgerBlue;
                    btnDelivery.Font = new Font("Microsoft YaHei", 12F);
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

        private void InitializeGroupButton()
        {
            if (btnGroupList.Count == 0)
            {
                if (ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls != null && ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls.Count > 0)
                {
                    foreach (BizControl control in ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls)
                    {
                        if (control.Name == "Group")
                        {
                            int width = (this.pnlGroup.Width - m_Space * (control.ColumnsCount + 1)) / control.ColumnsCount;
                            int height = (this.pnlGroup.Height - m_Space * (control.RowsCount + 1)) / control.RowsCount;
                            m_GroupPageSize = control.ColumnsCount * control.RowsCount - 2;    //扣除向前、向后两个按钮
                            //坐标
                            int px = m_Space, py = m_Space, times = 0, pageCount = 0;
                            for (int i = 0; i < m_GroupPageSize; i++)
                            {
                                CrystalButton btnGroup = new CrystalButton();
                                btnGroup.Name = "btnGroup" + i;
                                btnGroup.BackColor = btnGroup.DisplayColor = Color.DodgerBlue;
                                btnGroup.Location = new Point(px, py);
                                btnGroup.Size = new Size(width, height);
                                btnGroup.Click += new System.EventHandler(this.btnGroup_Click);

                                this.pnlGroup.Controls.Add(btnGroup);
                                btnGroupList.Add(btnGroup);
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
                            }
                            px = (control.ColumnsCount - 2) * width + (control.ColumnsCount - 2 + 1) * m_Space;
                            py = (control.RowsCount - 1) * height + control.RowsCount * m_Space;
                            btnPageUp.Width = width;
                            btnPageUp.Height = height;
                            btnPageUp.Location = new Point(px, py);
                            px += width + m_Space;
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
            if (btnItemList.Count == 0)
            {
                if (ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls != null && ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls.Count > 0)
                {
                    foreach (BizControl control in ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls)
                    {
                        if (control.Name == "Item")
                        {
                            int width = (this.pnlItem.Width - m_Space * (control.ColumnsCount + 1)) / control.ColumnsCount;
                            int height = (this.pnlItem.Height - m_Space * (control.RowsCount + 1)) / control.RowsCount;
                            m_ItemPageSize = control.ColumnsCount * control.RowsCount - 2;    //扣除向前、向后两个按钮
                            //坐标
                            int px = m_Space, py = m_Space, times = 0, pageCount = 0;
                            for (int i = 0; i < m_ItemPageSize; i++)
                            {
                                CrystalButton btnItem = new CrystalButton();
                                btnItem.Name = "btnItem" + i;
                                btnItem.BackColor = btnItem.DisplayColor = Color.DodgerBlue;
                                btnItem.Location = new Point(px, py);
                                btnItem.Size = new Size(width, height);
                                btnItem.Click += new System.EventHandler(this.btnItem_Click);

                                this.pnlItem.Controls.Add(btnItem);
                                btnItemList.Add(btnItem);
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
                            }
                            px = (control.ColumnsCount - 2) * width + (control.ColumnsCount - 2 + 1) * m_Space;
                            py = (control.RowsCount - 1) * height + control.RowsCount * m_Space;
                            btnHead.Width = width;
                            btnHead.Height = height;
                            btnHead.Location = new Point(px, py);
                            px += width + m_Space;
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
            int startIndex = m_GroupPageIndex * m_GroupPageSize;
            int endIndex = (m_GroupPageIndex + 1) * m_GroupPageSize;
            if (endIndex > ConstantValuePool.GoodsGroupList.Count)
            {
                unDisplayNum = endIndex - ConstantValuePool.GoodsGroupList.Count;
                endIndex = ConstantValuePool.GoodsGroupList.Count;
            }
            //隐藏没有内容的按钮
            for (int i = btnGroupList.Count - unDisplayNum; i < btnGroupList.Count; i++)
            {
                btnGroupList[i].Visible = false;
            }
            //显示有内容的按钮
            for (int i = 0, j = startIndex; j < endIndex; i++, j++)
            {
                GoodsGroup goodsGroup = ConstantValuePool.GoodsGroupList[j];
                CrystalButton btn = btnGroupList[i];
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
            if (m_CurrentGoodsGroup != null)
            {
                if (m_CurrentGoodsGroup.GoodsList == null || m_CurrentGoodsGroup.GoodsList.Count == 0)
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
                    int startIndex = m_ItemPageIndex * m_ItemPageSize;
                    int endIndex = (m_ItemPageIndex + 1) * m_ItemPageSize;
                    if (endIndex > m_CurrentGoodsGroup.GoodsList.Count)
                    {
                        unDisplayNum = endIndex - m_CurrentGoodsGroup.GoodsList.Count;
                        endIndex = m_CurrentGoodsGroup.GoodsList.Count;
                    }
                    //隐藏没有内容的按钮
                    for (int i = btnItemList.Count - unDisplayNum; i < btnItemList.Count; i++)
                    {
                        btnItemList[i].Visible = false;
                    }
                    //显示有内容的按钮
                    for (int i = 0, j = startIndex; j < endIndex; i++, j++)
                    {
                        Goods goods = m_CurrentGoodsGroup.GoodsList[j];
                        CrystalButton btn = btnItemList[i];
                        btn.Visible = true;
                        if (m_ShowSilverCode)
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
                    if (endIndex >= m_CurrentGoodsGroup.GoodsList.Count)
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
                if (item.IsCommon || m_CurrentDetailsGroupIDList.Contains(item.DetailsGroupID))
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
                int startIndex = m_GroupPageIndex * m_GroupPageSize;
                int endIndex = (m_GroupPageIndex + 1) * m_GroupPageSize;
                if (endIndex > detailGroupList.Count)
                {
                    unDisplayNum = endIndex - detailGroupList.Count;
                    endIndex = detailGroupList.Count;
                }
                //隐藏没有内容的按钮
                for (int i = btnGroupList.Count - unDisplayNum; i < btnGroupList.Count; i++)
                {
                    btnGroupList[i].Visible = false;
                }
                //显示有内容的按钮
                for (int i = 0, j = startIndex; j < endIndex; i++, j++)
                {
                    DetailsGroup detailsGroup = detailGroupList[j];
                    CrystalButton btn = btnGroupList[i];
                    btn.Visible = true;
                    btn.Text = detailsGroup.DetailsGroupName;
                    btn.Tag = detailsGroup;
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
            if (m_CurrentDetailsGroup != null)
            {
                if (m_CurrentDetailsGroup.DetailsList == null || m_CurrentDetailsGroup.DetailsList.Count == 0)
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
                    int startIndex = m_ItemPageIndex * m_ItemPageSize;
                    int endIndex = (m_ItemPageIndex + 1) * m_ItemPageSize;
                    if (endIndex > m_CurrentDetailsGroup.DetailsList.Count)
                    {
                        unDisplayNum = endIndex - m_CurrentDetailsGroup.DetailsList.Count;
                        endIndex = m_CurrentDetailsGroup.DetailsList.Count;
                    }
                    //隐藏没有内容的按钮
                    for (int i = btnItemList.Count - unDisplayNum; i < btnItemList.Count; i++)
                    {
                        btnItemList[i].Visible = false;
                    }
                    //显示有内容的按钮
                    for (int i = 0, j = startIndex; j < endIndex; i++, j++)
                    {
                        Details details = m_CurrentDetailsGroup.DetailsList[j];
                        CrystalButton btn = btnItemList[i];
                        btn.Visible = true;
                        if (m_ShowSilverCode)
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
                    if (endIndex >= m_CurrentDetailsGroup.DetailsList.Count)
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

            for (int i = 0; i < btnItemList.Count; i++)
            {
                btnItemList[i].Visible = false;
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

        private void LoadDefaultGoodsGroupButton()
        {
            m_GroupPageIndex = 0;
            m_ItemPageIndex = 0;
            DisplayGoodsGroupButton();
            HideItemButton();
        }

        #region goods or detail button event

        private void btnGroup_Click(object sender, EventArgs e)
        {
            CrystalButton btnGroup = sender as CrystalButton;
            if (btnGroup.Tag is GoodsGroup)
            {
                m_CurrentGoodsGroup = btnGroup.Tag as GoodsGroup;
                Color pressedColor = ConstantValuePool.PressedColor;
                foreach (ButtonStyle btnStyle in ConstantValuePool.ButtonStyleList)
                {
                    if (m_CurrentGoodsGroup.ButtonStyleID.Equals(btnStyle.ButtonStyleID))
                    {
                        pressedColor = ColorConvert.RGB(btnStyle.ClickedBackColor);
                        break;
                    }
                }
                btnGroup.BackColor = pressedColor;
                if (prevPressedButton == null)
                {
                    prevPressedButton = btnGroup;
                }
                else
                {
                    if (btnGroup.Text != prevPressedButton.Text)
                    {
                        prevPressedButton.BackColor = prevPressedButton.DisplayColor;
                    }
                    prevPressedButton = btnGroup;
                }
                m_ItemPageIndex = 0;
                DisplayGoodsButton();
            }
            if (btnGroup.Tag is DetailsGroup)
            {
                prevPressedButton.BackColor = prevPressedButton.DisplayColor;

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
                prevPressedButton = btnGroup;
                if (detailsGroup.DetailsList != null && detailsGroup.DetailsList.Count > 0)
                {
                    m_CurrentDetailsGroup = detailsGroup;
                    m_ItemPageIndex = 0;
                    DisplayDetailButton();
                }
            }
        }

        private void btnItem_Click(object sender, EventArgs e)
        {
            CrystalButton btnItem = sender as CrystalButton;
            if (btnItem.Tag is Goods)
            {
                Goods goods = btnItem.Tag as Goods;
                if (goods == null) return;

                decimal goodsDiscount = 0;
                Discount _discount = null;
                bool IsContainsSalePrice = false;   //是否包含特价

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
                    bool IsInGroup = false;
                    foreach (GoodsLimitedTimeSale item in ConstantValuePool.GroupLimitedTimeSaleList)
                    {
                        if (item.ItemID == m_CurrentGoodsGroup.GoodsGroupID)
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
                            IsInGroup = true;
                            IsContainsSalePrice = true;
                            break;
                        }
                    }
                    //品项特价
                    if (!IsInGroup)
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
                                IsContainsSalePrice = true;
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
                if (!IsContainsSalePrice)
                {
                    bool haveCirculate = false;
                    IList<GoodsSetMeal> goodsSetMealList = new List<GoodsSetMeal>();
                    foreach (GoodsSetMeal item in ConstantValuePool.GoodsSetMealList)
                    {
                        if (item.ParentGoodsID.Equals(goods.GoodsID))
                        {
                            goodsSetMealList.Add(item);
                            haveCirculate = true;
                        }
                        else
                        {
                            if (haveCirculate) break;
                        }
                    }
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
                        bool IsSingleGoodsSetMeal = false;
                        foreach (KeyValuePair<int, List<GoodsSetMeal>> item in dicGoodsSetMealByGroup)
                        {
                            if (item.Value[0].IsRequired == true && (int)item.Value[0].LimitedQty == item.Value.Count)
                            {
                                IsSingleGoodsSetMeal = true;
                            }
                            else
                            {
                                IsSingleGoodsSetMeal = false;
                                break;
                            }
                        }
                        if (IsSingleGoodsSetMeal)
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
                    m_GroupPageIndex = 0;
                    m_ItemPageIndex = 0;
                    if (goods.DetailsGroupIDList != null && goods.DetailsGroupIDList.Count > 0)
                    {
                        m_GoodsOrDetails = false;    //状态为细项
                        m_CurrentDetailsGroupIDList = goods.DetailsGroupIDList;
                        DisplayDetailGroupButton();
                        HideItemButton();
                        m_DetailsPrefix = "--";
                    }
                }
                #endregion

                //datagridview滚动条定位
                dgvGoodsOrder.Rows[selectedIndex].Selected = true;
                dgvGoodsOrder.CurrentCell = dgvGoodsOrder.Rows[selectedIndex].Cells["GoodsNum"];
                //统计
                BindOrderInfoSum();
                //清空
                txtSearch.Text = string.Empty;
            }
            if (btnItem.Tag is Details)
            {
                Details details = btnItem.Tag as Details;
                if (dgvGoodsOrder.CurrentRow != null)
                {
                    int selectIndex = dgvGoodsOrder.CurrentRow.Index;
                    if (m_CurrentDetailsGroup.LimitedNumbers > 0)
                    {
                        object objGroupLimitNum = dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Tag;
                        if (objGroupLimitNum == null)
                        {
                            Dictionary<Guid, int> dicGroupLimitNum = new Dictionary<Guid, int>();
                            dicGroupLimitNum.Add(m_CurrentDetailsGroup.DetailsGroupID, 1);
                            dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Tag = dicGroupLimitNum;
                        }
                        else
                        {
                            Dictionary<Guid, int> dicGroupLimitNum = objGroupLimitNum as Dictionary<Guid, int>;
                            if (dicGroupLimitNum.ContainsKey(m_CurrentDetailsGroup.DetailsGroupID))
                            {
                                int selectedNum = dicGroupLimitNum[m_CurrentDetailsGroup.DetailsGroupID];
                                if (selectedNum >= m_CurrentDetailsGroup.LimitedNumbers)
                                {
                                    MessageBox.Show("超出细项的数量限制！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                                else
                                {
                                    selectedNum++;
                                    dicGroupLimitNum[m_CurrentDetailsGroup.DetailsGroupID] = selectedNum;
                                    dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Tag = dicGroupLimitNum;
                                }
                            }
                            else
                            {
                                dicGroupLimitNum.Add(m_CurrentDetailsGroup.DetailsGroupID, 1);
                                dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Tag = dicGroupLimitNum;
                            }
                        }
                    }
                    //数量
                    decimal itemNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
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
            m_GroupPageIndex--;
            if (m_GoodsOrDetails)
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
            m_GroupPageIndex++;
            if (m_GoodsOrDetails)
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
            m_ItemPageIndex--;
            if (m_GoodsOrDetails)
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
            m_ItemPageIndex++;
            if (m_GoodsOrDetails)
            {
                DisplayGoodsButton();
            }
            else
            {
                DisplayDetailButton();
            }
        }

        private void btnPgUp_Click(object sender, EventArgs e)
        {
            m_PageIndex--;
            DisplayDeliveryOrderButton();
        }

        private void btnPgDown_Click(object sender, EventArgs e)
        {
            m_PageIndex++;
            DisplayDeliveryOrderButton();
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
            decimal actualPayMoney = CutOffDecimal.HandleCutOff(wholePayMoney, ConstantValuePool.SysConfig.IsCutTail, ConstantValuePool.SysConfig.CutTailType, ConstantValuePool.SysConfig.CutTailDigit);
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
            if (form.IsNeedExist)
            {
                if (ConstantValuePool.DeskForm != null)
                {
                    ConstantValuePool.DeskForm.Close();
                }
                this.Close();
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
            if (dgvGoodsOrder.Rows.Count > 0 && dgvGoodsOrder.CurrentRow != null)
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
                //计算未打折金额
                decimal noDiscountPrice = 0;
                foreach (DataGridViewRow dr in dgvGoodsOrder.Rows)
                {
                    bool canDiscount = Convert.ToBoolean(dr.Cells["CanDiscount"].Value);
                    if (canDiscount)
                    {
                        decimal itemPrice = Convert.ToDecimal(dr.Cells["GoodsPrice"].Value);
                        noDiscountPrice += itemPrice;
                    }
                }
                FormDiscount formDiscount = new FormDiscount(DiscountDisplayModel.WholeDiscount, noDiscountPrice, m_ActualPayMoney);
                formDiscount.ShowDialog();
                if (formDiscount.CurrentDiscount != null)
                {
                    Discount discount = formDiscount.CurrentDiscount;
                    int firstIndex = -1; //折价索引
                    decimal offFixedPay = 0;
                    for (int index = 0; index < dgvGoodsOrder.Rows.Count; index++)
                    {
                        DataGridViewRow dr = dgvGoodsOrder.Rows[index];
                        bool canDiscount = Convert.ToBoolean(dr.Cells["CanDiscount"].Value);
                        if (canDiscount)
                        {
                            if (discount.DiscountType == (int)DiscountItemType.DiscountRate)
                            {
                                dr.Cells["GoodsDiscount"].Value = -Convert.ToDecimal(dr.Cells["GoodsPrice"].Value) * discount.DiscountRate;
                            }
                            else
                            {
                                if (firstIndex < 0)
                                {
                                    firstIndex = index;
                                }
                                decimal itemPrice = Convert.ToDecimal(dr.Cells["GoodsPrice"].Value);
                                decimal discountPrice = itemPrice / noDiscountPrice * discount.OffFixPay;
                                discountPrice = Math.Round(discountPrice, 2);
                                dr.Cells["GoodsDiscount"].Value = -discountPrice;
                                offFixedPay += discountPrice;
                            }
                            dr.Cells["GoodsDiscount"].Tag = discount;
                        }
                    }
                    if (firstIndex >= 0)
                    {
                        decimal gap = discount.OffFixPay - offFixedPay;
                        gap = Math.Round(gap, 2);
                        decimal discountPrice = Math.Abs(Convert.ToDecimal(dgvGoodsOrder.Rows[firstIndex].Cells["GoodsDiscount"].Value));
                        discountPrice += gap;
                        dgvGoodsOrder.Rows[firstIndex].Cells["GoodsDiscount"].Value = -discountPrice;
                    }
                    //统计
                    BindOrderInfoSum();
                }
            }
        }

        private void btnTakeOut_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.Rows.Count <= 0)
            {
                MessageBox.Show("请先选择菜品！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!haveDailyClose)
            {
                MessageBox.Show("上次未日结，不能新增菜单，请先进行日结操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (m_SalesOrder == null || m_SalesOrder.order.EatType == (int)EatWayType.Takeout)
            {
                string deskName = string.Empty;
                if (m_SalesOrder == null)
                {
                    if (ConstantValuePool.BizSettingConfig.CarteMode)
                    {
                        FormNumericKeypad form = new FormNumericKeypad();
                        form.DisplayText = "请输入餐牌号";
                        form.ShowDialog();
                        if (string.IsNullOrEmpty(form.KeypadValue))
                        {
                            MessageBox.Show("餐牌号不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            if (form.KeypadValue.Length > 3)
                            {
                                MessageBox.Show("您输入的餐牌号码过大！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            else
                            {
                                deskName = "W" + form.KeypadValue.PadLeft(3, '0');
                            }
                        }
                    }
                    else
                    {
                        deskName = "W001";
                    }
                }
                else
                {
                    deskName = m_SalesOrder.order.DeskName;
                }
                int result = SubmitSalesOrder(deskName, EatWayType.Takeout);
                if (result == 1)
                {
                    this.lbTotalPrice.Text = "总金额：";
                    this.lbDiscount.Text = "折扣：";
                    this.lbNeedPayMoney.Text = "实际应付：";
                    this.lbCutOff.Text = "去零：";
                    dgvGoodsOrder.Rows.Clear();
                    m_SalesOrder = null;
                    btnDeliveryGoods.Enabled = false;
                    btnDeliveryGoods.BackColor = ConstantValuePool.DisabledColor;
                    txtTelephone.Text = string.Empty;
                    txtName.Text = string.Empty;
                    txtAddress.Text = string.Empty;
                    txtTelephone.ReadOnly = false;
                    txtName.ReadOnly = false;
                    //加载外卖单列表
                    IList<DeliveryOrder> deliveryOrderList = OrderService.GetInstance().GetDeliveryOrderList();
                    if (deliveryOrderList != null)
                    {
                        m_PageIndex = 0;
                        m_DeliveryOrderList = deliveryOrderList;
                        DisplayDeliveryOrderButton();
                    }
                    //更新第二屏信息
                    if (Screen.AllScreens.Length > 1 && ConstantValuePool.BizSettingConfig.SecondScreenEnabled)
                    {
                        if (ConstantValuePool.SecondScreenForm != null && ConstantValuePool.SecondScreenForm is FormSecondScreen)
                        {
                            ((FormSecondScreen)ConstantValuePool.SecondScreenForm).ClearGoodsOrderInfo();
                        }
                    }
                }
                else if (result == 0)
                {
                    MessageBox.Show("外带提交失败，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (result == 2)
                {
                    MessageBox.Show("没有数据可以提交！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (m_SalesOrder.order.EatType == (int)EatWayType.OutsideOrder)
                {
                    MessageBox.Show("当前账单状态为[外送]，请更改类型后再进行操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (m_SalesOrder.order.EatType == (int)EatWayType.DineIn)
                {
                    MessageBox.Show("当前账单状态为[堂食]，请更改类型后再进行操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBeNew_Click(object sender, EventArgs e)
        {
            this.lbTotalPrice.Text = "总金额：";
            this.lbDiscount.Text = "折扣：";
            this.lbNeedPayMoney.Text = "实际应付：";
            this.lbCutOff.Text = "去零：";
            dgvGoodsOrder.Rows.Clear();
            m_SalesOrder = null;
            btnDeliveryGoods.Enabled = false;
            btnDeliveryGoods.BackColor = ConstantValuePool.DisabledColor;
            btnOutsideOrder.Enabled = true;
            btnOutsideOrder.BackColor = btnOutsideOrder.DisplayColor;
            btnDiscount.Enabled = true;
            btnDiscount.BackColor = btnDiscount.DisplayColor;
            btnWholeDiscount.Enabled = true;
            btnWholeDiscount.BackColor = btnWholeDiscount.DisplayColor;
            btnTakeOut.Enabled = true;
            btnTakeOut.BackColor = btnTakeOut.DisplayColor;
            txtTelephone.Text = string.Empty;
            txtName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtTelephone.ReadOnly = false;
            txtName.ReadOnly = false;
            if (prevDeliveryButton != null)
            {
                prevDeliveryButton.ForeColor = Color.White;
            }
        }

        private void btnOutsideOrder_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.Rows.Count <= 0)
            {
                MessageBox.Show("请先选择菜品！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!haveDailyClose)
            {
                MessageBox.Show("上次未日结，不能新增菜单，请先进行日结操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!string.IsNullOrEmpty(txtTelephone.Text.Trim()))
            {
                if (string.IsNullOrEmpty(txtName.Text.Trim()))
                {
                    if (DialogResult.Yes == MessageBox.Show("当前订单顾客姓名未填写，是否返回？", "信息提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        return;
                }
                if (string.IsNullOrEmpty(txtAddress.Text.Trim()))
                {
                    if (DialogResult.Yes == MessageBox.Show("当前订单外送地址未填写，是否返回？", "信息提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        return;
                }
            }
            if (m_SalesOrder == null || m_SalesOrder.order.EatType == (int)EatWayType.OutsideOrder)
            {
                string deskName = string.Empty;
                if (m_SalesOrder == null)
                {
                    deskName = "W001";
                }
                else
                {
                    deskName = m_SalesOrder.order.DeskName;
                }
                int result = SubmitSalesOrder(deskName, EatWayType.OutsideOrder);
                if (result == 1)
                {
                    this.lbTotalPrice.Text = "总金额：";
                    this.lbDiscount.Text = "折扣：";
                    this.lbNeedPayMoney.Text = "实际应付：";
                    this.lbCutOff.Text = "去零：";
                    dgvGoodsOrder.Rows.Clear();
                    m_SalesOrder = null;
                    btnDeliveryGoods.Enabled = false;
                    btnDeliveryGoods.BackColor = ConstantValuePool.DisabledColor;
                    txtTelephone.Text = string.Empty;
                    txtName.Text = string.Empty;
                    txtAddress.Text = string.Empty;
                    txtTelephone.ReadOnly = false;
                    txtName.ReadOnly = false;
                    //加载外卖单列表
                    IList<DeliveryOrder> deliveryOrderList = OrderService.GetInstance().GetDeliveryOrderList();
                    if (deliveryOrderList != null)
                    {
                        m_PageIndex = 0;
                        m_DeliveryOrderList = deliveryOrderList;
                        DisplayDeliveryOrderButton();
                    }
                    //更新第二屏信息
                    if (Screen.AllScreens.Length > 1 && ConstantValuePool.BizSettingConfig.SecondScreenEnabled)
                    {
                        if (ConstantValuePool.SecondScreenForm != null && ConstantValuePool.SecondScreenForm is FormSecondScreen)
                        {
                            ((FormSecondScreen)ConstantValuePool.SecondScreenForm).ClearGoodsOrderInfo();
                        }
                    }
                }
                else if (result == 0)
                {
                    MessageBox.Show("外送提交失败，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (result == 2)
                {
                    MessageBox.Show("没有数据可以提交！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (m_SalesOrder.order.EatType == (int)EatWayType.Takeout)
                {
                    MessageBox.Show("当前账单状态为[外带]，请更改类型后再进行操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (m_SalesOrder.order.EatType == (int)EatWayType.DineIn)
                {
                    MessageBox.Show("当前账单状态为[堂食]，请更改类型后再进行操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDeliveryGoods_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.Rows.Count <= 0)
            {
                MessageBox.Show("请选择外送账单进行出货！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
                MessageBox.Show("存在新单，不能出货！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            PrintData printData = new PrintData();
            printData.ShopName = ConstantValuePool.CurrentShop.ShopName;
            printData.DeskName = m_SalesOrder.order.DeskName;
            printData.PersonNum = "1";
            printData.PrintTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            printData.EmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
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
            string telPhone = this.txtTelephone.Text;
            string customerName = this.txtName.Text;
            string address = this.txtAddress.Text;
            FormDeliveryGoods form = new FormDeliveryGoods(m_SalesOrder, printData, telPhone, customerName, address);
            form.ShowDialog();
            if (form.HasDeliveried)
            {
                btnDiscount.Enabled = false;
                btnDiscount.BackColor = ConstantValuePool.DisabledColor;
                btnWholeDiscount.Enabled = false;
                btnWholeDiscount.BackColor = ConstantValuePool.DisabledColor;
                btnTakeOut.Enabled = false;
                btnTakeOut.BackColor = ConstantValuePool.DisabledColor;
                btnOutsideOrder.Enabled = false;
                btnOutsideOrder.BackColor = ConstantValuePool.DisabledColor;
                btnDeliveryGoods.Enabled = false;
                btnDeliveryGoods.BackColor = ConstantValuePool.DisabledColor;
                //加载外卖单列表
                IList<DeliveryOrder> deliveryOrderList = OrderService.GetInstance().GetDeliveryOrderList();
                if (deliveryOrderList != null)
                {
                    m_PageIndex = 0;
                    m_DeliveryOrderList = deliveryOrderList;
                    DisplayDeliveryOrderButton();
                }
            }
        }

        private void btnPrintBill_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.Rows.Count > 0)
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
                    MessageBox.Show("存在新单，不能印单！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    if (ConstantValuePool.BizSettingConfig.printConfig.Enabled)
                    {
                        //打印
                        PrintData printData = new PrintData();
                        printData.ShopName = ConstantValuePool.CurrentShop.ShopName;
                        printData.DeskName = m_SalesOrder.order.DeskName;
                        printData.PersonNum = "1";
                        printData.PrintTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        printData.EmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
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
                        printData.CustomerPhone = this.txtTelephone.Text.Trim();
                        printData.CustomerName = this.txtName.Text.Trim();
                        printData.DeliveryAddress = this.txtAddress.Text.Trim();
                        printData.Remark = string.Empty;
                        printData.DeliveryEmployeeName = string.Empty;
                        int copies = ConstantValuePool.BizSettingConfig.printConfig.Copies;
                        string paperWidth = ConstantValuePool.BizSettingConfig.printConfig.PaperWidth;
                        if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.DRIVER)
                        {
                            string printerName = ConstantValuePool.BizSettingConfig.printConfig.Name;
                            DriverOrderPrint printer = DriverOrderPrint.GetInstance(printerName, paperWidth);
                            for (int i = 0; i < copies; i++)
                            {
                                if (m_SalesOrder.order.EatType == (int) EatWayType.Takeout)
                                {
                                    printer.DoPrintOrder(printData);
                                }
                                else
                                {
                                    printer.DoPrintDeliveryOrder(printData);
                                }
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
                                        if (m_SalesOrder.order.EatType == (int)EatWayType.Takeout)
                                        {
                                            printer.DoPrintOrder(printData);
                                        }
                                        else
                                        {
                                            printer.DoPrintDeliveryOrder(printData);
                                        }
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
                                if (m_SalesOrder.order.EatType == (int)EatWayType.Takeout)
                                {
                                    printer.DoPrintOrder(printData);
                                }
                                else
                                {
                                    printer.DoPrintDeliveryOrder(printData);
                                }
                            }
                        }
                        if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.USB)
                        {
                            string VID = ConstantValuePool.BizSettingConfig.printConfig.VID;
                            string PID = ConstantValuePool.BizSettingConfig.printConfig.PID;
                            InstructionOrderPrint printer = new InstructionOrderPrint(VID, PID, paperWidth);
                            for (int i = 0; i < copies; i++)
                            {
                                if (m_SalesOrder.order.EatType == (int)EatWayType.Takeout)
                                {
                                    printer.DoPrintOrder(printData);
                                }
                                else
                                {
                                    printer.DoPrintDeliveryOrder(printData);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            m_OnShow = false;
            if (ConstantValuePool.DeskForm == null)
            {
                this.Close();
            }
            else
            {
                this.Hide();
            }
        }

        private void btnPriceCode_Click(object sender, EventArgs e)
        {
            m_ShowSilverCode = !m_ShowSilverCode;
            if (m_GoodsOrDetails)
            {
                DisplayGoodsButton();
            }
            else
            {
                DisplayDetailButton();
            }
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            CrystalButton btnDelivery = sender as CrystalButton;
            if (btnDelivery.Tag != null)
            {
                DeliveryOrder deliveryOrder = btnDelivery.Tag as DeliveryOrder;
                if (deliveryOrder.PayTime == null)
                {
                    if (prevDeliveryButton == null)
                    {
                        btnDelivery.ForeColor = Color.DodgerBlue;
                        prevDeliveryButton = btnDelivery;
                    }
                    else
                    {
                        if (btnDelivery.Text != prevDeliveryButton.Text)
                        {
                            prevDeliveryButton.ForeColor = Color.White;
                            btnDelivery.ForeColor = Color.DodgerBlue;
                        }
                        prevDeliveryButton = btnDelivery;
                    }
                    if (deliveryOrder.EatType == (int)EatWayType.OutsideOrder && deliveryOrder.DeliveryTime == null)
                    {
                        btnDeliveryGoods.Enabled = true;
                        btnDeliveryGoods.BackColor = btnDeliveryGoods.DisplayColor;
                    }
                    else
                    {
                        btnDeliveryGoods.Enabled = false;
                        btnDeliveryGoods.BackColor = ConstantValuePool.DisabledColor;
                    }
                    if (deliveryOrder.EatType == (int)EatWayType.OutsideOrder && deliveryOrder.DeliveryTime != null)
                    {
                        btnOutsideOrder.Enabled = false;
                        btnOutsideOrder.BackColor = ConstantValuePool.DisabledColor;
                        btnDiscount.Enabled = false;
                        btnDiscount.BackColor = ConstantValuePool.DisabledColor;
                        btnWholeDiscount.Enabled = false;
                        btnWholeDiscount.BackColor = ConstantValuePool.DisabledColor;
                        btnTakeOut.Enabled = false;
                        btnTakeOut.BackColor = ConstantValuePool.DisabledColor;
                    }
                    else
                    {
                        btnOutsideOrder.Enabled = true;
                        btnOutsideOrder.BackColor = btnOutsideOrder.DisplayColor;
                        btnDiscount.Enabled = true;
                        btnDiscount.BackColor = btnDiscount.DisplayColor;
                        btnWholeDiscount.Enabled = true;
                        btnWholeDiscount.BackColor = btnWholeDiscount.DisplayColor;
                        btnTakeOut.Enabled = true;
                        btnTakeOut.BackColor = btnTakeOut.DisplayColor;
                    }
                    SalesOrder _salesOrder = SalesOrderService.GetInstance().GetSalesOrder(deliveryOrder.OrderID);
                    if (_salesOrder != null)
                    {
                        m_SalesOrder = _salesOrder;
                        BindGoodsOrderInfo();   //绑定订单信息
                        BindOrderInfoSum();
                        CustomerOrder customerOrder = CustomersService.GetInstance().GetCustomerOrder(m_SalesOrder.order.OrderID);
                        if (customerOrder != null)
                        {
                            this.txtTelephone.Text = customerOrder.Telephone;
                            this.txtName.Text = customerOrder.CustomerName;
                            this.txtAddress.Text = customerOrder.Address;
                            txtTelephone.ReadOnly = true;
                            txtName.ReadOnly = true;
                        }
                        else
                        {
                            this.txtTelephone.Text = string.Empty;
                            this.txtName.Text = string.Empty;
                            this.txtAddress.Text = string.Empty;
                            txtTelephone.ReadOnly = false;
                            txtName.ReadOnly = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("获取账单信息失败，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    SalesOrder _salesOrder = SalesOrderService.GetInstance().GetSalesOrder(deliveryOrder.OrderID);
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
        }

        /// <summary>
        /// 0:提交失败 1:成功 2:没有可提交的数据 3:沽清失败
        /// </summary>
        private Int32 SubmitSalesOrder(string deskName, EatWayType eatType)
        {
            int result = 0;
            Guid orderID = Guid.Empty;
            if (m_SalesOrder == null)    //新增的菜单
            {
                orderID = Guid.NewGuid();
            }
            else
            {
                orderID = m_SalesOrder.order.OrderID;
            }
            IList<GoodsCheckStock> temp = new List<GoodsCheckStock>();
            IList<OrderDetails> newOrderDetailsList = new List<OrderDetails>();
            IList<OrderDiscount> newOrderDiscountList = new List<OrderDiscount>();
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
                    orderDetails.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
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

                        GoodsCheckStock goodsCheckStock = new GoodsCheckStock();
                        goodsCheckStock.GoodsID = goods.GoodsID;
                        goodsCheckStock.GoodsName = goods.GoodsName;
                        goodsCheckStock.ReducedQuantity = orderDetails.ItemQty;
                        temp.Add(goodsCheckStock);
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
                            orderDiscount.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
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
                        orderDetails.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
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
                            orderDiscount.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
                            newOrderDiscountList.Add(orderDiscount);
                        }
                    }
                }
            }
            //品项沽清
            IList<GoodsCheckStock> tempGoodsStockList = GoodsService.GetInstance().GetGoodsCheckStock();
            if (tempGoodsStockList != null && tempGoodsStockList.Count > 0 && temp.Count > 0)
            {
                IList<GoodsCheckStock> goodsCheckStockList = new List<GoodsCheckStock>();
                foreach (GoodsCheckStock item in temp)
                {
                    bool IsContains = false;
                    foreach (GoodsCheckStock tempGoodsStock in tempGoodsStockList)
                    {
                        if (item.GoodsID.Equals(tempGoodsStock.GoodsID))
                        {
                            IsContains = true;
                            break;
                        }
                    }
                    if (IsContains)
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
                        return 3;
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
                order.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
                order.EmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;

                SalesOrder salesOrder = new SalesOrder();
                salesOrder.order = order;
                salesOrder.orderDetailsList = newOrderDetailsList;
                salesOrder.orderDiscountList = newOrderDiscountList;
                _tranSeq = SalesOrderService.GetInstance().CreateSalesOrder(salesOrder);
                if (_tranSeq > 0)
                {
                    //重新加载
                    m_SalesOrder = SalesOrderService.GetInstance().GetSalesOrder(orderID);
                    BindGoodsOrderInfo();   //绑定订单信息
                    BindOrderInfoSum();
                    result = 1;
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
                    order.DeviceNo = ConstantValuePool.BizSettingConfig.DeviceNo;
                    order.DeskName = deskName;
                    order.PeopleNum = 1;
                    order.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
                    order.EmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                    SalesOrder salesOrder = new SalesOrder();
                    salesOrder.order = order;
                    salesOrder.orderDetailsList = newOrderDetailsList;
                    salesOrder.orderDiscountList = newOrderDiscountList;
                    if (SalesOrderService.GetInstance().UpdateSalesOrder(salesOrder) == 1)
                    {
                        //重新加载
                        m_SalesOrder = SalesOrderService.GetInstance().GetSalesOrder(orderID);
                        BindGoodsOrderInfo();   //绑定订单信息
                        BindOrderInfoSum();
                        result = 1;
                    }
                    _tranSeq = m_SalesOrder.order.TranSequence;
                }
                else
                {
                    result = 2;
                }
            }
            if (eatType == EatWayType.OutsideOrder)
            {
                if (result == 1 || result == 2)
                {
                    //添加外送信息
                    CustomerOrder customerOrder = new CustomerOrder();
                    customerOrder.OrderID = orderID;
                    customerOrder.Telephone = txtTelephone.Text.Trim();
                    customerOrder.CustomerName = txtName.Text.Trim();
                    customerOrder.Address = txtAddress.Text.Trim();
                    if (CustomersService.GetInstance().CreateOrUpdateCustomerOrder(customerOrder))
                    {
                        result = 1;
                    }
                    else
                    {
                        MessageBox.Show("添加外送信息失败！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            if (eatType == EatWayType.Takeout)
            {
                if (result == 1)
                {
                    if (ConstantValuePool.BizSettingConfig.printConfig.Enabled)
                    {
                        //打印
                        PrintData printData = new PrintData();
                        printData.ShopName = ConstantValuePool.CurrentShop.ShopName;
                        printData.DeskName = deskName;
                        printData.PersonNum = "1";
                        printData.PrintTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        printData.EmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
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
                        if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.DRIVER)
                        {
                            string printerName = ConstantValuePool.BizSettingConfig.printConfig.Name;
                            DriverOrderPrint printer = DriverOrderPrint.GetInstance(printerName, paperWidth);
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
                            string VID = ConstantValuePool.BizSettingConfig.printConfig.VID;
                            string PID = ConstantValuePool.BizSettingConfig.printConfig.PID;
                            InstructionOrderPrint printer = new InstructionOrderPrint(VID, PID, paperWidth);
                            for (int i = 0; i < copies; i++)
                            {
                                printer.DoPrintOrder(printData);
                            }
                        }
                    }
                }
            }
            return result;
        }

        private void DisplayDeliveryOrderButton()
        {
            //禁止引发Layout事件
            this.pnlDelivery.SuspendLayout();
            this.SuspendLayout();

            int unDisplayNum = 0;
            int startIndex = m_PageIndex * m_PageSize;
            int endIndex = (m_PageIndex + 1) * m_PageSize;
            if (endIndex > m_DeliveryOrderList.Count)
            {
                unDisplayNum = endIndex - m_DeliveryOrderList.Count;
                endIndex = m_DeliveryOrderList.Count;
            }
            //隐藏没有内容的按钮
            for (int i = btnDeliveryList.Count - unDisplayNum; i < btnDeliveryList.Count; i++)
            {
                btnDeliveryList[i].Tag = null;
                btnDeliveryList[i].Text = string.Empty;
                btnDeliveryList[i].ForeColor = Color.White;
                btnDeliveryList[i].BackColor = btnDeliveryList[i].DisplayColor;
                btnDeliveryList[i].Enabled = true;
            }
            //显示有内容的按钮
            for (int i = 0, j = startIndex; j < endIndex; i++, j++)
            {
                btnDeliveryList[i].Tag = m_DeliveryOrderList[j];
                btnDeliveryList[i].ForeColor = Color.White;
                if (m_DeliveryOrderList[j].PayTime == null)
                {
                    if (m_DeliveryOrderList[j].EatType == (int)EatWayType.Takeout)
                    {
                        btnDeliveryList[i].BackColor = Color.Red;
                        btnDeliveryList[i].Text = m_DeliveryOrderList[j].TranSequence.ToString() + "-外带";
                    }
                    if (m_DeliveryOrderList[j].EatType == (int)EatWayType.OutsideOrder)
                    {
                        if (m_DeliveryOrderList[j].DeliveryTime == null)
                        {
                            btnDeliveryList[i].BackColor = Color.Orange;
                            btnDeliveryList[i].Text = m_DeliveryOrderList[j].TranSequence.ToString() + "-未出货";
                        }
                        else
                        {
                            btnDeliveryList[i].BackColor = Color.Olive;
                            btnDeliveryList[i].Text = m_DeliveryOrderList[j].TranSequence.ToString() + "-已出货";
                        }
                    }
                }
                else
                {
                    btnDeliveryList[i].Text = m_DeliveryOrderList[j].TranSequence + "\r\n " + Convert.ToDateTime(m_DeliveryOrderList[j].PayTime).ToString("MM-dd HH:mm");
                    btnDeliveryList[i].BackColor = Color.Green;
                }
            }
            //设置页码按钮的显示
            if (startIndex <= 0)
            {
                btnPgUp.Enabled = false;
                btnPgUp.BackColor = ConstantValuePool.DisabledColor;
            }
            else
            {
                btnPgUp.Enabled = true;
                btnPgUp.BackColor = btnPgUp.DisplayColor;
            }
            if (endIndex >= m_DeliveryOrderList.Count)
            {
                btnPgDown.Enabled = false;
                btnPgDown.BackColor = ConstantValuePool.DisabledColor;
            }
            else
            {
                btnPgDown.Enabled = true;
                btnPgDown.BackColor = btnPgDown.DisplayColor;
            }

            this.pnlDelivery.ResumeLayout(false);
            this.pnlDelivery.PerformLayout();
            this.ResumeLayout(false);
        }

        private void btnTelephone_Click(object sender, EventArgs e)
        {
            FormFindCustomer form = new FormFindCustomer();
            form.ShowDialog();
            if (form.SelectedCustomerInfo != null)
            {
                CustomerInfo customerInfo = form.SelectedCustomerInfo;
                string address = string.Empty;
                if (customerInfo.ActiveIndex == 1)
                {
                    address = customerInfo.DeliveryAddress1;
                }
                else if (customerInfo.ActiveIndex == 2)
                {
                    address = customerInfo.DeliveryAddress2;
                }
                else if (customerInfo.ActiveIndex == 3)
                {
                    address = customerInfo.DeliveryAddress3;
                }
                txtTelephone.Text = customerInfo.Telephone;
                txtName.Text = customerInfo.CustomerName;
                txtAddress.Text = address;
                txtTelephone.ReadOnly = true;
                txtName.ReadOnly = true;
            }
        }

        private void btnAddress_Click(object sender, EventArgs e)
        {
            string telephone = txtTelephone.Text.Trim();
            string address = txtAddress.Text.Trim();
            if (!string.IsNullOrEmpty(telephone))
            {
                int callType = 0;  //手动
                FormIncomingCall form = new FormIncomingCall(telephone, address, callType);
                form.ShowDialog();
                if (!string.IsNullOrEmpty(form.IncomingPhoneNo))
                {
                    txtTelephone.Text = form.IncomingPhoneNo;
                }
                if (!string.IsNullOrEmpty(form.SelectedAddress))
                {
                    txtAddress.Text = form.SelectedAddress;
                }
                if (form.CurCustomerInfo != null)
                {
                    txtName.Text = form.CurCustomerInfo.CustomerName;
                }
            }
        }

        private void btnShowDetails_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.CurrentRow != null)
            {
                int selectIndex = dgvGoodsOrder.CurrentRow.Index;
                if (dgvGoodsOrder.Rows[selectIndex].Cells["OrderDetailsID"].Value == null)
                {
                    m_GroupPageIndex = 0;
                    m_ItemPageIndex = 0;
                    int itemType = Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value);
                    if (itemType == (int)OrderItemType.Goods)
                    {
                        Goods goods = dgvGoodsOrder.Rows[selectIndex].Cells["ItemID"].Tag as Goods;
                        if (goods.DetailsGroupIDList != null && goods.DetailsGroupIDList.Count > 0)
                        {
                            m_GoodsOrDetails = false;    //状态为细项
                            m_CurrentDetailsGroupIDList = goods.DetailsGroupIDList;
                            DisplayDetailGroupButton();
                            HideItemButton();
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
                            HideItemButton();
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.CurrentRow != null)
            {
                int selectIndex = dgvGoodsOrder.CurrentRow.Index;
                if (dgvGoodsOrder.Rows[selectIndex].Cells["OrderDetailsID"].Value == null)
                {
                    decimal quantity = 0;
                    int itemType = Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value);
                    if (itemType == (int)OrderItemType.Goods)
                    {
                        Goods goods = dgvGoodsOrder.Rows[selectIndex].Cells["ItemID"].Tag as Goods;
                        decimal originalGoodsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                        decimal originalGoodsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsDiscount"].Value);
                        quantity = originalGoodsNum + 1;
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
                        quantity = originalDetailsNum + 1;
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

        private void btnSub_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.CurrentRow != null)
            {
                int selectIndex = dgvGoodsOrder.CurrentRow.Index;
                if (dgvGoodsOrder.Rows[selectIndex].Cells["OrderDetailsID"].Value == null)
                {
                    decimal quantity = 0;
                    int itemType = Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value);
                    if (itemType == (int)OrderItemType.Goods)
                    {
                        Goods goods = dgvGoodsOrder.Rows[selectIndex].Cells["ItemID"].Tag as Goods;
                        decimal originalGoodsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                        decimal originalGoodsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsDiscount"].Value);
                        quantity = originalGoodsNum - 1;
                        if (quantity <= 0) return;
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
                        quantity = originalDetailsNum - 1;
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
                    Guid orderDetailsID = new Guid(dgvGoodsOrder.Rows[selectIndex].Cells["OrderDetailsID"].Value.ToString());
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
                                else
                                {
                                    singleItemPriceSum += Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value) / Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value);
                                }
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
                            orderDetails.OrderDetailsID = orderDetailsID;
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
                                    else
                                    {
                                        orderDetailsID = new Guid(dgvGoodsOrder.Rows[index].Cells["OrderDetailsID"].Value.ToString());
                                        originalDetailsNum = Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value);
                                        originalDetailsDiscount = Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value);
                                        decimal delItemNum = originalDetailsNum / goodsNum * form.DelItemNum;
                                        remainNum = originalDetailsNum - delItemNum;
                                        dicRemainNum.Add(index, remainNum);
                                        DeletedOrderDetails item = new DeletedOrderDetails();
                                        item.OrderDetailsID = orderDetailsID;
                                        item.DeletedQuantity = -delItemNum;
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
                            deletedSingleOrder.OrderID = m_SalesOrder.order.OrderID;
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
                            orderDetails.OrderDetailsID = orderDetailsID;
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
                                    else
                                    {
                                        deletedIndexList.Add(index);
                                        orderDetailsID = new Guid(dgvGoodsOrder.Rows[index].Cells["OrderDetailsID"].Value.ToString());
                                        DeletedOrderDetails item = new DeletedOrderDetails();
                                        item.OrderDetailsID = orderDetailsID;
                                        item.DeletedQuantity = -Convert.ToDecimal(dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value);
                                        item.RemainQuantity = 0;
                                        item.OffPay = 0;
                                        item.AuthorisedManager = ConstantValuePool.CurrentEmployee.EmployeeID;
                                        item.CancelEmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                                        item.CancelReasonName = form.CurrentReason.ReasonName;
                                        deletedOrderDetailsList.Add(item);
                                    }
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
                            deletedSingleOrder.OrderID = m_SalesOrder.order.OrderID;
                            deletedSingleOrder.TotalSellPrice = totalPrice;
                            deletedSingleOrder.ActualSellPrice = actualPayMoney;
                            deletedSingleOrder.DiscountPrice = totalDiscount;
                            deletedSingleOrder.CutOffPrice = wholePayMoney - actualPayMoney;
                            deletedSingleOrder.deletedOrderDetailsList = deletedOrderDetailsList;

                            if (DeletedOrderService.GetInstance().DeleteSingleOrder(deletedSingleOrder))
                            {
                                for (int i = deletedIndexList.Count - 1; i >= 0; i--)
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
                                else
                                {
                                    dgvGoodsOrder.Rows.RemoveAt(i);
                                    i--;
                                }
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

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            if (m_SalesOrder != null)
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
                    deletedOrder.OrderID = m_SalesOrder.order.OrderID;
                    deletedOrder.AuthorisedManager = ConstantValuePool.CurrentEmployee.EmployeeID;
                    deletedOrder.CancelEmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                    deletedOrder.CancelReasonName = form.CurrentReason.ReasonName;

                    if (DeletedOrderService.GetInstance().DeleteWholeOrder(deletedOrder))
                    {
                        //加载外卖单列表
                        IList<DeliveryOrder> deliveryOrderList = OrderService.GetInstance().GetDeliveryOrderList();
                        if (deliveryOrderList != null)
                        {
                            m_PageIndex = 0;
                            m_DeliveryOrderList = deliveryOrderList;
                            DisplayDeliveryOrderButton();
                        }
                    }
                    else
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
            this.lbTotalPrice.Text = "总金额：";
            this.lbDiscount.Text = "折扣：";
            this.lbNeedPayMoney.Text = "实际应付：";
            this.lbCutOff.Text = "去零：";
            dgvGoodsOrder.Rows.Clear();
            m_SalesOrder = null;
            btnDeliveryGoods.Enabled = false;
            btnDeliveryGoods.BackColor = ConstantValuePool.DisabledColor;
            btnOutsideOrder.Enabled = true;
            btnOutsideOrder.BackColor = btnOutsideOrder.DisplayColor;
            txtTelephone.Text = string.Empty;
            txtName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtTelephone.ReadOnly = false;
            txtName.ReadOnly = false;
            //更新第二屏信息
            if (Screen.AllScreens.Length > 1 && ConstantValuePool.BizSettingConfig.SecondScreenEnabled)
            {
                if (ConstantValuePool.SecondScreenForm != null && ConstantValuePool.SecondScreenForm is FormSecondScreen)
                {
                    ((FormSecondScreen)ConstantValuePool.SecondScreenForm).ClearGoodsOrderInfo();
                }
            }
        }

        private bool IsItemButtonEnabled(Guid itemID, ItemsType itemType)
        {
            bool IsEnabled = true;
            foreach (GoodsCronTrigger trigger in ConstantValuePool.GoodsCronTriggerList)
            {
                if (itemID == trigger.ItemID && (int)itemType == trigger.ItemType)
                {
                    if (DateTime.Now >= DateTime.Parse(trigger.BeginDate) && DateTime.Now <= DateTime.Parse(trigger.EndDate))
                    {
                        DayOfWeek curWeek = DateTime.Now.DayOfWeek;
                        string curMonth = DateTime.Now.Month.ToString();
                        string curDay = DateTime.Now.Day.ToString();
                        int curHour = DateTime.Now.Hour;
                        int curMinute = DateTime.Now.Minute;
                        //判断周或者日
                        if (trigger.Week == "?")
                        {
                            //判断是否包含当日
                            if (trigger.Day != "*")
                            {
                                string[] dayArr = trigger.Day.Split(',');
                                bool IsContainDay = false;
                                foreach (string day in dayArr)
                                {
                                    if (curDay == day)
                                    {
                                        IsContainDay = true;
                                        break;
                                    }
                                }
                                if (!IsContainDay)
                                {
                                    IsEnabled = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            //判断是否包含周几
                            //判断包含# 例:当月第几周星期几
                            if (trigger.Week.IndexOf('#') > 0)
                            {
                                string weekIndex = trigger.Week.Split('#')[0];
                                string weekDay = trigger.Week.Split('#')[1];
                                //计算当日是当月的第几周
                                DateTime FirstofMonth = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + "01");
                                int i = (int)FirstofMonth.Date.DayOfWeek;
                                if (i == 0)
                                {
                                    i = 7;
                                }
                                int curWeekIndex = (DateTime.Now.Day + i - 1) / 7;
                                if (curWeekIndex != int.Parse(weekIndex))
                                {
                                    IsEnabled = false;
                                    break;
                                }
                                else
                                {
                                    if ((int)curWeek != int.Parse(weekDay))
                                    {
                                        IsEnabled = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                //不包含# 例:当月每个星期几
                                string[] weekArr = trigger.Week.Split(',');
                                bool IsContainWeek = false;
                                foreach (string week in weekArr)
                                {
                                    if ((int)curWeek == int.Parse(week))
                                    {
                                        IsContainWeek = true;
                                        break;
                                    }
                                }
                                if (!IsContainWeek)
                                {
                                    IsEnabled = false;
                                    break;
                                }
                            }
                        }
                        //判断时
                        if (trigger.Hour != "*")
                        {
                            if (trigger.Hour.IndexOf('-') > 0)
                            {
                                string hourMinute = curHour.ToString().PadLeft(2, '0') + ":" + curMinute.ToString().PadLeft(2, '0');
                                if (trigger.Hour.IndexOf(',') > 0) //多个小时时间段
                                {
                                    string[] hourArr = trigger.Hour.Split(',');
                                    bool IsContainHour = false;
                                    foreach (string hour in hourArr)
                                    {
                                        string beginHour = hour.Split('-')[0].Trim();
                                        string endHour = hour.Split('-')[1].Trim();
                                        if (string.Compare(hourMinute, beginHour) >= 0 && string.Compare(hourMinute, endHour) <= 0)
                                        {
                                            IsContainHour = true;
                                            break;
                                        }
                                    }
                                    if (!IsContainHour)
                                    {
                                        IsEnabled = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    string beginHour = trigger.Hour.Split('-')[0].Trim();
                                    string endHour = trigger.Hour.Split('-')[1].Trim();
                                    if (string.Compare(hourMinute, beginHour) < 0 || string.Compare(hourMinute, endHour) > 0)
                                    {
                                        IsEnabled = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (trigger.Hour.IndexOf(',') > 0) //多个小时
                                {
                                    string[] hourArr = trigger.Hour.Split(',');
                                    bool IsContainHour = false;
                                    foreach (string hour in hourArr)
                                    {
                                        if (curHour == int.Parse(hour))
                                        {
                                            IsContainHour = true;
                                            break;
                                        }
                                    }
                                    if (!IsContainHour)
                                    {
                                        IsEnabled = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (curHour != int.Parse(trigger.Hour))
                                    {
                                        IsEnabled = false;
                                        break;
                                    }
                                }
                            }
                        }
                        //判断分
                        if (trigger.Minute != "*")
                        {
                            if (trigger.Minute.IndexOf('-') > 0)
                            {
                                if (trigger.Minute.IndexOf(',') > 0) //多个分钟时间段
                                {
                                    string[] minuteArr = trigger.Minute.Split(',');
                                    bool IsContainMinute = false;
                                    foreach (string minute in minuteArr)
                                    {
                                        string beginMinute = minute.Split('-')[0];
                                        string endMinute = minute.Split('-')[1];
                                        if (curMinute >= int.Parse(beginMinute) && curMinute <= int.Parse(endMinute))
                                        {
                                            IsContainMinute = true;
                                            break;
                                        }
                                    }
                                    if (!IsContainMinute)
                                    {
                                        IsEnabled = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    string beginMinute = trigger.Minute.Split('-')[0];
                                    string endMinute = trigger.Minute.Split('-')[1];
                                    if (curMinute < int.Parse(beginMinute) || curMinute > int.Parse(endMinute))
                                    {
                                        IsEnabled = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (trigger.Minute.IndexOf(',') > 0) //多个分钟
                                {
                                    string[] minuteArr = trigger.Minute.Split(',');
                                    bool IsContainMinute = false;
                                    foreach (string minute in minuteArr)
                                    {
                                        if (curMinute == int.Parse(minute))
                                        {
                                            IsContainMinute = true;
                                            break;
                                        }
                                    }
                                    if (!IsContainMinute)
                                    {
                                        IsEnabled = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (curMinute != int.Parse(trigger.Minute))
                                    {
                                        IsEnabled = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        IsEnabled = false;
                    }
                    break;
                }
            }
            return IsEnabled;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string singleCode = this.txtSearch.Text.Trim();
            if (singleCode.Length > 3)
            {
                List<Goods> goodsList = new List<Goods>();
                foreach (GoodsGroup goodsGroup in ConstantValuePool.GoodsGroupList)
                {
                    foreach (Goods goods in goodsGroup.GoodsList)
                    {
                        if (!string.IsNullOrEmpty(goods.BrevityCode) && goods.BrevityCode.IndexOf(singleCode) >= 0)
                        {
                            goodsList.Add(goods);
                        }
                        else if (!string.IsNullOrEmpty(goods.PinyinCode) && goods.PinyinCode.IndexOf(singleCode) >= 0)
                        {
                            goodsList.Add(goods);
                        }
                    }
                }
                //少于一页的数量才显示
                if (goodsList.Count <= m_ItemPageSize)
                {
                    //禁止引发Layout事件
                    this.pnlItem.SuspendLayout();
                    this.SuspendLayout();

                    int unDisplayNum = m_ItemPageSize - goodsList.Count;
                    //隐藏没有内容的按钮
                    for (int i = btnItemList.Count - unDisplayNum; i < btnItemList.Count; i++)
                    {
                        btnItemList[i].Visible = false;
                    }
                    //显示有内容的按钮
                    for (int i = 0; i < goodsList.Count; i++)
                    {
                        Goods goods = goodsList[i];
                        CrystalButton btn = btnItemList[i];
                        btn.Visible = true;
                        if (m_ShowSilverCode)
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
                    btnHead.Enabled = false;
                    btnHead.BackColor = ConstantValuePool.DisabledColor;
                    btnBack.Enabled = false;
                    btnBack.BackColor = ConstantValuePool.DisabledColor;

                    this.pnlItem.ResumeLayout(false);
                    this.pnlItem.PerformLayout();
                    this.ResumeLayout(false);
                }
            }
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
                        Guid departID = Guid.Empty;
                        int itemType = Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value);
                        if (itemType == (int)OrderItemType.Goods)
                        {
                            Goods _goods = dgvGoodsOrder.Rows[selectIndex].Cells["ItemID"].Tag as Goods;
                            printSolutionName = _goods.PrintSolutionName;
                            departID = _goods.DepartID;
                        }
                        else if (itemType == (int)OrderItemType.Details)
                        {
                            Details _details = dgvGoodsOrder.Rows[selectIndex].Cells["ItemID"].Tag as Details;
                            printSolutionName = _details.PrintSolutionName;
                            departID = _details.DepartID;
                        }
                        Details details = new Details();
                        details.DetailsID = new Guid("77777777-7777-7777-7777-777777777777");
                        details.DetailsNo = "7777";
                        details.DetailsName = details.DetailsName2nd = form.CustomTasteName.Replace("-", "");
                        details.SellPrice = 0;
                        details.CanDiscount = false;
                        details.AutoShowDetails = false;
                        details.PrintSolutionName = printSolutionName;
                        details.DepartID = departID;
                        //数量
                        decimal itemNum = Convert.ToDecimal(dgvGoodsOrder.Rows[selectIndex].Cells["GoodsNum"].Value);
                        DataGridViewRow dgr = dgvGoodsOrder.Rows[0].Clone() as DataGridViewRow;
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
                            if (Convert.ToInt32(dgvGoodsOrder.Rows[selectIndex].Cells["ItemType"].Value) == (int)OrderItemType.Goods)
                            {
                                for (int i = selectIndex + 1; i < dgvGoodsOrder.RowCount; i++)
                                {
                                    itemType = Convert.ToInt32(dgvGoodsOrder.Rows[i].Cells["ItemType"].Value);
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
            }
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.Rows.Count <= 0)
            {
                MessageBox.Show("请先选择菜品！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!haveDailyClose)
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
                    MessageBox.Show("上次未日结，不能新增菜单，请先进行日结操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
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
            string deskName = string.Empty;
            if (m_SalesOrder == null)
            {
                if (ConstantValuePool.BizSettingConfig.CarteMode)
                {
                    FormNumericKeypad form = new FormNumericKeypad();
                    form.DisplayText = "请输入餐牌号";
                    form.ShowDialog();
                    if (string.IsNullOrEmpty(form.KeypadValue))
                    {
                        MessageBox.Show("餐牌号不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        if (form.KeypadValue.Length > 3)
                        {
                            MessageBox.Show("您输入的餐牌号码过大！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            deskName = "W" + form.KeypadValue.PadLeft(3, '0');
                        }
                    }
                }
                else
                {
                    deskName = "W001";
                }
            }
            else
            {
                deskName = m_SalesOrder.order.DeskName;
            }
            int result = SubmitSalesOrder(deskName, EatWayType.Takeout);
            if (result == 0)
            {
                MessageBox.Show("提交菜单信息出现异常，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //转入结账页面
                FormPayment checkForm = new FormPayment(m_SalesOrder);
                checkForm.ShowDialog();
                if (checkForm.IsPaidOrder)
                {
                    this.lbTotalPrice.Text = "总金额：";
                    this.lbDiscount.Text = "折扣：";
                    this.lbNeedPayMoney.Text = "实际应付：";
                    this.lbCutOff.Text = "去零：";
                    dgvGoodsOrder.Rows.Clear();
                    m_SalesOrder = null;
                    btnDeliveryGoods.Enabled = false;
                    btnDeliveryGoods.BackColor = ConstantValuePool.DisabledColor;
                    txtTelephone.Text = string.Empty;
                    txtName.Text = string.Empty;
                    txtAddress.Text = string.Empty;
                    txtTelephone.ReadOnly = false;
                    txtName.ReadOnly = false;
                    //加载外卖单列表
                    IList<DeliveryOrder> deliveryOrderList = OrderService.GetInstance().GetDeliveryOrderList();
                    if (deliveryOrderList != null)
                    {
                        m_PageIndex = 0;
                        m_DeliveryOrderList = deliveryOrderList;
                        DisplayDeliveryOrderButton();
                    }
                    //更新第二屏信息
                    if (Screen.AllScreens.Length > 1 && ConstantValuePool.BizSettingConfig.SecondScreenEnabled)
                    {
                        if (ConstantValuePool.SecondScreenForm != null && ConstantValuePool.SecondScreenForm is FormSecondScreen)
                        {
                            ((FormSecondScreen)ConstantValuePool.SecondScreenForm).ClearGoodsOrderInfo();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 判断是否存在限时组合销售
        /// </summary>
        /// <param name="dgvGoodsOrder"></param>
        private void JoinGoodsCombinedSale(DataGridView dgvGoodsOrder)
        {
            List<Guid> hasExistGoodsId = new List<Guid>();
            for (int index = 0; index < dgvGoodsOrder.Rows.Count; index++)
            {
                DataGridViewRow dr = dgvGoodsOrder.Rows[index];
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
                    bool IsInItem = false;
                    foreach (GoodsCombinedSale item in ConstantValuePool.GoodsCombinedSaleList)
                    {
                        if (item.ItemID == goodsId)
                        {
                            if (!IsValidDate(item.BeginDate, item.EndDate, item.Week, item.Day, item.Hour, item.Minute))
                            {
                                continue;
                            }
                            //组合优惠
                            if (goodsQty < item.Quantity)
                            {
                                continue;
                            }
                            decimal discountRate = Math.Abs(totalDiscount) / totalSellPrice;
                            if (discountRate > item.LeastDiscountRate)
                            {
                                continue;
                            }
                            bool IsFit = false;
                            if (item.MoreOrLess == 1)
                            {
                                IsFit = totalSellPrice / goodsQty > item.SellPrice;
                            }
                            if (item.MoreOrLess == 2)
                            {
                                IsFit = totalSellPrice / goodsQty == item.SellPrice;
                            }
                            if (item.MoreOrLess == 3)
                            {
                                IsFit = totalSellPrice / goodsQty < item.SellPrice;
                            }
                            if (!IsFit) continue;
                            //优惠区间 第二杯半价
                            if (item.PreferentialInterval == 2)
                            {
                                int count = 0;
                                for (int m = index + 1; m < dgvGoodsOrder.Rows.Count; m++)
                                {
                                    //限时组合销售只针对主品项
                                    if (Convert.ToInt32(dgvGoodsOrder.Rows[m].Cells["ItemType"].Value) == (int)OrderItemType.Goods)
                                    {
                                        Guid tempGoodsId = new Guid(dgvGoodsOrder.Rows[m].Cells["ItemID"].Value.ToString());
                                        decimal tempTotalPrice = Convert.ToDecimal(dgvGoodsOrder.Rows[m].Cells["GoodsPrice"].Value);
                                        if (tempGoodsId == item.ItemID)
                                        {
                                            if (count % 2 == 1)
                                            {
                                                count++;
                                                continue;
                                            }
                                            Discount _discount = new Discount();
                                            _discount.DiscountID = new Guid("66666666-6666-6666-6666-666666666666");
                                            _discount.DiscountName = "促销折扣";
                                            decimal goodsDiscount = 0;
                                            if (item.DiscountType2 == (int)DiscountItemType.DiscountRate)
                                            {
                                                goodsDiscount = tempTotalPrice * item.DiscountRate2;
                                                _discount.DiscountType = (int)DiscountItemType.DiscountRate;
                                                _discount.DiscountRate = item.DiscountRate2;
                                                _discount.OffFixPay = 0;
                                            }
                                            if (item.DiscountType2 == (int)DiscountItemType.OffFixPay)
                                            {
                                                goodsDiscount = item.OffFixPay2;
                                                _discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                _discount.DiscountRate = 0;
                                                _discount.OffFixPay = item.OffFixPay2;
                                            }
                                            if (item.DiscountType2 == (int)DiscountItemType.OffSaleTo)
                                            {
                                                goodsDiscount = tempTotalPrice - item.OffSaleTo2;
                                                _discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                _discount.DiscountRate = 0;
                                                _discount.OffFixPay = tempTotalPrice - item.OffSaleTo2;
                                            }
                                            dgvGoodsOrder.Rows[m].Cells["GoodsDiscount"].Value = (-goodsDiscount).ToString("f2");
                                            dgvGoodsOrder.Rows[m].Cells["GoodsDiscount"].Tag = _discount;
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
                                for (int m = index + 1; m < dgvGoodsOrder.Rows.Count; m++)
                                {
                                    //限时组合销售只针对主品项
                                    if (Convert.ToInt32(dgvGoodsOrder.Rows[m].Cells["ItemType"].Value) == (int)OrderItemType.Goods)
                                    {
                                        Guid tempGoodsId = new Guid(dgvGoodsOrder.Rows[m].Cells["ItemID"].Value.ToString());
                                        decimal tempTotalPrice = Convert.ToDecimal(dgvGoodsOrder.Rows[m].Cells["GoodsPrice"].Value);
                                        if (tempGoodsId == item.ItemID)
                                        {
                                            if (count % 3 == 0)
                                            {
                                                count++;
                                                continue;
                                            }
                                            if (count % 3 == 1)
                                            {
                                                Discount _discount = new Discount();
                                                _discount.DiscountID = new Guid("66666666-6666-6666-6666-666666666666");
                                                _discount.DiscountName = "促销折扣";
                                                decimal goodsDiscount = 0;
                                                if (item.DiscountType2 == (int)DiscountItemType.DiscountRate)
                                                {
                                                    goodsDiscount = tempTotalPrice * item.DiscountRate2;
                                                    _discount.DiscountType = (int)DiscountItemType.DiscountRate;
                                                    _discount.DiscountRate = item.DiscountRate2;
                                                    _discount.OffFixPay = 0;
                                                }
                                                if (item.DiscountType2 == (int)DiscountItemType.OffFixPay)
                                                {
                                                    goodsDiscount = item.OffFixPay2;
                                                    _discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                    _discount.DiscountRate = 0;
                                                    _discount.OffFixPay = item.OffFixPay2;
                                                }
                                                if (item.DiscountType2 == (int)DiscountItemType.OffSaleTo)
                                                {
                                                    goodsDiscount = tempTotalPrice - item.OffSaleTo2;
                                                    _discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                    _discount.DiscountRate = 0;
                                                    _discount.OffFixPay = tempTotalPrice - item.OffSaleTo2;
                                                }
                                                dgvGoodsOrder.Rows[m].Cells["GoodsDiscount"].Value = (-goodsDiscount).ToString("f2");
                                                dgvGoodsOrder.Rows[m].Cells["GoodsDiscount"].Tag = _discount;
                                                count++;
                                                continue;
                                            }
                                            if (count % 3 == 2)
                                            {
                                                Discount _discount = new Discount();
                                                _discount.DiscountID = new Guid("66666666-6666-6666-6666-666666666666");
                                                _discount.DiscountName = "促销折扣";
                                                decimal goodsDiscount = 0;
                                                if (item.DiscountType3 == (int)DiscountItemType.DiscountRate)
                                                {
                                                    goodsDiscount = tempTotalPrice * item.DiscountRate3;
                                                    _discount.DiscountType = (int)DiscountItemType.DiscountRate;
                                                    _discount.DiscountRate = item.DiscountRate3;
                                                    _discount.OffFixPay = 0;
                                                }
                                                if (item.DiscountType3 == (int)DiscountItemType.OffFixPay)
                                                {
                                                    goodsDiscount = item.OffFixPay3;
                                                    _discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                    _discount.DiscountRate = 0;
                                                    _discount.OffFixPay = item.OffFixPay3;
                                                }
                                                if (item.DiscountType3 == (int)DiscountItemType.OffSaleTo)
                                                {
                                                    goodsDiscount = tempTotalPrice - item.OffSaleTo3;
                                                    _discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                    _discount.DiscountRate = 0;
                                                    _discount.OffFixPay = tempTotalPrice - item.OffSaleTo3;
                                                }
                                                dgvGoodsOrder.Rows[m].Cells["GoodsDiscount"].Value = (-goodsDiscount).ToString("f2");
                                                dgvGoodsOrder.Rows[m].Cells["GoodsDiscount"].Tag = _discount;
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
                            IsInItem = true;
                            hasExistGoodsId.Add(goodsId);
                            break;
                        }
                    }
                    if (!IsInItem)
                    {
                        //在品项组中是否存在
                        foreach (GoodsCombinedSale item in ConstantValuePool.GroupCombinedSaleList)
                        {
                            if (!IsValidDate(item.BeginDate, item.EndDate, item.Week, item.Day, item.Hour, item.Minute))
                            {
                                continue;
                            }
                            if (GoodsUtil.IsGoodsInGroup(goodsId, item.ItemID))
                            {
                                if (!IsValidDate(item.BeginDate, item.EndDate, item.Week, item.Day, item.Hour, item.Minute))
                                {
                                    continue;
                                }
                                //组合优惠
                                if (goodsQty < item.Quantity)
                                {
                                    continue;
                                }
                                decimal discountRate = Math.Abs(totalDiscount) / totalSellPrice;
                                if (discountRate > item.LeastDiscountRate)
                                {
                                    continue;
                                }
                                bool IsFit = false;
                                if (item.MoreOrLess == 1)
                                {
                                    IsFit = totalSellPrice / goodsQty > item.SellPrice;
                                }
                                if (item.MoreOrLess == 2)
                                {
                                    IsFit = totalSellPrice / goodsQty == item.SellPrice;
                                }
                                if (item.MoreOrLess == 3)
                                {
                                    IsFit = totalSellPrice / goodsQty < item.SellPrice;
                                }
                                if (!IsFit) continue;
                                //优惠区间 第二杯半价
                                if (item.PreferentialInterval == 2)
                                {
                                    int count = 0;
                                    for (int m = index + 1; m < dgvGoodsOrder.Rows.Count; m++)
                                    {
                                        //限时组合销售只针对主品项
                                        if (Convert.ToInt32(dgvGoodsOrder.Rows[m].Cells["ItemType"].Value) == (int)OrderItemType.Goods)
                                        {
                                            Guid tempGoodsId = new Guid(dgvGoodsOrder.Rows[m].Cells["ItemID"].Value.ToString());
                                            decimal tempTotalPrice = Convert.ToDecimal(dgvGoodsOrder.Rows[m].Cells["GoodsPrice"].Value);
                                            if (GoodsUtil.IsGoodsInGroup(tempGoodsId, item.ItemID))
                                            {
                                                if (count % 2 == 1)
                                                {
                                                    count++;
                                                    continue;
                                                }
                                                Discount _discount = new Discount();
                                                _discount.DiscountID = new Guid("66666666-6666-6666-6666-666666666666");
                                                _discount.DiscountName = "促销折扣";
                                                decimal goodsDiscount = 0;
                                                if (item.DiscountType2 == (int)DiscountItemType.DiscountRate)
                                                {
                                                    goodsDiscount = tempTotalPrice * item.DiscountRate2;
                                                    _discount.DiscountType = (int)DiscountItemType.DiscountRate;
                                                    _discount.DiscountRate = item.DiscountRate2;
                                                    _discount.OffFixPay = 0;
                                                }
                                                if (item.DiscountType2 == (int)DiscountItemType.OffFixPay)
                                                {
                                                    goodsDiscount = item.OffFixPay2;
                                                    _discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                    _discount.DiscountRate = 0;
                                                    _discount.OffFixPay = item.OffFixPay2;
                                                }
                                                if (item.DiscountType2 == (int)DiscountItemType.OffSaleTo)
                                                {
                                                    goodsDiscount = tempTotalPrice - item.OffSaleTo2;
                                                    _discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                    _discount.DiscountRate = 0;
                                                    _discount.OffFixPay = tempTotalPrice - item.OffSaleTo2;
                                                }
                                                dgvGoodsOrder.Rows[m].Cells["GoodsDiscount"].Value = (-goodsDiscount).ToString("f2");
                                                dgvGoodsOrder.Rows[m].Cells["GoodsDiscount"].Tag = _discount;
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
                                    for (int m = index + 1; m < dgvGoodsOrder.Rows.Count; m++)
                                    {
                                        //限时组合销售只针对主品项
                                        if (Convert.ToInt32(dgvGoodsOrder.Rows[m].Cells["ItemType"].Value) == (int)OrderItemType.Goods)
                                        {
                                            Guid tempGoodsId = new Guid(dgvGoodsOrder.Rows[m].Cells["ItemID"].Value.ToString());
                                            decimal tempTotalPrice = Convert.ToDecimal(dgvGoodsOrder.Rows[m].Cells["GoodsPrice"].Value);
                                            if (GoodsUtil.IsGoodsInGroup(tempGoodsId, item.ItemID))
                                            {
                                                if (count % 3 == 0)
                                                {
                                                    count++;
                                                    continue;
                                                }
                                                if (count % 3 == 1)
                                                {
                                                    Discount _discount = new Discount();
                                                    _discount.DiscountID = new Guid("66666666-6666-6666-6666-666666666666");
                                                    _discount.DiscountName = "促销折扣";
                                                    decimal goodsDiscount = 0;
                                                    if (item.DiscountType2 == (int)DiscountItemType.DiscountRate)
                                                    {
                                                        goodsDiscount = tempTotalPrice * item.DiscountRate2;
                                                        _discount.DiscountType = (int)DiscountItemType.DiscountRate;
                                                        _discount.DiscountRate = item.DiscountRate2;
                                                        _discount.OffFixPay = 0;
                                                    }
                                                    if (item.DiscountType2 == (int)DiscountItemType.OffFixPay)
                                                    {
                                                        goodsDiscount = item.OffFixPay2;
                                                        _discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                        _discount.DiscountRate = 0;
                                                        _discount.OffFixPay = item.OffFixPay2;
                                                    }
                                                    if (item.DiscountType2 == (int)DiscountItemType.OffSaleTo)
                                                    {
                                                        goodsDiscount = tempTotalPrice - item.OffSaleTo2;
                                                        _discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                        _discount.DiscountRate = 0;
                                                        _discount.OffFixPay = tempTotalPrice - item.OffSaleTo2;
                                                    }
                                                    dgvGoodsOrder.Rows[m].Cells["GoodsDiscount"].Value = (-goodsDiscount).ToString("f2");
                                                    dgvGoodsOrder.Rows[m].Cells["GoodsDiscount"].Tag = _discount;
                                                    count++;
                                                    continue;
                                                }
                                                if (count % 3 == 2)
                                                {
                                                    Discount _discount = new Discount();
                                                    _discount.DiscountID = new Guid("66666666-6666-6666-6666-666666666666");
                                                    _discount.DiscountName = "促销折扣";
                                                    decimal goodsDiscount = 0;
                                                    if (item.DiscountType3 == (int)DiscountItemType.DiscountRate)
                                                    {
                                                        goodsDiscount = tempTotalPrice * item.DiscountRate3;
                                                        _discount.DiscountType = (int)DiscountItemType.DiscountRate;
                                                        _discount.DiscountRate = item.DiscountRate3;
                                                        _discount.OffFixPay = 0;
                                                    }
                                                    if (item.DiscountType3 == (int)DiscountItemType.OffFixPay)
                                                    {
                                                        goodsDiscount = item.OffFixPay3;
                                                        _discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                        _discount.DiscountRate = 0;
                                                        _discount.OffFixPay = item.OffFixPay3;
                                                    }
                                                    if (item.DiscountType3 == (int)DiscountItemType.OffSaleTo)
                                                    {
                                                        goodsDiscount = tempTotalPrice - item.OffSaleTo3;
                                                        _discount.DiscountType = (int)DiscountItemType.OffFixPay;
                                                        _discount.DiscountRate = 0;
                                                        _discount.OffFixPay = tempTotalPrice - item.OffSaleTo3;
                                                    }
                                                    dgvGoodsOrder.Rows[m].Cells["GoodsDiscount"].Value = (-goodsDiscount).ToString("f2");
                                                    dgvGoodsOrder.Rows[m].Cells["GoodsDiscount"].Tag = _discount;
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
                                break;
                            }
                        }
                    }
                }
            }
        }

        private bool IsValidDate(string beginDate, string endDate, string week, string day, string hour, string minute)
        {
            bool IsValid = true;
            if (DateTime.Now >= DateTime.Parse(beginDate) && DateTime.Now <= DateTime.Parse(endDate))
            {
                DayOfWeek curWeek = DateTime.Now.DayOfWeek;
                string curMonth = DateTime.Now.Month.ToString();
                string curDay = DateTime.Now.Day.ToString();
                int curHour = DateTime.Now.Hour;
                int curMinute = DateTime.Now.Minute;
                //判断周或者日
                if (week == "?")
                {
                    //判断是否包含当日
                    if (day != "*")
                    {
                        string[] dayArr = day.Split(',');
                        bool IsContainDay = false;
                        foreach (string item in dayArr)
                        {
                            if (curDay == item)
                            {
                                IsContainDay = true;
                                break;
                            }
                        }
                        if (!IsContainDay)
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
                        DateTime FirstofMonth = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + "01");
                        int i = (int)FirstofMonth.Date.DayOfWeek;
                        if (i == 0)
                        {
                            i = 7;
                        }
                        int curWeekIndex = (DateTime.Now.Day + i - 1) / 7;
                        if (curWeekIndex != int.Parse(weekIndex))
                        {
                            return false;
                        }
                        else
                        {
                            if ((int)curWeek != int.Parse(weekDay))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //不包含# 例:当月每个星期几
                        string[] weekArr = week.Split(',');
                        bool IsContainWeek = false;
                        foreach (string item in weekArr)
                        {
                            if ((int)curWeek == int.Parse(item))
                            {
                                IsContainWeek = true;
                                break;
                            }
                        }
                        if (!IsContainWeek)
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
                            bool IsContainHour = false;
                            foreach (string item in hourArr)
                            {
                                string beginHour = item.Split('-')[0].Trim();
                                string endHour = item.Split('-')[1].Trim();
                                if (string.Compare(hourMinute, beginHour) >= 0 && string.Compare(hourMinute, endHour) <= 0)
                                {
                                    IsContainHour = true;
                                    break;
                                }
                            }
                            if (!IsContainHour)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            string beginHour = hour.Split('-')[0].Trim();
                            string endHour = hour.Split('-')[1].Trim();
                            if (string.Compare(hourMinute, beginHour) < 0 || string.Compare(hourMinute, endHour) > 0)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (hour.IndexOf(',') > 0) //多个小时
                        {
                            string[] hourArr = hour.Split(',');
                            bool IsContainHour = false;
                            foreach (string item in hourArr)
                            {
                                if (curHour == int.Parse(item))
                                {
                                    IsContainHour = true;
                                    break;
                                }
                            }
                            if (!IsContainHour)
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
                            bool IsContainMinute = false;
                            foreach (string item in minuteArr)
                            {
                                string beginMinute = item.Split('-')[0];
                                string endMinute = item.Split('-')[1];
                                if (curMinute >= int.Parse(beginMinute) && curMinute <= int.Parse(endMinute))
                                {
                                    IsContainMinute = true;
                                    break;
                                }
                            }
                            if (!IsContainMinute)
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
                            string[] minuteArr = minute.Split(',');
                            bool IsContainMinute = false;
                            foreach (string item in minuteArr)
                            {
                                if (curMinute == int.Parse(item))
                                {
                                    IsContainMinute = true;
                                    break;
                                }
                            }
                            if (!IsContainMinute)
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
                IsValid = false;
            }
            return IsValid;
        }

        private void btnCustomPrice_Click(object sender, EventArgs e)
        {

        }

        private void btnRecentlyCall_Click(object sender, EventArgs e)
        {
            TakeawayCall.FormRecentlyCallRecord recentlyCallForm = new TakeawayCall.FormRecentlyCallRecord();
            recentlyCallForm.ShowDialog();
            if (string.IsNullOrEmpty(txtTelephone.Text))
            {
                string telephone = recentlyCallForm.CurTelephone;
                txtTelephone.Text = telephone;
                if (!string.IsNullOrEmpty(telephone))
                {
                    CustomerInfo customerInfo = CustomersService.GetInstance().GetCustomerInfoByPhone(telephone);
                    if (customerInfo != null)
                    {
                        string address = string.Empty;
                        if (customerInfo.ActiveIndex == 1)
                        {
                            address = customerInfo.DeliveryAddress1;
                        }
                        else if (customerInfo.ActiveIndex == 2)
                        {
                            address = customerInfo.DeliveryAddress2;
                        }
                        else if (customerInfo.ActiveIndex == 3)
                        {
                            address = customerInfo.DeliveryAddress3;
                        }
                        txtTelephone.Text = customerInfo.Telephone;
                        txtName.Text = customerInfo.CustomerName;
                        txtAddress.Text = address;
                        txtTelephone.ReadOnly = true;
                        txtName.ReadOnly = true;
                    }
                }
            }
        }

        private void btnRecords_Click(object sender, EventArgs e)
        {
            string telephone = this.txtTelephone.Text.Trim();
            if (!string.IsNullOrEmpty(telephone))
            {
                GoodsGroup tempGoodsGroup = null;
                if (m_CurrentGoodsGroup != null)
                {
                    //如果存在，则将对象暂存局部变量
                    tempGoodsGroup = CopyExtension.Clone<GoodsGroup>(m_CurrentGoodsGroup);
                }
                else
                {
                    m_CurrentGoodsGroup = new GoodsGroup();
                }
                TakeawayCall.FormHistoryCallOrder formHistoryOrder = new TakeawayCall.FormHistoryCallOrder(telephone, btnItem_Click, ref m_CurrentGoodsGroup);
                formHistoryOrder.ShowDialog();
                m_CurrentGoodsGroup = tempGoodsGroup;
            }
        }
    }
}