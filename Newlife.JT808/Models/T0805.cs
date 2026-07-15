using NewLife.JT808.Protocols;
using NewLife.JT808.Protocols;

namespace NewLife.JT808.Models
{
    /// <summary>
    /// 摄像头立即拍摄命令应答
    /// 0x0805
    /// </summary>
    [MessageKind(MessageKinds.摄像头立即拍摄命令应答)]
    public class T0805
    {
        /// <summary>
        /// 应答流水号
        /// 对应平台摄像头立即拍摄命令的消息流水号
        /// </summary>
        public UInt16 ReplyMsgNum { get; set; }

        /// <summary>
        /// 结果
        /// 0：成功；1：失败；2：通道不支持。以下字段在结果=0 时才有效。
        /// </summary>
        public Byte Result { get; set; }

        /// <summary>
        /// 多媒体ID个数
        /// 拍摄成功的多媒体个数
        /// </summary>
        public UInt16 MultimediaIdCount { get; set; }

        /// <summary>
        /// 多媒体ID列表
        /// </summary>
        public List<UInt32> MultimediaIds { get; set; }

        /// <summary>从数据流中读取消息体</summary>
        /// <param name="stream">数据流</param>
        /// <param name="context">上下文（JTMessage 对象）</param>
        /// <returns>是否读取成功</returns>
        public Boolean Read(Stream stream, Object? context)
        {
            var reader = Helper.CreateReader(stream);

            ReplyMsgNum = reader.ReadUInt16();
            Result = reader.ReadByte();

            if (Result == 0)
            {
                MultimediaIdCount = reader.ReadUInt16();
                MultimediaIds = new List<UInt32>();
                for (var i = 0; i < MultimediaIdCount; i++)
                {
                    var id = reader.ReadUInt32();
                    MultimediaIds.Add(id);
                }
            }

            return true;
        }

        /// <summary>将消息体写入数据流</summary>
        /// <param name="stream">数据流</param>
        /// <param name="context">上下文（JTMessage 对象）</param>
        /// <returns>是否写入成功</returns>
        public Boolean Write(Stream stream, Object? context)
        {
            var writer = Helper.CreateWriter(stream);

            writer.WriteUInt16(ReplyMsgNum);
            writer.WriteByte(Result);

            if (Result == 0)
            {
                writer.WriteUInt16((UInt16)MultimediaIds.Count);
                foreach (var item in MultimediaIds)
                {
                    writer.WriteUInt32(item);
                }
            }

            return true;
        }
    }
}