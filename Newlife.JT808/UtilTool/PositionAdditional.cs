using Newlife.JT808.Protocols;
using NewLife;
using NewLife.Serialization;

namespace Newlife.JT808.UtilTool;

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
    public static void WriteAdditional(this Binary writer, IDictionary<AdditionalKinds, object> dic)
    {
        if (dic == null || dic.Count == 0) return;

        var klvs = KLV.Convert(dic);
        KLV.Write(writer.Stream, klvs);

        //foreach (var item in dic)
        //{
        //    writer.Write((Byte)item.Key);
        //    if (item.Value == null)
        //        writer.Write(0);
        //    else if (item.Value is Byte[] dt)
        //    {
        //        writer.Write((Byte)dt.Length);
        //        writer.Write(dt, 0, dt.Length);
        //    }
        //    else if (item.Value is Byte b)
        //    {
        //        writer.Write(1);
        //        writer.Write(new[] { b }, 0, 1);
        //    }
        //    else if (item.Value is UInt16 m)
        //    {
        //        writer.Write(2);
        //        writer.Write(m.GetBytes(false), 0, 2);
        //    }
        //    else if (item.Value is UInt32 n)
        //    {
        //        writer.Write(4);
        //        writer.Write(n.GetBytes(false), 0, 4);
        //    }
        //    else
        //    {
        //        //var data = item.Key switch
        //        //{
        //        //    AdditionalKinds.里程 => 4,
        //        //    AdditionalKinds.油量 => 2,
        //        //    AdditionalKinds.速度 => 2,
        //        //    AdditionalKinds.报警事件 => 2,
        //        //    AdditionalKinds.胎压 => 30,
        //        //    AdditionalKinds.车厢温度 => 2,
        //        //    AdditionalKinds.扩展车辆信号 => 4,
        //        //    AdditionalKinds.IO状态 => 2,
        //        //    AdditionalKinds.模拟量 => 4,
        //        //    AdditionalKinds.信号强度 => 1,
        //        //    AdditionalKinds.卫星数 => 2,
        //        //    _ => -1,
        //        //};
        //        //writer.Write((Byte)item.Value.Length);
        //        //writer.Write(item.Value, 0, item.Value.Length);
        //        throw new Exception($"不支持的类型[{item.Value.GetType()}]");
        //    }
        //}
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       