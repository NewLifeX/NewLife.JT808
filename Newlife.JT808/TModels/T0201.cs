using Newlife.JT808.Protocols;

namespace Newlife.JT808.TModels
{
    /// <summary>位置信息查询应答</summary>
    [MessageKind(MessageKinds.位置信息查询应答)]
    public class T0201
    {
        /// <summary>序列</summary>
        public UInt16 Sequence { get; set; }

        /// <summary>位置</summary>
        public T0200 Position { get; set; }
    }
}