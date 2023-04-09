using NewLife.JT808.Protocols;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 查询指定终端参数
    /// 0x8106
    /// </summary>
    [MessageKind(MessageKinds.查询指定终端参数)]
    public class T8106
    {
        /// <summary>
        /// 参数 ID 列表
        /// 参数顺序排列，如“参数 ID1 参数 ID2......参数IDn”。
        /// </summary>
        public UInt32[] Parameters { get; set; }
    }
}