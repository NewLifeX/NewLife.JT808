using System.ComponentModel;
using NewLife.Caching;
using NewLife.JT808.Models;
using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace XUnitTest.Protocols;

/// <summary>集群共享状态单元测试</summary>
public class ClusterStateTests
{
    private static ClusterState CreateState(String node = "gw-1") => new(new MemoryCache()) { Node = node };

    private static JTSessionInfo CreateSession(String mobile, String node = "gw-1") => new()
    {
        Mobile = mobile,
        Node = node,
        SessionId = "1001",
        Remote = "127.0.0.1:5000",
        LoginTime = new DateTime(2026, 10, 20, 8, 0, 0),
        LastActive = new DateTime(2026, 10, 20, 8, 5, 0),
    };

    [Fact]
    [DisplayName("集群状态_发布会话后可读取")]
    public void PublishSession_CanBeRead()
    {
        var state = CreateState();

        state.PublishSession(CreateSession("13800138000"));

        var session = state.GetSession("13800138000");
        Assert.NotNull(session);
        Assert.Equal("13800138000", session.Mobile);
        Assert.Equal("gw-1", session.Node);
        Assert.Equal("1001", session.SessionId);
        Assert.Equal("127.0.0.1:5000", session.Remote);
        Assert.Equal(new DateTime(2026, 10, 20, 8, 5, 0), session.LastActive);
    }

    [Fact]
    [DisplayName("集群状态_空手机号不写入且查询返回空")]
    public void PublishSession_EmptyMobileIgnored()
    {
        var state = CreateState();

        state.PublishSession(new JTSessionInfo { Mobile = "", Node = "gw-1" });
        state.PublishSession(null!);

        Assert.Empty(state.GetSessions());
        Assert.Null(state.GetSession(""));
        Assert.Null(state.GetSession("13800138000"));
    }

    [Fact]
    [DisplayName("集群状态_移除会话同时清理索引")]
    public void RemoveSession_RemovesFromIndex()
    {
        var state = CreateState();
        state.PublishSession(CreateSession("13800138000"));

        state.RemoveSession("13800138000");

        Assert.Null(state.GetSession("13800138000"));
        Assert.Empty(state.GetSessions());
    }

    [Fact]
    [DisplayName("集群状态_会话列表按节点区分")]
    public void GetSessions_FiltersByNode()
    {
        var state = CreateState();
        state.PublishSession(CreateSession("13800138000", "gw-1"));
        state.PublishSession(CreateSession("13900139000", "gw-2"));

        var list = state.GetSessions();

        Assert.Equal(2, list.Count);
        Assert.Contains(list, e => e.Mobile == "13800138000" && e.Node == "gw-1");
        Assert.Contains(list, e => e.Mobile == "13900139000" && e.Node == "gw-2");
    }

    [Fact]
    [DisplayName("集群状态_会话过期后清理失效索引")]
    public void GetSessions_PrunesStaleIndexEntry()
    {
        var state = CreateState();
        state.PublishSession(CreateSession("13800138000"));
        state.PublishSession(CreateSession("13900139000"));

        // 模拟节点异常退出：会话键过期，索引残留
        state.Cache.Remove(ClusterState.GetSessionKey("13900139000"));

        var list = state.GetSessions();

        Assert.Single(list);
        Assert.Equal("13800138000", list[0].Mobile);

        // 残留索引已被清理，再次读取仍然只有一条
        Assert.Single(state.GetSessions());
        Assert.DoesNotContain("13900139000", state.Cache.GetSet<String>(ClusterState.SessionIndex));
    }

    [Fact]
    [DisplayName("集群状态_节点发布读取与移除")]
    public void PublishNode_GetAndRemove()
    {
        var state = CreateState();

        state.PublishNode(new JTNodeInfo
        {
            Node = "gw-1",
            Port = 808,
            StartTime = new DateTime(2026, 10, 20, 8, 0, 0),
            LastActive = new DateTime(2026, 10, 20, 8, 5, 0),
            Sessions = 12,
        });

        var nodes = state.GetNodes();
        Assert.Single(nodes);
        Assert.Equal("gw-1", nodes[0].Node);
        Assert.Equal(808, nodes[0].Port);
        Assert.Equal(12, nodes[0].Sessions);

        state.RemoveNode("gw-1");
        Assert.Empty(state.GetNodes());
    }

