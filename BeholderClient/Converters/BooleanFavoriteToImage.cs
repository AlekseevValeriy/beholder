using System.Globalization;

namespace Beholder.Converters;

public class BooleanFavoriteToImage : IValueConverter
{
    public Object Convert(Object? value, Type targetType, Object? parameter, CultureInfo culture)
    {
        if (value is null || value is not Boolean bvalue) return "";

        if (!bvalue) return "star_outline.png";

        return "star.png";



    }
    public Object ConvertBack(Object? value, Type targetType, Object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

