using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data;

/// <summary>驾驶员信息</summary>
public partial interface IDriver
{
    #region 属性
    /// <summary>编号</summary>
    Int32 Id { get; set; }

    /// <summary>设备编号</summary>
    Int32 DeviceId { get; set; }

    /// <summary>姓名</summary>
    String? Name { get; set; }

    /// <summary>身份证号</summary>
    String? IDCard { get; set; }

    /// <summary>从业资格证号</summary>
    String? Qualification { get; set; }

    /// <summary>手机号</summary>
    String? Mobile { get; set; }

    /// <summary>创建时间</summary>
    DateTime CreateTime { get; set; }

    /// <summary>更新时间</summary>
    DateTime UpdateTime { get; set; }
    #endregion
}
