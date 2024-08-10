using Gma.System.MouseKeyHook;
using PickColor.Helpers;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PickColor.Controls.Core;

public sealed class ColorCapturer : IDisposable
{
    public bool IsRunning { get; private set; } = false;
    private IKeyboardMouseEvents globalMouseHook = null!;
    private int x = default;
    private int y = default;

    private Bitmap buffer = new(1, 1);
    private Graphics graphics = null!;

    private Bitmap bufferRegion = new(10, 10);
    private Graphics graphicsRegion = null!;

    public event EventHandler<Color> ColorReceived = null!;

    public event EventHandler<Bitmap> ColorRegionReceived = null!;

    public event EventHandler Stopped = null!;

    public ColorCapturer()
    {
        graphics = Graphics.FromImage(buffer);
        graphicsRegion = Graphics.FromImage(bufferRegion);

        globalMouseHook = Hook.GlobalEvents();
        globalMouseHook.KeyUp -= OnGlobalKeyHookKeyUp;
        globalMouseHook.KeyUp += OnGlobalKeyHookKeyUp;
        globalMouseHook.MouseMove -= OnGlobalMouseHookMouseMove;
        globalMouseHook.MouseMove += OnGlobalMouseHookMouseMove;
        globalMouseHook.MouseUp -= OnGlobalMouseHookMouseUp;
        globalMouseHook.MouseUp += OnGlobalMouseHookMouseUp;
    }

    public void Dispose()
    {
        if (globalMouseHook != null)
        {
            globalMouseHook.KeyUp -= OnGlobalKeyHookKeyUp;
            globalMouseHook.MouseMove -= OnGlobalMouseHookMouseMove;
            globalMouseHook.MouseUp -= OnGlobalMouseHookMouseUp;
            globalMouseHook = null!;
        }

        if (graphics != null)
        {
            graphics.Dispose();
            graphics = null!;
        }

        if (buffer != null)
        {
            buffer.Dispose();
            buffer = null!;
        }

        if (bufferRegion != null)
        {
            bufferRegion.Dispose();
            bufferRegion = null!;
        }

        if (graphicsRegion != null)
        {
            graphicsRegion.Dispose();
            graphicsRegion = null!;
        }
    }

    public void Start()
    {
        IsRunning = true;
        using Stream stream = ResourceHelper.GetStream("pack://application:,,,/Assets/Images/Eyedropper.png");
        using Bitmap bitmap = new(stream);
        CursorHelper.SetSystemCursor(bitmap, new Point(0, bitmap.Height));
    }

    public void Stop()
    {
        IsRunning = false;
        CursorHelper.RestoreSystemCursor();
    }

    public void TestHit()
    {
        graphics?.CopyFromScreen(x, y, 0, 0, new Size(1, 1));
        if (buffer != null)
        {
            ColorReceived?.Invoke(this, buffer.GetPixel(0, 0));
        }
    }

    private void OnGlobalKeyHookKeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            Stop();
            Stopped?.Invoke(this, null!);
        }
    }

    private void OnGlobalMouseHookMouseMove(object sender, MouseEventArgs e)
    {
        x = e.X;
        y = e.Y;

        if (!IsRunning)
        {
            return;
        }

        graphics?.CopyFromScreen(x + 3, y + 11, 0, 0, new Size(1, 1));
        if (buffer != null)
        {
            ColorReceived?.Invoke(this, buffer.GetPixel(0, 0));
        }

        graphicsRegion?.CopyFromScreen(x - 2, y + 6, 0, 0, new Size(10, 10));
        if (bufferRegion != null)
        {
            ColorRegionReceived?.Invoke(this, bufferRegion);
        }
    }

    private void OnGlobalMouseHookMouseUp(object sender, MouseEventArgs e)
    {
        if (IsRunning)
        {
            Stop();
            Stopped?.Invoke(this, null!);
        }
    }
}
