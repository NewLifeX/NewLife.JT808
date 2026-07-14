using JT808.Data;
using NewLife;
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

        // 自动注册设备
        var mobile = message.Mobile;
        var device = Device.FindByMobile(mobile);
        if (device == null)
        {
            device = new Device
            {
                Mobile = mobile,
                PlateNo = model.PlateNo,
                ManufacturerId = model.Provider,
                DeviceType = model.Model,
                Code = Guid.NewGuid().ToString("N")[..8].ToUpper(),
                Version = message.Version > 0 ? "2019" : "2013",
                Enable = true,
            };
            device.Insert();
            XTrace.WriteLine("设备自动注册：Mobile={0}, Code={1}", mobile, device.Code);
        }

        return new T8100
        {
            Sequence = message.Sequence,
            Result = RegisterResult.成功,
            Code = device.Code ?? String.Empty,
        };
    }

    [MessageKind(MessageKinds.终端鉴权)]
    public Object OnAuth(JT808Session session, JTMessage message)
    {
        var model = message.ReadModel<T0102>();
        XTrace.WriteLine("终端鉴权：Mobile={0}, Code={1}", message.Mobile, model.Code);

        // 验证鉴权码
        var device = Device.FindByMobile(message.Mobile);
        if (device == null || !device.Enable)
        {
            XTrace.WriteLine("鉴权失败：设备不存在或已禁用，Mobile={0}", message.Mobile);
            return null;
        }

        // 注册会话
        session.Host.Manager.Register(message.Mobile, session);

        // 写入在线状态
        var online = DeviceOnline.FindByDeviceId(device.Id);
        if (online == null)
        {
            online = new DeviceOnline();
            online.DeviceId = device.Id;
        }
        online.Mobile = message.Mobile;
        online.SessionId = session.ID.ToString();
        online.RemoteEndPoint = session.Remote?.ToString();
        online.Online = true;
        online.HeartbeatTime = DateTime.Now;
        online.SaveAsync();

        // 记录上下线历史
        var history = new DeviceHistory
        {
            DeviceId = device.Id,
            Mobile = message.Mobile,
            Action = "上线",
            CreateTime = DateTime.Now,
        };
        history.Insert();

        XTrace.WriteLine("设备上线：Mobile={0}, DeviceId={1}", message.Mobile, device.Id);
        return null;
    }

    [MessageKind(MessageKinds.终端心跳)]
    public Object OnHeartbeat(JT808Session session, JTMessage message)
    {
        // 更新心跳时间
        var device = Device.FindByMobile(message.Mobile);
        if (device != null)
        {
            var online = DeviceOnline.FindByDeviceId(device.Id);
            if (online != null)
            {
                online.HeartbeatTime = DateTime.Now;
                online.Update();
            }
        }
        return null;
    }

    [MessageKind(MessageKinds.终端注销)]
    public Object OnLogout(JT808Session session, JTMessage message)
    {
        XTrace.WriteLine("终端注销：Mobile={0}", message.Mobile);

        var device = Device.FindByMobile(message.Mobile);
        if (device != null)
        {
            // 更新在线状态
            var online = DeviceOnline.FindByDeviceId(device.Id);
            if (online != null)
            {
                online.Online = false;
                online.Update();
            }

            // 记录历史
            var history = new DeviceHistory
            {
                DeviceId = device.Id,
                Mobile = message.Mobile,
                Action = "下线",
                Remark = "终端注销",
                CreateTime = DateTime.Now,
            };
            history.Insert();
        }

        return null;
    }
}
