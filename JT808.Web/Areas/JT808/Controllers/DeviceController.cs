using Microsoft.AspNetCore.Mvc;
using JT808.Data;
using NewLife;
using NewLife.Cube;
using NewLife.Cube.Extensions;
using NewLife.Cube.ViewModels;
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
    }

    //private readonly ITracer _tracer;

    //public DeviceController(ITracer tracer)
    //{
    //    _tracer = tracer;
    //}

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