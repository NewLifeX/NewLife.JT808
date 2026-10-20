using NewLife.JT808.Protocols;
using NewLife.Log;
using NewLife.Model;

namespace JT808.Worker.Workers;

/// <summary>流消息消费 Worker 基类</summary>
/// <remarks>
/// 通过 <see cref="IMessageBus"/> 创建消费者并订阅主题，子类只实现消息处理逻辑。
/// 消费端默认使用 Redis Stream，与网关生产端保持同一主题与消费组。
/// </remarks>
public abstract class StreamConsumerWorker : IHostedService
{
    private readonly IMsgConsumer<Object>? _consumer;

    /// <summary>主题名称</summary>
    public String Topic { get; }

    /// <summary>实例化</summary>
    /// <param name="bus">消息总线</param>
    /// <param name="topic">主题</param>
    protected StreamConsumerWorker(IMessageBus bus, String topic)
    {
        Topic = topic;
        _consumer = bus.CreateConsumer<Object>(topic);
    }

    /// <summary>启动</summary>
    /// <param name="cancellationToken">取消令牌</param>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        XTrace.WriteLine("{0} 已启动，主题：{1}", GetType().Name, Topic);

        return _consumer?.SubscribeAsync(OnMessageAsync, cancellationToken) ?? Task.CompletedTask;
    }

    /// <summary>停止</summary>
    /// <param name="cancellationToken">取消令牌</param>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        XTrace.WriteLine("{0} 已停止", GetType().Name);

        return _consumer?.UnsubscribeAsync(cancellationToken) ?? Task.CompletedTask;
    }

    /// <summary>处理一条消息</summary>
    /// <param name="mobile">终端手机号。Redis 载荷不单独携带</param>
    /// <param name="data">消息体</param>
    protected abstract Task ProcessAsync(String mobile, Object data);

    private async Task OnMessageAsync(String mobile, Object data)
    {
        try
        {
            await ProcessAsync(mobile, data).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            XTrace.WriteLine("{0} 处理异常：{1}", GetType().Name, ex.Message);
        }
    }
}