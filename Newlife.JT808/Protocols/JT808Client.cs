using System.Reflection;
using NewLife.Data;
using NewLife.Log;
using NewLife.Net;
using NewLife.Reflection;
using NewLife.Threading;
using NewLife.JT808.Protocols;

namespace NewLife.JT808.Protocols;

/// <summary>JT/T 808消息处理器委托</summary>
/// <param name="session">会话</param>
/// <param name="message">收到的消息</param>
/// <returns>响应消息体，返回null表示不回复</returns>
public delegate Object MessageHandler(JT808Client session, JTMessage message);

/// <summary>JT/T 808 TCP客户端</summary>
/// <remarks>
/// 封装TCP连接管理、消息收发、处理器路由。
/// 支持同步发送和异步等待响应，通过[MessageKind]特性自动路由消息到处理器。
/// 内置自动重连和心跳保活机制。
/// </remarks>
public class JT808Client : DisposeBase
{
    #region 属性
    /// <summary>名称</summary>
    public String Name { get; set; }

    /// <summary>服务端地址</summary>
    public NetUri Server { get; set; }

    /// <summary>终端手机号</summary>
    public String Mobile { get; set; }

    /// <summary>协议配置</summary>
    public JT808Config Config { get; set; } = new JT808Config();

    /// <summary>网络粘包编解码器</summary>
    public JTCodec Codec { get; set; } = new JTCodec();

    /// <summary>收到消息时触发</summary>
    public event EventHandler<ReceivedEventArgs> Received;

    /// <summary>消息处理器集合。Key为消息类型，Value为处理委托</summary>
    public IDictionary<MessageKinds, MessageHandler> Handlers { get; set; } = new Dictionary<MessageKinds, MessageHandler>();

    private ISocketClient _client;
    /// <summary>底层网络连接</summary>
    public ISocketClient Client => _client;

    /// <summary>日志</summary>
    public ILog Log { get; set; } = Logger.Null;

    /// <summary>性能追踪</summary>
    public ITracer Tracer { get; set; }

    /// <summary>是否已连接</summary>
    public Boolean Connected => _client != null && _client.Active;

    #region 自动重连
    /// <summary>是否启用自动重连。默认 true</summary>
    public Boolean AutoReconnect { get; set; } = true;

    /// <summary>重连间隔。默认 5000ms</summary>
    public Int32 ReconnectInterval { get; set; } = 5_000;

    /// <summary>最大重连次数。0 表示不限次数，默认 0</summary>
    public Int32 MaxReconnect { get; set; }

    private Int32 _reconnectCount;
    private TimerX? _reconnectTimer;
    #endregion

    #region 心跳
    /// <summary>心跳间隔。默认 30s，设为 0 禁用心跳</summary>
    public Int32 HeartbeatInterval { get; set; } = 30;

    private TimerX? _heartbeatTimer;
    private Int32 _seq;
    #endregion

    #region 统计
    /// <summary>已发送消息数</summary>
    public Int64 SentCount { get; private set; }

    /// <summary>已接收消息数</summary>
    public Int64 ReceivedCount { get; private set; }

    /// <summary>重连次数</summary>
    public Int64 ReconnectCount { get; private set; }
    #endregion
    #endregion

