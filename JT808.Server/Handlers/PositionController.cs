using NewLife.JT808.Models;
using NewLife.JT808.Protocols;
using NewLife.Log;

namespace JT808.Server.Handlers;

/// <summary>位置消息处理器（位置上报/电子围栏/行驶记录）</summary>
public class PositionController
{
    [MessageKind(MessageKinds.位置信息汇报)]
    public Object OnPosition(JT808Session session, JTMessage message)
    {
        var model = message.ReadModel<T0200>();
        XTrace.WriteLine("位置汇报：Mobile={0}, Lat={1}, Lng={2}, Speed={3}, Direction={4}",
            message.Mobile,
            model.Latitude / 1_000_000.0,
            model.Longitude / 1_000_000.0,
            model.Speed,
            model.Direction);

        // TODO: DeviceService 保存位置 + 推入 Redis Stream
        return null;
    }

    [MessageKind(MessageKinds.位置信息查询)]
    public Object OnQueryPosition(JT808Session session, JTMessage message)
    {
        XTrace.WriteLine("位置查询：Mobile={0}", message.Mobile);
        // TODO: 返回位置信息应答
        return null;
    }
}
