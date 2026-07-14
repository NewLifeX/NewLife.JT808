using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace JT808.Data.Platform;

/// <summary>监管服务。JT809监管平台对接配置</summary>
public partial interface ISuperviseServer
{
    #region 属性
    /// <summary>编号</summary>
    Int32 Id { get; set; }

    /// <summary>名称</summary>
    String Name { get; set; }

    /// <summary>主服务地址</summary>
    String? Server { get; set; }

    /// <summary>备服务地址</summary>
    String? BackupServer { get; set; }

    /// <summary>端口</summary>
    Int32 Port { get; set; }

    /// <summary>省域</summary>
    String? Province { get; set; }

    /// <summary>启用</summary>
    Boolean Enable { get; set; }

    /// <summary>创建时间</summary>
    DateTime CreateTime { get; set; }

    /// <summary>更新时间</summary>
    DateTime UpdateTime { get; set; }
    #endregion
}
