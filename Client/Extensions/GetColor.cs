namespace Beholder.Extensions;

public static class ResourceDictionaryExtensions
{
    public static Color GetColorOrDefault(this ResourceDictionary resourceDictionary, String colorName, Color defaultColor)
    {
        if (resourceDictionary.TryGetValue(colorName, out Object color))
        {
            return (Color)color;
        }
        return defaultColor;
    }
}
