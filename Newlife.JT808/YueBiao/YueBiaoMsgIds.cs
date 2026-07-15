namespace NewLife.JT808.YueBiao;

/// <summary>粤标透传消息子类型</summary>
/// <remarks>
/// 粤标（广东省主动安全扩展）在 0x0900/0x8900 透传消息中使用的子类型定义。
/// </remarks>
public enum YueBiaoPassthroughTypes : Byte
{
    /// <summary>USB外设状态信息上报。0x0900/0x8900 子类型</summary>
    USB外设状态 = 0xF7,

    /// <summary>USB外设信息上报。0x0900/0x8900 子类型</summary>
    USB外设信息 = 0xF8,
}
