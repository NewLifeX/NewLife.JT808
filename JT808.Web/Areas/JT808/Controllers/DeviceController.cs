using Microsoft.AspNetCore.Mvc;
using JT808.Data;
using JT808.Web.Services;
using NewLife;
using NewLife.Cube;
using NewLife.Cube.Extensions;
using NewLife.Cube.ViewModels;
using NewLife.JT808.Protocols;
using NewLife.Log;
using NewLife.Web;
using XCode.Membership;
using static JT808.Data.Device;

namespace JT808.Web.Areas.JT808.Controllers;

/// <summary>终端设备。JT808终端设备信息</summary>
[Menu(130, true, Icon = "fa-table")]
[JT808Area]
public class DeviceController : EntityController<Device>
{
    static DeviceController()
    {
        //LogOnChange = true;

        //ListFields.RemoveField("Id", "Creator");
        ListFields.RemoveCreateField().RemoveRemarkField();

        //{
        //    var df = ListFields.GetField("Code") as ListField;
        //    df.Url = "?code={Code}";
        //    df.Target = "_blank";
        //}
        //{
        //    var df = ListFields.AddListField("devices", null, "Onlines");
        //    df.DisplayName = "查看设备";
        //    df.Url = "Device?groupId={Id}";
        //    df.DataVisible = e => (e as Device).Devices > 0;
        //    df.Target = "_frame";
        //}
        //{
        //    var df = ListFields.GetField("Kind") as ListField;
        //    df.GetValue = e => ((Int32)(e as Device).Kind).ToString("X4");
        //}
        //ListFields.TraceUrl("TraceId");

        // 添加指令下发链接列
        {
            var df = ListFields.AddListField("sendCommand", null, "Enable");
            df.DisplayName = "指令下发";
            df.Url = "/JT808/Device/SendCommand?id={Id}";
            df.Target = "_blank";
            df.DataVisible = e => (e as Device)?.Enable == true;
        }

        // 添加统计看板链接
        {
            var df = ListFields.AddListField("stats", null, "CreateTime");
            df.DisplayName = "统计看板";
            df.Url = "/JT808/Device/Stats";
            df.Target = "_blank";
        }
    }

    //private readonly ITracer _tracer;

    //public DeviceController(ITracer tracer)
    //{
    //    _tracer = tracer;
    //}

