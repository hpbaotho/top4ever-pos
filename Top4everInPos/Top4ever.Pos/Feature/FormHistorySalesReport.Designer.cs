namespace Top4ever.Pos.Feature
{
    partial class FormHistorySalesReport
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.monthCalendar1 = new Top4ever.MonthCalendar.MonthCalendar();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvDailyStatement = new System.Windows.Forms.DataGridView();
            this.colOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBeginTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDailyStatementNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvDeviceNo = new System.Windows.Forms.DataGridView();
            this.colIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeviceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvHandover = new System.Windows.Forms.DataGridView();
            this.colOrderIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWorkSequence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmployeeNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHandoverTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHandoverRecordID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnShowDailyStatement = new Top4ever.CustomControl.CrystalButton();
            this.btnCancel = new Top4ever.CustomControl.CrystalButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDailyStatement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeviceNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHandover)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.monthCalendar1);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(330, 281);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择账务日期";
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.ColorTable.DayActiveGradientBegin = System.Drawing.Color.Bisque;
            this.monthCalendar1.ColorTable.DayActiveGradientEnd = System.Drawing.Color.Gold;
            this.monthCalendar1.ColorTable.DayHeaderText = System.Drawing.Color.MediumBlue;
            this.monthCalendar1.ColorTable.DayTextBold = System.Drawing.Color.DarkBlue;
            this.monthCalendar1.ColorTable.FooterActiveGradientBegin = System.Drawing.Color.Transparent;
            this.monthCalendar1.ColorTable.FooterActiveGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.monthCalendar1.ColorTable.FooterActiveText = System.Drawing.Color.DarkRed;
            this.monthCalendar1.ColorTable.HeaderActiveArrow = System.Drawing.Color.MidnightBlue;
            this.monthCalendar1.ColorTable.HeaderActiveGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(149)))), ((int)(((byte)(255)))));
            this.monthCalendar1.ColorTable.HeaderGradientEnd = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(177)))), ((int)(((byte)(255)))));
            this.monthCalendar1.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.monthCalendar1.Location = new System.Drawing.Point(14, 22);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 0;
            this.monthCalendar1.UseShortestDayNames = true;
            this.monthCalendar1.DateSelected += new System.EventHandler<System.Windows.Forms.DateRangeEventArgs>(this.monthCalendar1_DateSelected);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(8, 290);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "选择营业时间段：";
            // 
            // dgvDailyStatement
            // 
            this.dgvDailyStatement.AllowUserToAddRows = false;
            this.dgvDailyStatement.AllowUserToDeleteRows = false;
            this.dgvDailyStatement.AllowUserToResizeColumns = false;
            this.dgvDailyStatement.AllowUserToResizeRows = false;
            this.dgvDailyStatement.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDailyStatement.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDailyStatement.ColumnHeadersHeight = 43;
            this.dgvDailyStatement.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDailyStatement.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOrder,
            this.colBeginTime,
            this.colEndTime,
            this.colDailyStatementNo});
            this.dgvDailyStatement.EnableHeadersVisualStyles = false;
            this.dgvDailyStatement.Location = new System.Drawing.Point(12, 314);
            this.dgvDailyStatement.MultiSelect = false;
            this.dgvDailyStatement.Name = "dgvDailyStatement";
            this.dgvDailyStatement.ReadOnly = true;
            this.dgvDailyStatement.RowHeadersVisible = false;
            this.dgvDailyStatement.RowTemplate.Height = 23;
            this.dgvDailyStatement.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDailyStatement.Size = new System.Drawing.Size(332, 331);
            this.dgvDailyStatement.TabIndex = 2;
            this.dgvDailyStatement.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDailyStatement_CellClick);
            // 
            // colOrder
            // 
            this.colOrder.HeaderText = "序";
            this.colOrder.Name = "colOrder";
            this.colOrder.ReadOnly = true;
            this.colOrder.Width = 60;
            // 
            // colBeginTime
            // 
            this.colBeginTime.HeaderText = "开始时间";
            this.colBeginTime.Name = "colBeginTime";
            this.colBeginTime.ReadOnly = true;
            this.colBeginTime.Width = 134;
            // 
            // colEndTime
            // 
            this.colEndTime.HeaderText = "结束时间";
            this.colEndTime.Name = "colEndTime";
            this.colEndTime.ReadOnly = true;
            this.colEndTime.Width = 134;
            // 
            // colDailyStatementNo
            // 
            this.colDailyStatementNo.HeaderText = "日结号";
            this.colDailyStatementNo.Name = "colDailyStatementNo";
            this.colDailyStatementNo.ReadOnly = true;
            this.colDailyStatementNo.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(348, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "选择设备号：";
            // 
            // dgvDeviceNo
            // 
            this.dgvDeviceNo.AllowUserToAddRows = false;
            this.dgvDeviceNo.AllowUserToDeleteRows = false;
            this.dgvDeviceNo.AllowUserToResizeColumns = false;
            this.dgvDeviceNo.AllowUserToResizeRows = false;
            this.dgvDeviceNo.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDeviceNo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDeviceNo.ColumnHeadersHeight = 43;
            this.dgvDeviceNo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDeviceNo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIndex,
            this.colDeviceNo});
            this.dgvDeviceNo.EnableHeadersVisualStyles = false;
            this.dgvDeviceNo.Location = new System.Drawing.Point(352, 74);
            this.dgvDeviceNo.MultiSelect = false;
            this.dgvDeviceNo.Name = "dgvDeviceNo";
            this.dgvDeviceNo.ReadOnly = true;
            this.dgvDeviceNo.RowHeadersVisible = false;
            this.dgvDeviceNo.RowTemplate.Height = 23;
            this.dgvDeviceNo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDeviceNo.Size = new System.Drawing.Size(154, 571);
            this.dgvDeviceNo.TabIndex = 2;
            this.dgvDeviceNo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDeviceNo_CellClick);
            // 
            // colIndex
            // 
            this.colIndex.HeaderText = "序";
            this.colIndex.Name = "colIndex";
            this.colIndex.ReadOnly = true;
            this.colIndex.Width = 60;
            // 
            // colDeviceNo
            // 
            this.colDeviceNo.HeaderText = "设备号";
            this.colDeviceNo.Name = "colDeviceNo";
            this.colDeviceNo.ReadOnly = true;
            this.colDeviceNo.Width = 90;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(509, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 21);
            this.label3.TabIndex = 1;
            this.label3.Text = "选择交班记录：";
            // 
            // dgvHandover
            // 
            this.dgvHandover.AllowUserToAddRows = false;
            this.dgvHandover.AllowUserToDeleteRows = false;
            this.dgvHandover.AllowUserToResizeColumns = false;
            this.dgvHandover.AllowUserToResizeRows = false;
            this.dgvHandover.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHandover.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvHandover.ColumnHeadersHeight = 43;
            this.dgvHandover.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvHandover.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOrderIndex,
            this.colWorkSequence,
            this.colEmployeeNo,
            this.colHandoverTime,
            this.colHandoverRecordID});
            this.dgvHandover.EnableHeadersVisualStyles = false;
            this.dgvHandover.Location = new System.Drawing.Point(513, 74);
            this.dgvHandover.MultiSelect = false;
            this.dgvHandover.Name = "dgvHandover";
            this.dgvHandover.ReadOnly = true;
            this.dgvHandover.RowHeadersVisible = false;
            this.dgvHandover.RowTemplate.Height = 23;
            this.dgvHandover.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHandover.Size = new System.Drawing.Size(424, 571);
            this.dgvHandover.TabIndex = 2;
            // 
            // colOrderIndex
            // 
            this.colOrderIndex.HeaderText = "序";
            this.colOrderIndex.Name = "colOrderIndex";
            this.colOrderIndex.ReadOnly = true;
            this.colOrderIndex.Width = 60;
            // 
            // colWorkSequence
            // 
            this.colWorkSequence.HeaderText = "交班序号";
            this.colWorkSequence.Name = "colWorkSequence";
            this.colWorkSequence.ReadOnly = true;
            // 
            // colEmployeeNo
            // 
            this.colEmployeeNo.HeaderText = "交班员工";
            this.colEmployeeNo.Name = "colEmployeeNo";
            this.colEmployeeNo.ReadOnly = true;
            this.colEmployeeNo.Width = 125;
            // 
            // colHandoverTime
            // 
            this.colHandoverTime.HeaderText = "交班时间";
            this.colHandoverTime.Name = "colHandoverTime";
            this.colHandoverTime.ReadOnly = true;
            this.colHandoverTime.Width = 135;
            // 
            // colHandoverRecordID
            // 
            this.colHandoverRecordID.HeaderText = "交班主键";
            this.colHandoverRecordID.Name = "colHandoverRecordID";
            this.colHandoverRecordID.ReadOnly = true;
            this.colHandoverRecordID.Visible = false;
            // 
            // btnShowDailyStatement
            // 
            this.btnShowDailyStatement.BackColor = System.Drawing.Color.Teal;
            this.btnShowDailyStatement.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnShowDailyStatement.ForeColor = System.Drawing.Color.White;
            this.btnShowDailyStatement.Location = new System.Drawing.Point(707, 8);
            this.btnShowDailyStatement.Name = "btnShowDailyStatement";
            this.btnShowDailyStatement.Size = new System.Drawing.Size(102, 58);
            this.btnShowDailyStatement.TabIndex = 1;
            this.btnShowDailyStatement.Text = "查看";
            this.btnShowDailyStatement.UseVisualStyleBackColor = false;
            this.btnShowDailyStatement.Click += new System.EventHandler(this.btnShowDailyStatement_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(43)))), ((int)(((byte)(59)))));
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(835, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(102, 58);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "关闭";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FormHistorySalesReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 657);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnShowDailyStatement);
            this.Controls.Add(this.dgvHandover);
            this.Controls.Add(this.dgvDeviceNo);
            this.Controls.Add(this.dgvDailyStatement);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormHistorySalesReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormHistorySalesReport";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDailyStatement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeviceNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHandover)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private MonthCalendar.MonthCalendar monthCalendar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvDailyStatement;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvDeviceNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvHandover;
        private CustomControl.CrystalButton btnShowDailyStatement;
        private CustomControl.CrystalButton btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBeginTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDailyStatementNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeviceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWorkSequence;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmployeeNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHandoverTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHandoverRecordID;
    }
}