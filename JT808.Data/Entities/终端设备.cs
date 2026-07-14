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

/// <summary>终端设备。JT808终端设备信息</summary>
[Serializable]
[DataObject]
[Description("终端设备。JT808终端设备信息")]
[BindIndex("IU_Device_Mobile", true, "Mobile")]
[BindIndex("IU_Device_Code", true, "Code")]
[BindIndex("IX_Device_PlateNo", false, "PlateNo")]
[BindIndex("IX_Device_ProductId", false, "ProductId")]
[BindTable("Device", Description = "终端设备。JT808终端设备信息", ConnName = "GPS", DbType = DatabaseType.None)]
public partial class Device : IDevice, IEntity<IDevice>
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

    private String? _PlateNo;
    /// <summary>车牌号</summary>
    [DisplayName("车牌号")]
    [Description("车牌号")]
    [DataObjectField(false, false, true, 20)]
    [BindColumn("PlateNo", "车牌号", "")]
    public String? PlateNo { get => _PlateNo; set { if (OnPropertyChanging("PlateNo", value)) { _PlateNo = value; OnPropertyChanged("PlateNo"); } } }

    private Int32 _PlateColor;
    /// <summary>车牌颜色</summary>
    [DisplayName("车牌颜色")]
    [Description("车牌颜色")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("PlateColor", "车牌颜色", "")]
    public Int32 PlateColor { get => _PlateColor; set { if (OnPropertyChanging("PlateColor", value)) { _PlateColor = value; OnPropertyChanged("PlateColor"); } } }

    private Int32 _ProductId;
    /// <summary>产品编号</summary>
    [DisplayName("产品编号")]
    [Description("产品编号")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("ProductId", "产品编号", "")]
    public Int32 ProductId { get => _ProductId; set { if (OnPropertyChanging("ProductId", value)) { _ProductId = value; OnPropertyChanged("ProductId"); } } }

    private Int32 _GroupId;
    /// <summary>分组编号</summary>
    [DisplayName("分组编号")]
    [Description("分组编号")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("GroupId", "分组编号", "")]
    public Int32 GroupId { get => _GroupId; set { if (OnPropertyChanging("GroupId", value)) { _GroupId = value; OnPropertyChanged("GroupId"); } } }

    private Int16 _ProvinceId;
    /// <summary>省域ID</summary>
    [DisplayName("省域ID")]
    [Description("省域ID")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("ProvinceId", "省域ID", "")]
    public Int16 ProvinceId { get => _ProvinceId; set { if (OnPropertyChanging("ProvinceId", value)) { _ProvinceId = value; OnPropertyChanged("ProvinceId"); } } }

    private Int16 _CityId;
    /// <summary>市县域ID</summary>
    [DisplayName("市县域ID")]
    [Description("市县域ID")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("CityId", "市县域ID", "")]
    public Int16 CityId { get => _CityId; set { if (OnPropertyChanging("CityId", value)) { _CityId = value; OnPropertyChanged("CityId"); } } }

    private String? _ManufacturerId;
    /// <summary>制造商ID</summary>
    [DisplayName("制造商ID")]
    [Description("制造商ID")]
    [DataObjectField(false, false, true, 11)]
    [BindColumn("ManufacturerId", "制造商ID", "")]
    public String? ManufacturerId { get => _ManufacturerId; set { if (OnPropertyChanging("ManufacturerId", value)) { _ManufacturerId = value; OnPropertyChanged("ManufacturerId"); } } }

    private String? _DeviceType;
    /// <summary>终端型号</summary>
    [DisplayName("终端型号")]
    [Description("终端型号")]
    [DataObjectField(false, false, true, 30)]
    [BindColumn("DeviceType", "终端型号", "")]
    public String? DeviceType { get => _DeviceType; set { if (OnPropertyChanging("DeviceType", value)) { _DeviceType = value; OnPropertyChanged("DeviceType"); } } }

    private String? _DeviceId;
    /// <summary>终端ID</summary>
    [DisplayName("终端ID")]
    [Description("终端ID")]
    [DataObjectField(false, false, true, 30)]
    [BindColumn("DeviceId", "终端ID", "")]
    public String? DeviceId { get => _DeviceId; set { if (OnPropertyChanging("DeviceId", value)) { _DeviceId = value; OnPropertyChanged("DeviceId"); } } }

    private String? _Version;
    /// <summary>协议版本</summary>
    [DisplayName("协议版本")]
    [Description("协议版本")]
    [DataObjectField(false, false, true, 10)]
    [BindColumn("Version", "协议版本", "")]
    public String? Version { get => _Version; set { if (OnPropertyChanging("Version", value)) { _Version = value; OnPropertyChanged("Version"); } } }

    private String? _Code;
    /// <summary>鉴权码</summary>
    [DisplayName("鉴权码")]
    [Description("鉴权码")]
    [DataObjectField(false, false, true, 32)]
    [BindColumn("Code", "鉴权码", "")]
    public String? Code { get => _Code; set { if (OnPropertyChanging("Code", value)) { _Code = value; OnPropertyChanged("Code"); } } }

    private Boolean _Enable;
    /// <summary>启用</summary>
    [DisplayName("启用")]
    [Description("启用")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Enable", "启用", "", DefaultValue = "True")]
    public Boolean Enable { get => _Enable; set { if (OnPropertyChanging("Enable", value)) { _Enable = value; OnPropertyChanged("Enable"); } } }

    private String? _Remark;
    /// <summary>备注</summary>
    [DisplayName("备注")]
    [Description("备注")]
    [DataObjectField(false, false, true, 200)]
    [BindColumn("Remark", "备注", "")]
    public String? Remark { get => _Remark; set { if (OnPropertyChanging("Remark", value)) { _Remark = value; OnPropertyChanged("Remark"); } } }

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

    private String? _CreateUser;
    /// <summary>创建者</summary>
    [DisplayName("创建者")]
    [Description("创建者")]
    [DataObjectField(false, false, true, 50)]
    [BindColumn("CreateUser", "创建者", "")]
    public String? CreateUser { get => _CreateUser; set { if (OnPropertyChanging("CreateUser", value)) { _CreateUser = value; OnPropertyChanged("CreateUser"); } } }
    #endregion

    #region 拷贝
    /// <summary>拷贝模型对象</summary>
    /// <param name="model">模型</param>
    public void Copy(IDevice model)
    {
        Id = model.Id;
        Mobile = model.Mobile;
        PlateNo = model.PlateNo;
        PlateColor = model.PlateColor;
        ProductId = model.ProductId;
        GroupId = model.GroupId;
        ProvinceId = model.ProvinceId;
        CityId = model.CityId;
        ManufacturerId = model.ManufacturerId;
        DeviceType = model.DeviceType;
        DeviceId = model.DeviceId;
        Version = model.Version;
        Code = model.Code;
        Enable = model.Enable;
        Remark = model.Remark;
        CreateTime = model.CreateTime;
        UpdateTime = model.UpdateTime;
        CreateUser = model.CreateUser;
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
            "PlateNo" => _PlateNo,
            "PlateColor" => _PlateColor,
            "ProductId" => _ProductId,
            "GroupId" => _GroupId,
            "ProvinceId" => _ProvinceId,
            "CityId" => _CityId,
            "ManufacturerId" => _ManufacturerId,
            "DeviceType" => _DeviceType,
            "DeviceId" => _DeviceId,
            "Version" => _Version,
            "Code" => _Code,
            "Enable" => _Enable,
            "Remark" => _Remark,
            "CreateTime" => _CreateTime,
            "UpdateTime" => _UpdateTime,
            "CreateUser" => _CreateUser,
            _ => base[name]
        };
        set
        {
            switch (name)
            {
                case "Id": _Id = value.ToInt(); break;
                case "Mobile": _Mobile = Convert.ToString(value); break;
                case "PlateNo": _PlateNo = Convert.ToString(value); break;
                case "PlateColor": _PlateColor = value.ToInt(); break;
                case "ProductId": _ProductId = value.ToInt(); break;
                case "GroupId": _GroupId = value.ToInt(); break;
                case "ProvinceId": _ProvinceId = Convert.ToInt16(value); break;
                case "CityId": _CityId = Convert.ToInt16(value); break;
                case "ManufacturerId": _ManufacturerId = Convert.ToString(value); break;
                case "DeviceType": _DeviceType = Convert.ToString(value); break;
                case "DeviceId": _DeviceId = Convert.ToString(value); break;
                case "Version": _Version = Convert.ToString(value); break;
                case "Code": _Code = Convert.ToString(value); break;
                case "Enable": _Enable = value.ToBoolean(); break;
                case "Remark": _Remark = Convert.ToString(value); break;
                case "CreateTime": _CreateTime = value.ToDateTime(); break;
                case "UpdateTime": _UpdateTime = value.ToDateTime(); break;
                case "CreateUser": _CreateUser = Convert.ToString(value); break;
                default: base[name] = value; break;
            }
        }
    }
    #endregion

    #region 关联映射
    /// <summary>产品编号</summary>
    [XmlIgnore, IgnoreDataMember, ScriptIgnore]
    public Product? Product => Extends.Get(nameof(Product), k => Product.FindById(ProductId));

    /// <summary>产品编号</summary>
    [Map(nameof(ProductId), typeof(Product), "Id")]
    public String? ProductName => Product?.Name;

    #endregion

    #region 扩展查询
    /// <summary>根据编号查找</summary>
    /// <param name="id">编号</param>
    /// <returns>实体对象</returns>
    public static Device? FindById(Int32 id)
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
    public static Device? FindByMobile(String mobile)
    {
        if (mobile.IsNullOrEmpty()) return null;

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Mobile.EqualIgnoreCase(mobile));

        // 单对象缓存
        return Meta.SingleCache.GetItemWithSlaveKey(mobile) as Device;

        //return Find(_.Mobile == mobile);
    }

    /// <summary>根据鉴权码查找</summary>
    /// <param name="code">鉴权码</param>
    /// <returns>实体对象</returns>
    public static Device? FindByCode(String? code)
    {
        if (code == null) return null;

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Code.EqualIgnoreCase(code));

        return Find(_.Code == code);
    }

    /// <summary>根据车牌号查找</summary>
    /// <param name="plateNo">车牌号</param>
    /// <returns>实体列表</returns>
    public static IList<Device> FindAllByPlateNo(String? plateNo)
    {
        if (plateNo == null) return [];

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.FindAll(e => e.PlateNo.EqualIgnoreCase(plateNo));

        return FindAll(_.PlateNo == plateNo);
    }

    /// <summary>根据产品编号查找</summary>
    /// <param name="productId">产品编号</param>
    /// <returns>实体列表</returns>
    public static IList<Device> FindAllByProductId(Int32 productId)
    {
        if (productId < 0) return [];

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.FindAll(e => e.ProductId == productId);

        return FindAll(_.ProductId == productId);
    }
    #endregion

    #region 高级查询
    /// <summary>高级查询</summary>
    /// <param name="plateNo">车牌号</param>
    /// <param name="productId">产品编号</param>
    /// <param name="code">鉴权码</param>
    /// <param name="enable">启用</param>
    /// <param name="start">更新时间开始</param>
    /// <param name="end">更新时间结束</param>
    /// <param name="key">关键字</param>
    /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
    /// <returns>实体列表</returns>
    public static IList<Device> Search(String? plateNo, Int32 productId, String? code, Boolean? enable, DateTime start, DateTime end, String key, PageParameter page)
    {
        var exp = new WhereExpression();

        if (!plateNo.IsNullOrEmpty()) exp &= _.PlateNo == plateNo;
        if (productId >= 0) exp &= _.ProductId == productId;
        if (!code.IsNullOrEmpty()) exp &= _.Code == code;
        if (enable != null) exp &= _.Enable == enable;
        exp &= _.UpdateTime.Between(start, end);
        if (!key.IsNullOrEmpty()) exp &= SearchWhereByKeys(key);

        return FindAll(exp, page);
    }
    #endregion

    #region 字段名
    /// <summary>取得终端设备字段信息的快捷方式</summary>
    public partial class _
    {
        /// <summary>编号</summary>
        public static readonly Field Id = FindByName("Id");

        /// <summary>手机号</summary>
        public static readonly Field Mobile = FindByName("Mobile");

        /// <summary>车牌号</summary>
        public static readonly Field PlateNo = FindByName("PlateNo");

        /// <summary>车牌颜色</summary>
        public static readonly Field PlateColor = FindByName("PlateColor");

        /// <summary>产品编号</summary>
        public static readonly Field ProductId = FindByName("ProductId");

        /// <summary>分组编号</summary>
        public static readonly Field GroupId = FindByName("GroupId");

        /// <summary>省域ID</summary>
        public static readonly Field ProvinceId = FindByName("ProvinceId");

        /// <summary>市县域ID</summary>
        public static readonly Field CityId = FindByName("CityId");

        /// <summary>制造商ID</summary>
        public static readonly Field ManufacturerId = FindByName("ManufacturerId");

        /// <summary>终端型号</summary>
        public static readonly Field DeviceType = FindByName("DeviceType");

        /// <summary>终端ID</summary>
        public static readonly Field DeviceId = FindByName("DeviceId");

        /// <summary>协议版本</summary>
        public static readonly Field Version = FindByName("Version");

        /// <summary>鉴权码</summary>
        public static readonly Field Code = FindByName("Code");

        /// <summary>启用</summary>
        public static readonly Field Enable = FindByName("Enable");

        /// <summary>备注</summary>
        public static readonly Field Remark = FindByName("Remark");

        /// <summary>创建时间</summary>
        public static readonly Field CreateTime = FindByName("CreateTime");

        /// <summary>更新时间</summary>
        public static readonly Field UpdateTime = FindByName("UpdateTime");

        /// <summary>创建者</summary>
        public static readonly Field CreateUser = FindByName("CreateUser");

        static Field FindByName(String name) => Meta.Table.FindByName(name)!;
    }

    /// <summary>取得终端设备字段名称的快捷方式</summary>
    public partial class __
    {
        /// <summary>编号</summary>
        public const String Id = "Id";

        /// <summary>手机号</summary>
        public const String Mobile = "Mobile";

        /// <summary>车牌号</summary>
        public const String PlateNo = "PlateNo";

        /// <summary>车牌颜色</summary>
        public const String PlateColor = "PlateColor";

        /// <summary>产品编号</summary>
        public const String ProductId = "ProductId";

        /// <summary>分组编号</summary>
        public const String GroupId = "GroupId";

        /// <summary>省域ID</summary>
        public const String ProvinceId = "ProvinceId";

        /// <summary>市县域ID</summary>
        public const String CityId = "CityId";

        /// <summary>制造商ID</summary>
        public const String ManufacturerId = "ManufacturerId";

        /// <summary>终端型号</summary>
        public const String DeviceType = "DeviceType";

        /// <summary>终端ID</summary>
        public const String DeviceId = "DeviceId";

        /// <summary>协议版本</summary>
        public const String Version = "Version";

        /// <summary>鉴权码</summary>
        public const String Code = "Code";

        /// <summary>启用</summary>
        public const String Enable = "Enable";

        /// <summary>备注</summary>
        public const String Remark = "Remark";

        /// <summary>创建时间</summary>
        public const String CreateTime = "CreateTime";

        /// <summary>更新时间</summary>
        public const String UpdateTime = "UpdateTime";

        /// <summary>创建者</summary>
        public const String CreateUser = "CreateUser";
    }
    #endregion
}
