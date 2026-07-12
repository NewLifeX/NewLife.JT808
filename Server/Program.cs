using NewLife;
using NewLife.Log;
using NewLife.JT808.Models;
using NewLife.JT808.Protocols;

// 初始化消息工厂
MessageFactory.Instance.Init();

XTrace.UseConsole();

var server = new JT808Server
{
    Port = 808,
    Log = XTrace.Log,
    Tracer = DefaultTracer.Instance,
};

// 注册处理器
server.AddHandler(new NormalHandler());
server.AddHandler(new PositionHandler());

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

        return null;
    }
}
