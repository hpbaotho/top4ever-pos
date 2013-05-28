namespace Top4ever.Pos.Feature
{
    partial class FormDiscount
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDiscount));
            this.btnConfirm = new Top4ever.CustomControl.CrystalButton();
            this.btnCancel = new Top4ever.CustomControl.CrystalButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtDiscount = new System.Windows.Forms.TextBox();
            this.lbDiscount = new System.Windows.Forms.Label();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.exTabControl1 = new Top4ever.CustomControl.ExTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnCustomPercent = new Top4ever.CustomControl.CrystalButton();
            this.btnCustomFixedAmount = new Top4ever.CustomControl.CrystalButton();
            this.btnCustomDiscountTo = new Top4ever.CustomControl.CrystalButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.exTabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Teal;
            this.btnConfirm.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Location = new System.Drawing.Point(255, 445);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(92, 58);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(43)))), ((int)(((byte)(59)))));
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(107, 445);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 58);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtDiscount);
            this.groupBox1.Controls.Add(this.lbDiscount);
            this.groupBox1.Location = new System.Drawing.Point(12, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(443, 58);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // txtDiscount
            // 
            this.txtDiscount.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDiscount.Location = new System.Drawing.Point(136, 18);
            this.txtDiscount.Name = "txtDiscount";
            this.txtDiscount.Size = new System.Drawing.Size(242, 29);
            this.txtDiscount.TabIndex = 3;
            // 
            // lbDiscount
            // 
            this.lbDiscount.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDiscount.Location = new System.Drawing.Point(40, 21);
            this.lbDiscount.Name = "lbDiscount";
            this.lbDiscount.Size = new System.Drawing.Size(90, 19);
            this.lbDiscount.TabIndex = 2;
            this.lbDiscount.Text = "折扣方式：";
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.exTabControl1);
            this.groupBox2.Location = new System.Drawing.Point(12, 71);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(443, 355);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // exTabControl1
            // 
            this.exTabControl1.Alignment = System.Windows.Forms.TabAlignment.Right;
            this.exTabControl1.Controls.Add(this.tabPage1);
            this.exTabControl1.Controls.Add(this.tabPage2);
            this.exTabControl1.Controls.Add(this.tabPage3);
            this.exTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exTabControl1.ImageList = this.imageList;
            this.exTabControl1.ItemSize = new System.Drawing.Size(55, 60);
            this.exTabControl1.Location = new System.Drawing.Point(3, 17);
            this.exTabControl1.Multiline = true;
            this.exTabControl1.Name = "exTabControl1";
            this.exTabControl1.SelectedIndex = 0;
            this.exTabControl1.Size = new System.Drawing.Size(437, 335);
            this.exTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.exTabControl1.TabBackgroundImage = global::Top4ever.Pos.Properties.Resources.TabButtonBackground;
            this.exTabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.ImageIndex = 0;
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(369, 327);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "百分比";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.ImageIndex = 1;
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(369, 327);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "固定金额";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnCustomDiscountTo);
            this.tabPage3.Controls.Add(this.btnCustomFixedAmount);
            this.tabPage3.Controls.Add(this.btnCustomPercent);
            this.tabPage3.ImageIndex = 3;
            this.tabPage3.Location = new System.Drawing.Point(4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(369, 327);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "自定义折扣";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnCustomPercent
            // 
            this.btnCustomPercent.BackColor = System.Drawing.Color.MidnightBlue;
            this.btnCustomPercent.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCustomPercent.ForeColor = System.Drawing.Color.White;
            this.btnCustomPercent.Location = new System.Drawing.Point(8, 6);
            this.btnCustomPercent.Name = "btnCustomPercent";
            this.btnCustomPercent.Size = new System.Drawing.Size(354, 100);
            this.btnCustomPercent.TabIndex = 0;
            this.btnCustomPercent.Text = "折扣（百分比）";
            this.btnCustomPercent.UseVisualStyleBackColor = false;
            this.btnCustomPercent.Click += new System.EventHandler(this.btnCustomPercent_Click);
            // 
            // btnCustomFixedAmount
            // 
            this.btnCustomFixedAmount.BackColor = System.Drawing.Color.Indigo;
            this.btnCustomFixedAmount.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCustomFixedAmount.ForeColor = System.Drawing.Color.White;
            this.btnCustomFixedAmount.Location = new System.Drawing.Point(8, 112);
            this.btnCustomFixedAmount.Name = "btnCustomFixedAmount";
            this.btnCustomFixedAmount.Size = new System.Drawing.Size(354, 100);
            this.btnCustomFixedAmount.TabIndex = 0;
            this.btnCustomFixedAmount.Text = "折扣（固定金额）";
            this.btnCustomFixedAmount.UseVisualStyleBackColor = false;
            this.btnCustomFixedAmount.Click += new System.EventHandler(this.btnCustomFixedAmount_Click);
            // 
            // btnCustomDiscountTo
            // 
            this.btnCustomDiscountTo.BackColor = System.Drawing.Color.Maroon;
            this.btnCustomDiscountTo.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCustomDiscountTo.ForeColor = System.Drawing.Color.White;
            this.btnCustomDiscountTo.Location = new System.Drawing.Point(8, 218);
            this.btnCustomDiscountTo.Name = "btnCustomDiscountTo";
            this.btnCustomDiscountTo.Size = new System.Drawing.Size(354, 100);
            this.btnCustomDiscountTo.TabIndex = 0;
            this.btnCustomDiscountTo.Text = "折扣到（固定金额）";
            this.btnCustomDiscountTo.UseVisualStyleBackColor = false;
            this.btnCustomDiscountTo.Click += new System.EventHandler(this.btnCustomDiscountTo_Click);
            // 
            // FormDiscount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 523);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDiscount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "折扣方式";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.exTabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CustomControl.CrystalButton btnConfirm;
        private CustomControl.CrystalButton btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtDiscount;
        private System.Windows.Forms.Label lbDiscount;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.GroupBox groupBox2;
        private CustomControl.ExTabControl exTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private CustomControl.CrystalButton btnCustomPercent;
        private CustomControl.CrystalButton btnCustomDiscountTo;
        private CustomControl.CrystalButton btnCustomFixedAmount;
    }
}