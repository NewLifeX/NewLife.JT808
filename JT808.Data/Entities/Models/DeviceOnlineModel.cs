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

/// <summary>设备在线。终端设备在线会话信息</summary>
public partial class DeviceOnlineModel : IModel
{
    #region 属性
    /// <summary>编号</summary>
    public Int32 Id { get; set; }

    /// <summary>设备编号</summary>
    public Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    public String? Mobile { get; set; }

    /// <summary>会话ID</summary>
    public String? SessionId { get; set; }

    /// <summary>远程地址</summary>
    public String? RemoteEndPoint { get; set; }

    /// <summary>纬度</summary>
    public Int32 Latitude { get; set; }

    /// <summary>经度</summary>
    public Int32 Longitude { get; set; }

    /// <summary>速度</summary>
    public Int16 Speed { get; set; }

    /// <summary>方向</summary>
    public Int16 Direction { get; set; }

    /// <summary>地址</summary>
    public String? Address { get; set; }

    /// <summary>最后心跳时间</summary>
    public DateTime HeartbeatTime { get; set; }

    /// <summary>在线状态</summary>
    public Boolean Online { get; set; }

    /// <summary>上线时间</summary>
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
                "Mobile" => Mobile,
                "SessionId" => SessionId,
                "RemoteEndPoint" => RemoteEndPoint,
                "Latitude" => Latitude,
                "Longitude" => Longitude,
                "Speed" => Speed,
                "Direction" => Direction,
                "Address" => Address,
                "HeartbeatTime" => HeartbeatTime,
                "Online" => Online,
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
                case "Mobile": Mobile = Convert.ToString(value); break;
                case "SessionId": SessionId = Convert.ToString(value); break;
                case "RemoteEndPoint": RemoteEndPoint = Convert.ToString(value); break;
                case "Latitude": Latitude = value.ToInt(); break;
                case "Longitude": Longitude = value.ToInt(); break;
                case "Speed": Speed = Convert.ToInt16(value); break;
                case "Direction": Direction = Convert.ToInt16(value); break;
                case "Address": Address = Convert.ToString(value); break;
                case "HeartbeatTime": HeartbeatTime = value.ToDateTime(); break;
                case "Online": Online = value.ToBoolean(); break;
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
    public void Copy(IDeviceOnline model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        Mobile = model.Mobile;
        SessionId = model.SessionId;
        RemoteEndPoint = model.RemoteEndPoint;
        Latitude = model.Latitude;
        Longitude = model.Longitude;
        Speed = model.Speed;
        Direction = model.Direction;
        Address = model.Address;
        HeartbeatTime = model.HeartbeatTime;
        Online = model.Online;
        CreateTime = model.CreateTime;
        UpdateTime = model.UpdateTime;
    }
    #endregion
}
