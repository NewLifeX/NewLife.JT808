using NewLife.JT808.Protocols;
using NewLife.Serialization;
using System.Text;

namespace NewLife.JT808.UtilTool
{
    /// <summary>序列化助手</summary>
    public static class Helper
    {
        /// <summary>字符串编码</summary>
        public static Encoding Encoding { get; }

        static Helper()
        {
#if !NETFRAMEWORK
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            Encoding = Encoding.GetEncoding("GBK");
        }

        /// <summary>创建读取器</summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Binary CreateReader(Stream stream) => new()
        {
            Stream = stream,
            Encoding = Encoding,
            IsLittleEndian = false,
            UseFieldSize = true,
            TrimZero = true,
        };

        /// <summary>创建写入器</summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Binary CreateWriter(Stream stream) => new()
        {
            Stream = stream,
            Encoding = Encoding,
            IsLittleEndian = false,
            UseFieldSize = true,
            TrimZero = true,
        };

        /// <summary>创建读取器</summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Binary CreateReader(Stream stream, JTMessage? message) => new()
        {
            Stream = stream,
            Encoding = Encoding,
            IsLittleEndian = false,
            UseFieldSize = true,
            TrimZero = true,

            Version = message == null ? null : (message.Version > 0 ? "2019" : "2013"),
        };

        /// <summary>创建写入器</summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Binary CreateWriter(Stream stream, JTMessage? message) => new()
        {
            Stream = stream,
            Encoding = Encoding,
            IsLittleEndian = false,
            UseFieldSize = true,
            TrimZero = true,

            Version = message == null ? null : (message.Version > 0 ? "2019" : "2013"),
        };

        /// <summary>快速读取</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T FastRead<T>(Byte[] data)
        {
            var binary = new Binary
            {
                Stream = new MemoryStream(data),
                UseFieldSize = true,
                UseRef = false
            };
            return binary.Read<T>();
        }

        /// <summary>写入GBK字符串</summary>
        /// <param name="binary"></param>
        /// <param name="value"></param>
        public static void WriteString(this Binary binary, String value)
        {
            var buf = Encoding.GetBytes(value);
            binary.Write((Byte)buf.Length);
            binary.Write(buf, 0, buf.Length);
        }

        /// <summary>读取GBK字符串</summary>
        /// <param name="binary"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static String ReadString(this Binary binary, Int32 len = -1)
        {
            if (len < 0) len = binary.ReadByte();
            if (len == 0) return null;

            var buf = binary.ReadBytes(len);

            // 剔除头尾非法字符
            Int32 s, e;
            for (s = 0; s < len && (buf[s] == 0x00 || buf[s] == 0xFF); s++) ;
            for (e = len - 1; e >= 0 && (buf[e] == 0x00 || buf[e] == 0xFF); e--) ;

            if (s >= len || e < 0) return null;

            return Encoding.GetString(buf, s, e - s + 1);
        }

        /// <summary>按照GBK编码写入字符串</summary>
        /// <param name="binary"></param>
        /// <param name="value"></param>
        public static void WriteAllGBK(this Binary binary, String value)
        {
            var buf = Encoding.GetBytes(value);
            binary.Write(buf, 0, buf.Length);
        }

        /// <summary>按照GBK编码读取剩余字节作为字符串</summary>
        /// <param name="binary"></param>
        /// <returns></returns>
        public static String ReadAllGBK(this Binary binary)
        {
            var len = binary.Stream.Length - binary.Stream.Position;
            var buf = binary.ReadBytes((Int32)len);
            return Encoding.GetString(buf)?.Trim('\0');
        }

        /// <summary>Json序列化，使用枚举字符串</summary>
        /// <param name="value"></param>
        /// <param name="indented"></param>
        /// <returns></returns>
        public static String ToJsonJT808(this Object value, Boolean indented = false)
        {
            var jw = new JsonWriter
            {
                Indented = indented,
                EnumString = true,
                ByteArrayAsHex = true,
            };
            jw.Write(value);

            return jw.GetString();
        }
    }
}