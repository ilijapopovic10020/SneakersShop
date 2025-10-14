using SneakersShop.Exceptions;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace SneakersShop.Services.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly HttpClient _httpClient;

        public BrandService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<BrandsModel>> GetBrandsAsync()
        {
            var response = await _httpClient.GetAsync("/api/brands");

            ExceptionHelper.ThrowIfUnsuccessful(response);

            var brands = await response.Content.ReadFromJsonAsync<IEnumerable<BrandsModel>>() ?? 
                throw new UserFriendlyException("Nije moguće učitati brendove.", HttpStatusCode.BadRequest);

            return brands;
        }
    }
}
