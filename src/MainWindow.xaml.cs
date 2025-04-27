using PickColor.ViewModels;
using System;
using System.Windows.Input;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace PickColor;

public partial class MainWindow : FluentWindow
{
    public MainViewModel ViewModel { get; }

    public MainWindow()
    {
        DataContext = ViewModel = new();
        InitializeComponent();

        HtmlTextBlock.MouseLeftButtonDown += OnHtmlMouseLeftButtonDown;
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        if (WindowBackdrop.IsSupported(WindowBackdropType.Mica))
        {
            Background = new SolidColorBrush(Colors.Transparent);
            WindowBackdrop.ApplyBackdrop(this, WindowBackdropType.Mica);
        }
    }

    private void OnEyedropButtonMouseDown(object sender, MouseButtonEventArgs e)
    {
        ViewModel.Start();
    }

    private void OnRegionButtonMouseDown(object sender, MouseButtonEventArgs e)
    {
        ViewModel.TestHit();
    }

    private void OnHtmlMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        ViewModel.IsUpper = !ViewModel.IsUpper;
        HtmlTextBlock.Text = ViewModel.IsUpper ? "HTML" : "html";
    }
}
