using JT808.Data;
using JT808.Data.Safety;
using NewLife;
using NewLife.JT808.Models;
using NewLife.JT808.Protocols;
using NewLife.JT808.SuBiao;
using NewLife.Log;

namespace JT808.Server.Handlers;

/// <summary>文件上传消息处理器（报警附件/文件补传）</summary>
public class FileHandler
{
    [MessageKind(MessageKinds.报警附件信息)]
    public Object OnAlarmFileInfo(JT808Session session, JTMessage message)
    {
        var model = message.ReadModel<T1210>();
        XTrace.WriteLine("报警附件信息：TerminalId={0}, PlatformId={1}", model.TerminalId, model.PlatformId);

        var device = Device.FindByMobile(message.Mobile);
        if (device != null)
        {
            var alarmFile = new AlarmFile
            {
                DeviceId = device.Id,
                Mobile = message.Mobile,
                FileType = "报警附件",
                FileName = model.PlatformId,
                CreateTime = DateTime.Now,
            };
            _ = Task.Run(() => alarmFile.InsertAsync());
        }

        return null;
    }

    [MessageKind(MessageKinds.文件信息上传)]
    public Object OnFileInfoUpload(JT808Session session, JTMessage message)
    {
        var model = message.ReadModel<T1211>();
        XTrace.WriteLine("文件信息上传：Mobile={0}, FileName={1}", message.Mobile, model.FileName);

        var device = Device.FindByMobile(message.Mobile);
        if (device == null) return null;

        var cmd = new DeviceCommand
        {
            DeviceId = device.Id,
            Mobile = message.Mobile,
            Kind = (Int32)MessageKinds.文件信息上传,
            Status = 2,
            BodyData = $"文件信息上传: {model.FileName}",
            CreateTime = DateTime.Now,
        };
        _ = Task.Run(() => cmd.InsertAsync());

        return null;
    }

    [MessageKind(MessageKinds.文件上传完成通知)]
    public Object OnFileUploadComplete(JT808Session session, JTMessage message)
    {
        XTrace.WriteLine("文件上传完成：Mobile={0}", message.Mobile);

        var device = Device.FindByMobile(message.Mobile);
        if (device != null)
        {
            var cmd = new DeviceCommand
            {
                DeviceId = device.Id,
                Mobile = message.Mobile,
                Kind = (Int32)MessageKinds.文件上传完成通知,
                Status = 2,
                BodyData = $"文件上传完成: Seq={message.Sequence}",
                CreateTime = DateTime.Now,
            };
            _ = Task.Run(() => cmd.InsertAsync());
        }

        return null;
    }

    [MessageKind(MessageKinds.报警附件上传)]
    public Object OnAlarmFileUpload(JT808Session session, JTMessage message)
    {
        var model = message.ReadModel<T9208>();
        XTrace.WriteLine("报警附件上传指令：Mobile={0}, Server={1}:{2}",
            message.Mobile, model.Server, model.TcpPort);

        var device = Device.FindByMobile(message.Mobile);
        if (device == null) return null;

        var cmd = new DeviceCommand
        {
            DeviceId = device.Id,
            Mobile = message.Mobile,
            Kind = (Int32)MessageKinds.报警附件上传,
            Status = 2,
            BodyData = $"报警附件上传: {model.Server}:{model.TcpPort}",
            CreateTime = DateTime.Now,
        };
        _ = Task.Run(() => cmd.InsertAsync());

        return null;
    }
}
