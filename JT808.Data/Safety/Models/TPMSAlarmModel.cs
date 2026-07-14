using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using NewLife;
using NewLife.Data;
using NewLife.Reflection;

namespace JT808.Data.Safety;

/// <summary>TPMS报警。胎压监测报警</summary>
public partial class TPMSAlarmModel : IModel
{
    #region 属性
    /// <summary>编号</summary>
    public Int64 Id { get; set; }

    /// <summary>设备编号</summary>
    public Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    public String? Mobile { get; set; }

    /// <summary>报警事件编号</summary>
    public Int64 EventId { get; set; }

    /// <summary>报警类型。1-胎压低 2-胎压高 3-胎温高 4-漏气</summary>
    public Int32 AlarmType { get; set; }

    /// <summary>报警级别</summary>
    public Int32 Level { get; set; }

    /// <summary>轮胎编号</summary>
    public Int32 TireNo { get; set; }

    /// <summary>胎压(Pa)</summary>
    public Double Pressure { get; set; }

    /// <summary>温度(℃)</summary>
    public Int16 Temperature { get; set; }

    /// <summary>报警时间</summary>
    public DateTime AlarmTime { get; set; }

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
                "EventId" => EventId,
                "AlarmType" => AlarmType,
                "Level" => Level,
                "TireNo" => TireNo,
                "Pressure" => Pressure,
                "Temperature" => Temperature,
                "AlarmTime" => AlarmTime,
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
                case "EventId": EventId = value.ToLong(); break;
                case "AlarmType": AlarmType = value.ToInt(); break;
                case "Level": Level = value.ToInt(); break;
                case "TireNo": TireNo = value.ToInt(); break;
                case "Pressure": Pressure = value.ToDouble(); break;
                case "Temperature": Temperature = Convert.ToInt16(value); break;
                case "AlarmTime": AlarmTime = value.ToDateTime(); break;
                case "CreateTime": CreateTime = value.ToDateTime(); break;
                default: this.SetValue(name, value); break;
            }
        }
    }
    #endregion

    #region 拷贝
    /// <summary>拷贝模型对象</summary>
    /// <param name="model">模型</param>
    public void Copy(ITPMSAlarm model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        Mobile = model.Mobile;
        EventId = model.EventId;
        AlarmType = model.AlarmType;
        Level = model.Level;
        TireNo = model.TireNo;
        Pressure = model.Pressure;
        Temperature = model.Temperature;
        AlarmTime = model.AlarmTime;
        CreateTime = model.CreateTime;
    }
    #endregion
}
