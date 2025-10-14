using SneakersShop.Exceptions;
using SneakersShop.Extensions;
using SneakersShop.Models;
using SneakersShop.Models.Search;
using SneakersShop.Services.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace SneakersShop.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly HttpClient _httpClient;

        public ReviewService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedModel<ReviewsModel>> GetReviewsAsync(PagedSearchId search)
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

            var response = await _httpClient.GetAsync(endpoint);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            var reviews = await response.Content.ReadFromJsonAsync<PagedModel<ReviewsModel>>();

            return reviews ?? throw new UserFriendlyException("Trenutno nije moguće videti recenzije. Molimo pokušajte kasnije.", System.Net.HttpStatusCode.BadRequest);
        }

        public async Task<bool> CreateReviewAsync(CreateReviewModel model)
        {
            var user = await SecureStorage.Default.GetUser();
            var request = new HttpRequestMessage(HttpMethod.Post, "api/reviews")
            {
                Content = JsonContent.Create(model)
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AccessToken);

            var response = await _httpClient.SendAsync(request);
            ExceptionHelper.ThrowIfUnsuccessful(response);


            return response.StatusCode == HttpStatusCode.Created;
        }
    }
}
