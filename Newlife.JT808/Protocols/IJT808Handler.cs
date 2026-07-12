namespace NewLife.JT808.Protocols;

/// <summary>消息处理器标记接口</summary>
/// <remarks>
/// 标记实现类为 JT808 消息处理器，便于通过 DI 容器批量注册。
/// 具体处理方法通过 <see cref="MessageKindAttribute"/> 特性标记。
/// 配合 <see cref="JT808Server.AddHandler{T}(T)"/> 或 <see cref="JT808Client.AddHandler{T}(T)"/> 使用。
/// </remarks>
public interface IJT808Handler
{
}
