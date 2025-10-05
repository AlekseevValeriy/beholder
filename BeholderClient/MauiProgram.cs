using FFImageLoading.Maui;

namespace Beholder;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseFFImageLoading()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("SFProText-Regular.ttf", "SFProRegular");
                fonts.AddFont("SFProText-Semibold.ttf", "SFProSemibold");
            });
        builder.Services.AddSingleton<IApiClient, ApiClient>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IDataLoaderService, DataLoaderService>();
        builder.Services.AddSingleton<AppState>();

        builder.Services.AddTransient<ListPageViewModel>();
        builder.Services.AddTransient<AccountPageViewModel>();
        builder.Services.AddTransient<AuthorizationPageViewModel>();
        builder.Services.AddTransient<FavoritesPageViewModel>();
        builder.Services.AddTransient<SearchPageViewModel>();
        builder.Services.AddTransient<TeleprogramPageViewModel>();

        return builder.Build();
    }
}