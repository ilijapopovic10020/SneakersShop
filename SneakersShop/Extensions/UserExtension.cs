using Newtonsoft.Json;
using SneakersShop.Exceptions;
using SneakersShop.Models;

namespace SneakersShop.Extensions
{
    public static class UserExtension
    {
        public static async Task<AuthInfoModel> GetUser(this ISecureStorage storage)
        {
            var userJson = await storage.GetAsync("user");

            if (string.IsNullOrEmpty(userJson))
            {
                throw new UserNotFoundException("Došlo je do greške. Molimo pokušaj te ponovo.");
            }

            return JsonConvert.DeserializeObject<AuthInfoModel>(userJson) 
                ?? throw new UserNotFoundException("Došlo je do greške. Molimo pokušaj te ponovo.");

        }
    }
}
