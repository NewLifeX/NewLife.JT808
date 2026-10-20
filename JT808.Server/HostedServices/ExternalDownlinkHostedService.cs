using System.Text;
using JT808.Server.Messaging;
using JT808.Server.Services;
using NewLife;
using NewLife.JT808.Protocols;
using NewLife.Log;
using NewLife.Serialization;

namespace JT808.Server.HostedServices;

/// <summary>外部下行指令接入服务</summary>
/// <remarks>
/// 启用外部消息集成后，从外部主题（RocketMQ/Kafka）消费 <see cref="JTCommand"/> JSON 指令，
/// 交由 <see cref="CommandExecutor"/> 在本地会话执行。
/// 消息 Key 携带终端手机号，消息体为指令 JSON；指令体内缺少手机号时以 Key 补齐。
/// </remarks>
public class ExternalDownlinkHostedService : IHostedService
{
    private readonly MessagingRuntime _runtime;
    private readonly CommandExecutor _executor;

    /// <summary>实例化</summary>
    /// <param name="runtime">消息总线运行时</param>
    /// <param name="executor">指令执行器</param>
    public ExternalDownlinkHostedService(MessagingRuntime runtime, CommandExecutor executor)
    {
        _runtime = runtime;
        _executor = executor;
    }

    /// <summary>启动</summary>
    /// <param name="cancellationToken">取消令牌</param>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var subscriber = _runtime.DownSubscriber;
        if (subscriber == null)
        {
            XTrace.WriteLine("外部下行指令未启用");
            return Task.CompletedTask;
        }

        subscriber.Start(OnMessage);
        XTrace.WriteLine("外部下行指令已接入：{0}", _runtime.Options.External);

        return Task.CompletedTask;
    }

    /// <summary>停止</summary>
    /// <param name="cancellationToken">取消令牌</param>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        (_runtime.DownHandler as IDisposable)?.Dispose();
        return Task.CompletedTask;
    }

    private Task<Byte[]> OnMessage(String mobile, Byte[] data)
    {
        if (data == null || data.Length == 0) return Task.FromResult(Array.Empty<Byte>());

        try
        {
            var json = Encoding.UTF8.GetString(data);
            var cmd = JsonHelper.ToJsonEntity<JTCommand>(json);
            if (cmd == null)
            {
                XTrace.WriteLine("外部下行指令解析失败：{0}", json);
                return Task.FromResult(Array.Empty<Byte>());
            }

            if (cmd.Mobile.IsNullOrEmpty()) cmd.Mobile = mobile;

            var rs = _executor.Execute(cmd);
            return Task.FromResult(Encoding.UTF8.GetBytes(rs ? "ok" : "failed"));
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);
            return Task.FromResult(Array.Empty<Byte>());
        }
    }
}
