using NewLife.Caching;
using NewLife.Log;
using NewLife.Net;

namespace NewLife.JT808.Protocols;

/// <summary>会话管理器，维护终端手机号到网络会话的映射</summary>
/// <remarks>
/// 支持本地指令下发和跨服务器指令广播。
/// 当终端设备不在当前服务器时，通过 <see cref="CommandReceived"/> 事件通知上层，
/// 由上层（如 Stardust 事件总线）将指令广播到集群中其他服务器。
/// 
/// 使用示例：
/// <code>
/// var mgr = new SessionManager();
/// 
/// // 本地下发
/// mgr.Send("13800138000", new T8300 { ... });
/// 
/// // 跨服务器广播（需上层接入星尘事件总线）
/// mgr.CommandReceived += (s, e) => {
///     var evt = new CommandEvent { Mobile = e.Mobile, ... };
///     eventBus.PublishAsync(evt);
/// };
/// </code>
/// </remarks>
public class SessionManager : ICommandBus
{
    #region 属性
    private readonly ICache _cache;

    /// <summary>指令日志</summary>
    public ILog Log { get; set; } = Logger.Null;

    /// <summary>当本地未找到目标设备时触发，由上层负责广播到远程服务器</summary>
    public event EventHandler<CommandEventArgs>? CommandReceived;
    #endregion

    #region 构造
    /// <summary>实例化会话管理器</summary>
    public SessionManager()
    {
        _cache = new MemoryCache { Period = 60, Expire = 1200 };
    }
    #endregion

    #region 会话管理
    /// <summary>注册会话。终端上线时调用</summary>
    /// <param name="mobile">终端手机号</param>
    /// <param name="session">网络会话</param>
    public void Register(String mobile, INetSession session)
    {
        if (mobile.IsNullOrEmpty()) return;

        _cache.Set(mobile, session, 1200);

        Log?.Info("会话注册：Mobile={0}, Session={1}", mobile, session);
    }

    /// <summary>移除会话。终端离线时调用</summary>
    /// <param name="mobile">终端手机号</param>
    public void Remove(String mobile)
    {
        if (mobile.IsNullOrEmpty()) return;

        _cache.Remove(mobile);

        Log?.Info("会话移除：Mobile={0}", mobile);
    }

    /// <summary>根据手机号查找对应的网络会话</summary>
    /// <param name="mobile">终端手机号</param>
    /// <returns>网络会话，未找到返回null</returns>
    public INetSession? Find(String mobile)
    {
        if (mobile.IsNullOrEmpty()) return null;

        _cache.TryGetValue<INetSession>(mobile, out var session);
        return session;
    }

    /// <summary>判断终端是否在线（连接到本服务器）</summary>
    /// <param name="mobile">终端手机号</param>
    /// <returns>是否在线</returns>
    public Boolean IsOnline(String mobile) => Find(mobile) != null;
    #endregion

    #region ICommandBus 实现
    /// <summary>向指定终端发送指令，不等待响应</summary>
    /// <remarks>
    /// 优先查找本地会话，找到则直接下发；
    /// 未找到时触发 <see cref="CommandReceived"/> 事件，由上层广播到远程服务器。
    /// </remarks>
    /// <param name="mobile">终端手机号</param>
    /// <param name="body">消息体对象</param>
    /// <returns>本地发送成功或在集群中广播成功返回 true</returns>
    public Boolean Send(String mobile, Object body)
    {
        // 优先本地查找
        var session = Find(mobile);
        if (session is ICommand cmd)
        {
            cmd.Send(body);
            Log?.Info("指令本地下发：Mobile={0}, Body={1}", mobile, body);
            return true;
        }

        // 本地未找到，触发远程广播事件
        var args = new CommandEventArgs(mobile, body);
        CommandReceived?.Invoke(this, args);

        if (args.Handled)
        {
            Log?.Info("指令远程广播：Mobile={0}, Body={1}", mobile, body);
            return true;
        }

        Log?.Warn("指令下发失败，设备不在线：Mobile={0}", mobile);
        return false;
    }

    /// <summary>向指定终端发送指令并等待响应</summary>
    /// <typeparam name="TResult">响应消息体类型</typeparam>
    /// <param name="mobile">终端手机号</param>
    /// <param name="body">请求消息体</param>
    /// <returns>响应消息体，超时或失败返回 null</returns>
    public async Task<TResult?> SendAsync<TResult>(String mobile, Object body) where TResult : class, new()
    {
        var session = Find(mobile);
        if (session is ICommand cmd)
        {
            try
            {
                var result = await cmd.SendAsync<TResult>(body);
                Log?.Info("指令本地下发（异步）：Mobile={0}, Body={1}", mobile, body);
                return result;
            }
            catch (Exception ex)
            {
                Log?.Error("指令本地下发异常：Mobile={0}", mobile, ex);
                return null;
            }
        }

        // 异步下发也支持远程广播（但不等待响应）
        var args = new CommandEventArgs(mobile, body);
        CommandReceived?.Invoke(this, args);

        if (args.Handled)
        {
            Log?.Info("指令远程广播（异步）：Mobile={0}, Body={1}", mobile, body);
        }
        else
        {
            Log?.Warn("指令下发失败，设备不在线：Mobile={0}", mobile);
        }

        return null;
    }
    #endregion

    #region 远程指令处理
    /// <summary>处理来自远程服务器的指令下发事件</summary>
    /// <remarks>
    /// 由事件总线处理器调用，将远程广播的指令在本服务器上执行。
    /// </remarks>
    /// <param name="mobile">目标终端手机号</param>
    /// <param name="body">消息体对象</param>
    /// <returns>是否成功发送给本地终端</returns>
    public Boolean ProcessRemoteCommand(String mobile, Object body)
    {
        var session = Find(mobile);
        if (session is ICommand cmd)
        {
            cmd.Send(body);
            Log?.Info("远程指令本地执行：Mobile={0}, Body={1}", mobile, body);
            return true;
        }

        Log?.Warn("远程指令本地执行失败，设备已离线：Mobile={0}", mobile);
        return false;
    }
    #endregion
}
