namespace Top4ever.Pos.Feature
{
    partial class FormRightsCode
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
            this.lbMessage = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lbUserName = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lbPassword = new System.Windows.Forms.Label();
            this.btnCancel = new Top4ever.CustomControl.CrystalButton();
            this.btnZero = new Top4ever.CustomControl.CrystalButton();
            this.btnNine = new Top4ever.CustomControl.CrystalButton();
            this.btnEight = new Top4ever.CustomControl.CrystalButton();
            this.btnPunchOut = new Top4ever.CustomControl.CrystalButton();
            this.btnConfirm = new Top4ever.CustomControl.CrystalButton();
            this.btnPunchIn = new Top4ever.CustomControl.CrystalButton();
            this.btnSeven = new Top4ever.CustomControl.CrystalButton();
            this.btnSix = new Top4ever.CustomControl.CrystalButton();
            this.btnFive = new Top4ever.CustomControl.CrystalButton();
            this.btnFour = new Top4ever.CustomControl.CrystalButton();
            this.btnThree = new Top4ever.CustomControl.CrystalButton();
            this.btnTwo = new Top4ever.CustomControl.CrystalButton();
            this.btnOne = new Top4ever.CustomControl.CrystalButton();
            this.btnBackSpace = new Top4ever.CustomControl.CrystalButton();
            this.SuspendLayout();
            // 
            // lbMessage
            // 
            this.lbMessage.AutoSize = true;
            this.lbMessage.Location = new System.Drawing.Point(97, 12);
            this.lbMessage.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbMessage.Name = "lbMessage";
            this.lbMessage.Size = new System.Drawing.Size(90, 21);
            this.lbMessage.TabIndex = 0;
            this.lbMessage.Text = "ÇëÊäÈëÐÅÏ¢";
            // 
            // txtUserName
            // 
            this.txtUserName.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUserName.Location = new System.Drawing.Point(103, 47);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(162, 27);
            this.txtUserName.TabIndex = 0;
            this.txtUserName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtUserName_MouseDown);
            // 
            // lbUserName
            // 
            this.lbUserName.AutoSize = true;
            this.lbUserName.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbUserName.Location = new System.Drawing.Point(34, 49);
            this.lbUserName.Name = "lbUserName";
            this.lbUserName.Size = new System.Drawing.Size(69, 20);
            this.lbUserName.TabIndex = 1;
            this.lbUserName.Text = "ÓÃ»§Ãû£º";
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPassword.Location = new System.Drawing.Point(103, 80);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(162, 27);
            this.txtPassword.TabIndex = 32;
            this.txtPassword.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtPassword_MouseDown);
            // 
            // lbPassword
            // 
            this.lbPassword.AutoSize = true;
            this.lbPassword.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbPassword.Location = new System.Drawing.Point(41, 82);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(62, 20);
            this.lbPassword.TabIndex = 1;
            this.lbPassword.Text = "ÃÜ  Âë£º";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Red;
            this.btnCancel.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCancel.Location = new System.Drawing.Point(29, 285);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 49);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "È¡Ïû";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnZero
            // 
            this.btnZero.BackColor = System.Drawing.Color.Blue;
            this.btnZero.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnZero.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnZero.Location = new System.Drawing.Point(110, 285);
            this.btnZero.Name = "btnZero";
            this.btnZero.Size = new System.Drawing.Size(75, 49);
            this.btnZero.TabIndex = 1;
            this.btnZero.Text = "0";
            this.btnZero.UseVisualStyleBackColor = false;
            this.btnZero.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnNine
            // 
            this.btnNine.BackColor = System.Drawing.Color.Blue;
            this.btnNine.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnNine.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnNine.Location = new System.Drawing.Point(191, 230);
            this.btnNine.Name = "btnNine";
            this.btnNine.Size = new System.Drawing.Size(75, 49);
            this.btnNine.TabIndex = 1;
            this.btnNine.Text = "9";
            this.btnNine.UseVisualStyleBackColor = false;
            this.btnNine.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnEight
            // 
            this.btnEight.BackColor = System.Drawing.Color.Blue;
            this.btnEight.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEight.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnEight.Location = new System.Drawing.Point(110, 230);
            this.btnEight.Name = "btnEight";
            this.btnEight.Size = new System.Drawing.Size(75, 49);
            this.btnEight.TabIndex = 1;
            this.btnEight.Text = "8";
            this.btnEight.UseVisualStyleBackColor = false;
            this.btnEight.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnPunchOut
            // 
            this.btnPunchOut.BackColor = System.Drawing.Color.Teal;
            this.btnPunchOut.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPunchOut.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnPunchOut.Location = new System.Drawing.Point(151, 340);
            this.btnPunchOut.Name = "btnPunchOut";
            this.btnPunchOut.Size = new System.Drawing.Size(115, 49);
            this.btnPunchOut.TabIndex = 1;
            this.btnPunchOut.Text = "ÏÂ°à´ò¿¨";
            this.btnPunchOut.UseVisualStyleBackColor = false;
            this.btnPunchOut.Click += new System.EventHandler(this.btnPunchOut_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Green;
            this.btnConfirm.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnConfirm.Location = new System.Drawing.Point(29, 395);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(237, 49);
            this.btnConfirm.TabIndex = 1;
            this.btnConfirm.Text = "È·¶¨";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnPunchIn
            // 
            this.btnPunchIn.BackColor = System.Drawing.Color.Teal;
            this.btnPunchIn.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPunchIn.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnPunchIn.Location = new System.Drawing.Point(29, 340);
            this.btnPunchIn.Name = "btnPunchIn";
            this.btnPunchIn.Size = new System.Drawing.Size(115, 49);
            this.btnPunchIn.TabIndex = 1;
            this.btnPunchIn.Text = "ÉÏ°à´ò¿¨";
            this.btnPunchIn.UseVisualStyleBackColor = false;
            this.btnPunchIn.Click += new System.EventHandler(this.btnPunchIn_Click);
            // 
            // btnSeven
            // 
            this.btnSeven.BackColor = System.Drawing.Color.Blue;
            this.btnSeven.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSeven.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSeven.Location = new System.Drawing.Point(29, 230);
            this.btnSeven.Name = "btnSeven";
            this.btnSeven.Size = new System.Drawing.Size(75, 49);
            this.btnSeven.TabIndex = 1;
            this.btnSeven.Text = "7";
            this.btnSeven.UseVisualStyleBackColor = false;
            this.btnSeven.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnSix
            // 
            this.btnSix.BackColor = System.Drawing.Color.Blue;
            this.btnSix.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSix.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSix.Location = new System.Drawing.Point(190, 176);
            this.btnSix.Name = "btnSix";
            this.btnSix.Size = new System.Drawing.Size(75, 49);
            this.btnSix.TabIndex = 1;
            this.btnSix.Text = "6";
            this.btnSix.UseVisualStyleBackColor = false;
            this.btnSix.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnFive
            // 
            this.btnFive.BackColor = System.Drawing.Color.Blue;
            this.btnFive.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFive.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFive.Location = new System.Drawing.Point(109, 176);
            this.btnFive.Name = "btnFive";
            this.btnFive.Size = new System.Drawing.Size(75, 49);
            this.btnFive.TabIndex = 1;
            this.btnFive.Text = "5";
            this.btnFive.UseVisualStyleBackColor = false;
            this.btnFive.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnFour
            // 
            this.btnFour.BackColor = System.Drawing.Color.Blue;
            this.btnFour.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFour.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFour.Location = new System.Drawing.Point(29, 176);
            this.btnFour.Name = "btnFour";
            this.btnFour.Size = new System.Drawing.Size(75, 49);
            this.btnFour.TabIndex = 1;
            this.btnFour.Text = "4";
            this.btnFour.UseVisualStyleBackColor = false;
            this.btnFour.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnThree
            // 
            this.btnThree.BackColor = System.Drawing.Color.Blue;
            this.btnThree.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnThree.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnThree.Location = new System.Drawing.Point(190, 121);
            this.btnThree.Name = "btnThree";
            this.btnThree.Size = new System.Drawing.Size(75, 49);
            this.btnThree.TabIndex = 1;
            this.btnThree.Text = "3";
            this.btnThree.UseVisualStyleBackColor = false;
            this.btnThree.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnTwo
            // 
            this.btnTwo.BackColor = System.Drawing.Color.Blue;
            this.btnTwo.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTwo.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnTwo.Location = new System.Drawing.Point(110, 121);
            this.btnTwo.Name = "btnTwo";
            this.btnTwo.Size = new System.Drawing.Size(75, 49);
            this.btnTwo.TabIndex = 1;
            this.btnTwo.Text = "2";
            this.btnTwo.UseVisualStyleBackColor = false;
            this.btnTwo.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnOne
            // 
            this.btnOne.BackColor = System.Drawing.Color.Blue;
            this.btnOne.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOne.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnOne.Location = new System.Drawing.Point(29, 121);
            this.btnOne.Name = "btnOne";
            this.btnOne.Size = new System.Drawing.Size(75, 49);
            this.btnOne.TabIndex = 1;
            this.btnOne.Text = "1";
            this.btnOne.UseVisualStyleBackColor = false;
            this.btnOne.Click += new System.EventHandler(this.btnNumeric_Click);
            // 
            // btnBackSpace
            // 
            this.btnBackSpace.BackColor = System.Drawing.Color.Black;
            this.btnBackSpace.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBackSpace.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnBackSpace.Location = new System.Drawing.Point(191, 285);
            this.btnBackSpace.Name = "btnBackSpace";
            this.btnBackSpace.Size = new System.Drawing.Size(74, 49);
            this.btnBackSpace.TabIndex = 1;
            this.btnBackSpace.Text = "ÍË¸ñ";
            this.btnBackSpace.UseVisualStyleBackColor = false;
            this.btnBackSpace.Click += new System.EventHandler(this.btnBackSpace_Click);
            // 
            // FrmPunchCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 461);
            this.Controls.Add(this.lbPassword);
            this.Controls.Add(this.lbUserName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnZero);
            this.Controls.Add(this.btnNine);
            this.Controls.Add(this.btnEight);
            this.Controls.Add(this.btnPunchOut);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnPunchIn);
            this.Controls.Add(this.btnSeven);
            this.Controls.Add(this.btnSix);
            this.Controls.Add(this.btnFive);
            this.Controls.Add(this.btnFour);
            this.Controls.Add(this.btnThree);
            this.Controls.Add(this.btnTwo);
            this.Controls.Add(this.btnOne);
            this.Controls.Add(this.btnBackSpace);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.lbMessage);
            this.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPunchCard";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FrmPunchCard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbMessage;
        private CustomControl.CrystalButton btnCancel;
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
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lbUserName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lbPassword;
        private CustomControl.CrystalButton btnPunchIn;
        private CustomControl.CrystalButton btnPunchOut;
        private CustomControl.CrystalButton btnConfirm;
    }
}