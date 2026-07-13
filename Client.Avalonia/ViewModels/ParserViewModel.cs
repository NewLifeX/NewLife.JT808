using System.Reactive;
using NewLife;
using NewLife.Data;
using NewLife.JT808.Protocols;
using ReactiveUI;

namespace Client.Avalonia.ViewModels;

/// <summary>指令解析 ViewModel — 十六进制报文解码</summary>
public class ParserViewModel : ViewModelBase
{
    private String _hexInput = "";
    private String _statusText = "等待输入";
    private Boolean _hasResult;
    private String _resultKind = "";
    private String _resultKindName = "";
    private String _resultMobile = "";
    private String _resultSequence = "";
    private String _resultVersion = "";
    private String _resultHex = "";
    private String _resultBody = "";
    private String _resultRaw = "";
    private String _resultFieldList = "";

    /// <summary>十六进制输入</summary>
    public String HexInput
    {
        get => _hexInput;
        set => this.RaiseAndSetIfChanged(ref _hexInput, value);
    }

    /// <summary>状态文本</summary>
    public String StatusText
    {
        get => _statusText;
        set => this.RaiseAndSetIfChanged(ref _statusText, value);
    }

    /// <summary>是否有解析结果</summary>
    public Boolean HasResult
    {
        get => _hasResult;
        set => this.RaiseAndSetIfChanged(ref _hasResult, value);
    }

    public String ResultKind { get => _resultKind; set => this.RaiseAndSetIfChanged(ref _resultKind, value); }
    public String ResultKindName { get => _resultKindName; set => this.RaiseAndSetIfChanged(ref _resultKindName, value); }
    public String ResultMobile { get => _resultMobile; set => this.RaiseAndSetIfChanged(ref _resultMobile, value); }
    public String ResultSequence { get => _resultSequence; set => this.RaiseAndSetIfChanged(ref _resultSequence, value); }
    public String ResultVersion { get => _resultVersion; set => this.RaiseAndSetIfChanged(ref _resultVersion, value); }
    public String ResultHex { get => _resultHex; set => this.RaiseAndSetIfChanged(ref _resultHex, value); }
    public String ResultBody { get => _resultBody; set => this.RaiseAndSetIfChanged(ref _resultBody, value); }
    public String ResultRaw { get => _resultRaw; set => this.RaiseAndSetIfChanged(ref _resultRaw, value); }
    public String ResultFieldList { get => _resultFieldList; set => this.RaiseAndSetIfChanged(ref _resultFieldList, value); }

    /// <summary>解码命令</summary>
    public ReactiveCommand<Unit, Unit> DecodeCommand { get; }

    /// <summary>清空命令</summary>
    public ReactiveCommand<Unit, Unit> ClearCommand { get; }

    public ParserViewModel()
    {
        // 确保消息工厂已初始化
        MessageFactory.Instance.Init();

        DecodeCommand = ReactiveCommand.Create(DoDecode);
        ClearCommand = ReactiveCommand.Create(DoClear);
    }

    private void DoDecode()
    {
        try
        {
            var hex = HexInput.Trim();
            if (String.IsNullOrEmpty(hex))
            {
                StatusText = "请输入十六进制报文";
                return;
            }

            // 清理格式
            hex = hex.Replace(" ", "")
                     .Replace("0x", "")
                     .Replace("0X", "")
                     .Replace("\r", "")
                     .Replace("\n", "")
                     .Replace("-", "")
                     .Replace(" ", "");

            if (hex.Length % 2 != 0)
            {
                StatusText = "十六进制字符串长度必须为偶数";
                return;
            }

            var data = hex.ToHex();
            var msg = new JTMessage();
            if (!msg.Read(new MemoryStream(data), String.Empty))
            {
                StatusText = "解码失败，请检查报文格式";
                return;
            }

            // 解析消息体
            var body = MessageFactory.Instance.Parse(msg);

            ResultKind = ((UInt16)msg.Kind).ToString("X4");
            ResultKindName = msg.Kind.ToString();
            ResultMobile = msg.Mobile ?? "";
            ResultSequence = msg.Sequence.ToString();
            ResultVersion = msg.Version switch
            {
                1 => "2019 版",
                _ => "2011/2013 版"
            };
            ResultHex = msg.Payload?.ToHex(512) ?? "";
            ResultBody = body?.ToJsonJT808() ?? "（空消息体）";
            ResultRaw = msg.ToJsonJT808() ?? "";

            // 生成字段列表（从 JSON 中提取键值对）
            var fields = FormatFields(body);
            ResultFieldList = fields;

            HasResult = true;
            StatusText = "解码成功";
        }
        catch (Exception ex)
        {
            StatusText = $"解码出错：{ex.Message}";
            HasResult = false;
        }
    }

    private void DoClear()
    {
        HexInput = "";
        HasResult = false;
        ResultKind = "";
        ResultKindName = "";
        ResultMobile = "";
        ResultSequence = "";
        ResultVersion = "";
        ResultHex = "";
        ResultBody = "";
        ResultRaw = "";
        ResultFieldList = "";
        StatusText = "等待输入";
    }

    /// <summary>格式化消息体字段为可读列表</summary>
    private static String FormatFields(Object? body)
    {
        if (body == null) return "";

        var sb = new System.Text.StringBuilder();
        var type = body.GetType();
        var props = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        foreach (var prop in props)
        {
            if (prop.Name == "Parameter" || prop.Name == "Items") continue;
            var val = prop.GetValue(body);
            var display = val?.ToString() ?? "";
            sb.AppendLine($"{prop.Name}: {display}");
        }

        return sb.ToString();
    }
}
