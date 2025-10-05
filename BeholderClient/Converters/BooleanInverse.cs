using System.Globalization;

namespace Beholder.Converters;

public class BooleanInverse : IValueConverter
{
    public Object Convert(Object? value, Type targetType, Object? parameter, CultureInfo culture)
    {
        if (value is null || value is not Boolean bvalue) return false;

        return !bvalue;
    }
    public Object ConvertBack(Object? value, Type targetType, Object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

