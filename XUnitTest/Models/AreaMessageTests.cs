using System.ComponentModel;
using NewLife.JT808.Models;
using NewLife.JT808.Protocols;

namespace XUnitTest.Models;

/// <summary>区域与路线设置消息体测试（含行标字节样例）</summary>
public class AreaMessageTests
{
    #region 0x8600 圆形区域
    [Fact]
    [DisplayName("T8600_2013版_字节布局与行标一致")]
    public void T8600_Version2013_Layout()
    {
        var body = new T8600
        {
            SettingAreaProperty = 1,
            Areas =
            [
                new CircleArea
                {
                    AreaId = 100,
                    AreaProperty = 2,
                    Latitude = 31000562,
                    Longitude = 121451372,
                    Radius = 2345,
                    StartTime = new DateTime(2023, 4, 17, 22, 16, 33),
                    EndTime = new DateTime(2023, 4, 17, 22, 16, 55),
                    MaxSpeed = 77,
                    OverspeedDuration = 111,
                },
                new CircleArea
                {
                    AreaId = 101,
                    AreaProperty = 7,
                    Latitude = 31000562,
                    Longitude = 121451372,
                    Radius = 2345,
                    StartTime = new DateTime(2023, 4, 17, 22, 16, 33),
                    EndTime = new DateTime(2023, 4, 17, 23, 16, 55),
                    MaxSpeed = 88,
                    OverspeedDuration = 222,
                },
            ],
        };

        var hex = WriteBody(body, 0);

        Assert.Equal("010200000064000201D907F2073D336C00000929004D6F00000065000701D907F2073D336C000009292304172216332304172316550058DE", hex);
    }

    [Fact]
    [DisplayName("T8600_2019版_字节布局与行标一致")]
    public void T8600_Version2019_Layout()
    {
        var body = new T8600
        {
            SettingAreaProperty = 1,
            Areas =
            [
                new CircleArea
                {
                    AreaId = 100,
                    AreaProperty = 2,
                    Latitude = 31000562,
                    Longitude = 121451372,
                    Radius = 2345,
                    MaxSpeed = 77,
                    OverspeedDuration = 111,
                    NightMaxSpeed = 33,
                    AreaName = "区域α",
                },
                new CircleArea
                {
                    AreaId = 101,
                    AreaProperty = 7,
                    Latitude = 31000562,
                    Longitude = 121451372,
                    Radius = 2345,
                    StartTime = new DateTime(2023, 4, 17, 22, 16, 33),
                    EndTime = new DateTime(2023, 4, 17, 23, 16, 55),
                    MaxSpeed = 88,
                    OverspeedDuration = 222,
                    NightMaxSpeed = 33,
                    AreaName = "区域β",
                },
            ],
        };

        var hex = WriteBody(body, 1);

        Assert.Equal("010200000064000201D907F2073D336C00000929004D6F00210006C7F8D3F2A6C100000065000701D907F2073D336C000009292304172216332304172316550058DE00210006C7F8D3F2A6C2", hex);
    }

    [Fact]
    [DisplayName("T8600_无条件字段_按属性位省略")]
    public void T8600_NoConditionalFields()
    {
        var body = new T8600
        {
            SettingAreaProperty = 0,
            Areas =
            [
                new CircleArea
                {
                    AreaId = 100,
                    AreaProperty = 0,
                    Latitude = 31000562,
                    Longitude = 121451372,
                    Radius = 2345,
                    StartTime = new DateTime(2023, 4, 17, 22, 16, 33),
                    MaxSpeed = 77,
                },
            ],
        };

        var hex = WriteBody(body, 0);

        Assert.Equal("000100000064000001D907F2073D336C00000929", hex);
    }
    [Fact]
    [DisplayName("T8600_2019版_往返解析")]
    public void T8600_Version2019_RoundTrip()
    {
        var body = new T8600
        {
            SettingAreaProperty = 2,
            Areas =
            [
                new CircleArea
                {
                    AreaId = 100,
                    AreaProperty = 3,
                    Latitude = 31000562,
                    Longitude = 121451372,
                    Radius = 2345,
                    StartTime = new DateTime(2023, 4, 17, 22, 16, 33),
                    EndTime = new DateTime(2023, 4, 17, 23, 16, 55),
                    MaxSpeed = 88,
                    OverspeedDuration = 10,
                    NightMaxSpeed = 60,
                    AreaName = "区域α",
                },
            ],
        };

        using var ms = new MemoryStream();
        var header = new JTMessage { Version = 1 };
        Assert.True(body.Write(ms, header));
        ms.Position = 0;

        var result = new T8600();
        Assert.True(result.Read(ms, header));

        Assert.Equal((Byte)2, result.SettingAreaProperty);
        var area = Assert.Single(result.Areas);
        Assert.Equal(100u, area.AreaId);
        Assert.Equal((UInt16)3, area.AreaProperty);
        Assert.Equal(31000562u, area.Latitude);
        Assert.Equal(121451372u, area.Longitude);
        Assert.Equal(2345u, area.Radius);
        Assert.Equal(new DateTime(2023, 4, 17, 22, 16, 33), area.StartTime);
        Assert.Equal(new DateTime(2023, 4, 17, 23, 16, 55), area.EndTime);
        Assert.Equal((UInt16)88, area.MaxSpeed);
        Assert.Equal((Byte)10, area.OverspeedDuration);
        Assert.Equal((UInt16)60, area.NightMaxSpeed);
        Assert.Equal("区域α", area.AreaName);
    }
    #endregion

