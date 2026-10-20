using JT808.Data;
using NewLife;
using NewLife.JT808.Protocols;
using NewLife.Log;
using NewLife.Serialization;

namespace JT808.Server.Services;

/// <summary>下行指令执行器</summary>
/// <remarks>
/// Redis 节点队列（Web 下发）与外部消息主题（其它系统下发）共用同一套执行逻辑：
/// 按动作下发消息体或强制断开会话，并记录指令下发结果。
/// </remarks>
public class CommandExecutor
{
    private readonly JT808Server _server;
    private readonly ClusterState _cluster;

    /// <summary>实例化</summary>
    /// <param name="server">网关服务</param>
    /// <param name="cluster">集群共享状态</param>
    public CommandExecutor(JT808Server server, ClusterState cluster)
    {
        _server = server;
        _cluster = cluster;
    }

    /// <summary>执行指令。终端不在本节点时直接忽略</summary>
    /// <param name="cmd">指令</param>
    /// <returns>是否执行成功</returns>
    public Boolean Execute(JTCommand cmd)
    {
        if (cmd == null || cmd.Mobile.IsNullOrEmpty()) return false;

        var session = _server.Manager.Find(cmd.Mobile) as JT808Session;
        if (session == null)
        {
            XTrace.WriteLine("设备不在线，忽略指令：Mobile={0}, Action={1}", cmd.Mobile, cmd.Action);
            return false;
        }

        if (cmd.Action == JTCommandActions.Kick)
        {
            _server.Manager.Remove(cmd.Mobile);
            _cluster.RemoveSession(cmd.Mobile);
            session.Close("管理员强制断开");
            XTrace.WriteLine("会话已强制断开：Mobile={0}", cmd.Mobile);
            return true;
        }

        XTrace.WriteLine("收到指令：Mobile={0}, Type={1}", cmd.Mobile, cmd.BodyType);

        if (cmd.BodyType.IsNullOrEmpty() || cmd.BodyData.IsNullOrEmpty()) return false;

        var type = MessageFactory.FindType(cmd.BodyType);
        if (type == null)
        {
            XTrace.WriteLine("无法解析消息体类型：{0}", cmd.BodyType);
            return false;
        }

        var body = JsonHelper.ToJsonEntity(cmd.BodyData, type, null);
        if (body == null)
        {
            XTrace.WriteLine("无法反序列化消息体：{0}", cmd.BodyData);
            return false;
        }

        session.SendJT808(body);
        XTrace.WriteLine("指令已下发到终端：Mobile={0}, Type={1}", cmd.Mobile, cmd.BodyType);

        // 记录指令
        var device = Device.FindByMobile(cmd.Mobile);
        if (device != null)
        {
            var cmdRecord = new DeviceCommand
            {
                DeviceId = device.Id,
                Mobile = cmd.Mobile,
                Kind = 0,
                Status = 1, // 已下发
                BodyType = cmd.BodyType,
                BodyData = cmd.BodyData,
                CreateTime = DateTime.Now,
            };
            _ = Task.Run(() => cmdRecord.InsertAsync());
        }

        return true;
    }
}
