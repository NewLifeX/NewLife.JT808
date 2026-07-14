using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using NewLife;
using NewLife.Data;
using NewLife.Reflection;

namespace JT808.Data.Safety;

/// <summary>报警附件。报警关联的图片/视频/音频文件</summary>
public partial class AlarmFileModel : IModel
{
    #region 属性
    /// <summary>编号</summary>
    public Int64 Id { get; set; }

    /// <summary>设备编号</summary>
    public Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    public String? Mobile { get; set; }

    /// <summary>报警事件编号</summary>
    public Int64 EventId { get; set; }

    /// <summary>文件类型。image/video/audio</summary>
    public String? FileType { get; set; }

    /// <summary>文件名</summary>
    public String? FileName { get; set; }

    /// <summary>文件路径</summary>
    public String? FilePath { get; set; }

    /// <summary>文件大小</summary>
    public Int32 FileSize { get; set; }

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
                "EventId" => EventId,
                "FileType" => FileType,
                "FileName" => FileName,
                "FilePath" => FilePath,
                "FileSize" => FileSize,
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
                case "EventId": EventId = value.ToLong(); break;
                case "FileType": FileType = Convert.ToString(value); break;
                case "FileName": FileName = Convert.ToString(value); break;
                case "FilePath": FilePath = Convert.ToString(value); break;
                case "FileSize": FileSize = value.ToInt(); break;
                case "CreateTime": CreateTime = value.ToDateTime(); break;
                default: this.SetValue(name, value); break;
            }
        }
    }
    #endregion

    #region 拷贝
    /// <summary>拷贝模型对象</summary>
    /// <param name="model">模型</param>
    public void Copy(IAlarmFile model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        Mobile = model.Mobile;
        EventId = model.EventId;
        FileType = model.FileType;
        FileName = model.FileName;
        FilePath = model.FilePath;
        FileSize = model.FileSize;
        CreateTime = model.CreateTime;
    }
    #endregion
}
