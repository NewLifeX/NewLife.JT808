using System.Globalization;
using NewLife.JT808.Protocols;
using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    /// <summary>插拔状态</summary>
    public enum CardStatus : Byte
    {
        /// <summary>从业资格证IC卡插入_驾驶员上班</summary>
        插入 = 0x01,

        /// <summary>从业资格证IC卡拔出_驾驶员下班</summary>
        拔出 = 0x02
    }

    /// <summary>IC 卡读取结果</summary>
    public enum CardResult : Byte
    {
        /// <summary>IC卡读卡成功</summary>
        读卡成功 = 0x00,

        /// <summary>读卡失败_原因为卡片密钥认证未通过</summary>
        卡片密钥认证未通过 = 0x01,

        /// <summary>读卡失败_原因为卡片已被锁定</summary>
        卡片已被锁定 = 0x02,

        /// <summary>读卡失败_原因为卡片被拔出</summary>
        卡片被拔出 = 0x03,

        /// <summary>读卡失败_原因为数据校验错误</summary>
        数据校验错误 = 0x04,
    }

    /// <summary>驾驶员身份信息采集上报</summary>
    [MessageKind(MessageKinds.驾驶员上报)]
    public class T0702 : IAccessor
    {
        /// <summary>
        /// 状态
        /// 0x01：从业资格证 IC 卡插入（驾驶员上班）；
        /// 0x02：从业资格证 IC 卡拔出（驾驶员下班）。
        /// </summary>
        public CardStatus Status { get; set; }

        /// <summary>
        /// 插卡/拔卡时间，YY-MM-DD-hh-mm-ss；
        /// 以下字段在状态为 0x01 时才有效并做填充。
        /// BCD[6]
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// IC 卡读取结果
        /// 0x00：IC 卡读卡成功；
        /// 0x01：读卡失败，原因为卡片密钥认证未通过；
        /// 0x02：读卡失败，原因为卡片已被锁定；
        /// 0x03：读卡失败，原因为卡片被拔出；
        /// 0x04：读卡失败，原因为数据校验错误。
        /// 以下字段在 IC 卡读取结果等于 0x00 时才有效。
        /// </summary>
        public CardResult Result { get; set; }

        /// <summary>驾驶员姓名</summary>
        public String UserName { get; set; }

        /// <summary>
        /// 从业资格证编码 
        /// 2011 长度40 位 ，不足补 '\0'；
        /// 2013 长度 20 位，不足补 '\0'。
        /// </summary>
        public String QualificationCode { get; set; }

        /// <summary>发证机构名称</summary>
        public String LicenseIssuing { get; set; }

        /// <summary>证件有效期 BCD[4]</summary>
        public String CertificateExpire { get; set; }

        /// <summary>
        /// 驾驶员身份证号 长度20 不足补 '\0'
        /// 2011版本
        /// 2019版本
        /// </summary>
        public String IdentityCard { get; set; }

        /// <summary>
        /// 人脸匹配度
        /// 身份证或从业资格证照片与人脸匹配度比例：0~100
        /// 2019版本
        /// </summary>
        public Byte? FaceMatch { get; set; }

        /// <summary>
        /// 身份证 UID
        /// 长度 20 位，不足补0x00
        /// 2019版本
        /// </summary>
        public String UID { get; set; }

        /// <summary>解析消息</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Read(Stream stream, Object? context)
        {
            var reader = Helper.CreateReader(stream);
            var header = context as JTMessage;

            // 因2011第一个字节代表姓名长度 所以该值长度只能为  2，3，4，整个数据长度 62+m+n
            var firstByte = reader.ReadByte();
            if (firstByte == 0x01)
            {
                Status = (CardStatus)firstByte;

                // 过滤非标BCD数据
                var str = reader.ReadBCD(6);
                if (DateTime.TryParseExact(str, "yyMMddHHmmss", null, DateTimeStyles.None, out var dt))
                    Time = dt;
                else if (DateTime.TryParseExact(str[2..], "MMddHHmmss", null, DateTimeStyles.None, out dt))
                    Time = dt;

                if (Status == CardStatus.插入)
                {
                    Result = (CardResult)reader.ReadByte();
                    if (Result == CardResult.读卡成功)
                    {
                        UserName = reader.ReadString();
                        QualificationCode = reader.ReadString(20);
                        LicenseIssuing = reader.ReadString();
                        //UserName = reader.Read<String>();
                        //QualificationCode = reader.ReadFixedString(20);
                        //LicenseIssuing = reader.Read<String>();
                        CertificateExpire = reader.ReadBCD(4);

                        if (header.Version >= 1)
                        {
                            IdentityCard = reader.ReadString(20);
                            //IdentityCard = reader.ReadFixedString(20);

                            // 兼容808-2019 补充
                            if (stream.Position < stream.Length)
                            {
                                FaceMatch = reader.ReadByte();
                                UID = reader.ReadFixedString(20);
                            }
                        }
                    }
                }
            }
            else
            {
                if (firstByte == 0x02 && stream.Position + 6 == stream.Length)
                {
                    // 如果字节是0x02且长度只有7，那么该协议就是2013或者2019
                    Status = (CardStatus)firstByte;
                    var str = reader.ReadBCD(6);
                    if (DateTime.TryParseExact(str, "yyMMddHHmmss", null, DateTimeStyles.None, out var dt))
                        Time = dt;
                    //Time = reader.ReadDateTime();
                }
                else
                {
                    var len = firstByte;
                    UserName = reader.ReadFixedString(len);
                    IdentityCard = reader.ReadFixedString(20);
                    QualificationCode = reader.ReadFixedString(40);
                    LicenseIssuing = reader.ReadString();
                    //LicenseIssuing = reader.Read<String>();
                }
            }

            return true;
        }

        /// <summary>写入消息</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Write(Stream stream, Object? context)
        {
            var writer = Helper.CreateWriter(stream);
            var header = context as JTMessage;

            if (header.Version == 0)
            {
                writer.Write(UserName);
                writer.WriteFixedString(IdentityCard, 20);
                writer.WriteFixedString(QualificationCode, 40);
                writer.Write(LicenseIssuing);
            }
            else
            {
                writer.WriteByte((Byte)Status);
                //writer.WriteDateTime(Time);
                writer.WriteBCD(Time.ToString("yyMMddHHmmss"), 6);
                if (Status == CardStatus.插入)
                {
                    writer.WriteByte((Byte)Result);
                    if (Result == CardResult.读卡成功)
                    {
                        writer.Write(UserName);
                        writer.WriteFixedString(QualificationCode, 20);
                        writer.Write(LicenseIssuing);
                        writer.WriteBCD(CertificateExpire, 4);

                        if (header.Version == 1)
                        {
                            writer.WriteFixedString(IdentityCard, 20);

                            //兼容808-2019 补充
                            if (FaceMatch.HasValue) writer.WriteByte(FaceMatch.Value);
                            if (!String.IsNullOrEmpty(UID)) writer.WriteFixedString(UID, 20);
                        }
                    }
                }
            }

            return true;
        }
    }
}