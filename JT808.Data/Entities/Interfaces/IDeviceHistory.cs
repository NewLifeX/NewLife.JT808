using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data;

/// <summary>设备上下线历史</summary>
public partial interface IDeviceHistory
{
    #region 属性
    /// <summary>编号</summary>
    Int64 Id { get; set; }

    /// <summary>设备编号</summary>
    Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    String? Mobile { get; set; }

    /// <summary>动作。上线/下线</summary>
    String? Action { get; set; }

    /// <summary>备注</summary>
    String? Remark { get; set; }

    /// <summary>创建时间</summary>
    DateTime CreateTime { get; set; }
    #endregion
}