    #region 指令下发
    /// <summary>指令下发页面</summary>
    [EntityAuthorize(PermissionFlags.Update)]
    public async Task<ActionResult> SendCommand(Int32 id, String? type = null, String? data = null)
    {
        var device = Device.FindByKey(id);
        if (device == null) return Content("设备不存在");

        // POST 请求执行下发
        if ("POST".Equals(HttpContext.Request.Method, StringComparison.OrdinalIgnoreCase) && !type.IsNullOrEmpty())
        {
            try
            {
                var cluster = HttpContext.RequestServices.GetRequiredService<ClusterState>();
                var session = cluster.GetSession(device.Mobile);
                if (session == null) return Content("指令下发失败：设备不在线");

                // 统一传消息体类型全名，避免跨进程按程序集限定名解析失败
                var bodyType = MessageFactory.FindType(type)?.FullName ?? type;

                var client = HttpContext.RequestServices.GetRequiredService<CommandClient>();
                var success = client.SendCommand(session.Node, device.Mobile, bodyType, data ?? "{}");
                if (success)
                {
                    XTrace.WriteLine("Web 指令已下发：Mobile={0}, Type={1}, Node={2}", device.Mobile, bodyType, session.Node);
                    return RedirectToAction("Index", new { result = "指令已下发" });
                }
                return Content("指令下发失败：网关队列不可用");
            }
            catch (Exception ex)
            {
                return Content($"指令下发异常：{ex.Message}");
            }
        }

        // GET 请求显示下发页面
        var html = $@"<html><head><meta charset='utf-8'/><title>指令下发 - {device.Mobile}</title>
<style>body{{font-family:'Microsoft YaHei';margin:20px}} label{{display:block;margin-top:10px}} select,input,textarea{{width:100%;padding:8px;margin-top:5px}} .btn{{background:#337ab7;color:#fff;padding:10px 20px;border:none;cursor:pointer}} .btn:hover{{background:#286090}}</style>
</head><body>
<h2>指令下发</h2>
<p>终端：<strong>{device.Mobile}</strong> （{device.PlateNo}）</p>
<form method='post' action='/JT808/Device/SendCommand?id={id}'>
<label>指令类型</label>
<select name='type'>
<option value='NewLife.JT808.Models.T8300,NewLife.JT808'>文本信息下发</option>
<option value='NewLife.JT808.Models.T8103,NewLife.JT808'>设置终端参数</option>
<option value='NewLife.JT808.Models.T8201,NewLife.JT808'>位置信息查询</option>
<option value='NewLife.JT808.Models.T8203,NewLife.JT808'>临时位置跟踪</option>
</select>
<label>指令数据（JSON）</label>
<textarea name='data' rows='8'>{{}}</textarea>
<div style='margin-top:15px'><button type='submit' class='btn'>下发指令</button></div>
</form>
<p style='margin-top:20px;color:#888'>通过 Redis 队列发送到 JT808.Server，由服务器下发到终端设备。</p>
</body></html>";
        return Content(html, "text/html");
    }
    #endregion

    #region 统计看板
    /// <summary>设备统计看板</summary>
    [EntityAuthorize(PermissionFlags.Detail)]
    public ActionResult Stats()
    {
        var totalDevices = Device.FindCount();
        var onlineDevices = DeviceOnline.FindCount(DeviceOnline._.Online == true);
        var todayStart = DateTime.Today;
        var todayPositions = PositionData.FindCount(PositionData._.CreateTime >= todayStart);
        var todayAlarms = Alarm.FindCount(Alarm._.CreateTime >= todayStart);
        var totalCommands = DeviceCommand.FindCount();

        var html = $@"<html><head><meta charset='utf-8'/><title>设备统计看板</title>
<style>body{{font-family:'Microsoft YaHei';margin:20px;background:#f5f5f5}}
.card{{background:#fff;border-radius:8px;padding:20px;margin:15px;display:inline-block;min-width:200px;box-shadow:0 2px 8px rgba(0,0,0,.1)}}
.card h3{{margin:0 0 10px;color:#666;font-size:14px}}
.card .num{{font-size:36px;font-weight:bold;color:#337ab7}}
.card .num.green{{color:#5cb85c}}
.card .num.orange{{color:#f0ad4e}}
.card .num.red{{color:#d9534f}}
table{{border-collapse:collapse;width:100%;margin-top:20px}}
td,th{{padding:8px 12px;text-align:left;border-bottom:1px solid #eee}}
th{{background:#fafafa;font-weight:bold}}
a{{color:#337ab7;text-decoration:none}}
h2{{color:#333}}
</style></head><body>
<h2>📊 设备统计看板</h2>
<div class='card'><h3>📱 设备总数</h3><div class='num'>{totalDevices}</div></div>
<div class='card'><h3>🟢 在线设备</h3><div class='num green'>{onlineDevices}</div></div>
<div class='card'><h3>📍 今日位置</h3><div class='num orange'>{todayPositions}</div></div>
<div class='card'><h3>🚨 今日报警</h3><div class='num red'>{todayAlarms}</div></div>
<div class='card'><h3>📨 指令总数</h3><div class='num'>{totalCommands}</div></div>
<p style='margin-top:30px'><a href='/JT808/Device/Index'>← 返回设备列表</a></p>
</body></html>";
        return Content(html, "text/html");
    }
    #endregion

    /// <summary>高级搜索。列表页查询、导出Excel、导出Json、分享页等使用</summary>
    /// <param name="p">分页器。包含分页排序参数，以及Http请求参数</param>
    /// <returns></returns>
    protected override IEnumerable<Device> Search(Pager p)
    {
        var plateNo = p["plateNo"];
        var productId = p["productId"].ToInt(-1);
        var code = p["code"];
        var enable = p["enable"]?.ToBoolean();
        var key = p["Q"];

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return Device.Search(plateNo, productId, code, enable, start, end, key, p);
    }
}