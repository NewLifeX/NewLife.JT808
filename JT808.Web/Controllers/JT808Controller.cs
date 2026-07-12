using Microsoft.AspNetCore.Mvc;
using NewLife.JT808.Models;
using NewLife.JT808.Protocols;

namespace JT808.Web.Controllers;

/// <summary>JT808 管理 API 控制器</summary>
[ApiController]
[Route("api/jt808")]
public class JT808Controller : ControllerBase
{
    private readonly JT808Server _server;
    private readonly JT808Auth? _auth;

    /// <summary>实例化</summary>
    public JT808Controller(JT808Server server, JT808Auth? auth = null)
    {
        _server = server;
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
    [HttpPost("command")]
    public IActionResult SendCommand([FromBody] SendCommandRequest request)
    {
        var authResult = CheckAuth();
        if (authResult != null) return authResult;

        if (String.IsNullOrEmpty(request.Mobile) || request.Body == null)
            return BadRequest("缺少必要参数");

        try
        {
            var body = CreateMessageBody(request.BodyType, request.Body);
            if (body == null) return BadRequest("无法解析消息体");

            var bus = _server.CommandBus ?? _server.Manager;
            var result = bus.Send(request.Mobile, body);
            return result ? Ok(new { success = true, message = "指令已下发" })
                          : Ok(new { success = false, message = "设备不在线" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    #endregion

    #region 会话管理
    /// <summary>获取在线会话列表</summary>
    [HttpGet("sessions")]
    public IActionResult GetSessions()
    {
        return Ok(new { total = 0 });
    }

    /// <summary>获取指定终端会话详情</summary>
    [HttpGet("session/{mobile}")]
    public IActionResult GetSession(String mobile)
    {
        var authResult = CheckAuth();
        if (authResult != null) return authResult;

        var session = _server.Manager.Find(mobile);
        if (session == null) return NotFound(new { success = false, message = "设备不在线" });

        return Ok(new
        {
            mobile,
            sessionId = session.ID,
            remote = session.Remote?.ToString(),
            time = DateTime.UtcNow,
        });
    }

    /// <summary>强制移除会话</summary>
    [HttpDelete("session/{mobile}")]
    public IActionResult RemoveSession(String mobile)
    {
        var authResult = CheckAuth();
        if (authResult != null) return authResult;

        var session = _server.Manager.Find(mobile);
        if (session == null) return NotFound(new { success = false, message = "设备不在线" });

        _server.Manager.Remove(mobile);
        session.Close("管理员强制断开");

        return Ok(new { success = true, message = "会话已移除" });
    }
    #endregion

    #region 服务器统计
    /// <summary>获取服务器统计</summary>
    [HttpGet("stats")]
    public IActionResult GetStats()
    {
        return Ok(new
        {
            server = $"{_server.Name ?? "jt808"}:{_server.Port}",
            running = _server.Active,
            started = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
        });
    }
    #endregion

    #region 黑名单管理
    /// <summary>获取黑名单列表</summary>
    [HttpGet("blacklist")]
    public IActionResult GetBlacklist()
    {
        var authResult = CheckAuth();
        if (authResult != null) return authResult;

        var list = _server.Blacklist.GetAll();
        return Ok(new { total = list.Length, items = list });
    }

    /// <summary>添加手机号到黑名单</summary>
    [HttpPost("blacklist")]
    public IActionResult AddBlacklist([FromBody] BlacklistRequest request)
    {
        var authResult = CheckAuth();
        if (authResult != null) return authResult;

        if (String.IsNullOrEmpty(request.Mobile))
            return BadRequest(new { success = false, message = "缺少手机号" });

        var result = _server.Blacklist.Add(request.Mobile);
        return Ok(new { success = result, message = result ? "已加入黑名单" : "已在黑名单中" });
    }

    /// <summary>从黑名单移除</summary>
    [HttpDelete("blacklist/{mobile}")]
    public IActionResult RemoveBlacklist(String mobile)
    {
        var authResult = CheckAuth();
        if (authResult != null) return authResult;

        var result = _server.Blacklist.Remove(mobile);
        return Ok(new { success = result, message = result ? "已移出黑名单" : "不在黑名单中" });
    }
    #endregion

    #region 辅助
    private static Object? CreateMessageBody(String? bodyType, Dictionary<String, Object> body)
    {
        if (String.IsNullOrEmpty(bodyType)) return null;

        var type = Type.GetType(bodyType);
        if (type == null) return null;

        var json = NewLife.Serialization.JsonHelper.ToJson(body);
        return NewLife.Serialization.JsonHelper.ToJsonEntity(json, type);
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
        public Dictionary<String, Object> Body { get; set; } = null!;
    }

    /// <summary>黑名单操作请求</summary>
    public class BlacklistRequest
    {
        /// <summary>终端手机号</summary>
        public String Mobile { get; set; } = null!;
    }
    #endregion
}
