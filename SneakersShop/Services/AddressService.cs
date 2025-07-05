using SneakersShop.Helpers;
using SneakersShop.Helpers.Extensions;
using SneakersShop.MVVM.Models;
using System.Net;
using System.Net.Http.Json;

namespace SneakersShop.Services
{
    public class AddressService : BaseService
    {
        private readonly HttpService _httpService = new();
        public async Task<IEnumerable<AddressModel>> Get()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/addresses");

            var response = await _httpService.SendWithAutoRefresh(request);
            ExceptionHelper.ThrowIfUnsuccessful(response);

            var content = await response.Content.ReadFromJsonAsync<IEnumerable<AddressModel>>();

            return content == null ? 
                throw new UserFriendlyException("Nije moguće učitati adrese.", HttpStatusCode.BadRequest) 
                : content;
        }

        public async Task<bool> Post(CreateAddressModel address)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/addresses")
            {
                Content = JsonContent.Create(address)
            };

            var response = await _httpService.SendWithAutoRefresh(request);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            return response.StatusCode == HttpStatusCode.Created;
        }

        public async Task<bool> Update(int id, UpdateAddressModel model)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/addresses/{id}")
            {
                Content = JsonContent.Create(model)
            };

            var response = await _httpService.SendWithAutoRefresh(request);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            return response.IsSuccessStatusCode;
        }


        public async Task<bool> Delete(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/addresses/{id}");

            var response = await _httpService.SendWithAutoRefresh(request);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            return response.StatusCode == HttpStatusCode.NoContent || response.IsSuccessStatusCode;
        }

    }
}
