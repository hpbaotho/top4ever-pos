using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace Top4ever.CustomControl
{
    public class ExTabControl : TabControl
    {
        private Image _BackgroundImage;
        [Browsable(true), Category("ExTabControl Property")]
        public Image TabBackgroundImage
        {
            get { return _BackgroundImage; }
            set { _BackgroundImage = value; Invalidate(); }
        }

        bool mouseDown = false;
        Image icon;
        ImageAttributes ia;
        ColorMatrix cm;
        float[][] colorMatrix ={
            new float[]{0.299f, 0.299f, 0.299f, 0, 0},
            new float[]{0.587f, 0.587f, 0.587f, 0, 0},
            new float[]{0.114f, 0.114f, 0.114f, 0, 0},
            new float[]{0, 0, 0, 1, 0},
            new float[]{0, 0, 0, 1, 0}};

        public ExTabControl()
        {
            base.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            base.UpdateStyles();

            ia = new ImageAttributes();
            cm = new ColorMatrix(colorMatrix);
            ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            icon = new Bitmap(1, 1);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            for (int i = 0; i < this.TabCount; i++)
            {
                if (this.SelectedIndex == i && _BackgroundImage != null)
                {
                    e.Graphics.DrawImage(_BackgroundImage, this.GetTabRect(i));
                }

                if (this.ImageList != null && (this.TabPages[i].ImageIndex > -1 || !string.IsNullOrEmpty(this.TabPages[i].ImageKey)))
                {
                    if (this.TabPages[i].ImageIndex > -1)
                        icon = this.ImageList.Images[this.TabPages[i].ImageIndex];
                    else
                        if (!string.IsNullOrEmpty(this.TabPages[i].ImageKey))
                            icon = this.ImageList.Images[this.TabPages[i].ImageKey];

                    if (mouseDown && (this.SelectedIndex != i))
                        e.Graphics.DrawImage(
                            icon,
                            new Rectangle(
                                this.GetTabRect(i).X + (this.GetTabRect(i).Width - icon.Width) / 2,
                                this.GetTabRect(i).Y,
                                icon.Width,
                                icon.Height),
                            0,
                            0,
                            icon.Width,
                            icon.Height,
                            GraphicsUnit.Pixel,
                            ia);
                    else
                        e.Graphics.DrawImage(
                            icon,
                            this.GetTabRect(i).X + (this.GetTabRect(i).Width - icon.Width) / 2,
                            this.GetTabRect(i).Y);

                }

                SizeF textSize = e.Graphics.MeasureString(this.TabPages[i].Text, this.Font);

                e.Graphics.DrawString(
                    this.TabPages[i].Text,
                    this.Font,
                    SystemBrushes.ControlLightLight,
                    this.GetTabRect(i).X + (this.GetTabRect(i).Width - textSize.Width) / 2 + 1,
                    this.GetTabRect(i).Y + 36);

                e.Graphics.DrawString(
                    this.TabPages[i].Text,
                    this.Font,
                    SystemBrushes.ControlText,
                    this.GetTabRect(i).X + (this.GetTabRect(i).Width - textSize.Width) / 2,
                    this.GetTabRect(i).Y + 35);
            }
        }
    }
}
