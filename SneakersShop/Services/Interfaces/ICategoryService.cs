using SneakersShop.Models;

namespace SneakersShop.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoriesModel>> GetCategoriesAsync();
    }
}
