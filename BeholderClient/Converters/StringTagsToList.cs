using System.Globalization;

namespace Beholder.Converters;

public class StringTagsToList : IValueConverter
{
    public Object Convert(Object? value, Type targetType, Object? parameter, CultureInfo culture)
    {
        List<String> list = new();

        if (value is null || value is not String svalue) return list;

        list.AddRange(svalue.Split(","));

        return list;
    }
    public Object ConvertBack(Object? value, Type targetType, Object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

