using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Top4ever.CustomControl
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(DataGridView))]
    public class DataGirdViewExt : DataGridView
    {
        public Color ColumnHeaderColor{get;set;}

        public Color RowHeaderColor{get;set;}

        private Color defaultcolor;

        public DataGirdViewExt()
        {
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            ColumnHeaderColor=Color.FromArgb(186,210,249);
            RowHeaderColor=Color.FromArgb(210,220,248);
        }

        protected override void OnCreateControl()
        {
            this.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.MultiSelect = false;
            this.BackgroundColor = Color.FromKnownColor(System.Drawing.KnownColor.White);//WhiteSmoke
            this.GridColor = Color.FromKnownColor(System.Drawing.KnownColor.ActiveBorder);
            this.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.ColumnHeadersHeight = 32;

            //隔行颜色
            this.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(241, 244, 248);

            base.OnCreateControl();
        }

        #region DataGridView的鼠标跟随效果

        //进入单元格时保存当前的颜色
        protected override void OnCellMouseEnter(DataGridViewCellEventArgs e)
        {
            base.OnCellMouseEnter(e);
            if (e.RowIndex >= 0)
            {
                defaultcolor = Rows[e.RowIndex].DefaultCellStyle.BackColor;
                Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromKnownColor(System.Drawing.KnownColor.InactiveCaptionText);
            }
        }

        //离开时还原颜色
        protected override void OnCellMouseLeave(DataGridViewCellEventArgs e)
        {
            base.OnCellMouseLeave(e);
            if (e.RowIndex >= 0)
            {
                Rows[e.RowIndex].DefaultCellStyle.BackColor = defaultcolor;
            }
        }
        #endregion

        #region DataGridView的效果绘制

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            Rectangle newRect = new Rectangle(e.CellBounds.X - 1, e.CellBounds.Y - 1, e.CellBounds.Width, e.CellBounds.Height);

            if (e.ColumnIndex == -1 && e.RowIndex == -1)
            {
                e.Graphics.DrawRectangle(Pens.LightSteelBlue, newRect);
            }
            else
            {
                if (e.ColumnIndex == -1)
                {
                    Color begin = Color.FromArgb(241, 244, 248);
                    Color end = RowHeaderColor;
                    using (Brush brush = new LinearGradientBrush(
                        e.CellBounds, begin, end, LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                        e.Graphics.DrawRectangle(Pens.LightSteelBlue, newRect);
                    }

                    DataGridViewPaintParts pa =
                        DataGridViewPaintParts.Border |
                        DataGridViewPaintParts.ContentBackground |
                        DataGridViewPaintParts.ContentForeground |
                        DataGridViewPaintParts.ErrorIcon |
                        DataGridViewPaintParts.Focus |
                        DataGridViewPaintParts.SelectionBackground;
                    e.Paint(e.ClipBounds, pa);

                    e.Handled = true;
                }
                if (e.RowIndex == -1)
                {
                    Color begin = Color.FromArgb(241, 244, 248);
                    Color end = ColumnHeaderColor;

                    using (Brush brush = new LinearGradientBrush(
                        e.CellBounds, begin, end, LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                        e.Graphics.DrawRectangle(Pens.LightSteelBlue, newRect);
                    }

                    DataGridViewPaintParts pa =
                        DataGridViewPaintParts.Border |
                        DataGridViewPaintParts.ContentBackground |
                        DataGridViewPaintParts.ContentForeground |
                        DataGridViewPaintParts.ErrorIcon |
                        DataGridViewPaintParts.Focus |
                        DataGridViewPaintParts.SelectionBackground;
                    e.Paint(e.ClipBounds, pa);

                    e.Handled = true;
                }
            }
        }

        #endregion
    }
}
