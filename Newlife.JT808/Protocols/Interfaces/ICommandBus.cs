namespace NewLife.JT808.Protocols;

/// <summary>指令总线接口，支持本地和跨服务器指令下发</summary>
/// <remarks>
/// 提供统一的指令下发抽象：
/// <list type="bullet">
/// <item>终端在本服务器时直接通过 <see cref="ICommand.Send"/> 下发</item>
/// <item>终端不在本服务器时通过事件总线广播到集群其他服务器</item>
/// </list>
/// </remarks>
public interface ICommandBus
{
    /// <summary>发送指令给终端，不等待响应</summary>
    /// <param name="mobile">终端手机号</param>
    /// <param name="body">消息体对象</param>
    /// <returns>是否成功发送（本地发送成功返回 true，广播到远程返回 true）</returns>
    Boolean Send(String mobile, Object body);

    /// <summary>发送指令并等待终端响应</summary>
    /// <typeparam name="TResult">响应消息体类型</typeparam>
    /// <param name="mobile">终端手机号</param>
    /// <param name="body">请求消息体</param>
    /// <returns>响应消息体，超时或失败返回 null</returns>
    Task<TResult?> SendAsync<TResult>(String mobile, Object body) where TResult : class, new();
}
