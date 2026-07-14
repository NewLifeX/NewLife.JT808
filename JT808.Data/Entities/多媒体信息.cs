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

/// <summary>多媒体信息。终端多媒体文件信息</summary>
[Serializable]
[DataObject]
[Description("多媒体信息。终端多媒体文件信息")]
[BindIndex("IX_MediaInfo_DeviceId_CreateTime", false, "DeviceId,CreateTime")]
[BindTable("MediaInfo", Description = "多媒体信息。终端多媒体文件信息", ConnName = "GPS", DbType = DatabaseType.None)]
public partial class MediaInfo : IMediaInfo, IEntity<IMediaInfo>
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

    private Int32 _MediaType;
    /// <summary>媒体类型。0-图像 1-音频 2-视频</summary>
    [DisplayName("媒体类型")]
    [Description("媒体类型。0-图像 1-音频 2-视频")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("MediaType", "媒体类型。0-图像 1-音频 2-视频", "")]
    public Int32 MediaType { get => _MediaType; set { if (OnPropertyChanging("MediaType", value)) { _MediaType = value; OnPropertyChanged("MediaType"); } } }

    private Int32 _Format;
    /// <summary>格式编码</summary>
    [DisplayName("格式编码")]
    [Description("格式编码")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("Format", "格式编码", "")]
    public Int32 Format { get => _Format; set { if (OnPropertyChanging("Format", value)) { _Format = value; OnPropertyChanged("Format"); } } }

    private Int32 _FileSize;
    /// <summary>文件大小</summary>
    [DisplayName("文件大小")]
    [Description("文件大小")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("FileSize", "文件大小", "")]
    public Int32 FileSize { get => _FileSize; set { if (OnPropertyChanging("FileSize", value)) { _FileSize = value; OnPropertyChanged("FileSize"); } } }

    private String? _FilePath;
    /// <summary>文件路径</summary>
    [DisplayName("文件路径")]
    [Description("文件路径")]
    [DataObjectField(false, false, true, 500)]
    [BindColumn("FilePath", "文件路径", "")]
    public String? FilePath { get => _FilePath; set { if (OnPropertyChanging("FilePath", value)) { _FilePath = value; OnPropertyChanged("FilePath"); } } }

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
    public void Copy(IMediaInfo model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        Mobile = model.Mobile;
        MediaType = model.MediaType;
        Format = model.Format;
        FileSize = model.FileSize;
        FilePath = model.FilePath;
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
            "MediaType" => _MediaType,
            "Format" => _Format,
            "FileSize" => _FileSize,
            "FilePath" => _FilePath,
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
                case "MediaType": _MediaType = value.ToInt(); break;
                case "Format": _Format = value.ToInt(); break;
                case "FileSize": _FileSize = value.ToInt(); break;
                case "FilePath": _FilePath = Convert.ToString(value); break;
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
    public static MediaInfo? FindById(Int64 id)
    {
        if (id < 0) return null;

        return Find(_.Id == id);
    }
    #endregion

    #region 高级查询
    /// <summary>高级查询</summary>
    /// <param name="deviceId">设备编号</param>
    /// <param name="createTime">创建时间</param>
    /// <param name="start">编号开始</param>
    /// <param name="end">编号结束</param>
    /// <param name="key">关键字</param>
    /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
    /// <returns>实体列表</returns>
    public static IList<MediaInfo> Search(Int32 deviceId, DateTime createTime, DateTime start, DateTime end, String key, PageParameter page)
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
    /// <summary>取得多媒体信息字段信息的快捷方式</summary>
    public partial class _
    {
        /// <summary>编号</summary>
        public static readonly Field Id = FindByName("Id");

        /// <summary>设备编号</summary>
        public static readonly Field DeviceId = FindByName("DeviceId");

        /// <summary>手机号</summary>
        public static readonly Field Mobile = FindByName("Mobile");

        /// <summary>媒体类型。0-图像 1-音频 2-视频</summary>
        public static readonly Field MediaType = FindByName("MediaType");

        /// <summary>格式编码</summary>
        public static readonly Field Format = FindByName("Format");

        /// <summary>文件大小</summary>
        public static readonly Field FileSize = FindByName("FileSize");

        /// <summary>文件路径</summary>
        public static readonly Field FilePath = FindByName("FilePath");

        /// <summary>创建时间</summary>
        public static readonly Field CreateTime = FindByName("CreateTime");

        static Field FindByName(String name) => Meta.Table.FindByName(name)!;
    }

    /// <summary>取得多媒体信息字段名称的快捷方式</summary>
    public partial class __
    {
        /// <summary>编号</summary>
        public const String Id = "Id";

        /// <summary>设备编号</summary>
        public const String DeviceId = "DeviceId";

        /// <summary>手机号</summary>
        public const String Mobile = "Mobile";

        /// <summary>媒体类型。0-图像 1-音频 2-视频</summary>
        public const String MediaType = "MediaType";

        /// <summary>格式编码</summary>
        public const String Format = "Format";

        /// <summary>文件大小</summary>
        public const String FileSize = "FileSize";

        /// <summary>文件路径</summary>
        public const String FilePath = "FilePath";

        /// <summary>创建时间</summary>
        public const String CreateTime = "CreateTime";
    }
    #endregion
}
