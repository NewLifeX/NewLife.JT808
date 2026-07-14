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

/// <summary>设备在线。终端设备在线会话信息</summary>
[Serializable]
[DataObject]
[Description("设备在线。终端设备在线会话信息")]
[BindIndex("IU_DeviceOnline_DeviceId", true, "DeviceId")]
[BindIndex("IU_DeviceOnline_Mobile", true, "Mobile")]
[BindTable("DeviceOnline", Description = "设备在线。终端设备在线会话信息", ConnName = "GPS", DbType = DatabaseType.None)]
public partial class DeviceOnline : IDeviceOnline, IEntity<IDeviceOnline>
{
    #region 属性
    private Int32 _Id;
    /// <summary>编号</summary>
    [DisplayName("编号")]
    [Description("编号")]
    [DataObjectField(true, true, false, 0)]
    [BindColumn("Id", "编号", "")]
    public Int32 Id { get => _Id; set { if (OnPropertyChanging("Id", value)) { _Id = value; OnPropertyChanged("Id"); } } }

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
    [BindColumn("Mobile", "手机号", "", Master = true)]
    public String? Mobile { get => _Mobile; set { if (OnPropertyChanging("Mobile", value)) { _Mobile = value; OnPropertyChanged("Mobile"); } } }

    private String? _SessionId;
    /// <summary>会话ID</summary>
    [DisplayName("会话ID")]
    [Description("会话ID")]
    [DataObjectField(false, false, true, 50)]
    [BindColumn("SessionId", "会话ID", "")]
    public String? SessionId { get => _SessionId; set { if (OnPropertyChanging("SessionId", value)) { _SessionId = value; OnPropertyChanged("SessionId"); } } }

    private String? _RemoteEndPoint;
    /// <summary>远程地址</summary>
    [DisplayName("远程地址")]
    [Description("远程地址")]
    [DataObjectField(false, false, true, 50)]
    [BindColumn("RemoteEndPoint", "远程地址", "")]
    public String? RemoteEndPoint { get => _RemoteEndPoint; set { if (OnPropertyChanging("RemoteEndPoint", value)) { _RemoteEndPoint = value; OnPropertyChanged("RemoteEndPoint"); } } }

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

    private String? _Address;
    /// <summary>地址</summary>
    [DisplayName("地址")]
    [Description("地址")]
    [DataObjectField(false, false, true, 200)]
    [BindColumn("Address", "地址", "")]
    public String? Address { get => _Address; set { if (OnPropertyChanging("Address", value)) { _Address = value; OnPropertyChanged("Address"); } } }

    private DateTime _HeartbeatTime;
    /// <summary>最后心跳时间</summary>
    [DisplayName("最后心跳时间")]
    [Description("最后心跳时间")]
    [DataObjectField(false, false, true, 0)]
    [BindColumn("HeartbeatTime", "最后心跳时间", "")]
    public DateTime HeartbeatTime { get => _HeartbeatTime; set { if (OnPropertyChanging("HeartbeatTime", value)) { _HeartbeatTime = value; OnPropertyChanged("HeartbeatTime"); } } }

