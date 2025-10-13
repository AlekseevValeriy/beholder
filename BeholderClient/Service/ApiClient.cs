using System.Net;
using System.Text;

using Flurl;

using static System.Net.HttpStatusCode;

namespace Beholder.Service;

public class ApiClient : IApiClient
{
    public static String Address { get; set; } = @"https://localhost:7293";
    HttpClient HttpClient { get; } = new HttpClient();

    async public Task<ApiResponse<Boolean>> IsActiveAsync()
    {
        try
        {
            String request = Url.Combine(Address);

            HttpResponseMessage response = await HttpClient.GetAsync(request);

            if (response.StatusCode is OK) return new (true);
            else return new (response.StatusCode);
        }
        catch (HttpRequestException)
        {
            return new (BadGateway);
        }
        catch (Exception ex)
        {
            return new (ex);
        }
    }

    async public Task<ApiResponse<List<ChannelResponse>>> GetChannelsAsync()
    {
        try
        {
            String request = Url.Combine(Address, "channels");

            HttpResponseMessage response = await HttpClient.GetAsync(request);

            if (response.StatusCode is not OK) return new (response.StatusCode);

            String responseContent = await response.Content.ReadAsStringAsync();

            List<ChannelResponse>? result = JsonSerializer.Deserialize<List<ChannelResponse>>(responseContent);
            return new (result is null ? [] : result);
        }
        catch (HttpRequestException)
        {
            return new (BadGateway);
        }
        catch (Exception ex)
        {
            return new (ex);
        }
    }

    async public Task<ApiResponse<ChannelResponse>> GetChannelAsync(Int32 id)
    {
        try
        {
            String request = Url.Combine(Address, "channels", id.ToString());

            HttpResponseMessage response = await HttpClient.GetAsync(request);

            if (response.StatusCode is not OK) return new (response.StatusCode);

            String responseContent = await response.Content.ReadAsStringAsync();

            List<ChannelResponse>? result = JsonSerializer.Deserialize<List<ChannelResponse>>(responseContent);
            return new (result is null ? ChannelResponse.Empty : result.FirstOrDefault() ?? ChannelResponse.Empty);
        }
        catch (HttpRequestException)
        {
            return new (BadGateway);
        }
        catch (Exception ex)
        {
            return new (ex);
        }
    }

    async public Task<ApiResponse<List<ChannelResponse>>> GetChannelsByQueryAsync(String query)
    {
        try
        {
            String request = Url.Combine(Address, "channels", $"search?searchQuery={query.ToLower()}");

            HttpResponseMessage response = await HttpClient.GetAsync(request);

            if (response.StatusCode is not OK) return new (response.StatusCode);

            String responseContent = await response.Content.ReadAsStringAsync();

            List<ChannelResponse>? result = JsonSerializer.Deserialize<List<ChannelResponse>>(responseContent);
            return new (result is null ? [] : result);
        }
        catch (HttpRequestException)
        {
            return new (BadGateway);
        }
        catch (Exception ex)
        {
            return new (ex);
        }
    }


    async public Task<ApiResponse<List<ScheduleResponse>>> GetScheduleAsync(Int32 programId, DateTime date)
    {
        try
        {
            String request = Url.Combine(Address, "schedule");

            ScheduleRequest body = new()
            {
                id = programId,
                date = date
            };

            HttpContent content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await HttpClient.PostAsync(request, content);

            if (response.StatusCode is not OK) return new (response.StatusCode);

            String responseContent = await response.Content.ReadAsStringAsync();

            List<ScheduleResponse>? result = JsonSerializer.Deserialize<List<ScheduleResponse>>(responseContent);
            return new (result is null ? [] : result);
        }
        catch (HttpRequestException)
        {
            return new(BadGateway);
        }
        catch (Exception ex)
        {
            return new (ex);
        }
    }

    async public Task<ApiResponse<List<FavoriteResponse>>> GetFavoritesAsync(Int32 userId)
    {
        try
        {
            String request = Url.Combine(Address, "favorites", userId.ToString());

            HttpResponseMessage response = await HttpClient.GetAsync(request);

            if (response.StatusCode is not OK) return new (response.StatusCode);

            String responseContent = await response.Content.ReadAsStringAsync();

            List<FavoriteResponse>? result = JsonSerializer.Deserialize<List<FavoriteResponse>>(responseContent);
            return new (result is null ? [] : result);
        }
        catch (HttpRequestException)
        {
            return new(BadGateway);
        }
        catch (Exception ex)
        {
            return new (ex);
        }
    }

