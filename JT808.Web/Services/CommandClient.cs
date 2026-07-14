using NewLife;
using NewLife.Caching;
using NewLife.Log;
using NewLife.Serialization;

namespace JT808.Web.Services;

/// <summary>指令下发客户端</summary>
/// <remarks>
/// 通过 Redis 队列向 JT808.Server 下发指令。
/// 参考 GPS808 的 CommandClient 模式。
/// </remarks>
public class CommandClient
{
    private readonly ICacheProvider _cacheProvider;

    public CommandClient(ICacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;
    }

    private IProducerConsumer<Object>? _command;

    /// <summary>下发指令到终端</summary>
    /// <param name="mobile">终端手机号</param>
    /// <param name="bodyType">消息体类型全名</param>
    /// <param name="bodyData">消息体 JSON 数据</param>
    /// <returns>是否成功推入队列</returns>
    public Boolean SendCommand(String mobile, String bodyType, String bodyData)
    {
        _command ??= _cacheProvider.GetQueue<Object>("Command", "Group");
        if (_command == null) return false;

        var cmd = new CommandModel
        {
            Mobile = mobile,
            BodyType = bodyType,
            BodyData = bodyData,
            Source = Environment.MachineName,
            CreateTime = DateTime.Now,
        };

        var rs = _command.Add(cmd);
        XTrace.WriteLine("指令已推入队列：Mobile={0}, Type={1}", mobile, bodyType);
        return rs > 0;
    }

    /// <summary>下发指令到终端（强类型消息体）</summary>
    public Boolean SendCommand(String mobile, Object body)
    {
        var bodyType = body.GetType().AssemblyQualifiedName;
        var bodyData = body.ToJson();
        return SendCommand(mobile, bodyType ?? String.Empty, bodyData);
    }

    /// <summary>指令模型</summary>
    public class CommandModel
    {
        /// <summary>目标终端手机号</summary>
        public String Mobile { get; set; } = String.Empty;

        /// <summary>消息体类型全名</summary>
        public String BodyType { get; set; } = String.Empty;

        /// <summary>消息体 JSON 数据</summary>
        public String BodyData { get; set; } = String.Empty;

        /// <summary>来源</summary>
        public String? Source { get; set; }

        /// <summary>创建时间</summary>
        public DateTime CreateTime { get; set; }
    }
}
