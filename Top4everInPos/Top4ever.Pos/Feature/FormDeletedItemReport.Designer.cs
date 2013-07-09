namespace Top4ever.Pos.Feature
{
    partial class FormDeletedItemReport
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSearch = new Top4ever.CustomControl.CrystalButton();
            this.cmbDateType = new System.Windows.Forms.ComboBox();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtTotalAmount = new System.Windows.Forms.TextBox();
            this.txtTotalQty = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGirdViewExt1 = new Top4ever.CustomControl.DataGirdViewExt();
            this.colTranSequence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrderNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGoodsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCancelReasonName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCancelEmployeeNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastModifiedTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGirdViewExt2 = new Top4ever.CustomControl.DataGirdViewExt();
            this.colTranSequence1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeskName1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrderNo1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGoodsName1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemQty1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCancelReasonName1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCancelEmployeeNo1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastModifiedTime1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtTotalMoney = new System.Windows.Forms.TextBox();
            this.txtTotalNum = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnPrint = new Top4ever.CustomControl.CrystalButton();
            this.btnCancel = new Top4ever.CustomControl.CrystalButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGirdViewExt1)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGirdViewExt2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.cmbDateType);
            this.groupBox1.Controls.Add(this.dateTimePicker2);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(795, 68);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "搜索条件";
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Teal;
            this.btnSearch.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(693, 16);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(94, 45);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cmbDateType
            // 
            this.cmbDateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDateType.FormattingEnabled = true;
            this.cmbDateType.Items.AddRange(new object[] {
            "营业日",
            "账务日"});
            this.cmbDateType.Location = new System.Drawing.Point(562, 24);
            this.cmbDateType.Name = "cmbDateType";
            this.cmbDateType.Size = new System.Drawing.Size(115, 27);
            this.cmbDateType.TabIndex = 1;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(336, 24);
            this.dateTimePicker2.MinDate = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(126, 29);
            this.dateTimePicker2.TabIndex = 1;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(103, 24);
            this.dateTimePicker1.MinDate = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(126, 29);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(475, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 21);
            this.label3.TabIndex = 1;
            this.label3.Text = "日期类型：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(248, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "结束日期：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(16, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "开始日期：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtTotalAmount);
            this.groupBox2.Controls.Add(this.txtTotalQty);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.dataGirdViewExt1);
            this.groupBox2.Location = new System.Drawing.Point(12, 77);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(795, 270);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "单品删除报表";
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.txtTotalAmount.Location = new System.Drawing.Point(691, 234);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.Size = new System.Drawing.Size(94, 25);
            this.txtTotalAmount.TabIndex = 2;
            // 
            // txtTotalQty
            // 
            this.txtTotalQty.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.txtTotalQty.Location = new System.Drawing.Point(546, 234);
            this.txtTotalQty.Name = "txtTotalQty";
            this.txtTotalQty.Size = new System.Drawing.Size(46, 25);
            this.txtTotalQty.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(617, 237);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 19);
            this.label6.TabIndex = 2;
            this.label6.Text = "合计金额：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(471, 237);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 19);
            this.label5.TabIndex = 3;
            this.label5.Text = "合计笔数：";
            // 
            // dataGirdViewExt1
            // 
            this.dataGirdViewExt1.AllowUserToAddRows = false;
            this.dataGirdViewExt1.AllowUserToDeleteRows = false;
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
            this.colTranSequence,
            this.colDeskName,
            this.colOrderNo,
            this.colGoodsName,
            this.colItemQty,
            this.colCancelReasonName,
            this.colCancelEmployeeNo,
            this.colLastModifiedTime,
            this.colType});
            this.dataGirdViewExt1.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.dataGirdViewExt1.Location = new System.Drawing.Point(17, 25);
            this.dataGirdViewExt1.MultiSelect = false;
            this.dataGirdViewExt1.Name = "dataGirdViewExt1";
            this.dataGirdViewExt1.RowHeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(220)))), ((int)(((byte)(248)))));
            this.dataGirdViewExt1.RowHeadersVisible = false;
            this.dataGirdViewExt1.RowTemplate.Height = 23;
            this.dataGirdViewExt1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGirdViewExt1.Size = new System.Drawing.Size(768, 200);
            this.dataGirdViewExt1.TabIndex = 0;
            // 
            // colTranSequence
            // 
            this.colTranSequence.HeaderText = "流水号";
            this.colTranSequence.Name = "colTranSequence";
            this.colTranSequence.Width = 80;
            // 
            // colDeskName
            // 
            this.colDeskName.HeaderText = "桌号";
            this.colDeskName.Name = "colDeskName";
            this.colDeskName.Width = 68;
            // 
            // colOrderNo
            // 
            this.colOrderNo.HeaderText = "单号";
            this.colOrderNo.Name = "colOrderNo";
            this.colOrderNo.Width = 120;
            // 
            // colGoodsName
            // 
            this.colGoodsName.HeaderText = "品名";
            this.colGoodsName.Name = "colGoodsName";
            this.colGoodsName.Width = 160;
            // 
            // colItemQty
            // 
            this.colItemQty.HeaderText = "数量";
            this.colItemQty.Name = "colItemQty";
            this.colItemQty.Width = 68;
            // 
            // colCancelReasonName
            // 
            this.colCancelReasonName.HeaderText = "删除理由";
            this.colCancelReasonName.Name = "colCancelReasonName";
            this.colCancelReasonName.Width = 120;
            // 
            // colCancelEmployeeNo
            // 
            this.colCancelEmployeeNo.HeaderText = "操作员工";
            this.colCancelEmployeeNo.Name = "colCancelEmployeeNo";
            // 
            // colLastModifiedTime
            // 
            this.colLastModifiedTime.HeaderText = "操作时间";
            this.colLastModifiedTime.Name = "colLastModifiedTime";
            // 
            // colType
            // 
            this.colType.HeaderText = "状态";
            this.colType.Name = "colType";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGirdViewExt2);
            this.groupBox3.Controls.Add(this.txtTotalMoney);
            this.groupBox3.Controls.Add(this.txtTotalNum);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new System.Drawing.Point(12, 353);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(795, 270);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "整单删除报表";
            // 
            // dataGirdViewExt2
            // 
            this.dataGirdViewExt2.AllowUserToAddRows = false;
            this.dataGirdViewExt2.AllowUserToDeleteRows = false;
            this.dataGirdViewExt2.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(248)))));
            this.dataGirdViewExt2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGirdViewExt2.BackgroundColor = System.Drawing.Color.White;
            this.dataGirdViewExt2.ColumnHeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(210)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGirdViewExt2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGirdViewExt2.ColumnHeadersHeight = 32;
            this.dataGirdViewExt2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTranSequence1,
            this.colDeskName1,
            this.colOrderNo1,
            this.colGoodsName1,
            this.colItemQty1,
            this.colCancelReasonName1,
            this.colCancelEmployeeNo1,
            this.colLastModifiedTime1,
            this.colType1});
            this.dataGirdViewExt2.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.dataGirdViewExt2.Location = new System.Drawing.Point(17, 27);
            this.dataGirdViewExt2.MultiSelect = false;
            this.dataGirdViewExt2.Name = "dataGirdViewExt2";
            this.dataGirdViewExt2.RowHeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(220)))), ((int)(((byte)(248)))));
            this.dataGirdViewExt2.RowHeadersVisible = false;
            this.dataGirdViewExt2.RowTemplate.Height = 23;
            this.dataGirdViewExt2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGirdViewExt2.Size = new System.Drawing.Size(768, 200);
            this.dataGirdViewExt2.TabIndex = 9;
            // 
            // colTranSequence1
            // 
            this.colTranSequence1.HeaderText = "流水号";
            this.colTranSequence1.Name = "colTranSequence1";
            this.colTranSequence1.Width = 80;
            // 
            // colDeskName1
            // 
            this.colDeskName1.HeaderText = "桌号";
            this.colDeskName1.Name = "colDeskName1";
            this.colDeskName1.Width = 68;
            // 
            // colOrderNo1
            // 
            this.colOrderNo1.HeaderText = "单号";
            this.colOrderNo1.Name = "colOrderNo1";
            this.colOrderNo1.Width = 120;
            // 
            // colGoodsName1
            // 
            this.colGoodsName1.HeaderText = "品名";
            this.colGoodsName1.Name = "colGoodsName1";
            this.colGoodsName1.Width = 160;
            // 
            // colItemQty1
            // 
            this.colItemQty1.HeaderText = "数量";
            this.colItemQty1.Name = "colItemQty1";
            this.colItemQty1.Width = 68;
            // 
            // colCancelReasonName1
            // 
            this.colCancelReasonName1.HeaderText = "删除理由";
            this.colCancelReasonName1.Name = "colCancelReasonName1";
            this.colCancelReasonName1.Width = 120;
            // 
            // colCancelEmployeeNo1
            // 
            this.colCancelEmployeeNo1.HeaderText = "操作员工";
            this.colCancelEmployeeNo1.Name = "colCancelEmployeeNo1";
            // 
            // colLastModifiedTime1
            // 
            this.colLastModifiedTime1.HeaderText = "操作时间";
            this.colLastModifiedTime1.Name = "colLastModifiedTime1";
            // 
            // colType1
            // 
            this.colType1.HeaderText = "状态";
            this.colType1.Name = "colType1";
            // 
            // txtTotalMoney
            // 
            this.txtTotalMoney.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.txtTotalMoney.Location = new System.Drawing.Point(691, 234);
            this.txtTotalMoney.Name = "txtTotalMoney";
            this.txtTotalMoney.Size = new System.Drawing.Size(94, 25);
            this.txtTotalMoney.TabIndex = 5;
            // 
            // txtTotalNum
            // 
            this.txtTotalNum.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.txtTotalNum.Location = new System.Drawing.Point(546, 234);
            this.txtTotalNum.Name = "txtTotalNum";
            this.txtTotalNum.Size = new System.Drawing.Size(46, 25);
            this.txtTotalNum.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(617, 237);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 19);
            this.label4.TabIndex = 6;
            this.label4.Text = "合计金额：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(471, 237);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 19);
            this.label7.TabIndex = 8;
            this.label7.Text = "合计笔数：";
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.Black;
            this.btnPrint.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(539, 636);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(111, 56);
            this.btnPrint.TabIndex = 4;
            this.btnPrint.Text = "打印";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(43)))), ((int)(((byte)(59)))));
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(686, 636);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(111, 56);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FormDeletedItemReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 707);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDeletedItemReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormDeletedItemReport";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGirdViewExt1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGirdViewExt2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cmbDateType;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private CustomControl.CrystalButton btnSearch;
        private System.Windows.Forms.TextBox txtTotalAmount;
        private System.Windows.Forms.TextBox txtTotalQty;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private CustomControl.DataGirdViewExt dataGirdViewExt1;
        private System.Windows.Forms.TextBox txtTotalMoney;
        private System.Windows.Forms.TextBox txtTotalNum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private CustomControl.DataGirdViewExt dataGirdViewExt2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTranSequence;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeskName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGoodsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCancelReasonName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCancelEmployeeNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastModifiedTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTranSequence1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeskName1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderNo1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGoodsName1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemQty1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCancelReasonName1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCancelEmployeeNo1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastModifiedTime1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType1;
        private CustomControl.CrystalButton btnPrint;
        private CustomControl.CrystalButton btnCancel;
    }
}