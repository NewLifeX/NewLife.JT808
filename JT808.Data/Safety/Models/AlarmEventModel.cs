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

/// <summary>报警事件。主动安全报警事件聚合</summary>
public partial class AlarmEventModel : IModel
{
    #region 属性
    /// <summary>编号</summary>
    public Int64 Id { get; set; }

    /// <summary>设备编号</summary>
    public Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    public String? Mobile { get; set; }

    /// <summary>报警类型。1-ADAS 2-DSM 3-BSD 4-TPMS</summary>
    public Int32 Kind { get; set; }

    /// <summary>事件编码</summary>
    public Int32 Code { get; set; }

    /// <summary>报警级别。0-紧急 1-警告</summary>
    public Int32 Level { get; set; }

    /// <summary>累计次数</summary>
    public Int32 Times { get; set; }

    /// <summary>标志。0-开始 1-结束 2-持续</summary>
    public Int32 Flag { get; set; }

    /// <summary>纬度</summary>
    public Int32 Latitude { get; set; }

    /// <summary>经度</summary>
    public Int32 Longitude { get; set; }

    /// <summary>速度</summary>
    public Int16 Speed { get; set; }

    /// <summary>方向</summary>
    public Int16 Direction { get; set; }

    /// <summary>海拔</summary>
    public Int16 Altitude { get; set; }

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
                "Kind" => Kind,
                "Code" => Code,
                "Level" => Level,
                "Times" => Times,
                "Flag" => Flag,
                "Latitude" => Latitude,
                "Longitude" => Longitude,
                "Speed" => Speed,
                "Direction" => Direction,
                "Altitude" => Altitude,
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
                case "Kind": Kind = value.ToInt(); break;
                case "Code": Code = value.ToInt(); break;
                case "Level": Level = value.ToInt(); break;
                case "Times": Times = value.ToInt(); break;
                case "Flag": Flag = value.ToInt(); break;
                case "Latitude": Latitude = value.ToInt(); break;
                case "Longitude": Longitude = value.ToInt(); break;
                case "Speed": Speed = Convert.ToInt16(value); break;
                case "Direction": Direction = Convert.ToInt16(value); break;
                case "Altitude": Altitude = Convert.ToInt16(value); break;
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
    public void Copy(IAlarmEvent model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        Mobile = model.Mobile;
        Kind = model.Kind;
        Code = model.Code;
        Level = model.Level;
        Times = model.Times;
        Flag = model.Flag;
        Latitude = model.Latitude;
        Longitude = model.Longitude;
        Speed = model.Speed;
        Direction = model.Direction;
        Altitude = model.Altitude;
        AlarmTime = model.AlarmTime;
        CreateTime = model.CreateTime;
    }
    #endregion
}