    #region 0x8602 矩形区域
    [Fact]
    [DisplayName("T8602_2013版_字节布局与行标一致")]
    public void T8602_Version2013_Layout()
    {
        var body = new T8602
        {
            SettingAreaProperty = 1,
            Areas =
            [
                new RectangleArea
                {
                    AreaId = 100,
                    AreaProperty = 2,
                    LeftTopLatitude = 31235930,
                    LeftTopLongitude = 121480540,
                    RightBottomLatitude = 31198320,
                    RightBottomLongitude = 121547960,
                    MaxSpeed = 77,
                    OverspeedDuration = 111,
                },
                new RectangleArea
                {
                    AreaId = 101,
                    AreaProperty = 7,
                    LeftTopLatitude = 31235930,
                    LeftTopLongitude = 121480540,
                    RightBottomLatitude = 31198320,
                    RightBottomLongitude = 121547960,
                    StartTime = new DateTime(2023, 4, 17, 22, 16, 33),
                    EndTime = new DateTime(2023, 4, 18, 22, 22, 12),
                    MaxSpeed = 88,
                    OverspeedDuration = 222,
                },
            ],
        };

        var hex = WriteBody(body, 0);

        Assert.Equal("010200000064000201DC9F5A073DA55C01DC0C70073EACB8004D6F00000065000701DC9F5A073DA55C01DC0C70073EACB82304172216332304182222120058DE", hex);
    }

    [Fact]
    [DisplayName("T8602_2019版_字节布局与行标一致")]
    public void T8602_Version2019_Layout()
    {
        var body = new T8602
        {
            SettingAreaProperty = 1,
            Areas =
            [
                new RectangleArea
                {
                    AreaId = 100,
                    AreaProperty = 1,
                    LeftTopLatitude = 31235930,
                    LeftTopLongitude = 121480540,
                    RightBottomLatitude = 31198320,
                    RightBottomLongitude = 121547960,
                    StartTime = new DateTime(2023, 4, 17, 22, 16, 33),
                    EndTime = new DateTime(2023, 4, 18, 22, 22, 12),
                    AreaName = "区域A",
                },
                new RectangleArea
                {
                    AreaId = 101,
                    AreaProperty = 7,
                    LeftTopLatitude = 31235930,
                    LeftTopLongitude = 121480540,
                    RightBottomLatitude = 31198320,
                    RightBottomLongitude = 121547960,
                    StartTime = new DateTime(2023, 4, 17, 22, 16, 33),
                    EndTime = new DateTime(2023, 4, 18, 22, 22, 12),
                    MaxSpeed = 88,
                    OverspeedDuration = 99,
                    NightMaxSpeed = 6768,
                    AreaName = "区域B",
                },
            ],
        };

        var hex = WriteBody(body, 1);

        Assert.Equal("010200000064000101DC9F5A073DA55C01DC0C70073EACB82304172216332304182222120005C7F8D3F24100000065000701DC9F5A073DA55C01DC0C70073EACB82304172216332304182222120058631A700005C7F8D3F242", hex);
    }

