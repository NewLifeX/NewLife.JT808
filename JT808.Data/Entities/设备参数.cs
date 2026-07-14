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

/// <summary>设备参数。终端参数配置记录</summary>
[Serializable]
[DataObject]
[Description("设备参数。终端参数配置记录")]
[BindIndex("IX_DeviceParameter_DeviceId_ParamId", false, "DeviceId,ParamId")]
[BindTable("DeviceParameter", Description = "设备参数。终端参数配置记录", ConnName = "GPS", DbType = DatabaseType.None)]
public partial class DeviceParameter : IDeviceParameter, IEntity<IDeviceParameter>
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

    private Int32 _ParamId;
    /// <summary>参数ID</summary>
    [DisplayName("参数ID")]
    [Description("参数ID")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("ParamId", "参数ID", "")]
    public Int32 ParamId { get => _ParamId; set { if (OnPropertyChanging("ParamId", value)) { _ParamId = value; OnPropertyChanged("ParamId"); } } }

    private String? _ParamValue;
    /// <summary>参数值</summary>
    [DisplayName("参数值")]
    [Description("参数值")]
    [DataObjectField(false, false, true, 200)]
    [BindColumn("ParamValue", "参数值", "")]
    public String? ParamValue { get => _ParamValue; set { if (OnPropertyChanging("ParamValue", value)) { _ParamValue = value; OnPropertyChanged("ParamValue"); } } }

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
    public void Copy(IDeviceParameter model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        ParamId = model.ParamId;
        ParamValue = model.ParamValue;
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
            "ParamId" => _ParamId,
            "ParamValue" => _ParamValue,
            "CreateTime" => _CreateTime,
            _ => base[name]
        };
        set
        {
            switch (name)
            {
                case "Id": _Id = value.ToInt(); break;
                case "DeviceId": _DeviceId = value.ToInt(); break;
                case "ParamId": _ParamId = value.ToInt(); break;
                case "ParamValue": _ParamValue = Convert.ToString(value); break;
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
    public static DeviceParameter? FindById(Int32 id)
    {
        if (id < 0) return null;

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Id == id);

        // 单对象缓存
        return Meta.SingleCache[id];

        //return Find(_.Id == id);
    }

    /// <summary>根据设备编号、参数ID查找</summary>
    /// <param name="deviceId">设备编号</param>
    /// <param name="paramId">参数ID</param>
    /// <returns>实体列表</returns>
    public static IList<DeviceParameter> FindAllByDeviceIdAndParamId(Int32 deviceId, Int32 paramId)
    {
        if (deviceId < 0) return [];
        if (paramId < 0) return [];

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.FindAll(e => e.DeviceId == deviceId && e.ParamId == paramId);

        return FindAll(_.DeviceId == deviceId & _.ParamId == paramId);
    }
    #endregion

    #region 高级查询
    /// <summary>高级查询</summary>
    /// <param name="deviceId">设备编号</param>
    /// <param name="paramId">参数ID</param>
    /// <param name="start">创建时间开始</param>
    /// <param name="end">创建时间结束</param>
    /// <param name="key">关键字</param>
    /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
    /// <returns>实体列表</returns>
    public static IList<DeviceParameter> Search(Int32 deviceId, Int32 paramId, DateTime start, DateTime end, String key, PageParameter page)
    {
        var exp = new WhereExpression();

        if (deviceId >= 0) exp &= _.DeviceId == deviceId;
        if (paramId >= 0) exp &= _.ParamId == paramId;
        exp &= _.CreateTime.Between(start, end);
        if (!key.IsNullOrEmpty()) exp &= SearchWhereByKeys(key);

        return FindAll(exp, page);
    }
    #endregion

    #region 字段名
    /// <summary>取得设备参数字段信息的快捷方式</summary>
    public partial class _
    {
        /// <summary>编号</summary>
        public static readonly Field Id = FindByName("Id");

        /// <summary>设备编号</summary>
        public static readonly Field DeviceId = FindByName("DeviceId");

        /// <summary>参数ID</summary>
        public static readonly Field ParamId = FindByName("ParamId");

        /// <summary>参数值</summary>
        public static readonly Field ParamValue = FindByName("ParamValue");

        /// <summary>创建时间</summary>
        public static readonly Field CreateTime = FindByName("CreateTime");

        static Field FindByName(String name) => Meta.Table.FindByName(name)!;
    }

    /// <summary>取得设备参数字段名称的快捷方式</summary>
    public partial class __
    {
        /// <summary>编号</summary>
        public const String Id = "Id";

        /// <summary>设备编号</summary>
        public const String DeviceId = "DeviceId";

        /// <summary>参数ID</summary>
        public const String ParamId = "ParamId";

        /// <summary>参数值</summary>
        public const String ParamValue = "ParamValue";

        /// <summary>创建时间</summary>
        public const String CreateTime = "CreateTime";
    }
    #endregion
}
