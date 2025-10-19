using Beholder.Extensions;

using System.Globalization;

namespace Beholder.Converters;

public class BooleanFavoriteToImage : IValueConverter
{
    public Object Convert(Object? value, Type targetType, Object? parameter, CultureInfo culture)
    {
        if (value is null || value is not Boolean bvalue) return "";

        switch (Application.Current?.RequestedTheme)
        {
            case AppTheme.Light: return bvalue ? "star_fill_light.png" : "star_outline_light.png";
            case AppTheme.Dark: return bvalue ? "star_fill_dark.png" : "star_outline_dark.png";
            default: return "";
        };
    }
    public Object ConvertBack(Object? value, Type targetType, Object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