    [Fact]
    [DisplayName("T8602_2013版_往返解析")]
    public void T8602_Version2013_RoundTrip()
    {
        var body = new T8602
        {
            SettingAreaProperty = 0,
            Areas =
            [
                new RectangleArea
                {
                    AreaId = 101,
                    AreaProperty = 7,
                    LeftTopLatitude = 31235930,
                    LeftTopLongitude = 121480540,
                    RightBottomLatitude = 31198320,
                    RightBottomLongitude = 121547960,
                    StartTime = new DateTime(2023, 4, 17, 22, 16, 33),
                    EndTime = new DateTime(2023, 4, 18, 22, 22, 12),
                    MaxSpeed = 88,
                    OverspeedDuration = 222,
                },
            ],
        };

        using var ms = new MemoryStream();
        var header = new JTMessage { Version = 0 };
        Assert.True(body.Write(ms, header));
        ms.Position = 0;

        var result = new T8602();
        Assert.True(result.Read(ms, header));

        var area = Assert.Single(result.Areas);
        Assert.Equal(101u, area.AreaId);
        Assert.Equal((UInt16)7, area.AreaProperty);
        Assert.Equal(31235930u, area.LeftTopLatitude);
        Assert.Equal(121547960u, area.RightBottomLongitude);
        Assert.Equal(new DateTime(2023, 4, 17, 22, 16, 33), area.StartTime);
        Assert.Equal(new DateTime(2023, 4, 18, 22, 22, 12), area.EndTime);
        Assert.Equal((UInt16)88, area.MaxSpeed);
        Assert.Equal((Byte)222, area.OverspeedDuration);
        Assert.Null(area.AreaName);
    }
    #endregion

    #region 0x8604 多边形区域
    [Fact]
    [DisplayName("T8604_2013版_字节布局与行标一致")]
    public void T8604_Version2013_Layout()
    {
        var body = new T8604
        {
            AreaId = 100,
            AreaProperty = 2,
            StartTime = new DateTime(2023, 4, 17, 22, 16, 33),
            EndTime = new DateTime(2023, 4, 18, 22, 22, 12),
            HighestSpeed = 77,
            OverspeedDuration = 111,
            PeakItems =
            [
                new AreaVertex { Latitude = 31235930, Longitude = 121480540 },
                new AreaVertex { Latitude = 31198320, Longitude = 121547960 },
            ],
        };

        var hex = WriteBody(body, 0);

        Assert.Equal("000000640002004D6F000201DC9F5A073DA55C01DC0C70073EACB8", hex);
    }

    [Fact]
    [DisplayName("T8604_2019版_字节布局与行标一致")]
    public void T8604_Version2019_Layout()
    {
        var body = new T8604
        {
            AreaId = 100,
            AreaProperty = 2,
            HighestSpeed = 77,
            OverspeedDuration = 111,
            PeakItems =
            [
                new AreaVertex { Latitude = 31235930, Longitude = 121480540 },
                new AreaVertex { Latitude = 31198320, Longitude = 121547960 },
            ],
            NightMaximumSpeed = 666,
            AreaName = "区域名称abcdefg123",
        };

        var hex = WriteBody(body, 1);

        Assert.Equal("000000640002004D6F000201DC9F5A073DA55C01DC0C70073EACB8029A0012C7F8D3F2C3FBB3C661626364656667313233", hex);
    }

    [Fact]
    [DisplayName("T8604_往返_含条件字段与名称")]
    public void T8604_RoundTrip()
    {
        var body = new T8604
        {
            AreaId = 8,
            AreaProperty = 3,
            StartTime = new DateTime(2026, 10, 20, 8, 0, 0),
            EndTime = new DateTime(2026, 10, 20, 18, 0, 0),
            HighestSpeed = 80,
            OverspeedDuration = 10,
            NightMaximumSpeed = 60,
            AreaName = "围栏",
            PeakItems =
            [
                new AreaVertex { Latitude = 22334455, Longitude = 114112233 },
                new AreaVertex { Latitude = 22334456, Longitude = 114112234 },
                new AreaVertex { Latitude = 22334457, Longitude = 114112235 },
            ],
        };

        using var ms = new MemoryStream();
        var header = new JTMessage { Version = 1 };
        Assert.True(body.Write(ms, header));
        ms.Position = 0;

        var result = new T8604();
        Assert.True(result.Read(ms, header));

        Assert.Equal(8u, result.AreaId);
        Assert.Equal((UInt16)3, result.AreaProperty);
        Assert.Equal(new DateTime(2026, 10, 20, 8, 0, 0), result.StartTime);
        Assert.Equal(new DateTime(2026, 10, 20, 18, 0, 0), result.EndTime);
        Assert.Equal((UInt16)80, result.HighestSpeed);
        Assert.Equal((Byte)10, result.OverspeedDuration);
        Assert.Equal((UInt16)60, result.NightMaximumSpeed);
        Assert.Equal("围栏", result.AreaName);
        Assert.Equal((UInt16)3, result.PeakCount);
        Assert.Equal(3, result.PeakItems.Count);
        Assert.Equal(22334457u, result.PeakItems[2].Latitude);
    }
    #endregion

