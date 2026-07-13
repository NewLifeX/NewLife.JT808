using Avalonia;
using Avalonia.ReactiveUI;

namespace Client.Avalonia;

internal static class Program
{
    [STAThread]
    static void Main(String[] args) => BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

    static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
}
