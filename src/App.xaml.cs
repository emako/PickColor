using Microsoft.Extensions.DependencyInjection;
using PickColor.Controls.Core;
using PickColor.Helpers;
using System.Windows;

namespace PickColor;

public partial class App : Application
{
    public new static App Current => (Application.Current as App)!;
    public ServiceProvider ServiceProvider { get; }

    public App()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<ColorCapturer>();
        ServiceProvider = services.BuildServiceProvider();

        _ = DpiAwareHelper.SetProcessDpiAwareness();
        InitializeComponent();
    }

    public T GetService<T>() where T : class
    {
        return ServiceProvider.GetService<T>()!;
    }
}
