using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data;

/// <summary>多媒体信息。终端多媒体文件信息</summary>
public partial interface IMediaInfo
{
    #region 属性
    /// <summary>编号</summary>
    Int64 Id { get; set; }

    /// <summary>设备编号</summary>
    Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    String? Mobile { get; set; }

    /// <summary>媒体类型。0-图像 1-音频 2-视频</summary>
    Int32 MediaType { get; set; }

    /// <summary>格式编码</summary>
    Int32 Format { get; set; }

    /// <summary>文件大小</summary>
    Int32 FileSize { get; set; }

    /// <summary>文件路径</summary>
    String? FilePath { get; set; }

    /// <summary>创建时间</summary>
    DateTime CreateTime { get; set; }
    #endregion
}
