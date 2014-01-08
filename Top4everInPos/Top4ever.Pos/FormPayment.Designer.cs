namespace Top4ever.Pos
{
    partial class FormPayment
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlPayoffWay = new System.Windows.Forms.Panel();
            this.btnPageDown = new Top4ever.CustomControl.CrystalButton();
            this.btnPageUp = new Top4ever.CustomControl.CrystalButton();
            this.pnlNumericPad = new System.Windows.Forms.Panel();
            this.btnNumeric8 = new Top4ever.CustomControl.CrystalButton();
            this.btnNumeric5 = new Top4ever.CustomControl.CrystalButton();
            this.btnDot = new Top4ever.CustomControl.CrystalButton();
            this.btnNumeric2 = new Top4ever.CustomControl.CrystalButton();
            this.btnNumeric9 = new Top4ever.CustomControl.CrystalButton();
            this.btnNumeric6 = new Top4ever.CustomControl.CrystalButton();
            this.btnClear = new Top4ever.CustomControl.CrystalButton();
            this.btnNumeric3 = new Top4ever.CustomControl.CrystalButton();
            this.btnNumeric7 = new Top4ever.CustomControl.CrystalButton();
            this.btnZero = new Top4ever.CustomControl.CrystalButton();
            this.btnNumeric4 = new Top4ever.CustomControl.CrystalButton();
            this.btnNumeric1 = new Top4ever.CustomControl.CrystalButton();
            this.btnReal = new Top4ever.CustomControl.CrystalButton();
            this.btnAdd5 = new Top4ever.CustomControl.CrystalButton();
            this.btnAdd10 = new Top4ever.CustomControl.CrystalButton();
            this.btnAdd20 = new Top4ever.CustomControl.CrystalButton();
            this.btnAdd50 = new Top4ever.CustomControl.CrystalButton();
            this.btnAdd100 = new Top4ever.CustomControl.CrystalButton();
            this.btnAdd200 = new Top4ever.CustomControl.CrystalButton();
            this.btnAdd500 = new Top4ever.CustomControl.CrystalButton();
            this.pnlPayTypeNum = new System.Windows.Forms.Panel();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.lbMulti = new System.Windows.Forms.Label();
            this.txtPayoff = new System.Windows.Forms.TextBox();
            this.pnlBasicInfo = new System.Windows.Forms.Panel();
            this.lbNeedChangePay = new System.Windows.Forms.Label();
            this.lbPaidInMoney = new System.Windows.Forms.Label();
            this.lbReceMoney = new System.Windows.Forms.Label();
            this.lbUnpaidAmount = new System.Windows.Forms.Label();
            this.lbDiscount = new System.Windows.Forms.Label();
            this.lbTotalPrice = new System.Windows.Forms.Label();
            this.dgvPayment = new System.Windows.Forms.DataGridView();
            this.btnCancel = new Top4ever.CustomControl.CrystalButton();
            this.btnCheckOut = new Top4ever.CustomControl.CrystalButton();
            this.PayoffName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GoodsPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlPayoffWay.SuspendLayout();
            this.pnlNumericPad.SuspendLayout();
            this.pnlPayTypeNum.SuspendLayout();
            this.pnlBasicInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayment)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlPayoffWay
            // 
            this.pnlPayoffWay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPayoffWay.Controls.Add(this.btnPageDown);
            this.pnlPayoffWay.Controls.Add(this.btnPageUp);
            this.pnlPayoffWay.Location = new System.Drawing.Point(483, 0);
            this.pnlPayoffWay.Name = "pnlPayoffWay";
            this.pnlPayoffWay.Size = new System.Drawing.Size(526, 188);
            this.pnlPayoffWay.TabIndex = 1;
            // 
            // btnPageDown
            // 
            this.btnPageDown.BackColor = System.Drawing.Color.Teal;
            this.btnPageDown.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPageDown.ForeColor = System.Drawing.Color.White;
            this.btnPageDown.Location = new System.Drawing.Point(440, 106);
            this.btnPageDown.Name = "btnPageDown";
            this.btnPageDown.Size = new System.Drawing.Size(80, 76);
            this.btnPageDown.TabIndex = 2;
            this.btnPageDown.Text = "向后";
            this.btnPageDown.UseVisualStyleBackColor = false;
            this.btnPageDown.Click += new System.EventHandler(this.btnPageDown_Click);
            // 
            // btnPageUp
            // 
            this.btnPageUp.BackColor = System.Drawing.Color.Tomato;
            this.btnPageUp.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPageUp.ForeColor = System.Drawing.Color.White;
            this.btnPageUp.Location = new System.Drawing.Point(354, 106);
            this.btnPageUp.Name = "btnPageUp";
            this.btnPageUp.Size = new System.Drawing.Size(80, 76);
            this.btnPageUp.TabIndex = 1;
            this.btnPageUp.Text = "向前";
            this.btnPageUp.UseVisualStyleBackColor = false;
            this.btnPageUp.Click += new System.EventHandler(this.btnPageUp_Click);
            // 
            // pnlNumericPad
            // 
            this.pnlNumericPad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(84)))), ((int)(((byte)(97)))));
            this.pnlNumericPad.Controls.Add(this.btnNumeric8);
            this.pnlNumericPad.Controls.Add(this.btnNumeric5);
            this.pnlNumericPad.Controls.Add(this.btnDot);
            this.pnlNumericPad.Controls.Add(this.btnNumeric2);
            this.pnlNumericPad.Controls.Add(this.btnNumeric9);
            this.pnlNumericPad.Controls.Add(this.btnNumeric6);
            this.pnlNumericPad.Controls.Add(this.btnClear);
            this.pnlNumericPad.Controls.Add(this.btnNumeric3);
            this.pnlNumericPad.Controls.Add(this.btnNumeric7);
            this.pnlNumericPad.Controls.Add(this.btnZero);
            this.pnlNumericPad.Controls.Add(this.btnNumeric4);
            this.pnlNumericPad.Controls.Add(this.btnNumeric1);
            this.pnlNumericPad.Controls.Add(this.btnReal);
            this.pnlNumericPad.Controls.Add(this.btnAdd5);
            this.pnlNumericPad.Controls.Add(this.btnAdd10);
            this.pnlNumericPad.Controls.Add(this.btnAdd20);
            this.pnlNumericPad.Controls.Add(this.btnAdd50);
            this.pnlNumericPad.Controls.Add(this.btnAdd100);
            this.pnlNumericPad.Controls.Add(this.btnAdd200);
            this.pnlNumericPad.Controls.Add(this.btnAdd500);
            this.pnlNumericPad.Location = new System.Drawing.Point(483, 235);
            this.pnlNumericPad.Name = "pnlNumericPad";
            this.pnlNumericPad.Size = new System.Drawing.Size(526, 316);
            this.pnlNumericPad.TabIndex = 1;
            // 
            // btnNumeric8
            // 
            this.btnNumeric8.BackColor = System.Drawing.Color.Black;
            this.btnNumeric8.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.btnNumeric8.ForeColor = System.Drawing.Color.White;
            this.btnNumeric8.Location = new System.Drawing.Point(325, 6);
            this.btnNumeric8.Name = "btnNumeric8";
            this.btnNumeric8.Size = new System.Drawing.Size(95, 73);
            this.btnNumeric8.TabIndex = 17;
            this.btnNumeric8.Text = "8";
            this.btnNumeric8.UseVisualStyleBackColor = false;
            this.btnNumeric8.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnNumeric5
            // 
            this.btnNumeric5.BackColor = System.Drawing.Color.Black;
            this.btnNumeric5.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.btnNumeric5.ForeColor = System.Drawing.Color.White;
            this.btnNumeric5.Location = new System.Drawing.Point(325, 83);
            this.btnNumeric5.Name = "btnNumeric5";
            this.btnNumeric5.Size = new System.Drawing.Size(95, 73);
            this.btnNumeric5.TabIndex = 16;
            this.btnNumeric5.Text = "5";
            this.btnNumeric5.UseVisualStyleBackColor = false;
            this.btnNumeric5.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDot
            // 
            this.btnDot.BackColor = System.Drawing.Color.Black;
            this.btnDot.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDot.ForeColor = System.Drawing.Color.White;
            this.btnDot.Location = new System.Drawing.Point(325, 237);
            this.btnDot.Name = "btnDot";
            this.btnDot.Size = new System.Drawing.Size(95, 73);
            this.btnDot.TabIndex = 15;
            this.btnDot.Text = ".";
            this.btnDot.UseVisualStyleBackColor = false;
            this.btnDot.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnNumeric2
            // 
            this.btnNumeric2.BackColor = System.Drawing.Color.Black;
            this.btnNumeric2.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.btnNumeric2.ForeColor = System.Drawing.Color.White;
            this.btnNumeric2.Location = new System.Drawing.Point(325, 160);
            this.btnNumeric2.Name = "btnNumeric2";
            this.btnNumeric2.Size = new System.Drawing.Size(95, 73);
            this.btnNumeric2.TabIndex = 20;
            this.btnNumeric2.Text = "2";
            this.btnNumeric2.UseVisualStyleBackColor = false;
            this.btnNumeric2.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnNumeric9
            // 
            this.btnNumeric9.BackColor = System.Drawing.Color.Black;
            this.btnNumeric9.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.btnNumeric9.ForeColor = System.Drawing.Color.White;
            this.btnNumeric9.Location = new System.Drawing.Point(424, 6);
            this.btnNumeric9.Name = "btnNumeric9";
            this.btnNumeric9.Size = new System.Drawing.Size(95, 73);
            this.btnNumeric9.TabIndex = 19;
            this.btnNumeric9.Text = "9";
            this.btnNumeric9.UseVisualStyleBackColor = false;
            this.btnNumeric9.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnNumeric6
            // 
            this.btnNumeric6.BackColor = System.Drawing.Color.Black;
            this.btnNumeric6.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.btnNumeric6.ForeColor = System.Drawing.Color.White;
            this.btnNumeric6.Location = new System.Drawing.Point(424, 83);
            this.btnNumeric6.Name = "btnNumeric6";
            this.btnNumeric6.Size = new System.Drawing.Size(95, 73);
            this.btnNumeric6.TabIndex = 18;
            this.btnNumeric6.Text = "6";
            this.btnNumeric6.UseVisualStyleBackColor = false;
            this.btnNumeric6.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(123)))), ((int)(((byte)(31)))));
            this.btnClear.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(424, 237);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(95, 73);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "清除";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnNumeric3
            // 
            this.btnNumeric3.BackColor = System.Drawing.Color.Black;
            this.btnNumeric3.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.btnNumeric3.ForeColor = System.Drawing.Color.White;
            this.btnNumeric3.Location = new System.Drawing.Point(424, 160);
            this.btnNumeric3.Name = "btnNumeric3";
            this.btnNumeric3.Size = new System.Drawing.Size(95, 73);
            this.btnNumeric3.TabIndex = 10;
            this.btnNumeric3.Text = "3";
            this.btnNumeric3.UseVisualStyleBackColor = false;
            this.btnNumeric3.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnNumeric7
            // 
            this.btnNumeric7.BackColor = System.Drawing.Color.Black;
            this.btnNumeric7.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnNumeric7.ForeColor = System.Drawing.Color.White;
            this.btnNumeric7.Location = new System.Drawing.Point(226, 6);
            this.btnNumeric7.Name = "btnNumeric7";
            this.btnNumeric7.Size = new System.Drawing.Size(95, 73);
            this.btnNumeric7.TabIndex = 9;
            this.btnNumeric7.Text = "7";
            this.btnNumeric7.UseVisualStyleBackColor = false;
            this.btnNumeric7.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnZero
            // 
            this.btnZero.BackColor = System.Drawing.Color.Black;
            this.btnZero.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.btnZero.ForeColor = System.Drawing.Color.White;
            this.btnZero.Location = new System.Drawing.Point(226, 237);
            this.btnZero.Name = "btnZero";
            this.btnZero.Size = new System.Drawing.Size(95, 73);
            this.btnZero.TabIndex = 14;
            this.btnZero.Text = "0";
            this.btnZero.UseVisualStyleBackColor = false;
            this.btnZero.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnNumeric4
            // 
            this.btnNumeric4.BackColor = System.Drawing.Color.Black;
            this.btnNumeric4.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.btnNumeric4.ForeColor = System.Drawing.Color.White;
            this.btnNumeric4.Location = new System.Drawing.Point(226, 83);
            this.btnNumeric4.Name = "btnNumeric4";
            this.btnNumeric4.Size = new System.Drawing.Size(95, 73);
            this.btnNumeric4.TabIndex = 13;
            this.btnNumeric4.Text = "4";
            this.btnNumeric4.UseVisualStyleBackColor = false;
            this.btnNumeric4.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnNumeric1
            // 
            this.btnNumeric1.BackColor = System.Drawing.Color.Black;
            this.btnNumeric1.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.btnNumeric1.ForeColor = System.Drawing.Color.White;
            this.btnNumeric1.Location = new System.Drawing.Point(226, 160);
            this.btnNumeric1.Name = "btnNumeric1";
            this.btnNumeric1.Size = new System.Drawing.Size(95, 73);
            this.btnNumeric1.TabIndex = 12;
            this.btnNumeric1.Text = "1";
            this.btnNumeric1.UseVisualStyleBackColor = false;
            this.btnNumeric1.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnReal
            // 
            this.btnReal.BackColor = System.Drawing.Color.SeaGreen;
            this.btnReal.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.btnReal.ForeColor = System.Drawing.Color.White;
            this.btnReal.Location = new System.Drawing.Point(114, 237);
            this.btnReal.Name = "btnReal";
            this.btnReal.Size = new System.Drawing.Size(103, 73);
            this.btnReal.TabIndex = 6;
            this.btnReal.Text = "实际";
            this.btnReal.UseVisualStyleBackColor = false;
            this.btnReal.Click += new System.EventHandler(this.btnReal_Click);
            // 
            // btnAdd5
            // 
            this.btnAdd5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(105)))), ((int)(((byte)(118)))));
            this.btnAdd5.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd5.ForeColor = System.Drawing.Color.White;
            this.btnAdd5.Location = new System.Drawing.Point(114, 160);
            this.btnAdd5.Name = "btnAdd5";
            this.btnAdd5.Size = new System.Drawing.Size(103, 73);
            this.btnAdd5.TabIndex = 5;
            this.btnAdd5.Text = "+5";
            this.btnAdd5.UseVisualStyleBackColor = false;
            this.btnAdd5.Click += new System.EventHandler(this.btnAddBigNumber_Click);
            // 
            // btnAdd10
            // 
            this.btnAdd10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(105)))), ((int)(((byte)(118)))));
            this.btnAdd10.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd10.ForeColor = System.Drawing.Color.White;
            this.btnAdd10.Location = new System.Drawing.Point(114, 83);
            this.btnAdd10.Name = "btnAdd10";
            this.btnAdd10.Size = new System.Drawing.Size(103, 73);
            this.btnAdd10.TabIndex = 8;
            this.btnAdd10.Text = "+10";
            this.btnAdd10.UseVisualStyleBackColor = false;
            this.btnAdd10.Click += new System.EventHandler(this.btnAddBigNumber_Click);
            // 
            // btnAdd20
            // 
            this.btnAdd20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(105)))), ((int)(((byte)(118)))));
            this.btnAdd20.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd20.ForeColor = System.Drawing.Color.White;
            this.btnAdd20.Location = new System.Drawing.Point(114, 6);
            this.btnAdd20.Name = "btnAdd20";
            this.btnAdd20.Size = new System.Drawing.Size(103, 73);
            this.btnAdd20.TabIndex = 7;
            this.btnAdd20.Text = "+20";
            this.btnAdd20.UseVisualStyleBackColor = false;
            this.btnAdd20.Click += new System.EventHandler(this.btnAddBigNumber_Click);
            // 
            // btnAdd50
            // 
            this.btnAdd50.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(105)))), ((int)(((byte)(118)))));
            this.btnAdd50.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd50.ForeColor = System.Drawing.Color.White;
            this.btnAdd50.Location = new System.Drawing.Point(7, 237);
            this.btnAdd50.Name = "btnAdd50";
            this.btnAdd50.Size = new System.Drawing.Size(103, 73);
            this.btnAdd50.TabIndex = 2;
            this.btnAdd50.Text = "+50";
            this.btnAdd50.UseVisualStyleBackColor = false;
            this.btnAdd50.Click += new System.EventHandler(this.btnAddBigNumber_Click);
            // 
            // btnAdd100
            // 
            this.btnAdd100.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(105)))), ((int)(((byte)(118)))));
            this.btnAdd100.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd100.ForeColor = System.Drawing.Color.White;
            this.btnAdd100.Location = new System.Drawing.Point(7, 160);
            this.btnAdd100.Name = "btnAdd100";
            this.btnAdd100.Size = new System.Drawing.Size(103, 73);
            this.btnAdd100.TabIndex = 1;
            this.btnAdd100.Text = "+100";
            this.btnAdd100.UseVisualStyleBackColor = false;
            this.btnAdd100.Click += new System.EventHandler(this.btnAddBigNumber_Click);
            // 
            // btnAdd200
            // 
            this.btnAdd200.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(105)))), ((int)(((byte)(118)))));
            this.btnAdd200.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd200.ForeColor = System.Drawing.Color.White;
            this.btnAdd200.Location = new System.Drawing.Point(7, 83);
            this.btnAdd200.Name = "btnAdd200";
            this.btnAdd200.Size = new System.Drawing.Size(103, 73);
            this.btnAdd200.TabIndex = 4;
            this.btnAdd200.Text = "+200";
            this.btnAdd200.UseVisualStyleBackColor = false;
            this.btnAdd200.Click += new System.EventHandler(this.btnAddBigNumber_Click);
            // 
            // btnAdd500
            // 
            this.btnAdd500.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(105)))), ((int)(((byte)(118)))));
            this.btnAdd500.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd500.ForeColor = System.Drawing.Color.White;
            this.btnAdd500.Location = new System.Drawing.Point(7, 6);
            this.btnAdd500.Name = "btnAdd500";
            this.btnAdd500.Size = new System.Drawing.Size(103, 73);
            this.btnAdd500.TabIndex = 3;
            this.btnAdd500.Text = "+500";
            this.btnAdd500.UseVisualStyleBackColor = false;
            this.btnAdd500.Click += new System.EventHandler(this.btnAddBigNumber_Click);
            // 
            // pnlPayTypeNum
            // 
            this.pnlPayTypeNum.BackColor = System.Drawing.Color.White;
            this.pnlPayTypeNum.Controls.Add(this.txtAmount);
            this.pnlPayTypeNum.Controls.Add(this.lbMulti);
            this.pnlPayTypeNum.Controls.Add(this.txtPayoff);
            this.pnlPayTypeNum.Location = new System.Drawing.Point(483, 189);
            this.pnlPayTypeNum.Name = "pnlPayTypeNum";
            this.pnlPayTypeNum.Size = new System.Drawing.Size(526, 45);
            this.pnlPayTypeNum.TabIndex = 2;
            // 
            // txtAmount
            // 
            this.txtAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(116)))), ((int)(((byte)(155)))));
            this.txtAmount.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAmount.ForeColor = System.Drawing.Color.White;
            this.txtAmount.Location = new System.Drawing.Point(277, 7);
            this.txtAmount.Multiline = true;
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(216, 31);
            this.txtAmount.TabIndex = 8;
            this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbMulti
            // 
            this.lbMulti.AutoSize = true;
            this.lbMulti.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMulti.Location = new System.Drawing.Point(248, 10);
            this.lbMulti.Name = "lbMulti";
            this.lbMulti.Size = new System.Drawing.Size(26, 25);
            this.lbMulti.TabIndex = 7;
            this.lbMulti.Text = "X";
            // 
            // txtPayoff
            // 
            this.txtPayoff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(116)))), ((int)(((byte)(155)))));
            this.txtPayoff.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPayoff.ForeColor = System.Drawing.Color.White;
            this.txtPayoff.Location = new System.Drawing.Point(26, 7);
            this.txtPayoff.Multiline = true;
            this.txtPayoff.Name = "txtPayoff";
            this.txtPayoff.Size = new System.Drawing.Size(216, 31);
            this.txtPayoff.TabIndex = 6;
            this.txtPayoff.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pnlBasicInfo
            // 
            this.pnlBasicInfo.BackColor = System.Drawing.Color.Black;
            this.pnlBasicInfo.Controls.Add(this.lbNeedChangePay);
            this.pnlBasicInfo.Controls.Add(this.lbPaidInMoney);
            this.pnlBasicInfo.Controls.Add(this.lbReceMoney);
            this.pnlBasicInfo.Controls.Add(this.lbUnpaidAmount);
            this.pnlBasicInfo.Controls.Add(this.lbDiscount);
            this.pnlBasicInfo.Controls.Add(this.lbTotalPrice);
            this.pnlBasicInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlBasicInfo.Name = "pnlBasicInfo";
            this.pnlBasicInfo.Size = new System.Drawing.Size(482, 188);
            this.pnlBasicInfo.TabIndex = 3;
            // 
            // lbNeedChangePay
            // 
            this.lbNeedChangePay.AutoSize = true;
            this.lbNeedChangePay.Font = new System.Drawing.Font("微软雅黑", 18F);
            this.lbNeedChangePay.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lbNeedChangePay.Location = new System.Drawing.Point(255, 129);
            this.lbNeedChangePay.Name = "lbNeedChangePay";
            this.lbNeedChangePay.Size = new System.Drawing.Size(86, 31);
            this.lbNeedChangePay.TabIndex = 1;
            this.lbNeedChangePay.Text = "找零：";
            // 
            // lbPaidInMoney
            // 
            this.lbPaidInMoney.AutoSize = true;
            this.lbPaidInMoney.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbPaidInMoney.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(148)))), ((int)(((byte)(91)))));
            this.lbPaidInMoney.Location = new System.Drawing.Point(13, 129);
            this.lbPaidInMoney.Name = "lbPaidInMoney";
            this.lbPaidInMoney.Size = new System.Drawing.Size(134, 31);
            this.lbPaidInMoney.TabIndex = 0;
            this.lbPaidInMoney.Text = "实收金额：";
            // 
            // lbReceMoney
            // 
            this.lbReceMoney.AutoSize = true;
            this.lbReceMoney.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbReceMoney.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(228)))), ((int)(((byte)(44)))));
            this.lbReceMoney.Location = new System.Drawing.Point(13, 77);
            this.lbReceMoney.Name = "lbReceMoney";
            this.lbReceMoney.Size = new System.Drawing.Size(134, 31);
            this.lbReceMoney.TabIndex = 0;
            this.lbReceMoney.Text = "应收金额：";
            // 
            // lbUnpaidAmount
            // 
            this.lbUnpaidAmount.AutoSize = true;
            this.lbUnpaidAmount.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbUnpaidAmount.ForeColor = System.Drawing.Color.Orchid;
            this.lbUnpaidAmount.Location = new System.Drawing.Point(255, 77);
            this.lbUnpaidAmount.Name = "lbUnpaidAmount";
            this.lbUnpaidAmount.Size = new System.Drawing.Size(134, 31);
            this.lbUnpaidAmount.TabIndex = 0;
            this.lbUnpaidAmount.Text = "未付金额：";
            // 
            // lbDiscount
            // 
            this.lbDiscount.AutoSize = true;
            this.lbDiscount.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDiscount.ForeColor = System.Drawing.Color.Chocolate;
            this.lbDiscount.Location = new System.Drawing.Point(255, 26);
            this.lbDiscount.Name = "lbDiscount";
            this.lbDiscount.Size = new System.Drawing.Size(86, 31);
            this.lbDiscount.TabIndex = 0;
            this.lbDiscount.Text = "折扣：";
            // 
            // lbTotalPrice
            // 
            this.lbTotalPrice.AutoSize = true;
            this.lbTotalPrice.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTotalPrice.ForeColor = System.Drawing.Color.Chocolate;
            this.lbTotalPrice.Location = new System.Drawing.Point(13, 26);
            this.lbTotalPrice.Name = "lbTotalPrice";
            this.lbTotalPrice.Size = new System.Drawing.Size(110, 31);
            this.lbTotalPrice.TabIndex = 0;
            this.lbTotalPrice.Text = "总金额：";
            // 
            // dgvPayment
            // 
            this.dgvPayment.AllowUserToAddRows = false;
            this.dgvPayment.AllowUserToDeleteRows = false;
            this.dgvPayment.AllowUserToResizeColumns = false;
            this.dgvPayment.AllowUserToResizeRows = false;
            this.dgvPayment.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(222)))), ((int)(((byte)(175)))));
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(164)))), ((int)(((byte)(38)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPayment.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvPayment.ColumnHeadersHeight = 43;
            this.dgvPayment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPayment.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PayoffName,
            this.ItemNum,
            this.GoodsPrice});
            this.dgvPayment.EnableHeadersVisualStyles = false;
            this.dgvPayment.Location = new System.Drawing.Point(0, 189);
            this.dgvPayment.Name = "dgvPayment";
            this.dgvPayment.ReadOnly = true;
            this.dgvPayment.RowHeadersVisible = false;
            this.dgvPayment.RowTemplate.Height = 23;
            this.dgvPayment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPayment.Size = new System.Drawing.Size(482, 362);
            this.dgvPayment.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(43)))), ((int)(((byte)(59)))));
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(626, 560);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 59);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCheckOut
            // 
            this.btnCheckOut.BackColor = System.Drawing.Color.Teal;
            this.btnCheckOut.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCheckOut.ForeColor = System.Drawing.Color.White;
            this.btnCheckOut.Location = new System.Drawing.Point(793, 560);
            this.btnCheckOut.Name = "btnCheckOut";
            this.btnCheckOut.Size = new System.Drawing.Size(110, 59);
            this.btnCheckOut.TabIndex = 6;
            this.btnCheckOut.Text = "付款";
            this.btnCheckOut.UseVisualStyleBackColor = false;
            this.btnCheckOut.Click += new System.EventHandler(this.btnCheckOut_Click);
            // 
            // PayoffName
            // 
            this.PayoffName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PayoffName.DefaultCellStyle = dataGridViewCellStyle6;
            this.PayoffName.HeaderText = "付款方式";
            this.PayoffName.Name = "PayoffName";
            this.PayoffName.ReadOnly = true;
            this.PayoffName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ItemNum
            // 
            dataGridViewCellStyle7.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.NullValue = null;
            dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.ItemNum.DefaultCellStyle = dataGridViewCellStyle7;
            this.ItemNum.HeaderText = "数量";
            this.ItemNum.Name = "ItemNum";
            this.ItemNum.ReadOnly = true;
            this.ItemNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ItemNum.Width = 76;
            // 
            // GoodsPrice
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.Format = "N2";
            dataGridViewCellStyle8.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.GoodsPrice.DefaultCellStyle = dataGridViewCellStyle8;
            this.GoodsPrice.HeaderText = "付款金额";
            this.GoodsPrice.Name = "GoodsPrice";
            this.GoodsPrice.ReadOnly = true;
            this.GoodsPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.GoodsPrice.Width = 120;
            // 
            // FormPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 628);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCheckOut);
            this.Controls.Add(this.dgvPayment);
            this.Controls.Add(this.pnlBasicInfo);
            this.Controls.Add(this.pnlPayTypeNum);
            this.Controls.Add(this.pnlNumericPad);
            this.Controls.Add(this.pnlPayoffWay);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPayment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "账单结算";
            this.Load += new System.EventHandler(this.FormPayment_Load);
            this.pnlPayoffWay.ResumeLayout(false);
            this.pnlNumericPad.ResumeLayout(false);
            this.pnlPayTypeNum.ResumeLayout(false);
            this.pnlPayTypeNum.PerformLayout();
            this.pnlBasicInfo.ResumeLayout(false);
            this.pnlBasicInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayment)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlPayoffWay;
        private CustomControl.CrystalButton btnPageDown;
        private CustomControl.CrystalButton btnPageUp;
        private System.Windows.Forms.Panel pnlNumericPad;
        private CustomControl.CrystalButton btnNumeric8;
        private CustomControl.CrystalButton btnNumeric5;
        private CustomControl.CrystalButton btnDot;
        private CustomControl.CrystalButton btnNumeric2;
        private CustomControl.CrystalButton btnNumeric9;
        private CustomControl.CrystalButton btnNumeric6;
        private CustomControl.CrystalButton btnClear;
        private CustomControl.CrystalButton btnNumeric3;
        private CustomControl.CrystalButton btnNumeric7;
        private CustomControl.CrystalButton btnZero;
        private CustomControl.CrystalButton btnNumeric4;
        private CustomControl.CrystalButton btnNumeric1;
        private CustomControl.CrystalButton btnReal;
        private CustomControl.CrystalButton btnAdd5;
        private CustomControl.CrystalButton btnAdd10;
        private CustomControl.CrystalButton btnAdd20;
        private CustomControl.CrystalButton btnAdd50;
        private CustomControl.CrystalButton btnAdd100;
        private CustomControl.CrystalButton btnAdd200;
        private CustomControl.CrystalButton btnAdd500;
        private System.Windows.Forms.Panel pnlPayTypeNum;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label lbMulti;
        private System.Windows.Forms.TextBox txtPayoff;
        private System.Windows.Forms.Panel pnlBasicInfo;
        private System.Windows.Forms.Label lbReceMoney;
        private System.Windows.Forms.Label lbDiscount;
        private System.Windows.Forms.Label lbTotalPrice;
        private System.Windows.Forms.Label lbPaidInMoney;
        private System.Windows.Forms.Label lbUnpaidAmount;
        private System.Windows.Forms.DataGridView dgvPayment;
        private CustomControl.CrystalButton btnCancel;
        private CustomControl.CrystalButton btnCheckOut;
        private System.Windows.Forms.Label lbNeedChangePay;
        private System.Windows.Forms.DataGridViewTextBoxColumn PayoffName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsPrice;

    }
}