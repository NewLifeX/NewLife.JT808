using NewLife.JT808.Protocols;

namespace NewLife.JT808.TModels
{
    /// <summary>车辆控制应答</summary>
    [MessageKind(MessageKinds.车辆控制应答)]
    public class T0500
    {
        /// <summary>
        /// 应答流水号
        /// 对应的终端注册消息的流水号
        /// </summary>
        public UInt16 MsgNum { get; set; }

        /// <summary>
        /// 位置信息汇报消息体
        /// </summary>
        public T0200 Position { get; set; }
    }
}