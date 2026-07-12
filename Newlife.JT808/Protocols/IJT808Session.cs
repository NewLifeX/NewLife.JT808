namespace NewLife.JT808.Protocols;

/// <summary>JT/T 808会话接口，定义与设备连接的抽象操作</summary>
/// <remarks>
/// 服务端和客户端共享的会话抽象。服务端侧由 <see cref="JT808Session"/> 实现，
/// 客户端侧由 <see cref="JT808Client"/> 实现。
/// </remarks>
public interface IJT808Session
{
    /// <summary>终端手机号</summary>
    String Mobile { get; set; }

    /// <summary>发送消息，不等待响应</summary>
    /// <param name="body">消息体对象</param>
    void Send(Object body);

    /// <summary>发送消息并等待响应</summary>
    /// <typeparam name="TResult">响应消息体类型</typeparam>
    /// <param name="body">请求消息体</param>
    /// <returns>响应消息体</returns>
    Task<TResult> SendAsync<TResult>(Object body) where TResult : class, new();
}
