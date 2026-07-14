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

/// <summary>指令记录。平台下发给终端的指令</summary>
public partial class DeviceCommandModel : IModel
{
    #region 属性
    /// <summary>编号</summary>
    public Int64 Id { get; set; }

    /// <summary>设备编号</summary>
    public Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    public String? Mobile { get; set; }

    /// <summary>指令类型</summary>
    public Int32 Kind { get; set; }

    /// <summary>流水号</summary>
    public UInt16 Sequence { get; set; }

    /// <summary>状态。0-待下发 1-已下发 2-已应答 3-超时</summary>
    public Int32 Status { get; set; }

    /// <summary>结果</summary>
    public String? Result { get; set; }

    /// <summary>消息体类型</summary>
    public String? BodyType { get; set; }

    /// <summary>消息体数据</summary>
    public String? BodyData { get; set; }

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
                "Mobile" => Mobile,
                "Kind" => Kind,
                "Sequence" => Sequence,
                "Status" => Status,
                "Result" => Result,
                "BodyType" => BodyType,
                "BodyData" => BodyData,
                "CreateTime" => CreateTime,
                "UpdateTime" => UpdateTime,
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
                case "Sequence": Sequence = Convert.ToUInt16(value); break;
                case "Status": Status = value.ToInt(); break;
                case "Result": Result = Convert.ToString(value); break;
                case "BodyType": BodyType = Convert.ToString(value); break;
                case "BodyData": BodyData = Convert.ToString(value); break;
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
    public void Copy(IDeviceCommand model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        Mobile = model.Mobile;
        Kind = model.Kind;
        Sequence = model.Sequence;
        Status = model.Status;
        Result = model.Result;
        BodyType = model.BodyType;
        BodyData = model.BodyData;
        CreateTime = model.CreateTime;
        UpdateTime = model.UpdateTime;
    }
    #endregion
}
