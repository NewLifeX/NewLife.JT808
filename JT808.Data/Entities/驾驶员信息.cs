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

/// <summary>驾驶员信息</summary>
[Serializable]
[DataObject]
[Description("驾驶员信息")]
[BindIndex("IX_Driver_DeviceId", false, "DeviceId")]
[BindTable("Driver", Description = "驾驶员信息", ConnName = "GPS", DbType = DatabaseType.None)]
public partial class Driver : IDriver, IEntity<IDriver>
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

    private String? _Name;
    /// <summary>姓名</summary>
    [DisplayName("姓名")]
    [Description("姓名")]
    [DataObjectField(false, false, true, 20)]
    [BindColumn("Name", "姓名", "", Master = true)]
    public String? Name { get => _Name; set { if (OnPropertyChanging("Name", value)) { _Name = value; OnPropertyChanged("Name"); } } }

    private String? _IDCard;
    /// <summary>身份证号</summary>
    [DisplayName("身份证号")]
    [Description("身份证号")]
    [DataObjectField(false, false, true, 20)]
    [BindColumn("IDCard", "身份证号", "")]
    public String? IDCard { get => _IDCard; set { if (OnPropertyChanging("IDCard", value)) { _IDCard = value; OnPropertyChanged("IDCard"); } } }

    private String? _Qualification;
    /// <summary>从业资格证号</summary>
    [DisplayName("从业资格证号")]
    [Description("从业资格证号")]
    [DataObjectField(false, false, true, 40)]
    [BindColumn("Qualification", "从业资格证号", "")]
    public String? Qualification { get => _Qualification; set { if (OnPropertyChanging("Qualification", value)) { _Qualification = value; OnPropertyChanged("Qualification"); } } }

    private String? _Mobile;
    /// <summary>手机号</summary>
    [DisplayName("手机号")]
    [Description("手机号")]
    [DataObjectField(false, false, true, 20)]
    [BindColumn("Mobile", "手机号", "")]
    public String? Mobile { get => _Mobile; set { if (OnPropertyChanging("Mobile", value)) { _Mobile = value; OnPropertyChanged("Mobile"); } } }

    private DateTime _CreateTime;
    /// <summary>创建时间</summary>
    [DisplayName("创建时间")]
    [Description("创建时间")]
    [DataObjectField(false, false, true, 0)]
    [BindColumn("CreateTime", "创建时间", "", DefaultValue = "getdate()")]
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
    public void Copy(IDriver model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        Name = model.Name;
        IDCard = model.IDCard;
        Qualification = model.Qualification;
        Mobile = model.Mobile;
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
            "Name" => _Name,
            "IDCard" => _IDCard,
            "Qualification" => _Qualification,
            "Mobile" => _Mobile,
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
                case "Name": _Name = Convert.ToString(value); break;
                case "IDCard": _IDCard = Convert.ToString(value); break;
                case "Qualification": _Qualification = Convert.ToString(value); break;
                case "Mobile": _Mobile = Convert.ToString(value); break;
                case "CreateTime": _CreateTime = value.ToDateTime(); break;
                case "UpdateTime": _UpdateTime = value.ToDateTime(); break;
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
    public static Driver? FindById(Int32 id)
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
    /// <returns>实体列表</returns>
    public static IList<Driver> FindAllByDeviceId(Int32 deviceId)
    {
        if (deviceId < 0) return [];

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.FindAll(e => e.DeviceId == deviceId);

        return FindAll(_.DeviceId == deviceId);
    }
    #endregion

    #region 高级查询
    /// <summary>高级查询</summary>
    /// <param name="deviceId">设备编号</param>
    /// <param name="start">更新时间开始</param>
    /// <param name="end">更新时间结束</param>
    /// <param name="key">关键字</param>
    /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
    /// <returns>实体列表</returns>
    public static IList<Driver> Search(Int32 deviceId, DateTime start, DateTime end, String key, PageParameter page)
    {
        var exp = new WhereExpression();

        if (deviceId >= 0) exp &= _.DeviceId == deviceId;
        exp &= _.UpdateTime.Between(start, end);
        if (!key.IsNullOrEmpty()) exp &= SearchWhereByKeys(key);

        return FindAll(exp, page);
    }
    #endregion

    #region 字段名
    /// <summary>取得驾驶员信息字段信息的快捷方式</summary>
    public partial class _
    {
        /// <summary>编号</summary>
        public static readonly Field Id = FindByName("Id");

        /// <summary>设备编号</summary>
        public static readonly Field DeviceId = FindByName("DeviceId");

        /// <summary>姓名</summary>
        public static readonly Field Name = FindByName("Name");

        /// <summary>身份证号</summary>
        public static readonly Field IDCard = FindByName("IDCard");

        /// <summary>从业资格证号</summary>
        public static readonly Field Qualification = FindByName("Qualification");

        /// <summary>手机号</summary>
        public static readonly Field Mobile = FindByName("Mobile");

        /// <summary>创建时间</summary>
        public static readonly Field CreateTime = FindByName("CreateTime");

        /// <summary>更新时间</summary>
        public static readonly Field UpdateTime = FindByName("UpdateTime");

        static Field FindByName(String name) => Meta.Table.FindByName(name)!;
    }

    /// <summary>取得驾驶员信息字段名称的快捷方式</summary>
    public partial class __
    {
        /// <summary>编号</summary>
        public const String Id = "Id";

        /// <summary>设备编号</summary>
        public const String DeviceId = "DeviceId";

        /// <summary>姓名</summary>
        public const String Name = "Name";

        /// <summary>身份证号</summary>
        public const String IDCard = "IDCard";

        /// <summary>从业资格证号</summary>
        public const String Qualification = "Qualification";

        /// <summary>手机号</summary>
        public const String Mobile = "Mobile";

        /// <summary>创建时间</summary>
        public const String CreateTime = "CreateTime";

        /// <summary>更新时间</summary>
        public const String UpdateTime = "UpdateTime";
    }
    #endregion
}
