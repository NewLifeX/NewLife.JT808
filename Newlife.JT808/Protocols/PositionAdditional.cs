using NewLife;
using NewLife.Serialization;

namespace NewLife.JT808.Protocols;

/// <summary>位置报文附加信息</summary>
public static class PositionAdditional
{
    /// <summary>读取附加数据</summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public static IDictionary<AdditionalKinds, object> ReadAdditional(this Binary reader)
    {
        var stream = reader.Stream;
        var klvs = KLV.Read(stream);
        var dic = KLV.Convert<AdditionalKinds>(klvs, false);

        foreach (var item in dic)
        {
            var kind = item.Key;
            switch (kind)
            {
                case AdditionalKinds.车辆信号:
                    // 高位暂时用不上
                    dic[kind] = (SignalStatus)((uint)item.Value & 0xFF);
                    break;
                case (AdditionalKinds)238:
                    if (item.Value is byte[] data && data.Length >= 4)
                    {
                        var klvs2 = KLV.Read(data.ReadBytes(4, -1));

                        dic[kind] = new
                        {
                            name = data.ToStr(null, 0, 4),
                            data = KLV.Convert<byte>(klvs2)
                        };
                    }
                    break;
            }
        }

        return dic;
    }

    /// <summary>写入附加数据</summary>
    /// <param name="writer"></param>
    /// <param name="dic"></param>
    public static void WriteAdditional(this Binary writer, IDictionary<AdditionalKinds, object>? dic)
    {
        if (dic == null || dic.Count == 0) return;

        var klvs = KLV.Convert(dic);
        KLV.Write(writer.Stream, klvs);
    }
}