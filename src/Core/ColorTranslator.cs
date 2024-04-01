using System.Drawing;
using System.Text.RegularExpressions;

namespace PickColor.Core;

internal static class ColorTransformer
{
    public static bool TryParseRgb(string rgbColor, out byte r, out byte g, out byte b, out byte? a)
    {
        if (!string.IsNullOrWhiteSpace(rgbColor))
        {
            rgbColor = rgbColor.Replace(" ", ",");
            string[] rgb = rgbColor.Split(',');

            if (rgb.Length >= 3
             && byte.TryParse(rgb[0], out byte red)
             && byte.TryParse(rgb[1], out byte green)
             && byte.TryParse(rgb[2], out byte blue))
            {
                r = red;
                g = green;
                b = blue;
                a = null!;
                return true;
            }
        }

        r = g = b = default;
        a = null!;
        return false;
    }

    public static bool TryParseHtml(string htmlColor, out byte r, out byte g, out byte b, out byte? a)
    {
        if (!string.IsNullOrWhiteSpace(htmlColor))
        {
            Regex regex = new("^#([A-Fa-f0-9]{6})$", RegexOptions.IgnoreCase);

            if (regex.IsMatch(htmlColor))
            {
                Color color = ColorTranslator.FromHtml(htmlColor);

                r = color.R;
                g = color.G;
                b = color.B;
                a = null!;
                return true;
            }
        }

        r = g = b = default;
        a = null!;
        return false;
    }

    public static bool TryParseAss(string assColor, out byte r, out byte g, out byte b, out byte? a)
    {
        if (!string.IsNullOrWhiteSpace(assColor))
        {
            Regex regex = new("^&H([A-Fa-f0-9]{6})&$", RegexOptions.IgnoreCase);

            if (regex.IsMatch(assColor))
            {
                assColor = assColor.Replace("&", string.Empty).Replace("H", string.Empty)!;
                return TryParseHtml($"#{assColor[4..6]}{assColor[2..4]}{assColor[0..2]}", out r, out g, out b, out a);
            }
        }

        r = g = b = default;
        a = null!;
        return false;
    }
}
