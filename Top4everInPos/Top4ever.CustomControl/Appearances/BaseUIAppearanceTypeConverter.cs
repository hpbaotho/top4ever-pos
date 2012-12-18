using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Top4ever.CustomControl.Appearances
{
    public class BaseUIAppearanceTypeConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            // if destination type is the type we expected try to convert
            if (destinationType == typeof(string))
            {
                BaseUIAppearance appearance = value as BaseUIAppearance;
                // if the value is of type appearance as we need it
                if (appearance != null)
                {
                    return string.Format("Back {0}; Border {1}; Thickness [{2}]", appearance.BackColor.ToString(),
                        appearance.BorderColor.ToString(), appearance.BorderThicness.ToString());
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
