using NewLife.Caching;
using NewLife.Net;

namespace NewLife.JT808.Protocols;

/// <summary>会话管理器，维护终端手机号到网络会话的映射</summary>
/// <remarks>用于查找目标设备所在的连接，以支持服务端主动下发指令</remarks>
public class SessionManager
{
    private readonly ICache _cache;

    /// <summary>实例化会话管理器</summary>
    public SessionManager()
    {
        _cache = new MemoryCache { Period = 60, Expire = 1200 };
    }

    /// <summary>注册会话。终端上线时调用</summary>
    /// <param name="mobile">终端手机号</param>
    /// <param name="session">网络会话</param>
    public void Register(String mobile, INetSession session)
    {
        if (mobile.IsNullOrEmpty()) return;

        _cache.Set(mobile, session, 1200);
    }

    /// <summary>移除会话。终端离线时调用</summary>
    /// <param name="mobile">终端手机号</param>
    public void Remove(String mobile)
    {
        if (mobile.IsNullOrEmpty()) return;

        _cache.Remove(mobile);
    }

    /// <summary>根据手机号查找对应的网络会话</summary>
    /// <param name="mobile">终端手机号</param>
    /// <returns>网络会话，未找到返回null</returns>
    public INetSession Find(String mobile)
    {
        if (mobile.IsNullOrEmpty()) return null;

        _cache.TryGetValue<INetSession>(mobile, out var session);
        return session;
    }

    /// <summary>向指定终端发送消息</summary>
    /// <param name="mobile">终端手机号</param>
    /// <param name="body">消息体对象</param>
    /// <returns>是否成功发送</returns>
    public Boolean SendCommand(String mobile, Object body)
    {
        var session = Find(mobile);
        if (session is ICommand cmd)
        {
            cmd.Send(body);
            return true;
        }

        return false;
    }
}
