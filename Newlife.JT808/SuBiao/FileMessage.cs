using NewLife;
using NewLife.Data;
using NewLife.Log;
using NewLife.Serialization;

namespace NewLife.JT808.SuBiao
{
    /// <summary>文件信息</summary>
    public class FileMessage : IAccessor
    {
        #region 属性
        /// <summary>文件名称</summary>
        [FieldSize(50)]
        public String FileName { get; set; }

        /// <summary>偏移量</summary>
        public Int32 Offset { get; set; }

        /// <summary>负载数据长度</summary>
        public Int32 Length { get; set; }

        /// <summary>负载数据。默认长度64k，小于64k则为实际长度</summary>
        public Byte[] Payload { get; set; }
        #endregion

        #region 读写
        /// <summary>解析数据流</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Read(Stream stream, Object context)
        {
            if (stream == null && context == null) return false;

            // 头部标识探测
            var magic = stream.ReadBytes(4).ToStr();
            if (magic != "00cd") return false;

            var reader = new Binary { Stream = stream, IsLittleEndian = false };

            FileName = reader.ReadFixedString(50);
            Offset = reader.ReadInt32();
            Length = reader.ReadInt32();

            var len = reader.ReadInt32();
            if (len > 0) Payload = reader.ReadBytes(len);

            return true;
        }

        /// <summary>写入数据</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Boolean Write(Stream stream, Object context)
        {
            if (stream == null && context == null) return false;

            var writer = new Binary { Stream = stream, IsLittleEndian = false };

            writer.Write("00cd".GetBytes());

            writer.WriteFixedString(FileName, 50);
            writer.Write(Offset);
            writer.Write(Length);

            var len = Payload?.Length ?? 0;
            writer.Write(len);
            if (len > 0) writer.Write(Payload);

            return true;
        }

        /// <summary>读取数据包</summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public Boolean Read(IPacket pk) => Read(pk.GetStream(), null);

        /// <summary>转数据包</summary>
        /// <returns></returns>
        public IPacket ToPacket()
        {
            try
            {
                var ms = new MemoryStream();
                if (!Write(ms, null)) return null;

                ms.Position = 0;
                return new ArrayPacket(ms);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                throw;
            }
        }
        #endregion
    }
}