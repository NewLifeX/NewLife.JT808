using NewLife;
using NewLife.Caching;
using NewLife.Log;
using NewLife.Model;

namespace JT808.Worker.Workers;

/// <summary>位置数据消费 Worker</summary>
/// <remarks>消费 Redis Stream PositionData 主题，处理里程计算、坐标转换。</remarks>
public class PositionWorker : IHostedService
{
    private readonly IProducerConsumer<Object>? _queue;
    private CancellationTokenSource? _cts;
    private volatile Boolean _running;

    public PositionWorker(QueueService queueService)
    {
        _queue = queueService.Create<Object>("PositionData", "Group");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _running = true;
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _ = ConsumeAsync(_cts.Token);
        XTrace.WriteLine("PositionWorker 已启动");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _running = false;
        _cts?.Cancel();
        XTrace.WriteLine("PositionWorker 已停止");
        return Task.CompletedTask;
    }

    private async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        while (_running && !cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (_queue == null) return;

                var data = _queue.TakeOne();
                if (data != null)
                {
                    XTrace.WriteLine("消费位置数据：{0}", data);
                    // TODO: 里程计算、坐标转换、持久化
                }
                else
                {
                    await Task.Delay(1000, cancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                XTrace.WriteLine("PositionWorker 异常：{0}", ex.Message);
                await Task.Delay(5000, cancellationToken);
            }
        }
    }
}
