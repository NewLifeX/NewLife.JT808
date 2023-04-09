using Newlife.JT808.Protocols;

namespace Newlife.JT808.Models
{
    /// <summary>车辆控制</summary>
    [MessageKind(MessageKinds.车辆控制)]
    public class T8500
    {
        /// <summary>用于序列化的时候,由于厂家自定义类型比较多，所以直接用M8500_ControlType</summary>
        public List<UInt16> ControlTypes { get; set; }
    }
}