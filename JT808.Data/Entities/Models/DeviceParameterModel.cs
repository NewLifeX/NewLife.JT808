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

/// <summary>设备参数。终端参数配置记录</summary>
public partial class DeviceParameterModel : IModel
{
    #region 属性
    /// <summary>编号</summary>
    public Int32 Id { get; set; }

    /// <summary>设备编号</summary>
    public Int32 DeviceId { get; set; }

    /// <summary>参数ID</summary>
    public Int32 ParamId { get; set; }

    /// <summary>参数值</summary>
    public String? ParamValue { get; set; }

    /// <summary>创建时间</summary>
    public DateTime CreateTime { get; set; }
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
                "ParamId" => ParamId,
                "ParamValue" => ParamValue,
                "CreateTime" => CreateTime,
                _ => this.GetValue(name, false),
            };
        }
        set
        {
            switch (name)
            {
                case "Id": Id = value.ToInt(); break;
                case "DeviceId": DeviceId = value.ToInt(); break;
                case "ParamId": ParamId = value.ToInt(); break;
                case "ParamValue": ParamValue = Convert.ToString(value); break;
                case "CreateTime": CreateTime = value.ToDateTime(); break;
                default: this.SetValue(name, value); break;
            }
        }
    }
    #endregion

    #region 拷贝
    /// <summary>拷贝模型对象</summary>
    /// <param name="model">模型</param>
    public void Copy(IDeviceParameter model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        ParamId = model.ParamId;
        ParamValue = model.ParamValue;
        CreateTime = model.CreateTime;
    }
    #endregion
}
