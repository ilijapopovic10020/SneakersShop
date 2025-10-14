using SneakersShop.Exceptions;
using SneakersShop.Extensions;
using SneakersShop.Models;
using SneakersShop.Models.Search;
using SneakersShop.Services.Interfaces;
using System.Net.Http.Json;

namespace SneakersShop.Services.Implementations
{
    public class FavoriteService : IFavoriteService
    {
        private readonly HttpClient _httpClient;

        public FavoriteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedModel<ProductsModel>> GetFavoriteAsync(int? page, int? perPage, string? keyword)
        {
            var user = await SecureStorage.Default.GetUser();
            if (user == null)
                throw new UserFriendlyException("Niste prijavljeni.", System.Net.HttpStatusCode.Unauthorized);

            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(keyword))
                queryParams.Add("Keyword=" + Uri.EscapeDataString(keyword));

            if (perPage.HasValue)
                queryParams.Add("PerPage=" + perPage.Value);

            if (page.HasValue)
                queryParams.Add("Page=" + page.Value);

            var endpoint = "api/favorites" + (queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty);

            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AccessToken);

            var response = await _httpClient.SendAsync(request);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            var favorites = await response.Content.ReadFromJsonAsync<PagedModel<ProductsModel>>();

            return favorites ?? throw new UserFriendlyException("Nije moguće učitati omiljene proizvode. Molimo pokušajte ponovo.", System.Net.HttpStatusCode.BadRequest);
        }

        public async Task<bool> AddToFavoritesAsync(int productId)
        {
            var user = await SecureStorage.Default.GetUser() ?? throw new UserFriendlyException("Niste prijavljeni.", System.Net.HttpStatusCode.Unauthorized);

            var request = new HttpRequestMessage(HttpMethod.Post, "api/favorites")
            {
                Content = JsonContent.Create(new { productColorId = productId })
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AccessToken);

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                return true;

            ExceptionHelper.ThrowIfUnsuccessful(response);

            return false;
        }

        public async Task<bool> RemoveFromFavoritesAsync(int productId)
        {
            var user = await SecureStorage.Default.GetUser();
            if (user == null)
                throw new UserFriendlyException("Niste prijavljeni.", System.Net.HttpStatusCode.Unauthorized);

            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/favorites/{productId}");

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AccessToken);

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return true;

            ExceptionHelper.ThrowIfUnsuccessful(response);
            return false;
        }
    }
}
