namespace Top4ever.CustomControl
{
    partial class HandwritingPad
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnInkColor = new Top4ever.CustomControl.CrystalButton();
            this.btnRecognition = new Top4ever.CustomControl.CrystalButton();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.PicInkPad = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PicInkPad)).BeginInit();
            this.SuspendLayout();
            // 
            // btnInkColor
            // 
            this.btnInkColor.BackColor = System.Drawing.Color.Tomato;
            this.btnInkColor.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnInkColor.ForeColor = System.Drawing.Color.White;
            this.btnInkColor.Location = new System.Drawing.Point(310, 166);
            this.btnInkColor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnInkColor.Name = "btnInkColor";
            this.btnInkColor.Size = new System.Drawing.Size(94, 40);
            this.btnInkColor.TabIndex = 2;
            this.btnInkColor.Text = "墨水颜色";
            this.btnInkColor.UseVisualStyleBackColor = false;
            this.btnInkColor.Click += new System.EventHandler(this.btnInkColor_Click);
            // 
            // btnRecognition
            // 
            this.btnRecognition.BackColor = System.Drawing.Color.Teal;
            this.btnRecognition.ForeColor = System.Drawing.Color.White;
            this.btnRecognition.Location = new System.Drawing.Point(412, 166);
            this.btnRecognition.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRecognition.Name = "btnRecognition";
            this.btnRecognition.Size = new System.Drawing.Size(83, 40);
            this.btnRecognition.TabIndex = 3;
            this.btnRecognition.Text = "识别";
            this.btnRecognition.UseVisualStyleBackColor = false;
            this.btnRecognition.Click += new System.EventHandler(this.btnRecognition_Click);
            // 
            // PicInkPad
            // 
            this.PicInkPad.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PicInkPad.Dock = System.Windows.Forms.DockStyle.Top;
            this.PicInkPad.Location = new System.Drawing.Point(0, 0);
            this.PicInkPad.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PicInkPad.Name = "PicInkPad";
            this.PicInkPad.Size = new System.Drawing.Size(498, 156);
            this.PicInkPad.TabIndex = 1;
            this.PicInkPad.TabStop = false;
            // 
            // HandwritingPad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRecognition);
            this.Controls.Add(this.btnInkColor);
            this.Controls.Add(this.PicInkPad);
            this.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "HandwritingPad";
            this.Size = new System.Drawing.Size(498, 219);
            this.Load += new System.EventHandler(this.HandwritingPad_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PicInkPad)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PicInkPad;
        private CrystalButton btnInkColor;
        private CrystalButton btnRecognition;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}
