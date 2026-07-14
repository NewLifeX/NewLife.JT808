using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using NewLife;
using NewLife.Data;
using XCode;
using XCode.Cache;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace JT808.Data;

/// <summary>位置数据。终端上报的位置信息</summary>
[Serializable]
[DataObject]
[Description("位置数据。终端上报的位置信息")]
[BindIndex("IX_PositionData_DeviceId_GPSTime", false, "DeviceId,GPSTime")]
[BindIndex("IX_PositionData_Mobile_GPSTime", false, "Mobile,GPSTime")]
[BindTable("PositionData", Description = "位置数据。终端上报的位置信息", ConnName = "GPS", DbType = DatabaseType.None)]
public partial class PositionData : IPositionData, IEntity<IPositionData>
{
    #region 属性
    private Int64 _Id;
    /// <summary>编号</summary>
    [DisplayName("编号")]
    [Description("编号")]
    [DataObjectField(true, false, false, 0)]
    [BindColumn("Id", "编号", "", DataScale = "time")]
    public Int64 Id { get => _Id; set { if (OnPropertyChanging("Id", value)) { _Id = value; OnPropertyChanged("Id"); } } }

    private Int32 _DeviceId;
    /// <summary>设备编号</summary>
    [DisplayName("设备编号")]
    [Description("设备编号")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("DeviceId", "设备编号", "")]
    public Int32 DeviceId { get => _DeviceId; set { if (OnPropertyChanging("DeviceId", value)) { _DeviceId = value; OnPropertyChanged("DeviceId"); } } }

    private String? _Mobile;
    /// <summary>手机号</summary>
    [DisplayName("手机号")]
    [Description("手机号")]
    [DataObjectField(false, false, true, 20)]
    [BindColumn("Mobile", "手机号", "")]
    public String? Mobile { get => _Mobile; set { if (OnPropertyChanging("Mobile", value)) { _Mobile = value; OnPropertyChanged("Mobile"); } } }

