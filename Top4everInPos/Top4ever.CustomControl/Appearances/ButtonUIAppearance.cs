using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Top4ever.CustomControl.Appearances
{
    [TypeConverter(typeof(BaseUIAppearanceTypeConverter))]
    public class ButtonUIAppearance : BaseUIAppearance
    {
		private Color _mouseHoverBorderColor;
		private Color _mouseHoverBackColor;
		private Color _mouseHoverForeColor;
		private Color _mouseDownBorderColor;
		private Color _mouseDownBackColor;
		private Color _mouseDownForeColor;
		private Color _checkedBorderColor;
		private Color _checkedBackColor;
		private Color _checkedForeColor;
		private int _borderAngle;

		[Description("Gets or sets button's border color when mouse hovers the button."), 
		 Category("Custom Appearance"), DefaultValue(typeof(Color), "Black")]
		public Color MouseHoverBorderColor
		{
			get { return _mouseHoverBorderColor; }
			set 
			{
				if (value.Equals(Color.Transparent))
				{
					throw new ArgumentOutOfRangeException("MouseHoverBorderColor", "Parameter cannot be set to Color.Transparent");
				}
				if (!_mouseHoverBorderColor.Equals(value))
				{
					_mouseHoverBorderColor = value; 
					Owner.Invalidate();
				}				 				
			}
		}

		[Description("Gets or sets button's background color when mouse hovers the button."),
		 Category("Custom Appearance"), DefaultValue(typeof(Color), "White")]
		public Color MouseHoverBackColor
		{
			get { return _mouseHoverBackColor; }
			set
			{
				if (value.Equals(Color.Transparent))
				{
					throw new ArgumentOutOfRangeException("MouseHoverBackColor", "Parameter cannot be set to Color.Transparent");
				}
				if (!_mouseHoverBackColor.Equals(value))
				{
					_mouseHoverBackColor = value;
					Owner.Invalidate();
				}
			}
		}

		[Description("Gets or sets button's fore color when mouse hovers the button."),
		 Category("Custom Appearance"), DefaultValue(typeof(Color), "Black")]
		public Color MouseHoverForeColor
		{
			get { return _mouseHoverForeColor; }
			set
			{
				if (value.Equals(Color.Transparent))
				{
					throw new ArgumentOutOfRangeException("MouseHoverForeColor", "Parameter cannot be set to Color.Transparent");
				}
				if (!_mouseHoverForeColor.Equals(value))
				{
					_mouseHoverForeColor = value;
					Owner.Invalidate();
				}
			}
		}

		[Description("Gets or sets button's border color when mouse down."),
		 Category("Custom Appearance"), DefaultValue(typeof(Color), "Black")]
		public Color MouseDownBorderColor
		{
			get { return _mouseDownBorderColor; }
			set
			{
				if (value.Equals(Color.Transparent))
				{
					throw new ArgumentOutOfRangeException("MouseDownBorderColor", "Parameter cannot be set to Color.Transparent");
				}
				if (!_mouseDownBorderColor.Equals(value))
				{
					_mouseDownBorderColor = value;
					Owner.Invalidate();
				}
			}
		}

		[Description("Gets or sets button's background color when mouse down."),
		 Category("Custom Appearance"), DefaultValue(typeof(Color), "White")]
		public Color MouseDownBackColor
		{
			get { return _mouseDownBackColor; }
			set
			{
				if (value.Equals(Color.Transparent))
				{
					throw new ArgumentOutOfRangeException("MouseDownBackColor", "Parameter cannot be set to Color.Transparent");
				}
				if (!_mouseDownBackColor.Equals(value))
				{
					_mouseDownBackColor = value;
					Owner.Invalidate();
				}
			}
		}

		[Description("Gets or sets button's fore color when mouse down."),
		 Category("Custom Appearance"), DefaultValue(typeof(Color), "Black")]
		public Color MouseDownForeColor
		{
			get { return _mouseDownForeColor; }
			set
			{
				if (value.Equals(Color.Transparent))
				{
					throw new ArgumentOutOfRangeException("MouseDownForeColor", "Parameter cannot be set to Color.Transparent");
				}
				if (!_mouseDownForeColor.Equals(value))
				{
					_mouseDownForeColor = value;
					Owner.Invalidate();
				}
			}
		}

		[Description("Gets or sets button's border color when its state is checked."),
		 Category("Custom Appearance"), DefaultValue(typeof(Color), "Black")]
		public Color CheckedBorderColor
		{
			get { return _checkedBorderColor; }
			set
			{
				if (value.Equals(Color.Transparent))
				{
					throw new ArgumentOutOfRangeException("CheckedBorderColor", "Parameter cannot be set to Color.Transparent");
				}
				if (!_checkedBorderColor.Equals(value))
				{
					_checkedBorderColor = value;
					Owner.Invalidate();
				}
			}
		}

		[Description("Gets or sets button's background color when its state is checked."),
		 Category("Custom Appearance"), DefaultValue(typeof(Color), "White")]
		public Color CheckedBackColor
		{
			get { return _checkedBackColor; }
			set
			{
				if (value.Equals(Color.Transparent))
				{
					throw new ArgumentOutOfRangeException("CheckedBackColor", "Parameter cannot be set to Color.Transparent");
				}
				if (!_checkedBackColor.Equals(value))
				{
					_checkedBackColor = value;
					Owner.Invalidate();
				}
			}
		}

		[Description("Gets or sets button's fore color when its state is checked."),
		 Category("Custom Appearance"), DefaultValue(typeof(Color), "Black")]
		public Color CheckedForeColor
		{
			get { return _checkedForeColor; }
			set
			{
				if (value.Equals(Color.Transparent))
				{
					throw new ArgumentOutOfRangeException("CheckedForeColor", "Parameter cannot be set to Color.Transparent");
				}
				if (!_checkedForeColor.Equals(value))
				{
					_checkedForeColor = value;
					Owner.Invalidate();
				}
			}
		}

		[Description("Gets or Sets button border's angle."), Category("Custom Appearance"), DefaultValue(3)]
		public int BorderAngle
		{
			get { return _borderAngle; }
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("BorderAngle", "Parameter must be set to a valid integer value hihger than or equal to 0.");
				}
				if (_borderAngle != value)
				{
					_borderAngle = value;
					Owner.Invalidate();
				}
			}
		}

		public ButtonUIAppearance(Control owner) : base(owner)
		{
			_mouseHoverBorderColor = Color.Black;
			_mouseHoverBackColor = Color.White;
			_mouseHoverForeColor = Color.Black;
			_mouseDownBorderColor = Color.Black;
			_mouseDownBackColor = Color.White;
			_mouseDownForeColor = Color.Black;
			_checkedBorderColor = Color.Black;
			_checkedBackColor = Color.White;
			_checkedForeColor = Color.Black;
			_borderAngle = 3;
		}
	}
}
