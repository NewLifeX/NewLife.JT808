using JT808.Data;
using NewLife;
using NewLife.JT808.Models;
using NewLife.JT808.Protocols;
using NewLife.Log;

namespace JT808.Server.Handlers;

/// <summary>信息消息处理器（参数查询/设置/升级）</summary>
public class InfoController
{
    [MessageKind(MessageKinds.查询终端参数)]
    public Object OnQueryParam(JT808Session session, JTMessage message)
    {
        XTrace.WriteLine("查询终端参数：Mobile={0}", message.Mobile);

        // 记录参数查询指令
        var device = Device.FindByMobile(message.Mobile);
        if (device != null)
        {
            var cmd = new DeviceCommand
            {
                DeviceId = device.Id,
                Mobile = message.Mobile,
                Kind = (Int32)MessageKinds.查询终端参数,
                Status = 2, // 已应答
                BodyData = $"查询终端参数: Seq={message.Sequence}",
                CreateTime = DateTime.Now,
            };
            cmd.InsertAsync();
        }

        return null;
    }

    [MessageKind(MessageKinds.设置终端参数)]
    public Object OnSetParam(JT808Session session, JTMessage message)
    {
        XTrace.WriteLine("设置终端参数：Mobile={0}", message.Mobile);

        var device = Device.FindByMobile(message.Mobile);
        if (device != null)
        {
            var cmd = new DeviceCommand
            {
                DeviceId = device.Id,
                Mobile = message.Mobile,
                Kind = (Int32)MessageKinds.设置终端参数,
                Status = 2,
                BodyData = $"设置终端参数: Seq={message.Sequence}",
                CreateTime = DateTime.Now,
            };
            cmd.InsertAsync();
        }

        return null;
    }

    [MessageKind(MessageKinds.终端升级结果通知)]
    public Object OnUpgradeResult(JT808Session session, JTMessage message)
    {
        XTrace.WriteLine("终端升级结果：Mobile={0}", message.Mobile);
        return null;
    }
}
