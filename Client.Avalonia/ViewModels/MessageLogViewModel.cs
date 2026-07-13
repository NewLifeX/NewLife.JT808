using System.Collections.ObjectModel;
using System.Reactive;
using Client.Avalonia.Models;
using NewLife;
using NewLife.Data;
using NewLife.JT808.Protocols;
using ReactiveUI;

namespace Client.Avalonia.ViewModels;

/// <summary>消息日志 ViewModel — 实时显示收发消息</summary>
public class MessageLogViewModel : ViewModelBase
{
    private readonly ObservableCollection<MessageLogEntry> _entries = new();
    private MessageLogEntry? _selectedEntry;

    /// <summary>日志条目列表</summary>
    public ObservableCollection<MessageLogEntry> Entries => _entries;

    /// <summary>当前选中的条目</summary>
    public MessageLogEntry? SelectedEntry
    {
        get => _selectedEntry;
        set => this.RaiseAndSetIfChanged(ref _selectedEntry, value);
    }

    /// <summary>清空日志命令</summary>
    public ReactiveCommand<Unit, Unit> ClearCommand { get; }

    public MessageLogViewModel()
    {
        ClearCommand = ReactiveCommand.Create(() => _entries.Clear());
    }

    /// <summary>添加日志条目</summary>
    public void AddEntry(JTMessage msg, String direction)
    {
        var hex = msg.Payload?.ToHex(256) ?? "";
        var json = msg.ToJsonJT808();

        var entry = new MessageLogEntry
        {
            Time = DateTime.Now,
            Direction = direction,
            Kind = msg.Kind,
            KindName = msg.Kind.ToString(),
            Mobile = msg.Mobile ?? "",
            Sequence = msg.Sequence,
            Length = msg.Payload?.Length ?? 0,
            HexDump = hex,
            JsonBody = json,
            Message = msg,
        };

        _entries.Insert(0, entry);

        // 限制最大条目数
        while (_entries.Count > 5000)
            _entries.RemoveAt(_entries.Count - 1);
    }
}
