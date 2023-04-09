using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    /// <summary>升级类型</summary>
    public enum UpgradeType : Byte
    {
        /// <summary>终端</summary>
        终端 = 0,

        /// <summary>道路运输证IC卡读卡器</summary>
        道路运输证IC卡读卡器 = 0x0C,

        /// <summary>北斗卫星定位模块</summary>
        北斗卫星定位模块 = 0x34,

        /// <summary>粤标主动安全-高级驾驶辅助系统</summary>
        高级驾驶辅助系统 = 0x64,

        /// <summary>粤标主动安全-驾驶状态监控系统</summary>
        驾驶状态监控系统 = 0x65,

        /// <summary>粤标主动安全-胎压监测系统</summary>
        胎压监测系统 = 0x66,

        /// <summary>粤标主动安全-盲点监测系统</summary>
        盲点监测系统 = 0x67,
    }

    /// <summary>下发终端升级包</summary>
    [MessageKind(MessageKinds.下发终端升级包)]
    public class T8108 //: IAccessor
    {
        /// <summary>升级类型</summary>
        public UpgradeType UpgradeType { get; set; }

        /// <summary>
        /// 制造商 ID
        /// 2013版本 5 个字节，终端制造商编码
        /// 2019版本 11 个字节，终端制造商编码
        /// </summary>
        [FieldSize(5, Version = "2013")]
        [FieldSize(11, Version = "2019")]
        public String MakerId { get; set; }

        /// <summary>版本号</summary>
        public String Version { get; set; }

        /// <summary>升级数据包</summary>
        public Byte[] Data { get; set; }
    }
}