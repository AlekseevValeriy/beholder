namespace Beholder.Service;

public interface IApiClient
{
    Task<ApiResponse<Boolean>> IsActiveAsync();
    Task<ApiResponse<List<ChannelResponse>>> GetChannelsAsync();
    Task<ApiResponse<List<ChannelResponse>>> GetChannelsByQueryAsync(String query);
    Task<ApiResponse<ChannelResponse>> GetChannelAsync(Int32 id);
    Task<ApiResponse<List<ScheduleResponse>>> GetScheduleAsync(Int32 programId, DateTime date);
    Task<ApiResponse<List<FavoriteResponse>>> GetFavoritesAsync(Int32 userId);
    Task<ApiResponse<Boolean>> HasFavoriteAsync(Int32 userId, Int32 channelId);
    Task<ApiResponse<Boolean>> AddFavoritesAsync(Int32 programId, Int32 userId);
    Task<ApiResponse<Boolean>> DeleteFavoritesAsync(Int32 programId, Int32 userId);
    Task<ApiResponse<Int32>> GetUserAsync(String login, String password_hash);
    Task<ApiResponse<UserCreateResponse>> AddUserAsync(String login, String password_hash);
    Task<ApiResponse<Boolean>> DeleteUserAsync(Int32 userId, String login, String password_hash);
}
