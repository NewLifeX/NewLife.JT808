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

/// <summary>位置数据。终端上报的位置信息</summary>
public partial class PositionDataModel : IModel
{
    #region 属性
    /// <summary>编号</summary>
    public Int64 Id { get; set; }

    /// <summary>设备编号</summary>
    public Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    public String? Mobile { get; set; }

    /// <summary>报警标志</summary>
    public UInt32 Alarm { get; set; }

    /// <summary>状态</summary>
    public UInt32 Status { get; set; }

    /// <summary>纬度</summary>
    public Int32 Latitude { get; set; }

    /// <summary>经度</summary>
    public Int32 Longitude { get; set; }

    /// <summary>海拔高度</summary>
    public Int16 Altitude { get; set; }

    /// <summary>速度(km/h)</summary>
    public Int16 Speed { get; set; }

    /// <summary>方向</summary>
    public Int16 Direction { get; set; }

    /// <summary>里程(km)</summary>
    public Double Mileage { get; set; }

    /// <summary>油量(L)</summary>
    public Double Fuel { get; set; }

    /// <summary>百度纬度</summary>
    public Double LatitudeBD { get; set; }

    /// <summary>百度经度</summary>
    public Double LongitudeBD { get; set; }

    /// <summary>附加信息JSON</summary>
    public String? Extras { get; set; }

    /// <summary>GPS时间</summary>
    public DateTime GPSTime { get; set; }

    /// <summary>接收时间</summary>
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
                "Alarm" => Alarm,
                "Status" => Status,
                "Latitude" => Latitude,
                "Longitude" => Longitude,
                "Altitude" => Altitude,
                "Speed" => Speed,
                "Direction" => Direction,
                "Mileage" => Mileage,
                "Fuel" => Fuel,
                "LatitudeBD" => LatitudeBD,
                "LongitudeBD" => LongitudeBD,
                "Extras" => Extras,
                "GPSTime" => GPSTime,
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
                case "Alarm": Alarm = Convert.ToUInt32(value); break;
                case "Status": Status = Convert.ToUInt32(value); break;
                case "Latitude": Latitude = value.ToInt(); break;
                case "Longitude": Longitude = value.ToInt(); break;
                case "Altitude": Altitude = Convert.ToInt16(value); break;
                case "Speed": Speed = Convert.ToInt16(value); break;
                case "Direction": Direction = Convert.ToInt16(value); break;
                case "Mileage": Mileage = value.ToDouble(); break;
                case "Fuel": Fuel = value.ToDouble(); break;
                case "LatitudeBD": LatitudeBD = value.ToDouble(); break;
                case "LongitudeBD": LongitudeBD = value.ToDouble(); break;
                case "Extras": Extras = Convert.ToString(value); break;
                case "GPSTime": GPSTime = value.ToDateTime(); break;
                case "CreateTime": CreateTime = value.ToDateTime(); break;
                default: this.SetValue(name, value); break;
            }
        }
    }
    #endregion

    #region 拷贝
    /// <summary>拷贝模型对象</summary>
    /// <param name="model">模型</param>
    public void Copy(IPositionData model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        Mobile = model.Mobile;
        Alarm = model.Alarm;
        Status = model.Status;
        Latitude = model.Latitude;
        Longitude = model.Longitude;
        Altitude = model.Altitude;
        Speed = model.Speed;
        Direction = model.Direction;
        Mileage = model.Mileage;
        Fuel = model.Fuel;
        LatitudeBD = model.LatitudeBD;
        LongitudeBD = model.LongitudeBD;
        Extras = model.Extras;
        GPSTime = model.GPSTime;
        CreateTime = model.CreateTime;
    }
    #endregion
}
