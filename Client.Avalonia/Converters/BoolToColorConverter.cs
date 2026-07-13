using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Client.Avalonia.Converters;

/// <summary>布尔值转颜色转换器 — true 绿色，false 红色</summary>
public class BoolToColorConverter : IValueConverter
{
    public Object? Convert(Object? value, Type targetType, Object? parameter, CultureInfo culture)
    {
        if (value is Boolean b)
            return b ? Brushes.LimeGreen : Brushes.Red;
        return Brushes.Gray;
    }

    public Object? ConvertBack(Object? value, Type targetType, Object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
