using SneakersShop.Exceptions;
using SneakersShop.Extensions;
using SneakersShop.Models;
using SneakersShop.Models.Search;
using SneakersShop.Services.Interfaces;
using System.Net.Http.Json;

namespace SneakersShop.Services.Implementations
{
    public class ProductService(HttpClient httpClient) : IProductsService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<PagedModel<ProductsModel>> GetProductsAsync(ProductsSearch search)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(search.Keyword))
                queryParams.Add("Keyword=" + Uri.EscapeDataString(search.Keyword));

            if (search.PerPage.HasValue)
                queryParams.Add("PerPage=" + search.PerPage.Value);

            if (search.Page.HasValue)
                queryParams.Add("Page=" + search.Page.Value);

            if (search.CategoryId.HasValue)
                queryParams.Add("CategoryId=" + search.CategoryId.Value);

            if (search.BrandId != null && search.BrandId.Count > 0)
            {
                foreach (var id in search.BrandId)
                    queryParams.Add("BrandId=" + id);
            }

            if (search.Color != null && search.Color.Count > 0)
            {
                foreach (var color in search.Color)
                    queryParams.Add("Color=" + Uri.EscapeDataString(color));
            }

            if (search.MinPrice.HasValue)
                queryParams.Add("MinPrice=" + search.MinPrice.Value);

            if (search.MaxPrice.HasValue)
                queryParams.Add("MaxPrice=" + search.MaxPrice.Value);

            if (search.Filter.HasValue)
                queryParams.Add("Filter=" + search.Filter.Value);

            var endpoint = "api/products" + (queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty);

            var response = await _httpClient.GetAsync(endpoint);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            var products = await response.Content.ReadFromJsonAsync<PagedModel<ProductsModel>>();

            return products ?? throw new UserFriendlyException("Nije moguće učitati proizvode. Molimo pokušajte ponovo.", System.Net.HttpStatusCode.BadRequest);
        }

        public async Task<ProductModel> GetProductByIdAsync(int id)
        {
            var user = await SecureStorage.Default.GetUser();

            var endpoint = $"api/products/{id}";

            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            if (user != null)
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AccessToken);
            }

            var response = await _httpClient.SendAsync(request);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            var product = await response.Content.ReadFromJsonAsync<ProductModel>();

            return product ?? throw new UserFriendlyException("Nije moguće učitati proizvod. Molimo pokušajte ponovo.", System.Net.HttpStatusCode.BadRequest);
        }
    }
}
