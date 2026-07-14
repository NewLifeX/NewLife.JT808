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
        // TODO: 返回终端参数应答
        return null;
    }

    [MessageKind(MessageKinds.终端升级结果通知)]
    public Object OnUpgradeResult(JT808Session session, JTMessage message)
    {
        XTrace.WriteLine("终端升级结果：Mobile={0}", message.Mobile);
        return null;
    }
}
