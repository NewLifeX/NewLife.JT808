namespace NewLife.JT808.JT1078;

/// <summary>流控制指令</summary>
public enum StreamCommands : Byte
{
    /// <summary>关闭音视频传输</summary>
    Close = 0,

    /// <summary>切换码流</summary>
    SwitchStream = 1,

    /// <summary>暂停该通道所有流的发送</summary>
    Pause = 2,

    /// <summary>恢复暂停前流的发送</summary>
    Resume = 3,

    /// <summary>关闭双向对讲</summary>
    CloseDuplex = 4,
}
