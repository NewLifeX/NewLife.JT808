using NewLife;
using NewLife.JT808.Protocols;
using NewLife.Log;
using NewLife.Net;

namespace Client.Avalonia.Services;

/// <summary>TCP 连接服务实现 — 封装 JT808Client</summary>
public class ConnectionService : IConnectionService, IDisposable
{
    private JT808Client? _client;

    public String Mobile => _client?.Mobile ?? "";

    public Boolean IsConnected => _client?.Connected ?? false;
    public Int64 SentCount => _client?.SentCount ?? 0;
    public Int64 ReceivedCount => _client?.ReceivedCount ?? 0;
    public Int64 ReconnectCount => _client?.ReconnectCount ?? 0;

    public event EventHandler<Boolean>? ConnectionChanged;
    public event EventHandler<JTMessage>? MessageReceived;
    public event EventHandler<JTMessage>? MessageSent;

    public void Connect(String server, Int32 port, String mobile)
    {
        Disconnect();

        _client = new JT808Client
        {
            Server = new NewLife.Net.NetUri($"tcp://{server}:{port}"),
            Mobile = mobile,
            Name = $"JT808-{mobile}",
            AutoReconnect = true,
            HeartbeatInterval = 30,
            Log = XTrace.Log,
        };

        _client.Received += OnClientReceived;
        _client.Open();

        ConnectionChanged?.Invoke(this, true);
    }

    public void Disconnect()
    {
        if (_client != null)
        {
            _client.Received -= OnClientReceived;
            _client.Close();
            _client.TryDispose();
            _client = null;
        }

        ConnectionChanged?.Invoke(this, false);
    }

    public void Send(Object body)
    {
        // 为日志构造消息帧
        var logMsg = new JTMessage { Mobile = _client?.Mobile ?? "" };
        if (body is JTMessage jtMsg)
        {
            logMsg.Kind = jtMsg.Kind;
            logMsg.Sequence = jtMsg.Sequence;
        }
        else
        {
            logMsg.SetResult(body);
        }
        MessageSent?.Invoke(this, logMsg);

        _client?.Send(body);
    }

    public async Task<TResult?> SendAsync<TResult>(Object body, Int32 timeout = 3000) where TResult : class, new()
    {
        if (_client == null) return null;
        return await _client.SendAsync<TResult>(body, timeout);
    }

    private void OnClientReceived(Object? sender, ReceivedEventArgs e)
    {
        if (e.Message is JTMessage msg)
        {
            MessageReceived?.Invoke(this, msg);
        }
    }

    public void Dispose()
    {
        Disconnect();
    }
}
