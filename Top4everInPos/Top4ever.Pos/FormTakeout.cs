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
using Top4ever.Domain.Customers;
using Top4ever.Domain.GoodsRelated;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;
using Top4ever.Entity.Enum;
using Top4ever.Print.Entity;
using Top4ever.Print;
using Top4ever.Pos.Feature;
using Top4ever.Pos.TakeawayCall;

namespace Top4ever.Pos
{
    public partial class FormTakeout : Form
    {
        private const string deskName = "W001";
        private const int m_Space = 2;
        private List<CrystalButton> btnDeliveryList = new List<CrystalButton>();
        private List<CrystalButton> btnGroupList = new List<CrystalButton>();
        private List<CrystalButton> btnItemList = new List<CrystalButton>();
        //�������б�
        private const int m_PageSize = 10;
        private int m_PageIndex = 0;
        //Ʒ�����б�
        private int m_GroupPageSize = 0;
        private int m_GroupPageIndex = 0;
        //Ʒ���б�
        private int m_ItemPageSize = 0;
        private int m_ItemPageIndex = 0;
        private IList<DeliveryOrder> m_DeliveryOrderList = new List<DeliveryOrder>();
        private GoodsGroup m_CurrentGoodsGroup;
        private DetailsGroup m_CurrentDetailsGroup;
        private IList<Guid> m_CurrentDetailsGroupIDList;
        private string m_DetailsPrefix = string.Empty;
        private bool m_GoodsOrDetails = true;
        /// <summary>
        /// �ύ�Ķ�����Ϣ
        /// </summary>
        private SalesOrder m_SalesOrder;
        private decimal m_TotalPrice = 0;
        private decimal m_ActualPayMoney = 0;
        private decimal m_Discount = 0;
        private decimal m_CutOff = 0;
        private CrystalButton prevPressedButton = null;
        private bool m_ShowSilverCode = false;

        private bool m_OnShow = false;
        public bool VisibleShow
        {
            set { m_OnShow = value; }
        }

        public FormTakeout()
        {
            InitializeComponent();
            btnPageUp.DisplayColor = btnPageUp.BackColor;
            btnPageDown.DisplayColor = btnPageDown.BackColor;
            btnHead.DisplayColor = btnHead.BackColor;
            btnBack.DisplayColor = btnBack.BackColor;
            btnPgUp.DisplayColor = btnPgUp.BackColor;
            btnPgDown.DisplayColor = btnPgDown.BackColor;
            btnDeliveryGoods.DisplayColor = btnDeliveryGoods.BackColor;
            btnOutsideOrder.DisplayColor = btnOutsideOrder.BackColor;
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
            //����Size
            int locPX = this.pnlCodeSearch.Width - this.btnCheckout.Width - 5;
            this.btnCheckout.Location = new Point(locPX, 0);
            int distance = this.pnlCodeSearch.Width - 497;
            txtSearch.Width = 200 + distance;
            //����Size
            distance = this.pnlCustomerInfo.Width - 1024;
            int locWidth = 325 + distance;
            txtAddress.Width = locWidth;
            locPX = txtAddress.Location.X + locWidth + 8;
            btnRecords.Location = new Point(locPX, btnAddress.Location.Y);
            locPX += btnRecords.Width + 2;
            btnRecentlyCall.Location = new Point(locPX, btnAddress.Location.Y);
        }

        private void FormTakeout_VisibleChanged(object sender, EventArgs e)
        {
            if (m_OnShow)
            {
                //��ʼ����������ť
                InitDeliveryButton();
                //��ʼ��Ʒ���鰴ť
                InitializeGroupButton();
                //��ʼ��Ʒ�ť
                InitializeItemButton();
                //��ʼ��
                LoadDefaultGoodsGroupButton();
                m_GoodsOrDetails = true;
                m_DetailsPrefix = string.Empty;
                prevPressedButton = null;
                m_ShowSilverCode = false;
                //���
                txtTelephone.Text = string.Empty;
                txtName.Text = string.Empty;
                txtAddress.Text = string.Empty;
                btnDeliveryGoods.Enabled = false;
                btnDeliveryGoods.BackColor = ConstantValuePool.DisabledColor;
                //�����������б�
                OrderService orderService = new OrderService();
                IList<DeliveryOrder> deliveryOrderList = orderService.GetDeliveryOrderList();
                if (deliveryOrderList != null)
                {
                    m_PageIndex = 0;
                    m_DeliveryOrderList = deliveryOrderList;
                    DisplayDeliveryOrderButton();
                }
            }
        }

