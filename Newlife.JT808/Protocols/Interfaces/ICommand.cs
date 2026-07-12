namespace NewLife.JT808.Protocols;

/// <summary>设备命令接口，用于服务端向已连接的终端设备下发指令</summary>
public interface ICommand
{
    /// <summary>发送消息给终端，不等待响应</summary>
    /// <param name="body">消息体对象</param>
    void Send(Object body);

    /// <summary>发送消息并等待终端响应</summary>
    /// <typeparam name="TResult">响应消息体类型</typeparam>
    /// <param name="body">请求消息体</param>
    /// <returns></returns>
    Task<TResult> SendAsync<TResult>(Object body) where TResult : class, new();
}
