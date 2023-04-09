using NewLife.JT808.Protocols;

namespace NewLife.JT808.Models
{
    /// <summary>设置终端参数</summary>
    [MessageKind(MessageKinds.设置终端参数)]
    public class T8103
    {
        /// <summary>参数</summary>
        public List<TLV> Items { get; set; }
    }
}