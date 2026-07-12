namespace NewLife.JT808.Protocols;

/// <summary>指令事件参数，用于本地/跨服务器指令下发的消息传递</summary>
public class CommandEventArgs : EventArgs
{
    /// <summary>实例化指令事件参数</summary>
    /// <param name="mobile">终端手机号</param>
    /// <param name="body">消息体对象</param>
    public CommandEventArgs(String mobile, Object body)
    {
        Mobile = mobile;
        Body = body;
    }

    /// <summary>终端手机号</summary>
    public String Mobile { get; }

    /// <summary>消息体对象</summary>
    public Object Body { get; }

    /// <summary>是否已由远程处理器处理</summary>
    public Boolean Handled { get; set; }
}