    async public Task<ApiResponse<Boolean>> HasFavoriteAsync(Int32 userId, Int32 channelId)
    {
        try
        {
            String request = Url.Combine(Address, "favorites", userId.ToString(), channelId.ToString());

            HttpResponseMessage response = await HttpClient.GetAsync(request);

            if (response.StatusCode is OK) return new (true);
            else return new (response.StatusCode);
        }
        catch (HttpRequestException)
        {
            return new(BadGateway);
        }
        catch (Exception ex)
        {
            return new (ex);
        }
    }

    async public Task<ApiResponse<Boolean>> AddFavoritesAsync(Int32 channelId, Int32 userId)
    {
        try
        {
            String request = Url.Combine(Address, "favorites");

            FavoriteRequest body = new FavoriteRequest()
            {
                user_id = userId,
                channel_id = channelId
            };

            HttpContent content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await HttpClient.PutAsync(request, content);

            if (response.StatusCode is OK) return new (true);
            else return new (response.StatusCode);
        }
        catch (HttpRequestException)
        {
            return new(BadGateway);
        }
        catch (Exception ex)
        {
            return new (ex);
        }
    }

    async public Task<ApiResponse<Boolean>> DeleteFavoritesAsync(Int32 channelId, Int32 userId)
    {
        try
        {
            String request = Url.Combine(Address, $"favorites?channel_id={channelId}&user_id={userId}");

            HttpResponseMessage response = await HttpClient.DeleteAsync(request);

            if (response.StatusCode is OK) return new (true);
            else return new (response.StatusCode);
        }
        catch (HttpRequestException)
        {
            return new(BadGateway);
        }
        catch (Exception ex)
        {
            return new (ex);
        }
    }

    async public Task<ApiResponse<Int32>> GetUserAsync(String login, String password_hash)
    {
        try
        {
            String request = Url.Combine(Address, $"users?login={login}&password_hash={password_hash}");

            HttpResponseMessage response = await HttpClient.GetAsync(request);

            if (response.StatusCode is not OK) return new (response.StatusCode);

            String responseContent = await response.Content.ReadAsStringAsync();

            List<Int32>? result = JsonSerializer.Deserialize<List<Int32>>(responseContent);
            return new ((result is null || result.Count == 0) ? -1 : result.First());
        }
        catch (HttpRequestException)
        {
            return new(BadGateway);
        }
        catch (Exception ex)
        {
            return new (ex);
        }
    }

    async public Task<ApiResponse<UserCreateResponse>> AddUserAsync(String login, String password_hash)
    {
        try
        {
            String request = Url.Combine(Address, "users");

            UserRequest body = new UserRequest(login, password_hash);

            String serializedBody = JsonSerializer.Serialize(body);

            HttpContent content = new StringContent(serializedBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await HttpClient.PutAsync(request, content);

            if (response.StatusCode is not OK) return new (response.StatusCode);

            String responseContent = await response.Content.ReadAsStringAsync();

            List<UserCreateResponse>? result = JsonSerializer.Deserialize<List<UserCreateResponse>>(responseContent);
            return new (result is null ? UserCreateResponse.Empty : result.FirstOrDefault() ?? UserCreateResponse.Empty);
        }
        catch (HttpRequestException)
        {
            return new(BadGateway);
        }
        catch (Exception ex)
        {
            return new (ex);
        }
    }

    async public Task<ApiResponse<Boolean>> DeleteUserAsync(Int32 userId, String login, String password_hash)
    {
        try
        {
            String request = Url.Combine(Address, $"users?login={login}&password_hash={password_hash}&id={userId}");

            HttpResponseMessage response = await HttpClient.DeleteAsync(request);

            if (response.StatusCode is OK) return new (true);
            else return new (response.StatusCode);
        }
        catch (HttpRequestException)
        {
            return new(BadGateway);
        }
        catch (Exception ex)
        {
            return new (ex);
        }
    }
}
