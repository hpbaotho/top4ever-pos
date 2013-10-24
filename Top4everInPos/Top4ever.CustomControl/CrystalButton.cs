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

        private void DrawLinearGradient(Graphics g, bool xiacen)
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
            int beginRed = red + 3 > 255 ? 255 : red + 3;
            int beginGreen = green + 20 > 255 ? 255 : green + 20;
            int beginBlue = blue + 30 > 255 ? 255 : blue + 30;
            int endRed = red - 15 < 0 ? 0 : red - 15;
            int endGreen = green - 15 < 0 ? 0 : green - 15;
            int endBlue = blue - 10 < 0 ? 0 : blue - 10;

            GraphicsPath path = GetGraphicsPath(rect);
            RectangleF rect1 = path.GetBounds();
            rect1.Height = rect1.Height + 1;

            g.FillPath(new LinearGradientBrush(rect1,
             Color.FromArgb(0xff, beginRed, beginGreen, beginBlue),
             Color.FromArgb(0xff, endRed, endGreen, endBlue), LinearGradientMode.ForwardDiagonal), path);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            e.Graphics.FillRectangle(new SolidBrush(backColor), 0, 0, this.Width, this.Height);
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            DrawLinearGradient(e.Graphics, XiaCen);

            Rectangle imageRect, textRect;
            CalculateRect(out imageRect, out textRect);
            if (Image != null)
            {
                e.Graphics.DrawImage(Image, imageRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel);
            }
            if (XiaCen)
            {
                textRect.Y = textRect.Y + 1;
            }
            Font font = this.Font;
            if (mouseMove)
            {
                font = new Font(this.Font, FontStyle.Underline);
            }
            Color textColor = this.Enabled ? this.ForeColor : SystemColors.GrayText;
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString(this.Text, font, new SolidBrush(textColor), textRect, stringFormat);
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

        private void CrystalButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (XiaCen == false && e.KeyCode == Keys.Space)
            {
                XiaCen = true;
                this.Refresh();
            }
        }

        private void CrystalButton_KeyUp(object sender, KeyEventArgs e)
        {
            if (XiaCen == true && e.KeyCode == Keys.Space)
            {
                XiaCen = false;
                this.Refresh();
            }
        }

        private void CrystalButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (XiaCen == false)
            {
                XiaCen = true;
                this.Refresh();
            }
        }

        private void CrystalButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (XiaCen == true)
            {
                XiaCen = false;
                this.Refresh();
            }
        }

        private void CrystalButton_MouseEnter(object sender, EventArgs e)
        {
            if (mouseMove == false)
            {
                mouseMove = true;
                this.Refresh();
            }
        }

        private void CrystalButton_MouseLeave(object sender, EventArgs e)
        {
            if (mouseMove == true)
            {
                mouseMove = false;
                this.Refresh();
            }
        }

        private void CalculateRect(out Rectangle imageRect, out Rectangle textRect)
        {
            imageRect = Rectangle.Empty;
            textRect = Rectangle.Empty;
            if (Image == null)
            {
                textRect = new Rectangle(
                   3,
                   0,
                   Width - 6,
                   Height);
                return;
            }
            switch (TextImageRelation)
            {
                case TextImageRelation.Overlay:
                    imageRect = new Rectangle(
                        (this.Width - ImageWidth) / 2,
                        (this.Height - ImageHeight) / 2,
                        ImageWidth,
                        ImageHeight);
                    textRect = new Rectangle(
                        3,
                        0,
                        Width - 6,
                        Height);
                    break;
                case TextImageRelation.ImageAboveText:
                    imageRect = new Rectangle(
                        (this.Width - ImageWidth) / 2,
                        (this.Height - ImageHeight - 12) / 2,
                        ImageWidth,
                        ImageHeight);
                    textRect = new Rectangle(
                        3,
                        imageRect.Bottom,
                        Width - 6,
                        Height - imageRect.Bottom - 2);
                    break;
                case TextImageRelation.ImageBeforeText:
                    imageRect = new Rectangle(
                        3,
                        (Height - ImageWidth) / 2,
                        ImageWidth,
                        ImageWidth);
                    textRect = new Rectangle(
                        imageRect.Right + 3,
                        0,
                        Width - imageRect.Right - 6,
                        Height);
                    break;
                case TextImageRelation.TextAboveImage:
                    imageRect = new Rectangle(
                        (this.Width - ImageWidth) / 2,
                        this.Height / 3,
                        ImageWidth,
                        ImageHeight);
                    textRect = new Rectangle(
                        3,
                        this.Height / 9,
                        Width - 6,
                        this.Height / 3 - this.Height / 9);
                    break;
                case TextImageRelation.TextBeforeImage:
                    imageRect = new Rectangle(
                        Width - ImageWidth - 6,
                        (Height - ImageWidth) / 2,
                        ImageWidth,
                        ImageWidth);
                    textRect = new Rectangle(
                        3,
                        0,
                        imageRect.X - 3,
                        Height);
                    break;
            }
            if (RightToLeft == RightToLeft.Yes)
            {
                imageRect.X = Width - imageRect.Right;
                textRect.X = Width - textRect.Right;
            }
        }

        private int ImageWidth
        {
            get
            {
                if (Image == null)
                {
                    return 32;
                }
                else
                {
                    return Image.Width;
                }
            }
        }

        private int ImageHeight
        {
            get
            {
                if (Image == null)
                {
                    return 32;
                }
                else
                {
                    return Image.Height;
                }
            }
        }

        internal TextFormatFlags GetTextFormatFlags(ContentAlignment alignment, bool rightToleft)
        {
            TextFormatFlags flags = TextFormatFlags.WordBreak |
                TextFormatFlags.SingleLine;
            if (rightToleft)
            {
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            }

            switch (alignment)
            {
                case ContentAlignment.BottomCenter:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.BottomLeft:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                    break;
                case ContentAlignment.BottomRight:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                    break;
                case ContentAlignment.MiddleCenter:
                    flags |= TextFormatFlags.HorizontalCenter |
                        TextFormatFlags.VerticalCenter;
                    break;
                case ContentAlignment.MiddleLeft:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;
                case ContentAlignment.MiddleRight:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;
                case ContentAlignment.TopCenter:
                    flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.TopLeft:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Left;
                    break;
                case ContentAlignment.TopRight:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Right;
                    break;
            }
            return flags;
        }
    }
}