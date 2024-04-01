using System;
using System.Globalization;
using System.Windows.Data;

namespace PickColor.Converters;

[ValueConversion(typeof(System.Drawing.Color), typeof(System.Windows.Media.Brush))]
public sealed class ColorToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is System.Drawing.Color color1)
        {
            if (color1 == System.Drawing.Color.Transparent)
            {
                return System.Windows.Media.Brushes.Transparent;
            }
            return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(color1.A, color1.R, color1.G, color1.B));
        }
        else if (value is System.Windows.Media.Color color2)
        {
            if (color2 == System.Windows.Media.Colors.Transparent)
            {
                return System.Windows.Media.Brushes.Transparent;
            }
            return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(color2.A, color2.R, color2.G, color2.B));
        }

        return System.Windows.Media.Brushes.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
