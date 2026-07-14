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

/// <summary>胎压数据。轮胎压力和温度监测</summary>
public partial class TirePressureModel : IModel
{
    #region 属性
    /// <summary>编号</summary>
    public Int64 Id { get; set; }

    /// <summary>设备编号</summary>
    public Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    public String? Mobile { get; set; }

    /// <summary>轮胎编号</summary>
    public Int32 TireNo { get; set; }

    /// <summary>胎压(Pa)</summary>
    public Double Pressure { get; set; }

    /// <summary>温度(℃)</summary>
    public Int16 Temperature { get; set; }

    /// <summary>报警状态</summary>
    public Boolean Alarm { get; set; }

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
                "Mobile" => Mobile,
                "TireNo" => TireNo,
                "Pressure" => Pressure,
                "Temperature" => Temperature,
                "Alarm" => Alarm,
                "CreateTime" => CreateTime,
                _ => this.GetValue(name, false),
            };
        }
        set
        {
            switch (name)
            {
                case "Id": Id = value.ToLong(); break;
                case "DeviceId": DeviceId = value.ToInt(); break;
                case "Mobile": Mobile = Convert.ToString(value); break;
                case "TireNo": TireNo = value.ToInt(); break;
                case "Pressure": Pressure = value.ToDouble(); break;
                case "Temperature": Temperature = Convert.ToInt16(value); break;
                case "Alarm": Alarm = value.ToBoolean(); break;
                case "CreateTime": CreateTime = value.ToDateTime(); break;
                default: this.SetValue(name, value); break;
            }
        }
    }
    #endregion

    #region 拷贝
    /// <summary>拷贝模型对象</summary>
    /// <param name="model">模型</param>
    public void Copy(ITirePressure model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        Mobile = model.Mobile;
        TireNo = model.TireNo;
        Pressure = model.Pressure;
        Temperature = model.Temperature;
        Alarm = model.Alarm;
        CreateTime = model.CreateTime;
    }
    #endregion
}
