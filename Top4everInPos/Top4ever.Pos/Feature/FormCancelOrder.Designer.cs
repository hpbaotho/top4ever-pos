namespace Top4ever.Pos.Feature
{
    partial class FormCancelOrder
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
            this.pnlReason = new System.Windows.Forms.Panel();
            this.btnConfirm = new Top4ever.CustomControl.CrystalButton();
            this.btnCancel = new Top4ever.CustomControl.CrystalButton();
            this.btnNumber = new Top4ever.CustomControl.CrystalButton();
            this.txtItemNun = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pnlReason
            // 
            this.pnlReason.Location = new System.Drawing.Point(31, 97);
            this.pnlReason.Name = "pnlReason";
            this.pnlReason.Size = new System.Drawing.Size(315, 254);
            this.pnlReason.TabIndex = 2;
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Teal;
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Location = new System.Drawing.Point(72, 362);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(92, 58);
            this.btnConfirm.TabIndex = 4;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(43)))), ((int)(((byte)(59)))));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(208, 362);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 58);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNumber
            // 
            this.btnNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(148)))), ((int)(((byte)(91)))));
            this.btnNumber.ForeColor = System.Drawing.Color.White;
            this.btnNumber.Location = new System.Drawing.Point(248, 21);
            this.btnNumber.Name = "btnNumber";
            this.btnNumber.Size = new System.Drawing.Size(98, 64);
            this.btnNumber.TabIndex = 4;
            this.btnNumber.Text = "数量";
            this.btnNumber.UseVisualStyleBackColor = false;
            this.btnNumber.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // txtItemNun
            // 
            this.txtItemNun.Location = new System.Drawing.Point(33, 56);
            this.txtItemNun.Name = "txtItemNun";
            this.txtItemNun.Size = new System.Drawing.Size(201, 29);
            this.txtItemNun.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "请选择删除品项的数量：";
            // 
            // FormCancelOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 440);
            this.ControlBox = false;
            this.Controls.Add(this.btnNumber);
            this.Controls.Add(this.txtItemNun);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.pnlReason);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FormCancelOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "删除品项";
            this.Load += new System.EventHandler(this.FormReason_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlReason;
        private CustomControl.CrystalButton btnConfirm;
        private CustomControl.CrystalButton btnCancel;
        private CustomControl.CrystalButton btnNumber;
        private System.Windows.Forms.TextBox txtItemNun;
        private System.Windows.Forms.Label label1;
    }
}