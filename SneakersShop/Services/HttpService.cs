using SneakersShop.Helpers.Extensions;
using SneakersShop.MVVM.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace SneakersShop.Services
{
    public class HttpService : BaseService
    {
        public async Task<string?> RefreshToken()
        {
            var user = await SecureStorage.Default.GetUser();

            var response = await _httpClient.PostAsJsonAsync("api/auth/refresh", new
            {
                user.RefreshToken
            });

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<TokenResponseModel>();

            if (result == null)
                return null;

            user.AccessToken = result.AccessToken;
            user.RefreshToken = result.RefreshToken;
            await SecureStorage.Default.SetAsync("user", JsonSerializer.Serialize(user));

            return result.AccessToken;
        }

        public async Task<HttpResponseMessage> SendWithAutoRefresh(HttpRequestMessage request)
        {
            var user = await SecureStorage.Default.GetUser();

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AccessToken);

            var response = await _httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAccessToken = await RefreshToken();
                if (string.IsNullOrEmpty(newAccessToken))
                    return response;

                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", newAccessToken);
                response.Dispose();
                return await _httpClient.SendAsync(request);
            }

            return response;
        }
    }
}
