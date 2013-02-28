namespace Top4ever.Pos
{
    partial class FormConfirm
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtReceMoney = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPaidInMoney = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNeedChangePay = new System.Windows.Forms.TextBox();
            this.txtPayoffWay = new System.Windows.Forms.TextBox();
            this.btnConfirm = new Top4ever.CustomControl.CrystalButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "应收金额：";
            // 
            // txtReceMoney
            // 
            this.txtReceMoney.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.txtReceMoney.Location = new System.Drawing.Point(156, 63);
            this.txtReceMoney.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtReceMoney.Name = "txtReceMoney";
            this.txtReceMoney.ReadOnly = true;
            this.txtReceMoney.Size = new System.Drawing.Size(436, 41);
            this.txtReceMoney.TabIndex = 1;
            this.txtReceMoney.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 29);
            this.label2.TabIndex = 0;
            this.label2.Text = "收款：";
            // 
            // txtPaidInMoney
            // 
            this.txtPaidInMoney.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.txtPaidInMoney.Location = new System.Drawing.Point(156, 153);
            this.txtPaidInMoney.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPaidInMoney.Name = "txtPaidInMoney";
            this.txtPaidInMoney.Size = new System.Drawing.Size(436, 41);
            this.txtPaidInMoney.TabIndex = 2;
            this.txtPaidInMoney.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 210);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(313, 29);
            this.label3.TabIndex = 0;
            this.label3.Text = "找零（礼券不找零）：";
            // 
            // txtNeedChangePay
            // 
            this.txtNeedChangePay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtNeedChangePay.Location = new System.Drawing.Point(156, 256);
            this.txtNeedChangePay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtNeedChangePay.Name = "txtNeedChangePay";
            this.txtNeedChangePay.Size = new System.Drawing.Size(436, 41);
            this.txtNeedChangePay.TabIndex = 3;
            this.txtNeedChangePay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtPayoffWay
            // 
            this.txtPayoffWay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.txtPayoffWay.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPayoffWay.Location = new System.Drawing.Point(29, 328);
            this.txtPayoffWay.Multiline = true;
            this.txtPayoffWay.Name = "txtPayoffWay";
            this.txtPayoffWay.Size = new System.Drawing.Size(563, 62);
            this.txtPayoffWay.TabIndex = 4;
            this.txtPayoffWay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Green;
            this.btnConfirm.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Location = new System.Drawing.Point(469, 411);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(123, 47);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // FormConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(614, 478);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.txtPayoffWay);
            this.Controls.Add(this.txtNeedChangePay);
            this.Controls.Add(this.txtPaidInMoney);
            this.Controls.Add(this.txtReceMoney);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfirm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormConfirm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtReceMoney;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPaidInMoney;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNeedChangePay;
        private System.Windows.Forms.TextBox txtPayoffWay;
        private CustomControl.CrystalButton btnConfirm;
    }
}