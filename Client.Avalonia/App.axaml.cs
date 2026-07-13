using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Client.Avalonia.Models;
using Client.Avalonia.ViewModels;
using Client.Avalonia.Views;

namespace Client.Avalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var config = AppConfig.Load();
            var vm = new MainWindowViewModel(config);
            var win = new MainWindow
            {
                DataContext = vm,
            };
            win.Closing += (s, e) => vm.SaveConfig();
            desktop.MainWindow = win;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
