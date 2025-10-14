using SneakersShop.Models;

namespace SneakersShop.Services.Interfaces
{
    public interface IColorService
    {
        Task<IEnumerable<ColorsModel>> GetColorsAsync();
    }
}
