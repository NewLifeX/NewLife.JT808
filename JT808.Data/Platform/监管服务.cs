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

/// <summary>监管服务。JT809监管平台对接配置</summary>
[Serializable]
[DataObject]
[Description("监管服务。JT809监管平台对接配置")]
[BindTable("SuperviseServer", Description = "监管服务。JT809监管平台对接配置", ConnName = "Platform", DbType = DatabaseType.None)]
public partial class SuperviseServer : ISuperviseServer, IEntity<ISuperviseServer>
{
    #region 属性
    private Int32 _Id;
    /// <summary>编号</summary>
    [DisplayName("编号")]
    [Description("编号")]
    [DataObjectField(true, true, false, 0)]
    [BindColumn("Id", "编号", "")]
    public Int32 Id { get => _Id; set { if (OnPropertyChanging("Id", value)) { _Id = value; OnPropertyChanged("Id"); } } }

    private String _Name = null!;
    /// <summary>名称</summary>
    [DisplayName("名称")]
    [Description("名称")]
    [DataObjectField(false, false, false, 50)]
    [BindColumn("Name", "名称", "", Master = true)]
    public String Name { get => _Name; set { if (OnPropertyChanging("Name", value)) { _Name = value; OnPropertyChanged("Name"); } } }

    private String? _Server;
    /// <summary>主服务地址</summary>
    [DisplayName("主服务地址")]
    [Description("主服务地址")]
    [DataObjectField(false, false, true, 200)]
    [BindColumn("Server", "主服务地址", "")]
    public String? Server { get => _Server; set { if (OnPropertyChanging("Server", value)) { _Server = value; OnPropertyChanged("Server"); } } }

    private String? _BackupServer;
    /// <summary>备服务地址</summary>
    [DisplayName("备服务地址")]
    [Description("备服务地址")]
    [DataObjectField(false, false, true, 200)]
    [BindColumn("BackupServer", "备服务地址", "")]
    public String? BackupServer { get => _BackupServer; set { if (OnPropertyChanging("BackupServer", value)) { _BackupServer = value; OnPropertyChanged("BackupServer"); } } }

    private Int32 _Port;
    /// <summary>端口</summary>
    [DisplayName("端口")]
    [Description("端口")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Port", "端口", "")]
    public Int32 Port { get => _Port; set { if (OnPropertyChanging("Port", value)) { _Port = value; OnPropertyChanged("Port"); } } }

    private String? _Province;
    /// <summary>省域</summary>
    [DisplayName("省域")]
    [Description("省域")]
    [DataObjectField(false, false, true, 20)]
    [BindColumn("Province", "省域", "")]
    public String? Province { get => _Province; set { if (OnPropertyChanging("Province", value)) { _Province = value; OnPropertyChanged("Province"); } } }

    private Boolean _Enable;
    /// <summary>启用</summary>
    [DisplayName("启用")]
    [Description("启用")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Enable", "启用", "", DefaultValue = "True")]
    public Boolean Enable { get => _Enable; set { if (OnPropertyChanging("Enable", value)) { _Enable = value; OnPropertyChanged("Enable"); } } }

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
    public void Copy(ISuperviseServer model)
    {
        Id = model.Id;
        Name = model.Name;
        Server = model.Server;
        BackupServer = model.BackupServer;
        Port = model.Port;
        Province = model.Province;
        Enable = model.Enable;
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
            "Name" => _Name,
            "Server" => _Server,
            "BackupServer" => _BackupServer,
            "Port" => _Port,
            "Province" => _Province,
            "Enable" => _Enable,
            "CreateTime" => _CreateTime,
            "UpdateTime" => _UpdateTime,
            _ => base[name]
        };
        set
        {
            switch (name)
            {
                case "Id": _Id = value.ToInt(); break;
                case "Name": _Name = Convert.ToString(value); break;
                case "Server": _Server = Convert.ToString(value); break;
                case "BackupServer": _BackupServer = Convert.ToString(value); break;
                case "Port": _Port = value.ToInt(); break;
                case "Province": _Province = Convert.ToString(value); break;
                case "Enable": _Enable = value.ToBoolean(); break;
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
    public static SuperviseServer? FindById(Int32 id)
    {
        if (id < 0) return null;

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Id == id);

        // 单对象缓存
        return Meta.SingleCache[id];

        //return Find(_.Id == id);
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
    public static IList<SuperviseServer> Search(Boolean? enable, DateTime start, DateTime end, String key, PageParameter page)
    {
        var exp = new WhereExpression();

        if (enable != null) exp &= _.Enable == enable;
        exp &= _.UpdateTime.Between(start, end);
        if (!key.IsNullOrEmpty()) exp &= SearchWhereByKeys(key);

        return FindAll(exp, page);
    }
    #endregion

    #region 字段名
    /// <summary>取得监管服务字段信息的快捷方式</summary>
    public partial class _
    {
        /// <summary>编号</summary>
        public static readonly Field Id = FindByName("Id");

        /// <summary>名称</summary>
        public static readonly Field Name = FindByName("Name");

        /// <summary>主服务地址</summary>
        public static readonly Field Server = FindByName("Server");

        /// <summary>备服务地址</summary>
        public static readonly Field BackupServer = FindByName("BackupServer");

        /// <summary>端口</summary>
        public static readonly Field Port = FindByName("Port");

        /// <summary>省域</summary>
        public static readonly Field Province = FindByName("Province");

        /// <summary>启用</summary>
        public static readonly Field Enable = FindByName("Enable");

        /// <summary>创建时间</summary>
        public static readonly Field CreateTime = FindByName("CreateTime");

        /// <summary>更新时间</summary>
        public static readonly Field UpdateTime = FindByName("UpdateTime");

        static Field FindByName(String name) => Meta.Table.FindByName(name)!;
    }

    /// <summary>取得监管服务字段名称的快捷方式</summary>
    public partial class __
    {
        /// <summary>编号</summary>
        public const String Id = "Id";

        /// <summary>名称</summary>
        public const String Name = "Name";

        /// <summary>主服务地址</summary>
        public const String Server = "Server";

        /// <summary>备服务地址</summary>
        public const String BackupServer = "BackupServer";

        /// <summary>端口</summary>
        public const String Port = "Port";

        /// <summary>省域</summary>
        public const String Province = "Province";

        /// <summary>启用</summary>
        public const String Enable = "Enable";

        /// <summary>创建时间</summary>
        public const String CreateTime = "CreateTime";

        /// <summary>更新时间</summary>
        public const String UpdateTime = "UpdateTime";
    }
    #endregion
}
