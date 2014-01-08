namespace Top4ever.Pos.Feature
{
    partial class FormCustomQty
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
            this.btnConfirm = new Top4ever.CustomControl.CrystalButton();
            this.btnCancel = new Top4ever.CustomControl.CrystalButton();
            this.btnDot = new Top4ever.CustomControl.CrystalButton();
            this.btnZero = new Top4ever.CustomControl.CrystalButton();
            this.btnNine = new Top4ever.CustomControl.CrystalButton();
            this.btnEight = new Top4ever.CustomControl.CrystalButton();
            this.btnSeven = new Top4ever.CustomControl.CrystalButton();
            this.btnSix = new Top4ever.CustomControl.CrystalButton();
            this.btnFive = new Top4ever.CustomControl.CrystalButton();
            this.btnFour = new Top4ever.CustomControl.CrystalButton();
            this.btnThree = new Top4ever.CustomControl.CrystalButton();
            this.btnTwo = new Top4ever.CustomControl.CrystalButton();
            this.btnOne = new Top4ever.CustomControl.CrystalButton();
            this.btnBackSpace = new Top4ever.CustomControl.CrystalButton();
            this.txtCustomPrice = new System.Windows.Forms.TextBox();
            this.lbInput = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCustomQty = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Teal;
            this.btnConfirm.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Location = new System.Drawing.Point(161, 350);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(111, 56);
            this.btnConfirm.TabIndex = 0;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(43)))), ((int)(((byte)(59)))));
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(36, 350);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(111, 56);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDot
            // 
            this.btnDot.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnDot.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDot.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDot.Location = new System.Drawing.Point(116, 279);
            this.btnDot.Name = "btnDot";
            this.btnDot.Size = new System.Drawing.Size(75, 53);
            this.btnDot.TabIndex = 28;
            this.btnDot.Text = ".";
            this.btnDot.UseVisualStyleBackColor = false;
            this.btnDot.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnZero
            // 
            this.btnZero.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnZero.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnZero.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnZero.Location = new System.Drawing.Point(36, 279);
            this.btnZero.Name = "btnZero";
            this.btnZero.Size = new System.Drawing.Size(75, 53);
            this.btnZero.TabIndex = 27;
            this.btnZero.Text = "0";
            this.btnZero.UseVisualStyleBackColor = false;
            this.btnZero.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnNine
            // 
            this.btnNine.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnNine.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btnNine.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnNine.Location = new System.Drawing.Point(197, 220);
            this.btnNine.Name = "btnNine";
            this.btnNine.Size = new System.Drawing.Size(75, 53);
            this.btnNine.TabIndex = 32;
            this.btnNine.Text = "9";
            this.btnNine.UseVisualStyleBackColor = false;
            this.btnNine.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnEight
            // 
            this.btnEight.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnEight.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btnEight.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnEight.Location = new System.Drawing.Point(116, 220);
            this.btnEight.Name = "btnEight";
            this.btnEight.Size = new System.Drawing.Size(75, 53);
            this.btnEight.TabIndex = 31;
            this.btnEight.Text = "8";
            this.btnEight.UseVisualStyleBackColor = false;
            this.btnEight.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnSeven
            // 
            this.btnSeven.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnSeven.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btnSeven.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSeven.Location = new System.Drawing.Point(36, 220);
            this.btnSeven.Name = "btnSeven";
            this.btnSeven.Size = new System.Drawing.Size(75, 53);
            this.btnSeven.TabIndex = 30;
            this.btnSeven.Text = "7";
            this.btnSeven.UseVisualStyleBackColor = false;
            this.btnSeven.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnSix
            // 
            this.btnSix.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnSix.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btnSix.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSix.Location = new System.Drawing.Point(197, 161);
            this.btnSix.Name = "btnSix";
            this.btnSix.Size = new System.Drawing.Size(75, 53);
            this.btnSix.TabIndex = 29;
            this.btnSix.Text = "6";
            this.btnSix.UseVisualStyleBackColor = false;
            this.btnSix.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnFive
            // 
            this.btnFive.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnFive.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btnFive.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFive.Location = new System.Drawing.Point(116, 161);
            this.btnFive.Name = "btnFive";
            this.btnFive.Size = new System.Drawing.Size(75, 53);
            this.btnFive.TabIndex = 22;
            this.btnFive.Text = "5";
            this.btnFive.UseVisualStyleBackColor = false;
            this.btnFive.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnFour
            // 
            this.btnFour.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnFour.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btnFour.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFour.Location = new System.Drawing.Point(36, 161);
            this.btnFour.Name = "btnFour";
            this.btnFour.Size = new System.Drawing.Size(75, 53);
            this.btnFour.TabIndex = 21;
            this.btnFour.Text = "4";
            this.btnFour.UseVisualStyleBackColor = false;
            this.btnFour.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnThree
            // 
            this.btnThree.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnThree.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btnThree.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnThree.Location = new System.Drawing.Point(197, 102);
            this.btnThree.Name = "btnThree";
            this.btnThree.Size = new System.Drawing.Size(75, 53);
            this.btnThree.TabIndex = 26;
            this.btnThree.Text = "3";
            this.btnThree.UseVisualStyleBackColor = false;
            this.btnThree.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnTwo
            // 
            this.btnTwo.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnTwo.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btnTwo.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnTwo.Location = new System.Drawing.Point(116, 102);
            this.btnTwo.Name = "btnTwo";
            this.btnTwo.Size = new System.Drawing.Size(75, 53);
            this.btnTwo.TabIndex = 25;
            this.btnTwo.Text = "2";
            this.btnTwo.UseVisualStyleBackColor = false;
            this.btnTwo.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnOne
            // 
            this.btnOne.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnOne.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btnOne.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnOne.Location = new System.Drawing.Point(36, 102);
            this.btnOne.Name = "btnOne";
            this.btnOne.Size = new System.Drawing.Size(75, 53);
            this.btnOne.TabIndex = 24;
            this.btnOne.Text = "1";
            this.btnOne.UseVisualStyleBackColor = false;
            this.btnOne.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnBackSpace
            // 
            this.btnBackSpace.BackColor = System.Drawing.Color.OrangeRed;
            this.btnBackSpace.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackSpace.ForeColor = System.Drawing.Color.White;
            this.btnBackSpace.Location = new System.Drawing.Point(197, 279);
            this.btnBackSpace.Name = "btnBackSpace";
            this.btnBackSpace.Size = new System.Drawing.Size(75, 53);
            this.btnBackSpace.TabIndex = 2;
            this.btnBackSpace.Text = "退格";
            this.btnBackSpace.UseVisualStyleBackColor = false;
            this.btnBackSpace.Click += new System.EventHandler(this.btnBackSpace_Click);
            // 
            // txtCustomPrice
            // 
            this.txtCustomPrice.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCustomPrice.Location = new System.Drawing.Point(90, 19);
            this.txtCustomPrice.Name = "txtCustomPrice";
            this.txtCustomPrice.Size = new System.Drawing.Size(182, 29);
            this.txtCustomPrice.TabIndex = 1;
            this.txtCustomPrice.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtCustomPrice_MouseDown);
            // 
            // lbInput
            // 
            this.lbInput.AutoSize = true;
            this.lbInput.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbInput.Location = new System.Drawing.Point(32, 22);
            this.lbInput.Name = "lbInput";
            this.lbInput.Size = new System.Drawing.Size(58, 21);
            this.lbInput.TabIndex = 1;
            this.lbInput.Text = "价格：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(32, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "重量：";
            // 
            // txtCustomQty
            // 
            this.txtCustomQty.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCustomQty.Location = new System.Drawing.Point(90, 61);
            this.txtCustomQty.Name = "txtCustomQty";
            this.txtCustomQty.Size = new System.Drawing.Size(182, 29);
            this.txtCustomQty.TabIndex = 1;
            this.txtCustomQty.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtCustomQty_MouseDown);
            // 
            // FormCustomQty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 429);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDot);
            this.Controls.Add(this.btnZero);
            this.Controls.Add(this.btnNine);
            this.Controls.Add(this.btnEight);
            this.Controls.Add(this.btnSeven);
            this.Controls.Add(this.btnSix);
            this.Controls.Add(this.btnFive);
            this.Controls.Add(this.btnFour);
            this.Controls.Add(this.btnThree);
            this.Controls.Add(this.btnTwo);
            this.Controls.Add(this.btnOne);
            this.Controls.Add(this.btnBackSpace);
            this.Controls.Add(this.txtCustomQty);
            this.Controls.Add(this.txtCustomPrice);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCustomQty";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormCustomQty";
            this.Load += new System.EventHandler(this.FormCustomQty_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomControl.CrystalButton btnConfirm;
        private CustomControl.CrystalButton btnCancel;
        private CustomControl.CrystalButton btnDot;
        private CustomControl.CrystalButton btnZero;
        private CustomControl.CrystalButton btnNine;
        private CustomControl.CrystalButton btnEight;
        private CustomControl.CrystalButton btnSeven;
        private CustomControl.CrystalButton btnSix;
        private CustomControl.CrystalButton btnFive;
        private CustomControl.CrystalButton btnFour;
        private CustomControl.CrystalButton btnThree;
        private CustomControl.CrystalButton btnTwo;
        private CustomControl.CrystalButton btnOne;
        private CustomControl.CrystalButton btnBackSpace;
        private System.Windows.Forms.TextBox txtCustomPrice;
        private System.Windows.Forms.Label lbInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCustomQty;
    }
}