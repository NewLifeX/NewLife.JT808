using NewLife.JT808.Protocols;
using NewLife.JT808.Protocols;
using NewLife;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    [Flags]
    public enum T0107_Types : UInt16
    {
        客运车辆 = 1 << 0,
        危险品车辆 = 1 << 1,
        普通货运车辆 = 1 << 2,
        出租车辆 = 1 << 3,
        硬盘录像 = 1 << 6,
        分体机 = 1 << 7,
        挂车 = 1 << 8,
    }

    [Flags]
    public enum T0107_GNSSes : Byte
    {
        GPS = 1 << 0,
        北斗 = 1 << 1,
        GLONASS = 1 << 2,
        Galileo = 1 << 3,
    }

    [Flags]
    public enum T0107_Communications : Byte
    {
        GPRS = 1 << 0,
        CDMA = 1 << 1,
        TD_SCDMA = 1 << 2,
        WCDMA = 1 << 3,
        CDMA2000 = 1 << 4,
        TD_LTE = 1 << 5,
        Other = 1 << 6,
        Other2 = 1 << 7,
    }

    /// <summary>查询终端属性应答</summary>
    [MessageKind(MessageKinds.查询终端属性应答)]
    public class T0107 : IAccessor
    {
        /// <summary>
        /// 终端类型
        /// bit0，0：不适用客运车辆，1：适用客运车辆；
        /// bit1，0：不适用危险品车辆，1：适用危险品车辆；
        /// bit2，0：不适用普通货运车辆，1：适用普通货运车辆；
        /// bit3，0：不适用出租车辆，1：适用出租车辆；
        /// bit6，0：不支持硬盘录像，1：支持硬盘录像；
        /// bit7，0：一体机，1：分体机
        /// bit8, 0: 不适用挂车, 2:适用挂车
        /// </summary>
        public T0107_Types Type { get; set; }

        /// <summary>
        /// 制造商 ID
        /// 2013版本 5 个字节，终端制造商编码
        /// 2019版本 11 个字节，终端制造商编码
        /// </summary>
        public String MakerId { get; set; }

        /// <summary>
        /// 终端型号
        /// BYTE[20] 20 个字节，此终端型号由制造商自行定义，位数不足时，后补“0X00”。
        /// 2019版本
        /// BYTE[30] 30 个字节，此终端型号由制造商自行定义，位数不足时，后补“0X00”。
        /// </summary>
        public String Model { get; set; }

        /// <summary>
        /// 终端ID 
        /// BYTE[7]  7 个字节，由大写字母和数字组成，此终端 ID 由制造商自行定义，位数不足时，后补“0X00”
        /// 2019版本
        /// BYTE[30]  30 个字节，由大写字母和数字组成，此终端 ID 由制造商自行定义，位数不足时，后补“0X00”
        /// </summary>
        public String TerminalId { get; set; }

        /// <summary>
        /// 终端 SIM 卡 ICCID
        /// BCD[10]
        /// </summary>
        public String SIM_ICCID { get; set; }

        /// <summary>
        /// 终端硬件版本号
        /// </summary>
        public String HardwareVersion { get; set; }

        /// <summary>
        /// 终端固件版本号
        /// </summary>
        public String FirmwareVersion { get; set; }

        /// <summary>
        /// GNSS 模块属性
        /// bit0，0：不支持 GPS 定位， 1：支持 GPS 定位；
        /// bit1，0：不支持北斗定位， 1：支持北斗定位；
        /// bit2，0：不支持 GLONASS 定位， 1：支持 GLONASS 定位；
        /// bit3，0：不支持 Galileo 定位， 1：支持 Galileo 定位
        /// </summary>
        public T0107_GNSSes GNSSModule { get; set; }

        /// <summary>
        /// 通信模块属性
        /// bit0，0：不支持GPRS通信， 1：支持GPRS通信；
        /// bit1，0：不支持CDMA通信， 1：支持CDMA通信；
        /// bit2，0：不支持TD-SCDMA通信， 1：支持TD-SCDMA通信；
        /// bit3，0：不支持WCDMA通信， 1：支持WCDMA通信；
        /// bit4，0：不支持CDMA2000通信， 1：支持CDMA2000通信。
        /// bit5，0：不支持TD-LTE通信， 1：支持TD-LTE通信；
        /// bit7，0：不支持其他通信方式， 1：支持其他通信方式
        /// </summary>
        public T0107_Communications CommunicationModule { get; set; }

        /// <summary>读取</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Read(Stream stream, Object context)
        {
            var header = context as JTMessage;
            var reader = Helper.CreateReader(stream, header);
            var len = stream.Length - stream.Position;

            Type = (T0107_Types)reader.ReadUInt16();

            if (header != null && header.Version > 0)
            {
                // 资料写5字节，代码写11字节，实际上两种都存在
                if (len >= 137)
                {
                    MakerId = reader.ReadFixedString(11);
                    Model = reader.ReadFixedString(30);
                    TerminalId = reader.ReadFixedString(30);
                }
                else
                //!! 中天安驰的P9遇到问题
                {
                    MakerId = reader.ReadFixedString(5);
                    Model = reader.ReadFixedString(30);
                    TerminalId = reader.ReadFixedString(30);
                }
            }
            else
            {
                MakerId = reader.ReadFixedString(5);
                Model = reader.ReadFixedString(20);
                TerminalId = reader.ReadFixedString(7);
            }

            //!! 中天安驰的P9遇到问题
            if (!Model.IsNullOrEmpty())
            {
                var p = Model.IndexOf('\0');
                if (p > 0) Model = Model[..p];
            }

            SIM_ICCID = reader.ReadBCD(10);
            HardwareVersion = reader.ReadString();
            FirmwareVersion = reader.ReadString();
            GNSSModule = (T0107_GNSSes)reader.ReadByte();
            CommunicationModule = (T0107_Communications)reader.ReadByte();

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

            writer.WriteUInt16((UInt16)Type);
            if (header != null && header.Version > 0)
            {
                writer.WriteFixedString(MakerId, 11);
                writer.WriteFixedString(Model, 30);
                writer.WriteFixedString(TerminalId, 30);
            }
            else
            {
                writer.WriteFixedString(MakerId, 5);
                writer.WriteFixedString(Model, 20);
                writer.WriteFixedString(TerminalId, 7);
            }

            writer.WriteBCD(SIM_ICCID, 10);
            writer.WriteString(HardwareVersion);
            writer.WriteString(FirmwareVersion);
            writer.WriteByte((Byte)GNSSModule);
            writer.WriteByte((Byte)CommunicationModule);

            return true;
        }
    }
}