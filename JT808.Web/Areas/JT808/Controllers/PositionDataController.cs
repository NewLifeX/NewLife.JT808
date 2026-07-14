using Microsoft.AspNetCore.Mvc;
using JT808.Data;
using NewLife;
using NewLife.Cube;
using NewLife.Cube.Extensions;
using NewLife.Cube.ViewModels;
using NewLife.Log;
using NewLife.Web;
using XCode.Membership;
using static JT808.Data.PositionData;

namespace JT808.Web.Areas.JT808.Controllers;

/// <summary>位置数据。终端上报的位置信息</summary>
[Menu(70, true, Icon = "fa-table")]
[JT808Area]
public class PositionDataController : EntityController<PositionData>
{
    static PositionDataController()
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
        //    df.DataVisible = e => (e as PositionData).Devices > 0;
        //    df.Target = "_frame";
        //}
        //{
        //    var df = ListFields.GetField("Kind") as ListField;
        //    df.GetValue = e => ((Int32)(e as PositionData).Kind).ToString("X4");
        //}
        //ListFields.TraceUrl("TraceId");
    }

    //private readonly ITracer _tracer;

    //public PositionDataController(ITracer tracer)
    //{
    //    _tracer = tracer;
    //}

    /// <summary>高级搜索。列表页查询、导出Excel、导出Json、分享页等使用</summary>
    /// <param name="p">分页器。包含分页排序参数，以及Http请求参数</param>
    /// <returns></returns>
    protected override IEnumerable<PositionData> Search(Pager p)
    {
        var deviceId = p["deviceId"].ToInt(-1);
        var mobile = p["mobile"];
        var gPSTime = p["gPSTime"].ToDateTime();

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return PositionData.Search(deviceId, mobile, gPSTime, start, end, p["Q"], p);
    }
}