using NewLife.JT808.Protocols;

namespace NewLife.JT808.JT1078;

/// <summary>终端上传音视频属性</summary>
/// <remarks>
/// 终端响应平台查询音视频属性时上传，包含音频/视频编解码能力、通道数等。
/// 消息ID：0x1003
/// </remarks>
[MessageKind(MessageKinds.终端上传音视频属性)]
public class T1003
{
    /// <summary>音频编码方式</summary>
    public AudioCodecs AudioCodec { get; set; }

    /// <summary>音频声道数</summary>
    public Byte AudioChannels { get; set; }

    /// <summary>音频采样率。0：8 kHz； 1：22.05 kHz； 2：44.1 kHz； 3：48 kHz</summary>
    public AudioRates AudioRate { get; set; }

    /// <summary>音频采样位数</summary>
    public AudioBits AudioBit { get; set; }

    /// <summary>音频帧长度</summary>
    public UInt16 AudioFrameLength { get; set; }

    /// <summary>是否支持音频输出。0：不支持；1：支持</summary>
    public Byte AudioOutput { get; set; }

    /// <summary>视频编码方式</summary>
    public VideoCodecs VideoCodec { get; set; }

    /// <summary>终端支持的最大音频物理通道数量</summary>
    public Byte MaxAudioChannels { get; set; }

    /// <summary>终端支持的最大视频物理通道数量</summary>
    public Byte MaxVideoChannels { get; set; }
}
