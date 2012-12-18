using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.CustomControl.Appearances;

namespace Top4ever.CustomControl
{
    public partial class ExButton : Button
    {
        private bool _enableCustomAppearance;
        private ButtonUIAppearance _customAppearance;
        private bool _checked;
        private ButtonEvents _currentEvent;
        private EventHandler _checkedStatusChanged;

        public ExButton() : base()
        {
            _enableCustomAppearance = true;
            _customAppearance = new ButtonUIAppearance(this);
            _currentEvent = ButtonEvents.NoEvent;
            _checked = false;
        }

        public event EventHandler CheckedStatusChanged
        {
            add { _checkedStatusChanged += value; }
            remove { _checkedStatusChanged -= value; }
        }

        [Description("If true control's is drawn using custom UI appearance, otherwise it appears using standard drawing."), Category("Custom Appearance"),
        DefaultValue(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool EnableCustomAppearance
        {
            get { return _enableCustomAppearance; }
            set
            {
                if (_enableCustomAppearance != value)
                {
                    _enableCustomAppearance = value;
                    if (_enableCustomAppearance)
                    {
                        AutoSize = false;
                    }
                }
            }
        }

        [Description("This property specifies the way of drawing the control."), Category("Custom Appearance"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ButtonUIAppearance CustomAppearance
        {
            get { return _customAppearance; }
        }

        [Description("Get or sets the checked state."), Category("Custom Appearance"),
        DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Checked
        {
            get { return _checked; }
            set
            {
                if (_checked != value)
                {
                    _checked = value;
                    Invalidate();
                    OnCheckedStatusChanged(new EventArgs());
                }
            }
        }

        protected virtual void OnCheckedStatusChanged(EventArgs args)
        {
            if (_checkedStatusChanged != null)
            {
                _checkedStatusChanged(this, args);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _currentEvent = ButtonEvents.MouseEnter;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _currentEvent = ButtonEvents.MouseLeave;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            _currentEvent = ButtonEvents.MouseDown;
            Invalidate();
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            _currentEvent = ButtonEvents.MouseUp;
            Invalidate();
            base.OnMouseUp(mevent);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            // Call base class' OnPaint.
            base.OnPaint(pevent);
            if (!_enableCustomAppearance)
            {
                return;
            }

            int offset = 1;
            Color borderColor;
            Color backColor;
            Color foreColor;

            switch (_currentEvent)
            {
                case ButtonEvents.MouseDown:
                    borderColor = _customAppearance.MouseDownBorderColor;
                    backColor = _customAppearance.MouseDownBackColor;
                    foreColor = _customAppearance.MouseDownForeColor;
                    break;
                case ButtonEvents.MouseEnter:
                case ButtonEvents.MouseUp:
                    borderColor = _customAppearance.MouseHoverBorderColor;
                    backColor = _customAppearance.MouseHoverBackColor;
                    foreColor = _customAppearance.MouseHoverForeColor;
                    break;
                default: // MouseLeave and NoEvent
                    if (_checked)
                    {
                        borderColor = _customAppearance.CheckedBorderColor;
                        backColor = _customAppearance.CheckedBackColor;
                        foreColor = _customAppearance.CheckedForeColor;
                    }
                    else
                    {
                        borderColor = _customAppearance.BorderColor;
                        backColor = _customAppearance.BackColor;
                        foreColor = ForeColor;
                    }
                    break;
            }

            Graphics graphics = pevent.Graphics;
            graphics.Clear(BackColor);
            // get Text measure according to selected Font
            SizeF stringMeasure = graphics.MeasureString(Text, Font);
            // Set graphics object to paint nice using antialias.
            if (_customAppearance.EnableAntiAlias)
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            }

            // ++ Calculate drawing string offsets ++
            int leftOffset = (int)(ClientRectangle.Width - stringMeasure.Width) / 2;
            int topOffset = (int)(ClientRectangle.Height - stringMeasure.Height) / 2 + offset;

            if (leftOffset < 0)
            {
                leftOffset = offset + Padding.Left;
            }
            else
            {
                leftOffset += Padding.Left;
            }
            if (topOffset < 0)
            {
                topOffset = offset + Padding.Top;
            }
            else
            {
                topOffset += Padding.Top;
            }
            // -- Calculate drawing string offsets --

            int drawingAngleSize = _customAppearance.BorderAngle + _customAppearance.BorderThicness;
            int drawingWidth = Width - _customAppearance.BorderThicness;
            int drawingHeight = Height - _customAppearance.BorderThicness;

            // Points used to draw and fill button rectangle
            Point[] points = new Point[9] 
			{ 
				new Point(drawingAngleSize, _customAppearance.BorderThicness),
				new Point(drawingWidth - _customAppearance.BorderAngle - _customAppearance.BorderThicness, _customAppearance.BorderThicness),
				new Point(drawingWidth - _customAppearance.BorderThicness, drawingAngleSize),
				new Point(drawingWidth - _customAppearance.BorderThicness, drawingHeight - _customAppearance.BorderAngle),
				new Point(drawingWidth - _customAppearance.BorderAngle - _customAppearance.BorderThicness, drawingHeight),
				new Point(drawingAngleSize, drawingHeight),
				new Point(_customAppearance.BorderThicness, drawingHeight - _customAppearance.BorderAngle),
				new Point(_customAppearance.BorderThicness, drawingAngleSize),
				new Point(drawingAngleSize, _customAppearance.BorderThicness)
			};

            graphics.FillPolygon(new SolidBrush(backColor), points);
            graphics.DrawPolygon(new Pen(borderColor, _customAppearance.BorderThicness), points);

            graphics.DrawString(Text, Font, new SolidBrush(foreColor), new Point(leftOffset, topOffset));
        }

        private enum ButtonEvents
        {
            NoEvent,
            MouseEnter,
            MouseLeave,
            MouseDown,
            MouseUp
        }
    }
}
