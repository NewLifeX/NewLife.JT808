using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using NewLife;
using NewLife.Data;
using NewLife.Reflection;

namespace JT808.Data.Platform;

/// <summary>监管服务。JT809监管平台对接配置</summary>
public partial class SuperviseServerModel : IModel
{
    #region 属性
    /// <summary>编号</summary>
    public Int32 Id { get; set; }

    /// <summary>名称</summary>
    public String Name { get; set; } = null!;

    /// <summary>主服务地址</summary>
    public String? Server { get; set; }

    /// <summary>备服务地址</summary>
    public String? BackupServer { get; set; }

    /// <summary>端口</summary>
    public Int32 Port { get; set; }

    /// <summary>省域</summary>
    public String? Province { get; set; }

    /// <summary>启用</summary>
    public Boolean Enable { get; set; }

    /// <summary>创建时间</summary>
    public DateTime CreateTime { get; set; }

    /// <summary>更新时间</summary>
    public DateTime UpdateTime { get; set; }
    #endregion

    #region 获取/设置 字段值
    /// <summary>获取/设置 字段值</summary>
    /// <param name="name">字段名</param>
    /// <returns></returns>
    public virtual Object? this[String name]
    {
        get
        {
            return name switch
            {
                "Id" => Id,
                "Name" => Name,
                "Server" => Server,
                "BackupServer" => BackupServer,
                "Port" => Port,
                "Province" => Province,
                "Enable" => Enable,
                "CreateTime" => CreateTime,
                "UpdateTime" => UpdateTime,
                _ => this.GetValue(name, false),
            };
        }
        set
        {
            switch (name)
            {
                case "Id": Id = value.ToInt(); break;
                case "Name": Name = Convert.ToString(value); break;
                case "Server": Server = Convert.ToString(value); break;
                case "BackupServer": BackupServer = Convert.ToString(value); break;
                case "Port": Port = value.ToInt(); break;
                case "Province": Province = Convert.ToString(value); break;
                case "Enable": Enable = value.ToBoolean(); break;
                case "CreateTime": CreateTime = value.ToDateTime(); break;
                case "UpdateTime": UpdateTime = value.ToDateTime(); break;
                default: this.SetValue(name, value); break;
            }
        }
    }
    #endregion

    #region 拷贝
    /// <summary>拷贝模型对象</summary>
    /// <param name="model">模型</param>
    public void Copy(ISuperviseServer model)
    {
        Id = model.Id;
        Name = model.Name;
        Server = model.Server;
        BackupServer = model.BackupServer;
        Port = model.Port;
        Province = model.Province;
        Enable = model.Enable;
        CreateTime = model.CreateTime;
        UpdateTime = model.UpdateTime;
    }
    #endregion
}