    private UInt32 _Alarm;
    /// <summary>报警标志</summary>
    [DisplayName("报警标志")]
    [Description("报警标志")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Alarm", "报警标志", "")]
    public UInt32 Alarm { get => _Alarm; set { if (OnPropertyChanging("Alarm", value)) { _Alarm = value; OnPropertyChanged("Alarm"); } } }

    private UInt32 _Status;
    /// <summary>状态</summary>
    [DisplayName("状态")]
    [Description("状态")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Status", "状态", "")]
    public UInt32 Status { get => _Status; set { if (OnPropertyChanging("Status", value)) { _Status = value; OnPropertyChanged("Status"); } } }

    private Int32 _Latitude;
    /// <summary>纬度</summary>
    [DisplayName("纬度")]
    [Description("纬度")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Latitude", "纬度", "")]
    public Int32 Latitude { get => _Latitude; set { if (OnPropertyChanging("Latitude", value)) { _Latitude = value; OnPropertyChanged("Latitude"); } } }

    private Int32 _Longitude;
    /// <summary>经度</summary>
    [DisplayName("经度")]
    [Description("经度")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Longitude", "经度", "")]
    public Int32 Longitude { get => _Longitude; set { if (OnPropertyChanging("Longitude", value)) { _Longitude = value; OnPropertyChanged("Longitude"); } } }

    private Int16 _Altitude;
    /// <summary>海拔高度</summary>
    [DisplayName("海拔高度")]
    [Description("海拔高度")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Altitude", "海拔高度", "")]
    public Int16 Altitude { get => _Altitude; set { if (OnPropertyChanging("Altitude", value)) { _Altitude = value; OnPropertyChanged("Altitude"); } } }

    private Int16 _Speed;
    /// <summary>速度(km/h)</summary>
    [DisplayName("速度(km_h)")]
    [Description("速度(km/h)")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Speed", "速度(km/h)", "")]
    public Int16 Speed { get => _Speed; set { if (OnPropertyChanging("Speed", value)) { _Speed = value; OnPropertyChanged("Speed"); } } }

    private Int16 _Direction;
    /// <summary>方向</summary>
    [DisplayName("方向")]
    [Description("方向")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Direction", "方向", "")]
    public Int16 Direction { get => _Direction; set { if (OnPropertyChanging("Direction", value)) { _Direction = value; OnPropertyChanged("Direction"); } } }

    private Double _Mileage;
    /// <summary>里程(km)</summary>
    [DisplayName("里程(km)")]
    [Description("里程(km)")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Mileage", "里程(km)", "")]
    public Double Mileage { get => _Mileage; set { if (OnPropertyChanging("Mileage", value)) { _Mileage = value; OnPropertyChanged("Mileage"); } } }

    private Double _Fuel;
    /// <summary>油量(L)</summary>
    [DisplayName("油量(L)")]
    [Description("油量(L)")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Fuel", "油量(L)", "")]
    public Double Fuel { get => _Fuel; set { if (OnPropertyChanging("Fuel", value)) { _Fuel = value; OnPropertyChanged("Fuel"); } } }

    private Double _LatitudeBD;
    /// <summary>百度纬度</summary>
    [DisplayName("百度纬度")]
    [Description("百度纬度")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("LatitudeBD", "百度纬度", "")]
    public Double LatitudeBD { get => _LatitudeBD; set { if (OnPropertyChanging("LatitudeBD", value)) { _LatitudeBD = value; OnPropertyChanged("LatitudeBD"); } } }

    private Double _LongitudeBD;
    /// <summary>百度经度</summary>
    [DisplayName("百度经度")]
    [Description("百度经度")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("LongitudeBD", "百度经度", "")]
    public Double LongitudeBD { get => _LongitudeBD; set { if (OnPropertyChanging("LongitudeBD", value)) { _LongitudeBD = value; OnPropertyChanged("LongitudeBD"); } } }

    private String? _Extras;
    /// <summary>附加信息JSON</summary>
    [DisplayName("附加信息JSON")]
    [Description("附加信息JSON")]
    [DataObjectField(false, false, true, -1)]
    [BindColumn("Extras", "附加信息JSON", "")]
    public String? Extras { get => _Extras; set { if (OnPropertyChanging("Extras", value)) { _Extras = value; OnPropertyChanged("Extras"); } } }

    private DateTime _GPSTime;
    /// <summary>GPS时间</summary>
    [DisplayName("GPS时间")]
    [Description("GPS时间")]
    [DataObjectField(false, false, true, 0)]
    [BindColumn("GPSTime", "GPS时间", "")]
    public DateTime GPSTime { get => _GPSTime; set { if (OnPropertyChanging("GPSTime", value)) { _GPSTime = value; OnPropertyChanged("GPSTime"); } } }

    private DateTime _CreateTime;
    /// <summary>接收时间</summary>
    [DisplayName("接收时间")]
    [Description("接收时间")]
    [DataObjectField(false, false, true, 0)]
    [BindColumn("CreateTime", "接收时间", "", DefaultValue = "getdate()")]
    public DateTime CreateTime { get => _CreateTime; set { if (OnPropertyChanging("CreateTime", value)) { _CreateTime = value; OnPropertyChanged("CreateTime"); } } }
    #endregion

    #region 拷贝
    /// <summary>拷贝模型对象</summary>
    /// <param name="model">模型</param>
    public void Copy(IPositionData model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        Mobile = model.Mobile;
        Alarm = model.Alarm;
        Status = model.Status;
        Latitude = model.Latitude;
        Longitude = model.Longitude;
        Altitude = model.Altitude;
        Speed = model.Speed;
        Direction = model.Direction;
        Mileage = model.Mileage;
        Fuel = model.Fuel;
        LatitudeBD = model.LatitudeBD;
        LongitudeBD = model.LongitudeBD;
        Extras = model.Extras;
        GPSTime = model.GPSTime;
        CreateTime = model.CreateTime;
    }
    #endregion

    #region 获取/设置 字段值
    /// <summary>获取/设置 字段值</summary>
    /// <param name="name">字段名</param>
    /// <returns></returns>
    public override Object? this[String name]
    {
        get => name switch
        {
            "Id" => _Id,
            "DeviceId" => _DeviceId,
            "Mobile" => _Mobile,
            "Alarm" => _Alarm,
            "Status" => _Status,
            "Latitude" => _Latitude,
            "Longitude" => _Longitude,
            "Altitude" => _Altitude,
            "Speed" => _Speed,
            "Direction" => _Direction,
            "Mileage" => _Mileage,
            "Fuel" => _Fuel,
            "LatitudeBD" => _LatitudeBD,
            "LongitudeBD" => _LongitudeBD,
            "Extras" => _Extras,
            "GPSTime" => _GPSTime,
            "CreateTime" => _CreateTime,
            _ => base[name]
        };
        set
        {
            switch (name)
            {
                case "Id": _Id = value.ToLong(); break;
                case "DeviceId": _DeviceId = value.ToInt(); break;
                case "Mobile": _Mobile = Convert.ToString(value); break;
                case "Alarm": _Alarm = Convert.ToUInt32(value); break;
                case "Status": _Status = Convert.ToUInt32(value); break;
                case "Latitude": _Latitude = value.ToInt(); break;
                case "Longitude": _Longitude = value.ToInt(); break;
                case "Altitude": _Altitude = Convert.ToInt16(value); break;
                case "Speed": _Speed = Convert.ToInt16(value); break;
                case "Direction": _Direction = Convert.ToInt16(value); break;
                case "Mileage": _Mileage = value.ToDouble(); break;
                case "Fuel": _Fuel = value.ToDouble(); break;
                case "LatitudeBD": _LatitudeBD = value.ToDouble(); break;
                case "LongitudeBD": _LongitudeBD = value.ToDouble(); break;
                case "Extras": _Extras = Convert.ToString(value); break;
                case "GPSTime": _GPSTime = value.ToDateTime(); break;
                case "CreateTime": _CreateTime = value.ToDateTime(); break;
                default: base[name] = value; break;
            }
        }
    }
    #endregion

    #region 关联映射
    #endregion

    #region 扩展查询
    /// <summary>根据编号查找</summary>
    /// <param name="id">编号</param>
    /// <returns>实体对象</returns>
    public static PositionData? FindById(Int64 id)
    {
        if (id < 0) return null;

        return Find(_.Id == id);
    }
    #endregion

    #region 高级查询
    /// <summary>高级查询</summary>
    /// <param name="deviceId">设备编号</param>
    /// <param name="mobile">手机号</param>
    /// <param name="gPSTime">GPS时间</param>
    /// <param name="start">编号开始</param>
    /// <param name="end">编号结束</param>
    /// <param name="key">关键字</param>
    /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
    /// <returns>实体列表</returns>
    public static IList<PositionData> Search(Int32 deviceId, String? mobile, DateTime gPSTime, DateTime start, DateTime end, String key, PageParameter page)
    {
        var exp = new WhereExpression();

        if (deviceId >= 0) exp &= _.DeviceId == deviceId;
        if (!mobile.IsNullOrEmpty()) exp &= _.Mobile == mobile;
        exp &= _.Id.Between(start, end, Meta.Factory.Snow);
        if (!key.IsNullOrEmpty()) exp &= SearchWhereByKeys(key);

        return FindAll(exp, page);
    }
    #endregion

    #region 数据清理
    /// <summary>清理指定时间段内的数据</summary>
    /// <param name="start">开始时间。未指定时清理小于指定时间的所有数据</param>
    /// <param name="end">结束时间</param>
    /// <param name="maximumRows">最大删除行数。清理历史数据时，避免一次性删除过多导致数据库IO跟不上，0表示所有</param>
    /// <returns>清理行数</returns>
    public static Int32 DeleteWith(DateTime start, DateTime end, Int32 maximumRows = 0)
    {
        return Delete(_.Id.Between(start, end, Meta.Factory.Snow), maximumRows);
    }
    #endregion

    #region 字段名
    /// <summary>取得位置数据字段信息的快捷方式</summary>
    public partial class _
    {
        /// <summary>编号</summary>
        public static readonly Field Id = FindByName("Id");

        /// <summary>设备编号</summary>
        public static readonly Field DeviceId = FindByName("DeviceId");

        /// <summary>手机号</summary>
        public static readonly Field Mobile = FindByName("Mobile");

        /// <summary>报警标志</summary>
        public static readonly Field Alarm = FindByName("Alarm");

        /// <summary>状态</summary>
        public static readonly Field Status = FindByName("Status");

        /// <summary>纬度</summary>
        public static readonly Field Latitude = FindByName("Latitude");

        /// <summary>经度</summary>
        public static readonly Field Longitude = FindByName("Longitude");

        /// <summary>海拔高度</summary>
        public static readonly Field Altitude = FindByName("Altitude");

        /// <summary>速度(km/h)</summary>
        public static readonly Field Speed = FindByName("Speed");

        /// <summary>方向</summary>
        public static readonly Field Direction = FindByName("Direction");

        /// <summary>里程(km)</summary>
        public static readonly Field Mileage = FindByName("Mileage");

        /// <summary>油量(L)</summary>
        public static readonly Field Fuel = FindByName("Fuel");

        /// <summary>百度纬度</summary>
        public static readonly Field LatitudeBD = FindByName("LatitudeBD");

        /// <summary>百度经度</summary>
        public static readonly Field LongitudeBD = FindByName("LongitudeBD");

        /// <summary>附加信息JSON</summary>
        public static readonly Field Extras = FindByName("Extras");

        /// <summary>GPS时间</summary>
        public static readonly Field GPSTime = FindByName("GPSTime");

        /// <summary>接收时间</summary>
        public static readonly Field CreateTime = FindByName("CreateTime");

        static Field FindByName(String name) => Meta.Table.FindByName(name)!;
    }

    /// <summary>取得位置数据字段名称的快捷方式</summary>
    public partial class __
    {
        /// <summary>编号</summary>
        public const String Id = "Id";

        /// <summary>设备编号</summary>
        public const String DeviceId = "DeviceId";

        /// <summary>手机号</summary>
        public const String Mobile = "Mobile";

        /// <summary>报警标志</summary>
        public const String Alarm = "Alarm";

        /// <summary>状态</summary>
        public const String Status = "Status";

        /// <summary>纬度</summary>
        public const String Latitude = "Latitude";

        /// <summary>经度</summary>
        public const String Longitude = "Longitude";

        /// <summary>海拔高度</summary>
        public const String Altitude = "Altitude";

        /// <summary>速度(km/h)</summary>
        public const String Speed = "Speed";

        /// <summary>方向</summary>
        public const String Direction = "Direction";

        /// <summary>里程(km)</summary>
        public const String Mileage = "Mileage";

        /// <summary>油量(L)</summary>
        public const String Fuel = "Fuel";

        /// <summary>百度纬度</summary>
        public const String LatitudeBD = "LatitudeBD";

        /// <summary>百度经度</summary>
        public const String LongitudeBD = "LongitudeBD";

        /// <summary>附加信息JSON</summary>
        public const String Extras = "Extras";

        /// <summary>GPS时间</summary>
        public const String GPSTime = "GPSTime";

        /// <summary>接收时间</summary>
        public const String CreateTime = "CreateTime";
    }
    #endregion
}
