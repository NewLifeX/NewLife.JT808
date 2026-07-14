using Microsoft.AspNetCore.Mvc;
using JT808.Data;
using NewLife;
using NewLife.Cube;
using NewLife.Cube.Extensions;
using NewLife.Cube.ViewModels;
using NewLife.Log;
using NewLife.Web;
using XCode.Membership;
using static JT808.Data.MediaRecord;

namespace JT808.Web.Areas.JT808.Controllers;

/// <summary>媒体记录。多媒体文件上传记录</summary>
[Menu(30, true, Icon = "fa-table")]
[JT808Area]
public class MediaRecordController : EntityController<MediaRecord>
{
    static MediaRecordController()
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
        //    df.DataVisible = e => (e as MediaRecord).Devices > 0;
        //    df.Target = "_frame";
        //}
        //{
        //    var df = ListFields.GetField("Kind") as ListField;
        //    df.GetValue = e => ((Int32)(e as MediaRecord).Kind).ToString("X4");
        //}
        //ListFields.TraceUrl("TraceId");
    }

    //private readonly ITracer _tracer;

    //public MediaRecordController(ITracer tracer)
    //{
    //    _tracer = tracer;
    //}

    /// <summary>高级搜索。列表页查询、导出Excel、导出Json、分享页等使用</summary>
    /// <param name="p">分页器。包含分页排序参数，以及Http请求参数</param>
    /// <returns></returns>
    protected override IEnumerable<MediaRecord> Search(Pager p)
    {
        var deviceId = p["deviceId"].ToInt(-1);
        var createTime = p["createTime"].ToDateTime();

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return MediaRecord.Search(deviceId, createTime, start, end, p["Q"], p);
    }
}