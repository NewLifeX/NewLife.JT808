using Avalonia;
using Avalonia.ReactiveUI;
using NewLife.Log;
using NewLife.Model;
using Stardust;

namespace Client.Avalonia;

internal static class Program
{
    [STAThread]
    static void Main(String[] args)
    {
        XTrace.UseConsole();

        var services = ObjectContainer.Current;
        services.AddStardust();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
}
