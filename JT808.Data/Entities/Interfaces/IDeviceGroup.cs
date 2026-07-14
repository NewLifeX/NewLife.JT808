using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data;

/// <summary>设备分组。终端设备分组管理</summary>
public partial interface IDeviceGroup
{
    #region 属性
    /// <summary>编号</summary>
    Int32 Id { get; set; }

    /// <summary>名称</summary>
    String Name { get; set; }

    /// <summary>父级编号</summary>
    Int32 ParentId { get; set; }

    /// <summary>启用</summary>
    Boolean Enable { get; set; }

    /// <summary>创建时间</summary>
    DateTime CreateTime { get; set; }

    /// <summary>更新时间</summary>
    DateTime UpdateTime { get; set; }
    #endregion
}
