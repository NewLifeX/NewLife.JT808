namespace NewLife.JT808.JT1078;

/// <summary>扩展视频报警标志位</summary>
[Flags]
public enum VideoAlarmKinds : UInt32
{
    /// <summary>视频信号丢失报警</summary>
    视频信号丢失 = 0x01,

    /// <summary>视频信号遮挡报警</summary>
    视频信号遮挡 = 0x02,

    /// <summary>存储器故障报警</summary>
    存储器故障 = 0x04,

    /// <summary>异常驾驶行为报警详细描述</summary>
    异常驾驶行为 = 0x08,
}
