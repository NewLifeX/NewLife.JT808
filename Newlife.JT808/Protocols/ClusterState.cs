using NewLife.Caching;

namespace NewLife.JT808.Protocols;

/// <summary>终端会话信息，用于 Web 与网关之间共享在线状态</summary>
public class JTSessionInfo
{
    /// <summary>终端手机号</summary>
    public String Mobile { get; set; } = String.Empty;

    /// <summary>网关节点标识</summary>
    public String Node { get; set; } = String.Empty;

    /// <summary>会话编号</summary>
    public String SessionId { get; set; } = String.Empty;

    /// <summary>远端地址</summary>
    public String? Remote { get; set; }

    /// <summary>登录时间</summary>
    public DateTime LoginTime { get; set; }

    /// <summary>最后活跃时间</summary>
    public DateTime LastActive { get; set; }
}

/// <summary>网关节点信息</summary>
public class JTNodeInfo
{
    /// <summary>节点标识</summary>
    public String Node { get; set; } = String.Empty;

    /// <summary>协议端口</summary>
    public Int32 Port { get; set; }

    /// <summary>启动时间</summary>
    public DateTime StartTime { get; set; }

    /// <summary>最后心跳时间</summary>
    public DateTime LastActive { get; set; }

    /// <summary>本节点在线会话数</summary>
    public Int32 Sessions { get; set; }
}

/// <summary>下行指令动作</summary>
public enum JTCommandActions
{
    /// <summary>下发消息体</summary>
    Send = 0,

    /// <summary>强制断开会话</summary>
    Kick = 1,
}

/// <summary>下行指令模型</summary>
/// <remarks>Web 把指令写入目标节点的 Redis 队列，网关消费后在本地会话上执行。</remarks>
public class JTCommand
{
    /// <summary>目标终端手机号</summary>
    public String Mobile { get; set; } = String.Empty;

    /// <summary>指令动作</summary>
    public JTCommandActions Action { get; set; }

    /// <summary>消息体类型全名</summary>
    public String? BodyType { get; set; }

    /// <summary>消息体 JSON 数据</summary>
    public String? BodyData { get; set; }

    /// <summary>来源</summary>
    public String? Source { get; set; }

    /// <summary>创建时间</summary>
    public DateTime CreateTime { get; set; }
}

/// <summary>集群共享状态</summary>
/// <remarks>
/// 基于 <see cref="ICache"/> 保存跨进程共享的在线会话、网关节点与黑名单。
/// Web 与网关分别部署时统一以 Redis 为共享介质：
/// <list type="bullet">
/// <item>网关定时把本节点在线会话和节点心跳写入缓存，Web 据此得到真实在线列表和指令目标节点</item>
/// <item>黑名单以缓存为权威数据源，网关定时同步，接入时校验</item>
/// <item>下行指令写入目标节点队列 <see cref="GetCommandTopic"/>，由该节点消费</item>
/// </list>
/// 会话与节点均带过期时间，节点异常退出后残留记录会自动失效；会话索引中的失效成员在读取时惰性清理。
/// </remarks>
/// <example>
/// <code>
/// var state = new ClusterState(cacheProvider.Cache) { Node = "gw-1" };
/// state.PublishSession(new JTSessionInfo { Mobile = "13800138000", Node = "gw-1" });
///
/// var session = state.GetSession("13800138000");
/// if (session != null)
/// {
///     var queue = state.GetCommandQueue(session.Node);
///     queue?.Add(new JTCommand { Mobile = session.Mobile, Action = JTCommandActions.Kick });
/// }
/// </code>
/// </example>
public class ClusterState
{
    #region 常量
    /// <summary>会话键前缀</summary>
    public const String SessionPrefix = "jt808:session:";

    /// <summary>会话索引键</summary>
    public const String SessionIndex = "jt808:sessions";

    /// <summary>节点键前缀</summary>
    public const String NodePrefix = "jt808:node:";

