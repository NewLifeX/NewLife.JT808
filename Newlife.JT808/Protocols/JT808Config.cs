using NewLife;

namespace NewLife.JT808.Protocols;

/// <summary>JT/T 808协议版本</summary>
public enum JT808Version : Byte
{
    /// <summary>JT/T 808-2011</summary>
    JT2011 = 0,

    /// <summary>JT/T 808-2013</summary>
    JT2013 = 0,

    /// <summary>JT/T 808-2019</summary>
    JT2019 = 1,
}

/// <summary>JT/T 808协议配置</summary>
public class JT808Config
{
    /// <summary>协议版本</summary>
    public JT808Version Version { get; set; } = JT808Version.JT2013;

    /// <summary>协议版本号标识。上下级平台之间采用的标准协议版本编号，长度3字节，0x01020F表示v1.2.15</summary>
    public UInt32 VersionFlag { get; set; } = 0x01020F;

    /// <summary>加密密钥M1</summary>
    public UInt32 M1 { get; set; }

    /// <summary>加密密钥IA1</summary>
    public UInt32 IA1 { get; set; }

    /// <summary>加密密钥IC1</summary>
    public UInt32 IC1 { get; set; }

    /// <summary>跳过校验和验证。默认false，校验和不匹配时仅记录日志</summary>
    public Boolean SkipCRCCode { get; set; }

    /// <summary>克隆配置</summary>
    /// <returns></returns>
    public JT808Config Clone() => MemberwiseClone() as JT808Config;
}
