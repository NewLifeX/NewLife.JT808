using System.Text.Json;
using JT808.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NewLife;
using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace JT808.Web.Controllers;

/// <summary>JT808 管理 API 控制器</summary>
/// <remarks>
/// 会话、统计、黑名单与指令下发均基于分布式缓存中的集群共享状态，
/// 与网关进程解耦——Web 无需启动本地网关实例即可管理远程终端。
/// </remarks>
[ApiController]
[Route("api/jt808")]
public class JT808Controller : ControllerBase
{
    private readonly ClusterState _cluster;
    private readonly CommandClient _client;
    private readonly JT808Auth? _auth;

    /// <summary>实例化</summary>
    /// <param name="cluster">集群共享状态</param>
    /// <param name="client">指令下发客户端</param>
    /// <param name="auth">Token 鉴权服务</param>
    public JT808Controller(ClusterState cluster, CommandClient client, JT808Auth? auth = null)
    {
        _cluster = cluster;
        _client = client;
        _auth = auth;
    }

    #region 鉴权
    private Boolean VerifyToken()
    {
        if (_auth == null) return true;

        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        return _auth.VerifyHeader(authHeader);
    }

    private IActionResult? CheckAuth()
    {
        if (!VerifyToken())
            return Unauthorized(new { success = false, message = "Token 无效或未提供" });
        return null;
    }
    #endregion

