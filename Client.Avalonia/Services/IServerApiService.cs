namespace Client.Avalonia.Services;

/// <summary>服务端管理 API 接口</summary>
public interface IServerApiService
{
    /// <summary>获取服务器统计</summary>
    Task<ServerStats?> GetStatsAsync();

    /// <summary>获取在线会话列表</summary>
    Task<SessionList?> GetSessionsAsync();

    /// <summary>获取指定终端会话详情</summary>
    Task<SessionInfo?> GetSessionAsync(String mobile);

    /// <summary>强制移除会话</summary>
    Task<Boolean> RemoveSessionAsync(String mobile);

    /// <summary>获取黑名单列表</summary>
    Task<BlacklistInfo?> GetBlacklistAsync();

    /// <summary>添加黑名单</summary>
    Task<Boolean> AddBlacklistAsync(String mobile);

    /// <summary>移除黑名单</summary>
    Task<Boolean> RemoveBlacklistAsync(String mobile);

    /// <summary>下发指令</summary>
    Task<Boolean> SendCommandAsync(String mobile, String bodyType, Dictionary<String, Object> body);
}

/// <summary>服务器统计</summary>
public class ServerStats
{
    public String Server { get; set; } = "";
    public Boolean Running { get; set; }
    public String Started { get; set; } = "";
}

/// <summary>会话列表</summary>
public class SessionList
{
    public Int32 Total { get; set; }

    /// <summary>会话详情列表</summary>
    public SessionInfo[]? Items { get; set; }
}

/// <summary>会话详情</summary>
public class SessionInfo
{
    public String Mobile { get; set; } = "";
    public String SessionId { get; set; } = "";
    public String Remote { get; set; } = "";
    public String Time { get; set; } = "";
}

/// <summary>黑名单信息</summary>
public class BlacklistInfo
{
    public Int32 Total { get; set; }
    public String[] Items { get; set; } = [];
}
