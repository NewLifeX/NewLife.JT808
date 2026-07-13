using System.Reflection;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Client.Avalonia.ViewModels;

namespace Client.Avalonia;

/// <summary>ReactiveUI 视图定位器 — 按约定定位 Views/{ViewModelName}View</summary>
public class ViewLocator : IDataTemplate
{
    public Control? Build(Object? param)
    {
        if (param is null) return null;

        var name = param.GetType().FullName!.Replace("ViewModel", "View");
        var type = Assembly.GetExecutingAssembly().GetType(name);

        if (type is not null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = $"视图未找到: {name}" };
    }

    public Boolean Match(Object? data) => data is ViewModelBase;
}
