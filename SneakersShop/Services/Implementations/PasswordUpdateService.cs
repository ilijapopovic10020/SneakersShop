using SneakersShop.Exceptions;
using SneakersShop.Extensions;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using System.Net.Http.Json;

namespace SneakersShop.Services.Implementations
{
    public class PasswordUpdateService(HttpClient httpClient) : IPasswordUpdateService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<bool> UpdatePasswordAsync(PasswordUpdateModel model)
        {
            var user = await SecureStorage.Default.GetUser();

            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/password/{user.Id}")
            {
                Content = JsonContent.Create(model)
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AccessToken);

            var response = await _httpClient.SendAsync(request);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
