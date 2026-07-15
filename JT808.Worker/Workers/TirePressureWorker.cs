using JT808.Data;
using NewLife;
using NewLife.Caching;
using NewLife.Log;
using NewLife.Model;
using NewLife.Serialization;

namespace JT808.Worker.Workers;

/// <summary>胎压数据消费 Worker</summary>
public class TirePressureWorker : IHostedService
{
    private readonly IProducerConsumer<Object>? _queue;
    private CancellationTokenSource? _cts;
    private volatile Boolean _running;

    public TirePressureWorker(QueueService queueService)
    {
        _queue = queueService.Create<Object>("TirePressure", "Group");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _running = true;
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _ = ConsumeAsync(_cts.Token);
        XTrace.WriteLine("TirePressureWorker 已启动");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _running = false;
        _cts?.Cancel();
        XTrace.WriteLine("TirePressureWorker 已停止");
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
                    XTrace.WriteLine("消费胎压数据：{0}", data);

                    // 解析胎压数据并持久化
                    if (data is String json)
                    {
                        var tp = json.ToJsonEntity<TirePressure>();
                        if (tp != null)
                        {
                            await tp.InsertAsync();
                            XTrace.WriteLine("胎压数据已持久化：DeviceId={0}", tp.DeviceId);
                        }
                    }
                }
                else
                {
                    await Task.Delay(1000, cancellationToken);
                }
            }
            catch (TaskCanceledException) { break; }
            catch (Exception ex)
            {
                XTrace.WriteLine("TirePressureWorker 异常：{0}", ex.Message);
                await Task.Delay(5000, cancellationToken);
            }
        }
    }
}
