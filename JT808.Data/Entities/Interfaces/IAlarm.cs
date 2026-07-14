using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data;

/// <summary>报警信息。终端上报的报警事件</summary>
public partial interface IAlarm
{
    #region 属性
    /// <summary>编号</summary>
    Int64 Id { get; set; }

    /// <summary>设备编号</summary>
    Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    String? Mobile { get; set; }

    /// <summary>报警类型</summary>
    Int32 AlarmType { get; set; }

    /// <summary>报警时间</summary>
    DateTime AlarmTime { get; set; }

    /// <summary>纬度</summary>
    Int32 Latitude { get; set; }

    /// <summary>经度</summary>
    Int32 Longitude { get; set; }

    /// <summary>速度</summary>
    Int16 Speed { get; set; }

    /// <summary>方向</summary>
    Int16 Direction { get; set; }

    /// <summary>海拔</summary>
    Int16 Altitude { get; set; }

    /// <summary>报警内容</summary>
    String? Content { get; set; }

    /// <summary>创建时间</summary>
    DateTime CreateTime { get; set; }
    #endregion
}
