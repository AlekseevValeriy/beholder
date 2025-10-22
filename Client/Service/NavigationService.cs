namespace Beholder.Service;

public class NavigationService : INavigationService
{
    readonly IServiceProvider _services;

    public NavigationService(IServiceProvider services)
    {
        _services = services;
    }

    async public Task NavigateToAuthorizationPageAsync(ContentPage currentPage)
    {
        AuthorizationPageViewModel? viewModel = _services.GetService<AuthorizationPageViewModel>();
        if (viewModel is null) return;
        AuthorizationPage page = new AuthorizationPage(viewModel);
        await currentPage.Navigation.PushAsync(page);
    }

    async public Task NavigateToAccountPageAsync(ContentPage currentPage)
    {
        AccountPageViewModel? viewModel = _services.GetService<AccountPageViewModel>();
        if (viewModel is null) return;
        AccountPage page = new AccountPage(viewModel);
        await currentPage.Navigation.PushAsync(page);
    }

    async public Task NavigateToTeleprogramPageAsync(ContentPage currentPage, Int32 channelId)
    {
        TeleprogramPageViewModel? viewModel = _services.GetService<TeleprogramPageViewModel>();
        if (viewModel is null) return;
        TeleprogramPage page = new TeleprogramPage(viewModel, channelId);
        await currentPage.Navigation.PushAsync(page);
    }
}
