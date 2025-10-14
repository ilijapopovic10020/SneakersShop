using SneakersShop.Exceptions;
using SneakersShop.Extensions;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace SneakersShop.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserModel> GetUserById(int id)
        {
            var user = await SecureStorage.Default.GetUser();

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/users/{id}");

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AccessToken);

            var response = await _httpClient.SendAsync(request);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            return await response.Content.ReadFromJsonAsync<UserModel>() 
                ?? throw new UserFriendlyException("Korisnički podaci nisu pronađeni.", HttpStatusCode.NotFound); ;
        }

        public async Task<bool> Register(RegisterUserModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users", model);
            
            ExceptionHelper.ThrowIfUnsuccessful(response);

            return response.StatusCode == HttpStatusCode.Created;
        }

        public async Task<bool> Update(UpdateUserModel model)
        {
            var user = await SecureStorage.Default.GetUser();

            var request = new HttpRequestMessage(HttpMethod.Put, $"api/users/{user.Id}")
            {
                Content = JsonContent.Create(model)
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AccessToken);

            var response = await _httpClient.SendAsync(request);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            return response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.OK;
        }
    }
}
