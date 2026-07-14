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

/// <summary>物联网卡。SIM卡信息管理</summary>
[Serializable]
[DataObject]
[Description("物联网卡。SIM卡信息管理")]
[BindIndex("IU_SimCard_Mobile", true, "Mobile")]
[BindTable("SimCard", Description = "物联网卡。SIM卡信息管理", ConnName = "Platform", DbType = DatabaseType.None)]
public partial class SimCard : ISimCard, IEntity<ISimCard>
{
    #region 属性
    private Int32 _Id;
    /// <summary>编号</summary>
    [DisplayName("编号")]
    [Description("编号")]
    [DataObjectField(true, true, false, 0)]
    [BindColumn("Id", "编号", "")]
    public Int32 Id { get => _Id; set { if (OnPropertyChanging("Id", value)) { _Id = value; OnPropertyChanged("Id"); } } }

    private String _Mobile = null!;
    /// <summary>手机号</summary>
    [DisplayName("手机号")]
    [Description("手机号")]
    [DataObjectField(false, false, false, 20)]
    [BindColumn("Mobile", "手机号", "", Master = true)]
    public String Mobile { get => _Mobile; set { if (OnPropertyChanging("Mobile", value)) { _Mobile = value; OnPropertyChanged("Mobile"); } } }

    private String? _Operator;
    /// <summary>运营商</summary>
    [DisplayName("运营商")]
    [Description("运营商")]
    [DataObjectField(false, false, true, 20)]
    [BindColumn("Operator", "运营商", "")]
    public String? Operator { get => _Operator; set { if (OnPropertyChanging("Operator", value)) { _Operator = value; OnPropertyChanged("Operator"); } } }

    private Boolean _Enable;
    /// <summary>启用</summary>
    [DisplayName("启用")]
    [Description("启用")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Enable", "启用", "", DefaultValue = "True")]
    public Boolean Enable { get => _Enable; set { if (OnPropertyChanging("Enable", value)) { _Enable = value; OnPropertyChanged("Enable"); } } }

    private DateTime _ExpireTime;
    /// <summary>到期时间</summary>
    [DisplayName("到期时间")]
    [Description("到期时间")]
    [DataObjectField(false, false, true, 0)]
    [BindColumn("ExpireTime", "到期时间", "")]
    public DateTime ExpireTime { get => _ExpireTime; set { if (OnPropertyChanging("ExpireTime", value)) { _ExpireTime = value; OnPropertyChanged("ExpireTime"); } } }

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
    public void Copy(ISimCard model)
    {
        Id = model.Id;
        Mobile = model.Mobile;
        Operator = model.Operator;
        Enable = model.Enable;
        ExpireTime = model.ExpireTime;
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
            "Mobile" => _Mobile,
            "Operator" => _Operator,
            "Enable" => _Enable,
            "ExpireTime" => _ExpireTime,
            "CreateTime" => _CreateTime,
            "UpdateTime" => _UpdateTime,
            _ => base[name]
        };
        set
        {
            switch (name)
            {
                case "Id": _Id = value.ToInt(); break;
                case "Mobile": _Mobile = Convert.ToString(value); break;
                case "Operator": _Operator = Convert.ToString(value); break;
                case "Enable": _Enable = value.ToBoolean(); break;
                case "ExpireTime": _ExpireTime = value.ToDateTime(); break;
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
    public static SimCard? FindById(Int32 id)
    {
        if (id < 0) return null;

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Id == id);

        // 单对象缓存
        return Meta.SingleCache[id];

        //return Find(_.Id == id);
    }

    /// <summary>根据手机号查找</summary>
    /// <param name="mobile">手机号</param>
    /// <returns>实体对象</returns>
    public static SimCard? FindByMobile(String mobile)
    {
        if (mobile.IsNullOrEmpty()) return null;

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Mobile.EqualIgnoreCase(mobile));

        // 单对象缓存
        return Meta.SingleCache.GetItemWithSlaveKey(mobile) as SimCard;

        //return Find(_.Mobile == mobile);
    }
    #endregion

    #region 高级查询
    /// <summary>高级查询</summary>
    /// <param name="enable">启用</param>
    /// <param name="start">更新时间开始</param>
    /// <param name="end">更新时间结束</param>
    /// <param name="key">关键字</param>
    /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
    /// <returns>实体列表</returns>
    public static IList<SimCard> Search(Boolean? enable, DateTime start, DateTime end, String key, PageParameter page)
    {
        var exp = new WhereExpression();

        if (enable != null) exp &= _.Enable == enable;
        exp &= _.UpdateTime.Between(start, end);
        if (!key.IsNullOrEmpty()) exp &= SearchWhereByKeys(key);

        return FindAll(exp, page);
    }
    #endregion

    #region 字段名
    /// <summary>取得物联网卡字段信息的快捷方式</summary>
    public partial class _
    {
        /// <summary>编号</summary>
        public static readonly Field Id = FindByName("Id");

        /// <summary>手机号</summary>
        public static readonly Field Mobile = FindByName("Mobile");

        /// <summary>运营商</summary>
        public static readonly Field Operator = FindByName("Operator");

        /// <summary>启用</summary>
        public static readonly Field Enable = FindByName("Enable");

        /// <summary>到期时间</summary>
        public static readonly Field ExpireTime = FindByName("ExpireTime");

        /// <summary>创建时间</summary>
        public static readonly Field CreateTime = FindByName("CreateTime");

        /// <summary>更新时间</summary>
        public static readonly Field UpdateTime = FindByName("UpdateTime");

        static Field FindByName(String name) => Meta.Table.FindByName(name)!;
    }

    /// <summary>取得物联网卡字段名称的快捷方式</summary>
    public partial class __
    {
        /// <summary>编号</summary>
        public const String Id = "Id";

        /// <summary>手机号</summary>
        public const String Mobile = "Mobile";

        /// <summary>运营商</summary>
        public const String Operator = "Operator";

        /// <summary>启用</summary>
        public const String Enable = "Enable";

        /// <summary>到期时间</summary>
        public const String ExpireTime = "ExpireTime";

        /// <summary>创建时间</summary>
        public const String CreateTime = "CreateTime";

        /// <summary>更新时间</summary>
        public const String UpdateTime = "UpdateTime";
    }
    #endregion
}
