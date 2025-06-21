using SneakersShop.Helpers;
using SneakersShop.Helpers.Extensions;
using SneakersShop.MVVM.Models;
using System.Net;
using System.Net.Http.Json;

namespace SneakersShop.Services
{
    public class AddressService : BaseService
    {
        public async Task<IEnumerable<AddressModel>> Get()
        {
            var user = await SecureStorage.Default.GetUser();

            var request = new HttpRequestMessage(HttpMethod.Get, "api/addresses");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.Token);

            var response = await _httpClient.SendAsync(request);
            ExceptionHelper.ThrowIfUnsuccessful(response);

            var content = await response.Content.ReadFromJsonAsync<IEnumerable<AddressModel>>();

            if (content == null)
                throw new UserFriendlyException("Nije moguće učitati adrese.", HttpStatusCode.BadRequest);

            return content;
        }

        public async Task<bool> Post(CreateAddressModel address)
        {
            var user = await SecureStorage.Default.GetUser();

            var request = new HttpRequestMessage(HttpMethod.Post, "api/addresses")
            {
                Content = JsonContent.Create(address)
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.Token);

            var response = await _httpClient.SendAsync(request);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            return response.StatusCode == HttpStatusCode.Created;
        }

        public async Task<bool> Update(int id, UpdateAddressModel model)
        {
            var user = await SecureStorage.Default.GetUser();

            var request = new HttpRequestMessage(HttpMethod.Put, $"api/addresses/{id}")
            {
                Content = JsonContent.Create(model)
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.Token);

            var response = await _httpClient.SendAsync(request);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            return response.IsSuccessStatusCode;
        }


        public async Task<bool> Delete(int id)
        {
            var user = await SecureStorage.Default.GetUser();

            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/addresses/{id}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.Token);

            var response = await _httpClient.SendAsync(request);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            return response.StatusCode == HttpStatusCode.NoContent || response.IsSuccessStatusCode;

        }

    }
}
