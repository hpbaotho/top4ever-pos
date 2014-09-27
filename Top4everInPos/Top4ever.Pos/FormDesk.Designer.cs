namespace VechsoftPos
{
    partial class FormDesk
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
            this.pnlToolBar = new System.Windows.Forms.Panel();
            this.scrollingText1 = new Top4ever.CustomControl.ScrollingText();
            this.pnlSysTools = new System.Windows.Forms.Panel();
            this.btnExit = new Top4ever.CustomControl.CrystalButton();
            this.btnTakeOut = new Top4ever.CustomControl.CrystalButton();
            this.btnCheckOut = new Top4ever.CustomControl.CrystalButton();
            this.btnTurnTable = new Top4ever.CustomControl.CrystalButton();
            this.btnClear = new Top4ever.CustomControl.CrystalButton();
            this.btnOrder = new Top4ever.CustomControl.CrystalButton();
            this.btnManager = new Top4ever.CustomControl.CrystalButton();
            this.pnlDesk = new System.Windows.Forms.Panel();
            this.btnLookColor = new Top4ever.CustomControl.CrystalButton();
            this.btnTakeColor = new Top4ever.CustomControl.CrystalButton();
            this.btnFreeColor = new Top4ever.CustomControl.CrystalButton();
            this.pnlRegion = new System.Windows.Forms.Panel();
            this.pnlToolBar.SuspendLayout();
            this.pnlSysTools.SuspendLayout();
            this.pnlDesk.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlToolBar
            // 
            this.pnlToolBar.Controls.Add(this.scrollingText1);
            this.pnlToolBar.Controls.Add(this.pnlSysTools);
            this.pnlToolBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolBar.Location = new System.Drawing.Point(0, 0);
            this.pnlToolBar.Name = "pnlToolBar";
            this.pnlToolBar.Size = new System.Drawing.Size(1024, 110);
            this.pnlToolBar.TabIndex = 0;
            // 
            // scrollingText1
            // 
            this.scrollingText1.BackColor = System.Drawing.Color.Tomato;
            this.scrollingText1.BorderColor = System.Drawing.Color.Black;
            this.scrollingText1.Cursor = System.Windows.Forms.Cursors.Default;
            this.scrollingText1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scrollingText1.ForeColor = System.Drawing.Color.White;
            this.scrollingText1.ForegroundBrush = null;
            this.scrollingText1.Location = new System.Drawing.Point(0, 73);
            this.scrollingText1.Name = "scrollingText1";
            this.scrollingText1.ScrollDirection = Top4ever.CustomControl.ScrollDirection.RightToLeft;
            this.scrollingText1.ScrollText = "新版本测试使用，请在每天22点前结束营业";
            this.scrollingText1.ShowBorder = false;
            this.scrollingText1.Size = new System.Drawing.Size(1024, 35);
            this.scrollingText1.StopScrollOnMouseOver = true;
            this.scrollingText1.TabIndex = 2;
            this.scrollingText1.Text = "scrollingText1";
            this.scrollingText1.TextScrollDistance = 2;
            this.scrollingText1.TextScrollSpeed = 50;
            this.scrollingText1.VerticleTextPosition = Top4ever.CustomControl.VerticleTextPosition.Center;
            this.scrollingText1.TextClicked += new Top4ever.CustomControl.ScrollingText.TextClickEventHandler(this.scrollingText1_TextClicked);
            // 
            // pnlSysTools
            // 
            this.pnlSysTools.Controls.Add(this.btnExit);
            this.pnlSysTools.Controls.Add(this.btnTakeOut);
            this.pnlSysTools.Controls.Add(this.btnCheckOut);
            this.pnlSysTools.Controls.Add(this.btnTurnTable);
            this.pnlSysTools.Controls.Add(this.btnClear);
            this.pnlSysTools.Controls.Add(this.btnOrder);
            this.pnlSysTools.Controls.Add(this.btnManager);
            this.pnlSysTools.Location = new System.Drawing.Point(0, 0);
            this.pnlSysTools.Name = "pnlSysTools";
            this.pnlSysTools.Size = new System.Drawing.Size(1024, 72);
            this.pnlSysTools.TabIndex = 1;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Red;
            this.btnExit.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(747, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 71);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnTakeOut
            // 
            this.btnTakeOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(113)))), ((int)(((byte)(202)))));
            this.btnTakeOut.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTakeOut.ForeColor = System.Drawing.Color.White;
            this.btnTakeOut.Location = new System.Drawing.Point(627, 0);
            this.btnTakeOut.Name = "btnTakeOut";
            this.btnTakeOut.Size = new System.Drawing.Size(75, 71);
            this.btnTakeOut.TabIndex = 9;
            this.btnTakeOut.Text = "外卖";
            this.btnTakeOut.UseVisualStyleBackColor = false;
            this.btnTakeOut.Click += new System.EventHandler(this.btnTakeOut_Click);
            // 
            // btnCheckOut
            // 
            this.btnCheckOut.BackColor = System.Drawing.Color.Green;
            this.btnCheckOut.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btnCheckOut.ForeColor = System.Drawing.Color.White;
            this.btnCheckOut.Location = new System.Drawing.Point(505, 0);
            this.btnCheckOut.Name = "btnCheckOut";
            this.btnCheckOut.Size = new System.Drawing.Size(75, 71);
            this.btnCheckOut.TabIndex = 8;
            this.btnCheckOut.Text = "结帐";
            this.btnCheckOut.UseVisualStyleBackColor = false;
            this.btnCheckOut.Click += new System.EventHandler(this.btnCheckOut_Click);
            // 
            // btnTurnTable
            // 
            this.btnTurnTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(154)))), ((int)(((byte)(0)))));
            this.btnTurnTable.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btnTurnTable.ForeColor = System.Drawing.Color.White;
            this.btnTurnTable.Location = new System.Drawing.Point(383, 0);
            this.btnTurnTable.Name = "btnTurnTable";
            this.btnTurnTable.Size = new System.Drawing.Size(75, 71);
            this.btnTurnTable.TabIndex = 3;
            this.btnTurnTable.Text = "转台";
            this.btnTurnTable.UseVisualStyleBackColor = false;
            this.btnTurnTable.Click += new System.EventHandler(this.btnTurnTable_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Teal;
            this.btnClear.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(278, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 71);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnOrder
            // 
            this.btnOrder.BackColor = System.Drawing.Color.OrangeRed;
            this.btnOrder.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btnOrder.ForeColor = System.Drawing.Color.White;
            this.btnOrder.Location = new System.Drawing.Point(173, 0);
            this.btnOrder.Name = "btnOrder";
            this.btnOrder.Size = new System.Drawing.Size(75, 71);
            this.btnOrder.TabIndex = 5;
            this.btnOrder.Text = "点单";
            this.btnOrder.UseVisualStyleBackColor = false;
            this.btnOrder.Click += new System.EventHandler(this.btnOrder_Click);
            // 
            // btnManager
            // 
            this.btnManager.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnManager.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnManager.ForeColor = System.Drawing.Color.White;
            this.btnManager.Location = new System.Drawing.Point(72, 0);
            this.btnManager.Name = "btnManager";
            this.btnManager.Size = new System.Drawing.Size(75, 71);
            this.btnManager.TabIndex = 4;
            this.btnManager.Text = "功能面板";
            this.btnManager.UseVisualStyleBackColor = false;
            this.btnManager.Click += new System.EventHandler(this.btnManager_Click);
            // 
            // pnlDesk
            // 
            this.pnlDesk.Controls.Add(this.btnLookColor);
            this.pnlDesk.Controls.Add(this.btnTakeColor);
            this.pnlDesk.Controls.Add(this.btnFreeColor);
            this.pnlDesk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDesk.Location = new System.Drawing.Point(0, 110);
            this.pnlDesk.Name = "pnlDesk";
            this.pnlDesk.Size = new System.Drawing.Size(1024, 551);
            this.pnlDesk.TabIndex = 2;
            // 
            // btnLookColor
            // 
            this.btnLookColor.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnLookColor.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLookColor.ForeColor = System.Drawing.Color.White;
            this.btnLookColor.Location = new System.Drawing.Point(937, 510);
            this.btnLookColor.Name = "btnLookColor";
            this.btnLookColor.Size = new System.Drawing.Size(75, 28);
            this.btnLookColor.TabIndex = 0;
            this.btnLookColor.Text = "锁定";
            this.btnLookColor.UseVisualStyleBackColor = false;
            // 
            // btnTakeColor
            // 
            this.btnTakeColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(43)))), ((int)(((byte)(59)))));
            this.btnTakeColor.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTakeColor.ForeColor = System.Drawing.Color.White;
            this.btnTakeColor.Location = new System.Drawing.Point(856, 510);
            this.btnTakeColor.Name = "btnTakeColor";
            this.btnTakeColor.Size = new System.Drawing.Size(75, 28);
            this.btnTakeColor.TabIndex = 0;
            this.btnTakeColor.Text = "已占";
            this.btnTakeColor.UseVisualStyleBackColor = false;
            // 
            // btnFreeColor
            // 
            this.btnFreeColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(138)))), ((int)(((byte)(208)))));
            this.btnFreeColor.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFreeColor.ForeColor = System.Drawing.Color.White;
            this.btnFreeColor.Location = new System.Drawing.Point(775, 510);
            this.btnFreeColor.Name = "btnFreeColor";
            this.btnFreeColor.Size = new System.Drawing.Size(75, 28);
            this.btnFreeColor.TabIndex = 0;
            this.btnFreeColor.Text = "空闲";
            this.btnFreeColor.UseVisualStyleBackColor = false;
            // 
            // pnlRegion
            // 
            this.pnlRegion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(199)))), ((int)(((byte)(172)))));
            this.pnlRegion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlRegion.Location = new System.Drawing.Point(0, 661);
            this.pnlRegion.Name = "pnlRegion";
            this.pnlRegion.Size = new System.Drawing.Size(1024, 89);
            this.pnlRegion.TabIndex = 3;
            // 
            // FormDesk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 750);
            this.Controls.Add(this.pnlDesk);
            this.Controls.Add(this.pnlRegion);
            this.Controls.Add(this.pnlToolBar);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormDesk";
            this.Text = "FormDesk";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormDesk_Load);
            this.pnlToolBar.ResumeLayout(false);
            this.pnlSysTools.ResumeLayout(false);
            this.pnlDesk.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlToolBar;
        private System.Windows.Forms.Panel pnlDesk;
        private System.Windows.Forms.Panel pnlRegion;
        private Top4ever.CustomControl.CrystalButton btnFreeColor;
        private Top4ever.CustomControl.CrystalButton btnLookColor;
        private Top4ever.CustomControl.CrystalButton btnTakeColor;
        private System.Windows.Forms.Panel pnlSysTools;
        private Top4ever.CustomControl.CrystalButton btnExit;
        private Top4ever.CustomControl.CrystalButton btnTakeOut;
        private Top4ever.CustomControl.CrystalButton btnCheckOut;
        private Top4ever.CustomControl.CrystalButton btnTurnTable;
        private Top4ever.CustomControl.CrystalButton btnClear;
        private Top4ever.CustomControl.CrystalButton btnOrder;
        private Top4ever.CustomControl.CrystalButton btnManager;
        private Top4ever.CustomControl.ScrollingText scrollingText1;
    }
}