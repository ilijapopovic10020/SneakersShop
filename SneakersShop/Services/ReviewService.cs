using SneakersShop.Helpers;
using SneakersShop.Helpers.Extensions;
using SneakersShop.MVVM.Models;
using SneakersShop.MVVM.Models.Search;
using System.Net.Http.Json;

namespace SneakersShop.Services
{
    public class ReviewService : BaseService
    {
        public async Task<PagedResponseModel<ReviewsModel>> Get(PagedSearchId search)
        {
            var queryParams = new List<string>();

            if (search.Id != 0)
                queryParams.Add($"Id={search.Id}");
            if (search.PerPage.HasValue)
                queryParams.Add($"PerPage={search.PerPage.Value}");
            if (search.Page.HasValue)
                queryParams.Add($"Page={search.Page.Value}");

            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
            var endpoint = $"api/reviews{queryString}";

            var user = await SecureStorage.Default.GetUser();

            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.Token);

            var response = await _httpClient.SendAsync(request);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            var reviews = await response.Content.ReadFromJsonAsync<PagedResponseModel<ReviewsModel>>();

            if (reviews == null)
                throw new UserFriendlyException("Trenutno nije moguće videti recenzije. Molimo pokušajte kasnije.", System.Net.HttpStatusCode.BadRequest);

            return reviews;
        }
    }
}
