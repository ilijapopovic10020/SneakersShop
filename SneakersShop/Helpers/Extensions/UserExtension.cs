using SneakersShop.MVVM.Models;
using Newtonsoft.Json;

namespace SneakersShop.Helpers.Extensions
{
    public static class UserExtension
    {
        public static async Task<LoginModel> GetUser(this ISecureStorage storage)
        {
            var userJson = await storage.GetAsync("user");

            if (string.IsNullOrEmpty(userJson))
            {
                throw new InvalidOperationException("No user information");
            }

            var user = JsonConvert.DeserializeObject<LoginModel>(userJson)
                    ?? throw new InvalidOperationException("Failed to read user information");

            return user;
        }

        public static async Task<UserModel> GetUserDetail(this ISecureStorage storage)
        {
            var userJson = await storage.GetAsync("user_detail");

            if (string.IsNullOrEmpty(userJson))
            {
                throw new InvalidOperationException("No user information");
            }

            var user = JsonConvert.DeserializeObject<UserModel>(userJson)
                    ?? throw new InvalidOperationException("Failed to read user information");

            return user;
        }
    }
}
