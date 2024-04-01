using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Vanara.PInvoke;

namespace PickColor.Converters;

[ValueConversion(typeof(Bitmap), typeof(ImageSource))]
public sealed class BitmapToImageSourceConverter : IValueConverter
{
    public static BitmapToImageSourceConverter Instance => new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Bitmap bitmap)
        {
            return bitmap.ToImageSource();
        }
        return null!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

file static class BitmapExtension
{
    public static ImageSource ToImageSource(this Bitmap bitmap)
    {
        nint hBitmap = bitmap.GetHbitmap();
        ImageSource imageSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        _ = Gdi32.DeleteObject(hBitmap);
        return imageSource;
    }
}
