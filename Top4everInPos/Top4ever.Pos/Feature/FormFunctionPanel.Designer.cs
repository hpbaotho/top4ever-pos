namespace Top4ever.Pos.Feature
{
    partial class FormFunctionPanel
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
            this.pnlFunction = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlFunction
            // 
            this.pnlFunction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(84)))), ((int)(((byte)(97)))));
            this.pnlFunction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFunction.Location = new System.Drawing.Point(0, 0);
            this.pnlFunction.Name = "pnlFunction";
            this.pnlFunction.Size = new System.Drawing.Size(594, 422);
            this.pnlFunction.TabIndex = 0;
            // 
            // FormFunctionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 422);
            this.Controls.Add(this.pnlFunction);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFunctionPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormFunctionPanel";
            this.Load += new System.EventHandler(this.FormFunctionPanel_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlFunction;
    }
}