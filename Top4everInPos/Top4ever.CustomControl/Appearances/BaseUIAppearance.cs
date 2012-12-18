using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Top4ever.CustomControl.Appearances
{
    [TypeConverter(typeof(BaseUIAppearanceTypeConverter))]
    public class BaseUIAppearance
    {
        private Control _owner;
        private Color _borderColor;
        private Color _backColor;
        private int _borderThicness;
        private bool _enableAntiAlias;

        protected Control Owner
        {
            get
            {
                return _owner;
            }
        }

        [Description("Gets or Sets control's border color. It can not be set to Color.Transparent."), Category("Custom Appearance"), DefaultValue(typeof(Color), "Black")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                if (value.Equals(Color.Transparent))
                {
                    throw new ArgumentOutOfRangeException("BorderColor", "Parameter cannot be set to Color.Transparent");
                }
                if (!_borderColor.Equals(value))
                {
                    _borderColor = value;
                    _owner.Invalidate();
                }
            }
        }

        [Description("Gets or Sets control's background color. It can not be set to Color.Transparent."), Category("Custom Appearance"), DefaultValue(typeof(Color), "White")]
        public Color BackColor
        {
            get { return _backColor; }
            set
            {
                if (value.Equals(Color.Transparent))
                {
                    throw new ArgumentOutOfRangeException("BackColor", "Parameter cannot be set to Color.Transparent");
                }
                if (!_backColor.Equals(value))
                {
                    _backColor = value;
                    _owner.Invalidate();
                }
            }
        }

        [Description("Gets or Sets control's border thickness."), Category("Custom Appearance"), DefaultValue(1)]
        public int BorderThicness
        {
            get { return _borderThicness; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("BorderThicness", "Parameter must be set to a valid integer value hihger than 0.");
                }
                if (_borderThicness != value)
                {
                    _borderThicness = value;
                    _owner.Invalidate();
                }
            }
        }

        [Description("If true control's is drawn using antialiasing, otherwise antialiasing is not enabled."), Category("Custom Appearance"), DefaultValue(true)]
        public bool EnableAntiAlias
        {
            get { return _enableAntiAlias; }
            set
            {
                if (_enableAntiAlias != value)
                {
                    _enableAntiAlias = value;
                    _owner.Invalidate();
                }
            }
        }

        public BaseUIAppearance(Control owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }
            _owner = owner;
            _borderColor = Color.Black;
            _backColor = Color.White;
            _borderThicness = 1;
            _enableAntiAlias = true;
        }
    }
}
