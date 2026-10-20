using System.Diagnostics;
using NewLife;

namespace NewLife.JT808.Protocols
{
    /// <summary>类型、长度、数值结构</summary>
    [DebuggerDisplay("{Kind}")]
    public class KLV
    {
        #region 属性
        /// <summary>类型</summary>
        public Byte Kind { get; set; }

        /// <summary>数值</summary>
        public Byte[] Value { get; set; }
        #endregion

        #region 方法
        /// <summary>从数据流中解析KLV值</summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static IList<KLV> Read(Stream stream)
        {
            var list = new List<KLV>();
            while (true)
            {
                var kind = stream.ReadByte();
                if (kind <= 0) break;

                var len = stream.ReadByte();
                if (len <= 0) break;

                var data = new Byte[len];
                var count = stream.Read(data, 0, len);
                if (count > 0)
                {
                    list.Add(new KLV { Kind = (Byte)kind, Value = data });
                }
            }

            return list;
        }

        public static IList<KLV> Read(Byte[] data) => Read(new MemoryStream(data));

        /// <summary>列表式KLV转为字典</summary>
        /// <param name="list"></param>
        /// <param name="overide">遇到重复数据时是否覆盖</param>
        /// <returns></returns>
        public static IDictionary<TKind, Object> Convert<TKind>(IList<KLV> list, Boolean overide = true) where TKind : struct
        {
            var dic = new Dictionary<TKind, Object>();
            foreach (var item in list)
            {
                var value = item.Value;
                var data = (Object)item.Value;
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

                var kind = (TKind)(Object)item.Kind;
                if (overide || !dic.ContainsKey(kind)) dic[kind] = data;
            }

            return dic;
        }

        public static void Write(Stream stream, IList<KLV> list)
        {
            foreach (var item in list)
            {
                stream.WriteByte(item.Kind);

                var len = item.Value?.Length ?? 0;
                stream.WriteByte((Byte)len);

                if (len > 0) stream.Write(item.Value);
            }
        }

        /// <summary>将字典转换为 KLV 强类型列表</summary>
        /// <typeparam name="TKind">键类型（枚举类型）</typeparam>
        /// <param name="dic">原始字典（附加信息 ID→值的映射）</param>
        /// <returns>KLV 列表</returns>
        public static IList<KLV> Convert<TKind>(IDictionary<TKind, Object> dic) where TKind : struct
        {
            var list = new List<KLV>();
            foreach (var item in dic)
            {
                var klv = new KLV { Kind = (Byte)(Object)item.Key };
                list.Add(klv);

                if (item.Value != null)
                {
                    klv.Value = item.Value switch
                    {
                        Byte[] dt => dt,
                        Byte b => new[] { b },
                        UInt16 m => m.GetBytes(false),
                        Int16 m => m.GetBytes(false),
                        UInt32 n => n.GetBytes(false),
                        Int32 n => n.GetBytes(false),
                        _ => throw new Exception(L.T("Unsupported type [{0}].", item.Value.GetType())),
                    };
                }
            }

            return list;
        }
        #endregion
    }
}