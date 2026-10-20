using NewLife.Log;

namespace NewLife.JT808.Protocols;

/// <summary>JT808 管理 API Token 鉴权服务</summary>
/// <remarks>
/// 为 JT808 Web API 提供简单的 Token 鉴权保护。
/// Token 通过配置文件或环境变量设置。
/// 
/// 使用示例：
/// <code>
/// var auth = new JT808Auth("my-secret-token");
/// 
/// // 验证请求
/// if (!auth.Verify(token))
///     return Unauthorized();
/// </code>
/// </remarks>
public class JT808Auth
{
    #region 属性
    /// <summary>当前有效的 Token</summary>
    public String Token { get; set; }

    /// <summary>日志</summary>
    public ILog Log { get; set; } = Logger.Null;
    #endregion

    #region 构造
    /// <summary>实例化 Token 鉴权服务</summary>
    /// <param name="token">管理 API 的访问令牌</param>
    public JT808Auth(String? token = null)
    {
        // 优先使用环境变量
        Token = Environment.GetEnvironmentVariable("JT808_API_TOKEN")
            ?? token
            ?? "jt808-default-token";

        if (Token == "jt808-default-token")
            Log?.Warn("JT808 API token uses the default value, set it via the JT808_API_TOKEN environment variable.");
    }
    #endregion

    #region 方法
    /// <summary>验证 Token 是否有效</summary>
    /// <param name="token">请求中的 Token</param>
    /// <returns>是否有效</returns>
    public Boolean Verify(String? token) => !token.IsNullOrEmpty() && token == Token;

    /// <summary>从 Authorization Header 提取并验证 Token</summary>
    /// <param name="authorization">Authorization Header 值</param>
    /// <returns>是否验证通过</returns>
    public Boolean VerifyHeader(String? authorization)
    {
        if (authorization.IsNullOrEmpty()) return false;

        // 支持 "Bearer <token>" 和直接 token 两种格式
        var token = authorization;
        if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            token = token[7..];

        return Verify(token.Trim());
    }
    #endregion
}
