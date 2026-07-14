using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data;

/// <summary>位置数据。终端上报的位置信息</summary>
public partial interface IPositionData
{
    #region 属性
    /// <summary>编号</summary>
    Int64 Id { get; set; }

    /// <summary>设备编号</summary>
    Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    String? Mobile { get; set; }

    /// <summary>报警标志</summary>
    UInt32 Alarm { get; set; }

    /// <summary>状态</summary>
    UInt32 Status { get; set; }

    /// <summary>纬度</summary>
    Int32 Latitude { get; set; }

    /// <summary>经度</summary>
    Int32 Longitude { get; set; }

    /// <summary>海拔高度</summary>
    Int16 Altitude { get; set; }

    /// <summary>速度(km/h)</summary>
    Int16 Speed { get; set; }

    /// <summary>方向</summary>
    Int16 Direction { get; set; }

    /// <summary>里程(km)</summary>
    Double Mileage { get; set; }

    /// <summary>油量(L)</summary>
    Double Fuel { get; set; }

    /// <summary>百度纬度</summary>
    Double LatitudeBD { get; set; }

    /// <summary>百度经度</summary>
    Double LongitudeBD { get; set; }

    /// <summary>附加信息JSON</summary>
    String? Extras { get; set; }

    /// <summary>GPS时间</summary>
    DateTime GPSTime { get; set; }

    /// <summary>接收时间</summary>
    DateTime CreateTime { get; set; }
    #endregion
}