    private Boolean _Online;
    /// <summary>在线状态</summary>
    [DisplayName("在线状态")]
    [Description("在线状态")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Online", "在线状态", "", DefaultValue = "True")]
    public Boolean Online { get => _Online; set { if (OnPropertyChanging("Online", value)) { _Online = value; OnPropertyChanged("Online"); } } }

    private DateTime _CreateTime;
    /// <summary>上线时间</summary>
    [DisplayName("上线时间")]
    [Description("上线时间")]
    [DataObjectField(false, false, true, 0)]
    [BindColumn("CreateTime", "上线时间", "", DefaultValue = "getdate()")]
    public DateTime CreateTime { get => _CreateTime; set { if (OnPropertyChanging("CreateTime", value)) { _CreateTime = value; OnPropertyChanged("CreateTime"); } } }

    private DateTime _UpdateTime;
    /// <summary>更新时间</summary>
    [DisplayName("更新时间")]
    [Description("更新时间")]
    [DataObjectField(false, false, true, 0)]
    [BindColumn("UpdateTime", "更新时间", "", DefaultValue = "getdate()")]
    public DateTime UpdateTime { get => _UpdateTime; set { if (OnPropertyChanging("UpdateTime", value)) { _UpdateTime = value; OnPropertyChanged("UpdateTime"); } } }
    #endregion

    #region 拷贝
    /// <summary>拷贝模型对象</summary>
    /// <param name="model">模型</param>
    public void Copy(IDeviceOnline model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        Mobile = model.Mobile;
        SessionId = model.SessionId;
        RemoteEndPoint = model.RemoteEndPoint;
        Latitude = model.Latitude;
        Longitude = model.Longitude;
        Speed = model.Speed;
        Direction = model.Direction;
        Address = model.Address;
        HeartbeatTime = model.HeartbeatTime;
        Online = model.Online;
        CreateTime = model.CreateTime;
        UpdateTime = model.UpdateTime;
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
            "SessionId" => _SessionId,
            "RemoteEndPoint" => _RemoteEndPoint,
            "Latitude" => _Latitude,
            "Longitude" => _Longitude,
            "Speed" => _Speed,
            "Direction" => _Direction,
            "Address" => _Address,
            "HeartbeatTime" => _HeartbeatTime,
            "Online" => _Online,
            "CreateTime" => _CreateTime,
            "UpdateTime" => _UpdateTime,
            _ => base[name]
        };
        set
        {
            switch (name)
            {
                case "Id": _Id = value.ToInt(); break;
                case "DeviceId": _DeviceId = value.ToInt(); break;
                case "Mobile": _Mobile = Convert.ToString(value); break;
                case "SessionId": _SessionId = Convert.ToString(value); break;
                case "RemoteEndPoint": _RemoteEndPoint = Convert.ToString(value); break;
                case "Latitude": _Latitude = value.ToInt(); break;
                case "Longitude": _Longitude = value.ToInt(); break;
                case "Speed": _Speed = Convert.ToInt16(value); break;
                case "Direction": _Direction = Convert.ToInt16(value); break;
                case "Address": _Address = Convert.ToString(value); break;
                case "HeartbeatTime": _HeartbeatTime = value.ToDateTime(); break;
                case "Online": _Online = value.ToBoolean(); break;
                case "CreateTime": _CreateTime = value.ToDateTime(); break;
                case "UpdateTime": _UpdateTime = value.ToDateTime(); break;
                default: base[name] = value; break;
            }
        }
    }
    #endregion

    #region 关联映射
    /// <summary>设备编号</summary>
    [XmlIgnore, IgnoreDataMember, ScriptIgnore]
    public Device? Device => Extends.Get(nameof(Device), k => Device.FindById(DeviceId));

    /// <summary>设备编号</summary>
    [Map(nameof(DeviceId), typeof(Device), "Id")]
    public String? DeviceMobile => Device?.Mobile;

    #endregion

    #region 扩展查询
    /// <summary>根据编号查找</summary>
    /// <param name="id">编号</param>
    /// <returns>实体对象</returns>
    public static DeviceOnline? FindById(Int32 id)
    {
        if (id < 0) return null;

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Id == id);

        // 单对象缓存
        return Meta.SingleCache[id];

