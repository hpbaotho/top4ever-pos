using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Top4ever.Common;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;
using Top4ever.Entity.Enum;

namespace VechsoftPos
{
    public partial class FormSecondScreen : Form
    {
        public FormSecondScreen()
        {
            InitializeComponent();
            this.lbTotalPrice.Text = string.Empty;
            this.lbDiscount.Text = string.Empty;
            this.lbNeedPayMoney.Text = string.Empty;
            this.lbServiceFee1.Visible = false;
            this.lbServiceFee.Visible = false;
            //加载图片
            string strImage = ConstantValuePool.BizSettingConfig.ScreenImagePath;
            if (File.Exists(strImage))
            {
                this.pictureBox1.BackgroundImage = Image.FromFile(strImage);
                this.pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void FormSecondScreen_Load(object sender, EventArgs e)
        {
            if (Screen.AllScreens.Length > 1 && ConstantValuePool.BizSettingConfig.SecondScreenEnabled)
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.DesktopBounds = Screen.AllScreens[1].Bounds;
                this.WindowState = FormWindowState.Maximized;
                this.TopMost = true;
            }
            int px = (this.pnlBottom.Width - this.panel1.Width - this.lbCompany.Width) / 2;
            this.lbCompany.Location = new Point(px, 1);
            Play();
        }

        public void BindGoodsOrderInfo(DataGridView dgvGoodsOrder)
        {
            if (dgvGoodsOrder.Rows.Count > 0)
            {
                this.dgvItemOrder.Rows.Clear();
                foreach (DataGridViewRow dgr in dgvGoodsOrder.Rows)
                {
                    int index = this.dgvItemOrder.Rows.Add();
                    dgvItemOrder.Rows[index].Cells["GoodsNum"].Value = dgr.Cells["GoodsNum"].Value;
                    dgvItemOrder.Rows[index].Cells["GoodsName"].Value = dgr.Cells["GoodsName"].Value;
                    dgvItemOrder.Rows[index].Cells["GoodsPrice"].Value = dgr.Cells["GoodsPrice"].Value;
                    dgvItemOrder.Rows[index].Cells["GoodsDiscount"].Value = dgr.Cells["GoodsDiscount"].Value;
                    //设置样式
                    dgr.Selected = false;
                    dgvItemOrder.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                    dgvItemOrder.Rows[index].DefaultCellStyle.BackColor = Color.White;
                }
                dgvItemOrder.Rows[this.dgvItemOrder.RowCount - 1].Selected = true;
                BindOrderInfoSum();
            }
        }

        public void BindGoodsOrderInfo(SalesOrder salesOrder)
        {
            this.dgvItemOrder.Rows.Clear();
            if (salesOrder.orderDetailsList != null && salesOrder.orderDetailsList.Count > 0)
            {
                foreach (OrderDetails orderDetails in salesOrder.orderDetailsList)
                {
                    int index = this.dgvItemOrder.Rows.Add(new DataGridViewRow());
                    this.dgvItemOrder.Rows[index].Cells["GoodsNum"].Value = orderDetails.ItemQty;
                    if (orderDetails.ItemType == (int)OrderItemType.Goods)
                    {
                        this.dgvItemOrder.Rows[index].Cells["GoodsName"].Value = orderDetails.GoodsName;
                    }
                    else
                    {
                        string strLevelFlag = string.Empty;
                        int levelCount = orderDetails.ItemLevel * 2;
                        for (int i = 0; i < levelCount; i++)
                        {
                            strLevelFlag += "-";
                        }
                        this.dgvItemOrder.Rows[index].Cells["GoodsName"].Value = strLevelFlag + orderDetails.GoodsName;
                    }
                    this.dgvItemOrder.Rows[index].Cells["GoodsPrice"].Value = orderDetails.TotalSellPrice;
                    this.dgvItemOrder.Rows[index].Cells["GoodsDiscount"].Value = orderDetails.TotalDiscount;
                    //设置样式
                    this.dgvItemOrder.Rows[index].Selected = false;
                    this.dgvItemOrder.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                    this.dgvItemOrder.Rows[index].DefaultCellStyle.BackColor = Color.White;
                }
                this.dgvItemOrder.Rows[this.dgvItemOrder.RowCount - 1].Selected = true;
                BindOrderInfoSum();
            }
        }

        private void BindOrderInfoSum()
        {
            decimal totalPrice = 0, totalDiscount = 0;
            for (int i = 0; i < dgvItemOrder.Rows.Count; i++)
            {
                totalPrice += Convert.ToDecimal(dgvItemOrder.Rows[i].Cells["GoodsPrice"].Value);
                totalDiscount += Convert.ToDecimal(dgvItemOrder.Rows[i].Cells["GoodsDiscount"].Value);
            }
            this.lbTotalPrice.Text = totalPrice.ToString("f2");
            this.lbDiscount.Text = totalDiscount.ToString("f2");
            decimal wholePayMoney = totalPrice + totalDiscount;
            decimal actualPayMoney = CutOffDecimal.HandleCutOff(wholePayMoney, ConstantValuePool.SysConfig.IsCutTail, ConstantValuePool.SysConfig.CutTailType, ConstantValuePool.SysConfig.CutTailDigit);
            this.lbNeedPayMoney.Text = actualPayMoney.ToString("f2");
        }

        public void DisplayOrderInfoSum(decimal actualPayMoney, decimal needChangePay, List<OrderPayoff> orderPayoffList)
        {
            decimal paidInMoney = 0M;
            foreach (OrderPayoff item in orderPayoffList)
            {
                decimal totalPrice = item.Quantity * item.AsPay;
                paidInMoney += totalPrice;
            }
            this.lbTotalPrice1.Text = "应付：";
            this.lbDiscount1.Text = "已付：";
            this.lbNeedPayMoney1.Text = "找零：";
            this.lbTotalPrice.Text = actualPayMoney.ToString("f2");
            this.lbDiscount.Text = paidInMoney.ToString("f2");
            this.lbNeedPayMoney.Text = needChangePay.ToString("f2");
        }

        public void ClearGoodsOrderInfo()
        {
            this.dgvItemOrder.Rows.Clear();
            this.lbTotalPrice.Text = string.Empty;
            this.lbDiscount.Text = string.Empty;
            this.lbNeedPayMoney.Text = string.Empty;
        }

        public void InitGoodsOrderInfo()
        {
            this.dgvItemOrder.Rows.Clear();
            this.lbTotalPrice1.Text = "总金额：";
            this.lbDiscount1.Text = "折扣：";
            this.lbNeedPayMoney1.Text = "应付：";
            this.lbTotalPrice.Text = string.Empty;
            this.lbDiscount.Text = string.Empty;
            this.lbNeedPayMoney.Text = string.Empty;
            this.lbServiceFee1.Visible = false;
            this.lbServiceFee.Visible = false;
        }

        public void ShowOrderServiceFee(decimal needPayMoney, decimal serviceFee)
        {
            if (serviceFee > 0)
            {
                this.lbServiceFee1.Visible = true;
                this.lbServiceFee.Visible = true;
                this.lbServiceFee.Text = serviceFee.ToString("f2");
            }
            else
            {
                this.lbServiceFee1.Visible = false;
                this.lbServiceFee.Visible = false;
            }
            this.lbNeedPayMoney.Text = needPayMoney.ToString("f2");
        }

        private void Play()
        {
            try
            {
                //建立播放列表
                axWindowsMediaPlayer1.currentPlaylist = axWindowsMediaPlayer1.newPlaylist("videoMenu", "");
                DirectoryInfo dir = new DirectoryInfo(ConstantValuePool.BizSettingConfig.ScreenVideoPath);
                FileInfo[] finfo = dir.GetFiles();
                for (int index = 0; index < finfo.Length; index++)
                {
                    string strPath = ConstantValuePool.BizSettingConfig.ScreenVideoPath + "\\" + finfo[index].Name;
                    axWindowsMediaPlayer1.currentPlaylist.appendItem(axWindowsMediaPlayer1.newMedia(strPath));
                }

                this.axWindowsMediaPlayer1.Ctlcontrols.play();
                axWindowsMediaPlayer1.settings.setMode("loop", true);
            }
            catch
            {
                MessageBox.Show("视频播放出现错误！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
