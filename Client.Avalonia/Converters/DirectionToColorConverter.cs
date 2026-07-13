using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Client.Avalonia.Converters;

/// <summary>消息方向转颜色 — ↑ 绿色（发送），↓ 蓝色（接收）</summary>
public class DirectionToColorConverter : IValueConverter
{
    public Object? Convert(Object? value, Type targetType, Object? parameter, CultureInfo culture)
    {
        if (value is String dir)
        {
            return dir switch
            {
                "↑" => Brushes.ForestGreen,
                "↓" => Brushes.DodgerBlue,
                _ => Brushes.Gray,
            };
        }
        return Brushes.Gray;
    }

    public Object? ConvertBack(Object? value, Type targetType, Object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
