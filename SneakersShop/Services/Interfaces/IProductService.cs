using SneakersShop.Models;
using SneakersShop.Models.Search;

namespace SneakersShop.Services.Interfaces
{
    public interface IProductsService
    {
        Task<PagedModel<ProductsModel>> GetProductsAsync(ProductsSearch search);
        Task<ProductModel> GetProductByIdAsync(int id);
    }
}