    [Fact]
    [DisplayName("集群状态_节点过期后清理失效索引")]
    public void GetNodes_PrunesStaleIndexEntry()
    {
        var state = CreateState();
        state.PublishNode(new JTNodeInfo { Node = "gw-1", Port = 808, Sessions = 1 });
        state.PublishNode(new JTNodeInfo { Node = "gw-2", Port = 809, Sessions = 2 });

        state.Cache.Remove(ClusterState.GetNodeKey("gw-2"));

        var nodes = state.GetNodes();
        Assert.Single(nodes);
        Assert.Equal("gw-1", nodes[0].Node);
        Assert.DoesNotContain("gw-2", state.Cache.GetSet<String>(ClusterState.NodeIndex));
    }

    [Fact]
    [DisplayName("集群状态_黑名单共享读写")]
    public void Blacklist_AddRemoveGet()
    {
        var state = CreateState();

        Assert.True(state.AddBlacklist("13800138000"));
        Assert.False(state.AddBlacklist("13800138000"));
        Assert.False(state.AddBlacklist(""));

        var list = state.GetBlacklist();
        Assert.Single(list);
        Assert.Equal("13800138000", list[0]);

        Assert.True(state.RemoveBlacklist("13800138000"));
        Assert.False(state.RemoveBlacklist("13800138000"));
        Assert.Empty(state.GetBlacklist());
    }

    [Fact]
    [DisplayName("集群状态_指令队列按节点隔离")]
    public void CommandQueue_IsolatedByNode()
    {
        var state = CreateState("gw-1");

        var command = new JTCommand
        {
            Mobile = "13800138000",
            Action = JTCommandActions.Send,
            BodyType = typeof(T8201).FullName,
            BodyData = "{}",
            CreateTime = new DateTime(2026, 10, 20, 9, 0, 0),
        };

        var queue = state.GetCommandQueue();
        Assert.Equal(ClusterState.GetCommandTopic("gw-1"), "jt808:command:gw-1");
        Assert.Equal(1, queue.Add(command));

        // 其它节点队列不应收到该指令
        Assert.Equal(0, state.GetCommandQueue("gw-2").Count);

        var received = state.GetCommandQueue().TakeOne();
        Assert.NotNull(received);
        Assert.Equal("13800138000", received.Mobile);
        Assert.Equal(JTCommandActions.Send, received.Action);
        Assert.Equal(typeof(T8201).FullName, received.BodyType);
        Assert.Equal(0, state.GetCommandQueue().Count);
    }

    [Fact]
    [DisplayName("集群状态_强制下线指令可投递到目标节点")]
    public void CommandQueue_KickCommand()
    {
        var state = CreateState("gw-1");
        state.PublishSession(CreateSession("13800138000"));

        var session = state.GetSession("13800138000");
        Assert.NotNull(session);

        var queue = state.GetCommandQueue(session.Node);
        queue.Add(new JTCommand { Mobile = session.Mobile, Action = JTCommandActions.Kick });

        var received = queue.TakeOne();
        Assert.NotNull(received);
        Assert.Equal(JTCommandActions.Kick, received.Action);
        Assert.Equal("13800138000", received.Mobile);
    }

    [Fact]
    [DisplayName("集群状态_未设置节点时取队列抛异常")]
    public void CommandQueue_WithoutNode_Throws()
    {
        var state = new ClusterState(new MemoryCache());

        Assert.Throws<InvalidOperationException>(() => state.GetCommandQueue());
    }

    [Fact]
    [DisplayName("集群状态_指令消息体JSON可跨进程往返")]
    public void CommandBody_JsonRoundtrip()
    {
        var body = new T8300 { Flag = TextFlags.Service, Kind = TextKinds.Notice, Text = "调度通知" };
        var json = JsonHelper.ToJson(body);

        var type = MessageFactory.FindType(typeof(T8300).FullName!);
        Assert.Equal(typeof(T8300), type);

        var entity = JsonHelper.ToJsonEntity(json, type!, null);
        Assert.NotNull(entity);
        var result = Assert.IsType<T8300>(entity);
        Assert.Equal(body.Flag, result.Flag);
        Assert.Equal(body.Kind, result.Kind);
        Assert.Equal(body.Text, result.Text);
    }

    [Fact]
    [DisplayName("消息工厂_按名称查找消息体类型")]
    public void FindType_ByName()
    {
        Assert.Equal(typeof(T8201), MessageFactory.FindType("NewLife.JT808.Models.T8201"));
        Assert.Equal(typeof(T8201), MessageFactory.FindType("T8201"));
        Assert.Equal(typeof(T8201), MessageFactory.FindType(typeof(T8201).AssemblyQualifiedName!));
        Assert.Null(MessageFactory.FindType(""));
        Assert.Null(MessageFactory.FindType("Not.Exists.Type"));
    }
}
