using Microsoft.AspNetCore.Mvc;
using JT808.Data;
using NewLife;
using NewLife.Cube;
using NewLife.Cube.Extensions;
using NewLife.Cube.ViewModels;
using NewLife.Log;
using NewLife.Web;
using XCode.Membership;
using static JT808.Data.DeviceCommand;

namespace JT808.Web.Areas.JT808.Controllers;

/// <summary>指令记录。平台下发给终端的指令</summary>
[Menu(90, true, Icon = "fa-table")]
[JT808Area]
public class DeviceCommandController : EntityController<DeviceCommand>
{
    static DeviceCommandController()
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
        //    df.DataVisible = e => (e as DeviceCommand).Devices > 0;
        //    df.Target = "_frame";
        //}
        //{
        //    var df = ListFields.GetField("Kind") as ListField;
        //    df.GetValue = e => ((Int32)(e as DeviceCommand).Kind).ToString("X4");
        //}
        //ListFields.TraceUrl("TraceId");
    }

    //private readonly ITracer _tracer;

    //public DeviceCommandController(ITracer tracer)
    //{
    //    _tracer = tracer;
    //}

    /// <summary>高级搜索。列表页查询、导出Excel、导出Json、分享页等使用</summary>
    /// <param name="p">分页器。包含分页排序参数，以及Http请求参数</param>
    /// <returns></returns>
    protected override IEnumerable<DeviceCommand> Search(Pager p)
    {
        var deviceId = p["deviceId"].ToInt(-1);
        var mobile = p["mobile"];
        var createTime = p["createTime"].ToDateTime();

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return DeviceCommand.Search(deviceId, mobile, createTime, start, end, p["Q"], p);
    }
}