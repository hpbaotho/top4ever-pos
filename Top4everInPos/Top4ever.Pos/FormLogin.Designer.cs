using Top4ever.CustomControl;

namespace VechsoftPos
{
    partial class FormLogin
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
            this.pnlSystemLogin = new Top4ever.CustomControl.RoundPanel(this.components);
            this.btnSwipeCard = new Top4ever.CustomControl.CrystalButton();
            this.btnThree = new Top4ever.CustomControl.CrystalButton();
            this.btnTwo = new Top4ever.CustomControl.CrystalButton();
            this.btnSix = new Top4ever.CustomControl.CrystalButton();
            this.btnFive = new Top4ever.CustomControl.CrystalButton();
            this.btnNine = new Top4ever.CustomControl.CrystalButton();
            this.btnEight = new Top4ever.CustomControl.CrystalButton();
            this.btnExit = new Top4ever.CustomControl.CrystalButton();
            this.btnLogin = new Top4ever.CustomControl.CrystalButton();
            this.btnBackspace = new Top4ever.CustomControl.CrystalButton();
            this.btnZero = new Top4ever.CustomControl.CrystalButton();
            this.btnSeven = new Top4ever.CustomControl.CrystalButton();
            this.btnFour = new Top4ever.CustomControl.CrystalButton();
            this.btnOne = new Top4ever.CustomControl.CrystalButton();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lbPassword = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.lbSystemName = new System.Windows.Forms.Label();
            this.pnlSystemLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSystemLogin
            // 
            this.pnlSystemLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(199)))), ((int)(((byte)(172)))));
            this.pnlSystemLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlSystemLogin.Controls.Add(this.btnSwipeCard);
            this.pnlSystemLogin.Controls.Add(this.btnThree);
            this.pnlSystemLogin.Controls.Add(this.btnTwo);
            this.pnlSystemLogin.Controls.Add(this.btnSix);
            this.pnlSystemLogin.Controls.Add(this.btnFive);
            this.pnlSystemLogin.Controls.Add(this.btnNine);
            this.pnlSystemLogin.Controls.Add(this.btnEight);
            this.pnlSystemLogin.Controls.Add(this.btnExit);
            this.pnlSystemLogin.Controls.Add(this.btnLogin);
            this.pnlSystemLogin.Controls.Add(this.btnBackspace);
            this.pnlSystemLogin.Controls.Add(this.btnZero);
            this.pnlSystemLogin.Controls.Add(this.btnSeven);
            this.pnlSystemLogin.Controls.Add(this.btnFour);
            this.pnlSystemLogin.Controls.Add(this.btnOne);
            this.pnlSystemLogin.Controls.Add(this.txtName);
            this.pnlSystemLogin.Controls.Add(this.txtPassword);
            this.pnlSystemLogin.Controls.Add(this.lbPassword);
            this.pnlSystemLogin.Controls.Add(this.lbName);
            this.pnlSystemLogin.Controls.Add(this.lbSystemName);
            this.pnlSystemLogin.Location = new System.Drawing.Point(194, 145);
            this.pnlSystemLogin.Margin = new System.Windows.Forms.Padding(0);
            this.pnlSystemLogin.MatrixRound = 15;
            this.pnlSystemLogin.Name = "pnlSystemLogin";
            this.pnlSystemLogin.Size = new System.Drawing.Size(654, 518);
            this.pnlSystemLogin.TabIndex = 1;
            // 
            // btnSwipeCard
            // 
            this.btnSwipeCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(20)))));
            this.btnSwipeCard.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.btnSwipeCard.ForeColor = System.Drawing.Color.White;
            this.btnSwipeCard.Location = new System.Drawing.Point(403, 376);
            this.btnSwipeCard.Name = "btnSwipeCard";
            this.btnSwipeCard.Size = new System.Drawing.Size(151, 62);
            this.btnSwipeCard.TabIndex = 1;
            this.btnSwipeCard.Text = "刷卡登录";
            this.btnSwipeCard.UseVisualStyleBackColor = false;
            this.btnSwipeCard.Click += new System.EventHandler(this.btnSwipeCard_Click);
            // 
            // btnThree
            // 
            this.btnThree.BackColor = System.Drawing.Color.Black;
            this.btnThree.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.btnThree.ForeColor = System.Drawing.Color.White;
            this.btnThree.Location = new System.Drawing.Point(292, 172);
            this.btnThree.Name = "btnThree";
            this.btnThree.Size = new System.Drawing.Size(98, 62);
            this.btnThree.TabIndex = 1;
            this.btnThree.Text = "3";
            this.btnThree.UseVisualStyleBackColor = false;
            this.btnThree.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btnTwo
            // 
            this.btnTwo.BackColor = System.Drawing.Color.Black;
            this.btnTwo.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.btnTwo.ForeColor = System.Drawing.Color.White;
            this.btnTwo.Location = new System.Drawing.Point(189, 172);
            this.btnTwo.Name = "btnTwo";
            this.btnTwo.Size = new System.Drawing.Size(98, 62);
            this.btnTwo.TabIndex = 1;
            this.btnTwo.Text = "2";
            this.btnTwo.UseVisualStyleBackColor = false;
            this.btnTwo.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btnSix
            // 
            this.btnSix.BackColor = System.Drawing.Color.Black;
            this.btnSix.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.btnSix.ForeColor = System.Drawing.Color.White;
            this.btnSix.Location = new System.Drawing.Point(292, 240);
            this.btnSix.Name = "btnSix";
            this.btnSix.Size = new System.Drawing.Size(98, 62);
            this.btnSix.TabIndex = 1;
            this.btnSix.Text = "6";
            this.btnSix.UseVisualStyleBackColor = false;
            this.btnSix.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btnFive
            // 
            this.btnFive.BackColor = System.Drawing.Color.Black;
            this.btnFive.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.btnFive.ForeColor = System.Drawing.Color.White;
            this.btnFive.Location = new System.Drawing.Point(189, 240);
            this.btnFive.Name = "btnFive";
            this.btnFive.Size = new System.Drawing.Size(98, 62);
            this.btnFive.TabIndex = 1;
            this.btnFive.Text = "5";
            this.btnFive.UseVisualStyleBackColor = false;
            this.btnFive.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btnNine
            // 
            this.btnNine.BackColor = System.Drawing.Color.Black;
            this.btnNine.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.btnNine.ForeColor = System.Drawing.Color.White;
            this.btnNine.Location = new System.Drawing.Point(292, 308);
            this.btnNine.Name = "btnNine";
            this.btnNine.Size = new System.Drawing.Size(98, 62);
            this.btnNine.TabIndex = 1;
            this.btnNine.Text = "9";
            this.btnNine.UseVisualStyleBackColor = false;
            this.btnNine.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btnEight
            // 
            this.btnEight.BackColor = System.Drawing.Color.Black;
            this.btnEight.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.btnEight.ForeColor = System.Drawing.Color.White;
            this.btnEight.Location = new System.Drawing.Point(189, 308);
            this.btnEight.Name = "btnEight";
            this.btnEight.Size = new System.Drawing.Size(98, 62);
            this.btnEight.TabIndex = 1;
            this.btnEight.Text = "8";
            this.btnEight.UseVisualStyleBackColor = false;
            this.btnEight.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(43)))), ((int)(((byte)(59)))));
            this.btnExit.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(86, 376);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(98, 62);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "退出系统";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(113)))), ((int)(((byte)(202)))));
            this.btnLogin.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(403, 172);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(151, 198);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnBackspace
            // 
            this.btnBackspace.BackColor = System.Drawing.Color.Teal;
            this.btnBackspace.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBackspace.ForeColor = System.Drawing.Color.White;
            this.btnBackspace.Location = new System.Drawing.Point(292, 376);
            this.btnBackspace.Name = "btnBackspace";
            this.btnBackspace.Size = new System.Drawing.Size(98, 62);
            this.btnBackspace.TabIndex = 1;
            this.btnBackspace.Text = "退格";
            this.btnBackspace.UseVisualStyleBackColor = false;
            this.btnBackspace.Click += new System.EventHandler(this.btnBackspace_Click);
            // 
            // btnZero
            // 
            this.btnZero.BackColor = System.Drawing.Color.Black;
            this.btnZero.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.btnZero.ForeColor = System.Drawing.Color.White;
            this.btnZero.Location = new System.Drawing.Point(189, 376);
            this.btnZero.Name = "btnZero";
            this.btnZero.Size = new System.Drawing.Size(98, 62);
            this.btnZero.TabIndex = 1;
            this.btnZero.Text = "0";
            this.btnZero.UseVisualStyleBackColor = false;
            this.btnZero.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btnSeven
            // 
            this.btnSeven.BackColor = System.Drawing.Color.Black;
            this.btnSeven.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.btnSeven.ForeColor = System.Drawing.Color.White;
            this.btnSeven.Location = new System.Drawing.Point(86, 308);
            this.btnSeven.Name = "btnSeven";
            this.btnSeven.Size = new System.Drawing.Size(98, 62);
            this.btnSeven.TabIndex = 1;
            this.btnSeven.Text = "7";
            this.btnSeven.UseVisualStyleBackColor = false;
            this.btnSeven.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btnFour
            // 
            this.btnFour.BackColor = System.Drawing.Color.Black;
            this.btnFour.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.btnFour.ForeColor = System.Drawing.Color.White;
            this.btnFour.Location = new System.Drawing.Point(86, 240);
            this.btnFour.Name = "btnFour";
            this.btnFour.Size = new System.Drawing.Size(98, 62);
            this.btnFour.TabIndex = 1;
            this.btnFour.Text = "4";
            this.btnFour.UseVisualStyleBackColor = false;
            this.btnFour.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btnOne
            // 
            this.btnOne.BackColor = System.Drawing.Color.Black;
            this.btnOne.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.btnOne.ForeColor = System.Drawing.Color.White;
            this.btnOne.Location = new System.Drawing.Point(86, 172);
            this.btnOne.Name = "btnOne";
            this.btnOne.Size = new System.Drawing.Size(98, 62);
            this.btnOne.TabIndex = 1;
            this.btnOne.Text = "1";
            this.btnOne.UseVisualStyleBackColor = false;
            this.btnOne.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // txtName
            // 
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtName.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtName.Location = new System.Drawing.Point(162, 117);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(136, 26);
            this.txtName.TabIndex = 0;
            this.txtName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtName_MouseDown);
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPassword.Location = new System.Drawing.Point(418, 117);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(136, 26);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtPassword_MouseDown);
            // 
            // lbPassword
            // 
            this.lbPassword.AutoSize = true;
            this.lbPassword.BackColor = System.Drawing.Color.Transparent;
            this.lbPassword.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbPassword.ForeColor = System.Drawing.Color.White;
            this.lbPassword.Location = new System.Drawing.Point(354, 120);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(58, 21);
            this.lbPassword.TabIndex = 1;
            this.lbPassword.Text = "密码：";
            this.lbPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.BackColor = System.Drawing.Color.Transparent;
            this.lbName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbName.ForeColor = System.Drawing.Color.White;
            this.lbName.Location = new System.Drawing.Point(82, 120);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(74, 21);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "用户名：";
            this.lbName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbSystemName
            // 
            this.lbSystemName.AutoSize = true;
            this.lbSystemName.BackColor = System.Drawing.Color.Transparent;
            this.lbSystemName.Font = new System.Drawing.Font("楷体", 21.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbSystemName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lbSystemName.Location = new System.Drawing.Point(207, 47);
            this.lbSystemName.Name = "lbSystemName";
            this.lbSystemName.Size = new System.Drawing.Size(238, 29);
            this.lbSystemName.TabIndex = 0;
            this.lbSystemName.Text = "凡趣POS点餐系统";
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(212)))), ((int)(((byte)(229)))));
            this.BackgroundImage = global::VechsoftPos.Properties.Resources.welcome_bg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1024, 753);
            this.Controls.Add(this.pnlSystemLogin);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormLogin";
            this.Text = "FormLogin";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormLogin_Load);
            this.pnlSystemLogin.ResumeLayout(false);
            this.pnlSystemLogin.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private Top4ever.CustomControl.RoundPanel pnlSystemLogin;
        private Top4ever.CustomControl.CrystalButton btnThree;
        private Top4ever.CustomControl.CrystalButton btnTwo;
        private Top4ever.CustomControl.CrystalButton btnSix;
        private Top4ever.CustomControl.CrystalButton btnFive;
        private Top4ever.CustomControl.CrystalButton btnNine;
        private Top4ever.CustomControl.CrystalButton btnEight;
        private Top4ever.CustomControl.CrystalButton btnExit;
        private Top4ever.CustomControl.CrystalButton btnLogin;
        private Top4ever.CustomControl.CrystalButton btnBackspace;
        private Top4ever.CustomControl.CrystalButton btnZero;
        private Top4ever.CustomControl.CrystalButton btnSeven;
        private Top4ever.CustomControl.CrystalButton btnFour;
        private Top4ever.CustomControl.CrystalButton btnOne;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lbPassword;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Label lbSystemName;
        private Top4ever.CustomControl.CrystalButton btnSwipeCard;

    }
}