using SneakersShop.Helpers;
using SneakersShop.Helpers.Extensions;
using SneakersShop.MVVM.Models;
using SneakersShop.MVVM.Models.Search;
using System.Net.Http.Json;

namespace SneakersShop.Services
{
    public class OrderService : BaseService
    {
        public async Task<IEnumerable<OrdersModel>> Get(PagedSearchId search)
        {
            var user = await SecureStorage.Default.GetUser();
            if (user == null)
                throw new UserFriendlyException("Niste prijavljeni.", System.Net.HttpStatusCode.Unauthorized);

            var queryParams = new List<string>();

            if (search.Id > 0)
                queryParams.Add("Id=" + search.Id);

            if (search.PerPage.HasValue)
                queryParams.Add("PerPage=" + search.PerPage.Value);

            if (search.Page.HasValue)
                queryParams.Add("Page=" + search.Page.Value);

            var endpoint = "api/orders" + (queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty);

            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.Token);

            var response = await _httpClient.SendAsync(request);
            ExceptionHelper.ThrowIfUnsuccessful(response);

            var orders = await response.Content.ReadFromJsonAsync<IEnumerable<OrdersModel>>();
            if (orders == null)
                throw new UserFriendlyException("Nije moguće učitati porudžbine. Molimo pokušajte kasnije.", System.Net.HttpStatusCode.BadRequest);

            return orders;
        }

        public async Task<OrderModel> Get(int id)
        {
            var user = await SecureStorage.Default.GetUser();
            if (user == null)
                throw new UserFriendlyException("Niste prijavljeni.", System.Net.HttpStatusCode.Unauthorized);

            var endpoint = $"api/orders/{id}";
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.Token);

            var response = await _httpClient.SendAsync(request);
            ExceptionHelper.ThrowIfUnsuccessful(response);

            var order = await response.Content.ReadFromJsonAsync<OrderModel>();
            if (order == null)
                throw new UserFriendlyException("Nije moguće učitati porudžbinu. Molimo pokušajte kasnije.", System.Net.HttpStatusCode.BadRequest);

            return order;
        }

        public async Task<bool> Post(CreateOrderModel order)
        {
            var user = await SecureStorage.Default.GetUser();
            if (user == null)
                throw new UserFriendlyException("Niste prijavljeni.", System.Net.HttpStatusCode.Unauthorized);

            var request = new HttpRequestMessage(HttpMethod.Post, "api/orders")
            {
                Content = JsonContent.Create(order)
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.Token);

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                return true;

            ExceptionHelper.ThrowIfUnsuccessful(response);
            return false;
        }
    }
}
