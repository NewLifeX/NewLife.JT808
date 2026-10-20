using NewLife;

namespace NewLife.JT808.Protocols
{
    /// <summary>类型、长度、数值结构</summary>
    public class TLV
    {
        #region 属性
        /// <summary>类型</summary>
        public uint Type { get; set; }

        /// <summary>数值</summary>
        public byte[] Value { get; set; }
        #endregion

        #region 方法
        /// <summary>从数据流中解析TLV值</summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static IList<TLV> Read(Stream stream)
        {
            var list = new List<TLV>();
            var buf = new byte[4];
            while (true)
            {
                // 流末尾不足一个完整类型字段时结束，避免越过末尾抛异常
                var count = 0;
                while (count < buf.Length)
                {
                    var n = stream.Read(buf, count, buf.Length - count);
                    if (n <= 0) break;
                    count += n;
                }
                if (count < buf.Length) break;

                var kind = buf.ToUInt32(0, false);
                if (kind <= 0) break;

                var len = stream.ReadByte();
                if (len <= 0) break;

                var data = new byte[len];
                var read = 0;
                while (read < len)
                {
                    var n = stream.Read(data, read, len - read);
                    if (n <= 0) break;
                    read += n;
                }
                if (read > 0)
                {
                    if (read < len) Array.Resize(ref data, read);
                    list.Add(new TLV { Type = kind, Value = data });
                }
            }

            return list;
        }

        public static IList<TLV> Read(byte[] data) => Read(new MemoryStream(data));

        /// <summary>列表式TLV转为字典</summary>
        /// <param name="list"></param>
        /// <param name="overide">遇到重复数据时是否覆盖</param>
        /// <returns></returns>
        public static IDictionary<TKind, object> Convert<TKind>(IList<TLV> list, bool overide = true) where TKind : struct
        {
            var dic = new Dictionary<TKind, object>();
            foreach (var item in list)
            {
                var value = item.Value;
                var data = (object)item.Value;
                if (value != null)
                {
                    data = value.Length switch
                    {
                        1 => value[0],
                        2 => value.ToUInt16(0, false),
                        4 => value.ToUInt32(0, false),
                        _ => value,
                    };
                }

                var kind = (TKind)(object)item.Type;
                if (overide || !dic.ContainsKey(kind)) dic[kind] = data;
            }

            return dic;
        }

        public static void Write(Stream stream, IList<TLV> list)
        {
            foreach (var item in list)
            {
                stream.Write(item.Type.GetBytes(false));

                var len = item.Value?.Length ?? 0;
                stream.WriteByte((Byte)len);

                if (len > 0) stream.Write(item.Value);
            }
        }

        /// <summary>将字典转换为 TLV 强类型列表</summary>
        /// <typeparam name="TKind">键类型（枚举类型）</typeparam>
        /// <param name="dic">原始字典（参数 ID→值的映射）</param>
        /// <returns>TLV 列表</returns>
        public static IList<TLV> Convert<TKind>(IDictionary<TKind, object> dic) where TKind : struct
        {
            var list = new List<TLV>();
            foreach (var item in dic)
            {
                var klv = new TLV { Type = (uint)(object)item.Key };
                list.Add(klv);

                if (item.Value != null)
                {
                    klv.Value = item.Value switch
                    {
                        byte[] dt => dt,
                        byte b => new[] { b },
                        ushort m => m.GetBytes(false),
                        short m => m.GetBytes(false),
                        uint n => n.GetBytes(false),
                        int n => n.GetBytes(false),
                        _ => throw new Exception(L.T("Unsupported type [{0}].", item.Value.GetType())),
                    };
                }
            }

            return list;
        }
        #endregion
    }
}