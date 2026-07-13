using System.Reactive;
using System.Reactive.Linq;
using Client.Avalonia.Services;
using NewLife.JT808.Models;
using NewLife.JT808.Protocols;
using ReactiveUI;

namespace Client.Avalonia.ViewModels;

/// <summary>终端模拟 ViewModel — 注册/鉴权/位置上报/消息发送</summary>
public class DeviceViewModel : ViewModelBase
{
    private readonly IConnectionService _service;
    private readonly MessageLogViewModel _log;

    #region 注册与鉴权属性
    private String _province = "44";
    private String _city = "4403";
    private String _manufacturer = "NewLife";
    private String _deviceModel = "JT808-Test";
    private String _deviceId = "TEST001";
    private String _plateNo = "粤B12345";
    private String _authCode = "";
    private String _registerStatus = "";

    public String Province { get => _province; set => this.RaiseAndSetIfChanged(ref _province, value); }
    public String City { get => _city; set => this.RaiseAndSetIfChanged(ref _city, value); }
    public String Manufacturer { get => _manufacturer; set => this.RaiseAndSetIfChanged(ref _manufacturer, value); }
    public String DeviceModel { get => _deviceModel; set => this.RaiseAndSetIfChanged(ref _deviceModel, value); }
    public String DeviceId { get => _deviceId; set => this.RaiseAndSetIfChanged(ref _deviceId, value); }
    public String PlateNo { get => _plateNo; set => this.RaiseAndSetIfChanged(ref _plateNo, value); }
    public String AuthCode { get => _authCode; set => this.RaiseAndSetIfChanged(ref _authCode, value); }
    public String RegisterStatus { get => _registerStatus; set => this.RaiseAndSetIfChanged(ref _registerStatus, value); }
    #endregion

    #region 位置上报属性
    private String _latitude = "22650000";
    private String _longitude = "114030000";
    private String _altitude = "50";
    private String _speed = "0";
    private String _direction = "180";
    private Boolean _timerEnabled;
    private String _timerInterval = "30";
    private IDisposable? _timer;

    public String Latitude { get => _latitude; set => this.RaiseAndSetIfChanged(ref _latitude, value); }
    public String Longitude { get => _longitude; set => this.RaiseAndSetIfChanged(ref _longitude, value); }
    public String Altitude { get => _altitude; set => this.RaiseAndSetIfChanged(ref _altitude, value); }
    public String Speed { get => _speed; set => this.RaiseAndSetIfChanged(ref _speed, value); }
    public String Direction { get => _direction; set => this.RaiseAndSetIfChanged(ref _direction, value); }
    public Boolean TimerEnabled { get => _timerEnabled; set => this.RaiseAndSetIfChanged(ref _timerEnabled, value); }
    public String TimerInterval { get => _timerInterval; set => this.RaiseAndSetIfChanged(ref _timerInterval, value); }
    #endregion

    #region 通用消息发送
    private String _selectedMessageKind;
    private String _messageBodyJson = "{}";
    private Boolean _waitResponse;

    public String SelectedMessageKind
    {
        get => _selectedMessageKind;
        set => this.RaiseAndSetIfChanged(ref _selectedMessageKind, value);
    }

    public String MessageBodyJson
    {
        get => _messageBodyJson;
        set => this.RaiseAndSetIfChanged(ref _messageBodyJson, value);
    }

    public Boolean WaitResponse
    {
        get => _waitResponse;
        set => this.RaiseAndSetIfChanged(ref _waitResponse, value);
    }
    #endregion

    /// <summary>所有可用的消息类型列表</summary>
    public List<String> MessageKindNames { get; } = Enum.GetNames<MessageKinds>().OrderBy(e => e).ToList();

    public ReactiveCommand<Unit, Unit> SendRegisterCommand { get; }
    public ReactiveCommand<Unit, Unit> SendAuthCommand { get; }
    public ReactiveCommand<Unit, Unit> SendLocationCommand { get; }
    public ReactiveCommand<Unit, Unit> SendHeartbeatCommand { get; }
    public ReactiveCommand<Unit, Unit> SendMessageCommand { get; }
    public ReactiveCommand<Unit, Unit> ToggleTimerCommand { get; }

    public DeviceViewModel(ConnectionViewModel connection, MessageLogViewModel log)
    {
        _service = connection.Service;
        _log = log;
        _selectedMessageKind = "终端心跳";

        SendRegisterCommand = ReactiveCommand.Create(SendRegister);
        SendAuthCommand = ReactiveCommand.Create(SendAuth);
        SendLocationCommand = ReactiveCommand.Create(SendLocation);
        SendHeartbeatCommand = ReactiveCommand.Create(SendHeartbeat);
        SendMessageCommand = ReactiveCommand.Create(SendMessage);
        ToggleTimerCommand = ReactiveCommand.Create(ToggleTimer);

        this.WhenAnyValue(x => x.TimerEnabled)
            .Subscribe(enabled =>
            {
                if (enabled) StartTimer();
                else StopTimer();
            });
    }

    private void SendRegister()
    {
        var body = new T0100
        {
            Province = UInt16.TryParse(Province, out var p) ? p : (UInt16)0,
            City = UInt16.TryParse(City, out var c) ? c : (UInt16)0,
            Provider = Manufacturer,
            Model = DeviceModel,
            DeviceId = DeviceId,
            Color = CarColors.白色,
            PlateNo = PlateNo,
        };
        _service.Send(body);
        RegisterStatus = "已发送注册请求，等待应答...";
    }

    private void SendAuth()
    {
        if (String.IsNullOrEmpty(AuthCode))
        {
            RegisterStatus = "请先输入鉴权码";
            return;
        }
        _service.Send(new T0102 { Code = AuthCode });
        RegisterStatus = "鉴权已发送";
    }

    private void SendLocation()
    {
        var body = new T0200
        {
            Alarm = 0,
            Status = 0,
            Latitude = Int32.TryParse(Latitude, out var lat) ? lat : 0,
            Longitude = Int32.TryParse(Longitude, out var lng) ? lng : 0,
            Altitude = UInt16.TryParse(Altitude, out var alt) ? alt : (UInt16)0,
            Speed = UInt16.TryParse(Speed, out var spd) ? spd : (UInt16)0,
            Direction = UInt16.TryParse(Direction, out var dir) ? dir : (UInt16)0,
            GPSTime = DateTime.Now,
        };
        _service.Send(body);
    }

    private void SendHeartbeat()
    {
        _service.Send(new JTMessage { Kind = MessageKinds.终端心跳 });
    }

    private void SendMessage()
    {
        // 使用通用 JSON 方式发送（暂支持简单方式，后续完善）
        if (!Enum.TryParse<MessageKinds>(SelectedMessageKind, out var kind)) return;

        // 目前仅发送一个空的消息体
        var msg = new JTMessage { Kind = kind };
        _service.Send(msg);
    }

    private void ToggleTimer()
    {
        TimerEnabled = !TimerEnabled;
    }

    private void StartTimer()
    {
        StopTimer();
        var interval = Int32.TryParse(TimerInterval, out var sec) ? sec : 30;
        _timer = Observable.Interval(TimeSpan.FromSeconds(interval))
            .Subscribe(_ => SendLocation());
    }

    private void StopTimer()
    {
        _timer?.Dispose();
        _timer = null;
    }
}
