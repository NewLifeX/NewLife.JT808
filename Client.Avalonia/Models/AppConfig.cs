using System.Text.Json;

namespace Client.Avalonia.Models;

/// <summary>应用程序配置 — 持久化到 JSON 文件</summary>
public class AppConfig
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
    };

    /// <summary>最近使用的服务端地址</summary>
    public String ServerAddress { get; set; } = "127.0.0.1";

    /// <summary>最近使用的端口</summary>
    public Int32 ServerPort { get; set; } = 808;

    /// <summary>最近使用的手机号</summary>
    public String Mobile { get; set; } = "013800123456";

    /// <summary>Web API 地址</summary>
    public String ApiAddress { get; set; } = "http://127.0.0.1:5000";

    /// <summary>Web API Token</summary>
    public String ApiToken { get; set; } = "";

    /// <summary>窗口宽度</summary>
    public Double WindowWidth { get; set; } = 1100;

    /// <summary>窗口高度</summary>
    public Double WindowHeight { get; set; } = 750;

    /// <summary>配置文件路径</summary>
    public static String FilePath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "NewLife",
        "JT808",
        "Client.Avalonia",
        "config.json");

    /// <summary>加载配置</summary>
    public static AppConfig Load()
    {
        try
        {
            var path = FilePath;
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<AppConfig>(json, _jsonOptions) ?? new AppConfig();
            }
        }
        catch
        {
            // 忽略加载错误
        }
        return new AppConfig();
    }

    /// <summary>保存配置</summary>
    public void Save()
    {
        try
        {
            var path = FilePath;
            var dir = Path.GetDirectoryName(path);
            if (!String.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

            var json = JsonSerializer.Serialize(this, _jsonOptions);
            File.WriteAllText(path, json);
        }
        catch
        {
            // 忽略保存错误
        }
    }
}