        #region ��ʼ��

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
                    btnDelivery.Font = new Font("Microsoft YaHei", 9.75F);
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
                            m_GroupPageSize = control.ColumnsCount * control.RowsCount - 2;    //�۳���ǰ�����������ť
                            //����
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
                                //����Buttonλ��
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
                            //��ǰ
                            this.pnlGroup.Controls.Add(btnPageUp);
                            //���
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
                            m_ItemPageSize = control.ColumnsCount * control.RowsCount - 2;    //�۳���ǰ�����������ť
                            //����
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
                                //����Buttonλ��
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
                            //��ǰ
                            this.pnlItem.Controls.Add(btnHead);
                            //���
                            this.pnlItem.Controls.Add(btnBack);
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region ��ʾ��ť

        private void DisplayGoodsGroupButton()
        {
            //��ֹ����Layout�¼�
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
            //����û�����ݵİ�ť
            for (int i = btnGroupList.Count - unDisplayNum; i < btnGroupList.Count; i++)
            {
                btnGroupList[i].Visible = false;
            }
            //��ʾ�����ݵİ�ť
            for (int i = 0, j = startIndex; j < endIndex; i++, j++)
            {
                GoodsGroup goodsGroup = ConstantValuePool.GoodsGroupList[j];
                CrystalButton btn = btnGroupList[i];
                btn.Visible = true;
                btn.Text = goodsGroup.GoodsGroupName;
                btn.Tag = goodsGroup;
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
            //����ҳ�밴ť����ʾ
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
                    //��ֹ����Layout�¼�
                    this.pnlItem.SuspendLayout();
                    this.SuspendLayout();

                    //��ʾ�ؼ�
                    int unDisplayNum = 0;
                    int startIndex = m_ItemPageIndex * m_ItemPageSize;
                    int endIndex = (m_ItemPageIndex + 1) * m_ItemPageSize;
                    if (endIndex > m_CurrentGoodsGroup.GoodsList.Count)
                    {
                        unDisplayNum = endIndex - m_CurrentGoodsGroup.GoodsList.Count;
                        endIndex = m_CurrentGoodsGroup.GoodsList.Count;
                    }
                    //����û�����ݵİ�ť
                    for (int i = btnItemList.Count - unDisplayNum; i < btnItemList.Count; i++)
                    {
                        btnItemList[i].Visible = false;
                    }
                    //��ʾ�����ݵİ�ť
                    for (int i = 0, j = startIndex; j < endIndex; i++, j++)
                    {
                        Goods goods = m_CurrentGoodsGroup.GoodsList[j];
                        CrystalButton btn = btnItemList[i];
                        btn.Visible = true;
                        if (m_ShowSilverCode)
                        {
                            btn.Text = goods.GoodsName + "\r\n ��" + goods.SellPrice.ToString("f2");
                        }
                        else
                        {
                            btn.Text = goods.GoodsName;
                        }
                        btn.Tag = goods;
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
                    //����ҳ�밴ť����ʾ
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
                if (m_CurrentDetailsGroupIDList.Contains(item.DetailsGroupID))
                {
                    detailGroupList.Add(item);
                }
            }
            if (detailGroupList.Count > 0)
            {
                //��ֹ����Layout�¼�
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
                //����û�����ݵİ�ť
                for (int i = btnGroupList.Count - unDisplayNum; i < btnGroupList.Count; i++)
                {
                    btnGroupList[i].Visible = false;
                }
                //��ʾ�����ݵİ�ť
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
                //����ҳ�밴ť����ʾ
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
                    //��ֹ����Layout�¼�
                    this.pnlItem.SuspendLayout();
                    this.SuspendLayout();

                    //��ʾ�ؼ�
                    int unDisplayNum = 0;
                    int startIndex = m_ItemPageIndex * m_ItemPageSize;
                    int endIndex = (m_ItemPageIndex + 1) * m_ItemPageSize;
                    if (endIndex > m_CurrentDetailsGroup.DetailsList.Count)
                    {
                        unDisplayNum = endIndex - m_CurrentDetailsGroup.DetailsList.Count;
                        endIndex = m_CurrentDetailsGroup.DetailsList.Count;
                    }
                    //����û�����ݵİ�ť
                    for (int i = btnItemList.Count - unDisplayNum; i < btnItemList.Count; i++)
                    {
                        btnItemList[i].Visible = false;
                    }
                    //��ʾ�����ݵİ�ť
                    for (int i = 0, j = startIndex; j < endIndex; i++, j++)
                    {
                        Details details = m_CurrentDetailsGroup.DetailsList[j];
                        CrystalButton btn = btnItemList[i];
                        btn.Visible = true;
                        if (m_ShowSilverCode)
                        {
                            btn.Text = details.DetailsName + "\r\n ��" + details.SellPrice.ToString("f2");
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
                    //����ҳ�밴ť����ʾ
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
            //��ֹ����Layout�¼�
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
                    prevPressedButton.BackColor = prevPressedButton.DisplayColor;
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

                #region �ж��Ƿ��ײ�
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
                        //�����б�
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

                        #region �ж��ײ����Ƿ�ϸ��
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
                                //�����б�
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

                #region �ж��Ƿ��Զ���ʾϸ����
                if (goods.AutoShowDetails)
                {
                    m_GroupPageIndex = 0;
                    m_ItemPageIndex = 0;
                    if (goods.DetailsGroupIDList != null && goods.DetailsGroupIDList.Count > 0)
                    {
                        m_GoodsOrDetails = false;    //״̬Ϊϸ��
                        m_CurrentDetailsGroupIDList = goods.DetailsGroupIDList;
                        DisplayDetailGroupButton();
                        HideItemButton();
                        m_DetailsPrefix = "--";
                    }
                }
                #endregion

                //datagridview��������λ
                dgvGoodsOrder.Rows[selectedIndex].Selected = true;
                dgvGoodsOrder.CurrentCell = dgvGoodsOrder.Rows[selectedIndex].Cells["GoodsNum"];
                //ͳ��
                BindOrderInfoSum();
            }
            if (btnItem.Tag is Details)
            {
                Details details = btnItem.Tag as Details;
                if (dgvGoodsOrder.CurrentRow != null)
                {
                    int selectIndex = dgvGoodsOrder.CurrentRow.Index;
                    //����
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
                    //ͳ��
                    BindOrderInfoSum();
                }
            }
        }

        #endregion

        #region ��ҳ�¼�

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
            this.lbTotalPrice.Text = "�ܽ�" + totalPrice.ToString("f2");
            this.lbDiscount.Text = "�ۿۣ�" + totalDiscount.ToString("f2");
            decimal wholePayMoney = totalPrice + totalDiscount;
            decimal actualPayMoney = CutOffDecimal.HandleCutOff(wholePayMoney, CutOffType.ROUND_OFF, 0);
            m_ActualPayMoney = actualPayMoney;
            m_CutOff = wholePayMoney - actualPayMoney;
            this.lbNeedPayMoney.Text = "ʵ��Ӧ����" + actualPayMoney.ToString("f2");
            this.lbCutOff.Text = "ȥ�㣺" + (-m_CutOff).ToString("f2");
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
                //Ȩ����֤
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
                if (itemType == (int)OrderItemType.Goods)   //������ܴ���
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
                            //����ϸ��
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
                            //ͳ��
                            BindOrderInfoSum();
                        }
                    }
                }
            }
        }

        private void btnWholeDiscount_Click(object sender, EventArgs e)
        {
            //Ȩ����֤
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
                //ͳ��
                BindOrderInfoSum();
            }
        }

        private void btnTakeOut_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.Rows.Count > 0)
            {
                if (m_SalesOrder == null || m_SalesOrder.order.EatType == (int)EatWayType.Takeout)
                {
                    int result = SubmitSalesOrder(deskName, EatWayType.Takeout);
                    if (result == 1)
                    {
                        this.lbTotalPrice.Text = "�ܽ�";
                        this.lbDiscount.Text = "�ۿۣ�";
                        this.lbNeedPayMoney.Text = "ʵ��Ӧ����";
                        this.lbCutOff.Text = "ȥ�㣺";
                        dgvGoodsOrder.Rows.Clear();
                        m_SalesOrder = null;
                        btnDeliveryGoods.Enabled = false;
                        btnDeliveryGoods.BackColor = ConstantValuePool.DisabledColor;
                        txtTelephone.Text = string.Empty;
                        txtName.Text = string.Empty;
                        txtAddress.Text = string.Empty;
                        txtTelephone.ReadOnly = false;
                        txtName.ReadOnly = false;
                        //�����������б�
                        OrderService orderService = new OrderService();
                        IList<DeliveryOrder> deliveryOrderList = orderService.GetDeliveryOrderList();
                        if (deliveryOrderList != null)
                        {
                            m_PageIndex = 0;
                            m_DeliveryOrderList = deliveryOrderList;
                            DisplayDeliveryOrderButton();
                        }
                    }
                    else if (result == 2)
                    {
                        MessageBox.Show("û�����ݿ����ύ��", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("����ύʧ�ܣ������²�����", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("���Ͳ�һ�£���������ͺ��ٽ��в�����", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBeNew_Click(object sender, EventArgs e)
        {
            this.lbTotalPrice.Text = "�ܽ�";
            this.lbDiscount.Text = "�ۿۣ�";
            this.lbNeedPayMoney.Text = "ʵ��Ӧ����";
            this.lbCutOff.Text = "ȥ�㣺";
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
        }

        private void btnOutsideOrder_Click(object sender, EventArgs e)
        {
            if (dgvGoodsOrder.Rows.Count > 0)
            {
                if (m_SalesOrder == null || m_SalesOrder.order.EatType == (int)EatWayType.OutsideOrder)
                {
                    int result = SubmitSalesOrder(deskName, EatWayType.OutsideOrder);
                    if (result == 1)
                    {
                        this.lbTotalPrice.Text = "�ܽ�";
                        this.lbDiscount.Text = "�ۿۣ�";
                        this.lbNeedPayMoney.Text = "ʵ��Ӧ����";
                        this.lbCutOff.Text = "ȥ�㣺";
                        dgvGoodsOrder.Rows.Clear();
                        m_SalesOrder = null;
                        btnDeliveryGoods.Enabled = false;
                        btnDeliveryGoods.BackColor = ConstantValuePool.DisabledColor;
                        txtTelephone.Text = string.Empty;
                        txtName.Text = string.Empty;
                        txtAddress.Text = string.Empty;
                        txtTelephone.ReadOnly = false;
                        txtName.ReadOnly = false;
                        //�����������б�
                        OrderService orderService = new OrderService();
                        IList<DeliveryOrder> deliveryOrderList = orderService.GetDeliveryOrderList();
                        if (deliveryOrderList != null)
                        {
                            m_PageIndex = 0;
                            m_DeliveryOrderList = deliveryOrderList;
                            DisplayDeliveryOrderButton();
                        }
                    }
                    else if (result == 2)
                    {
                        MessageBox.Show("û�����ݿ����ύ��", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("�����ύʧ�ܣ������²�����", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("���Ͳ�һ�£���������ͺ��ٽ��в�����", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDeliveryGoods_Click(object sender, EventArgs e)
        {
            string telPhone = this.txtTelephone.Text;
            string customerName = this.txtName.Text;
            string address = this.txtAddress.Text;
            FormDeliveryGoods form = new FormDeliveryGoods(m_SalesOrder, telPhone, customerName, address);
            form.ShowDialog();
            if (form.HasDeliveried)
            {
                btnOutsideOrder.Enabled = false;
                btnOutsideOrder.BackColor = ConstantValuePool.DisabledColor;
                btnDeliveryGoods.Enabled = false;
                btnDeliveryGoods.BackColor = ConstantValuePool.DisabledColor;
                //�����������б�
                OrderService orderService = new OrderService();
                IList<DeliveryOrder> deliveryOrderList = orderService.GetDeliveryOrderList();
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

        }

        private void btnPromotion_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            m_OnShow = false;
            this.Hide();
        }

        private void btnPriceCode_Click(object sender, EventArgs e)
        {
            m_ShowSilverCode = !m_ShowSilverCode;
            DisplayGoodsButton();
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            CrystalButton btnDelivery = sender as CrystalButton;
            if (btnDelivery.Tag != null)
            {
                DeliveryOrder deliveryOrder = btnDelivery.Tag as DeliveryOrder;
                if (deliveryOrder.PayTime == null)
                {
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
                    }
                    else
                    {
                        btnOutsideOrder.Enabled = true;
                        btnOutsideOrder.BackColor = btnOutsideOrder.DisplayColor;
                    }
                    SalesOrderService salesOrderService = new SalesOrderService();
                    SalesOrder _salesOrder = salesOrderService.GetSalesOrder(deliveryOrder.OrderID);
                    if (_salesOrder != null)
                    {
                        m_SalesOrder = _salesOrder;
                        BindGoodsOrderInfo();   //�󶨶�����Ϣ
                        BindOrderInfoSum();
                        CustomersService customerService = new CustomersService();
                        CustomerOrder customerOrder = customerService.GetCustomerOrder(m_SalesOrder.order.OrderID);
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
                        MessageBox.Show("��ȡ�˵���Ϣʧ�ܣ������²�����", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        }

        /// <summary>
        /// 0:�ύʧ�� 1:�ɹ� 2:û�п��ύ������
        /// </summary>
        private Int32 SubmitSalesOrder(string deskName, EatWayType eatType)
        {
            int result = 0;
            Guid orderID = Guid.Empty;
            if (m_SalesOrder == null)    //�����Ĳ˵�
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
                    //���OrderDetails
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
                    //���OrderDiscount
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
                    //�޸Ĺ��ۿ۵��˵�
                    Discount discount = dr.Cells["GoodsDiscount"].Tag as Discount;
                    if (discount != null)
                    {
                        Guid orderDetailsID = new Guid(dr.Cells["OrderDetailsID"].Value.ToString());
                        int itemType = Convert.ToInt32(dr.Cells["ItemType"].Value);
                        string goodsName = dr.Cells["GoodsName"].Value.ToString();
                        //���OrderDetails
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
                        //���OrderDiscount
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
            int _tranSeq = 0;
            if (m_SalesOrder == null)    //�����Ĳ˵�
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
                order.OrderLastTime = 0;

                SalesOrder salesOrder = new SalesOrder();
                salesOrder.order = order;
                salesOrder.orderDetailsList = newOrderDetailsList;
                salesOrder.orderDiscountList = newOrderDiscountList;
                SalesOrderService orderService = new SalesOrderService();
                _tranSeq = orderService.CreateSalesOrder(salesOrder);
                if (_tranSeq > 0)
                {
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
                    order.OrderLastTime = 0;
                    SalesOrder salesOrder = new SalesOrder();
                    salesOrder.order = order;
                    salesOrder.orderDetailsList = newOrderDetailsList;
                    salesOrder.orderDiscountList = newOrderDiscountList;
                    SalesOrderService orderService = new SalesOrderService();
                    if (orderService.UpdateSalesOrder(salesOrder) == 1)
                    {
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
                    //���������Ϣ
                    CustomerOrder customerOrder = new CustomerOrder();
                    customerOrder.OrderID = orderID;
                    customerOrder.Telephone = txtTelephone.Text.Trim();
                    customerOrder.CustomerName = txtName.Text.Trim();
                    customerOrder.Address = txtAddress.Text.Trim();
                    CustomersService customerService = new CustomersService();
                    if (customerService.CreateCustomerOrder(customerOrder))
                    {
                        result = 1;
                    }
                    else
                    {
                        MessageBox.Show("���������Ϣʧ�ܣ�", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            if (eatType == EatWayType.Takeout)
            {
                if (result == 1)
                {
                    if (ConstantValuePool.BizSettingConfig.printConfig.Enabled)
                    {
                        //��ӡ
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
            return result;
        }

        private void DisplayDeliveryOrderButton()
        {
            //��ֹ����Layout�¼�
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
            //����û�����ݵİ�ť
            for (int i = btnDeliveryList.Count - unDisplayNum; i < btnDeliveryList.Count; i++)
            {
                btnDeliveryList[i].Tag = null;
                btnDeliveryList[i].Text = string.Empty;
                btnDeliveryList[i].BackColor = btnDeliveryList[i].DisplayColor;
                btnDeliveryList[i].Enabled = true;
            }
            //��ʾ�����ݵİ�ť
            for (int i = 0, j = startIndex; j < endIndex; i++, j++)
            {
                btnDeliveryList[i].Tag = m_DeliveryOrderList[j];
                if (m_DeliveryOrderList[j].PayTime == null)
                {
                    if (m_DeliveryOrderList[j].EatType == (int)EatWayType.Takeout)
                    {
                        btnDeliveryList[i].BackColor = Color.Red;
                        btnDeliveryList[i].Text = m_DeliveryOrderList[j].TranSequence.ToString() + "-���";
                    }
                    if (m_DeliveryOrderList[j].EatType == (int)EatWayType.OutsideOrder)
                    {
                        if (m_DeliveryOrderList[j].DeliveryTime == null)
                        {
                            btnDeliveryList[i].BackColor = Color.Orange;
                            btnDeliveryList[i].Text = m_DeliveryOrderList[j].TranSequence.ToString() + "-δ����";
                        }
                        else
                        {
                            btnDeliveryList[i].BackColor = Color.Olive;
                            btnDeliveryList[i].Text = m_DeliveryOrderList[j].TranSequence.ToString() + "-�ѳ���";
                        }
                    }
                }
                else
                {
                    btnDeliveryList[i].Text = m_DeliveryOrderList[j].TranSequence + "\r\n " + Convert.ToDateTime(m_DeliveryOrderList[j].PayTime).ToString("MM-dd HH:mm");
                    btnDeliveryList[i].BackColor = Color.Green;
                }
            }
            //����ҳ�밴ť����ʾ
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
                FormIncomingCall form = new FormIncomingCall(telephone, address);
                form.ShowDialog();
                if (!string.IsNullOrEmpty(form.SelectedAddress))
                {
                    txtTelephone.Text = form.SelectedAddress;
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
                            m_GoodsOrDetails = false;    //״̬Ϊϸ��
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
                            m_GoodsOrDetails = false;    //״̬Ϊϸ��
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
                        //����ϸ��
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
                    //ͳ��
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
                        //����ϸ��
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
                    //ͳ��
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
                        MessageBox.Show("ϸ��ܵ���ɾ����", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (itemType == (int)OrderItemType.SetMeal)
                    {
                        MessageBox.Show("�ײ���ܵ���ɾ����", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                                if (DialogResult.Yes == MessageBox.Show("��ǰ�û����߱���Ȩ�ޣ����ҳ�������˲��޶�Ƿ�����û���", "��Ϣ��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                                {
                                    //Ȩ����֤
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
                            //����
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
                            //ϸ��
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
                            //����۸���Ϣ
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
                            decimal actualPayMoney = CutOffDecimal.HandleCutOff(wholePayMoney, CutOffType.ROUND_OFF, 0);
                            //����DeletedSingleOrder����
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
                                MessageBox.Show("ɾ��Ʒ��ʧ�ܣ�", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        else
                        {
                            List<int> deletedIndexList = new List<int>();
                            List<DeletedOrderDetails> deletedOrderDetailsList = new List<DeletedOrderDetails>();
                            //����
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
                            //ϸ��
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
                            //����۸���Ϣ
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
                            decimal actualPayMoney = CutOffDecimal.HandleCutOff(wholePayMoney, CutOffType.ROUND_OFF, 0);
                            //����DeletedSingleOrder����
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
                                for (int i = deletedIndexList.Count - 1; i >= 0; i--)
                                {
                                    dgvGoodsOrder.Rows.RemoveAt(deletedIndexList[i]);
                                }
                            }
                            else
                            {
                                MessageBox.Show("ɾ��Ʒ��ʧ�ܣ�", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        MessageBox.Show("�ײ���ܵ���ɾ����", "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                //ͳ��
                BindOrderInfoSum();
            }
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            if (m_SalesOrder != null)
            {
                //Ȩ����֤
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
                    //ɾ������
                    DeletedOrder deletedOrder = new DeletedOrder();
                    deletedOrder.OrderID = m_SalesOrder.order.OrderID;
                    deletedOrder.AuthorisedManager = ConstantValuePool.CurrentEmployee.EmployeeID;
                    deletedOrder.CancelEmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
                    deletedOrder.CancelReasonName = form.CurrentReason.ReasonName;

                    DeletedOrderService deletedOrderService = new DeletedOrderService();
                    if (deletedOrderService.DeleteWholeOrder(deletedOrder))
                    {
                        //�����������б�
                        OrderService orderService = new OrderService();
                        IList<DeliveryOrder> deliveryOrderList = orderService.GetDeliveryOrderList();
                        if (deliveryOrderList != null)
                        {
                            m_PageIndex = 0;
                            m_DeliveryOrderList = deliveryOrderList;
                            DisplayDeliveryOrderButton();
                        }
                    }
                    else
                    {
                        MessageBox.Show("ɾ���˵�ʧ�ܣ�");
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            this.lbTotalPrice.Text = "�ܽ�";
            this.lbDiscount.Text = "�ۿۣ�";
            this.lbNeedPayMoney.Text = "ʵ��Ӧ����";
            this.lbCutOff.Text = "ȥ�㣺";
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
        }
    }
}