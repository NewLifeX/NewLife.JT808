using System.Reactive;
using System.Reactive.Linq;
using Client.Avalonia.Models;
using Client.Avalonia.Services;
using NewLife.JT808.Protocols;
using ReactiveUI;

namespace Client.Avalonia.ViewModels;

/// <summary>连接管理 ViewModel — TCP 连接配置与控制</summary>
public class ConnectionViewModel : ViewModelBase
{
    private String _serverAddress = "127.0.0.1";
    private Int32 _serverPort = 808;
    private String _mobile = "013800123456";
    private Boolean _isConnected;
    private String _statusText = "未连接";
    private Int64 _sentCount;
    private Int64 _receivedCount;
    private Int64 _reconnectCount;
    private readonly IConnectionService _service;
    private readonly MessageLogViewModel _log;
    private IDisposable? _pollTimer;

    /// <summary>服务端地址</summary>
    public String ServerAddress
    {
        get => _serverAddress;
        set => this.RaiseAndSetIfChanged(ref _serverAddress, value);
    }

    /// <summary>服务端端口</summary>
    public Int32 ServerPort
    {
        get => _serverPort;
        set => this.RaiseAndSetIfChanged(ref _serverPort, value);
    }

    /// <summary>终端手机号</summary>
    public String Mobile
    {
        get => _mobile;
        set => this.RaiseAndSetIfChanged(ref _mobile, value);
    }

    /// <summary>是否已连接</summary>
    public Boolean IsConnected
    {
        get => _isConnected;
        set
        {
            this.RaiseAndSetIfChanged(ref _isConnected, value);
            StatusText = value ? "已连接" : "未连接";
        }
    }

    /// <summary>状态文本</summary>
    public String StatusText
    {
        get => _statusText;
        set => this.RaiseAndSetIfChanged(ref _statusText, value);
    }

    /// <summary>已发送消息数</summary>
    public Int64 SentCount
    {
        get => _sentCount;
        set => this.RaiseAndSetIfChanged(ref _sentCount, value);
    }

    /// <summary>已接收消息数</summary>
    public Int64 ReceivedCount
    {
        get => _receivedCount;
        set => this.RaiseAndSetIfChanged(ref _receivedCount, value);
    }

    /// <summary>重连次数</summary>
    public Int64 ReconnectCount
    {
        get => _reconnectCount;
        set => this.RaiseAndSetIfChanged(ref _reconnectCount, value);
    }

    /// <summary>连接命令</summary>
    public ReactiveCommand<Unit, Unit> ConnectCommand { get; }

    /// <summary>断开命令</summary>
    public ReactiveCommand<Unit, Unit> DisconnectCommand { get; }

    /// <summary>内部连接服务（供 DeviceViewModel 使用）</summary>
    public IConnectionService Service => _service;

    public ConnectionViewModel(MessageLogViewModel log, AppConfig? config = null)
    {
        _log = log;
        _service = new ConnectionService();

        if (config != null)
        {
            _serverAddress = config.ServerAddress;
            _serverPort = config.ServerPort;
            _mobile = config.Mobile;
        }

        ConnectCommand = ReactiveCommand.Create(DoConnect);
        DisconnectCommand = ReactiveCommand.Create(DoDisconnect);

        _service.ConnectionChanged += (_, connected) =>
        {
            IsConnected = connected;
            if (connected)
            {
                StartPollStats();
            }
            else
            {
                StopPollStats();
            }
        };

        _service.MessageReceived += (_, msg) =>
        {
            _log.AddEntry(msg, "↓");
            RefreshStats();
        };

        _service.MessageSent += (_, msg) =>
        {
            _log.AddEntry(msg, "↑");
            RefreshStats();
        };
    }

    private void DoConnect()
    {
        _service.Connect(ServerAddress, ServerPort, Mobile);
    }

    private void DoDisconnect()
    {
        _service.Disconnect();
    }

    private void StartPollStats()
    {
        StopPollStats();
        _pollTimer = Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ => RefreshStats());
    }

    private void StopPollStats()
    {
        _pollTimer?.Dispose();
        _pollTimer = null;
    }

    private void RefreshStats()
    {
        SentCount = _service.SentCount;
        ReceivedCount = _service.ReceivedCount;
        ReconnectCount = _service.ReconnectCount;
    }
}
