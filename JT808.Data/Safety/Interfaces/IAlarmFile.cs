using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data.Safety;

/// <summary>报警附件。报警关联的图片/视频/音频文件</summary>
public partial interface IAlarmFile
{
    #region 属性
    /// <summary>编号</summary>
    Int64 Id { get; set; }

    /// <summary>设备编号</summary>
    Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    String? Mobile { get; set; }

    /// <summary>报警事件编号</summary>
    Int64 EventId { get; set; }

    /// <summary>文件类型。image/video/audio</summary>
    String? FileType { get; set; }

    /// <summary>文件名</summary>
    String? FileName { get; set; }

    /// <summary>文件路径</summary>
    String? FilePath { get; set; }

    /// <summary>文件大小</summary>
    Int32 FileSize { get; set; }

    /// <summary>创建时间</summary>
    DateTime CreateTime { get; set; }
    #endregion
}
