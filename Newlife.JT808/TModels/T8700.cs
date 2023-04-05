
using Newlife.JT808.Protocols;

namespace Newlife.JT808.Models;

/// <summary>行驶记录仪命令</summary>
public enum RecordCommands : Byte
{
    驾驶员号码及驾驶证号码 = 0x01,
    实时时钟 = 0x02,
    最近360h累计里程 = 0x03,
    车辆特征系数 = 0x04,
    最近360h行驶速度 = 0x05,
    车辆VIN车牌及分类 = 0x06,
    事故疑点 = 0x07,
    最近2天累计行驶里程 = 0x08,
    最近2天行驶速度 = 0x09,
    最近2天驾驶超3小时 = 0x10,
}

/// <summary>
/// 行驶记录数据采集命令
/// 对标 GB/T 19056
/// </summary>
[MessageKind(MessageKinds.行驶记录数据采集命令)]
public class T8700
{
    /// <summary>
    /// 命令Id
    /// </summary>
    public RecordCommands CommandId { get; set; }

    /// <summary>
    /// 行车记录仪下行数据包
    /// </summary>
    public Byte[] Data { get; set; }
}