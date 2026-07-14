using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data.Safety;

/// <summary>媒体资源。主动安全多媒体资源索引</summary>
public partial interface IMediaResource
{
    #region 属性
    /// <summary>编号</summary>
    Int64 Id { get; set; }

    /// <summary>设备编号</summary>
    Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    String? Mobile { get; set; }

    /// <summary>报警时间</summary>
    DateTime AlarmTime { get; set; }

    /// <summary>媒体类型</summary>
    Int32 MediaType { get; set; }

    /// <summary>资源地址</summary>
    String? Url { get; set; }

    /// <summary>创建时间</summary>
    DateTime CreateTime { get; set; }
    #endregion
}
