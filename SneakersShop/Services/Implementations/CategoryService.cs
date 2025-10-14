using SneakersShop.Exceptions;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using System.Net.Http.Json;

namespace SneakersShop.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }   
        public async Task<IEnumerable<CategoriesModel>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("/api/categories");

            ExceptionHelper.ThrowIfUnsuccessful(response);

            var categories = await response.Content.ReadFromJsonAsync<IEnumerable<CategoriesModel>>() ??
                throw new UserFriendlyException("Nije moguće učitati kategorije.", System.Net.HttpStatusCode.BadRequest);

            return categories;
        }
    }
}
