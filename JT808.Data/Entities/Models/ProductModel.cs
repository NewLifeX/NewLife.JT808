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

/// <summary>产品。终端设备型号分类</summary>
public partial class ProductModel : IModel
{
    #region 属性
    /// <summary>编号</summary>
    public Int32 Id { get; set; }

    /// <summary>名称</summary>
    public String Name { get; set; } = null!;

    /// <summary>制造商ID</summary>
    public String? ManufacturerId { get; set; }

    /// <summary>终端型号</summary>
    public String? DeviceType { get; set; }

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
                "ManufacturerId" => ManufacturerId,
                "DeviceType" => DeviceType,
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
                case "ManufacturerId": ManufacturerId = Convert.ToString(value); break;
                case "DeviceType": DeviceType = Convert.ToString(value); break;
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
    public void Copy(IProduct model)
    {
        Id = model.Id;
        Name = model.Name;
        ManufacturerId = model.ManufacturerId;
        DeviceType = model.DeviceType;
        Enable = model.Enable;
        CreateTime = model.CreateTime;
        UpdateTime = model.UpdateTime;
    }
    #endregion
}
