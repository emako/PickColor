using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PickColor.Controls.Core;
using PickColor.Core;
using PickColor.Helpers;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace PickColor.ViewModels;

public sealed partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string version = $"v{Assembly.GetExecutingAssembly().GetName().Version}";

    private ColorCapturer cap = null!;

    [ObservableProperty]
    private bool hasColor = false;

    [ObservableProperty]
    private bool isRunning = false;

    [ObservableProperty]
    private bool isUserHandling = true;

    [ObservableProperty]
    private bool isUseAlpha = false;

    [ObservableProperty]
    private Image capImage = null!;

    [ObservableProperty]
    private string assColor = "&H000000&";

    partial void OnAssColorChanged(string value)
    {
        if (IsUserHandling)
        {
            if (ColorTransformer.TryParseAss(value, out byte r, out byte g, out byte b, out byte? _))
            {
                Red = r;
                Green = g;
                Blue = b;
                UpdateColorProperty();
            }
        }
    }

    [ObservableProperty]
    private string htmlColor = "#000000";

    partial void OnHtmlColorChanged(string value)
    {
        if (IsUserHandling)
        {
            if (ColorTransformer.TryParseHtml(value, out byte r, out byte g, out byte b, out byte? _))
            {
                Red = r;
                Green = g;
                Blue = b;
                UpdateColorProperty();
            }
        }
    }

    [ObservableProperty]
    private string rgbColor = "0,0,0";

    partial void OnRgbColorChanged(string value)
    {
        if (IsUserHandling)
        {
            if (ColorTransformer.TryParseRgb(value, out byte r, out byte g, out byte b, out byte? _))
            {
                Red = r;
                Green = g;
                Blue = b;
                UpdateColorProperty();
            }
        }
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Color))]
    private byte red = default;

    partial void OnRedChanged(byte value) => UpdateColorProperty();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Color))]
    private byte green = default;

    partial void OnGreenChanged(byte value) => UpdateColorProperty();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Color))]
    private byte blue = default;

    partial void OnBlueChanged(byte value) => UpdateColorProperty();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Color))]
    private byte alpha = 0xFF;

    public Color Color => HasColor ? Color.FromArgb(Alpha, Red, Green, Blue) : Color.Transparent;

    public MainViewModel()
    {
        cap = App.Current.GetService<ColorCapturer>();
        cap.ColorReceived += ColorReceived;
        cap.ColorRegionReceived += ColorRegionReceived;
        cap.Stopped += Stopped;
    }

    public void Start()
    {
        IsRunning = true;
        cap.Start();
    }

    public void TestHit()
    {
        cap.TestHit();
    }

    private void UpdateColorProperty()
    {
        HasColor = true;
        IsUserHandling = false;
        HtmlColor = $"#{Red:X2}{Green:X2}{Blue:X2}";
        AssColor = $"&H{Blue:X2}{Green:X2}{Red:X2}&";
        RgbColor = $"{Red},{Green},{Blue}";
        IsUserHandling = true;
    }

    private void ColorReceived(object sender, Color e)
    {
        Debug.WriteLine(e.ToString());
        UpdateColorProperty();
        HasColor = true;
        IsUserHandling = false;
        Red = e.R;
        Green = e.G;
        Blue = e.B;
        UpdateColorProperty();
        IsUserHandling = true;
    }

    private void ColorRegionReceived(object sender, Bitmap e)
    {
        CapImage = e?.ZoomIn(20d)!;
    }

    private void Stopped(object sender, object e)
    {
        IsRunning = false;
    }

    [RelayCommand]
    public async Task OpenAbout(FrameworkElement ui)
    {
        Wpf.Ui.Controls.MessageBox box = new()
        {
            Title = "About",
            Content = ui,
            Owner = Application.Current.MainWindow,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
        };
        _ = await box.ShowDialogAsync(false);
    }

    [RelayCommand]
    public void OpenHomepage()
    {
        _ = Process.Start("https://github.com/emako/PickColor");
    }

    [RelayCommand]
    public void CreateStartMenuShortcut()
    {
        ShortcutHelper.CreateStartMenuShortcut("PickColor", Assembly.GetEntryAssembly().Location);
    }
}

file static class ImageExtension
{
    public static Image ZoomIn(this Image image, double factor = 2d)
    {
        int originalWidth = image.Width;
        int originalHeight = image.Height;

        Bitmap enlargedBitmap = new((int)(originalWidth * factor), (int)(originalHeight * factor));

        using Graphics g = Graphics.FromImage(enlargedBitmap);
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        g.DrawImage(image, 0, 0, (int)(originalWidth * factor), (int)(originalHeight * factor));

        return enlargedBitmap;
    }
}
