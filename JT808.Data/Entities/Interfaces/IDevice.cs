using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data;

/// <summary>终端设备。JT808终端设备信息</summary>
public partial interface IDevice
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

    /// <summary>产品编号</summary>
    Int32 ProductId { get; set; }

    /// <summary>分组编号</summary>
    Int32 GroupId { get; set; }

    /// <summary>省域ID</summary>
    Int16 ProvinceId { get; set; }

    /// <summary>市县域ID</summary>
    Int16 CityId { get; set; }

    /// <summary>制造商ID</summary>
    String? ManufacturerId { get; set; }

    /// <summary>终端型号</summary>
    String? DeviceType { get; set; }

    /// <summary>终端ID</summary>
    String? DeviceId { get; set; }

    /// <summary>协议版本</summary>
    String? Version { get; set; }

    /// <summary>鉴权码</summary>
    String? Code { get; set; }

    /// <summary>启用</summary>
    Boolean Enable { get; set; }

    /// <summary>备注</summary>
    String? Remark { get; set; }

    /// <summary>创建时间</summary>
    DateTime CreateTime { get; set; }

    /// <summary>更新时间</summary>
    DateTime UpdateTime { get; set; }

    /// <summary>创建者</summary>
    String? CreateUser { get; set; }
    #endregion
}
