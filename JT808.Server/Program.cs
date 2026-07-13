using NewLife;
using NewLife.Log;
using NewLife.JT808.Models;
using NewLife.JT808.Protocols;
using NewLife.Messaging;
using NewLife.Serialization;
using Stardust;

XTrace.UseConsole();

// 初始化消息工厂
MessageFactory.Instance.Init();

// 创建服务端实例（先创建，后续事件总线需引用它）
var server = new JT808Server
{
    Port = 808,
    Log = XTrace.Log,
    SessionLog = XTrace.Log,
    Tracer = DefaultTracer.Instance,
};

// 注册处理器
server.AddHandler(new NormalHandler());
server.AddHandler(new PositionHandler());

// ===== 星尘指令总线（可选）：跨服务器指令下发 =====
// 配置方式：设置环境变量 StarServer/StarAppId/StarSecret
// 未配置时仅支持本地指令下发。
ICommandBus? commandBus = null;
var starServer = Environment.GetEnvironmentVariable("StarServer");

if (!starServer.IsNullOrEmpty())
{
    var star = new StarFactory(starServer, "JT808Server", null)
    {
        Log = XTrace.Log,
    };

    // 获取星尘应用客户端（登录后可创建事件总线）
    var app = star.App;
    if (app != null)
    {
        // 等待客户端登录就绪
        for (var i = 0; i < 50 && !app.Logined; i++)
            await Task.Delay(100);

        // 创建跨服务器指令事件总线
        var eventBus = app.CreateEventBus<CommandEvent>("jt808:command", null);

        // 订阅远程指令事件：其他服务器广播的指令在本机执行
        eventBus.Subscribe((CommandEvent? cmd) =>
        {
            if (cmd == null) return;

            XTrace.WriteLine("收到远程指令：Mobile={0}, Source={1}", cmd.Mobile, cmd.Source);

            if (cmd.BodyType.IsNullOrEmpty() || cmd.BodyData.IsNullOrEmpty()) return;

            var type = Type.GetType(cmd.BodyType);
            if (type == null)
            {
                XTrace.WriteLine("无法解析消息体类型：{0}", cmd.BodyType);
                return;
            }

            var body = NewLife.Serialization.JsonHelper.ToJsonEntity(cmd.BodyData, type);
            if (body == null)
            {
                XTrace.WriteLine("无法反序列化消息体：{0}", cmd.BodyData);
                return;
            }

            // 在本服务器上执行指令下发
            var session = server.Manager.Find(cmd.Mobile);
            if (session is ICommand cmdSession)
            {
                cmdSession.Send(body);
                XTrace.WriteLine("远程指令已下发到终端：Mobile={0}", cmd.Mobile);
            }
            else
            {
                XTrace.WriteLine("设备不在本服务器：Mobile={0}", cmd.Mobile);
            }
        });

        // 包装为指令总线：优先本地下发，失败时广播到集群
        var bus = new DefaultCommandBus(server.Manager, eventBus);
        server.CommandBus = bus;
        commandBus = bus;

        XTrace.WriteLine("星尘指令总线已就绪，Server={0}", star.Server);
    }
}

// 启动服务端
server.Start();

XTrace.WriteLine("JT808服务端已启动，端口：{0}", server.Port);
XTrace.WriteLine("按Ctrl+C退出");

using var waitHandle = new ManualResetEvent(false);
Console.CancelKeyPress += (s, e) =>
{
    e.Cancel = true;
    waitHandle.Set();
};
waitHandle.WaitOne();

server.Stop("用户退出");

/// <summary>通用消息处理器（注册/鉴权/心跳）</summary>
internal class NormalHandler
{
    [MessageKind(MessageKinds.终端注册)]
    public Object OnRegister(JT808Session session, JTMessage message)
    {
        var model = message.ReadModel<T0100>();
        XTrace.WriteLine("终端注册：Mobile={0}, PlateNo={1}", message.Mobile, model.PlateNo);

        return new T8100
        {
            Sequence = message.Sequence,
            Result = RegisterResult.成功,
            Code = Guid.NewGuid().ToString("N")[..8].ToUpper()
        };
    }

    [MessageKind(MessageKinds.终端鉴权)]
    public Object OnAuth(JT808Session session, JTMessage message)
    {
        var model = message.ReadModel<T0102>();
        XTrace.WriteLine("终端鉴权：Mobile={0}, Code={1}", message.Mobile, model.Code);

        session.Host.Manager.Register(message.Mobile, session);

        return null;
    }

    [MessageKind(MessageKinds.终端心跳)]
    public Object OnHeartbeat(JT808Session session, JTMessage message)
    {
        return null;
    }
}

/// <summary>位置消息处理器</summary>
internal class PositionHandler
{
    [MessageKind(MessageKinds.位置信息汇报)]
    public Object OnPosition(JT808Session session, JTMessage message)
    {
        var model = message.ReadModel<T0200>();
        XTrace.WriteLine("位置汇报：Mobile={0}, Lat={1}, Lng={2}, Speed={3}",
            message.Mobile,
            model.Latitude / 1_000_000.0,
            model.Longitude / 1_000_000.0,
            model.Speed);

        // 示例：下发云端指令（如设置参数）
        // server.SendCommand(message.Mobile, new T8300 { ... });

        return null;
    }
}

/// <summary>默认指令总线：包装 SessionManager + 远程事件总线</summary>
internal class DefaultCommandBus : ICommandBus
{
    private readonly SessionManager _manager;
    private readonly NewLife.Messaging.IEventBus<CommandEvent> _eventBus;

    public DefaultCommandBus(SessionManager manager, NewLife.Messaging.IEventBus<CommandEvent> eventBus)
    {
        _manager = manager;
        _eventBus = eventBus;

        // 当本地找不到设备时，通过事件总线广播到集群
        _manager.CommandReceived += async (s, e) =>
        {
            var evt = new CommandEvent
            {
                Mobile = e.Mobile,
                BodyType = e.Body.GetType().AssemblyQualifiedName,
                BodyData = e.Body.ToJson(),
                Source = Environment.MachineName,
            };

            await _eventBus.PublishAsync(evt);
            XTrace.WriteLine("指令已广播到集群：Mobile={0}, Type={1}", e.Mobile, e.Body.GetType().Name);
        };
    }

    public Boolean Send(String mobile, Object body) => _manager.Send(mobile, body);

    public async Task<TResult?> SendAsync<TResult>(String mobile, Object body) where TResult : class, new()
        => await _manager.SendAsync<TResult>(mobile, body);
}
