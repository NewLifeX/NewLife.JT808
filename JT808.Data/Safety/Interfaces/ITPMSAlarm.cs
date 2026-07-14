using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data.Safety;

/// <summary>TPMS报警。胎压监测报警</summary>
public partial interface ITPMSAlarm
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

    /// <summary>报警类型。1-胎压低 2-胎压高 3-胎温高 4-漏气</summary>
    Int32 AlarmType { get; set; }

    /// <summary>报警级别</summary>
    Int32 Level { get; set; }

    /// <summary>轮胎编号</summary>
    Int32 TireNo { get; set; }

    /// <summary>胎压(Pa)</summary>
    Double Pressure { get; set; }

    /// <summary>温度(℃)</summary>
    Int16 Temperature { get; set; }

    /// <summary>报警时间</summary>
    DateTime AlarmTime { get; set; }

    /// <summary>创建时间</summary>
    DateTime CreateTime { get; set; }
    #endregion
}
