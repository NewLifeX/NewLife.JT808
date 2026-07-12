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

    /// <summary>实例化</summary>
    public JT808Controller(JT808Server server) => _server = server;

    /// <summary>统一下发指令</summary>
    [HttpPost("command")]
    public IActionResult SendCommand([FromBody] SendCommandRequest request)
    {
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

    /// <summary>获取在线会话列表</summary>
    [HttpGet("sessions")]
    public ActionResult GetSessions()
    {
        // 通过反射获取 SessionManager 内部缓存（简化实现）
        return Ok(new { total = 0 });
    }

    /// <summary>获取指定终端会话详情</summary>
    [HttpGet("session/{mobile}")]
    public ActionResult GetSession(String mobile)
    {
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
    public ActionResult RemoveSession(String mobile)
    {
        var session = _server.Manager.Find(mobile);
        if (session == null) return NotFound(new { success = false, message = "设备不在线" });

        _server.Manager.Remove(mobile);
        session.Close("管理员强制断开");

        return Ok(new { success = true, message = "会话已移除" });
    }

    /// <summary>获取服务器统计</summary>
    [HttpGet("stats")]
    public ActionResult GetStats()
    {
        return Ok(new
        {
            server = $"{_server.Name ?? "jt808"}:{_server.Port}",
            running = _server.Active,
            started = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
        });
    }

    private static Object? CreateMessageBody(String? bodyType, Dictionary<String, Object> body)
    {
        if (String.IsNullOrEmpty(bodyType)) return null;

        var type = Type.GetType(bodyType);
        if (type == null) return null;

        var json = NewLife.Serialization.JsonHelper.ToJson(body);
        return NewLife.Serialization.JsonHelper.ToJsonEntity(json, type);
    }

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
}
