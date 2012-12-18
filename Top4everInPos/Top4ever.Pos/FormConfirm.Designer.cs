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
            this.txtTime = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label1.Location = new System.Drawing.Point(18, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "应收金额：";
            // 
            // txtReceMoney
            // 
            this.txtReceMoney.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.txtReceMoney.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReceMoney.Location = new System.Drawing.Point(119, 59);
            this.txtReceMoney.Name = "txtReceMoney";
            this.txtReceMoney.ReadOnly = true;
            this.txtReceMoney.Size = new System.Drawing.Size(769, 47);
            this.txtReceMoney.TabIndex = 1;
            this.txtReceMoney.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label2.Location = new System.Drawing.Point(18, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 39);
            this.label2.TabIndex = 0;
            this.label2.Text = "收款：";
            // 
            // txtPaidInMoney
            // 
            this.txtPaidInMoney.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.txtPaidInMoney.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPaidInMoney.Location = new System.Drawing.Point(119, 212);
            this.txtPaidInMoney.Name = "txtPaidInMoney";
            this.txtPaidInMoney.Size = new System.Drawing.Size(769, 47);
            this.txtPaidInMoney.TabIndex = 2;
            this.txtPaidInMoney.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label3.Location = new System.Drawing.Point(18, 319);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(377, 39);
            this.label3.TabIndex = 0;
            this.label3.Text = "找零（礼券不找零）：";
            // 
            // txtNeedChangePay
            // 
            this.txtNeedChangePay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtNeedChangePay.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNeedChangePay.Location = new System.Drawing.Point(119, 361);
            this.txtNeedChangePay.Name = "txtNeedChangePay";
            this.txtNeedChangePay.Size = new System.Drawing.Size(769, 47);
            this.txtNeedChangePay.TabIndex = 3;
            this.txtNeedChangePay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtPayoffWay
            // 
            this.txtPayoffWay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.txtPayoffWay.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPayoffWay.Location = new System.Drawing.Point(37, 488);
            this.txtPayoffWay.Name = "txtPayoffWay";
            this.txtPayoffWay.Size = new System.Drawing.Size(949, 47);
            this.txtPayoffWay.TabIndex = 4;
            this.txtPayoffWay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Green;
            this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Location = new System.Drawing.Point(808, 678);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(154, 55);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // txtTime
            // 
            this.txtTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTime.Location = new System.Drawing.Point(215, 606);
            this.txtTime.Name = "txtTime";
            this.txtTime.ReadOnly = true;
            this.txtTime.Size = new System.Drawing.Size(258, 47);
            this.txtTime.TabIndex = 4;
            this.txtTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label4.Location = new System.Drawing.Point(18, 609);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(172, 39);
            this.label4.TabIndex = 0;
            this.label4.Text = "服务时间:";
            // 
            // FormConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(20F, 39F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.txtPayoffWay);
            this.Controls.Add(this.txtNeedChangePay);
            this.Controls.Add(this.txtPaidInMoney);
            this.Controls.Add(this.txtReceMoney);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.Name = "FormConfirm";
            this.Text = "FormConfirm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
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
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.Label label4;
    }
}