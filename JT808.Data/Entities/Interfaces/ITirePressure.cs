using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data;

/// <summary>胎压数据。轮胎压力和温度监测</summary>
public partial interface ITirePressure
{
    #region 属性
    /// <summary>编号</summary>
    Int64 Id { get; set; }

    /// <summary>设备编号</summary>
    Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    String? Mobile { get; set; }

    /// <summary>轮胎编号</summary>
    Int32 TireNo { get; set; }

    /// <summary>胎压(Pa)</summary>
    Double Pressure { get; set; }

    /// <summary>温度(℃)</summary>
    Int16 Temperature { get; set; }

    /// <summary>报警状态</summary>
    Boolean Alarm { get; set; }

    /// <summary>创建时间</summary>
    DateTime CreateTime { get; set; }
    #endregion
}
