namespace NewLife.JT808.JT1078;

/// <summary>关闭音视频通道类型</summary>
public enum CloseKinds : Byte
{
    /// <summary>关闭该通道有关的音视频数据</summary>
    All = 0,

    /// <summary>只关闭音频，保留视频</summary>
    AudioOnly = 1,

    /// <summary>只关闭视频，保留音频</summary>
    VideoOnly = 2,
}
