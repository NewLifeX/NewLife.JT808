namespace NewLife.JT808.YueBiao;

/// <summary>粤标报警/事件类型枚举</summary>
public enum YueBiaoAlarmKinds : Byte
{
    #region 高级驾驶辅助（ADAS）
    /// <summary>前向碰撞报警</summary>
    前向碰撞 = 0x01,

    /// <summary>车道偏离报警</summary>
    车道偏离 = 0x02,

    /// <summary>车距过近报警</summary>
    车距过近 = 0x03,

    /// <summary>行人碰撞报警</summary>
    行人碰撞 = 0x04,

    /// <summary>频繁变道报警</summary>
    频繁变道 = 0x05,

    /// <summary>道路标识超限报警</summary>
    道路标识超限 = 0x06,

    /// <summary>障碍物报警</summary>
    障碍物 = 0x07,

    /// <summary>道路标志识别</summary>
    道路标志识别 = 0x10,

    /// <summary>主动抓拍</summary>
    主动抓拍 = 0x11,
    #endregion

    #region 驾驶状态监测（DSM）
    /// <summary>疲劳驾驶报警</summary>
    疲劳驾驶 = 0x01,

    /// <summary>拨打电话报警</summary>
    拨打电话 = 0x02,

    /// <summary>抽烟报警</summary>
    抽烟 = 0x03,

    /// <summary>分神驾驶报警</summary>
    分神驾驶 = 0x04,

    /// <summary>驾驶员异常报警</summary>
    驾驶员异常 = 0x05,

    /// <summary>探头遮挡报警</summary>
    探头遮挡 = 0x06,

    /// <summary>打哈欠报警</summary>
    打哈欠 = 0x07,

    /// <summary>超时驾驶报警</summary>
    超时驾驶 = 0x08,

    /// <summary>未系安全带报警</summary>
    未系安全带 = 0x0A,

    /// <summary>红外阻断型墨镜失效报警</summary>
    红外阻断 = 0x0B,

    /// <summary>双手脱离方向盘报警</summary>
    双手脱把 = 0x0C,

    /// <summary>玩手机报警</summary>
    玩手机 = 0x0D,

    /// <summary>自动抓拍</summary>
    自动抓拍 = 0x10,

    /// <summary>驾驶员变更</summary>
    驾驶员变更 = 0x11,

    /// <summary>摄像头阻挡报警</summary>
    摄像头阻挡 = 0x13,
    #endregion

    #region 胎压监测（TPMS）
    /// <summary>胎压异常</summary>
    胎压异常 = 0x01,

    /// <summary>胎温异常</summary>
    胎温异常 = 0x02,

    /// <summary>传感器异常</summary>
    传感器异常 = 0x03,

    /// <summary>胎压不平衡</summary>
    胎压不平衡 = 0x04,

    /// <summary>电池电量低</summary>
    电池电量低 = 0x05,
    #endregion

    #region 盲区监测（BSD）
    /// <summary>后方接近报警</summary>
    后方接近 = 0x01,

    /// <summary>左侧后方接近报警</summary>
    左侧后方接近 = 0x02,

    /// <summary>右侧后方接近报警</summary>
    右侧后方接近 = 0x03,
    #endregion
}
