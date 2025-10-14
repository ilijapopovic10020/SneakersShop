using SneakersShop.Models;

namespace SneakersShop.Services.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandsModel>> GetBrandsAsync();
    }
}
