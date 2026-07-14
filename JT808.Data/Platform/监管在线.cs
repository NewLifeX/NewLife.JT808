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

namespace JT808.Data.Platform;

/// <summary>监管在线。监管平台在线状态</summary>
[Serializable]
[DataObject]
[Description("监管在线。监管平台在线状态")]
[BindIndex("IU_SuperviseOnline_ServerId", true, "ServerId")]
[BindTable("SuperviseOnline", Description = "监管在线。监管平台在线状态", ConnName = "Platform", DbType = DatabaseType.None)]
public partial class SuperviseOnline : ISuperviseOnline, IEntity<ISuperviseOnline>
{
    #region 属性
    private Int32 _Id;
    /// <summary>编号</summary>
    [DisplayName("编号")]
    [Description("编号")]
    [DataObjectField(true, true, false, 0)]
    [BindColumn("Id", "编号", "")]
    public Int32 Id { get => _Id; set { if (OnPropertyChanging("Id", value)) { _Id = value; OnPropertyChanged("Id"); } } }

    private Int32 _ServerId;
    /// <summary>监管服务编号</summary>
    [DisplayName("监管服务编号")]
    [Description("监管服务编号")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("ServerId", "监管服务编号", "")]
    public Int32 ServerId { get => _ServerId; set { if (OnPropertyChanging("ServerId", value)) { _ServerId = value; OnPropertyChanged("ServerId"); } } }

    private Boolean _Online;
    /// <summary>在线状态</summary>
    [DisplayName("在线状态")]
    [Description("在线状态")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Online", "在线状态", "")]
    public Boolean Online { get => _Online; set { if (OnPropertyChanging("Online", value)) { _Online = value; OnPropertyChanged("Online"); } } }

    private DateTime _HeartbeatTime;
    /// <summary>最后心跳时间</summary>
    [DisplayName("最后心跳时间")]
    [Description("最后心跳时间")]
    [DataObjectField(false, false, true, 0)]
    [BindColumn("HeartbeatTime", "最后心跳时间", "")]
    public DateTime HeartbeatTime { get => _HeartbeatTime; set { if (OnPropertyChanging("HeartbeatTime", value)) { _HeartbeatTime = value; OnPropertyChanged("HeartbeatTime"); } } }

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
    public void Copy(ISuperviseOnline model)
    {
        Id = model.Id;
        ServerId = model.ServerId;
        Online = model.Online;
        HeartbeatTime = model.HeartbeatTime;
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
            "ServerId" => _ServerId,
            "Online" => _Online,
            "HeartbeatTime" => _HeartbeatTime,
            "CreateTime" => _CreateTime,
            "UpdateTime" => _UpdateTime,
            _ => base[name]
        };
        set
        {
            switch (name)
            {
                case "Id": _Id = value.ToInt(); break;
                case "ServerId": _ServerId = value.ToInt(); break;
                case "Online": _Online = value.ToBoolean(); break;
                case "HeartbeatTime": _HeartbeatTime = value.ToDateTime(); break;
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
    public static SuperviseOnline? FindById(Int32 id)
    {
        if (id < 0) return null;

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Id == id);

        // 单对象缓存
        return Meta.SingleCache[id];

        //return Find(_.Id == id);
    }

    /// <summary>根据监管服务编号查找</summary>
    /// <param name="serverId">监管服务编号</param>
    /// <returns>实体对象</returns>
    public static SuperviseOnline? FindByServerId(Int32 serverId)
    {
        if (serverId < 0) return null;

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.ServerId == serverId);

        return Find(_.ServerId == serverId);
    }
    #endregion

    #region 高级查询
    /// <summary>高级查询</summary>
    /// <param name="serverId">监管服务编号</param>
    /// <param name="online">在线状态</param>
    /// <param name="start">更新时间开始</param>
    /// <param name="end">更新时间结束</param>
    /// <param name="key">关键字</param>
    /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
    /// <returns>实体列表</returns>
    public static IList<SuperviseOnline> Search(Int32 serverId, Boolean? online, DateTime start, DateTime end, String key, PageParameter page)
    {
        var exp = new WhereExpression();

        if (serverId >= 0) exp &= _.ServerId == serverId;
        if (online != null) exp &= _.Online == online;
        exp &= _.UpdateTime.Between(start, end);
        if (!key.IsNullOrEmpty()) exp &= SearchWhereByKeys(key);

        return FindAll(exp, page);
    }
    #endregion

    #region 字段名
    /// <summary>取得监管在线字段信息的快捷方式</summary>
    public partial class _
    {
        /// <summary>编号</summary>
        public static readonly Field Id = FindByName("Id");

        /// <summary>监管服务编号</summary>
        public static readonly Field ServerId = FindByName("ServerId");

        /// <summary>在线状态</summary>
        public static readonly Field Online = FindByName("Online");

        /// <summary>最后心跳时间</summary>
        public static readonly Field HeartbeatTime = FindByName("HeartbeatTime");

        /// <summary>创建时间</summary>
        public static readonly Field CreateTime = FindByName("CreateTime");

        /// <summary>更新时间</summary>
        public static readonly Field UpdateTime = FindByName("UpdateTime");

        static Field FindByName(String name) => Meta.Table.FindByName(name)!;
    }

    /// <summary>取得监管在线字段名称的快捷方式</summary>
    public partial class __
    {
        /// <summary>编号</summary>
        public const String Id = "Id";

        /// <summary>监管服务编号</summary>
        public const String ServerId = "ServerId";

        /// <summary>在线状态</summary>
        public const String Online = "Online";

        /// <summary>最后心跳时间</summary>
        public const String HeartbeatTime = "HeartbeatTime";

        /// <summary>创建时间</summary>
        public const String CreateTime = "CreateTime";

        /// <summary>更新时间</summary>
        public const String UpdateTime = "UpdateTime";
    }
    #endregion
}
