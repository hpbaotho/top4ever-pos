namespace Top4ever.Pos.TakeawayCall
{
    partial class FormTakeGoods
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtOrderAmount = new System.Windows.Forms.TextBox();
            this.txtEmployeeNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTranSeq = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvGoodsOrder = new System.Windows.Forms.DataGridView();
            this.GoodsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SellPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Discount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPayoffWay = new System.Windows.Forms.DataGridView();
            this.btnTakeGoods = new Top4ever.CustomControl.CrystalButton();
            this.btnCancel = new Top4ever.CustomControl.CrystalButton();
            this.PayDefName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChangePay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RealPay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGoodsOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayoffWay)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtOrderAmount);
            this.groupBox1.Controls.Add(this.txtEmployeeNo);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtTranSeq);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(520, 58);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // txtOrderAmount
            // 
            this.txtOrderAmount.Location = new System.Drawing.Point(427, 20);
            this.txtOrderAmount.Name = "txtOrderAmount";
            this.txtOrderAmount.ReadOnly = true;
            this.txtOrderAmount.Size = new System.Drawing.Size(84, 25);
            this.txtOrderAmount.TabIndex = 1;
            // 
            // txtEmployeeNo
            // 
            this.txtEmployeeNo.Location = new System.Drawing.Point(239, 20);
            this.txtEmployeeNo.Name = "txtEmployeeNo";
            this.txtEmployeeNo.ReadOnly = true;
            this.txtEmployeeNo.Size = new System.Drawing.Size(84, 25);
            this.txtEmployeeNo.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(352, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 19);
            this.label3.TabIndex = 0;
            this.label3.Text = "账单金额：";
            // 
            // txtTranSeq
            // 
            this.txtTranSeq.Location = new System.Drawing.Point(67, 20);
            this.txtTranSeq.Name = "txtTranSeq";
            this.txtTranSeq.ReadOnly = true;
            this.txtTranSeq.Size = new System.Drawing.Size(84, 25);
            this.txtTranSeq.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(177, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 19);
            this.label2.TabIndex = 0;
            this.label2.Text = "服务员：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "流水号：";
            // 
            // dgvGoodsOrder
            // 
            this.dgvGoodsOrder.AllowUserToAddRows = false;
            this.dgvGoodsOrder.AllowUserToDeleteRows = false;
            this.dgvGoodsOrder.AllowUserToResizeColumns = false;
            this.dgvGoodsOrder.AllowUserToResizeRows = false;
            this.dgvGoodsOrder.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGoodsOrder.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.dgvGoodsOrder.ColumnHeadersHeight = 35;
            this.dgvGoodsOrder.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GoodsName,
            this.ItemQty,
            this.SellPrice,
            this.Discount});
            this.dgvGoodsOrder.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvGoodsOrder.Location = new System.Drawing.Point(12, 66);
            this.dgvGoodsOrder.Name = "dgvGoodsOrder";
            this.dgvGoodsOrder.RowHeadersVisible = false;
            this.dgvGoodsOrder.RowTemplate.Height = 23;
            this.dgvGoodsOrder.Size = new System.Drawing.Size(520, 318);
            this.dgvGoodsOrder.TabIndex = 2;
            // 
            // GoodsName
            // 
            this.GoodsName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.GoodsName.DefaultCellStyle = dataGridViewCellStyle20;
            this.GoodsName.HeaderText = " 品名";
            this.GoodsName.Name = "GoodsName";
            // 
            // ItemQty
            // 
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle21.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.ItemQty.DefaultCellStyle = dataGridViewCellStyle21;
            this.ItemQty.HeaderText = "数量";
            this.ItemQty.Name = "ItemQty";
            this.ItemQty.Width = 86;
            // 
            // SellPrice
            // 
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle22.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle22.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.SellPrice.DefaultCellStyle = dataGridViewCellStyle22;
            this.SellPrice.HeaderText = "金额";
            this.SellPrice.Name = "SellPrice";
            // 
            // Discount
            // 
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle23.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle23.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.Discount.DefaultCellStyle = dataGridViewCellStyle23;
            this.Discount.HeaderText = "折扣";
            this.Discount.Name = "Discount";
            // 
            // dgvPayoffWay
            // 
            this.dgvPayoffWay.AllowUserToAddRows = false;
            this.dgvPayoffWay.AllowUserToDeleteRows = false;
            this.dgvPayoffWay.AllowUserToResizeColumns = false;
            this.dgvPayoffWay.AllowUserToResizeRows = false;
            this.dgvPayoffWay.BackgroundColor = System.Drawing.Color.White;
            this.dgvPayoffWay.ColumnHeadersVisible = false;
            this.dgvPayoffWay.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PayDefName,
            this.TotalMoney,
            this.ChangePay,
            this.RealPay});
            this.dgvPayoffWay.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvPayoffWay.Location = new System.Drawing.Point(12, 390);
            this.dgvPayoffWay.Name = "dgvPayoffWay";
            this.dgvPayoffWay.RowHeadersVisible = false;
            this.dgvPayoffWay.RowTemplate.Height = 23;
            this.dgvPayoffWay.Size = new System.Drawing.Size(520, 168);
            this.dgvPayoffWay.TabIndex = 2;
            // 
            // btnTakeGoods
            // 
            this.btnTakeGoods.BackColor = System.Drawing.Color.Teal;
            this.btnTakeGoods.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTakeGoods.ForeColor = System.Drawing.Color.White;
            this.btnTakeGoods.Location = new System.Drawing.Point(158, 576);
            this.btnTakeGoods.Name = "btnTakeGoods";
            this.btnTakeGoods.Size = new System.Drawing.Size(92, 50);
            this.btnTakeGoods.TabIndex = 2;
            this.btnTakeGoods.Text = "出货";
            this.btnTakeGoods.UseVisualStyleBackColor = false;
            this.btnTakeGoods.Click += new System.EventHandler(this.btnTakeGoods_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(43)))), ((int)(((byte)(59)))));
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(294, 576);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 50);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // PayDefName
            // 
            this.PayDefName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle24.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PayDefName.DefaultCellStyle = dataGridViewCellStyle24;
            this.PayDefName.HeaderText = "PayDefName";
            this.PayDefName.Name = "PayDefName";
            // 
            // TotalMoney
            // 
            dataGridViewCellStyle25.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle25.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.TotalMoney.DefaultCellStyle = dataGridViewCellStyle25;
            this.TotalMoney.HeaderText = "TotalMoney";
            this.TotalMoney.Name = "TotalMoney";
            this.TotalMoney.Width = 80;
            // 
            // ChangePay
            // 
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle26.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle26.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.ChangePay.DefaultCellStyle = dataGridViewCellStyle26;
            this.ChangePay.HeaderText = "ChangePay";
            this.ChangePay.Name = "ChangePay";
            this.ChangePay.Width = 86;
            // 
            // RealPay
            // 
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle27.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle27.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.RealPay.DefaultCellStyle = dataGridViewCellStyle27;
            this.RealPay.HeaderText = "RealPay";
            this.RealPay.Name = "RealPay";
            // 
            // FormTakeGoods
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 640);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnTakeGoods);
            this.Controls.Add(this.dgvPayoffWay);
            this.Controls.Add(this.dgvGoodsOrder);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTakeGoods";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "外带出货信息";
            this.Load += new System.EventHandler(this.FormTakeGoods_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGoodsOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayoffWay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtOrderAmount;
        private System.Windows.Forms.TextBox txtEmployeeNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTranSeq;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvGoodsOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn SellPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Discount;
        private System.Windows.Forms.DataGridView dgvPayoffWay;
        private CustomControl.CrystalButton btnTakeGoods;
        private CustomControl.CrystalButton btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn PayDefName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChangePay;
        private System.Windows.Forms.DataGridViewTextBoxColumn RealPay;
    }
}