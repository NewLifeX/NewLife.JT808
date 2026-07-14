using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data.Safety;

/// <summary>报警事件。主动安全报警事件聚合</summary>
public partial interface IAlarmEvent
{
    #region 属性
    /// <summary>编号</summary>
    Int64 Id { get; set; }

    /// <summary>设备编号</summary>
    Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    String? Mobile { get; set; }

    /// <summary>报警类型。1-ADAS 2-DSM 3-BSD 4-TPMS</summary>
    Int32 Kind { get; set; }

    /// <summary>事件编码</summary>
    Int32 Code { get; set; }

    /// <summary>报警级别。0-紧急 1-警告</summary>
    Int32 Level { get; set; }

    /// <summary>累计次数</summary>
    Int32 Times { get; set; }

    /// <summary>标志。0-开始 1-结束 2-持续</summary>
    Int32 Flag { get; set; }

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

    /// <summary>报警时间</summary>
    DateTime AlarmTime { get; set; }

    /// <summary>创建时间</summary>
    DateTime CreateTime { get; set; }
    #endregion
}
