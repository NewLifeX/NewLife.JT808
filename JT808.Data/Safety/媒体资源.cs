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

namespace JT808.Data.Safety;

/// <summary>媒体资源。主动安全多媒体资源索引</summary>
[Serializable]
[DataObject]
[Description("媒体资源。主动安全多媒体资源索引")]
[BindIndex("IX_MediaResource_DeviceId_AlarmTime", false, "DeviceId,AlarmTime")]
[BindTable("MediaResource", Description = "媒体资源。主动安全多媒体资源索引", ConnName = "Safety", DbType = DatabaseType.None)]
public partial class MediaResource : IMediaResource, IEntity<IMediaResource>
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

    private DateTime _AlarmTime;
    /// <summary>报警时间</summary>
    [DisplayName("报警时间")]
    [Description("报警时间")]
    [DataObjectField(false, false, true, 0)]
    [BindColumn("AlarmTime", "报警时间", "")]
    public DateTime AlarmTime { get => _AlarmTime; set { if (OnPropertyChanging("AlarmTime", value)) { _AlarmTime = value; OnPropertyChanged("AlarmTime"); } } }

    private Int32 _MediaType;
    /// <summary>媒体类型</summary>
    [DisplayName("媒体类型")]
    [Description("媒体类型")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("MediaType", "媒体类型", "")]
    public Int32 MediaType { get => _MediaType; set { if (OnPropertyChanging("MediaType", value)) { _MediaType = value; OnPropertyChanged("MediaType"); } } }

    private String? _Url;
    /// <summary>资源地址</summary>
    [DisplayName("资源地址")]
    [Description("资源地址")]
    [DataObjectField(false, false, true, 500)]
    [BindColumn("Url", "资源地址", "")]
    public String? Url { get => _Url; set { if (OnPropertyChanging("Url", value)) { _Url = value; OnPropertyChanged("Url"); } } }

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
    public void Copy(IMediaResource model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        Mobile = model.Mobile;
        AlarmTime = model.AlarmTime;
        MediaType = model.MediaType;
        Url = model.Url;
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
            "AlarmTime" => _AlarmTime,
            "MediaType" => _MediaType,
            "Url" => _Url,
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
                case "AlarmTime": _AlarmTime = value.ToDateTime(); break;
                case "MediaType": _MediaType = value.ToInt(); break;
                case "Url": _Url = Convert.ToString(value); break;
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
    public static MediaResource? FindById(Int64 id)
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
    public static IList<MediaResource> Search(Int32 deviceId, DateTime alarmTime, DateTime start, DateTime end, String key, PageParameter page)
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
    /// <summary>取得媒体资源字段信息的快捷方式</summary>
    public partial class _
    {
        /// <summary>编号</summary>
        public static readonly Field Id = FindByName("Id");

        /// <summary>设备编号</summary>
        public static readonly Field DeviceId = FindByName("DeviceId");

        /// <summary>手机号</summary>
        public static readonly Field Mobile = FindByName("Mobile");

        /// <summary>报警时间</summary>
        public static readonly Field AlarmTime = FindByName("AlarmTime");

        /// <summary>媒体类型</summary>
        public static readonly Field MediaType = FindByName("MediaType");

        /// <summary>资源地址</summary>
        public static readonly Field Url = FindByName("Url");

        /// <summary>创建时间</summary>
        public static readonly Field CreateTime = FindByName("CreateTime");

        static Field FindByName(String name) => Meta.Table.FindByName(name)!;
    }

    /// <summary>取得媒体资源字段名称的快捷方式</summary>
    public partial class __
    {
        /// <summary>编号</summary>
        public const String Id = "Id";

        /// <summary>设备编号</summary>
        public const String DeviceId = "DeviceId";

        /// <summary>手机号</summary>
        public const String Mobile = "Mobile";

        /// <summary>报警时间</summary>
        public const String AlarmTime = "AlarmTime";

        /// <summary>媒体类型</summary>
        public const String MediaType = "MediaType";

        /// <summary>资源地址</summary>
        public const String Url = "Url";

        /// <summary>创建时间</summary>
        public const String CreateTime = "CreateTime";
    }
    #endregion
}
