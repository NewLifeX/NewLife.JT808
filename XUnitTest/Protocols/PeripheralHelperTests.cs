using System.ComponentModel;
using NewLife.JT808.SuBiao;

namespace XUnitTest.Protocols;

/// <summary>外设状态/信息解析单元测试</summary>
public class PeripheralHelperTests
{
    [Fact]
    [DisplayName("外设状态_正常解析")]
    public void ReadStatus_Normal()
    {
        // count=1, kind=0x14, len=5, status=0x01, alarm=0x00000003
        var data = new Byte[] { 0x01, 0x14, 0x05, 0x01, 0x00, 0x00, 0x00, 0x03 };

        var dic = PeripheralHelper.ReadStatus(data);

        Assert.NotNull(dic);
        var status = dic![PeripheralKinds.VideoAlarm];
        Assert.Equal(PeripheralWorkStatus.正常工作, status.Status);
        Assert.True(status.Alarm.HasFlag(PeripheralAlarmStatus.摄像头));
        Assert.True(status.Alarm.HasFlag(PeripheralAlarmStatus.主存储器));
    }

    [Fact]
    [DisplayName("外设状态_长度不足不抛异常")]
    public void ReadStatus_ShortData_NoThrow()
    {
        // 声明长度2，仅1字节状态，缺少4字节报警
        var data = new Byte[] { 0x01, 0x14, 0x02, 0x04, 0x00 };

        var dic = PeripheralHelper.ReadStatus(data);

        Assert.NotNull(dic);
        var status = dic![PeripheralKinds.VideoAlarm];
        Assert.Equal(PeripheralWorkStatus.设备异常, status.Status);
        Assert.Equal((PeripheralAlarmStatus)0, status.Alarm);
    }

    [Fact]
    [DisplayName("外设状态_空数据返回null")]
    public void ReadStatus_Empty_ReturnsNull()
    {
        Assert.Null(PeripheralHelper.ReadStatus(null));
        Assert.Null(PeripheralHelper.ReadStatus([]));
        Assert.Null(PeripheralHelper.ReadStatus([0x00]));
    }
}
