using JT808.Data;
using NewLife;
using NewLife.JT808.Models;
using NewLife.JT808.Protocols;
using NewLife.Log;

namespace JT808.Server.Handlers;

/// <summary>多媒体消息处理器（多媒体事件/存储检索）</summary>
public class MediaController
{
    [MessageKind(MessageKinds.多媒体事件信息上传)]
    public Object OnMediaEvent(JT808Session session, JTMessage message)
    {
        var model = message.ReadModel<T0800>();
        XTrace.WriteLine("多媒体事件：Mobile={0}, MultimediaType={1}, Format={2}",
            message.Mobile, model.MultimediaType, model.MultimediaCodingFormat);

        var device = Device.FindByMobile(message.Mobile);
        if (device != null)
        {
            var media = new MediaInfo
            {
                DeviceId = device.Id,
                Mobile = message.Mobile,
                MediaType = model.MultimediaType,
                Format = model.MultimediaCodingFormat,
                CreateTime = DateTime.Now,
            };
            _ = Task.Run(() => media.InsertAsync());
        }

        return null;
    }

    [MessageKind(MessageKinds.多媒体数据上传)]
    public Object OnMediaUpload(JT808Session session, JTMessage message)
    {
        XTrace.WriteLine("多媒体数据上传：Mobile={0}", message.Mobile);
        return null;
    }

    [MessageKind(MessageKinds.存储多媒体数据检索)]
    public Object OnMediaRetrieval(JT808Session session, JTMessage message)
    {
        XTrace.WriteLine("存储多媒体检索：Mobile={0}", message.Mobile);
        return null;
    }
}
