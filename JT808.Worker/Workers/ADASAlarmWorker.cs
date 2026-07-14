using NewLife;
using NewLife.Caching;
using NewLife.Log;
using NewLife.Model;

namespace JT808.Worker.Workers;

/// <summary>ADAS 报警消费 Worker</summary>
public class ADASAlarmWorker : IHostedService
{
    private readonly IProducerConsumer<Object>? _queue;
    private CancellationTokenSource? _cts;
    private volatile Boolean _running;

    public ADASAlarmWorker(QueueService queueService)
    {
        _queue = queueService.Create<Object>("ADASAlarm", "Group");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _running = true;
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _ = ConsumeAsync(_cts.Token);
        XTrace.WriteLine("ADASAlarmWorker 已启动");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _running = false;
        _cts?.Cancel();
        XTrace.WriteLine("ADASAlarmWorker 已停止");
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
                    XTrace.WriteLine("消费 ADAS 报警：{0}", data);
                }
                else
                {
                    await Task.Delay(1000, cancellationToken);
                }
            }
            catch (TaskCanceledException) { break; }
            catch (Exception ex)
            {
                XTrace.WriteLine("ADASAlarmWorker 异常：{0}", ex.Message);
                await Task.Delay(5000, cancellationToken);
            }
        }
    }
}
