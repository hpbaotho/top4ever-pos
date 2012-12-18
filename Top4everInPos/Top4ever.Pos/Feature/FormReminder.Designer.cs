namespace Top4ever.Pos.Feature
{
    partial class FormReminder
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtReason = new System.Windows.Forms.TextBox();
            this.btnCancel = new Top4ever.CustomControl.CrystalButton();
            this.lbReminder = new System.Windows.Forms.Label();
            this.btnConfirm = new Top4ever.CustomControl.CrystalButton();
            this.pnlReason = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // txtReason
            // 
            this.txtReason.Location = new System.Drawing.Point(34, 44);
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(240, 29);
            this.txtReason.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(43)))), ((int)(((byte)(59)))));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(215, 362);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 58);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lbReminder
            // 
            this.lbReminder.AutoSize = true;
            this.lbReminder.Location = new System.Drawing.Point(30, 15);
            this.lbReminder.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbReminder.Name = "lbReminder";
            this.lbReminder.Size = new System.Drawing.Size(154, 21);
            this.lbReminder.TabIndex = 1;
            this.lbReminder.Text = "请选择催单的理由：";
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Teal;
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Location = new System.Drawing.Point(71, 362);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(92, 58);
            this.btnConfirm.TabIndex = 1;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // pnlReason
            // 
            this.pnlReason.Location = new System.Drawing.Point(34, 85);
            this.pnlReason.Name = "pnlReason";
            this.pnlReason.Size = new System.Drawing.Size(315, 266);
            this.pnlReason.TabIndex = 1;
            // 
            // FormReminder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 440);
            this.ControlBox = false;
            this.Controls.Add(this.txtReason);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lbReminder);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.pnlReason);
            this.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "FormReminder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "催单理由";
            this.Load += new System.EventHandler(this.FormReminder_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtReason;
        private CustomControl.CrystalButton btnCancel;
        private System.Windows.Forms.Label lbReminder;
        private CustomControl.CrystalButton btnConfirm;
        private System.Windows.Forms.Panel pnlReason;

    }
}