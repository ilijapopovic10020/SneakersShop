using SneakersShop.Models;

namespace SneakersShop.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> GetUserById(int id);
        Task<bool> Register(RegisterUserModel model);
        Task<bool> Update(UpdateUserModel model);
    }
}
