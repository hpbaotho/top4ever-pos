namespace VechsoftPos
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
            this.GoodsDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlPayInfo = new System.Windows.Forms.Panel();
            this.lbNeedPayMoney = new System.Windows.Forms.Label();
            this.lbDiscount = new System.Windows.Forms.Label();
            this.lbTotalPrice = new System.Windows.Forms.Label();
            this.lbNeedPayMoney1 = new System.Windows.Forms.Label();
            this.lbDiscount1 = new System.Windows.Forms.Label();
            this.lbTotalPrice1 = new System.Windows.Forms.Label();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlVideo = new System.Windows.Forms.Panel();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.pnlImage = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lbCompany = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lbServiceFee1 = new System.Windows.Forms.Label();
            this.lbServiceFee = new System.Windows.Forms.Label();
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
            this.pnlBottom.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.scrollingText1.BackColor = System.Drawing.Color.DodgerBlue;
            this.scrollingText1.BorderColor = System.Drawing.Color.Black;
            this.scrollingText1.Cursor = System.Windows.Forms.Cursors.Default;
            this.scrollingText1.Dock = System.Windows.Forms.DockStyle.Top;
            this.scrollingText1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.scrollingText1.TextScrollDistance = 1;
            this.scrollingText1.TextScrollSpeed = 90;
            this.scrollingText1.VerticleTextPosition = Top4ever.CustomControl.VerticleTextPosition.Center;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.pnlGoodsInfo);
            this.pnlLeft.Controls.Add(this.pnlPayInfo);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 32);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(425, 600);
            this.pnlLeft.TabIndex = 1;
            // 
            // pnlGoodsInfo
            // 
            this.pnlGoodsInfo.Controls.Add(this.dgvItemOrder);
            this.pnlGoodsInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGoodsInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlGoodsInfo.Name = "pnlGoodsInfo";
            this.pnlGoodsInfo.Size = new System.Drawing.Size(425, 403);
            this.pnlGoodsInfo.TabIndex = 3;
            // 
            // dgvItemOrder
            // 
            this.dgvItemOrder.AllowUserToAddRows = false;
            this.dgvItemOrder.AllowUserToDeleteRows = false;
            this.dgvItemOrder.AllowUserToResizeColumns = false;
            this.dgvItemOrder.AllowUserToResizeRows = false;
            this.dgvItemOrder.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(179)))), ((int)(((byte)(129)))));
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
            this.GoodsPrice,
            this.GoodsDiscount});
            this.dgvItemOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItemOrder.Location = new System.Drawing.Point(0, 0);
            this.dgvItemOrder.Name = "dgvItemOrder";
            this.dgvItemOrder.ReadOnly = true;
            this.dgvItemOrder.RowHeadersVisible = false;
            this.dgvItemOrder.RowTemplate.Height = 23;
            this.dgvItemOrder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItemOrder.Size = new System.Drawing.Size(425, 403);
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
            // GoodsDiscount
            // 
            this.GoodsDiscount.HeaderText = "折扣";
            this.GoodsDiscount.Name = "GoodsDiscount";
            this.GoodsDiscount.ReadOnly = true;
            this.GoodsDiscount.Visible = false;
            // 
            // pnlPayInfo
            // 
            this.pnlPayInfo.BackColor = System.Drawing.Color.Black;
            this.pnlPayInfo.Controls.Add(this.lbNeedPayMoney);
            this.pnlPayInfo.Controls.Add(this.lbServiceFee);
            this.pnlPayInfo.Controls.Add(this.lbDiscount);
            this.pnlPayInfo.Controls.Add(this.lbTotalPrice);
            this.pnlPayInfo.Controls.Add(this.lbNeedPayMoney1);
            this.pnlPayInfo.Controls.Add(this.lbServiceFee1);
            this.pnlPayInfo.Controls.Add(this.lbDiscount1);
            this.pnlPayInfo.Controls.Add(this.lbTotalPrice1);
            this.pnlPayInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPayInfo.Location = new System.Drawing.Point(0, 403);
            this.pnlPayInfo.Name = "pnlPayInfo";
            this.pnlPayInfo.Size = new System.Drawing.Size(425, 197);
            this.pnlPayInfo.TabIndex = 2;
            // 
            // lbNeedPayMoney
            // 
            this.lbNeedPayMoney.AutoSize = true;
            this.lbNeedPayMoney.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold);
            this.lbNeedPayMoney.ForeColor = System.Drawing.Color.Red;
            this.lbNeedPayMoney.Location = new System.Drawing.Point(118, 123);
            this.lbNeedPayMoney.Name = "lbNeedPayMoney";
            this.lbNeedPayMoney.Size = new System.Drawing.Size(88, 24);
            this.lbNeedPayMoney.TabIndex = 1;
            this.lbNeedPayMoney.Text = "850.00";
            // 
            // lbDiscount
            // 
            this.lbDiscount.AutoSize = true;
            this.lbDiscount.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDiscount.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lbDiscount.Location = new System.Drawing.Point(305, 47);
            this.lbDiscount.Name = "lbDiscount";
            this.lbDiscount.Size = new System.Drawing.Size(88, 24);
            this.lbDiscount.TabIndex = 1;
            this.lbDiscount.Text = "-10.00";
            // 
            // lbTotalPrice
            // 
            this.lbTotalPrice.AutoSize = true;
            this.lbTotalPrice.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTotalPrice.ForeColor = System.Drawing.Color.Teal;
            this.lbTotalPrice.Location = new System.Drawing.Point(118, 47);
            this.lbTotalPrice.Name = "lbTotalPrice";
            this.lbTotalPrice.Size = new System.Drawing.Size(88, 24);
            this.lbTotalPrice.TabIndex = 1;
            this.lbTotalPrice.Text = "860.27";
            // 
            // lbNeedPayMoney1
            // 
            this.lbNeedPayMoney1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbNeedPayMoney1.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Bold);
            this.lbNeedPayMoney1.ForeColor = System.Drawing.Color.Chocolate;
            this.lbNeedPayMoney1.Location = new System.Drawing.Point(46, 125);
            this.lbNeedPayMoney1.Name = "lbNeedPayMoney1";
            this.lbNeedPayMoney1.Size = new System.Drawing.Size(72, 20);
            this.lbNeedPayMoney1.TabIndex = 3;
            this.lbNeedPayMoney1.Text = "应付：";
            this.lbNeedPayMoney1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbDiscount1
            // 
            this.lbDiscount1.AutoSize = true;
            this.lbDiscount1.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Bold);
            this.lbDiscount1.ForeColor = System.Drawing.Color.Chocolate;
            this.lbDiscount1.Location = new System.Drawing.Point(237, 49);
            this.lbDiscount1.Name = "lbDiscount1";
            this.lbDiscount1.Size = new System.Drawing.Size(72, 20);
            this.lbDiscount1.TabIndex = 4;
            this.lbDiscount1.Text = "折扣：";
            // 
            // lbTotalPrice1
            // 
            this.lbTotalPrice1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTotalPrice1.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTotalPrice1.ForeColor = System.Drawing.Color.Chocolate;
            this.lbTotalPrice1.Location = new System.Drawing.Point(25, 49);
            this.lbTotalPrice1.Name = "lbTotalPrice1";
            this.lbTotalPrice1.Size = new System.Drawing.Size(93, 20);
            this.lbTotalPrice1.TabIndex = 1;
            this.lbTotalPrice1.Text = "总金额：";
            this.lbTotalPrice1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.pnlVideo);
            this.pnlRight.Controls.Add(this.pnlImage);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(425, 32);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(537, 600);
            this.pnlRight.TabIndex = 1;
            // 
            // pnlVideo
            // 
            this.pnlVideo.Controls.Add(this.axWindowsMediaPlayer1);
            this.pnlVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlVideo.Location = new System.Drawing.Point(0, 0);
            this.pnlVideo.Name = "pnlVideo";
            this.pnlVideo.Size = new System.Drawing.Size(537, 403);
            this.pnlVideo.TabIndex = 4;
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(0, 0);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(537, 403);
            this.axWindowsMediaPlayer1.TabIndex = 0;
            // 
            // pnlImage
            // 
            this.pnlImage.Controls.Add(this.pictureBox1);
            this.pnlImage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlImage.Location = new System.Drawing.Point(0, 403);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(537, 197);
            this.pnlImage.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(537, 197);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lbCompany);
            this.pnlBottom.Controls.Add(this.panel1);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 632);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(962, 22);
            this.pnlBottom.TabIndex = 2;
            // 
            // lbCompany
            // 
            this.lbCompany.AutoSize = true;
            this.lbCompany.Location = new System.Drawing.Point(342, 1);
            this.lbCompany.Name = "lbCompany";
            this.lbCompany.Size = new System.Drawing.Size(191, 19);
            this.lbCompany.TabIndex = 1;
            this.lbCompany.Text = "讯创科技有限公司提供技术支持";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(762, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 22);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务电话：400-820-6016";
            // 
            // lbServiceFee1
            // 
            this.lbServiceFee1.AutoSize = true;
            this.lbServiceFee1.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Bold);
            this.lbServiceFee1.ForeColor = System.Drawing.Color.Chocolate;
            this.lbServiceFee1.Location = new System.Drawing.Point(237, 125);
            this.lbServiceFee1.Name = "lbServiceFee1";
            this.lbServiceFee1.Size = new System.Drawing.Size(93, 20);
            this.lbServiceFee1.TabIndex = 4;
            this.lbServiceFee1.Text = "服务费：";
            // 
            // lbServiceFee
            // 
            this.lbServiceFee.AutoSize = true;
            this.lbServiceFee.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbServiceFee.ForeColor = System.Drawing.Color.BlanchedAlmond;
            this.lbServiceFee.Location = new System.Drawing.Point(327, 121);
            this.lbServiceFee.Name = "lbServiceFee";
            this.lbServiceFee.Size = new System.Drawing.Size(75, 24);
            this.lbServiceFee.TabIndex = 1;
            this.lbServiceFee.Text = "15.00";
            // 
            // FormSecondScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 654);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlBanner);
            this.Controls.Add(this.pnlBottom);
            this.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormSecondScreen";
            this.Text = "FormSecondScreen";
            this.Load += new System.EventHandler(this.FormSecondScreen_Load);
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
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBanner;
        private Top4ever.CustomControl.ScrollingText scrollingText1;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlGoodsInfo;
        private System.Windows.Forms.Panel pnlPayInfo;
        private System.Windows.Forms.Panel pnlVideo;
        private System.Windows.Forms.Panel pnlImage;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbNeedPayMoney1;
        private System.Windows.Forms.Label lbDiscount1;
        private System.Windows.Forms.Label lbTotalPrice1;
        private System.Windows.Forms.DataGridView dgvItemOrder;
        private System.Windows.Forms.Label lbNeedPayMoney;
        private System.Windows.Forms.Label lbDiscount;
        private System.Windows.Forms.Label lbTotalPrice;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lbCompany;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsDiscount;
        private System.Windows.Forms.Label lbServiceFee;
        private System.Windows.Forms.Label lbServiceFee1;
    }
}