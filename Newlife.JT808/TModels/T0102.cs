using Newlife.JT808.Protocols;
using NewLife.Serialization;

namespace Newlife.JT808.TModels
{
    /// <summary>终端鉴权</summary>
    [MessageKind(MessageKinds.终端鉴权)]
    public class T0102 
    {
        /// <summary>
        /// 鉴权码
        /// 鉴权码内容 2019版本
        /// </summary>
        [FullString2(Version = "2013")]
        [FieldSize(0, Version = "2019")]
        public String Code { get; set; }

        /// <summary>终端IMEI  长度15 2019版本</summary>
        [JT2019]
        [FieldSize(15)]
        public String IMEI { get; set; }

        /// <summary>软件版本号 长度20 后补 "0x00" 2019版本</summary>
        [JT2019]
        [FieldSize(20)]
        public String Version { get; set; }
    }
}