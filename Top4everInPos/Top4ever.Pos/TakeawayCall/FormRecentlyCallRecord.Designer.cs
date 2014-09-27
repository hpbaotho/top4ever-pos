namespace VechsoftPos.TakeawayCall
{
    partial class FormRecentlyCallRecord
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecentlyCallRecord));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnClose = new Top4ever.CustomControl.CrystalButton();
            this.btnIgnore = new Top4ever.CustomControl.CrystalButton();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.exTabControl1 = new Top4ever.CustomControl.ExTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvCallRecord = new Top4ever.CustomControl.DataGirdViewExt();
            this.ckbSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colTelephone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCallTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCallRecordID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGirdViewExt1 = new Top4ever.CustomControl.DataGirdViewExt();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CallRecordID1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dataGirdViewExt2 = new Top4ever.CustomControl.DataGirdViewExt();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CallRecordID2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dataGirdViewExt3 = new Top4ever.CustomControl.DataGirdViewExt();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CallRecordID3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCallRecord)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGirdViewExt1)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGirdViewExt2)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGirdViewExt3)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(43)))), ((int)(((byte)(59)))));
            this.btnClose.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(374, 361);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 57);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnIgnore
            // 
            this.btnIgnore.BackColor = System.Drawing.Color.Teal;
            this.btnIgnore.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnIgnore.ForeColor = System.Drawing.Color.White;
            this.btnIgnore.Location = new System.Drawing.Point(263, 361);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(90, 57);
            this.btnIgnore.TabIndex = 2;
            this.btnIgnore.Text = "忽略";
            this.btnIgnore.UseVisualStyleBackColor = false;
            this.btnIgnore.Click += new System.EventHandler(this.btnIgnore_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "application-sidebar.png");
            this.imageList.Images.SetKeyName(1, "color-swatch.png");
            this.imageList.Images.SetKeyName(2, "blue-document-text.png");
            this.imageList.Images.SetKeyName(3, "calendar-blue.png");
            // 
            // exTabControl1
            // 
            this.exTabControl1.Controls.Add(this.tabPage1);
            this.exTabControl1.Controls.Add(this.tabPage2);
            this.exTabControl1.Controls.Add(this.tabPage3);
            this.exTabControl1.Controls.Add(this.tabPage4);
            this.exTabControl1.Font = new System.Drawing.Font("宋体", 9F);
            this.exTabControl1.ImageList = this.imageList;
            this.exTabControl1.ItemSize = new System.Drawing.Size(60, 55);
            this.exTabControl1.Location = new System.Drawing.Point(12, 8);
            this.exTabControl1.Multiline = true;
            this.exTabControl1.Name = "exTabControl1";
            this.exTabControl1.SelectedIndex = 0;
            this.exTabControl1.Size = new System.Drawing.Size(455, 344);
            this.exTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.exTabControl1.TabBackgroundImage = global::VechsoftPos.Properties.Resources.TabButtonBackground;
            this.exTabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvCallRecord);
            this.tabPage1.ImageIndex = 0;
            this.tabPage1.Location = new System.Drawing.Point(4, 59);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(447, 281);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "未接来电";
            // 
            // dgvCallRecord
            // 
            this.dgvCallRecord.AllowUserToAddRows = false;
            this.dgvCallRecord.AllowUserToDeleteRows = false;
            this.dgvCallRecord.AllowUserToResizeColumns = false;
            this.dgvCallRecord.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(248)))));
            this.dgvCallRecord.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCallRecord.BackgroundColor = System.Drawing.Color.White;
            this.dgvCallRecord.ColumnHeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(210)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCallRecord.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCallRecord.ColumnHeadersHeight = 32;
            this.dgvCallRecord.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ckbSelect,
            this.colTelephone,
            this.colCustomerName,
            this.colCallTime,
            this.colCallRecordID});
            this.dgvCallRecord.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.dgvCallRecord.Location = new System.Drawing.Point(0, 0);
            this.dgvCallRecord.MultiSelect = false;
            this.dgvCallRecord.Name = "dgvCallRecord";
            this.dgvCallRecord.RowHeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(220)))), ((int)(((byte)(248)))));
            this.dgvCallRecord.RowHeadersVisible = false;
            this.dgvCallRecord.RowTemplate.Height = 23;
            this.dgvCallRecord.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCallRecord.Size = new System.Drawing.Size(447, 281);
            this.dgvCallRecord.TabIndex = 2;
            this.dgvCallRecord.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCallRecord_CellDoubleClick);
            // 
            // ckbSelect
            // 
            this.ckbSelect.HeaderText = "选择";
            this.ckbSelect.Name = "ckbSelect";
            this.ckbSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ckbSelect.Width = 52;
            // 
            // colTelephone
            // 
            this.colTelephone.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTelephone.HeaderText = "联系电话";
            this.colTelephone.Name = "colTelephone";
            this.colTelephone.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colCustomerName
            // 
            this.colCustomerName.HeaderText = "姓名";
            this.colCustomerName.Name = "colCustomerName";
            this.colCustomerName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCustomerName.Width = 80;
            // 
            // colCallTime
            // 
            this.colCallTime.HeaderText = "来电时间";
            this.colCallTime.Name = "colCallTime";
            this.colCallTime.Width = 150;
            // 
            // colCallRecordID
            // 
            this.colCallRecordID.HeaderText = "CallRecordID";
            this.colCallRecordID.Name = "colCallRecordID";
            this.colCallRecordID.Visible = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGirdViewExt1);
            this.tabPage2.ImageIndex = 1;
            this.tabPage2.Location = new System.Drawing.Point(4, 59);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(447, 281);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "已接来电";
            // 
            // dataGirdViewExt1
            // 
            this.dataGirdViewExt1.AllowUserToAddRows = false;
            this.dataGirdViewExt1.AllowUserToDeleteRows = false;
            this.dataGirdViewExt1.AllowUserToResizeColumns = false;
            this.dataGirdViewExt1.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(248)))));
            this.dataGirdViewExt1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGirdViewExt1.BackgroundColor = System.Drawing.Color.White;
            this.dataGirdViewExt1.ColumnHeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(210)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGirdViewExt1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGirdViewExt1.ColumnHeadersHeight = 32;
            this.dataGirdViewExt1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.CallRecordID1});
            this.dataGirdViewExt1.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.dataGirdViewExt1.Location = new System.Drawing.Point(0, 0);
            this.dataGirdViewExt1.MultiSelect = false;
            this.dataGirdViewExt1.Name = "dataGirdViewExt1";
            this.dataGirdViewExt1.RowHeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(220)))), ((int)(((byte)(248)))));
            this.dataGirdViewExt1.RowHeadersVisible = false;
            this.dataGirdViewExt1.RowTemplate.Height = 23;
            this.dataGirdViewExt1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGirdViewExt1.Size = new System.Drawing.Size(447, 281);
            this.dataGirdViewExt1.TabIndex = 1;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.HeaderText = "联系电话";
            this.Column1.Name = "Column1";
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "姓名";
            this.Column2.Name = "Column2";
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 80;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "来电时间";
            this.Column3.Name = "Column3";
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column3.Width = 150;
            // 
            // CallRecordID1
            // 
            this.CallRecordID1.HeaderText = "CallRecordID1";
            this.CallRecordID1.Name = "CallRecordID1";
            this.CallRecordID1.Visible = false;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGirdViewExt2);
            this.tabPage3.ImageIndex = 2;
            this.tabPage3.Location = new System.Drawing.Point(4, 59);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(447, 281);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "忽略来电";
            // 
            // dataGirdViewExt2
            // 
            this.dataGirdViewExt2.AllowUserToAddRows = false;
            this.dataGirdViewExt2.AllowUserToDeleteRows = false;
            this.dataGirdViewExt2.AllowUserToResizeColumns = false;
            this.dataGirdViewExt2.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(248)))));
            this.dataGirdViewExt2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGirdViewExt2.BackgroundColor = System.Drawing.Color.White;
            this.dataGirdViewExt2.ColumnHeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(210)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGirdViewExt2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGirdViewExt2.ColumnHeadersHeight = 32;
            this.dataGirdViewExt2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column4,
            this.Column5,
            this.Column6,
            this.CallRecordID2});
            this.dataGirdViewExt2.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.dataGirdViewExt2.Location = new System.Drawing.Point(0, 0);
            this.dataGirdViewExt2.MultiSelect = false;
            this.dataGirdViewExt2.Name = "dataGirdViewExt2";
            this.dataGirdViewExt2.RowHeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(220)))), ((int)(((byte)(248)))));
            this.dataGirdViewExt2.RowHeadersVisible = false;
            this.dataGirdViewExt2.RowTemplate.Height = 23;
            this.dataGirdViewExt2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGirdViewExt2.Size = new System.Drawing.Size(447, 281);
            this.dataGirdViewExt2.TabIndex = 1;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column4.HeaderText = "联系电话";
            this.Column4.Name = "Column4";
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "姓名";
            this.Column5.Name = "Column5";
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column5.Width = 80;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "来电时间";
            this.Column6.Name = "Column6";
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column6.Width = 150;
            // 
            // CallRecordID2
            // 
            this.CallRecordID2.HeaderText = "CallRecordID2";
            this.CallRecordID2.Name = "CallRecordID2";
            this.CallRecordID2.Visible = false;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.dataGirdViewExt3);
            this.tabPage4.ImageIndex = 3;
            this.tabPage4.Location = new System.Drawing.Point(4, 59);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(447, 281);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "全部通话";
            // 
            // dataGirdViewExt3
            // 
            this.dataGirdViewExt3.AllowUserToAddRows = false;
            this.dataGirdViewExt3.AllowUserToDeleteRows = false;
            this.dataGirdViewExt3.AllowUserToResizeColumns = false;
            this.dataGirdViewExt3.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(248)))));
            this.dataGirdViewExt3.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGirdViewExt3.BackgroundColor = System.Drawing.Color.White;
            this.dataGirdViewExt3.ColumnHeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(210)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGirdViewExt3.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dataGirdViewExt3.ColumnHeadersHeight = 32;
            this.dataGirdViewExt3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column7,
            this.Column8,
            this.Column9,
            this.CallRecordID3});
            this.dataGirdViewExt3.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.dataGirdViewExt3.Location = new System.Drawing.Point(0, 0);
            this.dataGirdViewExt3.MultiSelect = false;
            this.dataGirdViewExt3.Name = "dataGirdViewExt3";
            this.dataGirdViewExt3.RowHeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(220)))), ((int)(((byte)(248)))));
            this.dataGirdViewExt3.RowHeadersVisible = false;
            this.dataGirdViewExt3.RowTemplate.Height = 23;
            this.dataGirdViewExt3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGirdViewExt3.Size = new System.Drawing.Size(447, 281);
            this.dataGirdViewExt3.TabIndex = 3;
            // 
            // Column7
            // 
            this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column7.HeaderText = "联系电话";
            this.Column7.Name = "Column7";
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "姓名";
            this.Column8.Name = "Column8";
            this.Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column8.Width = 80;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "来电时间";
            this.Column9.Name = "Column9";
            this.Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column9.Width = 150;
            // 
            // CallRecordID3
            // 
            this.CallRecordID3.HeaderText = "CallRecordID3";
            this.CallRecordID3.Name = "CallRecordID3";
            this.CallRecordID3.Visible = false;
            // 
            // FormRecentlyCallRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 436);
            this.Controls.Add(this.exTabControl1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnIgnore);
            this.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRecentlyCallRecord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "最近通话记录";
            this.Load += new System.EventHandler(this.FormRecentlyCallRecord_Load);
            this.exTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCallRecord)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGirdViewExt1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGirdViewExt2)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGirdViewExt3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Top4ever.CustomControl.CrystalButton btnClose;
        private Top4ever.CustomControl.CrystalButton btnIgnore;
        private System.Windows.Forms.ImageList imageList;
        private Top4ever.CustomControl.ExTabControl exTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private Top4ever.CustomControl.DataGirdViewExt dgvCallRecord;
        private Top4ever.CustomControl.DataGirdViewExt dataGirdViewExt1;
        private Top4ever.CustomControl.DataGirdViewExt dataGirdViewExt2;
        private Top4ever.CustomControl.DataGirdViewExt dataGirdViewExt3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ckbSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTelephone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCallTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCallRecordID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn CallRecordID1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn CallRecordID2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn CallRecordID3;
    }
}