using System.Reactive;
using System.Collections.ObjectModel;
using Client.Avalonia.Models;
using Client.Avalonia.Services;
using ReactiveUI;

namespace Client.Avalonia.ViewModels;

/// <summary>服务端管理 ViewModel — Web API 会话/统计/黑名单</summary>
public class ServerViewModel : ViewModelBase
{
    private readonly ConnectionViewModel _connection;
    private readonly ServerApiService _api;
    private String _apiAddress;
    private String _apiToken;
    private String _apiStatus = "";

    #region 统计属性
    private String _statsServer = "";
    private Boolean _statsRunning;
    private String _statsStarted = "";

    public String StatsServer { get => _statsServer; set => this.RaiseAndSetIfChanged(ref _statsServer, value); }
    public Boolean StatsRunning { get => _statsRunning; set => this.RaiseAndSetIfChanged(ref _statsRunning, value); }
    public String StatsStarted { get => _statsStarted; set => this.RaiseAndSetIfChanged(ref _statsStarted, value); }
    #endregion

    #region 会话属性
    private readonly ObservableCollection<SessionInfo> _sessions = new();
    public ObservableCollection<SessionInfo> Sessions => _sessions;

    private Int32 _sessionTotal;
    public Int32 SessionTotal { get => _sessionTotal; set => this.RaiseAndSetIfChanged(ref _sessionTotal, value); }
    #endregion

    #region 黑名单属性
    private readonly ObservableCollection<String> _blacklist = new();
    public ObservableCollection<String> Blacklist => _blacklist;

    private String _newBlacklistMobile = "";
    public String NewBlacklistMobile { get => _newBlacklistMobile; set => this.RaiseAndSetIfChanged(ref _newBlacklistMobile, value); }
    #endregion

    public String ApiAddress
    {
        get => _apiAddress;
        set
        {
            this.RaiseAndSetIfChanged(ref _apiAddress, value);
            _api.BaseUrl = value;
        }
    }

    public String ApiToken
    {
        get => _apiToken;
        set
        {
            this.RaiseAndSetIfChanged(ref _apiToken, value);
            _api.Token = value;
        }
    }

    public String ApiStatus
    {
        get => _apiStatus;
        set => this.RaiseAndSetIfChanged(ref _apiStatus, value);
    }

    public ReactiveCommand<Unit, Unit> RefreshStatsCommand { get; }
    public ReactiveCommand<Unit, Unit> RefreshSessionsCommand { get; }
    public ReactiveCommand<Unit, Unit> RefreshBlacklistCommand { get; }
    public ReactiveCommand<Unit, Unit> AddBlacklistCommand { get; }
    public ReactiveCommand<String, Unit> RemoveBlacklistCommand { get; }
    public ReactiveCommand<String, Unit> RemoveSessionCommand { get; }

    public ServerViewModel(ConnectionViewModel connection, AppConfig? config = null)
    {
        _connection = connection;
        _api = new ServerApiService();

        _apiAddress = config?.ApiAddress ?? "http://127.0.0.1:5000";
        _apiToken = config?.ApiToken ?? "";
        _api.BaseUrl = _apiAddress;
        _api.Token = _apiToken;

        RefreshStatsCommand = ReactiveCommand.CreateFromTask(DoRefreshStats);
        RefreshSessionsCommand = ReactiveCommand.CreateFromTask(DoRefreshSessions);
        RefreshBlacklistCommand = ReactiveCommand.CreateFromTask(DoRefreshBlacklist);
        AddBlacklistCommand = ReactiveCommand.CreateFromTask(DoAddBlacklist);
        RemoveBlacklistCommand = ReactiveCommand.CreateFromTask<String>(DoRemoveBlacklist);
        RemoveSessionCommand = ReactiveCommand.CreateFromTask<String>(DoRemoveSession);
    }

    private async Task DoRefreshStats()
    {
        try
        {
            ApiStatus = "正在获取统计...";
            var stats = await _api.GetStatsAsync();
            if (stats != null)
            {
                StatsServer = stats.Server;
                StatsRunning = stats.Running;
                StatsStarted = stats.Started;
                ApiStatus = "统计已刷新";
            }
            else
            {
                ApiStatus = "获取统计失败";
            }
        }
        catch (Exception ex)
        {
            ApiStatus = $"错误: {ex.Message}";
        }
    }

    private async Task DoRefreshSessions()
    {
        try
        {
            ApiStatus = "正在获取会话...";
            var list = await _api.GetSessionsAsync();
            if (list != null)
            {
                SessionTotal = list.Total;
                _sessions.Clear();
                ApiStatus = $"会话已刷新 (共 {list.Total} 个)";
            }
            else
            {
                ApiStatus = "获取会话失败";
            }
        }
        catch (Exception ex)
        {
            ApiStatus = $"错误: {ex.Message}";
        }
    }

    private async Task DoRefreshBlacklist()
    {
        try
        {
            ApiStatus = "正在获取黑名单...";
            var info = await _api.GetBlacklistAsync();
            if (info != null)
            {
                _blacklist.Clear();
                foreach (var item in info.Items)
                    _blacklist.Add(item);
                ApiStatus = $"黑名单已刷新 (共 {info.Total} 个)";
            }
            else
            {
                ApiStatus = "获取黑名单失败";
            }
        }
        catch (Exception ex)
        {
            ApiStatus = $"错误: {ex.Message}";
        }
    }

    private async Task DoAddBlacklist()
    {
        if (String.IsNullOrEmpty(NewBlacklistMobile)) return;

        try
        {
            var result = await _api.AddBlacklistAsync(NewBlacklistMobile);
            ApiStatus = result ? "已加入黑名单" : "操作失败";
            if (result)
            {
                _blacklist.Add(NewBlacklistMobile);
                NewBlacklistMobile = "";
            }
        }
        catch (Exception ex)
        {
            ApiStatus = $"错误: {ex.Message}";
        }
    }

    private async Task DoRemoveBlacklist(String mobile)
    {
        try
        {
            var result = await _api.RemoveBlacklistAsync(mobile);
            ApiStatus = result ? "已移出黑名单" : "操作失败";
            if (result)
                _blacklist.Remove(mobile);
        }
        catch (Exception ex)
        {
            ApiStatus = $"错误: {ex.Message}";
        }
    }

    private async Task DoRemoveSession(String mobile)
    {
        try
        {
            var result = await _api.RemoveSessionAsync(mobile);
            ApiStatus = result ? "会话已移除" : "操作失败";
            if (result)
                await DoRefreshSessions();
        }
        catch (Exception ex)
        {
            ApiStatus = $"错误: {ex.Message}";
        }
    }
}
