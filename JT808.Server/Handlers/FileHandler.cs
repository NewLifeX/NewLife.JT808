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
        // TODO: 处理文件上传完成通知
        return null;
    }

    [MessageKind(MessageKinds.文件上传完成通知)]
    public Object OnFileUploadNotify(JT808Session session, JTMessage message)
    {
        XTrace.WriteLine("文件上传完成通知：Mobile={0}", message.Mobile);
        return null;
    }
}
