using NewLife.JT808.Protocols;

namespace NewLife.JT808.Models;

/// <summary>返回结果</summary>
public enum ResultKinds : byte
{
    /// <summary>成功/确认</summary>
    Success = 0x00,

    /// <summary>失败</summary>
    Failure = 0x01,

    /// <summary>消息有误</summary>
    Error = 0x02,

    /// <summary>不支持</summary>
    NotSupport = 0x03,

    /// <summary>报警处理确认</summary>
    Alarm = 0x04
}
