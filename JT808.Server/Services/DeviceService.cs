using NewLife;
using NewLife.Caching;
using NewLife.Log;

namespace JT808.Server.Services;

/// <summary>设备管理服务</summary>
/// <remarks>
/// 管理设备注册、鉴权、在线状态。依赖 JT808.Data 实体（xcode 生成后启用）。
/// </remarks>
public class DeviceService
{
    private readonly ICacheProvider _cacheProvider;

    public DeviceService(ICacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;
    }

    /// <summary>根据手机号获取设备</summary>
    public Object? FindByMobile(String mobile)
    {
        if (mobile.IsNullOrEmpty()) return null;
        // TODO: 实体生成后替换为 Device.FindByMobile(mobile)
        return null;
    }

    /// <summary>设备自动注册。不存在则创建</summary>
    public Object? AutoRegister(String mobile, String? manufacturerId, String? deviceType, String? plateNo)
    {
        XTrace.WriteLine("自动注册设备：Mobile={0}", mobile);
        // TODO: 实体生成后实现 Device 自动注册逻辑
        return null;
    }
}
