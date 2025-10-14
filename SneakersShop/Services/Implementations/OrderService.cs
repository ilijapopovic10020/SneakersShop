using System.Net;
using System.Net.Http.Json;
using SneakersShop.Exceptions;
using SneakersShop.Extensions;
using SneakersShop.Models;
using SneakersShop.Models.Search;
using SneakersShop.Services.Interfaces;

namespace SneakersShop.Services.Implementations
{
    public class OrderService(HttpClient httpClient) : IOrderService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<PagedModel<OrdersModel>> GetOrdersAsync(int? page, int? perPage)
        {
            var user = await SecureStorage.Default.GetUser();

            var queryParams = new List<string>();

            if (perPage.HasValue)
                queryParams.Add("PerPage=" + perPage.Value);

            if (page.HasValue)
                queryParams.Add("Page=" + page.Value);

            var endpoint =
                "api/orders"
                + (queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty);

            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                user.AccessToken
            );

            var response = await _httpClient.SendAsync(request);
            ExceptionHelper.ThrowIfUnsuccessful(response);

            var orders = await response.Content.ReadFromJsonAsync<PagedModel<OrdersModel>>();

            return orders
                ?? throw new UserFriendlyException(
                    "Nije moguće učitati porudžbine. Molimo pokušajte kasnije.",
                    HttpStatusCode.BadRequest
                );
        }

        public async Task<OrderModel> GetOrderByIdAsync(int id)
        {
            var user = await SecureStorage.Default.GetUser();

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/orders/{id}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                user.AccessToken
            );

            var response = await _httpClient.SendAsync(request);
            ExceptionHelper.ThrowIfUnsuccessful(response);

            var order = await response.Content.ReadFromJsonAsync<OrderModel>();

            return order
                ?? throw new UserFriendlyException(
                    "Nije moguće učitati porudžbinu. Molimo pokušajte kasnije.",
                    HttpStatusCode.BadRequest
                );
        }

        public async Task<bool> CreateOrderAsync(CreateOrderModel orderModel)
        {
            var user = await SecureStorage.Default.GetUser();

            var request = new HttpRequestMessage(HttpMethod.Post, "api/orders")
            {
                Content = JsonContent.Create(orderModel),
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                user.AccessToken
            );

            var response = await _httpClient.SendAsync(request);
            ExceptionHelper.ThrowIfUnsuccessful(response);

            return response.StatusCode == HttpStatusCode.Created;
        }

        public async Task<bool> CancelOrderAsync(int id)
        {
            var user = SecureStorage.Default.GetUser();

            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/orders/{id}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.Result.AccessToken);

            var response = await _httpClient.SendAsync(request);
            ExceptionHelper.ThrowIfUnsuccessful(response);

            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}