        //return Find(_.Id == id);
    }

    /// <summary>根据设备编号查找</summary>
    /// <param name="deviceId">设备编号</param>
    /// <returns>实体对象</returns>
    public static DeviceOnline? FindByDeviceId(Int32 deviceId)
    {
        if (deviceId < 0) return null;

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.DeviceId == deviceId);

        return Find(_.DeviceId == deviceId);
    }

    /// <summary>根据手机号查找</summary>
    /// <param name="mobile">手机号</param>
    /// <returns>实体对象</returns>
    public static DeviceOnline? FindByMobile(String? mobile)
    {
        if (mobile == null) return null;

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Mobile.EqualIgnoreCase(mobile));

        // 单对象缓存
        return Meta.SingleCache.GetItemWithSlaveKey(mobile) as DeviceOnline;

        //return Find(_.Mobile == mobile);
    }
    #endregion

    #region 高级查询
    /// <summary>高级查询</summary>
    /// <param name="deviceId">设备编号</param>
    /// <param name="online">在线状态</param>
    /// <param name="start">更新时间开始</param>
    /// <param name="end">更新时间结束</param>
    /// <param name="key">关键字</param>
    /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
    /// <returns>实体列表</returns>
    public static IList<DeviceOnline> Search(Int32 deviceId, Boolean? online, DateTime start, DateTime end, String key, PageParameter page)
    {
        var exp = new WhereExpression();

        if (deviceId >= 0) exp &= _.DeviceId == deviceId;
        if (online != null) exp &= _.Online == online;
        exp &= _.UpdateTime.Between(start, end);
        if (!key.IsNullOrEmpty()) exp &= SearchWhereByKeys(key);

        return FindAll(exp, page);
    }
    #endregion

    #region 字段名
    /// <summary>取得设备在线字段信息的快捷方式</summary>
    public partial class _
    {
        /// <summary>编号</summary>
        public static readonly Field Id = FindByName("Id");

        /// <summary>设备编号</summary>
        public static readonly Field DeviceId = FindByName("DeviceId");

        /// <summary>手机号</summary>
        public static readonly Field Mobile = FindByName("Mobile");

        /// <summary>会话ID</summary>
        public static readonly Field SessionId = FindByName("SessionId");

        /// <summary>远程地址</summary>
        public static readonly Field RemoteEndPoint = FindByName("RemoteEndPoint");

        /// <summary>纬度</summary>
        public static readonly Field Latitude = FindByName("Latitude");

        /// <summary>经度</summary>
        public static readonly Field Longitude = FindByName("Longitude");

        /// <summary>速度</summary>
        public static readonly Field Speed = FindByName("Speed");

        /// <summary>方向</summary>
        public static readonly Field Direction = FindByName("Direction");

        /// <summary>地址</summary>
        public static readonly Field Address = FindByName("Address");

        /// <summary>最后心跳时间</summary>
        public static readonly Field HeartbeatTime = FindByName("HeartbeatTime");

        /// <summary>在线状态</summary>
        public static readonly Field Online = FindByName("Online");

        /// <summary>上线时间</summary>
        public static readonly Field CreateTime = FindByName("CreateTime");

        /// <summary>更新时间</summary>
        public static readonly Field UpdateTime = FindByName("UpdateTime");

        static Field FindByName(String name) => Meta.Table.FindByName(name)!;
    }

    /// <summary>取得设备在线字段名称的快捷方式</summary>
    public partial class __
    {
        /// <summary>编号</summary>
        public const String Id = "Id";

        /// <summary>设备编号</summary>
        public const String DeviceId = "DeviceId";

        /// <summary>手机号</summary>
        public const String Mobile = "Mobile";

        /// <summary>会话ID</summary>
        public const String SessionId = "SessionId";

        /// <summary>远程地址</summary>
        public const String RemoteEndPoint = "RemoteEndPoint";

        /// <summary>纬度</summary>
        public const String Latitude = "Latitude";

        /// <summary>经度</summary>
        public const String Longitude = "Longitude";

        /// <summary>速度</summary>
        public const String Speed = "Speed";

        /// <summary>方向</summary>
        public const String Direction = "Direction";

        /// <summary>地址</summary>
        public const String Address = "Address";

        /// <summary>最后心跳时间</summary>
        public const String HeartbeatTime = "HeartbeatTime";

        /// <summary>在线状态</summary>
        public const String Online = "Online";

        /// <summary>上线时间</summary>
        public const String CreateTime = "CreateTime";

        /// <summary>更新时间</summary>
        public const String UpdateTime = "UpdateTime";
    }
    #endregion
}
