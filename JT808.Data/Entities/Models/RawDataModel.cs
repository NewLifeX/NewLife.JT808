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

/// <summary>原始报文。终端上传的原始数据</summary>
public partial class RawDataModel : IModel
{
    #region 属性
    /// <summary>编号</summary>
    public Int64 Id { get; set; }

    /// <summary>设备编号</summary>
    public Int32 DeviceId { get; set; }

    /// <summary>手机号</summary>
    public String? Mobile { get; set; }

    /// <summary>消息类型</summary>
    public UInt16 Kind { get; set; }

    /// <summary>十六进制数据</summary>
    public String? HexData { get; set; }

    /// <summary>备注</summary>
    public String? Remark { get; set; }

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
                "HexData" => HexData,
                "Remark" => Remark,
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
                case "Kind": Kind = Convert.ToUInt16(value); break;
                case "HexData": HexData = Convert.ToString(value); break;
                case "Remark": Remark = Convert.ToString(value); break;
                case "CreateTime": CreateTime = value.ToDateTime(); break;
                default: this.SetValue(name, value); break;
            }
        }
    }
    #endregion

    #region 拷贝
    /// <summary>拷贝模型对象</summary>
    /// <param name="model">模型</param>
    public void Copy(IRawData model)
    {
        Id = model.Id;
        DeviceId = model.DeviceId;
        Mobile = model.Mobile;
        Kind = model.Kind;
        HexData = model.HexData;
        Remark = model.Remark;
        CreateTime = model.CreateTime;
    }
    #endregion
}
