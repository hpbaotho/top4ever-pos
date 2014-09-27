namespace VechsoftPos.TakeawayCall
{
    partial class FormHistoryCallOrder
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lbTitle = new System.Windows.Forms.Label();
            this.dataGirdViewExt1 = new Top4ever.CustomControl.DataGirdViewExt();
            this.btnCancel = new Top4ever.CustomControl.CrystalButton();
            this.colGoodsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGoodsPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTimes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGoodsID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCanDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGirdViewExt1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTitle.Location = new System.Drawing.Point(43, 18);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(336, 21);
            this.lbTitle.TabIndex = 1;
            this.lbTitle.Text = "电话为 021-12345678 的历史排行前10位订餐";
            // 
            // dataGirdViewExt1
            // 
            this.dataGirdViewExt1.AllowUserToAddRows = false;
            this.dataGirdViewExt1.AllowUserToDeleteRows = false;
            this.dataGirdViewExt1.AllowUserToResizeColumns = false;
            this.dataGirdViewExt1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(248)))));
            this.dataGirdViewExt1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGirdViewExt1.BackgroundColor = System.Drawing.Color.White;
            this.dataGirdViewExt1.ColumnHeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(210)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGirdViewExt1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGirdViewExt1.ColumnHeadersHeight = 32;
            this.dataGirdViewExt1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGoodsName,
            this.colGoodsPrice,
            this.colTimes,
            this.colGoodsID,
            this.colCanDiscount});
            this.dataGirdViewExt1.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.dataGirdViewExt1.Location = new System.Drawing.Point(21, 51);
            this.dataGirdViewExt1.MultiSelect = false;
            this.dataGirdViewExt1.Name = "dataGirdViewExt1";
            this.dataGirdViewExt1.ReadOnly = true;
            this.dataGirdViewExt1.RowHeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(220)))), ((int)(((byte)(248)))));
            this.dataGirdViewExt1.RowHeadersVisible = false;
            this.dataGirdViewExt1.RowTemplate.Height = 23;
            this.dataGirdViewExt1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGirdViewExt1.Size = new System.Drawing.Size(385, 325);
            this.dataGirdViewExt1.TabIndex = 1;
            this.dataGirdViewExt1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGirdViewExt1_CellDoubleClick);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(43)))), ((int)(((byte)(59)))));
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(295, 391);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(111, 56);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "关闭";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // colGoodsName
            // 
            this.colGoodsName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.colGoodsName.DefaultCellStyle = dataGridViewCellStyle3;
            this.colGoodsName.HeaderText = "品名";
            this.colGoodsName.Name = "colGoodsName";
            this.colGoodsName.ReadOnly = true;
            this.colGoodsName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colGoodsPrice
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.Format = "N2";
            this.colGoodsPrice.DefaultCellStyle = dataGridViewCellStyle4;
            this.colGoodsPrice.HeaderText = "价格";
            this.colGoodsPrice.Name = "colGoodsPrice";
            this.colGoodsPrice.ReadOnly = true;
            this.colGoodsPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colGoodsPrice.Width = 80;
            // 
            // colTimes
            // 
            this.colTimes.HeaderText = "点菜次数";
            this.colTimes.Name = "colTimes";
            this.colTimes.ReadOnly = true;
            this.colTimes.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colTimes.Width = 80;
            // 
            // colGoodsID
            // 
            this.colGoodsID.HeaderText = "GoodsID";
            this.colGoodsID.Name = "colGoodsID";
            this.colGoodsID.ReadOnly = true;
            this.colGoodsID.Visible = false;
            // 
            // colCanDiscount
            // 
            this.colCanDiscount.HeaderText = "CanDiscount";
            this.colCanDiscount.Name = "colCanDiscount";
            this.colCanDiscount.ReadOnly = true;
            this.colCanDiscount.Visible = false;
            // 
            // FormHistoryCallOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 465);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.dataGirdViewExt1);
            this.Controls.Add(this.lbTitle);
            this.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormHistoryCallOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "历史通话订单统计";
            this.Load += new System.EventHandler(this.FormHistoryCallOrder_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGirdViewExt1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbTitle;
        private Top4ever.CustomControl.DataGirdViewExt dataGirdViewExt1;
        private Top4ever.CustomControl.CrystalButton btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGoodsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGoodsPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTimes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGoodsID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCanDiscount;

    }
}