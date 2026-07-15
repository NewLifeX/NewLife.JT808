using NewLife.JT808.Protocols;

namespace NewLife.JT808.YueBiao;

/// <summary>设置终端参数-前向碰撞报警参数（0x8103 参数ID 0xF364）</summary>
public class T8103_F364
{
    /// <summary>参数ID</summary>
    public const UInt32 ParamId = YueBiaoConstants.Param_ADAS;

    /// <summary>报警使能。位标志，bit0=前向碰撞</summary>
    public UInt32 Enable { get; set; }

    /// <summary>报警级别。1=一级，2=二级</summary>
    public Byte Level { get; set; }

    /// <summary>报警速度阈值。单位 km/h</summary>
    public Byte SpeedThreshold { get; set; }

    /// <summary>报警距离阈值。单位 m</summary>
    public Int16 DistanceThreshold { get; set; }

    /// <summary>报警持续时间。单位 s</summary>
    public Byte Duration { get; set; }

    /// <summary>是否主动拍照。0=否，1=是</summary>
    public Byte EnablePhoto { get; set; }

    /// <summary>拍照间隔。单位 s</summary>
    public Byte PhotoInterval { get; set; }

    /// <summary>预留</summary>
    public Byte Reserved { get; set; }
}
