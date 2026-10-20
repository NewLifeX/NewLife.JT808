using NewLife;
using NewLife.JT808.Protocols;
using NewLife.Log;

namespace JT808.Web.Services;

/// <summary>指令下发客户端</summary>
/// <remarks>
/// 通过分布式缓存中的节点队列向网关下发指令。
/// Web 不直接持有网关会话，依据共享会话状态判断终端所在节点，把指令投递到该节点队列。
/// </remarks>
public class CommandClient
{
    private readonly ClusterState _cluster;

    /// <summary>实例化</summary>
    /// <param name="cluster">集群共享状态</param>
    public CommandClient(ClusterState cluster) => _cluster = cluster;

    /// <summary>向终端所在节点下发消息体</summary>
    /// <param name="node">终端所在节点</param>
    /// <param name="mobile">终端手机号</param>
    /// <param name="bodyType">消息体类型全名</param>
    /// <param name="bodyData">消息体 JSON 数据</param>
    /// <returns>是否成功推入队列</returns>
    public Boolean SendCommand(String node, String mobile, String bodyType, String bodyData)
    {
        if (node.IsNullOrEmpty() || mobile.IsNullOrEmpty()) return false;

        var cmd = new JTCommand
        {
            Mobile = mobile,
            Action = JTCommandActions.Send,
            BodyType = bodyType,
            BodyData = bodyData,
            Source = Environment.MachineName,
            CreateTime = DateTime.Now,
        };

        return Push(node, cmd);
    }

    /// <summary>通知终端所在节点强制断开会话</summary>
    /// <param name="node">终端所在节点</param>
    /// <param name="mobile">终端手机号</param>
    /// <returns>是否成功推入队列</returns>
    public Boolean Kick(String node, String mobile)
    {
        if (node.IsNullOrEmpty() || mobile.IsNullOrEmpty()) return false;

        var cmd = new JTCommand
        {
            Mobile = mobile,
            Action = JTCommandActions.Kick,
            Source = Environment.MachineName,
            CreateTime = DateTime.Now,
        };

        return Push(node, cmd);
    }

    private Boolean Push(String node, JTCommand cmd)
    {
        var queue = _cluster.GetCommandQueue(node);
        var rs = queue.Add(cmd);

        XTrace.WriteLine("指令已推入节点队列：Node={0}, Mobile={1}, Action={2}", node, cmd.Mobile, cmd.Action);

        return rs > 0;
    }
}
