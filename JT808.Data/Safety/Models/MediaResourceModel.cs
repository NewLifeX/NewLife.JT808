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

/// <summary>媒体资源。主动安全多媒体资源索引</summary>
public partial class MediaResourceModel : IModel
{
    #region 属性
    /// <summary>编号</summary>
    public Int64 Id { get; set; }

    /// <summary>设备编号</summary>
    public Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    public String? Mobile { get; set; }

    /// <summary>报警时间</summary>
    public DateTime AlarmTime { get; set; }

    /// <summary>媒体类型</summary>
    public Int32 MediaType { get; set; }

    /// <summary>资源地址</summary>
    public String? Url { get; set; }

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
                "AlarmTime" => AlarmTime,
                "MediaType" => MediaType,
                "Url" => Url,
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
                case "AlarmTime": AlarmTime = value.ToDateTime(); break;
                case "MediaType": MediaType = value.ToInt(); break;
                case "Url": Url = Convert.ToString(value); break;
                case "CreateTime": CreateTime = value.ToDateTime(); break;
                default: this.SetValue(name, value); break;
            }
        }
    }
    #endregion

    #region 拷贝
    /// <summary>拷贝模型对象</summary>
    /// <param name="model">模型</param>
    public void Copy(IMediaResource model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        Mobile = model.Mobile;
        AlarmTime = model.AlarmTime;
        MediaType = model.MediaType;
        Url = model.Url;
        CreateTime = model.CreateTime;
    }
    #endregion
}