    #region 构造
    [System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "协议库通过反射注册全部消息体，AOT 部署需保留 NewLife.JT808 程序集")]
    static JT808Client()
    {
        // 确保消息工厂已初始化
        MessageFactory.Instance.Init();
    }

    /// <summary>销毁时释放网络连接</summary>
    /// <param name="disposing"></param>
    protected override void Dispose(Boolean disposing)
    {
        base.Dispose(disposing);

        StopHeartbeat();
        StopReconnect();

        if (_client != null)
        {
            _client.Received -= OnReceive;
            _client.TryDispose();
            _client = null;
        }
    }
    #endregion

    #region 连接管理
    /// <summary>打开连接并开始接收消息</summary>
    [System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "协议库通过反射注册全部消息体，AOT 部署需保留 NewLife.JT808 程序集")]
    public virtual void Open()
    {
        MessageFactory.Instance.Init();

        // 先关闭旧连接
        if (_client != null)
        {
            _client.Received -= OnReceive;
            _client.TryDispose();
            _client = null;
        }

        var client = Server.CreateRemote();
        client.Name = Name;
        client.Tracer = Tracer;
        client.Log = Log;

        client.Received += OnReceive;

#if DEBUG
        client.LogSend = true;
        client.LogReceive = true;
#endif

        client.Open();

        _client = client;
        _reconnectCount = 0;

        // 启动心跳
        StartHeartbeat();
    }

    /// <summary>关闭连接</summary>
    public virtual void Close()
    {
        StopHeartbeat();
        StopReconnect();

        if (_client != null)
        {
            _client.Received -= OnReceive;
            _client.TryDispose();
            _client = null;
        }
    }

    /// <summary>检查连接状态，断开时自动重连</summary>
    public virtual void EnsureConnected()
    {
        if (_client == null || !_client.Active)
        {
            Open();
        }
    }
    #endregion

    #region 接收处理
    /// <summary>接收消息回调</summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnReceive(Object sender, ReceivedEventArgs e)
    {
        // 统计接收
        ReceivedCount++;

        // 使用JTCodec处理粘包，解码为JTMessage列表
        var list = Codec.Decode(e.Packet);
        if (list == null || list.Count == 0) return;

        foreach (var msg in list)
        {
            using var span = Tracer?.NewSpan($"jt808:Receive:{msg.Kind}", msg);

            Received?.Invoke(this, new ReceivedEventArgs { Packet = e.Packet, Message = msg });

            // 路由到已注册的处理器
            if (Handlers.TryGetValue(msg.Kind, out var handler))
                handler(this, msg);
        }
    }
    #endregion

    #region 处理器注册
    /// <summary>注册消息处理器实例</summary>
    /// <remarks>
    /// 反射扫描处理器类型的所有方法，匹配规则：
    /// 1. 非静态方法
    /// 2. 返回类型为Object
    /// 3. 参数个数为2
    /// 4. 带有[MessageKind]特性
    /// </remarks>
    /// <typeparam name="T">处理器类型</typeparam>
    /// <param name="handler">处理器实例</param>
    public virtual void AddHandler<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicMethods)] T>(T handler)
    {
        var type = typeof(T);
        foreach (var method in type.GetMethods())
        {
            if (method.IsStatic) continue;
            if (method.ReturnType != typeof(Object)) continue;

            // 参数匹配：(JT808Client, JTMessage)
            var pis = method.GetParameters();
            if (pis.Length != 2) continue;
            if (!pis[0].ParameterType.IsAssignableFrom(typeof(JT808Client))) continue;
            if (!pis[1].ParameterType.IsAssignableFrom(typeof(JTMessage))) continue;

            // 获取消息类型映射
            var att = method.GetCustomAttribute<MessageKindAttribute>();
            if (att != null)
            {
                Handlers[att.Kind] = method.As<MessageHandler>(handler);

                WriteLog("Registered [{0}]: {1}.{2}", att.Kind, method.DeclaringType.FullName, method.Name);
            }
        }
    }

    /// <summary>手动注册消息处理器</summary>
    /// <param name="kind">消息类型</param>
    /// <param name="handler">处理委托</param>
    public void RegisterHandler(MessageKinds kind, MessageHandler handler)
    {
        Handlers[kind] = handler;
    }
    #endregion

    #region 消息收发
    /// <summary>发送消息，不等待响应</summary>
    /// <param name="body">消息体对象</param>
    public virtual void Send(Object body)
    {
        var msg = new JTMessage
        {
            Mobile = Mobile,
        };

        if (Config != null) msg.Version = (Byte)(Config.Version == JT808Version.JT2019 ? 1 : 0);

        msg.SetResult(body);

        using var span = Tracer?.NewSpan($"jt808:Send:{msg.Kind}", body);

        WriteLog("=>{0}", body.ToJsonJT808());

        var pk = Codec.Encode(msg);
        _client.Send(pk);
        SentCount++;
    }

    /// <summary>发送消息并等待响应</summary>
    /// <typeparam name="TResult">响应消息体类型</typeparam>
    /// <param name="body">请求消息体</param>
    /// <param name="timeout">超时毫秒</param>
    /// <returns>响应消息体</returns>
    public virtual async Task<TResult> SendAsync<TResult>(Object body, Int32 timeout = 3000) where TResult : class, new()
    {
        var msg = new JTMessage
        {
            Mobile = Mobile,
        };

        if (Config != null) msg.Version = (Byte)(Config.Version == JT808Version.JT2019 ? 1 : 0);

        msg.SetResult(body);

        using var span = Tracer?.NewSpan($"jt808:SendAsync:{msg.Kind}", body);

        WriteLog("=>{0}", body.ToJsonJT808());

        var pk = Codec.Encode(msg);

        // 使用TaskCompletionSource等待响应。按期望响应类型与手机号过滤，避免被同连接的其他报文抢先完成
        var expected = typeof(TResult).GetCustomAttribute<MessageKindAttribute>()?.Kind;
        var tcs = new TaskCompletionSource<JTMessage>();
        EventHandler<ReceivedEventArgs> handler = null;
        handler = (s, e) =>
        {
            if (e.Message is JTMessage reply &&
                (expected == null || reply.Kind == expected) &&
                (reply.Mobile.IsNullOrEmpty() || Mobile.IsNullOrEmpty() || reply.Mobile == Mobile))
                tcs.TrySetResult(reply);
        };

        try
        {
            Received += handler;
            _client.Send(pk);

            using var cts = new CancellationTokenSource(timeout);
            cts.Token.Register(() => tcs.TrySetCanceled());
            var reply = await tcs.Task;
            if (reply == null) return null;

            var result = reply.ReadModel<TResult>();

            WriteLog("<={0}", result.ToJsonJT808());

            return result;
        }
        finally
        {
            Received -= handler;
        }
    }

    /// <summary>发送原始JTMessage帧，不等待响应</summary>
    /// <param name="message">消息帧</param>
    public virtual void SendMessage(JTMessage message)
    {
        if (message.Mobile.IsNullOrEmpty()) message.Mobile = Mobile;

        var pk = Codec.Encode(message);
        _client.Send(pk);
    }
    #endregion

    #region 心跳管理
    /// <summary>启动心跳</summary>
    protected virtual void StartHeartbeat()
    {
        StopHeartbeat();

        if (HeartbeatInterval <= 0) return;

        _heartbeatTimer = new TimerX(DoHeartbeat, null, HeartbeatInterval * 1000, HeartbeatInterval * 1000)
        {
            Async = true,
        };
    }

    /// <summary>停止心跳</summary>
    protected virtual void StopHeartbeat()
    {
        _heartbeatTimer.TryDispose();
        _heartbeatTimer = null;
    }

    private async Task DoHeartbeat(Object state)
    {
        // 连接断开时不发送心跳
        if (_client == null || !_client.Active)
        {
            // 触发自动重连
            if (AutoReconnect)
            {
                StartReconnect();
            }
            return;
        }

        try
        {
            var seq = (UInt16)Interlocked.Increment(ref _seq);
            var msg = new JTMessage
            {
                Mobile = Mobile,
                Version = Config?.Version == JT808Version.JT2019 ? (Byte)1 : (Byte)0,
                Kind = MessageKinds.终端心跳,
                Sequence = seq,
            };

            var pk = Codec.Encode(msg);
            _client.Send(pk);
            SentCount++;

            WriteLog("Heartbeat => {0}", seq);
        }
        catch (Exception ex)
        {
            WriteLog("Heartbeat error: {0}", ex.Message);
        }
    }
    #endregion

    #region 自动重连
    /// <summary>启动自动重连</summary>
    protected virtual void StartReconnect()
    {
        if (!AutoReconnect) return;
        if (_reconnectTimer != null) return;

        if (MaxReconnect > 0 && _reconnectCount >= MaxReconnect)
        {
            WriteLog("Maximum reconnection attempts {0} reached, stop reconnecting.", MaxReconnect);
            return;
        }

        _reconnectTimer = new TimerX(DoReconnect, null, ReconnectInterval, ReconnectInterval)
        {
            Async = true,
        };
    }

    /// <summary>停止自动重连</summary>
    protected virtual void StopReconnect()
    {
        _reconnectTimer.TryDispose();
        _reconnectTimer = null;
    }

    private async Task DoReconnect(Object state)
    {
        // 已连接则取消重连
        if (_client != null && _client.Active)
        {
            StopReconnect();
            _reconnectCount = 0;
            return;
        }

        // 达到最大重连次数后停止，避免无限重试
        if (MaxReconnect > 0 && _reconnectCount >= MaxReconnect)
        {
            StopReconnect();
            WriteLog("Maximum reconnection attempts {0} reached, stop reconnecting.", MaxReconnect);
            return;
        }

        _reconnectCount++;
        ReconnectCount++;

        WriteLog("Auto reconnect attempt {0}...", _reconnectCount);

        try
        {
            Open();
        }
        catch (Exception ex)
        {
            WriteLog("Auto reconnect failed: {0}", ex.Message);
        }
    }
    #endregion

    #region 日志
    /// <summary>写日志</summary>
    /// <param name="format"></param>
    /// <param name="args"></param>
    public void WriteLog(String format, params Object[] args) => Log?.Info(format, args);
    #endregion
}
