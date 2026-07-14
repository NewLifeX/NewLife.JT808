using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using NewLife;
using NewLife.Data;
using NewLife.Log;
using NewLife.Model;
using NewLife.Reflection;
using NewLife.Threading;
using NewLife.Web;
using XCode;
using XCode.Cache;
using XCode.Configuration;
using XCode.DataAccessLayer;
using XCode.Membership;
using XCode.Shards;

namespace JT808.Data;

public partial class PositionData : Entity<PositionData>
{
    #region 对象操作
    static PositionData()
    {
        // 累加字段，生成 Update xx Set Count=Count+1234 Where xxx
        //var df = Meta.Factory.AdditionalFields;
        //df.Add(nameof(DeviceId));

        // 拦截器 UserInterceptor、TimeInterceptor、IPInterceptor
        Meta.Interceptors.Add<TimeInterceptor>();

        // 实体缓存
        // var ec = Meta.Cache;
        // ec.Expire = 60;
    }

    /// <summary>验证并修补数据，返回验证结果，或者通过抛出异常的方式提示验证失败。</summary>
    /// <param name="method">添删改方法</param>
    public override Boolean Valid(DataMethod method)
    {
        //if (method == DataMethod.Delete) return true;
        // 如果没有脏数据，则不需要进行任何处理
        if (!HasDirty) return true;

        // 建议先调用基类方法，基类方法会做一些统一处理
        if (!base.Valid(method)) return false;

        // 在新插入数据或者修改了指定字段时进行修正

        // 保留2位小数
        //Mileage = Math.Round(Mileage, 2);
        //Fuel = Math.Round(Fuel, 2);
        //LatitudeBD = Math.Round(LatitudeBD, 2);
        //LongitudeBD = Math.Round(LongitudeBD, 2);
        //if (method == DataMethod.Insert && !Dirtys[nameof(CreateTime)]) CreateTime = DateTime.Now;

        return true;
    }

    ///// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //protected override void InitData()
    //{
    //    // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
    //    if (Meta.Session.Count > 0) return;

    //    if (XTrace.Debug) XTrace.WriteLine("开始初始化PositionData[位置数据]数据……");

    //    var entity = new PositionData();
    //    entity.Id = 0;
    //    entity.DeviceId = 0;
    //    entity.Mobile = "abc";
    //    entity.Alarm = 0;
    //    entity.Status = 0;
    //    entity.Latitude = 0;
    //    entity.Longitude = 0;
    //    entity.Altitude = 0;
    //    entity.Speed = 0;
    //    entity.Direction = 0;
    //    entity.Mileage = 0.0;
    //    entity.Fuel = 0.0;
    //    entity.LatitudeBD = 0.0;
    //    entity.LongitudeBD = 0.0;
    //    entity.Extras = "abc";
    //    entity.GPSTime = DateTime.Now;
    //    entity.Insert();

    //    if (XTrace.Debug) XTrace.WriteLine("完成初始化PositionData[位置数据]数据！");
    //}

    ///// <summary>已重载。基类先调用Valid(true)验证数据，然后在事务保护内调用OnInsert</summary>
    ///// <returns></returns>
    //public override Int32 Insert()
    //{
    //    return base.Insert();
    //}

    ///// <summary>已重载。在事务保护范围内处理业务，位于Valid之后</summary>
    ///// <returns></returns>
    //protected override Int32 OnDelete()
    //{
    //    return base.OnDelete();
    //}
    #endregion

    #region 扩展属性
    /// <summary>设备编号</summary>
    [XmlIgnore, IgnoreDataMember, ScriptIgnore]
    public Device? Device => Extends.Get(nameof(Device), k => Device.FindById(DeviceId));

    /// <summary>设备编号</summary>
    [Map(nameof(DeviceId), typeof(Device), "Id")]
    public String? DeviceMobile => Device?.Mobile;
    #endregion

    #region 高级查询

    // Select Count(Id) as Id,Mobile From PositionData Where GPSTime>'2020-01-24 00:00:00' Group By Mobile Order By Id Desc limit 20
    static readonly FieldCache<PositionData> _MobileCache = new(nameof(Mobile))
    {
        //Where = _.GPSTime > DateTime.Today.AddDays(-30) & Expression.Empty
    };

    /// <summary>获取手机号列表，字段缓存10分钟，分组统计数据最多的前20种，用于魔方前台下拉选择</summary>
    /// <returns></returns>
    public static IDictionary<String, String> GetMobileList() => _MobileCache.FindAllName();
    #endregion

    #region 业务操作
    public IPositionData ToModel()
    {
        var model = new PositionData();
        model.Copy(this);

        return model;
    }

    #endregion
}
