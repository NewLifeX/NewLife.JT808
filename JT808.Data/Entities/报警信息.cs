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

/// <summary>报警信息。终端上报的报警事件</summary>
[Serializable]
[DataObject]
[Description("报警信息。终端上报的报警事件")]
[BindIndex("IX_Alarm_DeviceId_AlarmTime", false, "DeviceId,AlarmTime")]
[BindTable("Alarm", Description = "报警信息。终端上报的报警事件", ConnName = "GPS", DbType = DatabaseType.None)]
public partial class Alarm : IAlarm, IEntity<IAlarm>
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

    private Int32 _AlarmType;
    /// <summary>报警类型</summary>
    [DisplayName("报警类型")]
    [Description("报警类型")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("AlarmType", "报警类型", "")]
    public Int32 AlarmType { get => _AlarmType; set { if (OnPropertyChanging("AlarmType", value)) { _AlarmType = value; OnPropertyChanged("AlarmType"); } } }

    private DateTime _AlarmTime;
    /// <summary>报警时间</summary>
    [DisplayName("报警时间")]
    [Description("报警时间")]
    [DataObjectField(false, false, true, 0)]
    [BindColumn("AlarmTime", "报警时间", "")]
    public DateTime AlarmTime { get => _AlarmTime; set { if (OnPropertyChanging("AlarmTime", value)) { _AlarmTime = value; OnPropertyChanged("AlarmTime"); } } }

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

    private Int16 _Speed;
    /// <summary>速度</summary>
    [DisplayName("速度")]
    [Description("速度")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Speed", "速度", "")]
    public Int16 Speed { get => _Speed; set { if (OnPropertyChanging("Speed", value)) { _Speed = value; OnPropertyChanged("Speed"); } } }

    private Int16 _Direction;
    /// <summary>方向</summary>
    [DisplayName("方向")]
    [Description("方向")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Direction", "方向", "")]
    public Int16 Direction { get => _Direction; set { if (OnPropertyChanging("Direction", value)) { _Direction = value; OnPropertyChanged("Direction"); } } }

    private Int16 _Altitude;
    /// <summary>海拔</summary>
    [DisplayName("海拔")]
    [Description("海拔")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Altitude", "海拔", "")]
    public Int16 Altitude { get => _Altitude; set { if (OnPropertyChanging("Altitude", value)) { _Altitude = value; OnPropertyChanged("Altitude"); } } }

    private String? _Content;
    /// <summary>报警内容</summary>
    [DisplayName("报警内容")]
    [Description("报警内容")]
    [DataObjectField(false, false, true, 2000)]
    [BindColumn("Content", "报警内容", "")]
    public String? Content { get => _Content; set { if (OnPropertyChanging("Content", value)) { _Content = value; OnPropertyChanged("Content"); } } }

    private DateTime _CreateTime;
    /// <summary>创建时间</summary>
    [DisplayName("创建时间")]
    [Description("创建时间")]
    [DataObjectField(false, false, true, 0)]
    [BindColumn("CreateTime", "创建时间", "", DefaultValue = "getdate()")]
    public DateTime CreateTime { get => _CreateTime; set { if (OnPropertyChanging("CreateTime", value)) { _CreateTime = value; OnPropertyChanged("CreateTime"); } } }
    #endregion

    #region 拷贝
    /// <summary>拷贝模型对象</summary>
    /// <param name="model">模型</param>
    public void Copy(IAlarm model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        Mobile = model.Mobile;
        AlarmType = model.AlarmType;
        AlarmTime = model.AlarmTime;
        Latitude = model.Latitude;
        Longitude = model.Longitude;
        Speed = model.Speed;
        Direction = model.Direction;
        Altitude = model.Altitude;
        Content = model.Content;
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
            "AlarmType" => _AlarmType,
            "AlarmTime" => _AlarmTime,
            "Latitude" => _Latitude,
            "Longitude" => _Longitude,
            "Speed" => _Speed,
            "Direction" => _Direction,
            "Altitude" => _Altitude,
            "Content" => _Content,
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
                case "AlarmType": _AlarmType = value.ToInt(); break;
                case "AlarmTime": _AlarmTime = value.ToDateTime(); break;
                case "Latitude": _Latitude = value.ToInt(); break;
                case "Longitude": _Longitude = value.ToInt(); break;
                case "Speed": _Speed = Convert.ToInt16(value); break;
                case "Direction": _Direction = Convert.ToInt16(value); break;
                case "Altitude": _Altitude = Convert.ToInt16(value); break;
                case "Content": _Content = Convert.ToString(value); break;
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
    public static Alarm? FindById(Int64 id)
    {
        if (id < 0) return null;

        return Find(_.Id == id);
    }
    #endregion

    #region 高级查询
    /// <summary>高级查询</summary>
    /// <param name="deviceId">设备编号</param>
    /// <param name="alarmTime">报警时间</param>
    /// <param name="start">编号开始</param>
    /// <param name="end">编号结束</param>
    /// <param name="key">关键字</param>
    /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
    /// <returns>实体列表</returns>
    public static IList<Alarm> Search(Int32 deviceId, DateTime alarmTime, DateTime start, DateTime end, String key, PageParameter page)
    {
        var exp = new WhereExpression();

        if (deviceId >= 0) exp &= _.DeviceId == deviceId;
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
    /// <summary>取得报警信息字段信息的快捷方式</summary>
    public partial class _
    {
        /// <summary>编号</summary>
        public static readonly Field Id = FindByName("Id");

        /// <summary>设备编号</summary>
        public static readonly Field DeviceId = FindByName("DeviceId");

        /// <summary>手机号</summary>
        public static readonly Field Mobile = FindByName("Mobile");

        /// <summary>报警类型</summary>
        public static readonly Field AlarmType = FindByName("AlarmType");

        /// <summary>报警时间</summary>
        public static readonly Field AlarmTime = FindByName("AlarmTime");

        /// <summary>纬度</summary>
        public static readonly Field Latitude = FindByName("Latitude");

        /// <summary>经度</summary>
        public static readonly Field Longitude = FindByName("Longitude");

        /// <summary>速度</summary>
        public static readonly Field Speed = FindByName("Speed");

        /// <summary>方向</summary>
        public static readonly Field Direction = FindByName("Direction");

        /// <summary>海拔</summary>
        public static readonly Field Altitude = FindByName("Altitude");

        /// <summary>报警内容</summary>
        public static readonly Field Content = FindByName("Content");

        /// <summary>创建时间</summary>
        public static readonly Field CreateTime = FindByName("CreateTime");

        static Field FindByName(String name) => Meta.Table.FindByName(name)!;
    }

    /// <summary>取得报警信息字段名称的快捷方式</summary>
    public partial class __
    {
        /// <summary>编号</summary>
        public const String Id = "Id";

        /// <summary>设备编号</summary>
        public const String DeviceId = "DeviceId";

        /// <summary>手机号</summary>
        public const String Mobile = "Mobile";

        /// <summary>报警类型</summary>
        public const String AlarmType = "AlarmType";

        /// <summary>报警时间</summary>
        public const String AlarmTime = "AlarmTime";

        /// <summary>纬度</summary>
        public const String Latitude = "Latitude";

        /// <summary>经度</summary>
        public const String Longitude = "Longitude";

        /// <summary>速度</summary>
        public const String Speed = "Speed";

        /// <summary>方向</summary>
        public const String Direction = "Direction";

        /// <summary>海拔</summary>
        public const String Altitude = "Altitude";

        /// <summary>报警内容</summary>
        public const String Content = "Content";

        /// <summary>创建时间</summary>
        public const String CreateTime = "CreateTime";
    }
    #endregion
}