    /// <summary>节点索引键</summary>
    public const String NodeIndex = "jt808:nodes";

    /// <summary>黑名单键</summary>
    public const String BlacklistKey = "jt808:blacklist";

    /// <summary>指令队列前缀</summary>
    public const String CommandPrefix = "jt808:command:";

    /// <summary>获取会话键</summary>
    /// <param name="mobile">终端手机号</param>
    /// <returns>缓存键</returns>
    public static String GetSessionKey(String mobile) => SessionPrefix + mobile;

    /// <summary>获取节点键</summary>
    /// <param name="node">节点标识</param>
    /// <returns>缓存键</returns>
    public static String GetNodeKey(String node) => NodePrefix + node;

    /// <summary>获取节点指令队列主题</summary>
    /// <param name="node">节点标识</param>
    /// <returns>队列主题</returns>
    public static String GetCommandTopic(String node) => CommandPrefix + node;
    #endregion

    #region 属性
    /// <summary>缓存实例</summary>
    public ICache Cache { get; }

    /// <summary>当前节点标识</summary>
    public String Node { get; set; } = String.Empty;

    /// <summary>会话过期时间（秒）。网关定时续期，超时未续期视为离线</summary>
    public Int32 SessionExpire { get; set; } = 90;

    /// <summary>节点心跳过期时间（秒）</summary>
    public Int32 NodeExpire { get; set; } = 60;
    #endregion

    #region 构造
    /// <summary>实例化集群共享状态</summary>
    /// <param name="cache">缓存实例</param>
    public ClusterState(ICache cache) => Cache = cache ?? throw new ArgumentNullException(nameof(cache));
    #endregion

    #region 会话
    /// <summary>发布会话信息，同时把手机号加入会话索引</summary>
    /// <param name="session">会话信息</param>
    /// <param name="expire">过期时间（秒）。小于等于0时使用 <see cref="SessionExpire"/></param>
    public void PublishSession(JTSessionInfo session, Int32 expire = 0)
    {
        if (session == null || session.Mobile.IsNullOrEmpty()) return;

        if (expire <= 0) expire = SessionExpire;

        Cache.Set(GetSessionKey(session.Mobile), session, expire);
        Cache.GetSet<String>(SessionIndex).Add(session.Mobile);
    }

    /// <summary>移除会话信息，同时移出会话索引</summary>
    /// <param name="mobile">终端手机号</param>
    public void RemoveSession(String mobile)
    {
        if (mobile.IsNullOrEmpty()) return;

        Cache.Remove(GetSessionKey(mobile));
        Cache.GetSet<String>(SessionIndex).Remove(mobile);
    }

    /// <summary>获取会话信息</summary>
    /// <param name="mobile">终端手机号</param>
    /// <returns>会话信息，不在线时返回 null</returns>
    public JTSessionInfo? GetSession(String mobile)
    {
        if (mobile.IsNullOrEmpty()) return null;

        return Cache.Get<JTSessionInfo>(GetSessionKey(mobile));
    }

    /// <summary>获取全部在线会话</summary>
    /// <returns>会话列表。已过期但残留在索引中的手机号会被清理</returns>
    public IList<JTSessionInfo> GetSessions()
    {
        var list = new List<JTSessionInfo>();

        var index = Cache.GetSet<String>(SessionIndex);
        var mobiles = new List<String>();
        foreach (var mobile in index)
        {
            if (!mobile.IsNullOrEmpty()) mobiles.Add(mobile);
        }
        if (mobiles.Count == 0) return list;

        var keys = new String[mobiles.Count];
        for (var i = 0; i < mobiles.Count; i++) keys[i] = GetSessionKey(mobiles[i]);

        var dic = Cache.GetAll<JTSessionInfo>(keys);
        foreach (var mobile in mobiles)
        {
            var key = GetSessionKey(mobile);
            if (dic.TryGetValue(key, out var info) && info != null)
                list.Add(info);
            else if (dic.TryGetValue(mobile, out info) && info != null)
                list.Add(info);
            else
                index.Remove(mobile);
        }

        return list;
    }
    #endregion

