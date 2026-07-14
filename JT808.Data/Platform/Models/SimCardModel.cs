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

/// <summary>物联网卡。SIM卡信息管理</summary>
public partial class SimCardModel : IModel
{
    #region 属性
    /// <summary>编号</summary>
    public Int32 Id { get; set; }

    /// <summary>手机号</summary>
    public String Mobile { get; set; } = null!;

    /// <summary>运营商</summary>
    public String? Operator { get; set; }

    /// <summary>启用</summary>
    public Boolean Enable { get; set; }

    /// <summary>到期时间</summary>
    public DateTime ExpireTime { get; set; }

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
                "Mobile" => Mobile,
                "Operator" => Operator,
                "Enable" => Enable,
                "ExpireTime" => ExpireTime,
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
                case "Mobile": Mobile = Convert.ToString(value); break;
                case "Operator": Operator = Convert.ToString(value); break;
                case "Enable": Enable = value.ToBoolean(); break;
                case "ExpireTime": ExpireTime = value.ToDateTime(); break;
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
    public void Copy(ISimCard model)
    {
        Id = model.Id;
        Mobile = model.Mobile;
        Operator = model.Operator;
        Enable = model.Enable;
        ExpireTime = model.ExpireTime;
        CreateTime = model.CreateTime;
        UpdateTime = model.UpdateTime;
    }
    #endregion
}
