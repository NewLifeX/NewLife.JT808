using JT808.Data;
using NewLife;
using NewLife.JT808.Models;
using NewLife.JT808.Protocols;
using NewLife.Log;

namespace JT808.Server.Handlers;

/// <summary>文件上传消息处理器（报警附件/文件补传）</summary>
public class FileHandler
{
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
}
