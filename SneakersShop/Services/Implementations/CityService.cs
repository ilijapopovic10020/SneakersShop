using SneakersShop.Exceptions;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace SneakersShop.Services.Implementations
{
    public class CityService : ICityService
    {
        private readonly HttpClient _httpClient;

        public CityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<CityModel>> GetCitiesAsync()
        {
            var response = await _httpClient.GetAsync("/api/cities");

            ExceptionHelper.ThrowIfUnsuccessful(response);

            var cities = await response.Content.ReadFromJsonAsync<IEnumerable<CityModel>>() ??
                throw new UserFriendlyException("Nije moguće učitati brendove.", HttpStatusCode.BadRequest);

            return cities;
        }
    }
}
