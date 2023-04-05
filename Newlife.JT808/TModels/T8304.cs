using Newlife.JT808.Protocols;
using NewLife.Serialization;

namespace Newlife.JT808.Models
{
    /// <summary>
    /// 信息服务
    /// 0x8304
    /// 2019版本已作删除
    /// </summary>
    [MessageKind(MessageKinds.信息服务)]
    public class T8304
    {
        /// <summary>
        /// 信息类型
        /// </summary>
        public Byte Type { get; set; }

        /// <summary>
        /// 信息长度
        /// </summary>
        public UInt16 Length { get; set; }

        /// <summary>
        /// 信息内容
        /// 经 GBK 编码
        /// </summary>
        [FieldSize(nameof(Length))]
        public String Content { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="reader"></param>
        ///// <param name="config"></param>
        ///// <returns></returns>
        //public Boolean Read(Stream stream, Object context)
        //{
        //    var reader = new BinaryReader(stream);
        //    Type = reader.ReadByte();
        //    Length = reader.ReadByte();
        //    Content = reader.ReadString();

        //    return true;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="writer"></param>
        ///// <param name="value"></param>
        ///// <param name="config"></param>
        //public Boolean Write(Stream stream, Object context)
        //{
        //    var writer = new BinaryWriter(stream);
        //    writer.Write(Type);
        //    writer.Write(Length);
        //    writer.Write(Content);

        //    return true;
        //}
    }
}