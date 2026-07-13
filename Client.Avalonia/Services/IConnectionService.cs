using NewLife.JT808.Protocols;

namespace Client.Avalonia.Services;

/// <summary>TCP 连接服务接口 — 封装 JT808Client 生命周期</summary>
public interface IConnectionService
{
    /// <summary>终端手机号</summary>
    String Mobile { get; }

    /// <summary>是否已连接</summary>
    Boolean IsConnected { get; }

    /// <summary>已发送消息数</summary>
    Int64 SentCount { get; }

    /// <summary>已接收消息数</summary>
    Int64 ReceivedCount { get; }

    /// <summary>重连次数</summary>
    Int64 ReconnectCount { get; }

    /// <summary>连接状态变更事件</summary>
    event EventHandler<Boolean>? ConnectionChanged;

    /// <summary>收到消息事件</summary>
    event EventHandler<JTMessage>? MessageReceived;

    /// <summary>发送消息事件</summary>
    event EventHandler<JTMessage>? MessageSent;

    /// <summary>连接到服务端</summary>
    void Connect(String server, Int32 port, String mobile);

    /// <summary>断开连接</summary>
    void Disconnect();

    /// <summary>发送消息体</summary>
    void Send(Object body);

    /// <summary>发送消息体并等待响应</summary>
    Task<TResult?> SendAsync<TResult>(Object body, Int32 timeout = 3000) where TResult : class, new();
}
