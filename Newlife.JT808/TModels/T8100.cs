using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    /// <summary>终端注册返回结果</summary>
    public enum RegisterResult : Byte
    {
        /// <summary>成功</summary>
        成功 = 0x00,

        /// <summary>车辆已被注册</summary>
        车辆已被注册 = 0x01,

        /// <summary>数据库中无该车辆</summary>
        数据库中无该车辆 = 0x02,

        /// <summary>终端已被注册</summary>
        终端已被注册 = 0x03,

        /// <summary>数据库中无该终端</summary>
        数据库中无该终端 = 0x04
    }

    /// <summary>终端注册应答</summary>
    [MessageKind(MessageKinds.终端注册应答)]
    public class T8100
    {
        /// <summary>
        /// 应答流水号
        /// 对应的终端注册消息的流水号
        /// </summary>
        public UInt16 Sequence { get; set; }

        /// <summary>结果</summary>
        public RegisterResult Result { get; set; }

        /// <summary>
        /// 鉴权码
        /// 只有在成功后才有该字段
        /// </summary>
        [FullString]
        public String Code { get; set; }
    }
}