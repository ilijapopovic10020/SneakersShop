using SneakersShop.Models;
using SneakersShop.Models.Search;

namespace SneakersShop.Services.Interfaces
{
    public interface IReviewService
    {
        Task<PagedModel<ReviewsModel>> GetReviewsAsync(PagedSearchId search);
        Task<bool> CreateReviewAsync(CreateReviewModel model);
    }
}
