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
            while (true)
            {
                var kind = stream.ReadBytes(4).ToUInt32(0, false);
                if (kind <= 0) break;

                var len = stream.ReadByte();
                if (len <= 0) break;

                var data = new byte[len];
                var count = stream.Read(data, 0, len);
                if (count > 0)
                {
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
                        _ => throw new Exception($"不支持的类型[{item.Value.GetType()}]"),
                    };
                }
            }

            return list;
        }
        #endregion
    }
}