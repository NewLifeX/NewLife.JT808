using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data.Platform;

/// <summary>物联网卡。SIM卡信息管理</summary>
public partial interface ISimCard
{
    #region 属性
    /// <summary>编号</summary>
    Int32 Id { get; set; }

    /// <summary>手机号</summary>
    String Mobile { get; set; }

    /// <summary>运营商</summary>
    String? Operator { get; set; }

    /// <summary>启用</summary>
    Boolean Enable { get; set; }

    /// <summary>到期时间</summary>
    DateTime ExpireTime { get; set; }

    /// <summary>创建时间</summary>
    DateTime CreateTime { get; set; }

    /// <summary>更新时间</summary>
    DateTime UpdateTime { get; set; }
    #endregion
}
