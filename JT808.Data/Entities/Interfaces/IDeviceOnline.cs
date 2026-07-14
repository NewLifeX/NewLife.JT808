using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data;

/// <summary>设备在线。终端设备在线会话信息</summary>
public partial interface IDeviceOnline
{
    #region 属性
    /// <summary>编号</summary>
    Int32 Id { get; set; }

    /// <summary>设备编号</summary>
    Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    String? Mobile { get; set; }

    /// <summary>会话ID</summary>
    String? SessionId { get; set; }

    /// <summary>远程地址</summary>
    String? RemoteEndPoint { get; set; }

    /// <summary>纬度</summary>
    Int32 Latitude { get; set; }

    /// <summary>经度</summary>
    Int32 Longitude { get; set; }

    /// <summary>速度</summary>
    Int16 Speed { get; set; }

    /// <summary>方向</summary>
    Int16 Direction { get; set; }

    /// <summary>地址</summary>
    String? Address { get; set; }

    /// <summary>最后心跳时间</summary>
    DateTime HeartbeatTime { get; set; }

    /// <summary>在线状态</summary>
    Boolean Online { get; set; }

    /// <summary>上线时间</summary>
    DateTime CreateTime { get; set; }

    /// <summary>更新时间</summary>
    DateTime UpdateTime { get; set; }
    #endregion
}
