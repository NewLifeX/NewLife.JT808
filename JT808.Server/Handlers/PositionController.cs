using JT808.Data;
using NewLife;
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

        try
        {
            // 查找设备
            var device = Device.FindByMobile(message.Mobile);
            if (device == null) return null;

            // 保存位置数据
            var pos = new PositionData
            {
                DeviceId = device.Id,
                Mobile = message.Mobile,
                Alarm = (UInt32)model.Alarm,
                Status = (UInt32)model.Status,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Altitude = (Int16)model.Altitude,
                Speed = (Int16)model.Speed,
                Direction = (Int16)model.Direction,
                GPSTime = model.GPSTime,
                CreateTime = DateTime.Now,
            };
            // 异步位置数据保存（不阻塞消息处理）
            _ = Task.Run(() => pos.InsertAsync());

            // 更新设备在线实时位置
            var online = DeviceOnline.FindByDeviceId(device.Id);
            if (online != null)
            {
                online.Latitude = model.Latitude;
                online.Longitude = model.Longitude;
                online.Speed = (Int16)model.Speed;
                online.Direction = (Int16)model.Direction;
                online.HeartbeatTime = DateTime.Now;
                online.Update();
            }

            // 处理报警标志
            if ((UInt32)model.Alarm != 0)
            {
                var alarm = new Alarm
                {
                    DeviceId = device.Id,
                    Mobile = message.Mobile,
                    AlarmType = (Int32)model.Alarm,
                    AlarmTime = model.GPSTime,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Speed = (Int16)model.Speed,
                    Direction = (Int16)model.Direction,
                    Altitude = (Int16)model.Altitude,
                    Content = $"报警标志: 0x{(UInt32)model.Alarm:X8}",
                    CreateTime = DateTime.Now,
                };
                _ = Task.Run(() => alarm.InsertAsync());
            }
        }
        catch (Exception ex)
        {
            XTrace.WriteLine("位置数据处理异常：{0}", ex.Message);
        }

        return null;
    }

    [MessageKind(MessageKinds.位置信息查询)]
    public Object? OnQueryPosition(JT808Session session, JTMessage message)
    {
        XTrace.WriteLine("位置查询：Mobile={0}", message.Mobile);

        // 以设备在线状态中的实时位置应答；未登记或不在线的终端不返回
        var device = Device.FindByMobile(message.Mobile);
        if (device == null) return null;

        var online = DeviceOnline.FindByDeviceId(device.Id);
        if (online == null) return null;

        return new T0201
        {
            Sequence = message.Sequence,
            Position = new T0200
            {
                Latitude = online.Latitude,
                Longitude = online.Longitude,
                Speed = (UInt16)online.Speed,
                Direction = (UInt16)online.Direction,
                GPSTime = online.HeartbeatTime,
            },
        };
    }
}
