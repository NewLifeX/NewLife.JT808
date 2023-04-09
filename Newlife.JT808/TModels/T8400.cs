using Newlife.JT808.Protocols;
using NewLife.Serialization;

namespace Newlife.JT808.Models
{
    /// <summary>电话回拨</summary>
    [MessageKind(MessageKinds.电话回拨)]
    public class T8400
    {
        /// <summary>0:普通通话；1:监听</summary>
        public Byte CallBack { get; set; }

        /// <summary>
        /// 电话号码 
        /// 最长为 20 字节
        /// </summary>
        [FieldSize(20)]
        public String PhoneNumber { get; set; }
    }
}