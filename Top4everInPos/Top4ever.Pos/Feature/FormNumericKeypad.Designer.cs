namespace Top4ever.Pos.Feature
{
    partial class FormNumericKeypad
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
            this.lbInput = new System.Windows.Forms.Label();
            this.txtNumeric = new System.Windows.Forms.TextBox();
            this.btnConfirm = new Top4ever.CustomControl.CrystalButton();
            this.btnCancel = new Top4ever.CustomControl.CrystalButton();
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
            this.btnDot = new Top4ever.CustomControl.CrystalButton();
            this.SuspendLayout();
            // 
            // lbInput
            // 
            this.lbInput.AutoSize = true;
            this.lbInput.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbInput.Location = new System.Drawing.Point(20, 14);
            this.lbInput.Name = "lbInput";
            this.lbInput.Size = new System.Drawing.Size(90, 22);
            this.lbInput.TabIndex = 1;
            this.lbInput.Text = "请输入数字";
            // 
            // txtNumeric
            // 
            this.txtNumeric.Font = new System.Drawing.Font("Microsoft YaHei", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtNumeric.Location = new System.Drawing.Point(24, 46);
            this.txtNumeric.Name = "txtNumeric";
            this.txtNumeric.Size = new System.Drawing.Size(171, 39);
            this.txtNumeric.TabIndex = 2;
            this.txtNumeric.Text = "0";
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(148)))), ((int)(((byte)(91)))));
            this.btnConfirm.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnConfirm.Location = new System.Drawing.Point(185, 264);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 49);
            this.btnConfirm.TabIndex = 31;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(43)))), ((int)(((byte)(59)))));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCancel.Location = new System.Drawing.Point(24, 264);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 49);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnZero
            // 
            this.btnZero.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnZero.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnZero.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnZero.Location = new System.Drawing.Point(105, 264);
            this.btnZero.Name = "btnZero";
            this.btnZero.Size = new System.Drawing.Size(75, 49);
            this.btnZero.TabIndex = 29;
            this.btnZero.Text = "0";
            this.btnZero.UseVisualStyleBackColor = false;
            this.btnZero.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnNine
            // 
            this.btnNine.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnNine.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.btnNine.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnNine.Location = new System.Drawing.Point(185, 207);
            this.btnNine.Name = "btnNine";
            this.btnNine.Size = new System.Drawing.Size(75, 49);
            this.btnNine.TabIndex = 28;
            this.btnNine.Text = "9";
            this.btnNine.UseVisualStyleBackColor = false;
            this.btnNine.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnEight
            // 
            this.btnEight.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnEight.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.btnEight.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnEight.Location = new System.Drawing.Point(105, 207);
            this.btnEight.Name = "btnEight";
            this.btnEight.Size = new System.Drawing.Size(75, 49);
            this.btnEight.TabIndex = 27;
            this.btnEight.Text = "8";
            this.btnEight.UseVisualStyleBackColor = false;
            this.btnEight.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnSeven
            // 
            this.btnSeven.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnSeven.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.btnSeven.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSeven.Location = new System.Drawing.Point(24, 207);
            this.btnSeven.Name = "btnSeven";
            this.btnSeven.Size = new System.Drawing.Size(75, 49);
            this.btnSeven.TabIndex = 26;
            this.btnSeven.Text = "7";
            this.btnSeven.UseVisualStyleBackColor = false;
            this.btnSeven.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnSix
            // 
            this.btnSix.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnSix.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.btnSix.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSix.Location = new System.Drawing.Point(185, 151);
            this.btnSix.Name = "btnSix";
            this.btnSix.Size = new System.Drawing.Size(75, 49);
            this.btnSix.TabIndex = 25;
            this.btnSix.Text = "6";
            this.btnSix.UseVisualStyleBackColor = false;
            this.btnSix.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnFive
            // 
            this.btnFive.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnFive.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.btnFive.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFive.Location = new System.Drawing.Point(104, 151);
            this.btnFive.Name = "btnFive";
            this.btnFive.Size = new System.Drawing.Size(75, 49);
            this.btnFive.TabIndex = 24;
            this.btnFive.Text = "5";
            this.btnFive.UseVisualStyleBackColor = false;
            this.btnFive.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnFour
            // 
            this.btnFour.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnFour.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.btnFour.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFour.Location = new System.Drawing.Point(24, 151);
            this.btnFour.Name = "btnFour";
            this.btnFour.Size = new System.Drawing.Size(75, 49);
            this.btnFour.TabIndex = 23;
            this.btnFour.Text = "4";
            this.btnFour.UseVisualStyleBackColor = false;
            this.btnFour.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnThree
            // 
            this.btnThree.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnThree.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.btnThree.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnThree.Location = new System.Drawing.Point(185, 94);
            this.btnThree.Name = "btnThree";
            this.btnThree.Size = new System.Drawing.Size(75, 49);
            this.btnThree.TabIndex = 22;
            this.btnThree.Text = "3";
            this.btnThree.UseVisualStyleBackColor = false;
            this.btnThree.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnTwo
            // 
            this.btnTwo.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnTwo.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.btnTwo.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnTwo.Location = new System.Drawing.Point(105, 94);
            this.btnTwo.Name = "btnTwo";
            this.btnTwo.Size = new System.Drawing.Size(75, 49);
            this.btnTwo.TabIndex = 21;
            this.btnTwo.Text = "2";
            this.btnTwo.UseVisualStyleBackColor = false;
            this.btnTwo.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnOne
            // 
            this.btnOne.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnOne.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.btnOne.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnOne.Location = new System.Drawing.Point(24, 94);
            this.btnOne.Name = "btnOne";
            this.btnOne.Size = new System.Drawing.Size(75, 49);
            this.btnOne.TabIndex = 20;
            this.btnOne.Text = "1";
            this.btnOne.UseVisualStyleBackColor = false;
            this.btnOne.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnBackSpace
            // 
            this.btnBackSpace.BackColor = System.Drawing.Color.OrangeRed;
            this.btnBackSpace.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackSpace.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnBackSpace.Location = new System.Drawing.Point(200, 43);
            this.btnBackSpace.Name = "btnBackSpace";
            this.btnBackSpace.Size = new System.Drawing.Size(60, 45);
            this.btnBackSpace.TabIndex = 19;
            this.btnBackSpace.Text = "<-----";
            this.btnBackSpace.UseVisualStyleBackColor = false;
            this.btnBackSpace.Click += new System.EventHandler(this.btnBackSpace_Click);
            // 
            // btnDot
            // 
            this.btnDot.BackColor = System.Drawing.SystemColors.ControlText;
            this.btnDot.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDot.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDot.Location = new System.Drawing.Point(24, 321);
            this.btnDot.Name = "btnDot";
            this.btnDot.Size = new System.Drawing.Size(236, 45);
            this.btnDot.TabIndex = 29;
            this.btnDot.Text = ".";
            this.btnDot.UseVisualStyleBackColor = false;
            this.btnDot.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // FormNumericKeypad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(285, 393);
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
            this.Controls.Add(this.txtNumeric);
            this.Controls.Add(this.lbInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNumericKeypad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmNumericKeypad";
            this.Load += new System.EventHandler(this.FormNumericKeypad_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNumeric;
        private CustomControl.CrystalButton btnBackSpace;
        private CustomControl.CrystalButton btnOne;
        private CustomControl.CrystalButton btnTwo;
        private CustomControl.CrystalButton btnThree;
        private CustomControl.CrystalButton btnFour;
        private CustomControl.CrystalButton btnFive;
        private CustomControl.CrystalButton btnSix;
        private CustomControl.CrystalButton btnSeven;
        private CustomControl.CrystalButton btnEight;
        private CustomControl.CrystalButton btnNine;
        private CustomControl.CrystalButton btnZero;
        private CustomControl.CrystalButton btnCancel;
        private CustomControl.CrystalButton btnConfirm;
        private System.Windows.Forms.Label lbInput;
        private CustomControl.CrystalButton btnDot;
    }
}