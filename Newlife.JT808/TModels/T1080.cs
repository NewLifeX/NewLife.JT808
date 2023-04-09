using Newlife.JT808.Protocols;

namespace Newlife.JT808.TModels
{
    /// <summary></summary>
    [MessageKind(MessageKinds.T1080)]
    public class T1080
    {
        /// <summary></summary>
        public UInt32 Num { get; set; }
    }
}