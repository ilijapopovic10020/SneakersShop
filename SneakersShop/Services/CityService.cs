using SneakersShop.Helpers;
using SneakersShop.MVVM.Models;
using System.Net.Http.Json;

namespace SneakersShop.Services
{
    public class CityService : BaseService
    {
        public async Task<IEnumerable<CityModel>> Get()
        {
            var response = await _httpClient.GetAsync("api/cities");

            ExceptionHelper.ThrowIfUnsuccessful(response);

            var cities = await response.Content.ReadFromJsonAsync<IEnumerable<CityModel>>();

            if (cities == null)
                throw new UserFriendlyException("Nije moguće učitati gradove.", System.Net.HttpStatusCode.BadRequest);

            return cities;
        }
    }
}
