using System;
using System.Drawing;
using System.Windows.Forms;
using Vanara.PInvoke;

namespace PickColor.Helpers;

internal static class CursorHelper
{
    public static bool SetSystemCursor(Bitmap cursorImage, Point hotPoint)
    {
        User32.SafeHCURSOR cursorHandle = new(cursorImage.ToCursor(hotPoint).Handle);

        if (!cursorHandle.IsNull)
        {
            return User32.SetSystemCursor((HCURSOR)cursorHandle.DangerousGetHandle(), User32.OCR.OCR_NORMAL);
        }
        return false;
    }

    public static bool RestoreSystemCursor()
    {
        return User32.SystemParametersInfo(User32.SPI.SPI_SETCURSORS, 0, IntPtr.Zero, User32.SPIF.SPIF_SENDCHANGE);
    }
}

file static class IconExtension
{
    public static Cursor ToCursor(this Bitmap bitmap, Point hotPoint)
    {
        using Bitmap myNewCursor = new(bitmap.Width * 2 - hotPoint.X, bitmap.Height * 2 - hotPoint.Y);
        using Graphics g = Graphics.FromImage(myNewCursor);

        g.Clear(Color.FromArgb(0, 0, 0, 0));
        g.DrawImage(bitmap, bitmap.Width - hotPoint.X, bitmap.Height - hotPoint.Y, bitmap.Width, bitmap.Height);
        return new Cursor(myNewCursor.GetHicon());
    }
}
