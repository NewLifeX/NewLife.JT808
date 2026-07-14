using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using NewLife;
using NewLife.Data;
using NewLife.Reflection;

namespace JT808.Data;

/// <summary>多媒体信息。终端多媒体文件信息</summary>
public partial class MediaInfoModel : IModel
{
    #region 属性
    /// <summary>编号</summary>
    public Int64 Id { get; set; }

    /// <summary>设备编号</summary>
    public Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    public String? Mobile { get; set; }

    /// <summary>媒体类型。0-图像 1-音频 2-视频</summary>
    public Int32 MediaType { get; set; }

    /// <summary>格式编码</summary>
    public Int32 Format { get; set; }

    /// <summary>文件大小</summary>
    public Int32 FileSize { get; set; }

    /// <summary>文件路径</summary>
    public String? FilePath { get; set; }

    /// <summary>创建时间</summary>
    public DateTime CreateTime { get; set; }
    #endregion

    #region 获取/设置 字段值
    /// <summary>获取/设置 字段值</summary>
    /// <param name="name">字段名</param>
    /// <returns></returns>
    public virtual Object? this[String name]
    {
        get
        {
            return name switch
            {
                "Id" => Id,
                "DeviceId" => DeviceId,
                "Mobile" => Mobile,
                "MediaType" => MediaType,
                "Format" => Format,
                "FileSize" => FileSize,
                "FilePath" => FilePath,
                "CreateTime" => CreateTime,
                _ => this.GetValue(name, false),
            };
        }
        set
        {
            switch (name)
            {
                case "Id": Id = value.ToLong(); break;
                case "DeviceId": DeviceId = value.ToInt(); break;
                case "Mobile": Mobile = Convert.ToString(value); break;
                case "MediaType": MediaType = value.ToInt(); break;
                case "Format": Format = value.ToInt(); break;
                case "FileSize": FileSize = value.ToInt(); break;
                case "FilePath": FilePath = Convert.ToString(value); break;
                case "CreateTime": CreateTime = value.ToDateTime(); break;
                default: this.SetValue(name, value); break;
            }
        }
    }
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
}
