using Beholder.Helpers;

namespace Beholder.Service;

public class AppState : INotifyPropertyChanged
{
    readonly IApiClient _apiClient;

    public ApiResponse<List<ChannelResponse>>? Channels { get; private set; }
    public ApiResponse<List<ChannelResponse>>? ChannelsQueryResult { get; private set; }
    public ApiResponse<List<ScheduleResponse>>? Schedule { get; private set; }
    public ApiResponse<List<FavoriteResponse>>? Favorites { get; private set; }
    public ApiResponse<UserCreateResponse>? User { get; private set; }
    public ApiResponse<ChannelResponse>? Channel { get; private set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public AppState(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    async public Task LoadChannelsAsync()
    {
        Channels = await _apiClient.GetChannelsAsync();

        OnPropertyChanged(nameof(Channels));
    }

    async public Task LoadChannelsByQueryAsync(String query)
    {
        ChannelsQueryResult = await _apiClient.GetChannelsByQueryAsync(query);
        OnPropertyChanged(nameof(ChannelsQueryResult));
    }

    async public Task LoadChannelAsync(Int32 id)
    {
        Channel = await _apiClient.GetChannelAsync(id);
        OnPropertyChanged(nameof(Channel));
    }

    async public Task LoadScheduleAsync(Int32 programId, DateTime date)
    {
        ApiResponse<List<ScheduleResponse>> result = await _apiClient.GetScheduleAsync(programId, date);

        if (result.IsSuccess && result.Content is not null && Schedule is not null && Schedule.IsSuccess && Schedule.Content is not null)
        {
            Schedule.Content.AddRange(result.Content);                
        }
        else
        {
            Schedule = result;
        }

        OnPropertyChanged(nameof(Schedule));
    }

    async public Task LoadFavoritesAsync(Int32 userId)
    {
        Favorites = await _apiClient.GetFavoritesAsync(userId);
        OnPropertyChanged(nameof(Favorites));
    }

    async public Task<ApiResponse<Boolean>> hasFavoriteAsync(Int32 userId, Int32 channelId)
    {
        return await _apiClient.HasFavoriteAsync(userId, channelId);
    }

    async public Task<ApiResponse<Boolean>> AddfavoriteAsync(Int32 programId, Int32 userId)
    {
        ApiResponse<Boolean> result = await _apiClient.AddFavoritesAsync(programId, userId);

        if (result.IsSuccess && result.Content)
        {
            await LoadFavoritesAsync(userId);
        }

        return result;
    }

    async public Task<ApiResponse<Boolean>> DeletefavoriteAsync(Int32 programId, Int32 userId)
    {
        ApiResponse<Boolean> result = await _apiClient.DeleteFavoritesAsync(programId, userId);

        if (result.IsSuccess && result.Content)
        {
            await LoadFavoritesAsync(userId);
        }

        return result;
    }

    async public Task GetUserAsync(String login, String password)
    {
        String password_hash = Crypt.StringToSha256Hash(password);

        ApiResponse<Int32> response = await _apiClient.GetUserAsync(login, password_hash);

        if (response.HasProblem)
        {
            User = new(response);
        }
        else if (response.IsSuccess && response.Content == -1)
        {

        }
        else
        {
            User = new(new UserCreateResponse(response.Content, login, password));
        }
        OnPropertyChanged(nameof(User));
    }

    async public Task AddUserAsync(String login, String password)
    {
        String password_hash = Crypt.StringToSha256Hash(password);

        User = await _apiClient.AddUserAsync(login, password_hash);
        OnPropertyChanged(nameof(User));
    }

    async public Task DeleteUserAsync()
    {
        if (User is null || User.HasProblem || User.Content is null) return;

        String password_hash = Crypt.StringToSha256Hash(User.Content.password);

        ApiResponse<Boolean> response = await _apiClient.DeleteUserAsync(User.Content.id, User.Content.login, password_hash);

        if (response.IsSuccess && !response.Content) return;

        if (response.HasProblem)
        {
            User = new(response);
        }

        User = null;
        OnPropertyChanged(nameof(User));
    }

    public void ClearUser()
    {
        User = null;
        OnPropertyChanged(nameof(User));
    }

    public void ClearSchedule()
    {
        Schedule = null;
        OnPropertyChanged(nameof(Schedule));
    }

    public void ClearChannel()
    {
        Channel = null;
        OnPropertyChanged(nameof(Channel));
    }

    public void OnPropertyChanged([CallerMemberName] String prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public void SetChannels(List<ChannelResponse> channels)
    {
        Channels = new(channels);
        OnPropertyChanged(nameof(Channels));
    }
}
