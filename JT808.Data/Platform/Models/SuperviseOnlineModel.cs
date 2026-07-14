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

/// <summary>监管在线。监管平台在线状态</summary>
public partial class SuperviseOnlineModel : IModel
{
    #region 属性
    /// <summary>编号</summary>
    public Int32 Id { get; set; }

    /// <summary>监管服务编号</summary>
    public Int32 ServerId { get; set; }

    /// <summary>在线状态</summary>
    public Boolean Online { get; set; }

    /// <summary>最后心跳时间</summary>
    public DateTime HeartbeatTime { get; set; }

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
                "ServerId" => ServerId,
                "Online" => Online,
                "HeartbeatTime" => HeartbeatTime,
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
                case "ServerId": ServerId = value.ToInt(); break;
                case "Online": Online = value.ToBoolean(); break;
                case "HeartbeatTime": HeartbeatTime = value.ToDateTime(); break;
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
    public void Copy(ISuperviseOnline model)
    {
        Id = model.Id;
        ServerId = model.ServerId;
        Online = model.Online;
        HeartbeatTime = model.HeartbeatTime;
        CreateTime = model.CreateTime;
        UpdateTime = model.UpdateTime;
    }
    #endregion
}
