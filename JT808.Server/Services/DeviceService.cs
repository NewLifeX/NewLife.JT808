using JT808.Data;
using NewLife;
using NewLife.Caching;
using NewLife.Log;

namespace JT808.Server.Services;

/// <summary>设备管理服务</summary>
/// <remarks>
/// 管理设备注册、鉴权、在线状态。
/// </remarks>
public class DeviceService
{
    private readonly ICacheProvider _cacheProvider;

    public DeviceService(ICacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;
    }

    /// <summary>根据手机号获取设备</summary>
    public Device? FindByMobile(String mobile)
    {
        if (mobile.IsNullOrEmpty()) return null;
        return Device.FindByMobile(mobile);
    }

    /// <summary>设备自动注册。不存在则创建</summary>
    public Device AutoRegister(String mobile, String? manufacturerId, String? deviceType, String? plateNo)
    {
        var device = Device.FindByMobile(mobile);
        if (device != null) return device;

        device = new Device
        {
            Mobile = mobile,
            ManufacturerId = manufacturerId,
            DeviceType = deviceType,
            PlateNo = plateNo,
            Code = Guid.NewGuid().ToString("N")[..8].ToUpper(),
            Enable = true,
        };
        device.Insert();

        XTrace.WriteLine("自动注册设备：Mobile={0}, Code={1}", mobile, device.Code);
        return device;
    }
}
