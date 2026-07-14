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

/// <summary>胎压数据。轮胎压力和温度监测</summary>
[Serializable]
[DataObject]
[Description("胎压数据。轮胎压力和温度监测")]
[BindIndex("IX_TirePressure_DeviceId_CreateTime", false, "DeviceId,CreateTime")]
[BindTable("TirePressure", Description = "胎压数据。轮胎压力和温度监测", ConnName = "GPS", DbType = DatabaseType.None)]
public partial class TirePressure : ITirePressure, IEntity<ITirePressure>
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

    private Int32 _TireNo;
    /// <summary>轮胎编号</summary>
    [DisplayName("轮胎编号")]
    [Description("轮胎编号")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("TireNo", "轮胎编号", "")]
    public Int32 TireNo { get => _TireNo; set { if (OnPropertyChanging("TireNo", value)) { _TireNo = value; OnPropertyChanged("TireNo"); } } }

    private Double _Pressure;
    /// <summary>胎压(Pa)</summary>
    [DisplayName("胎压(Pa)")]
    [Description("胎压(Pa)")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Pressure", "胎压(Pa)", "")]
    public Double Pressure { get => _Pressure; set { if (OnPropertyChanging("Pressure", value)) { _Pressure = value; OnPropertyChanged("Pressure"); } } }

    private Int16 _Temperature;
    /// <summary>温度(℃)</summary>
    [DisplayName("温度(℃)")]
    [Description("温度(℃)")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Temperature", "温度(℃)", "")]
    public Int16 Temperature { get => _Temperature; set { if (OnPropertyChanging("Temperature", value)) { _Temperature = value; OnPropertyChanged("Temperature"); } } }

    private Boolean _Alarm;
    /// <summary>报警状态</summary>
    [DisplayName("报警状态")]
    [Description("报警状态")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Alarm", "报警状态", "")]
    public Boolean Alarm { get => _Alarm; set { if (OnPropertyChanging("Alarm", value)) { _Alarm = value; OnPropertyChanged("Alarm"); } } }

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
    public void Copy(ITirePressure model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        Mobile = model.Mobile;
        TireNo = model.TireNo;
        Pressure = model.Pressure;
        Temperature = model.Temperature;
        Alarm = model.Alarm;
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
            "TireNo" => _TireNo,
            "Pressure" => _Pressure,
            "Temperature" => _Temperature,
            "Alarm" => _Alarm,
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
                case "TireNo": _TireNo = value.ToInt(); break;
                case "Pressure": _Pressure = value.ToDouble(); break;
                case "Temperature": _Temperature = Convert.ToInt16(value); break;
                case "Alarm": _Alarm = value.ToBoolean(); break;
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
    public static TirePressure? FindById(Int64 id)
    {
        if (id < 0) return null;

        return Find(_.Id == id);
    }
    #endregion

    #region 高级查询
    /// <summary>高级查询</summary>
    /// <param name="deviceId">设备编号</param>
    /// <param name="createTime">创建时间</param>
    /// <param name="alarm">报警状态</param>
    /// <param name="start">编号开始</param>
    /// <param name="end">编号结束</param>
    /// <param name="key">关键字</param>
    /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
    /// <returns>实体列表</returns>
    public static IList<TirePressure> Search(Int32 deviceId, DateTime createTime, Boolean? alarm, DateTime start, DateTime end, String key, PageParameter page)
    {
        var exp = new WhereExpression();

        if (deviceId >= 0) exp &= _.DeviceId == deviceId;
        if (alarm != null) exp &= _.Alarm == alarm;
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
    /// <summary>取得胎压数据字段信息的快捷方式</summary>
    public partial class _
    {
        /// <summary>编号</summary>
        public static readonly Field Id = FindByName("Id");

        /// <summary>设备编号</summary>
        public static readonly Field DeviceId = FindByName("DeviceId");

        /// <summary>手机号</summary>
        public static readonly Field Mobile = FindByName("Mobile");

        /// <summary>轮胎编号</summary>
        public static readonly Field TireNo = FindByName("TireNo");

        /// <summary>胎压(Pa)</summary>
        public static readonly Field Pressure = FindByName("Pressure");

        /// <summary>温度(℃)</summary>
        public static readonly Field Temperature = FindByName("Temperature");

        /// <summary>报警状态</summary>
        public static readonly Field Alarm = FindByName("Alarm");

        /// <summary>创建时间</summary>
        public static readonly Field CreateTime = FindByName("CreateTime");

        static Field FindByName(String name) => Meta.Table.FindByName(name)!;
    }

    /// <summary>取得胎压数据字段名称的快捷方式</summary>
    public partial class __
    {
        /// <summary>编号</summary>
        public const String Id = "Id";

        /// <summary>设备编号</summary>
        public const String DeviceId = "DeviceId";

        /// <summary>手机号</summary>
        public const String Mobile = "Mobile";

        /// <summary>轮胎编号</summary>
        public const String TireNo = "TireNo";

        /// <summary>胎压(Pa)</summary>
        public const String Pressure = "Pressure";

        /// <summary>温度(℃)</summary>
        public const String Temperature = "Temperature";

        /// <summary>报警状态</summary>
        public const String Alarm = "Alarm";

        /// <summary>创建时间</summary>
        public const String CreateTime = "CreateTime";
    }
    #endregion
}
