using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace PickColor.Controls;

public class SvgIconElement : IconElement
{
    public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
        nameof(Data), typeof(string), typeof(SvgIconElement), new PropertyMetadata(string.Empty, OnDataPropertyChanged));

    public string Data
    {
        get => (string)GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    static SvgIconElement()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SvgIconElement), new FrameworkPropertyMetadata(typeof(SvgIconElement)));
    }

    protected override UIElement InitializeChildren()
    {
        var path = new Path
        {
            Data = Geometry.Parse(Data),
            Fill = Foreground,
            Width = Width,
            Height = Height,
            Stretch = Stretch.Uniform,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        return path;
    }

    private static void OnDataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var iconElement = (SvgIconElement)d;
        iconElement.InvalidateVisual();
    }
}
