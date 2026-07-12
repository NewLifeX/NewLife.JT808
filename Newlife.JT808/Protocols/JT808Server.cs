using System.Reflection;
using NewLife.Caching;
using NewLife.Data;
using NewLife.Log;
using NewLife.Net;
using NewLife.Reflection;

namespace NewLife.JT808.Protocols;

/// <summary>JT/T 808消息处理器委托</summary>
/// <param name="session">会话</param>
/// <param name="message">收到的消息</param>
/// <returns>响应消息体。返回null表示不回复，返回消息体则自动封装为JTMessage发送</returns>
public delegate Object JT808Handler(JT808Session session, JTMessage message);

/// <summary>JT/T 808 TCP服务端</summary>
/// <remarks>
/// 轻量级TCP服务端，仅负责协议解析、Handler路由、分包合并和Session管理。
/// 不内置数据库访问和业务逻辑——这些由Handler注入实现。
/// 
/// 使用示例：
/// <code>
/// var server = new JT808Server { Port = 808 };
/// server.AddHandler(new MyPositionHandler());
/// server.AddHandler(new MyNormalHandler());
/// server.Start();
/// </code>
/// </remarks>
public class JT808Server : NetServer<JT808Session>
{
    #region 属性
    /// <summary>消息处理器集合。Key为消息类型，Value为处理委托</summary>
    public IDictionary<MessageKinds, JT808Handler> Handlers { get; } = new Dictionary<MessageKinds, JT808Handler>();

    /// <summary>会话管理器</summary>
    public SessionManager Manager { get; set; } = new();

    /// <summary>日志</summary>
    public ILog Log { get; set; } = Logger.Null;
    #endregion

    #region 构造
    /// <summary>实例化JT808服务端</summary>
    public JT808Server()
    {
        Port = 808;
    }
    #endregion

    #region 处理器注册
    /// <summary>注册消息处理器实例</summary>
    /// <remarks>
    /// 反射扫描处理器类型的所有方法，匹配规则：
    /// 1. 非静态方法
    /// 2. 返回类型为Object
    /// 3. 参数为 (JT808Session, JTMessage)
    /// 4. 带有[MessageKind]特性
    /// </remarks>
    /// <typeparam name="T">处理器类型</typeparam>
    /// <param name="handler">处理器实例</param>
    public void AddHandler<T>(T handler)
    {
        var type = handler.GetType();
        foreach (var method in type.GetMethods())
        {
            if (method.IsStatic) continue;
            if (method.ReturnType != typeof(Object)) continue;

            // 参数匹配：(JT808Session, JTMessage)
            var pis = method.GetParameters();
            if (pis.Length != 2) continue;

            // 获取消息类型映射
            var att = method.GetCustomAttribute<MessageKindAttribute>();
            if (att != null)
            {
                Handlers[att.Kind] = method.As<JT808Handler>(handler);

                WriteLog("注册[{0}]：{1}.{2}", att.Kind, method.DeclaringType.FullName, method.Name);
            }
        }
    }

    /// <summary>手动注册消息处理器</summary>
    /// <param name="kind">消息类型</param>
    /// <param name="handler">处理委托</param>
    public void RegisterHandler(MessageKinds kind, JT808Handler handler)
    {
        Handlers[kind] = handler;
    }
    #endregion

    #region 分包合并
    private readonly ICache _mergeCache = new MemoryCache { Period = 60, Expire = 60 };

    /// <summary>合并分包消息</summary>
    /// <remarks>
    /// 按 SessionID + MessageKinds 作为缓存键，60s超时。
    /// 集齐所有分包后返回完整消息，否则返回null并缓存当前包。
    /// </remarks>
    /// <param name="session">网络会话</param>
    /// <param name="message">收到的消息</param>
    /// <returns>集齐后的完整消息，未集齐返回null</returns>
    public JTMessage Merge(INetSession session, JTMessage message)
    {
        if (message == null) return message;
        if (!message.Property.HasFlag(PropertyFlags.Package)) return message;
        if (message.PackageCount == 0 || message.PackageIndex == 0 || message.PackageIndex > message.PackageCount) return message;

        var name = MessageFactory.GetKindName(message.Kind);
        using var span = Tracer?.NewSpan($"jt808:{name}:Merge", $"count={message.PackageCount} index={message.PackageIndex}");

        // 找到该消息的首包，把后续包附加进去
        var key = $"{session.ID}-{message.Kind}";
        if (_mergeCache.TryGetValue<JTMessage>(key, out var first))
        {
            if (first.Append(message))
            {
                using var span2 = Tracer?.NewSpan($"jt808:{name}:MergeFinish", first);

                _mergeCache.Remove(key);
                return first;
            }
        }
        else
        {
            // 拷贝数据，避免网络缓冲区被重用导致数据损坏
            message.Payload = new Packet(message.Payload.ToArray());
            _mergeCache.Set(key, message, 60);
        }

        return null;
    }
    #endregion

    #region 日志
    /// <summary>写日志</summary>
    /// <param name="format"></param>
    /// <param name="args"></param>
    public void WriteLog(String format, params Object[] args) => Log?.Info(format, args);
    #endregion
}
