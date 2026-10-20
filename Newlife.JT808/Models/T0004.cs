using NewLife.JT808.Protocols;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 查询服务器时间请求
    /// 2019版本
    /// </summary>
    [MessageKind(MessageKinds.查询服务器时间请求)]
    public class T0004
    {
    }

    /// <summary>
    /// 查询服务器时间应答
    /// 2019版本
    /// </summary>
    [MessageKind(MessageKinds.查询服务器时间应答)]
    public class T8004
    {
        /// <summary>
        /// UTC时间
        /// </summary>
        [BCDTime(6, "yyMMddHHmmss")]
        public DateTime Time { get; set; }
    }
}
