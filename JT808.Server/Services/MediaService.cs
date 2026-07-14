using NewLife.Log;

namespace JT808.Server.Services;

/// <summary>多媒体文件管理服务</summary>
public class MediaService
{
    public MediaService()
    {
    }

    /// <summary>处理多媒体文件上传</summary>
    public void ProcessMediaUpload(String mobile, Int32 mediaType, Byte[] data)
    {
        XTrace.WriteLine("多媒体上传：Mobile={0}, MediaType={1}, Size={2}", mobile, mediaType, data.Length);
        // TODO: 文件存储、记录入库
    }
}
