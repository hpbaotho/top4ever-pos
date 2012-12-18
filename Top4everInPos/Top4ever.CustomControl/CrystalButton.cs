using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Top4ever.CustomControl
{
    /// <summary>
    /// CrystalButton 的摘要说明。
    /// </summary>
    public class CrystalButton : Button
    {
        /// <summary>
        /// 保存默认背景颜色
        /// </summary>
        public Color DisplayColor;

        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;
        private bool XiaCen = false;
        private bool mouseMove = false;
        private Color backColor;

        public CrystalButton()
        {
            // 该调用是 Windows.Forms 窗体设计器所必需的。
            InitializeComponent();
            // TODO: 在 InitComponent 调用后添加任何初始化
            base.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            base.UpdateStyles();

            backColor = Color.DarkGray;
        }

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码
        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器 
        /// 修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // CrystalButton
            // 
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CrystalButton_MouseUp);
            this.MouseEnter += new System.EventHandler(this.CrystalButton_MouseEnter);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CrystalButton_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CrystalButton_KeyDown);
            this.BackColorChanged += new System.EventHandler(this.CrystalButton_BackColorChanged);
            this.MouseLeave += new System.EventHandler(this.CrystalButton_MouseLeave);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CrystalButton_MouseDown);
        }
        #endregion
        
        protected GraphicsPath GetGraphicsPath(Rectangle rect)
        {
            GraphicsPath ClientPath = new System.Drawing.Drawing2D.GraphicsPath();
            if (rect.Width <= 0)
            {
                rect.Width = 1;
            }
            if (rect.Height <= 0)
            {
                rect.Height = 1;
            }

            ClientPath.AddRectangle(rect);

            ClientPath.CloseFigure();
            return ClientPath;
        }

        private void DrawGaoLiang(Graphics g, bool xiacen)
        {
            Rectangle rect = this.ClientRectangle;
            rect.Inflate(-1, -1);

            if (xiacen)
            {
                rect.Y = rect.Y + 1;
            }

            int red = backColor.R;
            int green = backColor.G;
            int blue = backColor.B;
            red = red + 3;
            green = green + 20;
            blue = blue + 30;
            if (red > 255) red = 255;
            if (green > 255) green = 255;
            if (blue > 255) blue = 255;

            GraphicsPath path = GetGraphicsPath(rect);
            RectangleF rect1 = path.GetBounds();
            rect1.Height = rect1.Height + 1;
            g.FillPath(new LinearGradientBrush(rect1,
             Color.FromArgb(0xff, red, green, blue),
             Color.FromArgb(0xff, backColor), LinearGradientMode.ForwardDiagonal), path);
        }

        private void DrawText(Graphics g, bool xiacen)
        {
            Rectangle rect = this.ClientRectangle;
            Rectangle rect1 = this.ClientRectangle;
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            rect.Y = this.ClientRectangle.Height / 5;
            if (xiacen)
            {
                rect.Y = rect.Y + 1;
                rect1.Y = rect1.Y + 1;
            }

            Font font = this.Font;
            
            if (mouseMove)
            {
                font = new Font(this.Font, FontStyle.Underline);
            }

            g.DrawString(this.Text, font, new SolidBrush(this.ForeColor), rect1, stringFormat);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            e.Graphics.FillRectangle(new SolidBrush(backColor), 0, 0, this.Width, this.Height);
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            GraphicsPath ClientPath = GetGraphicsPath(rect);
            e.Graphics.FillPath(new SolidBrush(backColor), ClientPath);
            this.Region = new System.Drawing.Region(ClientPath);
            DrawGaoLiang(e.Graphics, XiaCen);
            DrawText(e.Graphics, XiaCen);

            if (this.Focused)
            {
                e.Graphics.DrawPath(new Pen(Color.FromArgb(0x22, 0xff, 0xff, 0xff), 3), ClientPath);
            }
        }

        private void CrystalButton_BackColorChanged(object sender, System.EventArgs e)
        {
            int r = BackColor.R;
            int g = BackColor.G;
            int b = BackColor.B;
            r = r + 0x22;
            g = g + 0x22;
            b = b + 0x22;
            if (r > 255) r = 255;
            if (g > 255) g = 255;
            if (b > 255) b = 255;
            backColor = Color.FromArgb(r, g, b);
        }

        private void CrystalButton_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (XiaCen == false && e.KeyCode == Keys.Space)
            {
                XiaCen = true;
                this.Refresh();
            }
        }

        private void CrystalButton_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (XiaCen == true && e.KeyCode == Keys.Space)
            {
                XiaCen = false;
                this.Refresh();
            }
        }

        private void CrystalButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (XiaCen == false)
            {
                XiaCen = true;
                this.Refresh();
            }
        }

        private void CrystalButton_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (XiaCen == true)
            {
                XiaCen = false;
                this.Refresh();
            }
        }

        private void CrystalButton_MouseEnter(object sender, System.EventArgs e)
        {
            if (mouseMove == false)
            {
                mouseMove = true;
                this.Refresh();
            }
        }

        private void CrystalButton_MouseLeave(object sender, System.EventArgs e)
        {
            if (mouseMove == true)
            {
                mouseMove = false;
                this.Refresh();
            }
        }
    }
}