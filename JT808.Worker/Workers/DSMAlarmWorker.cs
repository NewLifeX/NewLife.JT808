using JT808.Data.Safety;
using NewLife;
using NewLife.Caching;
using NewLife.Log;
using NewLife.Model;
using NewLife.Serialization;

namespace JT808.Worker.Workers;

/// <summary>DSM 报警消费 Worker</summary>
public class DSMAlarmWorker : IHostedService
{
    private readonly IProducerConsumer<Object>? _queue;
    private CancellationTokenSource? _cts;
    private volatile Boolean _running;

    public DSMAlarmWorker(QueueService queueService)
    {
        _queue = queueService.Create<Object>("DSMAlarm", "Group");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _running = true;
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _ = ConsumeAsync(_cts.Token);
        XTrace.WriteLine("DSMAlarmWorker 已启动");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _running = false;
        _cts?.Cancel();
        XTrace.WriteLine("DSMAlarmWorker 已停止");
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
                    XTrace.WriteLine("消费 DSM 报警：{0}", data);

                    // 解析 DSM 报警数据并持久化
                    if (data is String json)
                    {
                        var alarm = json.ToJsonEntity<DSMAlarm>();
                        if (alarm != null)
                        {
                            await alarm.InsertAsync();
                            XTrace.WriteLine("DSM 报警已持久化：DeviceId={0}, AlarmType={1}",
                                alarm.DeviceId, alarm.AlarmType);
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
                XTrace.WriteLine("DSMAlarmWorker 异常：{0}", ex.Message);
                await Task.Delay(5000, cancellationToken);
            }
        }
    }
}
