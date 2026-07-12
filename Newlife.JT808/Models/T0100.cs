using NewLife.JT808.Protocols;
using NewLife.JT808.UtilTool;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    /// <summary>终端注册</summary>
    [MessageKind(MessageKinds.终端注册)]
    public class T0100 : IAccessor
    {
        /// <summary>
        /// 省域 ID
        /// 标示终端安装车辆所在的省域，0 保留，由平台取默
        /// 认值。省域 ID 采用 GB/T 2260 中规定的行政区划代
        /// 码六位中前两位
        /// </summary>
        public UInt16 Province { get; set; }

        /// <summary>
        /// 市县域 ID
        /// 标示终端安装车辆所在的市域和县域，0 保留，由平
        /// 台取默认值。市县域 ID 采用 GB/T 2260 中规定的行
        /// 政区划代码六位中后四位。
        /// </summary>
        public UInt16 City { get; set; }

        /// <summary>
        /// 制造商 ID
        /// 2013版本 5 个字节，终端制造商编码
        /// 2019版本 11 个字节，终端制造商编码
        /// </summary>
        public String Provider { get; set; }

        /// <summary>
        /// 终端型号
        /// 2011版本   8个字节  ，此终端型号由制造商自行定义，位数不足时，后补“0X00”
        /// 2013版本   20 个字节，此终端型号由制造商自行定义，位数不足时，后补“0X00”。
        /// 2019版本   30 个字节，此终端型号由制造商自行定义，位数不足时，后补“0X00”。
        /// </summary>
        public String Model { get; set; }

        /// <summary>
        /// 终端 ID
        /// 2013版本  7个字节，由大写字母和数字组成，此终端 ID 由制造商自行定义，位数不足时，后补“0X00”。
        /// 2019版本  30个字节，由大写字母和数字组成，此终端 ID 由制造商自行定义，位数不足时，后补“0X00”。
        /// </summary>
        public String DeviceId { get; set; }

        /// <summary>
        /// 车牌颜色
        /// 车牌颜色，按照 JT/T415-2006 的 5.4.12。
        /// 未上牌时，取值为 0。
        /// </summary>
        public CarColors Color { get; set; }

        /// <summary>
        /// 车辆标识
        /// 车牌颜色为 0 时，表示车辆 VIN；
        /// 否则，表示公安交通管理部门颁发的机动车号牌。
        /// </summary>
        public String PlateNo { get; set; }

        /// <summary>读取</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Read(Stream stream, Object context)
        {
            var header = context as JTMessage;
            var reader = Helper.CreateReader(stream, header);

            Province = reader.Read<UInt16>();
            City = reader.Read<UInt16>();

            if (header.Version >= 1)
            {
                Provider = reader.ReadFixedString(11);
                Model = reader.ReadFixedString(30);
                DeviceId = reader.ReadFixedString(30);
            }
            else
            {
                var len = stream.Length - stream.Position > 33 ? 20 : 8;
                Provider = reader.ReadFixedString(5);
                Model = reader.ReadFixedString(len);
                DeviceId = reader.ReadFixedString(7);
            }

            Color = (CarColors)reader.ReadByte();
            PlateNo = reader.ReadAllGBK();

            return true;
        }

        /// <summary>写入</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Write(Stream stream, Object context)
        {
            var header = context as JTMessage;
            var writer = Helper.CreateWriter(stream, header);

            writer.Write(Province);
            writer.Write(City);

            if (header.Version >= 1)
            {
                writer.WriteFixedString(Provider, 11);
                writer.WriteFixedString(Model, 30);
                writer.WriteFixedString(DeviceId, 30);
            }
            else
            {
                writer.WriteFixedString(Provider, 5);
                writer.WriteFixedString(Model, 20);
                writer.WriteFixedString(DeviceId, 7);
            }

            writer.Write((Byte)Color);
            writer.WriteAllGBK(PlateNo);

            return true;
        }
    }
}