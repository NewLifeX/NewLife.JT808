using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using NewLife;
using NewLife.Data;
using NewLife.Reflection;

namespace JT808.Data;

/// <summary>驾驶员信息</summary>
public partial class DriverModel : IModel
{
    #region 属性
    /// <summary>编号</summary>
    public Int32 Id { get; set; }

    /// <summary>设备编号</summary>
    public Int32 DeviceId { get; set; }

    /// <summary>姓名</summary>
    public String? Name { get; set; }

    /// <summary>身份证号</summary>
    public String? IDCard { get; set; }

    /// <summary>从业资格证号</summary>
    public String? Qualification { get; set; }

    /// <summary>手机号</summary>
    public String? Mobile { get; set; }

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
                "DeviceId" => DeviceId,
                "Name" => Name,
                "IDCard" => IDCard,
                "Qualification" => Qualification,
                "Mobile" => Mobile,
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
                case "DeviceId": DeviceId = value.ToInt(); break;
                case "Name": Name = Convert.ToString(value); break;
                case "IDCard": IDCard = Convert.ToString(value); break;
                case "Qualification": Qualification = Convert.ToString(value); break;
                case "Mobile": Mobile = Convert.ToString(value); break;
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
    public void Copy(IDriver model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        Name = model.Name;
        IDCard = model.IDCard;
        Qualification = model.Qualification;
        Mobile = model.Mobile;
        CreateTime = model.CreateTime;
        UpdateTime = model.UpdateTime;
    }
    #endregion
}
