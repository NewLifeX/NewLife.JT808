using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data.Platform;

/// <summary>车辆清单。平台接入车辆白名单</summary>
public partial interface ICarList
{
    #region 属性
    /// <summary>编号</summary>
    Int32 Id { get; set; }

    /// <summary>手机号</summary>
    String Mobile { get; set; }

    /// <summary>车牌号</summary>
    String? PlateNo { get; set; }

    /// <summary>车牌颜色</summary>
    Int32 PlateColor { get; set; }

    /// <summary>启用</summary>
    Boolean Enable { get; set; }

    /// <summary>创建时间</summary>
    DateTime CreateTime { get; set; }

    /// <summary>更新时间</summary>
    DateTime UpdateTime { get; set; }
    #endregion
}
