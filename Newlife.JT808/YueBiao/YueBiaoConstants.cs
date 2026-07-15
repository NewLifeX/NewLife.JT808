namespace NewLife.JT808.YueBiao;

/// <summary>粤标常量</summary>
public static class YueBiaoConstants
{
    #region 参数ID（0x8103）
    /// <summary>主动安全参数-前向碰撞报警参数</summary>
    public const UInt32 Param_ADAS = 0xF364;

    /// <summary>主动安全参数-车道偏离报警参数</summary>
    public const UInt32 Param_LaneDeparture = 0xF365;

    /// <summary>主动安全参数-车距过近报警参数</summary>
    public const UInt32 Param_NearDistance = 0xF366;

    /// <summary>主动安全参数-行人碰撞报警参数</summary>
    public const UInt32 Param_PedestrianCollision = 0xF367;

    /// <summary>主动安全参数-疲劳驾驶报警参数</summary>
    public const UInt32 Param_FatigueDriving = 0xF368;

    /// <summary>主动安全参数-拨打电话报警参数</summary>
    public const UInt32 Param_PhoneCall = 0xF369;

    /// <summary>主动安全参数-抽烟报警参数</summary>
    public const UInt32 Param_Smoking = 0xF36A;

    /// <summary>主动安全参数-分神驾驶报警参数</summary>
    public const UInt32 Param_DistractedDriving = 0xF36B;

    /// <summary>主动安全参数-驾驶员异常报警参数</summary>
    public const UInt32 Param_DriverAbnormal = 0xF36C;

    /// <summary>主动安全参数-超时驾驶报警参数</summary>
    public const UInt32 Param_OvertimeDriving = 0xF36D;

    /// <summary>主动安全参数-设备自检参数</summary>
    public const UInt32 Param_SelfCheck = 0xF36E;

    /// <summary>主动安全参数-车辆信息设置</summary>
    public const UInt32 Param_VehicleInfo = 0xF36F;

    /// <summary>主动安全参数-盲区监测参数</summary>
    public const UInt32 Param_BSD = 0xF370;
    #endregion

    #region 透传消息子类型（0x0900/0x8900）
    /// <summary>USB外设状态信息上报</summary>
    public const Byte Passthrough_USBStatus = 0xF7;

    /// <summary>USB外设信息上报</summary>
    public const Byte Passthrough_USBInfo = 0xF8;
    #endregion

    #region 报警附件协议终端ID长度
    /// <summary>苏标终端ID长度</summary>
    public const Int32 TerminalIdLength_SuBiao = 7;

    /// <summary>粤标终端ID长度</summary>
    public const Int32 TerminalIdLength_YueBiao = 30;

    /// <summary>粤标报警标识号长度</summary>
    public const Int32 AlarmIdLength_YueBiao = 40;
    #endregion
}
