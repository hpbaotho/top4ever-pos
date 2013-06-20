namespace Top4ever.Pos
{
    partial class FormSecondScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSecondScreen));
            this.pnlBanner = new System.Windows.Forms.Panel();
            this.scrollingText1 = new Top4ever.CustomControl.ScrollingText();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlGoodsInfo = new System.Windows.Forms.Panel();
            this.dgvItemOrder = new System.Windows.Forms.DataGridView();
            this.GoodsNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GoodsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GoodsPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlPayInfo = new System.Windows.Forms.Panel();
            this.lbNeedPayMoney = new System.Windows.Forms.Label();
            this.lbDiscount = new System.Windows.Forms.Label();
            this.lbCutOff = new System.Windows.Forms.Label();
            this.lbTotalPrice = new System.Windows.Forms.Label();
            this.lbNeedPayMoney1 = new System.Windows.Forms.Label();
            this.lbDiscount1 = new System.Windows.Forms.Label();
            this.lbCutOff1 = new System.Windows.Forms.Label();
            this.lbTotalPrice1 = new System.Windows.Forms.Label();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlVideo = new System.Windows.Forms.Panel();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.pnlImage = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlBanner.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlGoodsInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemOrder)).BeginInit();
            this.pnlPayInfo.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlVideo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            this.pnlImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBanner
            // 
            this.pnlBanner.Controls.Add(this.scrollingText1);
            this.pnlBanner.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBanner.Location = new System.Drawing.Point(0, 0);
            this.pnlBanner.Name = "pnlBanner";
            this.pnlBanner.Size = new System.Drawing.Size(962, 32);
            this.pnlBanner.TabIndex = 0;
            // 
            // scrollingText1
            // 
            this.scrollingText1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.scrollingText1.BorderColor = System.Drawing.Color.Black;
            this.scrollingText1.Cursor = System.Windows.Forms.Cursors.Default;
            this.scrollingText1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scrollingText1.ForeColor = System.Drawing.Color.White;
            this.scrollingText1.ForegroundBrush = null;
            this.scrollingText1.Location = new System.Drawing.Point(0, 0);
            this.scrollingText1.Name = "scrollingText1";
            this.scrollingText1.ScrollDirection = Top4ever.CustomControl.ScrollDirection.RightToLeft;
            this.scrollingText1.ScrollText = "新版本测试使用，欢迎光临本店，请注意营业时间";
            this.scrollingText1.ShowBorder = false;
            this.scrollingText1.Size = new System.Drawing.Size(962, 32);
            this.scrollingText1.StopScrollOnMouseOver = true;
            this.scrollingText1.TabIndex = 1;
            this.scrollingText1.Text = "scrollingText1";
            this.scrollingText1.TextScrollDistance = 2;
            this.scrollingText1.TextScrollSpeed = 50;
            this.scrollingText1.VerticleTextPosition = Top4ever.CustomControl.VerticleTextPosition.Center;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.pnlGoodsInfo);
            this.pnlLeft.Controls.Add(this.pnlPayInfo);
            this.pnlLeft.Location = new System.Drawing.Point(0, 32);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(425, 596);
            this.pnlLeft.TabIndex = 1;
            // 
            // pnlGoodsInfo
            // 
            this.pnlGoodsInfo.Controls.Add(this.dgvItemOrder);
            this.pnlGoodsInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGoodsInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlGoodsInfo.Name = "pnlGoodsInfo";
            this.pnlGoodsInfo.Size = new System.Drawing.Size(425, 399);
            this.pnlGoodsInfo.TabIndex = 3;
            // 
            // dgvItemOrder
            // 
            this.dgvItemOrder.AllowUserToAddRows = false;
            this.dgvItemOrder.AllowUserToDeleteRows = false;
            this.dgvItemOrder.AllowUserToResizeColumns = false;
            this.dgvItemOrder.AllowUserToResizeRows = false;
            this.dgvItemOrder.BackgroundColor = System.Drawing.Color.LightYellow;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItemOrder.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvItemOrder.ColumnHeadersHeight = 35;
            this.dgvItemOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvItemOrder.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GoodsNum,
            this.GoodsName,
            this.GoodsPrice});
            this.dgvItemOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItemOrder.Location = new System.Drawing.Point(0, 0);
            this.dgvItemOrder.Name = "dgvItemOrder";
            this.dgvItemOrder.ReadOnly = true;
            this.dgvItemOrder.RowHeadersVisible = false;
            this.dgvItemOrder.RowTemplate.Height = 23;
            this.dgvItemOrder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItemOrder.Size = new System.Drawing.Size(425, 399);
            this.dgvItemOrder.TabIndex = 5;
            // 
            // GoodsNum
            // 
            dataGridViewCellStyle2.Format = "N1";
            this.GoodsNum.DefaultCellStyle = dataGridViewCellStyle2;
            this.GoodsNum.HeaderText = "数量";
            this.GoodsNum.Name = "GoodsNum";
            this.GoodsNum.ReadOnly = true;
            this.GoodsNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.GoodsNum.Width = 76;
            // 
            // GoodsName
            // 
            this.GoodsName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.GoodsName.HeaderText = "品名";
            this.GoodsName.Name = "GoodsName";
            this.GoodsName.ReadOnly = true;
            this.GoodsName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // GoodsPrice
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.GoodsPrice.DefaultCellStyle = dataGridViewCellStyle3;
            this.GoodsPrice.HeaderText = "价格";
            this.GoodsPrice.Name = "GoodsPrice";
            this.GoodsPrice.ReadOnly = true;
            this.GoodsPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // pnlPayInfo
            // 
            this.pnlPayInfo.BackColor = System.Drawing.Color.LightYellow;
            this.pnlPayInfo.Controls.Add(this.lbNeedPayMoney);
            this.pnlPayInfo.Controls.Add(this.lbDiscount);
            this.pnlPayInfo.Controls.Add(this.lbCutOff);
            this.pnlPayInfo.Controls.Add(this.lbTotalPrice);
            this.pnlPayInfo.Controls.Add(this.lbNeedPayMoney1);
            this.pnlPayInfo.Controls.Add(this.lbDiscount1);
            this.pnlPayInfo.Controls.Add(this.lbCutOff1);
            this.pnlPayInfo.Controls.Add(this.lbTotalPrice1);
            this.pnlPayInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPayInfo.Location = new System.Drawing.Point(0, 399);
            this.pnlPayInfo.Name = "pnlPayInfo";
            this.pnlPayInfo.Size = new System.Drawing.Size(425, 197);
            this.pnlPayInfo.TabIndex = 2;
            // 
            // lbNeedPayMoney
            // 
            this.lbNeedPayMoney.AutoSize = true;
            this.lbNeedPayMoney.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbNeedPayMoney.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lbNeedPayMoney.Location = new System.Drawing.Point(114, 101);
            this.lbNeedPayMoney.Name = "lbNeedPayMoney";
            this.lbNeedPayMoney.Size = new System.Drawing.Size(109, 38);
            this.lbNeedPayMoney.TabIndex = 1;
            this.lbNeedPayMoney.Text = "850.00";
            // 
            // lbDiscount
            // 
            this.lbDiscount.AutoSize = true;
            this.lbDiscount.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDiscount.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lbDiscount.Location = new System.Drawing.Point(308, 101);
            this.lbDiscount.Name = "lbDiscount";
            this.lbDiscount.Size = new System.Drawing.Size(105, 38);
            this.lbDiscount.TabIndex = 1;
            this.lbDiscount.Text = "-10.00";
            // 
            // lbCutOff
            // 
            this.lbCutOff.AutoSize = true;
            this.lbCutOff.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbCutOff.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lbCutOff.Location = new System.Drawing.Point(308, 29);
            this.lbCutOff.Name = "lbCutOff";
            this.lbCutOff.Size = new System.Drawing.Size(88, 38);
            this.lbCutOff.TabIndex = 1;
            this.lbCutOff.Text = "-0.27";
            // 
            // lbTotalPrice
            // 
            this.lbTotalPrice.AutoSize = true;
            this.lbTotalPrice.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTotalPrice.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lbTotalPrice.Location = new System.Drawing.Point(114, 29);
            this.lbTotalPrice.Name = "lbTotalPrice";
            this.lbTotalPrice.Size = new System.Drawing.Size(109, 38);
            this.lbTotalPrice.TabIndex = 1;
            this.lbTotalPrice.Text = "860.27";
            // 
            // lbNeedPayMoney1
            // 
            this.lbNeedPayMoney1.AutoSize = true;
            this.lbNeedPayMoney1.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbNeedPayMoney1.ForeColor = System.Drawing.Color.Chocolate;
            this.lbNeedPayMoney1.Location = new System.Drawing.Point(5, 106);
            this.lbNeedPayMoney1.Name = "lbNeedPayMoney1";
            this.lbNeedPayMoney1.Size = new System.Drawing.Size(117, 28);
            this.lbNeedPayMoney1.TabIndex = 3;
            this.lbNeedPayMoney1.Text = "实际应付：";
            // 
            // lbDiscount1
            // 
            this.lbDiscount1.AutoSize = true;
            this.lbDiscount1.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDiscount1.ForeColor = System.Drawing.Color.Chocolate;
            this.lbDiscount1.Location = new System.Drawing.Point(236, 106);
            this.lbDiscount1.Name = "lbDiscount1";
            this.lbDiscount1.Size = new System.Drawing.Size(75, 28);
            this.lbDiscount1.TabIndex = 4;
            this.lbDiscount1.Text = "折扣：";
            // 
            // lbCutOff1
            // 
            this.lbCutOff1.AutoSize = true;
            this.lbCutOff1.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbCutOff1.ForeColor = System.Drawing.Color.Chocolate;
            this.lbCutOff1.Location = new System.Drawing.Point(236, 34);
            this.lbCutOff1.Name = "lbCutOff1";
            this.lbCutOff1.Size = new System.Drawing.Size(75, 28);
            this.lbCutOff1.TabIndex = 1;
            this.lbCutOff1.Text = "去零：";
            // 
            // lbTotalPrice1
            // 
            this.lbTotalPrice1.AutoSize = true;
            this.lbTotalPrice1.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTotalPrice1.ForeColor = System.Drawing.Color.Chocolate;
            this.lbTotalPrice1.Location = new System.Drawing.Point(26, 34);
            this.lbTotalPrice1.Name = "lbTotalPrice1";
            this.lbTotalPrice1.Size = new System.Drawing.Size(96, 28);
            this.lbTotalPrice1.TabIndex = 1;
            this.lbTotalPrice1.Text = "总金额：";
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.pnlVideo);
            this.pnlRight.Controls.Add(this.pnlImage);
            this.pnlRight.Location = new System.Drawing.Point(428, 32);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(534, 596);
            this.pnlRight.TabIndex = 1;
            // 
            // pnlVideo
            // 
            this.pnlVideo.Controls.Add(this.axWindowsMediaPlayer1);
            this.pnlVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlVideo.Location = new System.Drawing.Point(0, 0);
            this.pnlVideo.Name = "pnlVideo";
            this.pnlVideo.Size = new System.Drawing.Size(534, 399);
            this.pnlVideo.TabIndex = 4;
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(0, 0);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(534, 399);
            this.axWindowsMediaPlayer1.TabIndex = 0;
            // 
            // pnlImage
            // 
            this.pnlImage.Controls.Add(this.pictureBox1);
            this.pnlImage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlImage.Location = new System.Drawing.Point(0, 399);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(534, 197);
            this.pnlImage.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(534, 197);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 632);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(962, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(799, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "讯创科技有限公司提供技术支持";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(148, 17);
            this.toolStripStatusLabel2.Text = "服务电话：4007-666-888";
            // 
            // FormSecondScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 654);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlBanner);
            this.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormSecondScreen";
            this.Text = "FormSecondScreen";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlBanner.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.pnlGoodsInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemOrder)).EndInit();
            this.pnlPayInfo.ResumeLayout(false);
            this.pnlPayInfo.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            this.pnlVideo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            this.pnlImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBanner;
        private CustomControl.ScrollingText scrollingText1;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Panel pnlGoodsInfo;
        private System.Windows.Forms.Panel pnlPayInfo;
        private System.Windows.Forms.Panel pnlVideo;
        private System.Windows.Forms.Panel pnlImage;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbNeedPayMoney1;
        private System.Windows.Forms.Label lbDiscount1;
        private System.Windows.Forms.Label lbCutOff1;
        private System.Windows.Forms.Label lbTotalPrice1;
        private System.Windows.Forms.DataGridView dgvItemOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsPrice;
        private System.Windows.Forms.Label lbNeedPayMoney;
        private System.Windows.Forms.Label lbDiscount;
        private System.Windows.Forms.Label lbCutOff;
        private System.Windows.Forms.Label lbTotalPrice;
    }
}