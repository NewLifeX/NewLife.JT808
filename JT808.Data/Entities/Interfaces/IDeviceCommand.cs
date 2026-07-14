using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data;

/// <summary>指令记录。平台下发给终端的指令</summary>
public partial interface IDeviceCommand
{
    #region 属性
    /// <summary>编号</summary>
    Int64 Id { get; set; }

    /// <summary>设备编号</summary>
    Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    String? Mobile { get; set; }

    /// <summary>指令类型</summary>
    Int32 Kind { get; set; }

    /// <summary>流水号</summary>
    UInt16 Sequence { get; set; }

    /// <summary>状态。0-待下发 1-已下发 2-已应答 3-超时</summary>
    Int32 Status { get; set; }

    /// <summary>结果</summary>
    String? Result { get; set; }

    /// <summary>消息体类型</summary>
    String? BodyType { get; set; }

    /// <summary>消息体数据</summary>
    String? BodyData { get; set; }

    /// <summary>创建时间</summary>
    DateTime CreateTime { get; set; }

    /// <summary>更新时间</summary>
    DateTime UpdateTime { get; set; }
    #endregion
}
