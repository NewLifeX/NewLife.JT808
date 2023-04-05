using Newlife.JT808.Protocols;

namespace Newlife.JT808.TModels
{
    /// <summary>位置信息查询应答</summary>
    [MessageKind(MessageKinds.位置信息查询应答)]
    public class T0201
    {
        public UInt16 Sequence { get; set; }

        public T0200 Position { get; set; }
    }
}