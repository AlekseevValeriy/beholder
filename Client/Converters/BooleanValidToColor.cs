using System.Globalization;

namespace Beholder.Converters;

public class BooleanValidToColor : IValueConverter
{
    public Object Convert(Object? value, Type targetType, Object? parameter, CultureInfo culture)
    {
        if (value is null || value is not Boolean bol) return Colors.Transparent;

        return bol switch
        {
            true => Colors.Green,
            false => Colors.Red
        };
    }
    public Object ConvertBack(Object? value, Type targetType, Object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

