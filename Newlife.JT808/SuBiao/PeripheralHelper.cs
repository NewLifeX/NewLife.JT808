using NewLife.JT808.Protocols;
using NewLife;
using NewLife.Serialization;

namespace NewLife.JT808.SuBiao
{
    /// <summary>外设助手</summary>
    public static class PeripheralHelper
    {
        /// <summary>解析外设状态</summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IDictionary<PeripheralKinds, PeripheralStatus> ReadStatus(Byte[] data)
        {
            if (data == null || data.Length == 0 || data[0] == 0) return null;

            var dic = new Dictionary<PeripheralKinds, PeripheralStatus>();

            var reader = Helper.CreateReader(new MemoryStream(data));
            var count = reader.ReadByte();

            for (var i = 0; i < count; i++)
            {
                var kind = (PeripheralKinds)reader.ReadByte();
                var len = reader.ReadByte();
                //var ps = reader.Read<PeripheralStatus>();
                var dt = reader.ReadBytes(len);
                if (dt == null || dt.Length == 0) continue;

                // 状态1字节 + 报警4字节；长度不足时按缺失字段为零处理，避免畸形数据抛异常
                var ps = new PeripheralStatus
                {
                    Status = (PeripheralWorkStatus)dt[0],
                    Alarm = dt.Length >= 5 ? (PeripheralAlarmStatus)dt.ToUInt32(1, false) : 0
                };
                //var ps = Binary.FastRead<PeripheralStatus>(new MemoryStream(dt));
                dic[kind] = ps;
            }

            return dic;
        }

        /// <summary>解析外设信息</summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IDictionary<PeripheralKinds, PeripheralInfo> ReadInfo(Byte[] data)
        {
            if (data == null || data.Length == 0 || data[0] == 0) return null;

            var dic = new Dictionary<PeripheralKinds, PeripheralInfo>();

            var reader = Helper.CreateReader(new MemoryStream(data));
            var count = reader.ReadByte();

            for (var i = 0; i < count; i++)
            {
                var kind = (PeripheralKinds)reader.ReadByte();
                var len = reader.ReadByte();
                //var ps = reader.Read<PeripheralInfo>();
                var dt = reader.ReadBytes(len);
                var ps = Binary.FastRead<PeripheralInfo>(new MemoryStream(dt));
                dic[kind] = ps;
            }

            return dic;
        }
    }
}