    #region 0x8606 设置路线
    [Fact]
    [DisplayName("T8606_2019版_条件字段往返")]
    public void T8606_Version2019_RoundTrip()
    {
        var body = new T8606
        {
            RouteId = 5,
            RouteProperty = 1,
            StartTime = new DateTime(2026, 10, 20, 9, 0, 0),
            EndTime = new DateTime(2026, 10, 20, 17, 0, 0),
            RouteName = "线路A",
            InflectionPointItems =
            [
                new RoutePoint
                {
                    InflectionPointId = 1,
                    SectionId = 11,
                    Latitude = 22334455,
                    Longitude = 114112233,
                    SectionWidth = 20,
                    SectionProperty = 0,
                },
                new RoutePoint
                {
                    InflectionPointId = 2,
                    SectionId = 12,
                    Latitude = 22334456,
                    Longitude = 114112234,
                    SectionWidth = 30,
                    SectionProperty = 3,
                    LongDrivingThreshold = 100,
                    ShortDrivingThreshold = 60,
                    HighestSpeed = 80,
                    OverspeedDuration = 5,
                    NightMaximumSpeed = 70,
                },
            ],
        };

        using var ms = new MemoryStream();
        var header = new JTMessage { Version = 1 };
        Assert.True(body.Write(ms, header));
        ms.Position = 0;

        var result = new T8606();
        Assert.True(result.Read(ms, header));

        Assert.Equal(5u, result.RouteId);
        Assert.Equal((UInt16)1, result.RouteProperty);
        Assert.Equal(new DateTime(2026, 10, 20, 9, 0, 0), result.StartTime);
        Assert.Equal("线路A", result.RouteName);
        Assert.Equal((UInt16)2, result.InflectionPointCount);
        Assert.Equal(2, result.InflectionPointItems.Count);

        var first = result.InflectionPointItems[0];
        Assert.Equal(0, first.SectionProperty);
        Assert.Null(first.LongDrivingThreshold);
        Assert.Null(first.HighestSpeed);

        var second = result.InflectionPointItems[1];
        Assert.Equal((Byte)3, second.SectionProperty);
        Assert.Equal((UInt16)100, second.LongDrivingThreshold);
        Assert.Equal((UInt16)60, second.ShortDrivingThreshold);
        Assert.Equal((UInt16)80, second.HighestSpeed);
        Assert.Equal((Byte)5, second.OverspeedDuration);
        Assert.Equal((UInt16)70, second.NightMaximumSpeed);
    }

    [Fact]
    [DisplayName("T8606_2013版_不限时不限速_无名称")]
    public void T8606_Version2013_NoConditionalFields()
    {
        var body = new T8606
        {
            RouteId = 1,
            RouteProperty = 0,
            StartTime = new DateTime(2026, 10, 20, 9, 0, 0),
            RouteName = "忽略",
            InflectionPointItems =
            [
                new RoutePoint
                {
                    InflectionPointId = 1,
                    SectionId = 2,
                    Latitude = 22334455,
                    Longitude = 114112233,
                    SectionWidth = 10,
                    SectionProperty = 0,
                },
            ],
        };

        var hex = WriteBody(body, 0);

        // 路线ID(4) + 属性(2) + 拐点数(2) + 单拐点(4+4+4+4+1+1)
        Assert.Equal(26, hex.Length / 2);

        // 无限时/限速字段时，条件字段不写入；2013 版无路线名称
        using var ms = new MemoryStream(Convert.FromHexString(hex));
        var header = new JTMessage { Version = 0 };
        var result = new T8606();
        Assert.True(result.Read(ms, header));
        Assert.Null(result.StartTime);
        Assert.Null(result.RouteName);
        Assert.Single(result.InflectionPointItems);
        Assert.Equal(22334455u, result.InflectionPointItems[0].Latitude);
        Assert.Equal((Byte)10, result.InflectionPointItems[0].SectionWidth);
    }
    #endregion

    #region 辅助
    private static String WriteBody(T8600 body, Byte version)
    {
        using var ms = new MemoryStream();
        Assert.True(body.Write(ms, new JTMessage { Version = version }));
        return Convert.ToHexString(ms.ToArray());
    }

    private static String WriteBody(T8602 body, Byte version)
    {
        using var ms = new MemoryStream();
        Assert.True(body.Write(ms, new JTMessage { Version = version }));
        return Convert.ToHexString(ms.ToArray());
    }

    private static String WriteBody(T8604 body, Byte version)
    {
        using var ms = new MemoryStream();
        Assert.True(body.Write(ms, new JTMessage { Version = version }));
        return Convert.ToHexString(ms.ToArray());
    }

    private static String WriteBody(T8606 body, Byte version)
    {
        using var ms = new MemoryStream();
        Assert.True(body.Write(ms, new JTMessage { Version = version }));
        return Convert.ToHexString(ms.ToArray());
    }
    #endregion
}
