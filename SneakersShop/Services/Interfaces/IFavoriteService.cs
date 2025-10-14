using SneakersShop.Models;
using SneakersShop.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersShop.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task<bool> AddToFavoritesAsync(int productId);
        Task<bool> RemoveFromFavoritesAsync(int productId);
        Task<PagedModel<ProductsModel>> GetFavoriteAsync(int? page, int? perPage, string? keyword);
    }
}
