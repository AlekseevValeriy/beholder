using System.Globalization;

namespace Beholder.Converters;

public class IsVisiableToEye : IValueConverter
{
    public Object Convert(Object? value, Type targetType, Object? parameter, CultureInfo culture)
    {
        if (value is null || value is not Boolean bvalue) return "";

        if (bvalue) return "eye_close_primary.png";

        return "eye_open_primary.png";
    }
    public Object ConvertBack(Object? value, Type targetType, Object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

