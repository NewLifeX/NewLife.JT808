using NewLife.JT808.Models;
using NewLife.JT808.Protocols;
using NewLife.Log;

namespace JT808.Server.Handlers;

/// <summary>通用消息处理器（注册/鉴权/心跳/注销）</summary>
public class NormalController
{
    [MessageKind(MessageKinds.终端注册)]
    public Object OnRegister(JT808Session session, JTMessage message)
    {
        var model = message.ReadModel<T0100>();
        XTrace.WriteLine("终端注册：Mobile={0}, PlateNo={1}, Provider={2}, Model={3}",
            message.Mobile, model.PlateNo, model.Provider, model.Model);

        // TODO: DeviceService 设备自动注册/验证
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

        // TODO: DeviceService 鉴权验证
        session.Host.Manager.Register(message.Mobile, session);

        return null;
    }

    [MessageKind(MessageKinds.终端心跳)]
    public Object OnHeartbeat(JT808Session session, JTMessage message)
    {
        // TODO: DeviceOnlineHostedService 更新心跳时间
        return null;
    }

    [MessageKind(MessageKinds.终端注销)]
    public Object OnLogout(JT808Session session, JTMessage message)
    {
        XTrace.WriteLine("终端注销：Mobile={0}", message.Mobile);

        // TODO: DeviceService 更新在线状态
        return null;
    }
}
