using NewLife.Log;
using NewLife.RocketMQ;
using NewLife.Serialization;

namespace NewLife.JT808.Protocols.RocketMQ;

/// <summary>基于 RocketMQ 的下行指令处理器</summary>
/// <remarks>
/// 消费 RocketMQ 指令队列中的下行指令，将字符串类型的 BodyData 转换为 Byte[]。
/// 配合 CommandClient 使用，CommandClient 向 RocketMQ 发送指令，此处理器接收并转换。
/// </remarks>
public class RocketMQDownHandler : IDownMessageHandler, IDisposable
{
    #region 属性
    /// <summary>RocketMQ 消费者实例</summary>
    public Consumer Consumer { get; }

    /// <summary>指令主题</summary>
    public String Topic { get; }

    /// <summary>是否已启动</summary>
    public Boolean Active => Consumer?.Active ?? false;

    private Func<String, Byte[], Task<Byte[]>>? _handler;
    #endregion

    #region 构造
    /// <summary>实例化 RocketMQ 下行指令处理器</summary>
    /// <param name="nameServer">NameServer 地址</param>
    /// <param name="topic">指令主题</param>
    /// <param name="group">消费组</param>
    /// <param name="cloudProvider">云厂商适配器。阿里云传 <c>new AliyunProvider{AccessKey=...,SecretKey=...,InstanceId=...}</c>；华为云传 <c>new HuaweiProvider{...}</c>；腾讯云传 <c>new TencentProvider{...}</c>；ACL 传 <c>new AclProvider{...}</c></param>
    public RocketMQDownHandler(String nameServer, String topic, String group, NewLife.RocketMQ.ICloudProvider? cloudProvider = null)
    {
        Topic = topic;
        Consumer = new Consumer
        {
            NameServerAddress = nameServer,
            Topic = topic,
            Group = group,
            CloudProvider = cloudProvider,
        };
    }
    #endregion

    #region 方法
    /// <summary>处理下行消息</summary>
    /// <param name="mobile">目标终端手机号</param>
    /// <param name="data">消息体序列化数据</param>
    /// <returns>处理结果</returns>
    public Task<Byte[]> HandleAsync(String mobile, Byte[] data)
    {
        return Task.FromResult(data);
    }

    /// <summary>开始消费指令队列</summary>
    /// <param name="onMessage">消费回调：mobile, data → 结果</param>
    public void Start(Func<String, Byte[], Task<Byte[]>> onMessage)
    {
        _handler = onMessage;

        Consumer.OnConsumeAsync = async (queue, messages, ct) =>
        {
            foreach (var msg in messages)
            {
                try
                {
                    var mobile = msg.Keys ?? String.Empty;
                    var data = msg.Body ?? new Byte[0];

                    if (_handler != null)
                        await _handler(mobile, data);
                }
                catch (Exception ex)
                {
                    Log?.Error("RocketMQ 指令消费异常: {0}", ex.Message);
                    return false;
                }
            }
            return true;
        };

        Consumer.Start();
    }

    /// <summary>释放资源</summary>
    public void Dispose()
    {
        if (Consumer?.Active == true)
            Consumer.Stop();
    }

    /// <summary>日志</summary>
    public ILog Log { get; set; } = Logger.Null;
    #endregion
}
