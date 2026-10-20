using NewLife;
using NewLife.Log;
using NewLife.Net;
using NewLife.JT808.Models;

namespace NewLife.JT808.Protocols;

/// <summary>JT/T 808服务端会话</summary>
public class JT808Session : NetSession<JT808Server>, ICommand
{
    /// <summary>粘包编解码器</summary>
    public JTCodec Codec { get; set; } = new();

    /// <summary>当前设备手机号</summary>
    public String Mobile { get; set; }

    /// <summary>协议版本号。0=2013版，1=2019版</summary>
    public Byte Version { get; set; }

    private Int32 _gid;

    /// <summary>连接建立时触发</summary>
    protected override void OnConnected()
    {
        base.OnConnected();

        WriteLog("Terminal connected {0}", Remote);
    }

    /// <summary>连接断开时触发</summary>
    /// <param name="reason"></param>
    protected override void OnDisconnected(String reason)
    {
        base.OnDisconnected(reason);

        // 从会话管理器移除
        if (!Mobile.IsNullOrEmpty())
            Host.Manager.Remove(Mobile);

        WriteLog("Terminal disconnected {0} {1}", Remote, reason);
    }

    /// <summary>收到消息时触发</summary>
    /// <param name="e"></param>
    protected override void OnReceive(ReceivedEventArgs e)
    {
        // 使用JTCodec解码粘包
        var list = Codec.Decode(e.Packet);
        if (list == null || list.Count == 0) return;

        foreach (var rawMsg in list)
        {
            var result = Host.Merge(this, rawMsg);

            if (result != null)
            {
                Mobile = result.Mobile;
                Version = result.Version;

                WriteLog("<= {0}", result);
            }

            // 分包合并未完成时，告知终端继续发送
            if (result == null && rawMsg != null)
            {
                var response = new T8001
                {
                    Sequence = rawMsg.Sequence,
                    Kind = rawMsg.Kind,
                    Result = ResultKinds.Success
                };

                SendJT808(response);
            }

            if (result != null)
            {
                ProcessMessage(result);
            }
        }
    }

    /// <summary>处理收到的完整消息</summary>
    /// <param name="msg">完整的JTMessage（分包已合并）</param>
    protected virtual void ProcessMessage(JTMessage msg)
    {
        // 黑名单设备直接断开
        if (!Mobile.IsNullOrEmpty() && Host.Blacklist.IsBlacklisted(Mobile))
        {
            WriteLog("Blacklisted device rejected: {0}", Mobile);
            Close("Blacklisted device");
            return;
        }

        // 任意消息都代表终端活跃，刷新会话有效期
        if (!Mobile.IsNullOrEmpty()) Host.Manager.Touch(Mobile);

        var name = MessageFactory.GetKindName(msg.Kind);
        using var span = Host.Tracer?.NewSpan($"jt808:{name}", msg);

        try
        {
            // 路由到处理器
            if (!Host.Handlers.TryGetValue(msg.Kind, out var handler))
            {
                WriteLog("Unsupported command [{0}]", name);
                return;
            }

            // 执行处理器
            var result = handler(this, msg);

            // 处理响应
            if (result != null)
            {
                if (result is T8001 response)
                {
                    response.Kind = msg.Kind;
                    response.Sequence = msg.Sequence;
                }

                SendJT808(result);
            }
        }
        catch (Exception ex)
        {
            span?.SetError(ex, null);
            XTrace.WriteException(ex);

            // 返回失败应答
            var response = new T8001
            {
                Sequence = msg.Sequence,
                Kind = msg.Kind,
                Result = ex switch
                {
                    NotSupportedException => ResultKinds.NotSupport,
                    InvalidDataException => ResultKinds.Error,
                    _ => ResultKinds.Failure,
                }
            };

            SendJT808(response);
        }
    }

    /// <summary>发送JT808消息给终端</summary>
    /// <param name="body">消息体对象</param>
    public virtual void SendJT808(Object body)
    {
        var msg = new JTMessage
        {
            Mobile = Mobile,
            Version = Version,
            Sequence = (UInt16)Interlocked.Increment(ref _gid),
        };

        msg.SetResult(body);

        WriteLog("=> {0}", msg);

        SendMessage(msg);
    }

    /// <summary>发送消息。实现ICommand接口</summary>
    /// <param name="body"></param>
    void ICommand.Send(Object body) => SendJT808(body);

    /// <summary>异步发送并等待响应。实现ICommand接口</summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="body"></param>
    /// <returns></returns>
    async Task<TResult> ICommand.SendAsync<TResult>(Object body)
    {
        var msg = new JTMessage
        {
            Mobile = Mobile,
            Version = Version,
            Sequence = (UInt16)Interlocked.Increment(ref _gid),
        };

        msg.SetResult(body);

        WriteLog("=> {0}", msg);

        var rs = await SendMessageAsync(msg);

        return (rs as JTMessage)?.ReadModel<TResult>();
    }
}
