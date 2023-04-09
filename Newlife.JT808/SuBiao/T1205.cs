using NewLife.JT808.Protocols;
using NewLife.JT808.UtilTool;
using NewLife.Serialization;

namespace NewLife.JT808.SuBiao
{
    /// <summary>终端上传音视频资源列表</summary>
    [MessageKind(MessageKinds.音视频资源列表)]
    public class T1205 : IAccessor
    {
        /// <summary>流水号</summary>
        public Int16 SerialNumber { get; set; }

        /// <summary>资源列表</summary>
        public List<ResourceInfo> Items { get; set; }

        /// <summary>读取</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Read(Stream stream, Object context)
        {
            var reader = Helper.CreateReader(stream);

            SerialNumber = reader.ReadInt16();
            var count = reader.ReadInt32();

            var list = new List<ResourceInfo>();
            for (var i = 0; i < count; i++)
            {
                var item = reader.Read<ResourceInfo>();

                list.Add(item);
            }
            Items = list;

            return true;
        }

        /// <summary>写入</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Write(Stream stream, Object context)
        {
            var writer = Helper.CreateWriter(stream);

            writer.Write(SerialNumber);

            var count = Items?.Count ?? 0;
            writer.Write(count);

            for (var i = 0; i < count; i++)
            {
                writer.Write(Items[i]);
            }

            return true;
        }
    }

    /// <summary>音视频类型</summary>
    public enum T1205_Kinds : Byte
    {
        /// <summary>音视频</summary>
        音视频 = 0,

        /// <summary>音频</summary>
        音频 = 1,

        /// <summary>视频</summary>
        视频 = 2,
    }

    /// <summary>存储器类型</summary>
    public enum T1205_StorageKinds : Byte
    {
        /// <summary>主存储器</summary>
        主存储器 = 1,

        /// <summary>灾备存储器</summary>
        灾备存储器 = 2,
    }

    /// <summary>音视频资源</summary>
    public class ResourceInfo
    {
        /// <summary>逻辑通道号</summary>
        public Byte Channel { get; set; }

        /// <summary>开始时间</summary>
        public DateTime StartTime { get; set; }

        /// <summary>结束时间</summary>
        public DateTime EndTime { get; set; }

        /// <summary>报警标识</summary>
        public UInt64 Flag { get; set; }

        /// <summary>类型。0音视频，1音频，2视频</summary>
        public T1205_Kinds Kind { get; set; }

        /// <summary>码流。1主码流，2子码流</summary>
        public BitStreams BitStream { get; set; }

        /// <summary>类型。0音视频，1音频，2视频</summary>
        public T1205_StorageKinds StorageKind { get; set; }

        /// <summary>文件大小。单位字节</summary>
        public Int32 FileSize { get; set; }
    }
}