using Client.Avalonia.Models;
using ReactiveUI;

namespace Client.Avalonia.ViewModels;

/// <summary>主窗口 ViewModel — 管理标签页导航</summary>
public class MainWindowViewModel : ViewModelBase
{
    private Int32 _selectedTab;
    private String _title = "JT808 测试客户端";
    private readonly AppConfig _config;

    /// <summary>窗口标题</summary>
    public String Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    /// <summary>当前选中的标签页索引</summary>
    public Int32 SelectedTab
    {
        get => _selectedTab;
        set => this.RaiseAndSetIfChanged(ref _selectedTab, value);
    }

    /// <summary>连接管理</summary>
    public ConnectionViewModel Connection { get; }

    /// <summary>终端模拟</summary>
    public DeviceViewModel Device { get; }

    /// <summary>消息日志</summary>
    public MessageLogViewModel MessageLog { get; }

    /// <summary>服务端管理</summary>
    public ServerViewModel Server { get; }

    /// <summary>指令解析</summary>
    public ParserViewModel Parser { get; }

    public MainWindowViewModel(AppConfig config)
    {
        _config = config;

        var log = new MessageLogViewModel();
        Connection = new ConnectionViewModel(log, config);
        Device = new DeviceViewModel(Connection, log);
        MessageLog = log;
        Server = new ServerViewModel(Connection, config);
        Parser = new ParserViewModel();
    }

    /// <summary>保存配置到文件</summary>
    public void SaveConfig()
    {
        _config.ServerAddress = Connection.ServerAddress;
        _config.ServerPort = Connection.ServerPort;
        _config.Mobile = Connection.Mobile;
        _config.ApiAddress = Server.ApiAddress;
        _config.ApiToken = Server.ApiToken;

        _config.Save();
    }
}
