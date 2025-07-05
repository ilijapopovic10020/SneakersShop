using SneakersShop.Helpers;
using SneakersShop.Helpers.Extensions;
using SneakersShop.MVVM.Models;
using SneakersShop.MVVM.Models.Search;
using System.Net.Http.Json;

namespace SneakersShop.Services
{
    public class FavoriteService : BaseService
    {
        private readonly HttpService _httpService = new();

        public async Task<PagedResponseModel<ProductsModel>> Get(PagedSearchKw search)
        {
            var user = await SecureStorage.Default.GetUser();
            if (user == null)
                throw new UserFriendlyException("Niste prijavljeni.", System.Net.HttpStatusCode.Unauthorized);

            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(search.Keyword))
                queryParams.Add("Keyword=" + Uri.EscapeDataString(search.Keyword));

            if (search.PerPage.HasValue)
                queryParams.Add("PerPage=" + search.PerPage.Value);

            if (search.Page.HasValue)
                queryParams.Add("Page=" + search.Page.Value);

            var endpoint = "api/favorites" + (queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty);

            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var response = await _httpService.SendWithAutoRefresh(request);
            ExceptionHelper.ThrowIfUnsuccessful(response);

            var content = await response.Content.ReadFromJsonAsync<PagedResponseModel<ProductsModel>>();
            if (content == null)
                throw new UserFriendlyException("Nije moguće učitati omiljene proizvode.", System.Net.HttpStatusCode.BadRequest);

            return content;
        }

        public async Task<bool> Post(int id)
        {
            var user = await SecureStorage.Default.GetUser();
            if (user == null)
                throw new UserFriendlyException("Niste prijavljeni.", System.Net.HttpStatusCode.Unauthorized);

            var request = new HttpRequestMessage(HttpMethod.Post, "api/favorites")
            {
                Content = JsonContent.Create(new { productColorId = id })
            };

            var response = await _httpService.SendWithAutoRefresh(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                return true;

            ExceptionHelper.ThrowIfUnsuccessful(response);
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            var user = await SecureStorage.Default.GetUser();
            if (user == null)
                throw new UserFriendlyException("Niste prijavljeni.", System.Net.HttpStatusCode.Unauthorized);

            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/favorites/{id}");

            var response = await _httpService.SendWithAutoRefresh(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return true;

            ExceptionHelper.ThrowIfUnsuccessful(response);
            return false;
        }
    }
}