    #region 指令下发
    /// <summary>统一下发指令</summary>
    /// <param name="request">下发请求</param>
    /// <returns>下发结果</returns>
    [HttpPost("command")]
    public IActionResult SendCommand([FromBody] SendCommandRequest request)
    {
        var authResult = CheckAuth();
        if (authResult != null) return authResult;

        if (String.IsNullOrEmpty(request.Mobile) || request.BodyType.IsNullOrEmpty())
            return BadRequest("缺少必要参数");
        if (request.Body.ValueKind != JsonValueKind.Object)
            return BadRequest("缺少必要参数");

        var type = MessageFactory.FindType(request.BodyType);
        if (type == null) return BadRequest("无法解析消息体");

        var json = request.Body.GetRawText();
        Object? body = null;
        try
        {
            body = JsonHelper.ToJsonEntity(json, type, null);
        }
        catch (Exception)
        {
            // 反序列化失败按参数错误处理，避免把非法请求体当成服务端故障
        }
        if (body == null) return BadRequest("无法解析消息体");

        var session = _cluster.GetSession(request.Mobile);
        if (session == null) return Ok(new { success = false, message = "设备不在线" });

        try
        {
            var result = _client.SendCommand(session.Node, request.Mobile, type.FullName ?? type.Name, json);
            return result ? Ok(new { success = true, message = "指令已下发" })
                          : Ok(new { success = false, message = "指令下发失败" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    #endregion

    #region 会话管理
    /// <summary>获取在线会话列表</summary>
    /// <returns>在线终端列表</returns>
    [HttpGet("sessions")]
    public IActionResult GetSessions()
    {
        var list = _cluster.GetSessions().Select(e => new
        {
            mobile = e.Mobile,
            node = e.Node,
            sessionId = e.SessionId,
            remote = e.Remote,
            time = e.LoginTime.ToString("yyyy-MM-dd HH:mm:ss"),
        }).ToList();

        return Ok(new { total = list.Count, items = list });
    }

    /// <summary>获取指定终端会话详情</summary>
    /// <param name="mobile">终端手机号</param>
    /// <returns>会话详情</returns>
    [HttpGet("session/{mobile}")]
    public IActionResult GetSession(String mobile)
    {
        var authResult = CheckAuth();
        if (authResult != null) return authResult;

        var session = _cluster.GetSession(mobile);
        if (session == null) return NotFound(new { success = false, message = "设备不在线" });

        return Ok(new
        {
            mobile = session.Mobile,
            node = session.Node,
            sessionId = session.SessionId,
            remote = session.Remote,
            time = session.LoginTime.ToString("yyyy-MM-dd HH:mm:ss"),
        });
    }

    /// <summary>强制移除会话</summary>
    /// <param name="mobile">终端手机号</param>
    /// <returns>移除结果</returns>
    [HttpDelete("session/{mobile}")]
    public IActionResult RemoveSession(String mobile)
    {
        var authResult = CheckAuth();
        if (authResult != null) return authResult;

        var session = _cluster.GetSession(mobile);
        if (session == null) return NotFound(new { success = false, message = "设备不在线" });

        var result = _client.Kick(session.Node, mobile);
        return result ? Ok(new { success = true, message = "已通知网关断开会话" })
                      : StatusCode(500, new { success = false, message = "网关队列不可用" });
    }
    #endregion

    #region 服务器统计
    /// <summary>获取服务器统计</summary>
    /// <returns>节点与在线会话统计</returns>
    [HttpGet("stats")]
    public IActionResult GetStats()
    {
        var nodes = _cluster.GetNodes();
        var sessions = _cluster.GetSessions();

        return Ok(new
        {
            server = String.Join(",", nodes.Select(e => e.Node)),
            running = nodes.Count > 0,
            started = nodes.Count == 0 ? String.Empty : nodes.Min(e => e.StartTime).ToString("yyyy-MM-dd HH:mm:ss"),
            nodes = nodes.Select(e => new
            {
                node = e.Node,
                port = e.Port,
                sessions = e.Sessions,
                started = e.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                active = e.LastActive.ToString("yyyy-MM-dd HH:mm:ss"),
            }).ToList(),
            sessions = sessions.Count,
        });
    }
    #endregion

    #region 黑名单管理
    /// <summary>获取黑名单列表</summary>
    /// <returns>黑名单</returns>
    [HttpGet("blacklist")]
    public IActionResult GetBlacklist()
    {
        var authResult = CheckAuth();
        if (authResult != null) return authResult;

        var list = _cluster.GetBlacklist();
        return Ok(new { total = list.Length, items = list });
    }

    /// <summary>添加手机号到黑名单</summary>
    /// <param name="request">黑名单请求</param>
    /// <returns>添加结果</returns>
    [HttpPost("blacklist")]
    public IActionResult AddBlacklist([FromBody] BlacklistRequest request)
    {
        var authResult = CheckAuth();
        if (authResult != null) return authResult;

        if (String.IsNullOrEmpty(request.Mobile))
            return BadRequest(new { success = false, message = "缺少手机号" });

        var result = _cluster.AddBlacklist(request.Mobile);

        // 已在线设备立即断开
        var session = _cluster.GetSession(request.Mobile);
        if (session != null) _client.Kick(session.Node, request.Mobile);

        return Ok(new { success = result, message = result ? "已加入黑名单" : "已在黑名单中" });
    }

    /// <summary>从黑名单移除</summary>
    /// <param name="mobile">终端手机号</param>
    /// <returns>移除结果</returns>
    [HttpDelete("blacklist/{mobile}")]
    public IActionResult RemoveBlacklist(String mobile)
    {
        var authResult = CheckAuth();
        if (authResult != null) return authResult;

        var result = _cluster.RemoveBlacklist(mobile);
        return Ok(new { success = result, message = result ? "已移出黑名单" : "不在黑名单中" });
    }
    #endregion

    #region 请求模型
    /// <summary>统一下发请求</summary>
    public class SendCommandRequest
    {
        /// <summary>终端手机号</summary>
        public String Mobile { get; set; } = null!;

        /// <summary>消息体类型全名</summary>
        public String? BodyType { get; set; }

        /// <summary>消息体数据（JSON 对象）</summary>
        public JsonElement Body { get; set; }
    }

    /// <summary>黑名单操作请求</summary>
    public class BlacklistRequest
    {
        /// <summary>终端手机号</summary>
        public String Mobile { get; set; } = null!;
    }
    #endregion
}
