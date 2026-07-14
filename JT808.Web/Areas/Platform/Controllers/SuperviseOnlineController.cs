using Microsoft.AspNetCore.Mvc;
using JT808.Data.Platform;
using NewLife;
using NewLife.Cube;
using NewLife.Cube.Extensions;
using NewLife.Cube.ViewModels;
using NewLife.Log;
using NewLife.Web;
using XCode.Membership;
using static JT808.Data.Platform.SuperviseOnline;

namespace JT808.Web.Areas.Platform.Controllers;

/// <summary>监管在线。监管平台在线状态</summary>
[Menu(30, true, Icon = "fa-table")]
[PlatformArea]
public class SuperviseOnlineController : EntityController<SuperviseOnline>
{
    static SuperviseOnlineController()
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
        //    df.DataVisible = e => (e as SuperviseOnline).Devices > 0;
        //    df.Target = "_frame";
        //}
        //{
        //    var df = ListFields.GetField("Kind") as ListField;
        //    df.GetValue = e => ((Int32)(e as SuperviseOnline).Kind).ToString("X4");
        //}
        //ListFields.TraceUrl("TraceId");
    }

    //private readonly ITracer _tracer;

    //public SuperviseOnlineController(ITracer tracer)
    //{
    //    _tracer = tracer;
    //}

    /// <summary>高级搜索。列表页查询、导出Excel、导出Json、分享页等使用</summary>
    /// <param name="p">分页器。包含分页排序参数，以及Http请求参数</param>
    /// <returns></returns>
    protected override IEnumerable<SuperviseOnline> Search(Pager p)
    {
        var serverId = p["serverId"].ToInt(-1);
        var online = p["online"]?.ToBoolean();

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return SuperviseOnline.Search(serverId, online, start, end, p["Q"], p);
    }
}