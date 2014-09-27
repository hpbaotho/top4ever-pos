namespace VechsoftPos.Membership
{
    partial class FormVIPCardRefundPay
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGirdViewExt1 = new Top4ever.CustomControl.DataGirdViewExt();
            this.colCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colCardNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTradePayNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPayAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeviceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsFixed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmployeeNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStoreValueID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRefundPay = new Top4ever.CustomControl.CrystalButton();
            this.btnCancel = new Top4ever.CustomControl.CrystalButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGirdViewExt1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGirdViewExt1);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(764, 408);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "支付退款的会员信息列表";
            // 
            // dataGirdViewExt1
            // 
            this.dataGirdViewExt1.AllowUserToAddRows = false;
            this.dataGirdViewExt1.AllowUserToDeleteRows = false;
            this.dataGirdViewExt1.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(248)))));
            this.dataGirdViewExt1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGirdViewExt1.BackgroundColor = System.Drawing.Color.White;
            this.dataGirdViewExt1.ColumnHeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(210)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGirdViewExt1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dataGirdViewExt1.ColumnHeadersHeight = 32;
            this.dataGirdViewExt1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCheck,
            this.colCardNo,
            this.colTradePayNo,
            this.colPayAmount,
            this.colDeviceNo,
            this.colIsFixed,
            this.colEmployeeNo,
            this.colCreateTime,
            this.colStoreValueID});
            this.dataGirdViewExt1.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.dataGirdViewExt1.Location = new System.Drawing.Point(7, 24);
            this.dataGirdViewExt1.MultiSelect = false;
            this.dataGirdViewExt1.Name = "dataGirdViewExt1";
            this.dataGirdViewExt1.RowHeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(220)))), ((int)(((byte)(248)))));
            this.dataGirdViewExt1.RowHeadersVisible = false;
            this.dataGirdViewExt1.RowTemplate.Height = 23;
            this.dataGirdViewExt1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGirdViewExt1.Size = new System.Drawing.Size(750, 376);
            this.dataGirdViewExt1.TabIndex = 2;
            // 
            // colCheck
            // 
            this.colCheck.HeaderText = "选择";
            this.colCheck.Name = "colCheck";
            this.colCheck.Width = 46;
            // 
            // colCardNo
            // 
            this.colCardNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCardNo.HeaderText = "卡号";
            this.colCardNo.Name = "colCardNo";
            this.colCardNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colTradePayNo
            // 
            this.colTradePayNo.HeaderText = "交易流水号";
            this.colTradePayNo.Name = "colTradePayNo";
            this.colTradePayNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colTradePayNo.Width = 135;
            // 
            // colPayAmount
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.colPayAmount.DefaultCellStyle = dataGridViewCellStyle9;
            this.colPayAmount.HeaderText = "支付金额";
            this.colPayAmount.Name = "colPayAmount";
            this.colPayAmount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colPayAmount.Width = 80;
            // 
            // colDeviceNo
            // 
            this.colDeviceNo.HeaderText = "设备号";
            this.colDeviceNo.Name = "colDeviceNo";
            this.colDeviceNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDeviceNo.Width = 80;
            // 
            // colIsFixed
            // 
            this.colIsFixed.HeaderText = "已处理";
            this.colIsFixed.Name = "colIsFixed";
            this.colIsFixed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colIsFixed.Width = 60;
            // 
            // colEmployeeNo
            // 
            this.colEmployeeNo.HeaderText = "操作员工";
            this.colEmployeeNo.Name = "colEmployeeNo";
            this.colEmployeeNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colEmployeeNo.Width = 80;
            // 
            // colCreateTime
            // 
            this.colCreateTime.HeaderText = "操作时间";
            this.colCreateTime.Name = "colCreateTime";
            this.colCreateTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCreateTime.Width = 135;
            // 
            // colStoreValueID
            // 
            this.colStoreValueID.HeaderText = "StoreValueID";
            this.colStoreValueID.Name = "colStoreValueID";
            this.colStoreValueID.Visible = false;
            // 
            // btnRefundPay
            // 
            this.btnRefundPay.BackColor = System.Drawing.Color.Teal;
            this.btnRefundPay.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRefundPay.ForeColor = System.Drawing.Color.White;
            this.btnRefundPay.Location = new System.Drawing.Point(677, 423);
            this.btnRefundPay.Name = "btnRefundPay";
            this.btnRefundPay.Size = new System.Drawing.Size(92, 56);
            this.btnRefundPay.TabIndex = 2;
            this.btnRefundPay.Text = "退款";
            this.btnRefundPay.UseVisualStyleBackColor = false;
            this.btnRefundPay.Click += new System.EventHandler(this.btnRefundPay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(43)))), ((int)(((byte)(59)))));
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(545, 423);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 56);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FormVIPCardRefundPay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 496);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRefundPay);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormVIPCardRefundPay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "支付退款管理";
            this.Load += new System.EventHandler(this.FormVIPCardRefundPay_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGirdViewExt1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Top4ever.CustomControl.DataGirdViewExt dataGirdViewExt1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCardNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTradePayNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPayAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeviceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIsFixed;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmployeeNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStoreValueID;
        private Top4ever.CustomControl.CrystalButton btnRefundPay;
        private Top4ever.CustomControl.CrystalButton btnCancel;
    }
}