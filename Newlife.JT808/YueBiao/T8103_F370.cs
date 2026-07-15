using NewLife.JT808.Protocols;

namespace NewLife.JT808.YueBiao;

/// <summary>设置终端参数-盲区监测参数（0x8103 参数ID 0xF370）</summary>
public class T8103_F370
{
    /// <summary>参数ID</summary>
    public const UInt32 ParamId = YueBiaoConstants.Param_BSD;

    /// <summary>报警使能。位标志</summary>
    public UInt32 Enable { get; set; }

    /// <summary>报警级别。1=一级，2=二级</summary>
    public Byte Level { get; set; }

    /// <summary>报警速度阈值。单位 km/h</summary>
    public Byte SpeedThreshold { get; set; }

    /// <summary>预留</summary>
    public Byte Reserved { get; set; }
}
