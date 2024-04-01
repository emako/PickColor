using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace PickColor.Controls;

public class ColorSlider : Slider
{
    static ColorSlider()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorSlider), new FrameworkPropertyMetadata(typeof(ColorSlider)));
    }

    public ColorSlider()
    {
        Loaded += ColorSlider_Loaded;
    }

    private void ColorSlider_Loaded(object sender, RoutedEventArgs e)
    {
        if (Template?.FindName("PART_Track", this) is Track track)
        {
            Thumb thumb = track.Thumb;
            thumb.MouseEnter += new MouseEventHandler(Thumb_MouseEnter);
        }
    }

    private void Thumb_MouseEnter(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed
            && e.MouseDevice.Captured == null)
        {
            MouseButtonEventArgs args = new(e.MouseDevice, e.Timestamp, MouseButton.Left)
            {
                RoutedEvent = MouseLeftButtonDownEvent,
            };
            (sender as Thumb)?.RaiseEvent(args);
        }
    }
}
