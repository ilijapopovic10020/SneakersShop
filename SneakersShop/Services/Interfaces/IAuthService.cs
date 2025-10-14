using SneakersShop.Models;

namespace SneakersShop.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthInfoModel> LoginAsync(AuthModel model);
    }
}
