using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data;

/// <summary>原始报文。终端上传的原始数据</summary>
public partial interface IRawData
{
    #region 属性
    /// <summary>编号</summary>
    Int64 Id { get; set; }

    /// <summary>设备编号</summary>
    Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    String? Mobile { get; set; }

    /// <summary>消息类型</summary>
    UInt16 Kind { get; set; }

    /// <summary>十六进制数据</summary>
    String? HexData { get; set; }

    /// <summary>备注</summary>
    String? Remark { get; set; }

    /// <summary>创建时间</summary>
    DateTime CreateTime { get; set; }
    #endregion
}
