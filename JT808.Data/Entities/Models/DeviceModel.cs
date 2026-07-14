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

/// <summary>终端设备。JT808终端设备信息</summary>
public partial class DeviceModel : IModel
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

    /// <summary>产品编号</summary>
    public Int32 ProductId { get; set; }

    /// <summary>分组编号</summary>
    public Int32 GroupId { get; set; }

    /// <summary>省域ID</summary>
    public Int16 ProvinceId { get; set; }

    /// <summary>市县域ID</summary>
    public Int16 CityId { get; set; }

    /// <summary>制造商ID</summary>
    public String? ManufacturerId { get; set; }

    /// <summary>终端型号</summary>
    public String? DeviceType { get; set; }

    /// <summary>终端ID</summary>
    public String? DeviceId { get; set; }

    /// <summary>协议版本</summary>
    public String? Version { get; set; }

    /// <summary>鉴权码</summary>
    public String? Code { get; set; }

    /// <summary>启用</summary>
    public Boolean Enable { get; set; }

    /// <summary>备注</summary>
    public String? Remark { get; set; }

    /// <summary>创建时间</summary>
    public DateTime CreateTime { get; set; }

    /// <summary>更新时间</summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>创建者</summary>
    public String? CreateUser { get; set; }
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
                "ProductId" => ProductId,
                "GroupId" => GroupId,
                "ProvinceId" => ProvinceId,
                "CityId" => CityId,
                "ManufacturerId" => ManufacturerId,
                "DeviceType" => DeviceType,
                "DeviceId" => DeviceId,
                "Version" => Version,
                "Code" => Code,
                "Enable" => Enable,
                "Remark" => Remark,
                "CreateTime" => CreateTime,
                "UpdateTime" => UpdateTime,
                "CreateUser" => CreateUser,
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
                case "ProductId": ProductId = value.ToInt(); break;
                case "GroupId": GroupId = value.ToInt(); break;
                case "ProvinceId": ProvinceId = Convert.ToInt16(value); break;
                case "CityId": CityId = Convert.ToInt16(value); break;
                case "ManufacturerId": ManufacturerId = Convert.ToString(value); break;
                case "DeviceType": DeviceType = Convert.ToString(value); break;
                case "DeviceId": DeviceId = Convert.ToString(value); break;
                case "Version": Version = Convert.ToString(value); break;
                case "Code": Code = Convert.ToString(value); break;
                case "Enable": Enable = value.ToBoolean(); break;
                case "Remark": Remark = Convert.ToString(value); break;
                case "CreateTime": CreateTime = value.ToDateTime(); break;
                case "UpdateTime": UpdateTime = value.ToDateTime(); break;
                case "CreateUser": CreateUser = Convert.ToString(value); break;
                default: this.SetValue(name, value); break;
            }
        }
    }
    #endregion

    #region 拷贝
    /// <summary>拷贝模型对象</summary>
    /// <param name="model">模型</param>
    public void Copy(IDevice model)
    {
        Id = model.Id;
        Mobile = model.Mobile;
        PlateNo = model.PlateNo;
        PlateColor = model.PlateColor;
        ProductId = model.ProductId;
        GroupId = model.GroupId;
        ProvinceId = model.ProvinceId;
        CityId = model.CityId;
        ManufacturerId = model.ManufacturerId;
        DeviceType = model.DeviceType;
        DeviceId = model.DeviceId;
        Version = model.Version;
        Code = model.Code;
        Enable = model.Enable;
        Remark = model.Remark;
        CreateTime = model.CreateTime;
        UpdateTime = model.UpdateTime;
        CreateUser = model.CreateUser;
    }
    #endregion
}
