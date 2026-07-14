using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data.Safety;

/// <summary>DSM报警。驾驶员状态监测报警</summary>
public partial interface IDSMAlarm
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

    /// <summary>报警类型。1-疲劳驾驶 2-打电话 3-抽烟 4-分神 5-探头遮挡</summary>
    Int32 AlarmType { get; set; }

    /// <summary>报警级别</summary>
    Int32 Level { get; set; }

    /// <summary>速度</summary>
    Int16 Speed { get; set; }

    /// <summary>纬度</summary>
    Int32 Latitude { get; set; }

    /// <summary>经度</summary>
    Int32 Longitude { get; set; }

    /// <summary>报警时间</summary>
    DateTime AlarmTime { get; set; }

    /// <summary>创建时间</summary>
    DateTime CreateTime { get; set; }
    #endregion
}
