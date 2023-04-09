using Newlife.JT808.Protocols;

namespace Newlife.JT808.Models
{
    /// <summary>
    /// 删除圆形区域
    /// 0x8601
    /// </summary>
    [MessageKind(MessageKinds.删除圆形区域)]
    public class T8601
    {
        /// <summary>区域ID集合</summary>
        public List<UInt32> AreaIds { get; set; }
    }
}