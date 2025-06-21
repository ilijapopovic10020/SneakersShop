using SneakersShop.Helpers;
using SneakersShop.MVVM.Models;
using System.Net.Http.Json;

namespace SneakersShop.Services
{
    public class CategoryService : BaseService
    {
        public async Task<IEnumerable<CategoryModel>> Get()
        {
            var response = await _httpClient.GetAsync("api/categories");

            ExceptionHelper.ThrowIfUnsuccessful(response);

            var categories = await response.Content.ReadFromJsonAsync<IEnumerable<CategoryModel>>();

            if (categories == null)
                throw new UserFriendlyException("Nije moguće učitati kategorije.", System.Net.HttpStatusCode.BadRequest);

            return categories;
        }
    }
}
