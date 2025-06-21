using SneakersShop.Helpers;

namespace SneakersShop.Services;
public abstract class BaseService
{
    protected static readonly string BaseUrl = AppConstants.API_URL; // koristi AppConstants ako želiš

    protected readonly HttpClient _httpClient;

    protected BaseService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };
    }
}
