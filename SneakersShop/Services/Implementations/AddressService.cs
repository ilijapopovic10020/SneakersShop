using SneakersShop.Exceptions;
using SneakersShop.Extensions;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using System.Net;
using System.Net.Http.Json;
using System.Reflection;

namespace SneakersShop.Services.Implementations
{
    public class AddressService(HttpClient httpClient) : IAddressService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<AddressesModel> GetAddressByIdAsync(int id)
        {
            var user = await SecureStorage.Default.GetUser();

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/addresses/{id}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AccessToken);

            var response = await _httpClient.SendAsync(request);
            ExceptionHelper.ThrowIfUnsuccessful(response);

            return await response.Content.ReadFromJsonAsync<AddressesModel>() ??
                throw new UserFriendlyException("Nije moguće učitati adrese. Molimo pokušajte ponovo.", HttpStatusCode.BadRequest);
        }

        public async Task<IEnumerable<AddressesModel>> GetAddressesAsync()
        {
            var user = await SecureStorage.Default.GetUser();

            var request = new HttpRequestMessage(HttpMethod.Get, "api/Addresses");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AccessToken);

            var response = await _httpClient.SendAsync(request);
            ExceptionHelper.ThrowIfUnsuccessful(response);

            return await response.Content.ReadFromJsonAsync<IEnumerable<AddressesModel>>() ??
                throw new UserFriendlyException("Nije moguće učitati adrese. Molimo pokušajte ponovo.", HttpStatusCode.BadRequest);

        }

        public async Task<bool> CreateAddressAsync(CreateAddressModel model)
        {
            var user = await SecureStorage.Default.GetUser();

            var request = new HttpRequestMessage(HttpMethod.Post, "api/Addresses")
            {
                Content = JsonContent.Create(model)
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AccessToken);

            var response = await _httpClient.SendAsync(request);
            ExceptionHelper.ThrowIfUnsuccessful(response);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAddressAsync(int id)
        {
            var user = await SecureStorage.Default.GetUser();

            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/Addresses/{id}");

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AccessToken);

            var response = await _httpClient.SendAsync(request);
            ExceptionHelper.ThrowIfUnsuccessful(response);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAddressAsync(int id, UpdateAddressModel model)
        {
            var user = await SecureStorage.Default.GetUser();

            var request = new HttpRequestMessage(HttpMethod.Put, "api/Addresses")
            {
                Content = JsonContent.Create(model)
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AccessToken);

            var response = await _httpClient.SendAsync(request);
            ExceptionHelper.ThrowIfUnsuccessful(response);

            return response.IsSuccessStatusCode;
        }
    }
}
