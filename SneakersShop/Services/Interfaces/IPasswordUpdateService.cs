using SneakersShop.Models;

namespace SneakersShop.Services.Interfaces
{
    public interface IPasswordUpdateService
    {
        Task<bool> UpdatePasswordAsync(PasswordUpdateModel model);
    }
}
