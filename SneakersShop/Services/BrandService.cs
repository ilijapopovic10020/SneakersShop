using SneakersShop.Helpers;
using SneakersShop.MVVM.Models;
using System.Net.Http.Json;

namespace SneakersShop.Services
{
    public class BrandService : BaseService
    {
        public async Task<IEnumerable<BrandModel>> Get()
        {
            var response = await _httpClient.GetAsync("api/brands");

            ExceptionHelper.ThrowIfUnsuccessful(response);

            var brands = await response.Content.ReadFromJsonAsync<IEnumerable<BrandModel>>();

            if (brands == null)
                throw new UserFriendlyException("Nije moguće učitati brendove.", System.Net.HttpStatusCode.BadRequest);

            return brands;
        }
    }
}
