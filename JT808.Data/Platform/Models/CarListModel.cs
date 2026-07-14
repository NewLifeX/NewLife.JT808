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

/// <summary>车辆清单。平台接入车辆白名单</summary>
public partial class CarListModel : IModel
{
    #region 属性
    /// <summary>编号</summary>
    public Int32 Id { get; set; }

    /// <summary>手机号</summary>
    public String Mobile { get; set; } = null!;

    /// <summary>车牌号</summary>
    public String? PlateNo { get; set; }

    /// <summary>车牌颜色</summary>
    public Int32 PlateColor { get; set; }

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
                "Mobile" => Mobile,
                "PlateNo" => PlateNo,
                "PlateColor" => PlateColor,
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
                case "Mobile": Mobile = Convert.ToString(value); break;
                case "PlateNo": PlateNo = Convert.ToString(value); break;
                case "PlateColor": PlateColor = value.ToInt(); break;
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
    public void Copy(ICarList model)
    {
        Id = model.Id;
        Mobile = model.Mobile;
        PlateNo = model.PlateNo;
        PlateColor = model.PlateColor;
        Enable = model.Enable;
        CreateTime = model.CreateTime;
        UpdateTime = model.UpdateTime;
    }
    #endregion
}
