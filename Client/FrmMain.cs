using NewLife;
using NewLife.Data;
using NewLife.Log;
using NewLife.JT808.Protocols;
using NewLife.JT808.Models;
using NewLife.JT808.Protocols;

namespace Client;

/// <summary>JT808测试客户端 — 主窗体</summary>
public partial class FrmMain : Form
{
    private readonly TextBox _txtHex;
    private readonly Button _btnDecode;
    private readonly PropertyGrid _gridResult;
    private readonly Button _btnClear;

    public FrmMain()
    {
        Text = "JT808报文解析工具";
        Size = new Size(900, 700);
        StartPosition = FormStartPosition.CenterScreen;

        // 初始化消息工厂
        MessageFactory.Instance.Init();

        // 布局
        var split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal };
        Controls.Add(split);

        // 上方面板（输入）
        var topPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
        split.Panel1.Controls.Add(topPanel);

        var lbl = new Label { Text = "粘贴十六进制报文（支持带空格和0x前缀）：", Dock = DockStyle.Top, Height = 25 };
        topPanel.Controls.Add(lbl);

        _txtHex = new TextBox { Dock = DockStyle.Fill, Multiline = true, Font = new Font("Consolas", 10) };
        topPanel.Controls.Add(_txtHex);

        var btnPanel = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 40, FlowDirection = FlowDirection.RightToLeft };
        _btnClear = new Button { Text = "清空", Width = 80 };
        _btnClear.Click += (s, e) => { _txtHex.Clear(); _gridResult.SelectedObject = null; };
        btnPanel.Controls.Add(_btnClear);

        _btnDecode = new Button { Text = "解码", Width = 80 };
        _btnDecode.Click += BtnDecode_Click;
        btnPanel.Controls.Add(_btnDecode);

        topPanel.Controls.Add(btnPanel);

        // 下方面板（结果）
        _gridResult = new PropertyGrid { Dock = DockStyle.Fill, PropertySort = PropertySort.Categorized };
        split.Panel2.Controls.Add(_gridResult);
    }

    private void BtnDecode_Click(Object sender, EventArgs e)
    {
        try
        {
            var hex = _txtHex.Text.Trim();
            if (String.IsNullOrEmpty(hex))
            {
                MessageBox.Show("请输入十六进制报文", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 清理格式：去除空格、0x前缀、换行
            hex = hex.Replace(" ", "")
                     .Replace("0x", "")
                     .Replace("0X", "")
                     .Replace("\r", "")
                     .Replace("\n", "")
                     .Replace("-", "");

            if (hex.Length % 2 != 0)
            {
                MessageBox.Show("十六进制字符串长度必须为偶数", "格式错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var data = hex.ToHex();
            var msg = new JTMessage();
            if (!msg.Read(new MemoryStream(data), null))
            {
                MessageBox.Show("解码失败，请检查报文格式", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 解析消息体
            var body = MessageFactory.Instance.Parse(msg);
            var result = new DecodeResult
            {
                Kind = msg.Kind,
                KindName = msg.Kind.ToString(),
                Mobile = msg.Mobile,
                Sequence = msg.Sequence,
                Version = msg.Version,
                Hex = msg.Payload?.ToHex(512) ?? "",
                Body = body?.ToJsonJT808() ?? "",
                RawMessage = msg.ToJsonJT808()
            };

            _gridResult.SelectedObject = result;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"解码出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

/// <summary>解码结果展示类</summary>
public class DecodeResult
{
    [System.ComponentModel.Category("基本信息")]
    [System.ComponentModel.DisplayName("消息ID")]
    public MessageKinds Kind { get; set; }

    [System.ComponentModel.Category("基本信息")]
    [System.ComponentModel.DisplayName("消息名称")]
    public String KindName { get; set; }

    [System.ComponentModel.Category("基本信息")]
    [System.ComponentModel.DisplayName("终端手机号")]
    public String Mobile { get; set; }

    [System.ComponentModel.Category("基本信息")]
    [System.ComponentModel.DisplayName("流水号")]
    public UInt16 Sequence { get; set; }

    [System.ComponentModel.Category("基本信息")]
    [System.ComponentModel.DisplayName("协议版本")]
    public Byte Version { get; set; }

    [System.ComponentModel.Category("消息体")]
    [System.ComponentModel.DisplayName("十六进制负载")]
    public String Hex { get; set; }

    [System.ComponentModel.Category("消息体")]
    [System.ComponentModel.DisplayName("JSON消息体")]
    public String Body { get; set; }

    [System.ComponentModel.Category("原始报文")]
    [System.ComponentModel.DisplayName("JSON原始")]
    public String RawMessage { get; set; }
}
