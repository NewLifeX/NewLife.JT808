using JT808.Data;
using NewLife;
using NewLife.Log;

namespace JT808.Server.Services;

/// <summary>多媒体文件管理服务</summary>
public class MediaService
{
    /// <summary>处理多媒体文件上传</summary>
    /// <param name="mobile">终端手机号</param>
    /// <param name="mediaType">多媒体类型</param>
    /// <param name="data">多媒体数据</param>
    public void ProcessMediaUpload(String mobile, Int32 mediaType, Byte[] data)
    {
        XTrace.WriteLine("多媒体上传：Mobile={0}, MediaType={1}, Size={2}", mobile, mediaType, data.Length);

        // 保存媒体记录
        try
        {
            var device = Device.FindByMobile(mobile);
            if (device != null)
            {
                var record = new MediaRecord
                {
                    DeviceId = device.Id,
                    Mobile = mobile,
                    MediaId = mediaType,
                    Status = 0, // 上传中
                    CreateTime = DateTime.Now,
                };
                record.InsertAsync();
            }
        }
        catch (Exception ex)
        {
            XTrace.WriteLine("多媒体文件存储异常：{0}", ex.Message);
        }
    }
}
