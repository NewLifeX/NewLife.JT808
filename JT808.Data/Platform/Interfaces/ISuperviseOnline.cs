using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data.Platform;

/// <summary>监管在线。监管平台在线状态</summary>
public partial interface ISuperviseOnline
{
    #region 属性
    /// <summary>编号</summary>
    Int32 Id { get; set; }

    /// <summary>监管服务编号</summary>
    Int32 ServerId { get; set; }

    /// <summary>在线状态</summary>
    Boolean Online { get; set; }

    /// <summary>最后心跳时间</summary>
    DateTime HeartbeatTime { get; set; }

    /// <summary>创建时间</summary>
    DateTime CreateTime { get; set; }

    /// <summary>更新时间</summary>
    DateTime UpdateTime { get; set; }
    #endregion
}
