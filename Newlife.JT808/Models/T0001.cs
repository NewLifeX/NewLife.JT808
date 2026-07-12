using NewLife.JT808.Protocols;

namespace NewLife.JT808.Models
{
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

    /// <summary>
    /// 终端通用应答
    /// </summary>
    [MessageKind(MessageKinds.终端通用应答)]
    public class T0001
    {
        /// <summary>
        /// 应答流水号
        /// 对应的平台消息的流水号
        /// </summary>
        public ushort Sequence { get; set; }

        /// <summary>
        /// 应答 ID
        /// 对应的平台消息的 ID
        /// </summary>
        public MessageKinds Kind { get; set; }

        /// <summary>
        /// 结果
        /// 0：成功/确认；1：失败；2：消息有误；3：不支持
        /// </summary>
        public ResultKinds Result { get; set; }

        /// <summary>已重载。</summary>
        /// <returns></returns>
        public override string ToString() => $"Response [{Kind}] {Result}";
    }
}