    #region 节点
    /// <summary>发布节点心跳信息</summary>
    /// <param name="node">节点信息</param>
    /// <param name="expire">过期时间（秒）。小于等于0时使用 <see cref="NodeExpire"/></param>
    public void PublishNode(JTNodeInfo node, Int32 expire = 0)
    {
        if (node == null || node.Node.IsNullOrEmpty()) return;

        if (expire <= 0) expire = NodeExpire;

        Cache.Set(GetNodeKey(node.Node), node, expire);
        Cache.GetSet<String>(NodeIndex).Add(node.Node);
    }

    /// <summary>移除节点信息</summary>
    /// <param name="node">节点标识</param>
    public void RemoveNode(String node)
    {
        if (node.IsNullOrEmpty()) return;

        Cache.Remove(GetNodeKey(node));
        Cache.GetSet<String>(NodeIndex).Remove(node);
    }

    /// <summary>获取全部存活节点</summary>
    /// <returns>节点列表。心跳超时但残留在索引中的节点会被清理</returns>
    public IList<JTNodeInfo> GetNodes()
    {
        var list = new List<JTNodeInfo>();

        var index = Cache.GetSet<String>(NodeIndex);
        var nodes = new List<String>();
        foreach (var node in index)
        {
            if (!node.IsNullOrEmpty()) nodes.Add(node);
        }
        if (nodes.Count == 0) return list;

        var keys = new String[nodes.Count];
        for (var i = 0; i < nodes.Count; i++) keys[i] = GetNodeKey(nodes[i]);

        var dic = Cache.GetAll<JTNodeInfo>(keys);
        foreach (var node in nodes)
        {
            var key = GetNodeKey(node);
            if (dic.TryGetValue(key, out var info) && info != null)
                list.Add(info);
            else if (dic.TryGetValue(node, out info) && info != null)
                list.Add(info);
            else
                index.Remove(node);
        }

        return list;
    }
    #endregion

    #region 黑名单
    /// <summary>获取黑名单</summary>
    /// <returns>黑名单手机号数组</returns>
    public String[] GetBlacklist()
    {
        var set = Cache.GetSet<String>(BlacklistKey);
        var list = new List<String>();
        foreach (var mobile in set)
        {
            if (!mobile.IsNullOrEmpty()) list.Add(mobile);
        }

        return [.. list];
    }

    /// <summary>加入黑名单</summary>
    /// <param name="mobile">终端手机号</param>
    /// <returns>是否新增</returns>
    public Boolean AddBlacklist(String mobile)
    {
        if (mobile.IsNullOrEmpty()) return false;

        var set = Cache.GetSet<String>(BlacklistKey);
        if (set.Contains(mobile)) return false;

        set.Add(mobile);
        return true;
    }

    /// <summary>移出黑名单</summary>
    /// <param name="mobile">终端手机号</param>
    /// <returns>是否存在并移除</returns>
    public Boolean RemoveBlacklist(String mobile)
    {
        if (mobile.IsNullOrEmpty()) return false;

        return Cache.GetSet<String>(BlacklistKey).Remove(mobile);
    }
    #endregion

    #region 指令队列
    /// <summary>获取指定节点的指令队列</summary>
    /// <param name="node">节点标识。为空时使用 <see cref="Node"/></param>
    /// <returns>指令队列</returns>
    public IProducerConsumer<JTCommand> GetCommandQueue(String? node = null)
    {
        node ??= Node;
        if (node.IsNullOrEmpty()) throw new InvalidOperationException("Cluster node is not set.");

        return Cache.GetQueue<JTCommand>(GetCommandTopic(node));
    }
    #endregion
}
