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

/// <summary>指令记录。平台下发给终端的指令</summary>
[Serializable]
[DataObject]
[Description("指令记录。平台下发给终端的指令")]
[BindIndex("IX_DeviceCommand_DeviceId_CreateTime", false, "DeviceId,CreateTime")]
[BindIndex("IX_DeviceCommand_Mobile_CreateTime", false, "Mobile,CreateTime")]
[BindTable("DeviceCommand", Description = "指令记录。平台下发给终端的指令", ConnName = "GPS", DbType = DatabaseType.None)]
public partial class DeviceCommand : IDeviceCommand, IEntity<IDeviceCommand>
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

    private Int32 _Kind;
    /// <summary>指令类型</summary>
    [DisplayName("指令类型")]
    [Description("指令类型")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Kind", "指令类型", "")]
    public Int32 Kind { get => _Kind; set { if (OnPropertyChanging("Kind", value)) { _Kind = value; OnPropertyChanged("Kind"); } } }

    private UInt16 _Sequence;
    /// <summary>流水号</summary>
    [DisplayName("流水号")]
    [Description("流水号")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Sequence", "流水号", "")]
    public UInt16 Sequence { get => _Sequence; set { if (OnPropertyChanging("Sequence", value)) { _Sequence = value; OnPropertyChanged("Sequence"); } } }

    private Int32 _Status;
    /// <summary>状态。0-待下发 1-已下发 2-已应答 3-超时</summary>
    [DisplayName("状态")]
    [Description("状态。0-待下发 1-已下发 2-已应答 3-超时")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Status", "状态。0-待下发 1-已下发 2-已应答 3-超时", "")]
    public Int32 Status { get => _Status; set { if (OnPropertyChanging("Status", value)) { _Status = value; OnPropertyChanged("Status"); } } }

    private String? _Result;
    /// <summary>结果</summary>
    [DisplayName("结果")]
    [Description("结果")]
    [DataObjectField(false, false, true, 200)]
    [BindColumn("Result", "结果", "")]
    public String? Result { get => _Result; set { if (OnPropertyChanging("Result", value)) { _Result = value; OnPropertyChanged("Result"); } } }

    private String? _BodyType;
    /// <summary>消息体类型</summary>
    [DisplayName("消息体类型")]
    [Description("消息体类型")]
    [DataObjectField(false, false, true, 200)]
    [BindColumn("BodyType", "消息体类型", "")]
    public String? BodyType { get => _BodyType; set { if (OnPropertyChanging("BodyType", value)) { _BodyType = value; OnPropertyChanged("BodyType"); } } }

    private String? _BodyData;
    /// <summary>消息体数据</summary>
    [DisplayName("消息体数据")]
    [Description("消息体数据")]
    [DataObjectField(false, false, true, -1)]
    [BindColumn("BodyData", "消息体数据", "")]
    public String? BodyData { get => _BodyData; set { if (OnPropertyChanging("BodyData", value)) { _BodyData = value; OnPropertyChanged("BodyData"); } } }

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
    public void Copy(IDeviceCommand model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        Mobile = model.Mobile;
        Kind = model.Kind;
        Sequence = model.Sequence;
        Status = model.Status;
        Result = model.Result;
        BodyType = model.BodyType;
        BodyData = model.BodyData;
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
            "Kind" => _Kind,
            "Sequence" => _Sequence,
            "Status" => _Status,
            "Result" => _Result,
            "BodyType" => _BodyType,
            "BodyData" => _BodyData,
            "CreateTime" => _CreateTime,
            "UpdateTime" => _UpdateTime,
            _ => base[name]
        };
        set
        {
            switch (name)
            {
                case "Id": _Id = value.ToLong(); break;
                case "DeviceId": _DeviceId = value.ToInt(); break;
                case "Mobile": _Mobile = Convert.ToString(value); break;
                case "Kind": _Kind = value.ToInt(); break;
                case "Sequence": _Sequence = Convert.ToUInt16(value); break;
                case "Status": _Status = value.ToInt(); break;
                case "Result": _Result = Convert.ToString(value); break;
                case "BodyType": _BodyType = Convert.ToString(value); break;
                case "BodyData": _BodyData = Convert.ToString(value); break;
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
    public static DeviceCommand? FindById(Int64 id)
    {
        if (id < 0) return null;

        return Find(_.Id == id);
    }
    #endregion

    #region 高级查询
    /// <summary>高级查询</summary>
    /// <param name="deviceId">设备编号</param>
    /// <param name="mobile">手机号</param>
    /// <param name="createTime">创建时间</param>
    /// <param name="start">编号开始</param>
    /// <param name="end">编号结束</param>
    /// <param name="key">关键字</param>
    /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
    /// <returns>实体列表</returns>
    public static IList<DeviceCommand> Search(Int32 deviceId, String? mobile, DateTime createTime, DateTime start, DateTime end, String key, PageParameter page)
    {
        var exp = new WhereExpression();

        if (deviceId >= 0) exp &= _.DeviceId == deviceId;
        if (!mobile.IsNullOrEmpty()) exp &= _.Mobile == mobile;
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
    /// <summary>取得指令记录字段信息的快捷方式</summary>
    public partial class _
    {
        /// <summary>编号</summary>
        public static readonly Field Id = FindByName("Id");

        /// <summary>设备编号</summary>
        public static readonly Field DeviceId = FindByName("DeviceId");

        /// <summary>手机号</summary>
        public static readonly Field Mobile = FindByName("Mobile");

        /// <summary>指令类型</summary>
        public static readonly Field Kind = FindByName("Kind");

        /// <summary>流水号</summary>
        public static readonly Field Sequence = FindByName("Sequence");

        /// <summary>状态。0-待下发 1-已下发 2-已应答 3-超时</summary>
        public static readonly Field Status = FindByName("Status");

        /// <summary>结果</summary>
        public static readonly Field Result = FindByName("Result");

        /// <summary>消息体类型</summary>
        public static readonly Field BodyType = FindByName("BodyType");

        /// <summary>消息体数据</summary>
        public static readonly Field BodyData = FindByName("BodyData");

        /// <summary>创建时间</summary>
        public static readonly Field CreateTime = FindByName("CreateTime");

        /// <summary>更新时间</summary>
        public static readonly Field UpdateTime = FindByName("UpdateTime");

        static Field FindByName(String name) => Meta.Table.FindByName(name)!;
    }

    /// <summary>取得指令记录字段名称的快捷方式</summary>
    public partial class __
    {
        /// <summary>编号</summary>
        public const String Id = "Id";

        /// <summary>设备编号</summary>
        public const String DeviceId = "DeviceId";

        /// <summary>手机号</summary>
        public const String Mobile = "Mobile";

        /// <summary>指令类型</summary>
        public const String Kind = "Kind";

        /// <summary>流水号</summary>
        public const String Sequence = "Sequence";

        /// <summary>状态。0-待下发 1-已下发 2-已应答 3-超时</summary>
        public const String Status = "Status";

        /// <summary>结果</summary>
        public const String Result = "Result";

        /// <summary>消息体类型</summary>
        public const String BodyType = "BodyType";

        /// <summary>消息体数据</summary>
        public const String BodyData = "BodyData";

        /// <summary>创建时间</summary>
        public const String CreateTime = "CreateTime";

        /// <summary>更新时间</summary>
        public const String UpdateTime = "UpdateTime";
    }
    #endregion
}
