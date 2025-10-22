namespace Beholder.Service;

public interface INavigationService
{
    Task NavigateToAuthorizationPageAsync(ContentPage currentPage);
    Task NavigateToAccountPageAsync(ContentPage currentPage);
    Task NavigateToTeleprogramPageAsync(ContentPage currentPage, Int32 channelId);
}
