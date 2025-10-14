using SneakersShop.Models;

namespace SneakersShop.Services.Interfaces
{
    public interface ICityService
    {
        Task<IEnumerable<CityModel>> GetCitiesAsync();
    }
}
